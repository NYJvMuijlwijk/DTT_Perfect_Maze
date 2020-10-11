using System;
using System.Collections.Generic;
using TMPro;
using UnityEngine;

namespace Assets.Scripts
{
    public class MazeBuilder : MonoBehaviour
    {
        [Header("Maze settings")]
        [SerializeField, Range(1,100)] private int _mazeWidth = 10;
        [SerializeField, Range(1,100)] private int _mazeHeight = 10;
        [SerializeField] private TMP_InputField[] _widthFields;
        [SerializeField] private TMP_InputField[] _heightFields;
        [Header("Maze Object Components")]
        [SerializeField] private GameObject _wallObject;
        [SerializeField] private GameObject _floorObject;

        private Maze _maze;
        private List<GameObject> _walls;
        private List<GameObject> _floors;

        public static event Action<Maze> MazeBuilt;

        void Start()
        {
            _walls = new List<GameObject>();
            _floors = new List<GameObject>();

            // Initialize all dimension input fields
            UpdateWidthFields();
            UpdateHeightFields();
        }

        /// <summary>
        /// Updates all registered height input fields
        /// </summary>
        private void UpdateHeightFields()
        {
            foreach (TMP_InputField field in _heightFields)
                field.text = _mazeHeight.ToString();
        }

        /// <summary>
        /// Updates all registered width input fields
        /// </summary>
        private void UpdateWidthFields()
        {
            foreach (TMP_InputField field in _widthFields)
                field.text = _mazeWidth.ToString();
        }

        /// <summary>
        /// Build maze using provided wall and floor objects using the selected maze algorithm(not yet implemented)
        /// </summary>
        public void BuildMaze()
        {
            // Generate the maze
            _maze = Eller.GenerateMaze(_mazeWidth, _mazeHeight);

            // remove all previously made walls and floors
            _walls.ForEach(Destroy);
            _floors.ForEach(Destroy);
            _walls.Clear();
            _floors.Clear();

            // construct the maze using the floor and wall objects
            for (int i = 0; i < _maze.Cells.Length; i++)
            {
                Cell c = _maze.Cells[i];
                // get the current row and column
                int row = i / _mazeWidth + 1;
                int column = i % _mazeWidth + 1;

                // get the current cell's position
                Vector3 cellPos = new Vector3(-column, 0, row);
                // instantiate floor object at cell position and add to _floors list
                _floors.Add(Instantiate(_floorObject, cellPos + _floorObject.transform.position, Quaternion.identity));//TODO: instantiate floor as one object under entire maze

                // instantiate a wall if one should be present. offset by half the cell size and rotate depending on which side it is.
                // walls get added to the _walls list
                if (c.Down) _walls.Add(Instantiate(_wallObject, cellPos - Vector3.back / 2 + _wallObject.transform.position, _wallObject.transform.rotation));
                if (c.Left) _walls.Add(Instantiate(_wallObject, cellPos - Vector3.left / 2 + _wallObject.transform.position, _wallObject.transform.rotation * Quaternion.Euler(0, 90, 0)));
                if (c.Up) _walls.Add(Instantiate(_wallObject, cellPos - Vector3.forward / 2 + _wallObject.transform.position, _wallObject.transform.rotation * Quaternion.Euler(0, 180, 0)));
                if (c.Right) _walls.Add(Instantiate(_wallObject, cellPos - Vector3.right / 2 + _wallObject.transform.position, _wallObject.transform.rotation * Quaternion.Euler(0, 270, 0)));
            }

            // Invoke MazeBuilt event
            OnMazeBuilt(_maze);
        }

        /// <summary>
        /// Set maze width to given parsed string value
        /// </summary>
        /// <param name="width">integral number string</param>
        public void SetMazeWidth(string width)
        {
            bool result = int.TryParse(width, out var _width);

            if (!result) return;
            _mazeWidth = Mathf.Clamp(_width, 1, 100);
            UpdateWidthFields();
        }

        /// <summary>
        /// Set maze width to given value
        /// </summary>
        /// <param name="width">width of maze</param>
        public void SetMazeWidth(int width)
        {
            _mazeWidth = Mathf.Clamp(width,1,100);
            UpdateWidthFields();
        }

        /// <summary>
        /// Set maze height to given parsed string value
        /// </summary>
        /// <param name="height">integral number string</param>
        public void SetMazeHeight(string height)
        {
            bool result = int.TryParse(height, out var _height);

            if (!result) return;
            _mazeHeight = Mathf.Clamp(_height, 1, 100);
            UpdateHeightFields();
        }

        /// <summary>
        /// Set maze height to given value
        /// </summary>
        /// <param name="height">height of maze</param>
        public void SetMazeHeight(int height)
        {
            _mazeHeight = Mathf.Clamp(height, 1, 100);
            UpdateHeightFields();
        } 


        /// <summary>
        /// Increment or decrement _mazeWidth by one
        /// </summary>
        /// <param name="increment">false for decrement</param>
        public void IncrementMazeWidth(bool increment)
        {
            if (increment && _mazeWidth + 1 <= 100) _mazeWidth++;
            else if (_mazeWidth - 1 > 0) _mazeWidth--;
            else return;

            UpdateWidthFields();
        }

        /// <summary>
        /// Increment or decrement _mazeHeight by one
        /// </summary>
        /// <param name="increment">false for decrement</param>
        public void IncrementMazeHeight(bool increment)
        {
            if (increment && _mazeHeight + 1 <= 100) _mazeHeight++;
            else if (_mazeHeight - 1 > 0) _mazeHeight--;
            else return;

            UpdateHeightFields();
        }

        /// <summary>
        /// Invokes the MazeBuilt event
        /// </summary>
        /// <param name="maze">The generated maze</param>
        private static void OnMazeBuilt(Maze maze)
        {
            MazeBuilt?.Invoke(maze);
        }
    }
}
