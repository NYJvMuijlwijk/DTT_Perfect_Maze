using UnityEngine;

namespace Assets.Scripts
{
    public class MazeBuilder : MonoBehaviour
    {
        [SerializeField] private int _mazeWidth = 10;
        [SerializeField] private int _mazeHeight = 10;
        [SerializeField] private float _cellSize = 1f;
        [SerializeField] private float _mazeOffset = .5f;
        [SerializeField] private Object _wallObject;
        [SerializeField] private Object _floorObject;

        private Maze _maze;

        public void BuildMaze()
        {
            // Generate the maze
            _maze = Eller.GenerateMaze(_mazeWidth, _mazeHeight);

            // construct the maze using the floor and wall objects
            for (int i = 0; i < _maze.Cells.Length; i++)
            {
                Cell c = _maze.Cells[i];
                // get the current row and column
                int row = i / _mazeWidth + 1;
                int column = i % _mazeWidth + 1;
                
                // get the current cell's position
                Vector3 cellPos = new Vector3(column * _cellSize - (_cellSize * _mazeOffset), 0 ,row * _cellSize - (_cellSize * _mazeOffset));
                // instantiate floor object at cell position
                Instantiate(_floorObject, cellPos,Quaternion.identity);//TODO: instantiate floor as one object under entire maze

                // instantiate a wall if one should be present. offset by half the cell size and rotate depending on which side it is
                if (c.Down) Instantiate(_wallObject, cellPos - Vector3.back * _cellSize / 2, Quaternion.identity);
                if (c.Left) Instantiate(_wallObject, cellPos - Vector3.left * _cellSize / 2, Quaternion.identity * Quaternion.Euler(0, 90, 0));
                if (c.Up) Instantiate(_wallObject, cellPos - Vector3.forward * _cellSize / 2, Quaternion.identity * Quaternion.Euler(0,180,0));
                if (c.Right) Instantiate(_wallObject, cellPos - Vector3.right * _cellSize / 2, Quaternion.identity * Quaternion.Euler(0, 270, 0));
            }

            Debug.Log(_maze);
        }

        void Update()
        {
            if (Input.GetKeyDown(KeyCode.Space)) BuildMaze();
        }
    }
}
