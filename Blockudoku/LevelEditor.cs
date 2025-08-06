using Blockudoku.Constants;
using Blockudoku.GameObjects;
using Blockudoku.GameObjects.FileIOUtilities;
using Blockudoku.GameObjects.GeneralUtilities;
using Blockudoku.GameObjects.Shapes;
using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;

namespace Blockudoku
{
    public partial class LevelEditor : Form
    {
        protected const int BLOCK_HOVER_OFFSET = CommonConstants.BLOCK_SIZE / 2;

        private IShapeFactory _shapeFactory;
        private Timer _timer = new Timer();
        private Point _originalBlockLocation;
        private BoardPlaceHolder _highlightedPlaceholder;
        private List<BoardPlaceHolder> _boardPlaceHolders = new List<BoardPlaceHolder>();
        private bool _selectionMode = false;
        private Block[,] _gameBoard;
        private BoardPlaceHolder _selectedPlaceholder;
        private int _scoreGoal = CustomModeConstants.DEFAULT_SCORE_GOAL;

        public LevelEditor(IShapeFactory shapeFactory)
        {
            InitializeComponent();
            _shapeFactory = shapeFactory;
            _originalBlockLocation = new Point(singleBlockShape1.Location.X, singleBlockShape1.Location.Y);
            _timer.Interval = 50;
            _timer.Tick += _timer_Tick;
            _timer.Start();
        }

        #region event handlers

        private void _timer_Tick(object sender, EventArgs e)
        {
            if (_selectionMode)
            {
                SetLocationOfBlockToCursorPosition();
            }
        }

        private void LevelEditor_Load(object sender, EventArgs e)
        {
            txtHealth.Text = "1";
            txtScoreGoal.Text = _scoreGoal.ToString();
            _gameBoard = new Block[CommonConstants.GAME_BOARD_SIZE, CommonConstants.GAME_BOARD_SIZE];
            foreach (var block in singleBlockShape1.Blocks)
            {
                this.Controls.Add(block);
                block.Location = singleBlockShape1.Location;
                block.BringToFront();
                block.Click += SingleBlockShape1_Click;
            }
            this.InitializeBoardPlaceHolders();
            this.InitializeDefaultMapName();
            this.InitializeDropDownListForMaps();
        }

        private void SingleBlockShape1_Click(object sender, EventArgs e)
        {
            _selectionMode = true;
            if (_selectedPlaceholder != null)
            {
                _selectedPlaceholder.BorderStyle = BorderStyle.None;
            }
            SetLocationOfBlockToCursorPosition();
        }

        private void TxtHealth_TextChanged(object sender, EventArgs e)
        {
            if (int.TryParse(txtHealth.Text, out int health))
            {
                singleBlockShape1.SetShapeHealth(health);
            }
            else
            {
                singleBlockShape1.SetShapeHealth(1);
            }
        }

        private void PbGameBoard_Click(object sender, EventArgs e)
        {
            if (_selectionMode)
                PlaceBlock();
        }

        private void LevelEditor_Click(object sender, EventArgs e)
        {
            if (_selectionMode)
                PlaceBlock();
        }

        private void LevelEditor_KeyUp(object sender, KeyEventArgs e)
        {
            switch (e.KeyData)
            {
                case Keys.Escape:
                    if (_selectionMode)
                    {
                        ClearBlockSelection();
                    }
                    else
                    {
                        foreach (var ph in _boardPlaceHolders)
                        {
                            ph.BorderStyle = BorderStyle.None;
                        }
                    }
                    break;
                case Keys.Delete:
                    DeleteSelectedPlaceholder();
                    break;
            }
        }

        private void Placeholder_Click(object sender, EventArgs e)
        {
            if (_selectionMode)
            {
                PlaceBlock();
            }
            else
            {
                if (_selectedPlaceholder != null)
                {
                    _selectedPlaceholder.BorderStyle = BorderStyle.None;
                }
                var placeholder = (BoardPlaceHolder)sender;
                if (placeholder.HasBlock)
                {
                    _selectedPlaceholder = placeholder;
                    placeholder.BorderStyle = BorderStyle.Fixed3D;
                }
            }
        }

        private void TxtScoreGoal_TextChanged(object sender, EventArgs e)
        {
            _scoreGoal = int.TryParse(txtScoreGoal.Text, out int val) ? val : CustomModeConstants.DEFAULT_SCORE_GOAL;
        }

        private void BtnSave_Click(object sender, EventArgs e)
        {
            if (!int.TryParse(txtScoreGoal.Text, out int goal))
            {
                MessageBox.Show("Please enter a valid integer for the score goal.", "Error", MessageBoxButtons.OK, MessageBoxIcon.Error);
                return;
            }
           
            if (LevelEditorFileIOUtility.SaveMap(txtMapName.Text, _gameBoard, goal))
            {
                MessageBox.Show($"Map { txtMapName.Text } save successfully.", "Success", MessageBoxButtons.OK, MessageBoxIcon.Information);
            }
            else
            {
                MessageBox.Show($"Error saving map { txtMapName.Text }.", "Error", MessageBoxButtons.OK, MessageBoxIcon.Error);
            }
        }

        private void BtnLoad_Click(object sender, EventArgs e)
        {
            if (cmbMapList.SelectedItem != null)
            {
                string mapName = cmbMapList.SelectedItem.ToString();
                if (LevelEditorFileIOUtility.LoadMap(mapName, out Block[,] gameBoard, out int scoreGoal))
                {
                    _gameBoard = gameBoard;
                    _selectedPlaceholder = null;
                    ClearBlockSelection();
                    _selectionMode = false;
                    _highlightedPlaceholder = null;
                    foreach (var ph in _boardPlaceHolders)
                    {
                        ph.BorderStyle = BorderStyle.None;
                        if (_gameBoard[ph.XIndex, ph.YIndex] != null)
                        {
                            ph.Block = _gameBoard[ph.XIndex, ph.YIndex];
                            ph.Block.XOffset = ph.XIndex * (CommonConstants.BLOCK_SIZE + 1);
                            ph.Block.YOffset = ph.YIndex * (CommonConstants.BLOCK_SIZE + 1);
                            ph.BringToFront();
                        }
                    }
                    txtMapName.Text = mapName;
                    txtScoreGoal.Text = scoreGoal.ToString();
                }
                else
                {
                    MessageBox.Show($"Error reading map { txtMapName.Text }: Check if the file exists and is formatted correctly.", "Error", MessageBoxButtons.OK, MessageBoxIcon.Error);
                }
            }
        }

        private void BtnCreateNew_Click(object sender, EventArgs e)
        {
            ClearBlockSelection();
            InitializeDefaultMapName();
            cmbMapList.SelectedItem = null;
            _selectedPlaceholder = null;
            _highlightedPlaceholder = null;
            _selectionMode = false;
            _gameBoard = new Block[CommonConstants.GAME_BOARD_SIZE, CommonConstants.GAME_BOARD_SIZE];
            foreach (var placeholder in _boardPlaceHolders)
            {
                placeholder.Block = null;
                placeholder.Image = null;
                placeholder.SendToBack();
            }
            txtScoreGoal.Text = CustomModeConstants.DEFAULT_SCORE_GOAL.ToString();
        }

        #endregion

        #region private methods

        private void SetLocationOfBlockToCursorPosition()
        {

            singleBlockShape1.Location = new Point(Cursor.Position.X, Cursor.Position.Y);
            foreach (var block in singleBlockShape1.Blocks)
            {
                block.Location = new Point(Cursor.Position.X + block.XOffset, Cursor.Position.Y + block.YOffset);
                block.BringToFront();
            }
            OnShapeMoved();
        }

        private void OnShapeMoved()
        {
            ProduceShadowEffectForOpenSpace();
        }

        private void ProduceShadowEffectForOpenSpace()
        {
            //run this code only for selection mode and only when the block is hovering over the board
            //otherwise, clear highlighted selection
            if (!_selectionMode || !IsBlockInGrid(singleBlockShape1.LeadBlock) && _highlightedPlaceholder != null && !_highlightedPlaceholder.HasBlock)
            {
                _highlightedPlaceholder.SendToBack();
                return;
            }
            //clear previous selection
            if (_highlightedPlaceholder != null && !_highlightedPlaceholder.HasBlock)
            {
                _highlightedPlaceholder.SendToBack();
            }

            //getClosestplaceHolder
            int x = singleBlockShape1.Location.X, y = singleBlockShape1.Location.Y;
            var minDistance = _boardPlaceHolders.Select(b => BlockUtilities.GetPythagorianDistance(x, b.Location.X, y, b.Location.Y)).Min();
            //highlight new placeholder
            _highlightedPlaceholder = _boardPlaceHolders.FirstOrDefault(b => BlockUtilities.GetPythagorianDistance(x, b.Location.X, y, b.Location.Y) == minDistance);
            if (_highlightedPlaceholder != null && !_highlightedPlaceholder.HasBlock)
            {
                _highlightedPlaceholder.BringToFront();
            }
            //bring selected shape to front
            singleBlockShape1.BringToFront();
            singleBlockShape1.LeadBlock?.BringToFront();
        }

        protected virtual bool IsBlockInGrid(Block block)
        {
            var pbTop = pbGameBoard.Top;
            var pbBottom = pbGameBoard.Bottom;
            var pbRight = pbGameBoard.Right;
            var pbLeft = pbGameBoard.Left;

            bool tooFarRight = block.Right >= (pbRight + BLOCK_HOVER_OFFSET);
            bool tooFarLeft = block.Left <= (pbLeft - BLOCK_HOVER_OFFSET);
            bool tooFarUp = block.Top <= (pbTop - BLOCK_HOVER_OFFSET);
            bool tooFarDown = block.Bottom >= (pbBottom + BLOCK_HOVER_OFFSET);

            bool isInBounds = !(tooFarLeft || tooFarRight || tooFarUp || tooFarDown);
            return isInBounds;
        }

        private void InitializeDropDownListForMaps()
        {
            var maps = LevelEditorFileIOUtility.GetMaps().ToArray();
            cmbMapList.Items.AddRange(maps);
        }

        private void InitializeDefaultMapName()
        {
            string mapName = LevelEditorFileIOUtility.DEFAULT_MAP_NAME;

            bool exists = LevelEditorFileIOUtility.MapExists(mapName);
            int incrementExtension = 1;

            while (exists)
            {
                mapName = LevelEditorFileIOUtility.DEFAULT_MAP_NAME + "_" + incrementExtension;
                exists = LevelEditorFileIOUtility.MapExists(mapName);
                incrementExtension++;
            }
            txtMapName.Text = mapName;
        }

        private void PlaceBlock()
        {
            if (_highlightedPlaceholder != null)
            {
                //assign blocks to board
                Block block = new Block();
                block.XOffset = _highlightedPlaceholder.XIndex * (CommonConstants.BLOCK_SIZE + 1);
                block.YOffset = _highlightedPlaceholder.YIndex * (CommonConstants.BLOCK_SIZE + 1);
                block.SetHealth(singleBlockShape1.LeadBlock?.Health ?? 1);
                _gameBoard[_highlightedPlaceholder.XIndex, _highlightedPlaceholder.YIndex] = block;
                _highlightedPlaceholder.Block = _gameBoard[_highlightedPlaceholder.XIndex, _highlightedPlaceholder.YIndex];
                _highlightedPlaceholder.BringToFront();
            }
        }
      
        private void InitializeBoardPlaceHolders()
        {
            int xOffset = pbGameBoard.Location.X + 2;
            int yOffset = pbGameBoard.Location.Y + 2;
            for (int i = 0; i < _gameBoard.GetLength(0); i++)
            {
                for (int j = 0; j < _gameBoard.GetLength(1); j++)
                {
                    BoardPlaceHolder boardPlaceHolder = new BoardPlaceHolder(i, j);
                    int x = (i * (CommonConstants.BLOCK_SIZE + 4)) + xOffset;
                    int y = (j * (CommonConstants.BLOCK_SIZE + 4)) + yOffset;
                    boardPlaceHolder.Size = new Size(CommonConstants.BLOCK_SIZE, CommonConstants.BLOCK_SIZE);
                    boardPlaceHolder.Location = new Point(x, y);
                    this.Controls.Add(boardPlaceHolder);
                    _boardPlaceHolders.Add(boardPlaceHolder);
                    boardPlaceHolder.Click += Placeholder_Click;
                }
            }
        }

        private void DeleteSelectedPlaceholder()
        {
            if (!_selectionMode && _selectedPlaceholder != null)
            {
                this.Controls.Remove(_selectedPlaceholder.Block);
                _selectedPlaceholder.Block = null;
                _gameBoard[_selectedPlaceholder.XIndex, _selectedPlaceholder.YIndex] = null;
                _selectedPlaceholder.Image = null;
                _selectedPlaceholder.SendToBack();
                _selectedPlaceholder.BorderStyle = BorderStyle.None;
                _selectedPlaceholder = null;
                _selectionMode = false;
            }
        }

        private void ClearBlockSelection()
        {
            singleBlockShape1.Location = new Point(_originalBlockLocation.X, _originalBlockLocation.Y);
            if (singleBlockShape1.LeadBlock != null)
                singleBlockShape1.LeadBlock.Location = new Point(_originalBlockLocation.X, _originalBlockLocation.Y);
            if (_highlightedPlaceholder != null)
                _highlightedPlaceholder.SendToBack();

            _selectionMode = false;
        }

        #endregion
    }
}
