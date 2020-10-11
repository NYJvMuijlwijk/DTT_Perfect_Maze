using UnityEngine;

namespace Assets.Scripts
{
    [RequireComponent(typeof(Camera))]
    public class MainCamera : MonoBehaviour
    {
        [SerializeField] private float _extraCameraSize = .1f;

        private Camera _camera;
        private Vector2 _screenResolution;
        private Maze _maze;

        // Start is called before the first frame update
        void Start()
        {
            _camera = GetComponent<Camera>();
            // Adjust camera position and size every time a new maze gets created
            MazeBuilder.MazeBuilt += AdjustCameraPosition;
            MazeBuilder.MazeBuilt += UpdateMaze;

            // Get current screen resolution for checking resolution changes
            _screenResolution = new Vector2(Screen.width,Screen.height);
        }

        void Update()
        {
            CheckScreenResChanges();
        }

        /// <summary>
        /// Check for screen resolution changes and adjust camera to fit maze accordingly
        /// </summary>
        private void CheckScreenResChanges()
        {
            Vector2 newRes = new Vector2(Screen.width, Screen.height);
            // Adjust camera position if screen dimensions changed
            if (_screenResolution == newRes || _maze == null) return;
            AdjustCameraPosition(_maze);
            _screenResolution = newRes;
        }

        /// <summary>
        /// Moves the main camera to show maze in its entirety
        /// </summary>
        /// <param name="maze">The maze to adjust camera to</param>
        private void AdjustCameraPosition(Maze maze)
        {
            // set position to middle of maze
            transform.position = new Vector3(-maze.Width / 2f, 10, maze.Height / 2f);
            // adjust orthographic size to always fit maze in width and height.
            _camera.orthographicSize = Mathf.Max(maze.Height / 2f,maze.Width * (1 / _camera.aspect) / 2f);
            // add extra size to fully show maze
            _camera.orthographicSize += maze.Height > maze.Width * (1 / _camera.aspect) ? _extraCameraSize : _extraCameraSize * (1 / _camera.aspect);
        }

        /// <summary>
        /// Updates the _maze variable
        /// </summary>
        /// <param name="maze">the new maze</param>
        private void UpdateMaze(Maze maze) => _maze = maze;

        void OnDestroy()
        {
            // remove any subscriptions made
            MazeBuilder.MazeBuilt -= AdjustCameraPosition;
            MazeBuilder.MazeBuilt -= UpdateMaze;
        }
    }
}
