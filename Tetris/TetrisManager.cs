using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Tetris
{
    // This class contains all the information about the game itself,
    // including the grid of stationary blocks, a list of upcoming blocks (whose head is the current block),
    // the score, the difficulty level, and the state of the game itself
    class TetrisManager
    {
        // public static System.Drawing.Pen pen = new System.Drawing.Pen(System.Drawing.Color.Black, 2);

        private LinkedList<Tetromino> minos;
        private GridBlock[,] grid;
        private bool game_end; // true if the game has ended, false otherwise
        private List<GridBlock> overflow;

        private int level;
        private int score; // number of lines * level
        private bool fast; // doubles speed

        private int scale;
        private int shift_x; // shift tetris table right
        private int shift_y; // shift tetris table down

        // Constructor
        public TetrisManager(int scale, int sx, int sy)
        {
            this.minos = new LinkedList<Tetromino>();
            this.grid = new GridBlock[10,20];
            this.game_end = false;
            this.overflow = new List<GridBlock>();

            this.level = 1;
            this.score = 0;
            this.fast = false;

            this.scale = scale;
            this.shift_x = sx;
            this.shift_y = sy;

            // initialize the list of upcoming blocks, and the current block
            for (int i = 0; i < 5; i++)
            {
                this.minos.AddLast(new Tetromino(scale));
            }
            this.minos.First().SetStart(scale * 4, -1 * scale); // set starting position

            // initialize all the grid blocks
            for (int x = 0; x < 10; x++)
            {
                for (int y = 0; y < 20; y++)
                {
                    grid[x, y] = new GridBlock(x * scale, y * scale);
                }
            }
        }

        // Call to paint the grid itself
        public void Paint(System.Windows.Forms.PaintEventArgs e)
        {
            System.Drawing.Graphics g = e.Graphics;
            System.Drawing.Pen p = new System.Drawing.Pen(System.Drawing.Color.Black, 1);  // outline
            System.Drawing.Brush b;

            // draw the filled-in grid
            for (int x = 0; x < 10; x++)
            {
                for (int y = 0; y < 20; y++)
                {
                    b = new System.Drawing.SolidBrush(grid[x,y].Color);
                    System.Drawing.Rectangle rect =
                        new System.Drawing.Rectangle(grid[x, y].X + this.shift_x, grid[x, y].Y + this.shift_y,
                        scale, scale);

                    g.FillRectangle(b, rect);
                    g.DrawRectangle(p, rect);
                }
            }

            if (game_end)
            {
                foreach (GridBlock gb in this.overflow)
                {
                    b = new System.Drawing.SolidBrush(gb.Color);
                    System.Drawing.Rectangle rect =
                        new System.Drawing.Rectangle(gb.X + this.shift_x, gb.Y + this.shift_y,
                        scale, scale);

                    g.FillRectangle(b, rect);
                    g.DrawRectangle(p, rect);
                }
            }
            else
            {
                Tetromino f = this.minos.First();
                b = new System.Drawing.SolidBrush(f.Color);
                foreach (System.Drawing.Rectangle r in f.GetRectangles(this.shift_x, this.shift_y))
                {
                    g.FillRectangle(b, r);
                    g.DrawRectangle(p, r);
                }
            }

            int x_shift = this.shift_x + 12 * this.scale;
            int y_shift = this.shift_y + 3 * this.scale;
            for (int i = 1; i < 4; i++)
            {
                Tetromino t = this.minos.ElementAt(i);
                b = new System.Drawing.SolidBrush(t.Color);
                foreach (System.Drawing.Rectangle r in t.GetRectangles(x_shift, y_shift))
                {
                    g.FillRectangle(b, r);
                    g.DrawRectangle(p, r);
                }
                y_shift += 5 * this.scale;
            }
        }

        // instant drop
        // Returns true if the game continues, false if the game ends
        public bool DeadDrop()
        {
            if (this.game_end)
            {
                return false;
            }
            Tetromino f = this.minos.First();
            bool no_touch = true; // the bottom does not touch any blocks
            while (no_touch)
            {
                // keep going until the block hits the floor
                f.AdvanceTime(this.scale);
                foreach (int i in f.GetColumns())
                {
                    int b = f.GetBottom(i);
                    int bott = b / this.scale;
                    no_touch &= bott != 20 && this.grid[i, bott].Color == System.Drawing.Color.FromArgb(128, 255, 255, 255);
                }
            }
            bool going = true;
            List<int> rows = new List<int>();
            foreach (System.Drawing.Rectangle r in f.GetRectangles(0, 0))
            {
                // fill in the grid
                int x = r.X / this.scale;
                int y = r.Y / this.scale;
                if (x >= 0 && x < 10 && y >= 0 && y < 20)
                {
                    this.grid[x, y].Color = f.Color;
                    rows.Add(y);
                }
                if (y < 0)
                {
                    going = false;
                    GridBlock tmp = new GridBlock(r.X, r.Y - (r.Y % this.scale));
                    tmp.Color = f.Color;
                    this.overflow.Add(tmp);
                }
            }
            this.ClearLines(rows);
            if (going)
            {
                this.minos.RemoveFirst();
                this.minos.AddLast(new Tetromino(scale));
                this.minos.First().SetStart(this.scale * 4, -3 * this.scale);

                // validate that the new piece doesn't cause a game over
                foreach (System.Drawing.Rectangle r in this.minos.First().GetRectangles(0, 0))
                {
                    int x = r.X / this.scale;
                    int y = r.Y / this.scale;
                    if (x >= 0 && x < 10 && y >= 0 && y < 20)
                    {
                        // if a piece already occupies where the new block ends up, the game is over
                        going &= (this.grid[x, y].Color == System.Drawing.Color.FromArgb(128, 255, 255, 255));
                    }
                }

                if (!going)
                {
                    f = this.minos.First();
                    foreach (System.Drawing.Rectangle r in this.minos.First().GetRectangles(0, 0))
                    {
                        int x = r.X / this.scale;
                        int y = r.Y / this.scale;
                        if (x >= 0 && x < 10 && y >= 0 && y < 20)
                        {
                            // if a piece already occupies where the new block ends up, the game is over
                            this.grid[x, y].Color = f.Color;
                        }
                        else
                        {
                            GridBlock tmp = new GridBlock(r.X, r.Y - (r.Y % this.scale));
                            tmp.Color = f.Color;
                            this.overflow.Add(tmp);
                        }
                    }
                }
            }

            this.game_end = !going;
            return going;
        }

        // Return true if the game is still going
        // Return false if the game ends
        public bool AdvanceTime()
        {
            if (game_end)
            {
                return false;
            }
            Tetromino f = this.minos.First();
            f.AdvanceTime(this.fast ? this.level * 2 : this.level);
            bool going = true;

            // check bottom of f
            bool touch = false;
            foreach (int i in f.GetColumns())
            {
                int b = f.GetBottom(i);
                int bott = b / this.scale;
                if (bott == 20 || this.grid[i, bott].Color != System.Drawing.Color.FromArgb(128, 255, 255, 255))
                {
                    touch = true;
                    break;
                }
            }
            if (touch)
            {
                // fill squares if the bottom of the piece now touches another piece/the bottom
                List<int> rows = new List<int>();
                foreach (System.Drawing.Rectangle r in f.GetRectangles(0,0))
                {
                    int x = r.X / this.scale;
                    int y = r.Y / this.scale;
                    if (x >= 0 && x < 10 && y >= 0 && y < 20)
                    {
                        this.grid[x, y].Color = f.Color;
                        rows.Add(y);
                    }
                    if (y < 0)
                    {
                        // game ends if y < 0
                        going = false;
                        GridBlock tmp = new GridBlock(r.X, r.Y - (r.Y % this.scale));
                        tmp.Color = f.Color;
                        this.overflow.Add(tmp);
                    }
                }
                this.ClearLines(rows);
                if (going)
                {
                    this.minos.RemoveFirst();
                    this.minos.AddLast(new Tetromino(scale));
                    this.minos.First().SetStart(this.scale * 4, -3 * this.scale);

                    // validate that the new piece doesn't cause a game over
                    foreach (System.Drawing.Rectangle r in this.minos.First().GetRectangles(0, 0))
                    {
                        int x = r.X / this.scale;
                        int y = r.Y / this.scale;
                        if (x >= 0 && x < 10 && y >= 0 && y < 20)
                        {
                            // if a piece already occupies where the new block ends up, the game is over
                            going &= (this.grid[x, y].Color == System.Drawing.Color.FromArgb(128, 255, 255, 255));
                        }
                    }

                    if (!going)
                    {
                        f = this.minos.First();
                        foreach (System.Drawing.Rectangle r in this.minos.First().GetRectangles(0, 0))
                        {
                            int x = r.X / this.scale;
                            int y = r.Y / this.scale;
                            if (x >= 0 && x < 10 && y >= 0 && y < 20)
                            {
                                // if a piece already occupies where the new block ends up, the game is over
                                this.grid[x, y].Color = f.Color;
                            }
                            else
                            {
                                GridBlock tmp = new GridBlock(r.X, r.Y - (r.Y % this.scale));
                                tmp.Color = f.Color;
                                this.overflow.Add(tmp);
                            }
                        }
                    }
                }
            }
            this.game_end = !going;
            return going;
        }

        private void ClearLines(List<int> rows)
        {
            // top rws first (lowest y)
            rows.Sort();

            // for each line, check if all blocks are not white
            // if so, increase score and move all blocks from above down
            foreach (int i in rows)
            {
                bool clean = true;
                System.Drawing.Color c = System.Drawing.Color.FromArgb(128, 255, 255, 255);
                for (int j = 0; j < 10; j++)
                {
                    GridBlock gb = this.grid[j, i];
                    clean &= gb.Color != c;
                }

                if (clean)
                {
                    // increase score
                    this.score += this.level;

                    for (int k = i; k > 0; k--)
                    {
                        // bring down each block
                        for (int x = 0; x < 10; x++)
                        {
                            this.grid[x, k].Color = this.grid[x, k - 1].Color;
                        }
                    }

                    for (int x = 0; x < 10; x++)
                    {
                        this.grid[x, 0].Color = c;
                    }
                }
            }
        }

        public void RotateCW()
        {
            if (this.game_end) return;
            Tetromino f = this.minos.First();
            int left = f.GetLeft();
            f.RotateCCW();
            bool revert = false;
            foreach (System.Drawing.Rectangle r in this.minos.First().GetRectangles(0,0))
            {
                int x = r.X / this.scale;
                int y = r.Y / this.scale;
                if (x >= 0 && x < 10 && y >= 0 && y < 20)
                    revert |= this.grid[x, y].Color != System.Drawing.Color.FromArgb(128, 255, 255, 255);
            }
            if (revert)
            {
                f.RotateCW();
                int displacement = left - f.GetLeft();
                f.SetStart(displacement, 0);
            }
        }

        public void RotateCCW()
        {
            if (this.game_end) return;
            Tetromino f = this.minos.First();
            int left = f.GetLeft();
            f.RotateCW();
            bool revert = false;
            foreach (System.Drawing.Rectangle r in this.minos.First().GetRectangles(0, 0))
            {
                int x = r.X / this.scale;
                int y = r.Y / this.scale;
                if (x >= 0 && x < 10 && y >= 0 && y < 20)
                    revert |= this.grid[x, y].Color != System.Drawing.Color.FromArgb(128, 255, 255, 255);
            }
            if (revert)
            {
                f.RotateCCW();
                int displacement = left - f.GetLeft();
                f.SetStart(displacement, 0);
            }
        }

        // Moves current block to the right
        // Will check boundaries, will do nothing if the game is over
        public void MoveRight()
        {
            if (game_end) return;
            Tetromino f = this.minos.First();
            if (f.GetRight() < 10 * this.scale)
            {
                // check if any blocks are to the right
                bool go = true;
                foreach (System.Drawing.Rectangle rect in f.GetRectangles(0,0))
                {
                    int x = (rect.X + this.scale) / this.scale;
                    int y = rect.Y / this.scale;
                    if (y < 0)
                    {
                        continue;
                    }
                    go &= (grid[x, y].Color == System.Drawing.Color.FromArgb(128, 255, 255, 255));
                    if (y < 20)
                    {
                        go &= (grid[x, y+1].Color == System.Drawing.Color.FromArgb(128, 255, 255, 255));
                    }
                }

                if (go)
                {
                    f.Shift(true);
                }
            }
        }

        // Moves current block to the left
        public void MoveLeft()
        {
            if (game_end) return;
            Tetromino f = this.minos.First();
            if (f.GetLeft() > 0)
            {
                // check if any blocks are to the right
                bool go = true;
                foreach (System.Drawing.Rectangle rect in f.GetRectangles(0, 0))
                {
                    int x = (rect.X - this.scale) / this.scale;
                    int y = rect.Y / this.scale;
                    if (y < 0)
                    {
                        continue;
                    }
                    go &= (grid[x, y].Color == System.Drawing.Color.FromArgb(128, 255, 255, 255));
                    if (y < 20)
                    {
                        go &= (grid[x, y + 1].Color == System.Drawing.Color.FromArgb(128, 255, 255, 255));
                    }
                }

                if (go)
                {
                    f.Shift(false);
                }
            }
        }

        // Change the grid's shift
        public int Shift_X
        {
            set { this.shift_x = value; }
        }
        public int Shift_y
        {
            set { this.shift_y = value; }
        }

        // Get score
        public int Score
        {
            get { return this.score; }
        }

        // Get and set level
        public int Level
        {
            get { return this.level; }
            set { this.level = value; }
        }

        // Change fast mode
        public bool Down
        {
            set { this.fast = value; }
        }

        // Check if the game is over
        public bool Ended
        {
            get { return this.game_end; }
        }
    }
}
