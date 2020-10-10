using System;
using System.Collections.Generic;
using UnityEngine;

namespace Assets.Scripts
{
    public class MazeBuilder : MonoBehaviour
    {
        [Header("Maze settings")]
        [SerializeField, Min(1)] private int _mazeWidth = 10;
        [SerializeField, Min(1)] private int _mazeHeight = 10;
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
        }

        /// <summary>
        /// Build maze using provided wall and floor objects using the selected maze algorithm
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
                Vector3 cellPos = new Vector3(-column, 0 ,row);
                // instantiate floor object at cell position and add to _floors list
                _floors.Add(Instantiate(_floorObject, cellPos + _floorObject.transform.position, Quaternion.identity));//TODO: instantiate floor as one object under entire maze

                // instantiate a wall if one should be present. offset by half the cell size and rotate depending on which side it is.
                // walls get added to the _walls list
                if (c.Down) _walls.Add(Instantiate(_wallObject, cellPos - Vector3.back / 2 + _wallObject.transform.position, _wallObject.transform.rotation ));
                if (c.Left) _walls.Add(Instantiate(_wallObject, cellPos - Vector3.left / 2 + _wallObject.transform.position, _wallObject.transform.rotation * Quaternion.Euler(0, 90, 0)));
                if (c.Up) _walls.Add(Instantiate(_wallObject, cellPos - Vector3.forward / 2 + _wallObject.transform.position, _wallObject.transform.rotation * Quaternion.Euler(0,180, 0)));
                if (c.Right) _walls.Add(Instantiate(_wallObject, cellPos - Vector3.right / 2 + _wallObject.transform.position, _wallObject.transform.rotation * Quaternion.Euler(0, 270, 0)));
            }

            // Invoke MazeBuilt event
            OnMazeBuilt(_maze);
        }

        void Update()
        {
            if (Input.GetKeyDown(KeyCode.Space)) BuildMaze();
        }

        /// <summary>
        /// Set maze width to given parsed string value
        /// </summary>
        /// <param name="width">integral number string</param>
        public void SetMazeWidth(string width)
        {
            bool result = int.TryParse(width,out var _width);

            if (result) _mazeWidth = _width;
        }

        /// <summary>
        /// Set maze width to given value
        /// </summary>
        /// <param name="width">width of maze</param>
        public void SetMazeWidth(int width) => _mazeWidth = width;

        /// <summary>
        /// Set maze height to given parsed string value
        /// </summary>
        /// <param name="height">integral number string</param>
        public void SetMazeHeight(string height)
        {
            bool result = int.TryParse(height,out var _height);

            if (result) _mazeHeight = _height;
        }

        /// <summary>
        /// Set maze height to given value
        /// </summary>
        /// <param name="height">height of maze</param>
        public void SetMazeHeight(int height) => _mazeHeight = height;

        /// <summary>
        /// Increment or decrement _mazeWidth by one
        /// </summary>
        /// <param name="increment">false for decrement</param>
        public void IncrementMazeWidth(bool increment)
        {
            if (increment) _mazeWidth++;
            else if (_mazeWidth - 1 > 0) _mazeWidth--;
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
