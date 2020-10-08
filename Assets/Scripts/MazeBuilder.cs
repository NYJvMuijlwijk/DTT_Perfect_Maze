using UnityEngine;

namespace Assets.Scripts
{
    public class MazeBuilder : MonoBehaviour
    {
        [SerializeField] private int _mazeWidth = 10;
        [SerializeField] private int _mazeHeight = 10;

        private Maze _maze;


        public void BuildMaze()
        {
            _maze = Eller.GenerateMaze(_mazeWidth, _mazeHeight);
            Debug.Log(_maze);
        }

        void Update()
        {
            if (Input.GetKeyDown(KeyCode.Space)) BuildMaze();
        }
    }
}
