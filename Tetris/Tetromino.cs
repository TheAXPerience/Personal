using System;
using System.Collections.Generic;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Tetris
{
    enum Blockcolors
    {
        Grey, // square
        Red, // lightning bolt (reverse)
        Green, // lightning bolt
        Blue, // line
        Yellow, // T
        Magenta, // L
        Cyan, // L (backwards)
        White // no block, useful in TetrisManager, otherwise used for number of block types
    }

    class Tetromino
    {
        private static Random rand = new Random();

        // List of coordinates that maintains the shape of the block
        private Rectangle[] blocks;

        // Color for the block
        private Color color;

        // Size
        private int scale;

        // constructor
        public Tetromino (int scale)
        {
            this.blocks = new Rectangle[4];
            this.scale = scale;
            InitMino((Blockcolors)(rand.Next() % (int)Blockcolors.White));
        }

        public Tetromino (int scale, Blockcolors b)
        {
            this.blocks = new Rectangle[4];
            this.scale = scale;
            InitMino(b);
        }

        private void InitMino(Blockcolors b)
        {
            switch (b)
            {
                case Blockcolors.Grey:
                    this.blocks[0] = new Rectangle(0, 0, scale, scale);
                    this.blocks[1] = new Rectangle(0, scale, scale, scale);
                    this.blocks[2] = new Rectangle(scale, 0, scale, scale);
                    this.blocks[3] = new Rectangle(scale, scale, scale, scale);
                    this.color = Color.FromArgb(128, 128, 128);
                    break;
                case Blockcolors.Red:
                    this.blocks[0] = new Rectangle(0, 0, scale, scale);
                    this.blocks[1] = new Rectangle(0, scale, scale, scale);
                    this.blocks[2] = new Rectangle(scale, scale, scale, scale);
                    this.blocks[3] = new Rectangle(scale, 2 * scale, scale, scale);
                    this.color = Color.FromArgb(255, 0, 0);
                    break;
                case Blockcolors.Green:
                    this.blocks[0] = new Rectangle(0, scale, scale, scale);
                    this.blocks[1] = new Rectangle(0, 2 * scale, scale, scale);
                    this.blocks[2] = new Rectangle(scale, 0, scale, scale);
                    this.blocks[3] = new Rectangle(scale, scale, scale, scale);
                    this.color = Color.FromArgb(0, 255, 0);
                    break;
                case Blockcolors.Blue:
                    this.blocks[0] = new Rectangle(0, 0, scale, scale);
                    this.blocks[1] = new Rectangle(0, scale, scale, scale);
                    this.blocks[2] = new Rectangle(0, 2 * scale, scale, scale);
                    this.blocks[3] = new Rectangle(0, 3 * scale, scale, scale);
                    this.color = Color.FromArgb(0, 0, 255);
                    break;
                case Blockcolors.Yellow:
                    this.blocks[0] = new Rectangle(0, 0, scale, scale);
                    this.blocks[1] = new Rectangle(0, scale, scale, scale);
                    this.blocks[2] = new Rectangle(0, 2 * scale, scale, scale);
                    this.blocks[3] = new Rectangle(scale, scale, scale, scale);
                    this.color = Color.FromArgb(255, 255, 0);
                    break;
                case Blockcolors.Magenta:
                    this.blocks[0] = new Rectangle(0, 0, scale, scale);
                    this.blocks[1] = new Rectangle(0, scale, scale, scale);
                    this.blocks[2] = new Rectangle(0, 2 * scale, scale, scale);
                    this.blocks[3] = new Rectangle(scale, 0, scale, scale);
                    this.color = Color.FromArgb(255, 0, 255);
                    break;
                case Blockcolors.Cyan:
                    this.blocks[0] = new Rectangle(scale, 0, scale, scale);
                    this.blocks[1] = new Rectangle(scale, scale, scale, scale);
                    this.blocks[2] = new Rectangle(scale, 2 * scale, scale, scale);
                    this.blocks[3] = new Rectangle(0, 0, scale, scale);
                    this.color = Color.FromArgb(0, 255, 255);
                    break;
                default:
                    throw new ArgumentOutOfRangeException();
                    break;
            }
        }

        // Sets the starting position of the block
        public void SetStart(int x, int y)
        {
            for (int i = 0; i < 4; i++)
            {
                this.blocks[i].X += x;
                this.blocks[i].Y += y;
            }
        }

        // Shifts the blocks either left or right
        public void Shift(bool right)
        {
            bool undo = false;
            for (int i = 0; i < 4; i++)
            {
                if (right)
                {
                    blocks[i].X += this.scale;
                    if (blocks[i].X > 9 * this.scale)
                    {
                        undo = true;
                    }
                }
                else
                {
                    blocks[i].X -= this.scale;
                    if (blocks[i].X < 0)
                    {
                        undo = true;
                    }
                }
            }
            if (undo)
            {
                for (int i = 0; i < 4; i++)
                {
                    if (right)
                    {
                        blocks[i].X -= this.scale;
                    }
                    else
                    {
                        blocks[i].X += this.scale;
                    }
                }
            }
        }

        // Moves the blocks down
        // Giving a negative number will shift the blocks up
        // Note: rate is given bc the level of the game may change. This will be stored elsewhere
        public void AdvanceTime(int rate)
        {
            for (int i = 0; i < 4; i++)
            {
                blocks[i].Y += rate;
            }
        }

        // Returns the y-position of the top of the block
        // May be useful in determining if the game is over
        // Might be another way to do it though, but it'll be nice to add this
        public int GetTop()
        {
            int ret = this.blocks[0].Y;
            for (int i = 1; i < 4; i++)
            {
                ret = Math.Min(ret, this.blocks[i].Y);
            }
            return ret;
        }

        // Returns the y position of the bottom of the block
        public int GetBottom()
        {
            int ret = this.blocks[0].Y + this.scale;
            for (int i = 1; i < 4; i++)
            {
                ret = Math.Max(ret, this.blocks[i].Y + this.scale);
            }
            return ret;
        }

        // Returns the y-position of the bottom of the block
        // in a specific column (0 to 9)
        public int GetBottom(int col)
        {
            int ret = 0;
            for (int i = 0; i < 4; i++)
            {
                if (this.blocks[i].X == this.scale * col)
                {
                    ret = Math.Max(ret, this.blocks[i].Y + this.scale);
                }
            }
            return ret;
        }

        public int GetLeft()
        {
            int ret = this.blocks[0].X;
            for (int i = 1; i < 4; i++)
            {
                ret = Math.Min(ret, this.blocks[i].X);
            }
            return ret;
        }

        public int GetRight()
        {
            int ret = this.blocks[0].X + this.scale;
            for (int i = 1; i < 4; i++)
            {
                ret = Math.Max(ret, this.blocks[i].X + this.scale);
            }
            return ret;
        }

        // Returns the color of the blocks
        public Color Color
        {
            get { return color; }
        }

        public List<int> GetColumns()
        {
            List<int> ret = new List<int>();
            foreach (Rectangle r in this.blocks)
            {
                ret.Add(r.X / this.scale);
            }
            return ret;
        }

        // Returns the blocks' size and positions
        public List<Rectangle> GetRectangles(int shift_x, int shift_y)
        {
            List<Rectangle> rects = new List<Rectangle>();
            foreach (Rectangle r in this.blocks) {
                Rectangle rect = new Rectangle(r.X + shift_x, r.Y + shift_y, scale, scale);
                rects.Add(rect);
            }
            return rects;
        }

        public void RotateCW()
        {
            int[] xy = this.GetCenter();
            Rectangle[] new_rects = new Rectangle[4];
            bool shift_left = false;
            bool shift_right = false;
            for (int i = 0; i < 4; i++)
            {
                Rectangle r = this.blocks[i];

                // translate to origin
                int curr_x = r.X - xy[0];
                int curr_y = r.Y - xy[1];

                // rotate 270 degrees (cos = 0, sin = -1)
                int new_x = curr_y + xy[0];
                int new_y = curr_x * -1 + xy[1];
                if (new_x % this.scale != 0)
                {
                    new_x -= (new_x % this.scale < 0) ? (this.scale + new_x % this.scale) : new_x % this.scale;
                }
                shift_left |= new_x > 9 * this.scale;
                shift_right |= new_x < 0;

                // save new rectangle
                new_rects[i] = new Rectangle(new_x, new_y, this.scale, this.scale);
            }
            while (shift_left)
            {
                bool sl = false;
                for (int i = 0; i < 4; i++)
                {
                    Rectangle tmp = new_rects[i];
                    new_rects[i] = new Rectangle(tmp.X - this.scale, tmp.Y, this.scale, this.scale);
                    sl |= (tmp.X - this.scale) > 9 * this.scale;
                }
                shift_left = sl;
            }
            while (shift_right)
            {
                bool sr = false;
                for (int i = 0; i < 4; i++)
                {
                    Rectangle tmp = new_rects[i];
                    new_rects[i] = new Rectangle(tmp.X + this.scale, tmp.Y, this.scale, this.scale);
                    sr |= (tmp.X + this.scale) < 0;
                }
                shift_right = sr;
            }
            this.blocks = new_rects;
        }

        public void RotateCCW()
        {
            int[] xy = this.GetCenter();
            Rectangle[] new_rects = new Rectangle[4];
            bool shift_left = false;
            bool shift_right = false;
            for (int i = 0; i < 4; i++)
            {
                Rectangle r = this.blocks[i];

                // translate to origin
                int curr_x = r.X - xy[0];
                int curr_y = r.Y - xy[1];

                // rotate 90 degrees (cos = 0, sin = 1)
                int new_x = curr_y * -1 + xy[0];
                int new_y = curr_x + xy[1];
                if (new_x % this.scale != 0)
                {
                    int s = (new_x % this.scale) < 0 ? this.scale + (new_x % this.scale) : (new_x % this.scale);
                    new_x += this.scale - s;
                }
                shift_right |= new_x < 0;
                shift_left |= new_x > 9 * this.scale;

                // save new rectangle
                new_rects[i] = new Rectangle(new_x, new_y, this.scale, this.scale);
            }
            while (shift_left)
            {
                bool sl = false;
                for (int i = 0; i < 4; i++)
                {
                    Rectangle tmp = new_rects[i];
                    new_rects[i] = new Rectangle(tmp.X - this.scale, tmp.Y, this.scale, this.scale);
                    sl |= (tmp.X - this.scale) > 9 * this.scale;
                }
                shift_left = sl;
            }
            while (shift_right)
            {
                bool sr = false;
                for (int i = 0; i < 4; i++)
                {
                    Rectangle tmp = new_rects[i];
                    new_rects[i] = new Rectangle(tmp.X + this.scale, tmp.Y, this.scale, this.scale);
                    sr |= (tmp.X + this.scale) < 0;
                }
                shift_right = sr;
            }
            this.blocks = new_rects;
        }

        // Returns the center of the block
        // Possibly necessary for rotation
        private int[] GetCenter()
        {
            int maxX = this.blocks[0].X;
            int minX = this.blocks[0].X;
            int maxY = this.blocks[0].Y;
            int minY = this.blocks[0].Y;

            for (int i = 1; i < 4; i++)
            {
                maxX = Math.Max(maxX, this.blocks[i].X);
                minX = Math.Min(minX, this.blocks[i].X);
                maxY = Math.Max(maxY, this.blocks[i].Y);
                minY = Math.Min(minY, this.blocks[i].Y);
            }

            int[] ret = new int[2];
            ret[0] = (maxX + minX) / 2;
            ret[1] = (maxY + minY) / 2;
            return ret;
        }
    }
}
