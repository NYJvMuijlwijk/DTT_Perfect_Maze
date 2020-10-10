using Unity.Collections;
using UnityEngine;
using UnityEngine.UI;

namespace Assets.Scripts
{
    public class SidebarMenu : MonoBehaviour
    {
        [SerializeField, Range(0,1)] private float _menuWidthPercent = .3f;
        [SerializeField] private bool _visible;
        [SerializeField] private RectTransform _toggleButton;
        [SerializeField] private float _toggleSize = 60f;
        [SerializeField] private GameObject _sidebarMenu;
        [SerializeField] private Canvas _canvas;

        private RectTransform _transform;
        private Vector2 _canvasDimensions;
        private Image _toggleImage;

        void Awake()
        {
            _transform = _sidebarMenu.GetComponent<RectTransform>();
        }

        void Start()
        {
            _canvasDimensions = _canvas.GetComponent<CanvasScaler>().referenceResolution;
            _toggleImage = _toggleButton.GetComponent<Image>();

            // show/hide sidebar depending on visibility
            if (_visible)
                ShowSidebar();
            else 
                HideSidebar();

            MainMenu.PlayGame += EnableSidebar;
            MainMenu.MainMenuOpened += DisableSidebar;
        }

        private void EnableSidebar()
        {
            foreach (Transform child in GetComponentsInChildren<Transform>(true))
            {
                child.gameObject.SetActive(true);
            }

            if(_visible) HideSidebar();
        }

        private void DisableSidebar()
        {
            foreach (Transform child in transform)
            {
                child.gameObject.SetActive(false);
            }
        }

        /// <summary>
        /// (un)hides the sidebar menu
        /// </summary>
        public void ToggleMenu()
        {
            // depending on visibility, move sidebar and toggle to the side or to its specified width
            if (_visible)
                HideSidebar();
            else
                ShowSidebar();
        }

        /// <summary>
        /// Sets the sidebars position and dimensions according to the menu width percentage
        /// </summary>
        private void ShowSidebar()
        {
            // enable the menu
            _sidebarMenu.SetActive(true);
            // move sidebar back to its specified width
            _transform.offsetMin = Vector2.right * (_canvasDimensions.x - _canvasDimensions.x * _menuWidthPercent);
            _transform.offsetMax = Vector2.zero;
            // move toggle button to the edge of the menu
            _toggleButton.offsetMin =
                new Vector2(_canvasDimensions.x / 2 - _canvasDimensions.x * _menuWidthPercent - _toggleSize / 2, -_toggleSize / 2);
            _toggleButton.offsetMax =
                new Vector2(_canvasDimensions.x / 2 - _canvasDimensions.x * _menuWidthPercent + _toggleSize / 2, _toggleSize / 2);

            // set icon rotation
            _toggleImage.transform.rotation = Quaternion.identity;

            _visible = true;
        }

        /// <summary>
        /// Hides the sidebar menu
        /// </summary>
        private void HideSidebar()
        {
            // hide sidebar off screen
            _transform.offsetMin = Vector2.right * _canvasDimensions.x;
            _transform.offsetMax = Vector2.right * _canvasDimensions.x * _menuWidthPercent;
            // move toggle button to the side of the screen
            _toggleButton.offsetMin = new Vector2(_canvasDimensions.x / 2 - _toggleSize, -_toggleSize / 2);
            _toggleButton.offsetMax = new Vector2(_canvasDimensions.x / 2, _toggleSize / 2);
            // disable the menu
            _sidebarMenu.SetActive(false);

            // set icon rotation
            _toggleImage.transform.rotation = Quaternion.identity * Quaternion.Euler(0,0,180f);

            _visible = false;
        }

        void OnDestroy()
        {
            MainMenu.PlayGame -= EnableSidebar;
            MainMenu.MainMenuOpened -= DisableSidebar;
        }
    }
}
