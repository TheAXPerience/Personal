using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Tetris
{
    class GridBlock
    {
        private System.Drawing.Color color;
        private int x;
        private int y;

        public GridBlock(int x, int y)
        {
            this.color = System.Drawing.Color.FromArgb(128, 255, 255, 255);
            this.x = x;
            this.y = y;
        }

        public System.Drawing.Color Color
        {
            get { return this.color; }
            set { this.color = value; }
        }

        public int X
        {
            get { return this.x; }
            set { this.x = value; }
        }

        public int Y
        {
            get { return this.y; }
            set { this.y = value; }
        }
    }
}
