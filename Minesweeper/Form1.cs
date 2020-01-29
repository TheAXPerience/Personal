using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;

namespace Minesweeper
{
    public partial class Form1 : Form
    {
        public const int NUM_X = 20;
        public const int NUM_Y = 14;
        public const int SHIFT_X = 170;
        public const int SHIFT_Y = 10;
        public const int SIZE = 30;
        public const int NUM_MINES = 40;

        Square[,] grid;
        Label clock;
        Label unmarked;
        Label unfound;
        Button reset;
        Button marker;
        Timer time;
        int seconds;
        int num_unmarked;
        int num_unfound;
        bool mark_mode;
        bool game_end;

        public Form1()
        {
            InitializeComponent();
            this.Text = "Minesweeper";
            this.Name = "MineWindow";

            grid = new Square[NUM_X, NUM_Y];
            clock = new Label();
            unmarked = new Label();
            unfound = new Label();
            reset = new Button();
            marker = new Button();
            time = new Timer();
        }

        private void Form1_Load(object sender, EventArgs e)
        {
            this.Height = 500;
            this.Width = 850;

            for (int x = 0; x < NUM_X; x++)
            {
                for (int y = 0; y < NUM_Y; y++)
                {
                    grid[x, y] = new Square(x * NUM_Y + y, SHIFT_X + x * SIZE, SHIFT_Y + y * SIZE, SIZE, SIZE);
                    this.Controls.Add(grid[x, y].GetButton());
                    grid[x, y].GetButton().Click += new EventHandler(button_click);
                }
            }

            for (int x = 0; x < NUM_X; x++)
            {
                for (int y = 0; y < NUM_Y; y++)
                {
                    if (x - 1 >= 0 && y - 1 >= 0) grid[x, y].AddNeighbor(grid[x - 1, y - 1]);
                    if (x - 1 >= 0) grid[x, y].AddNeighbor(grid[x - 1, y]);
                    if (x - 1 >= 0 && y + 1 < NUM_Y) grid[x, y].AddNeighbor(grid[x - 1, y + 1]);
                    if (y - 1 >= 0) grid[x, y].AddNeighbor(grid[x, y - 1]);
                    if (y + 1 < NUM_Y) grid[x, y].AddNeighbor(grid[x, y + 1]);
                    if (x + 1 < NUM_X && y - 1 >= 0) grid[x, y].AddNeighbor(grid[x + 1, y - 1]);
                    if (x + 1 < NUM_X) grid[x, y].AddNeighbor(grid[x + 1, y]);
                    if (x + 1 < NUM_X && y + 1 < NUM_Y) grid[x, y].AddNeighbor(grid[x + 1, y + 1]);
                }
            }

            this.BackColor = System.Drawing.Color.LightGray;

            clock.Top = 20;
            clock.Left = 30;
            clock.Width = 110;
            clock.Height = 40;
            clock.Font = new Font("Arial", 16, FontStyle.Regular);
            clock.ForeColor = Color.White;
            clock.BackColor = Color.Black;
            clock.TextAlign = ContentAlignment.MiddleCenter;
            clock.Visible = true;
            clock.Text = "00:00:00";
            this.Controls.Add(clock);

            time.Interval = 1000;
            time.Tick += new EventHandler(timer_tick);

            Label tmp1 = new Label();
            tmp1.Top = 80;
            tmp1.Left = 30;
            tmp1.Width = 110;
            tmp1.Height = 20;
            tmp1.Font = new Font("Arial", 12, FontStyle.Regular);
            tmp1.TextAlign = ContentAlignment.MiddleCenter;
            tmp1.Visible = true;
            tmp1.Text = "Mines left";
            this.Controls.Add(tmp1);

            unmarked.Top = 105;
            unmarked.Left = 30;
            unmarked.Width = 110;
            unmarked.Height = 45;
            unmarked.BackColor = System.Drawing.Color.Yellow;
            unmarked.Visible = true;
            unmarked.TextAlign = ContentAlignment.MiddleCenter;
            unmarked.Font = new Font("Arial", 20, FontStyle.Regular);
            this.Controls.Add(unmarked);

            Label tmp2 = new Label();
            tmp2.Top = 170;
            tmp2.Left = 30;
            tmp2.Width = 110;
            tmp2.Height = 20;
            tmp2.Font = new Font("Arial", 12, FontStyle.Regular);
            tmp2.TextAlign = ContentAlignment.MiddleCenter;
            tmp2.Visible = true;
            tmp2.Text = "Spaces left";
            this.Controls.Add(tmp2);

            unfound.Top = 195;
            unfound.Left = 30;
            unfound.Width = 110;
            unfound.Height = 45;
            unfound.BackColor = System.Drawing.Color.GreenYellow;
            unfound.Visible = true;
            unfound.TextAlign = ContentAlignment.MiddleCenter;
            unfound.Font = new Font("Arial", 20, FontStyle.Regular);
            this.Controls.Add(unfound);

            marker.Top = 270;
            marker.Left = 30;
            marker.Width = 110;
            marker.Height = 55;
            marker.BackColor = System.Drawing.Color.OrangeRed;
            marker.Visible = true;
            marker.Text = "Mark";
            marker.TextAlign = ContentAlignment.MiddleCenter;
            marker.Font = new Font("Arial", 18, FontStyle.Regular);
            mark_mode = false;
            marker.Click += new EventHandler(marker_click);
            this.Controls.Add(marker);

            reset.Top = 350;
            reset.Left = 30;
            reset.Width = 110;
            reset.Height = 55;
            reset.BackColor = System.Drawing.Color.DimGray;
            reset.Visible = true;
            reset.Text = "Reset";
            reset.TextAlign = ContentAlignment.MiddleCenter;
            reset.Font = new Font("Arial", 20, FontStyle.Regular);
            reset.Click += new EventHandler(reset_click);
            this.Controls.Add(reset);

            StartGame();
        }

        private void StartGame()
        {
            reset.BackColor = System.Drawing.Color.DimGray;
            seconds = 0;
            this.BackColor = System.Drawing.Color.LightGray;
            game_end = false;
            AssignMines(NUM_MINES);
            num_unmarked = NUM_MINES;
            num_unfound = NUM_X * NUM_Y - NUM_MINES;
            time.Start();
            UpdateText();
        }

        private void EndGame()
        {
            game_end = true;
            time.Stop();
            foreach (Square s in grid)
            {
                if (s.IsMine())
                {
                    s.GetButton().Text = "M";
                }
            }
            reset.BackColor = Color.Lavender;
        }

        private void UpdateText()
        {
            unmarked.Text = num_unmarked.ToString();
            unfound.Text = num_unfound.ToString();
            if (seconds > 359999)
            {
                clock.Text = "99:59:59";
            }
            else
            {
                int sec = seconds % 60;
                int min = seconds / 60;
                int hr = min / 60;
                min = min % 60;
                clock.Text = (hr < 10 ? "0" + hr : hr.ToString()) + ":"
                    + (min < 10 ? "0" + min : min.ToString()) + ":"
                    + (sec < 10 ? "0" + sec : sec.ToString());
            }

            if (num_unfound == 0)
            {
                this.BackColor = Color.AntiqueWhite;
                EndGame();
            }
        }

        private void AssignMines(int num)
        {
            Random rand = new Random();
            for (int i = 0; i < num; i++)
            {
                int x = rand.Next(NUM_X);
                int y = rand.Next(NUM_Y);
                if (!grid[x, y].IsMine())
                {
                    grid[x, y].MakeMine();
                } else
                {
                    i--;
                }
            }
        }

        private void button_click(object sender, EventArgs e)
        {
            Button button = sender as Button;
            int num = Convert.ToInt32(button.Name);
            if (!grid[num / NUM_Y, num % NUM_Y].IsMarked()
                && !game_end && !mark_mode)
            {
                EasyClick(num);
            }
            else if (!grid[num / NUM_Y, num % NUM_Y].IsClicked()
                && !game_end
                && mark_mode)
            {
                if (grid[num / NUM_Y, num % NUM_Y].IsMarked())
                {
                    grid[num / NUM_Y, num % NUM_Y].Unmark();
                    num_unmarked++;
                }
                else
                {
                    grid[num / NUM_Y, num % NUM_Y].Mark();
                    num_unmarked--;
                }
                UpdateText();
            }
        }

        private void reset_click(object sender, EventArgs e)
        {
            foreach (Square s in grid)
            {
                s.Reset();
            }
            StartGame();
        }

        private void marker_click(object sender, EventArgs e)
        {
            mark_mode = !mark_mode;
            if (mark_mode)
            {
                marker.Text = "Unmark";
                marker.BackColor = System.Drawing.Color.SkyBlue;
            }
            else
            {
                marker.Text = "Mark";
                marker.BackColor = System.Drawing.Color.OrangeRed;
            }
        }

        private void timer_tick(object sender, EventArgs e)
        {
            seconds++;
            UpdateText();
        }

        private void EasyClick(int num)
        {
            int count = grid[num / NUM_Y, num % NUM_Y].Click();
            int curr = 0;
            foreach (Square s in grid)
            {
                if (!s.IsClicked())
                {
                    curr++;
                }
            }
            num_unfound = curr - NUM_MINES;
            if (count == -1)
            {
                EndGame();
                this.BackColor = System.Drawing.Color.DarkGray;
            }

            UpdateText();
        }
    }
}
