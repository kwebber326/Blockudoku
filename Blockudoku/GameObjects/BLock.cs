using Blockudoku.Constants;
using System;
using System.Collections.Generic;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;

namespace Blockudoku.GameObjects
{
    public class Block : PictureBox
    {
        protected bool _canBePlaced;
        protected int _health;
        public event EventHandler Destroyed;
        private Label _lblHealth = new Label();
        public Block(bool canBePlaced = true, int health = 1)
        {
            this.CanBePlaced = canBePlaced;
            _health = health;
            this.SizeMode = PictureBoxSizeMode.AutoSize;
            DrawCurrentImage();
            this.BringToFront();

            _lblHealth.Text = this.Health.ToString();
            this.Controls.Add(_lblHealth);
            _lblHealth.BringToFront();

            _lblHealth.Visible = this.Health > 1;

        }

        public Label HealthLabel
        {
            get
            {
                return _lblHealth;
            }
        }

        public bool Destroy()
        {
            if (--_health <= 0)
            {
                OnDestroyed();
                return true;
            }
            else
            {
                _lblHealth.Text = this.Health.ToString();
                _lblHealth.Visible = this.Health > 1;
            }
            return false;
        }

        public int Health
        {
            get
            {
                return _health;
            }
        }

        public void SetHealth(int health)
        {
            if (health > 0)
            {
                _health = health;
                _lblHealth.Text = _health.ToString();
                _lblHealth.Visible = _health > 1;
                _lblHealth.BringToFront();
            }
        }
        public virtual bool CanBePlaced
        {
            get
            {
                return _canBePlaced;
            }
            set
            {
                _canBePlaced = value;
                this.Image = _canBePlaced ? Properties.Resources.blockudoku_block : Properties.Resources.blockudoku_block_faded;
            }
        }

        public int XOffset { get; set; }

        public int YOffset { get; set; }

        public int XIndex { get; set; }

        public int YIndex { get; set; }

        protected virtual void OnDestroyed()
        {
            this.Destroyed?.Invoke(this, EventArgs.Empty);
        }

        protected void DrawCurrentImage()
        {
            var bitmap = new Bitmap(CommonConstants.BLOCK_SIZE, CommonConstants.BLOCK_SIZE);
            using (Graphics g = Graphics.FromImage(bitmap))
            {
                g.DrawImage(this.Image, new Rectangle(new Point(0, 0), new Size(CommonConstants.BLOCK_SIZE, CommonConstants.BLOCK_SIZE)));
            }
            this.Image = bitmap;
        }
    }
}
