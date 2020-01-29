using System;
using System.Collections.Generic;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;

namespace Minesweeper
{
    class Square
    {
        private Button button;
        private List<Square> neighbors;
        private int num_neighbor_mines;
        private bool is_marked;
        private bool is_clicked;
        private bool is_mine;

        public Square(int n, int x, int y, int w, int h)
        {
            button = new Button();
            button.Top = y;
            button.Left = x;
            button.Width = w;
            button.Height = h;

            neighbors = new List<Square>();
            is_marked = false;
            is_clicked = false;
            is_mine = false;
            button.BackColor = Color.White;
            button.Visible = true;
            button.Name = n.ToString();
            this.num_neighbor_mines = 0;
        }

        public Button GetButton()
        {
            return button;
        }

        public int GetName()
        {
            return Convert.ToInt32(button.Name);
        }

        public void AddNeighbor(Square n)
        {
            neighbors.Add(n);
        }

        public void MakeMine()
        {
            is_mine = true;
        }

        public bool IsClicked()
        {
            return is_clicked;
        }

        public bool IsMarked()
        {
            return is_marked;
        }

        public bool IsMine()
        {
            return is_mine;
        }

        public void Mark()
        {
            is_marked = true;
            button.BackColor = Color.Yellow;
        }

        public void Unmark()
        {
            is_marked = false;
            button.BackColor = Color.White;
        }

        public int Click()
        {
            if (is_marked)
            {
                return 9;
            }
            else if (is_clicked)
            {
                int count = 0;
                foreach (Square s in this.GetNeighbors())
                {
                    if (s.is_marked)
                    {
                        count++;
                    }
                }
                int min_val = 9;
                if (this.num_neighbor_mines == count)
                {
                    foreach (Square s in this.GetNeighbors())
                    {
                        if (!s.is_marked && !s.is_clicked)
                        {
                            min_val = Math.Min(min_val, s.Click());
                        }
                    }
                }
                return min_val;
            }
            else if (is_mine)
            {
                button.BackColor = Color.Red;
                is_clicked = true;
                return -1;
            }
            else
            {
                int count = ClickAndCount();
                if (count == 0)
                {
                    List<Square> queue = new List<Square>();
                    queue.Add(this);
                    while (queue.Count > 0)
                    {
                        Square s = queue[0];
                        queue.RemoveAt(0);

                        int c = s.ClickAndCount();
                        if (c == 0)
                        {
                            foreach (Square n in s.GetNeighbors())
                            {
                                if (!n.is_clicked)
                                {
                                    queue.Add(n);
                                }
                            }
                        }
                    }
                }
                return count;
            }
        }

        private int ClickAndCount()
        {
            button.BackColor = Color.Green;
            int count = 0;
            foreach (Square s in neighbors)
            {
                count = (s.is_mine) ? count + 1 : count;
            }
            if (count > 0)
            {
                button.Text = count.ToString();
                button.TextAlign = ContentAlignment.MiddleCenter;
            }
            is_clicked = true;
            this.num_neighbor_mines = count;
            return count;
        }

        public List<Square> GetNeighbors()
        {
            List<Square> ret = new List<Square>();
            foreach (Square s in neighbors)
            {
                ret.Add(s);
            }
            return ret;
        }

        public void Reset()
        {
            is_marked = false;
            is_clicked = false;
            is_mine = false;
            button.Text = "";
            button.BackColor = Color.White;
            button.Visible = true;
        }
    }
}
