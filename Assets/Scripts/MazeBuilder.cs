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
                _floors.Add(Instantiate(_floorObject, cellPos,Quaternion.identity));//TODO: instantiate floor as one object under entire maze

                // instantiate a wall if one should be present. offset by half the cell size and rotate depending on which side it is.
                // walls get added to the _walls list
                if (c.Down) _walls.Add(Instantiate(_wallObject, cellPos - Vector3.back / 2 + _wallObject.transform.position, _wallObject.transform.rotation ));
                if (c.Left) _walls.Add(Instantiate(_wallObject, cellPos - Vector3.left / 2 + _wallObject.transform.position, _wallObject.transform.rotation * Quaternion.Euler(0, 90, 0)));
                if (c.Up) _walls.Add(Instantiate(_wallObject, cellPos - Vector3.forward / 2 + _wallObject.transform.position, _wallObject.transform.rotation * Quaternion.Euler(0,180, 0)));
                if (c.Right) _walls.Add(Instantiate(_wallObject, cellPos - Vector3.right / 2 + _wallObject.transform.position, _wallObject.transform.rotation * Quaternion.Euler(0, 270, 0)));
            }

            OnMazeBuilt(_maze);
        }

        void Update()
        {
            if (Input.GetKeyDown(KeyCode.Space)) BuildMaze();
        }

        private static void OnMazeBuilt(Maze maze)
        {
            MazeBuilt?.Invoke(maze);
        }
    }
}
