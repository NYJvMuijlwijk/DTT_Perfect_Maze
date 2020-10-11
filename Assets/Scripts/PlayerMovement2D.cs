using System;
using UnityEngine;

namespace Assets.Scripts
{
    [RequireComponent(typeof(MeshRenderer))]
    public class PlayerMovement2D : MonoBehaviour
    {
        private Maze _maze;
        private MeshRenderer _meshRenderer;

        void Awake()
        {
            _meshRenderer = GetComponent<MeshRenderer>();
        }

        // Start is called before the first frame update
        void Start()
        {
            MazeBuilder.MazeBuilt += ResetPosition;
            MainMenu.PlayGame += EnablePlayer;
            MainMenu.MainMenuOpened += DisablePlayer;

            _meshRenderer.enabled = false;
        }

        /// <summary>
        /// Hides the player
        /// </summary>
        private void DisablePlayer()
        {
            _meshRenderer.enabled = false;
        }

        /// <summary>
        /// Shows the player
        /// </summary>
        private void EnablePlayer()
        {
            _meshRenderer.enabled = true;
        }

        // Update is called once per frame
        void Update()
        {
            if (_maze == null || !_meshRenderer.enabled) return;

            Cell currentCell = GetCurrentCell();

            // handle player input
            //TODO: make it actually work
            if (Input.GetKeyDown(KeyCode.LeftArrow) && currentCell.Left)
                transform.position = transform.position + Vector3.left;
            else if (Input.GetKeyDown(KeyCode.RightArrow) && currentCell.Right)
                transform.position = transform.position + Vector3.right;
            else if (Input.GetKeyDown(KeyCode.DownArrow) && currentCell.Down)
                transform.position = transform.position + Vector3.back;
            else if (Input.GetKeyDown(KeyCode.UpArrow) && currentCell.Up)
                transform.position = transform.position + Vector3.forward;

        }

        /// <summary>
        /// Get the current cell the player is in
        /// </summary>
        /// <returns>Cell the player is in</returns>
        private Cell GetCurrentCell()
        {
            // Get the current column and row in the maze.
            int column = (int)-transform.position.x;
            int row = (int)transform.position.z;

            //Debug.Log($"Column: {column}, Row: {row}");

            return _maze.Cells[row * _maze.Width + column];
        }

        /// <summary>
        /// Reset player to maze starting position
        /// </summary>
        /// <param name="maze">The generated maze</param>
        private void ResetPosition(Maze maze)
        {
            _maze = maze;
            transform.position = new Vector3(-0.5f,0,0.5f);
        }

        void OnDestroy()
        {
            MazeBuilder.MazeBuilt -= ResetPosition;
            MainMenu.PlayGame -= EnablePlayer;
            MainMenu.MainMenuOpened -= DisablePlayer;
        }
    }
}
