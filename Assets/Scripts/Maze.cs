using UnityEngine;

namespace Assets.Scripts
{
    public class Maze
    {
        public readonly int Width;
        public readonly int Height;

        public readonly Cell[] Cells;

        public Maze(int w, int h)
        {
            Width = w;
            Height = h;

            Cells = new Cell[Width * Height];

            for (int i = 0; i < Cells.Length; i++)
            {
                Cells[i] = new Cell();
            }
        }

        public Cell GetCell(int r, int c)
        {
            return Cells[r * Width + c];
        }
    }
}