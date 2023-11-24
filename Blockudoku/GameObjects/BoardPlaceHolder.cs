using Blockudoku.Constants;
using Blockudoku.GameObjects.Shapes;
using System;
using System.Collections.Generic;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;

namespace Blockudoku.GameObjects
{
    public class BoardPlaceHolder : PictureBox
    {
        private Block _block;
        private int _xIndex;
        private int _yIndex;
        private Timer _destroyAminationTimer = new Timer();

        private const int ANIMATION_COUNT = 3;
        private int _currentAnimation = 0;

        public BoardPlaceHolder(int xIndex, int yIndex)
        {
            this.BackColor = Color.Gray;
            _xIndex = xIndex;
            _yIndex = yIndex;
            _destroyAminationTimer.Interval = 50;
            _destroyAminationTimer.Tick += _destroyAminationTimer_Tick;
        }

        private void _destroyAminationTimer_Tick(object sender, EventArgs e)
        {
            RunDestructionAnimation();
        }

        private void RunDestructionAnimation()
        {
            if (++_currentAnimation >= ANIMATION_COUNT)
            {
                this.SendToBack();
                this.Image = null;
               
                this.Size = new Size(CommonConstants.BLOCK_SIZE, CommonConstants.BLOCK_SIZE);
                this.SizeMode = PictureBoxSizeMode.AutoSize;
                _destroyAminationTimer.Stop();
                _currentAnimation = 0;
            }
            else if (this.Image != null)
            {
                this.SizeMode = PictureBoxSizeMode.StretchImage;
                int newSize = CommonConstants.BLOCK_SIZE / (_currentAnimation + 1);
                this.Size = new Size(newSize, newSize);
            }
        }

        public bool HasBlock
        {
            get
            {
                return this.Block != null;
            }
        }
        public int XIndex
        {
            get
            {
                return _xIndex;
            }
        }

        public int YIndex
        {
            get
            {
                return _yIndex;
            }
        }

        public Block Block
        {
            get
            {
                return _block;
            }
            set
            {
                if (_block != null)
                {
                    this.Controls.Remove(_block.HealthLabel);
                    _block.Destroyed -= _block_Destroyed;
                    AssignBlockValue(value);
                }
                else if (value != null)
                {
                    AssignBlockValue(value);
                }
                else
                {
                    _block = null;
                }
            }
        }

        private void AssignBlockValue(Block value)
        {
            _block = value;
            if (_block != null)
            {
                this.Image = _block.Image;
                _block.Destroyed += _block_Destroyed;
                this.Controls.Add(_block.HealthLabel);
                _block.HealthLabel.BringToFront();
            }
        }

        private void _block_Destroyed(object sender, EventArgs e)
        {
            this.Block = null;
            RunDestructionAnimation();
            _destroyAminationTimer.Start();
        }
    }
}
