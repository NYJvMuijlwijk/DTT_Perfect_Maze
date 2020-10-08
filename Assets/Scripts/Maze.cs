namespace Assets.Scripts
{
    public class Maze
    {
        public readonly int width;
        public readonly int height;

        public readonly Cell[] cells;

        public Maze(int w, int h)
        {
            width = w;
            height = h;

            cells = new Cell[width * height];

            for (int i = 0; i < cells.Length; i++)
            {
                cells[i] = new Cell();
            }
        }

        public Cell GetCell(int r, int c)
        {
            return cells[r * width + c];
        }
    }
}