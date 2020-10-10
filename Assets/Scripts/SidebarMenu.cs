using Unity.Collections;
using UnityEngine;
using UnityEngine.UI;

namespace Assets.Scripts
{
    public class SidebarMenu : MonoBehaviour
    {
        [SerializeField, Range(0,1)] private float _menuWidthPercent = .3f;
        [SerializeField] private bool _visible = true;
        [SerializeField] private RectTransform _toggleButton;
        [SerializeField] private GameObject _sidebarMenu;

        private RectTransform _transform;
        private float _canvasWidth;

        void Awake()
        {
            _transform = _sidebarMenu.GetComponent<RectTransform>();
        }

        void Start()
        {
            _canvasWidth = GetComponent<CanvasScaler>().referenceResolution.x;

            if (_visible)
            {
                _transform.offsetMin = Vector2.right * (_canvasWidth - _canvasWidth * _menuWidthPercent);
                _transform.offsetMax = Vector2.zero;
            }
        }

        public void ToggleMenu()
        {
            float buttonWidth = (_toggleButton.offsetMax - _toggleButton.offsetMin).x;
            if (_visible)
            {
                _transform.offsetMin = Vector2.right * _canvasWidth;
                _transform.offsetMax = Vector2.right * _canvasWidth * _menuWidthPercent;
                //_toggleButton.transform.position = new Vector2(_canvasWidth / 2 - buttonWidth,_canvasWidth/2);
                _sidebarMenu.SetActive(false);
            }
            else
            {
                _sidebarMenu.SetActive(true);
                _transform.offsetMin = Vector2.right * (_canvasWidth - _canvasWidth * _menuWidthPercent);
                _transform.offsetMax = Vector2.zero;
                //_toggleButton.transform.position = new Vector2(_canvasWidth / 2 - buttonWidth/2 - _canvasWidth * _menuWidthPercent, _canvasWidth / 2);

            }

            _toggleButton.Rotate(Vector3.forward, 180f);
            _visible = !_visible;
        }
    }
}
