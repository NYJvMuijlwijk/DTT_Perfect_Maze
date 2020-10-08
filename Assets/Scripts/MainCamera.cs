using UnityEngine;

namespace Assets.Scripts
{
    [RequireComponent(typeof(Camera))]
    public class MainCamera : MonoBehaviour
    {
        [SerializeField] private float _extraCameraSize = .1f;
        private Camera _camera;

        // Start is called before the first frame update
        void Start()
        {
            _camera = GetComponent<Camera>();
            // Adjust camera position and size every time a new maze gets created
            MazeBuilder.MazeBuilt += AdjustCameraPosition;
        }

        private void AdjustCameraPosition(Maze maze)
        {
            // set position to middle of maze
            transform.position = new Vector3(-maze.Width / 2f, 10, maze.Height / 2f);
            // adjust orthographic size to always fit maze in width and height.
            _camera.orthographicSize = Mathf.Max(maze.Height / 2f,maze.Width / 16f * 9 / 2f) + _extraCameraSize;
        }

        void OnDestroy()
        {
            MazeBuilder.MazeBuilt -= AdjustCameraPosition;
        }
    }
}
