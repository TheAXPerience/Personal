using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;

namespace Tetris
{
    public partial class TetrisForm : Form
    {
        private TetrisManager game;
        private Timer time;
        private long ticks;
        private Random rand;

        private Button Start;
        private System.Windows.Forms.Label scoreboard;
        private System.Windows.Forms.Label showscore;
        private System.Windows.Forms.Label levelboard;
        private System.Windows.Forms.Label showlevel;

        public TetrisForm()
        {
            game = null;
            time = new Timer();
            ticks = 0;
            rand = new Random();

            InitializeComponent();
        }

        private void TetrisForm_Load(object sender, EventArgs e)
        {
            this.Name = "Tetris";
            this.Text = "Tetris";
            this.ResizeRedraw = false;
            this.Height = 600;
            this.Width = 800;
            this.BackColor = Color.FromArgb(220, 220, 220);
            this.DoubleBuffered = true;
            // Image bg = Image.FromFile("beh.jpg");
            // this.BackgroundImage = (Image)(new Bitmap(bg, new Size(800, 600)));

            this.KeyDown += new KeyEventHandler(key_down);
            this.KeyPress += new KeyPressEventHandler(key_press);
            this.KeyUp += new KeyEventHandler(key_up);

            int align_x = this.Width / 2 - 240;
            Font f = new Font("Comic Sans MS", 13);

            time.Interval = 20;
            time.Tick += new EventHandler(timer_tick);

            Label next_blocks = new Label();
            next_blocks.BackColor = System.Drawing.Color.FromArgb(255, 32, 32, 32);
            next_blocks.ForeColor = System.Drawing.Color.White;
            next_blocks.TextAlign = System.Drawing.ContentAlignment.MiddleCenter;
            next_blocks.Top = 100;
            next_blocks.Left = 520;
            next_blocks.Height = 40;
            next_blocks.Width = 80;
            next_blocks.Text = "NEXT";
            next_blocks.Font = f;
            next_blocks.Visible = true;
            this.Controls.Add(next_blocks);

            scoreboard = new System.Windows.Forms.Label();
            scoreboard.BackColor = System.Drawing.Color.FromArgb(255, 32, 32, 32);
            scoreboard.ForeColor = System.Drawing.Color.White;
            scoreboard.TextAlign = System.Drawing.ContentAlignment.MiddleCenter;
            scoreboard.Top = 410; //280;
            scoreboard.Left = align_x;
            scoreboard.Height = 40;
            scoreboard.Width = 100;
            scoreboard.Text = "SCORE";
            scoreboard.Font = f;
            scoreboard.Visible = true;
            this.Controls.Add(scoreboard);

            showscore = new System.Windows.Forms.Label();
            showscore.BackColor = System.Drawing.Color.FromArgb(255, 225, 225, 225);
            showscore.ForeColor = System.Drawing.Color.Black;
            showscore.TextAlign = System.Drawing.ContentAlignment.MiddleCenter;
            showscore.Top = 450; //320;
            showscore.Left = align_x;
            showscore.Height = 50;
            showscore.Width = 100;
            showscore.Text = "";
            showscore.Font = f;
            showscore.Visible = true;
            this.Controls.Add(showscore);

            levelboard = new System.Windows.Forms.Label();
            levelboard.BackColor = System.Drawing.Color.FromArgb(255, 32, 32, 32);
            levelboard.ForeColor = System.Drawing.Color.White;
            levelboard.TextAlign = System.Drawing.ContentAlignment.MiddleCenter;
            levelboard.Top = 280; // 410;
            levelboard.Left = align_x;
            levelboard.Height = 40;
            levelboard.Width = 100;
            levelboard.Text = "LEVEL";
            levelboard.Font = f;
            levelboard.Visible = true;
            this.Controls.Add(levelboard);

            showlevel = new System.Windows.Forms.Label();
            showlevel.BackColor = System.Drawing.Color.FromArgb(255, 225, 225, 225);
            showlevel.ForeColor = System.Drawing.Color.Black;
            showlevel.TextAlign = System.Drawing.ContentAlignment.MiddleCenter;
            showlevel.Top = 320; // 450;
            showlevel.Left = align_x;
            showlevel.Height = 50;
            showlevel.Width = 100;
            showlevel.Text = "";
            showlevel.Visible = true;
            showlevel.Font = f;
            this.Controls.Add(showlevel);

            Start = new Button();
            Start.BackColor = Color.FromArgb(200, 100, 0); // some random color
            Start.Text = "Start Game";
            Start.Top = 250;
            Start.Left = 300;
            Start.Height = 100;
            Start.Width = 200;
            Start.Font = f;
            Start.Visible = true;
            Start.Click += new EventHandler(StartGame);
            this.Controls.Add(Start);
        }

        private void StartGame(object sender, EventArgs e)
        {
            Start.Visible = false;
            Start.Enabled = false;
            this.BackColor = Color.FromArgb(rand.Next(32, 220), rand.Next(32, 220), rand.Next(32, 220));
            game = new TetrisManager(20, 300, 100);
            this.Invalidate();
            time.Start();
        }

        protected override void OnPaint(PaintEventArgs e)
        {
            base.OnPaint(e);
            if (game != null)
            {
                game.Paint(e);
                showscore.Text = "" + game.Score;
                showlevel.Text = "" + game.Level;
            }
        }

        private void key_up(object sender, KeyEventArgs e)
        {
            if (e.KeyCode == Keys.S || e.KeyCode == Keys.Down)
            {
                // whatever?
                game.Down = false;
            }
        }

        private void key_down(object sender, KeyEventArgs e)
        {
            switch (e.KeyCode)
            {
                case Keys.A:
                case Keys.Left:
                    game.MoveLeft();
                    this.Invalidate();
                    break;
                case Keys.D:
                case Keys.Right:
                    game.MoveRight();
                    this.Invalidate();
                    break;
                case Keys.W:
                case Keys.Up:
                    time.Stop();
                    bool go = game.DeadDrop();
                    this.Invalidate();
                    if (go) time.Start();
                    else
                    {
                        Start.Visible = true;
                        Start.Enabled = true;
                        this.BackColor = Color.FromArgb(32, 32, 32);
                    }
                    break;
                case Keys.S:
                case Keys.Down:
                    game.Down = true;
                    break;
                case Keys.K:
                case Keys.Q:
                    game.RotateCCW();
                    this.Invalidate();
                    break;
                case Keys.L:
                case Keys.E:
                    game.RotateCW();
                    this.Invalidate();
                    break;
                default:
                    break;
            }
        }

        private void key_press(object sender, KeyPressEventArgs e)
        {
            this.Invalidate();
        }

        private void timer_tick(object sender, EventArgs e)
        {
            time.Stop();
            ticks++;
            bool go = game.AdvanceTime();
            this.Invalidate();
            game.Level = Math.Min(10, ((int)ticks / 3000) + 1);
            if (go) time.Start();
            else
            {
                Start.Visible = true;
                Start.Enabled = true;
                this.BackColor = Color.FromArgb(32, 32, 32);
            }
        }
    }
}
