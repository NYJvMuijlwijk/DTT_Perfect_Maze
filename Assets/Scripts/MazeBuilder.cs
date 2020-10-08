using System.Collections.Generic;
using UnityEngine;

namespace Assets.Scripts
{
    public class MazeBuilder : MonoBehaviour
    {
        [SerializeField] private int _mazeWidth = 10;
        [SerializeField] private int _mazeHeight = 10;
        [SerializeField] private float _cellSize = 1f;
        [SerializeField] private float _mazeOffset = .5f;
        [SerializeField] private float _rotationOffset = 0f;
        [SerializeField] private Object _wallObject;
        [SerializeField] private Object _floorObject;

        private Maze _maze;
        private List<Object> _walls;
        private List<Object> _floors;

        void Start()
        {
            _walls = new List<Object>();
            _floors = new List<Object>();
        }

        public void BuildMaze()
        {
            // Generate the maze
            _maze = Eller.GenerateMaze(_mazeWidth, _mazeHeight);

            // remove all previously made walls and floors
            _walls.ForEach(Destroy);
            _floors.ForEach(Destroy);

            // construct the maze using the floor and wall objects
            for (int i = 0; i < _maze.Cells.Length; i++)
            {
                Cell c = _maze.Cells[i];
                // get the current row and column
                int row = i / _mazeWidth + 1;
                int column = i % _mazeWidth + 1;
                
                // get the current cell's position
                Vector3 cellPos = new Vector3(column * _cellSize - _cellSize * _mazeOffset, 0 ,row * _cellSize - _cellSize * _mazeOffset);
                // instantiate floor object at cell position and add to _floors list
                _floors.Add(Instantiate(_floorObject, cellPos,Quaternion.identity));//TODO: instantiate floor as one object under entire maze

                // instantiate a wall if one should be present. offset by half the cell size and rotate depending on which side it is.
                // walls get added to the _walls list
                if (c.Down) _walls.Add(Instantiate(_wallObject, cellPos - Vector3.back * _cellSize / 2, Quaternion.identity * Quaternion.Euler(0, _rotationOffset, 0)));
                if (c.Left) _walls.Add(Instantiate(_wallObject, cellPos - Vector3.left * _cellSize / 2, Quaternion.identity * Quaternion.Euler(0, 90 + _rotationOffset, 0)));
                if (c.Up) _walls.Add(Instantiate(_wallObject, cellPos - Vector3.forward * _cellSize / 2, Quaternion.identity * Quaternion.Euler(0,180 + _rotationOffset, 0)));
                if (c.Right) _walls.Add(Instantiate(_wallObject, cellPos - Vector3.right * _cellSize / 2, Quaternion.identity * Quaternion.Euler(0, 270 + _rotationOffset, 0)));
            }
        }

        void Update()
        {
            if (Input.GetKeyDown(KeyCode.Space)) BuildMaze();
        }
    }
}
