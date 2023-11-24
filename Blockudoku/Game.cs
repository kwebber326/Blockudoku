using Blockudoku.Constants;
using Blockudoku.EventArgsObjects;
using Blockudoku.GameObjects;
using Blockudoku.GameObjects.FileIOUtilities;
using Blockudoku.GameObjects.FileIOUtilities.DataContracts;
using Blockudoku.GameObjects.GameLogicUtilities;
using Blockudoku.GameObjects.GeneralUtilities;
using Blockudoku.GameObjects.Shapes;
using Blockudoku.UserControls;
using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Diagnostics;
using System.Drawing;
using System.IO;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;

namespace Blockudoku
{
    public partial class Game : Form
    {
        protected const int INVENTORY_PANEL_PADDING_HORIZONTAL = 48;
        protected const int INVENTORY_PANEL_PADDING_VERTICAL = 16;
        protected const int SHAPE_SPACING_OFFSET = 64;
        protected const int BLOCK_HOVER_OFFSET = CommonConstants.BLOCK_SIZE / 2;


        private GameMode _gameMode;
        protected Block[,] _gameBoard;

        private readonly IGameLogicFactory _gameLogicFactory;
        private readonly IShapeFactory _shapeFactory;
        private readonly IGameLogic _gameLogic;

        protected Timer _cursorUpdateTimer;
        protected List<Block> _cursorBlocks = new List<Block>();
        protected List<BoardPlaceHolder> _boardPlaceHolders = new List<BoardPlaceHolder>();
        protected List<BoardPlaceHolder> _highlightedPlaceholders = new List<BoardPlaceHolder>();
        protected Shape _selectedShape;

        protected List<Shape> _overrideInventory = null;

        private Queue<string> _messageQueue = new Queue<string>();
        private Timer _messageQueueTimer = new Timer();
        private Timer _matchScoreAnimationTimer = new Timer();
        private int _matchScoreAnimationSequenceNum = 0;
        private const int MATCH_SCORE_ANIMATION_SEQUENCES = 5;

        public event EventHandler<ShapeChangedEventArgs> SelectedShapeChanged;
        public event EventHandler<ShapeMovedEventArgs> ShapeMove;

        private int _currentStreak = 0;
        private int _totalScore = 0;

        private int _scoreTilNextPass = CommonConstants.POINTS_PER_PASS;
        private int _passes;

        private int _scoreTileNextRotate = CommonConstants.POINTS_PER_ROTATE;
        private int _rotations;

        private bool _debugMode = false;
        private bool _isCustomMap;

        private int _scoreGoal;
        private readonly string _customMapName;
        private bool _isGameOver = false;
        public Game(GameMode gameMode, IGameLogicFactory gameLogicFactory, IShapeFactory shapeFactory)
        {
            _gameMode = gameMode;
            _gameLogicFactory = gameLogicFactory ?? throw new ArgumentNullException(nameof(gameLogicFactory) + " is null");
            _shapeFactory = shapeFactory ?? throw new ArgumentNullException(nameof(shapeFactory) + " is null");
            _gameLogic = _gameLogicFactory.CreateGameLogic(_gameMode);
            _gameBoard = new Block[CommonConstants.GAME_BOARD_SIZE, CommonConstants.GAME_BOARD_SIZE];
            _isCustomMap = false;
            InitializeComponent();
        }
        //new custom map
        public Game(GameMode gameMode, IGameLogicFactory gameLogicFactory, IShapeFactory shapeFactory, Block[,] gameBoard, int scoreGoal, string customMapName)
        {
            _gameMode = gameMode;
            _gameLogicFactory = gameLogicFactory ?? throw new ArgumentNullException(nameof(gameLogicFactory) + " is null");
            _shapeFactory = shapeFactory ?? throw new ArgumentNullException(nameof(shapeFactory) + " is null");
            _gameLogic = _gameLogicFactory.CreateGameLogic(_gameMode);
            _gameBoard = gameBoard ?? throw new ArgumentNullException(nameof(gameBoard) + " is null");
            _isCustomMap = true;
            _scoreGoal = scoreGoal;
            _customMapName = customMapName;

            InitializeComponent();
        }
        //standard mode load game
        public Game(GameMode gameMode, IGameLogicFactory gameLogicFactory, IShapeFactory shapeFactory, int totalScore, Block[,] gameBoard, List<Shape> inventory, int currentStreak)
        {
            _gameMode = gameMode;
            _gameLogicFactory = gameLogicFactory ?? throw new ArgumentNullException(nameof(gameLogicFactory) + " is null");
            _shapeFactory = shapeFactory ?? throw new ArgumentNullException(nameof(shapeFactory) + " is null");
            _gameLogic = _gameLogicFactory.CreateGameLogic(_gameMode);
            _gameBoard = gameBoard ?? throw new ArgumentNullException(nameof(gameBoard) + " is null");
            _totalScore = totalScore;
            _overrideInventory = inventory;
            _currentStreak = currentStreak;

            InitializeComponent();
        }
        //indi mode load game
        public Game(GameMode gameMode, IGameLogicFactory gameLogicFactory, IShapeFactory shapeFactory, int totalScore, int passes, int rotations, Block[,] gameBoard, List<Shape> inventory, int currentStreak)
        {
            _gameMode = gameMode;
            _gameLogicFactory = gameLogicFactory ?? throw new ArgumentNullException(nameof(gameLogicFactory) + " is null");
            _shapeFactory = shapeFactory ?? throw new ArgumentNullException(nameof(shapeFactory) + " is null");
            _gameLogic = _gameLogicFactory.CreateGameLogic(_gameMode);
            _gameBoard = gameBoard ?? throw new ArgumentNullException(nameof(gameBoard) + " is null");
            _totalScore = totalScore;
            _passes = passes;
            _scoreTilNextPass = (_totalScore - (_totalScore % CommonConstants.POINTS_PER_PASS)) + CommonConstants.POINTS_PER_PASS;

            _rotations = rotations;
            _scoreTileNextRotate = (_totalScore - (_totalScore % CommonConstants.POINTS_PER_ROTATE)) + CommonConstants.POINTS_PER_ROTATE;

            _overrideInventory = inventory;
            _currentStreak = currentStreak;

            InitializeComponent();
        }

        //loaded custom map
        public Game(GameMode gameMode, IGameLogicFactory gameLogicFactory, IShapeFactory shapeFactory, Block[,] gameBoard, int scoreGoal, string customMapName, int totalScore, int passes, int rotations, List<Shape> inventory, int currentStreak)
        {
            _gameMode = gameMode;
            _gameLogicFactory = gameLogicFactory ?? throw new ArgumentNullException(nameof(gameLogicFactory) + " is null");
            _shapeFactory = shapeFactory ?? throw new ArgumentNullException(nameof(shapeFactory) + " is null");
            _gameLogic = _gameLogicFactory.CreateGameLogic(_gameMode);
            _gameBoard = gameBoard ?? throw new ArgumentNullException(nameof(gameBoard) + " is null");
            _isCustomMap = true;
            _scoreGoal = scoreGoal;
            _customMapName = customMapName;
            _totalScore = totalScore;
            _passes = passes;
            _rotations = rotations;
            _overrideInventory = inventory;
            _currentStreak = currentStreak;

            InitializeComponent();
        }

        protected virtual void Game_Load(object sender, EventArgs e)
        {
            _cursorUpdateTimer = new Timer();
            _cursorUpdateTimer.Interval = 100;
            _cursorUpdateTimer.Tick += _cursorUpdateTimer_Tick;
            _cursorUpdateTimer.Start();

            _messageQueueTimer = new Timer();
            _messageQueueTimer.Interval = 2000;
            _messageQueueTimer.Tick += _messageQueueTimer_Tick;

            _matchScoreAnimationTimer = new Timer();
            _matchScoreAnimationTimer.Interval = 100;
            _matchScoreAnimationTimer.Tick += _matchScoreAnimationTimer_Tick;

            lblPassCount.Visible = _gameMode == GameMode.INDI_MODE;
            lblPassCount.Text = $"Passes: {_passes}";
            lblRotateCount.Visible = _gameMode == GameMode.INDI_MODE;
            lblRotateCount.Text = $"Rotations: {_rotations}";

            lblMessage.Text = string.Empty;
            lblLeadBlockLocation.Visible = _debugMode;

            lblScoreGoal.Visible = _isCustomMap;
            lblScoreGoal.Text = $"Goal: {_scoreGoal}";

            lblScore.Text = _totalScore.ToString();
            if (_overrideInventory == null)
            {
                this.GenerateShapeInventory();
            }
            else
            {
                this.GenerateShapeInventory(_overrideInventory);
            }
            this.InitializeBoardPlaceHolders();
            if (!_isCustomMap)
            {
                this.InitializeHighScoreBoard();
            }
            this.InitializeBoardBlocksIfAny();

            lblScoreGoal.Visible = _isCustomMap;
        }

        private void InitializeBoardBlocksIfAny()
        {
            for (int i = 0; i < _gameBoard.GetLength(0); i++)
            {
                for (int j = 0; j < _gameBoard.GetLength(1); j++)
                {
                    if (_gameBoard[i, j] != null)
                    {
                        var placeholder = _boardPlaceHolders.FirstOrDefault(p => p.XIndex == i && p.YIndex == j);
                        if (placeholder != null)
                        {
                            Block block = _gameBoard[i, j];
                            placeholder.Block = block;
                            block.XOffset = placeholder.XIndex * (CommonConstants.BLOCK_SIZE + 1);
                            block.YOffset = placeholder.YIndex * (CommonConstants.BLOCK_SIZE + 1);
                            placeholder.Click += Placeholder_Click;
                            placeholder.BringToFront();
                        }
                    }
                }
            }
        }

        private void _matchScoreAnimationTimer_Tick(object sender, EventArgs e)
        {
            if (++_matchScoreAnimationSequenceNum >= MATCH_SCORE_ANIMATION_SEQUENCES)
            {
                _matchScoreAnimationSequenceNum = 0;
                lblMatchScore.Visible = false;
                _matchScoreAnimationTimer.Stop();
            }
            else
            {
                lblMatchScore.Location = new Point(lblMatchScore.Location.X, lblMatchScore.Location.Y - 2);
            }
        }

        private void InitializeHighScoreBoard()
        {
            HighScoreBreakdown highScoreBreakdown = new HighScoreBreakdown(_gameMode);
            this.Controls.Add(highScoreBreakdown);
            highScoreBreakdown.Location = new Point(1200, 32);
        }

        private void _messageQueueTimer_Tick(object sender, EventArgs e)
        {
            if (!_messageQueue.Any())
            {
                _messageQueueTimer.Stop();
                lblMessage.Text = string.Empty;
                return;
            }

            lblMessage.Text = _messageQueue.Dequeue();
        }

        protected virtual void InitializeBoardPlaceHolders()
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
                }
            }
        }

        private void GenerateShapeInventory()
        {
            int x = INVENTORY_PANEL_PADDING_HORIZONTAL, y = INVENTORY_PANEL_PADDING_VERTICAL;

            for (int i = 0; i < CommonConstants.INVENTORY_SIZE; i++)
            {
                Shape shape = _shapeFactory.GenerateShape();
                bool isShapePlayable = _gameLogic.IsShapePlayable(_gameBoard, shape);
                shape.SetShapePlayability(isShapePlayable);
                shape.Location = new Point(x, y);
                shape.BringToFront();
                pnlShapeInventory.Controls.Add(shape);
                shape.Click += Shape_Click;

                x = shape.Right + SHAPE_SPACING_OFFSET;
            }
        }

        private void GenerateShapeInventory(List<Shape> shapes)
        {
            int x = INVENTORY_PANEL_PADDING_HORIZONTAL, y = INVENTORY_PANEL_PADDING_VERTICAL;

            for (int i = 0; i < shapes.Count; i++)
            {
                Shape shape = shapes[i];
                bool isShapePlayable = _gameLogic.IsShapePlayable(_gameBoard, shape);
                shape.SetShapePlayability(isShapePlayable);
                shape.Location = new Point(x, y);
                shape.BringToFront();
                pnlShapeInventory.Controls.Add(shape);
                shape.Click += Shape_Click;

                x = shape.Right + SHAPE_SPACING_OFFSET;
            }
        }

        private void ClearCursorBlocks()
        {
            if (_cursorBlocks.Any())
            {
                foreach (var block in _cursorBlocks)
                {
                    this.Controls.Remove(block);
                }
            }
        }

        private void AddCursorBlocksToControls(List<Block> blocks)
        {
            foreach (var block in blocks)
            {
                block.BringToFront();
                this.Controls.Add(block);
            }
        }

        private List<Block> GetCursorBlocksFromShape(Shape shape)
        {
            if (shape == null || shape.BlockMatrix == null)
                return new List<Block>();

            var blockMatrix = shape.BlockMatrix;
            int matrixWidth = blockMatrix.GetLength(0);
            int matrixHeight = blockMatrix.GetLength(1);
            List<Block> blocks = new List<Block>();

            for (int i = 0; i < matrixWidth; i++)
            {
                for (int j = 0; j < matrixHeight; j++)
                {
                    if (blockMatrix[i, j] != null)
                    {
                        Block block = new Block()
                        {
                            XOffset = blockMatrix[i, j].XOffset,
                            YOffset = blockMatrix[i, j].YOffset,
                            XIndex = blockMatrix[i, j].XIndex,
                            YIndex = blockMatrix[i, j].YIndex,
                            //CanBePlaced = shape.IsPlayable()
                        };

                        block.BringToFront();
                        blocks.Add(block);
                    }
                }
            }
            return blocks;
        }

        private void SetLocationOfCursorBlocks()
        {
            if (_debugMode)
                lblLeadBlockLocation.Text = Cursor.Position.ToString();

            foreach (var block in _cursorBlocks)
            {
                block.Location = new Point(Cursor.Position.X + block.XOffset, Cursor.Position.Y + block.YOffset);
                block.BringToFront();
                OnShapeMoved();
            }
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

        protected virtual void Shape_Click(object sender, EventArgs e)
        {
            Shape shape = sender as Shape;
            ClearCursorBlocks();
            if (shape != null && (shape.IsPlayable() || (_gameMode == GameMode.INDI_MODE && (_passes > 0 || _rotations > 0))))
            {
                _selectedShape = shape;
                _cursorBlocks = this.GetCursorBlocksFromShape(shape);
                AddCursorBlocksToControls(_cursorBlocks);
                SetLocationOfCursorBlocks();
                OnShapeChanged(shape);

            }
        }
        protected virtual void _cursorUpdateTimer_Tick(object sender, EventArgs e)
        {
            SetLocationOfCursorBlocks();
        }

        private void Game_MouseMove(object sender, MouseEventArgs e)
        {

        }

        private void PbGameBoard_Click(object sender, EventArgs e)
        {
            PlaceBlocksIfApplicable();
        }

        protected virtual void OnShapeChanged(Shape newShape)
        {
            this.SelectedShapeChanged?.Invoke(this, new ShapeChangedEventArgs() { NewShape = newShape });
        }

        protected virtual void OnShapeMoved()
        {
            ProduceShadowEffectForOpenSpaces();

            ProduceBorderEffectForPotentialMatches();

            this.ShapeMove?.Invoke(this, new ShapeMovedEventArgs() { Blocks = _cursorBlocks });
        }

        private void ProduceBorderEffectForPotentialMatches()
        {
            foreach (var block in _boardPlaceHolders)
            {
                block.BorderStyle = BorderStyle.None;
            }
            if (_selectedShape == null || _gameBoard == null)
                return;
            //check if all blocks are in the grid
            if (_highlightedPlaceholders == null || !_highlightedPlaceholders.Any())
                return;


            int xOffset = _highlightedPlaceholders.Select(x => x.XIndex).Min();
            int yOffset = _highlightedPlaceholders.Select(y => y.YIndex).Min();
            List<Point> previewMatches = _gameLogic.PreviewMatches(_gameBoard, _selectedShape, xOffset, yOffset);
            if (previewMatches != null && previewMatches.Any())
            {
                foreach (var match in previewMatches)
                {
                    var placeholder = _boardPlaceHolders.FirstOrDefault(b => b.XIndex == match.X && b.YIndex == match.Y);
                    if (placeholder != null)
                    {
                        placeholder.BorderStyle = BorderStyle.Fixed3D;

                    }
                }
            }


        }

        protected virtual void ProduceShadowEffectForOpenSpaces()
        {
            //start by clearing previous selection for empty placeholders
            var emptyPlaceholders = _boardPlaceHolders.Where(b => !b.HasBlock).ToList();
            foreach (var placeholder in emptyPlaceholders)
            {
                placeholder.SendToBack();
            }
            //clear previous highlight selection
            _highlightedPlaceholders.Clear();
            //check if all blocks are in the grid
            bool allBlocksInGrid = _cursorBlocks.All(b => IsBlockInGrid(b));
            if (allBlocksInGrid && _selectedShape != null)//use selected shape
            {
                //get lead cursor block
                var leadBlock = _selectedShape.LeadBlock;
                var leadCursorBlock = _cursorBlocks.FirstOrDefault(c => c.XOffset == leadBlock.XOffset && c.YOffset == leadBlock.YOffset);

                //find closest placeholder
                if (leadCursorBlock != null)
                {
                    var closestPlaceholder = GetClosestPlaceHolder(leadCursorBlock);
                    //if that placeholder does not have a block, go through the rest of the selected shape and find
                    //the other placeholders by indices
                    //if none of them have a block, highlight all

                    if (closestPlaceholder != null && !closestPlaceholder.HasBlock)
                    {
                        var placeholdersToHighlight = GetMatchingPlaceholdersForCursorBlocksByLocation(closestPlaceholder, emptyPlaceholders);
                        //for all open placeholders matching the shape, highlight each 
                        foreach (var placeholder in placeholdersToHighlight)
                        {
                            placeholder.BringToFront();
                        }
                        //then bring the cursor blocks to the front to make a shadow effect
                        foreach (var block in _cursorBlocks)
                        {
                            block.BringToFront();
                        }
                    }
                }
            }
        }

        protected virtual BoardPlaceHolder GetClosestPlaceHolder(Block block)
        {
            var collidingPlaceHolders = _boardPlaceHolders.Where(c => c.Bounds.IntersectsWith(block.Bounds)).ToList();
            if (collidingPlaceHolders.Any())
            {
                double minDistance = collidingPlaceHolders.Select(c => BlockUtilities.GetPythagorianDistance(c.Location.X, block.Location.X, c.Location.Y, block.Location.Y)).Min();

                var closestBlock = collidingPlaceHolders.FirstOrDefault(c => BlockUtilities.GetPythagorianDistance(c.Location.X, block.Location.X, c.Location.Y, block.Location.Y) == minDistance);
                return closestBlock;
            }

            return null;
        }

        private List<BoardPlaceHolder> GetMatchingPlaceholdersForCursorBlocksByLocation(BoardPlaceHolder referencePlaceholder, List<BoardPlaceHolder> placeholderList)
        {
            List<BoardPlaceHolder> placeholdersToHighlight = new List<BoardPlaceHolder>();
            int xIndexOffset = referencePlaceholder.XIndex - _selectedShape.LeadBlock.XIndex;
            int yIndexOffset = referencePlaceholder.YIndex;
            int xLength = _selectedShape.BlockMatrix.GetLength(0);
            int yLength = _selectedShape.BlockMatrix.GetLength(1);
            for (int i = 0; i < xLength; i++)
            {
                for (int j = 0; j < yLength; j++)
                {
                    if (_selectedShape.BlockMatrix[i, j] != null)
                    {
                        int x = i + xIndexOffset;
                        int y = j + yIndexOffset;
                        var matchingPlaceholder = placeholderList.FirstOrDefault(e => e.XIndex == x && e.YIndex == y);
                        if (matchingPlaceholder != null)
                        {
                            placeholdersToHighlight.Add(matchingPlaceholder);
                        }
                        else
                        {
                            _highlightedPlaceholders = new List<BoardPlaceHolder>();
                            return new List<BoardPlaceHolder>();
                        }
                    }
                }
            }
            _highlightedPlaceholders = placeholdersToHighlight;
            return placeholdersToHighlight;
        }

        protected virtual void Game_MouseClick(object sender, MouseEventArgs e)
        {
            PlaceBlocksIfApplicable();
        }

        protected virtual void PlaceBlocksIfApplicable()
        {
            if (_highlightedPlaceholders.Any())
            {
                //assign blocks to board
                foreach (var placeholder in _highlightedPlaceholders)
                {
                    Block block = new Block();
                    block.XOffset = placeholder.XIndex * (CommonConstants.BLOCK_SIZE + 1);
                    block.YOffset = placeholder.YIndex * (CommonConstants.BLOCK_SIZE + 1);
                    _gameBoard[placeholder.XIndex, placeholder.YIndex] = block;
                    placeholder.Block = _gameBoard[placeholder.XIndex, placeholder.YIndex];
                    placeholder.BringToFront();
                    placeholder.Click += Placeholder_Click;
                }
                if (_gameMode == GameMode.INDI_MODE && PacManEatsThePacker())
                {
                    ShowMessagesInMessageQueue("PACMAN EATS THE PACKER!");
                }
                //asses the score
                int currentStreak = _currentStreak;
                int score = _gameLogic.DestroyMatches(_gameBoard, ref _currentStreak, out string message);
                IncrementScore(score);
                int residualBlockCount = _highlightedPlaceholders.Count(b => b.Block != null);//increment any left over matches
                IncrementScore(residualBlockCount);
                if (currentStreak == _currentStreak)//if the block placement does not increase the streak, set it zero
                {
                    _currentStreak = 0;
                }
                //temporarily show match score label animation at cursor position at the time of the match
                ShowScoringAnimation(score + residualBlockCount);

                //remove shape from list of shapes
                pnlShapeInventory.Controls.Remove(_selectedShape);

                //if the list of shapes is empty, give 3 new blocks
                if (pnlShapeInventory.Controls.Count == 0)
                {
                    this.GenerateShapeInventory();
                }

                //clear selection after placing a block
                ClearShapeSelection();

                //if all shapes left are unplayable, execute "game over" logic
                CheckIfGameOver();

                //Otherwise, show messages
                ShowMessagesInMessageQueue(message);
            }
        }

        private bool PacManEatsThePacker()
        {
            if (_selectedShape != null)
            {
                string shapeName = _selectedShape.GetType().Name;
                switch (shapeName)
                {
                    case "CBlockUp":
                        var leftX = _highlightedPlaceholders.Select(x => x.XIndex).Min();
                        var topY = _highlightedPlaceholders.Select(x => x.YIndex).Min();
                        var mouthPlaceholder = _boardPlaceHolders.FirstOrDefault(f => f.XIndex == leftX + 1 && f.YIndex == topY);
                        if (mouthPlaceholder != null && mouthPlaceholder.HasBlock)
                        {
                            return true;
                        }
                        break;
                    case "CBlockDown":
                        leftX = _highlightedPlaceholders.Select(x => x.XIndex).Min();
                        var bottomY = _highlightedPlaceholders.Select(x => x.YIndex).Max();
                        mouthPlaceholder = _boardPlaceHolders.FirstOrDefault(f => f.XIndex == leftX + 1 && f.YIndex == bottomY);
                        if (mouthPlaceholder != null && mouthPlaceholder.HasBlock)
                        {
                            return true;
                        }
                        break;
                    case "CBlockLeft":
                        leftX = _highlightedPlaceholders.Select(x => x.XIndex).Min();
                        bottomY = _highlightedPlaceholders.Select(x => x.YIndex).Max();
                        mouthPlaceholder = _boardPlaceHolders.FirstOrDefault(f => f.XIndex == leftX && f.YIndex == bottomY - 1);
                        if (mouthPlaceholder != null && mouthPlaceholder.HasBlock)
                        {
                            return true;
                        }
                        break;
                    case "CBlockRight":
                        var rightX = _highlightedPlaceholders.Select(x => x.XIndex).Max();
                        bottomY = _highlightedPlaceholders.Select(x => x.YIndex).Max();
                        mouthPlaceholder = _boardPlaceHolders.FirstOrDefault(f => f.XIndex == rightX && f.YIndex == bottomY - 1);
                        if (mouthPlaceholder != null && mouthPlaceholder.HasBlock)
                        {
                            return true;
                        }
                        break;
                }
            }
            return false;
        }

        private void ShowScoringAnimation(int score)
        {
            lblMatchScore.Location = Cursor.Position;
            if (_currentStreak > 0)
            {
                _matchScoreAnimationSequenceNum = 0;
                _matchScoreAnimationTimer.Stop();
                lblMatchScore.Text = $"+{score}";
                lblMatchScore.Visible = true;
                lblMatchScore.BringToFront();
                _matchScoreAnimationTimer.Start();
            }
        }

        private void ShowMessagesInMessageQueue(string message)
        {
            if (!string.IsNullOrEmpty(message))
            {
                List<string> messages = message.Split(';').ToList();
                if (messages != null)
                {
                    foreach (var m in messages)
                    {
                        _messageQueue.Enqueue(m);
                    }
                    if (!_messageQueueTimer.Enabled)
                    {
                        lblMessage.Text = _messageQueue.Dequeue();
                        _messageQueueTimer.Start();
                    }
                }
            }
        }

        private void CheckIfGameOver()
        {
            if (pnlShapeInventory.Controls.Count > 0)
            {
                bool isGameOver = true;
                List<Shape> shapesAvailable = new List<Shape>();
                foreach (var control in pnlShapeInventory.Controls)
                {
                    var shape = control as Shape;
                    if (shape != null)
                    {
                        shapesAvailable.Add(shape);
                    }
                }
                if (_gameMode == GameMode.INDI_MODE && _passes > 0)
                {
                    isGameOver = false;
                    foreach (var shape in shapesAvailable)
                    {
                        bool isPlayable = _gameLogic.IsShapePlayable(_gameBoard, shape);
                        shape.SetShapePlayability(isPlayable);
                    }
                }
                else
                {
                    if (_gameMode == GameMode.INDI_MODE && (_rotations > 0 || shapesAvailable.Any(s => s.CanRotate)))
                    {
                        var shapesToCheck = _rotations > 0 ? shapesAvailable : shapesAvailable.Where(s => s.CanRotate).ToList();
                        int count = shapesToCheck.Count;
                        for (int i = 0; i < count; i++)
                        {
                            var currentRotatedShape = shapesToCheck[i];
                            for (int j = 0; j < 3; j++)
                            {
                                currentRotatedShape = currentRotatedShape.Rotate90(currentRotatedShape, true);
                                shapesAvailable.Add(currentRotatedShape);
                            }
                        }
                    }
                    foreach (var shape in shapesAvailable)
                    {
                        bool isPlayable = _gameLogic.IsShapePlayable(_gameBoard, shape);
                        if (pnlShapeInventory.Controls.Contains(shape))
                        {
                            shape.SetShapePlayability(isPlayable);
                        }
                        if (isPlayable)
                            isGameOver = false;
                    }
                }

                

                _isGameOver = isGameOver;

                if (isGameOver)
                {
                    MessageBox.Show($"Game Over!  Final Score {_totalScore}");
                    if (!_isCustomMap)
                    {
                        //Write to HighScores File for game Mode
                        WriteScoreToFile();
                    }
                    this.Close();
                }

                if (_totalScore >= _scoreGoal && _isCustomMap)
                {
                    MessageBox.Show("Goal Achieved!");
                    this.Close();
                }
            }
        }

        private void WriteScoreToFile()
        {
            var scoreList = HighScoreUtility.ReadScoreList(_gameMode);
            var scoreBreakdown = HighScoreUtility.GetHighScores(scoreList);

            if (_totalScore >= scoreBreakdown.AllTimeHigh)
            {
                MessageBox.Show($"New all time high score: { _totalScore }!!!");
            }
            else if (_totalScore >= scoreBreakdown.MonthlyHigh)
            {
                MessageBox.Show($"New monthly high score: { _totalScore }!!!");
            }
            else if (_totalScore >= scoreBreakdown.WeeklyHigh)
            {
                MessageBox.Show($"New weekly high score: { _totalScore }!!!");
            }
            else if (_totalScore >= scoreBreakdown.DailyHigh)
            {
                MessageBox.Show($"New daily high score: { _totalScore }!!!");
            }

            HighScoreUtility.WriteScore(_totalScore, _gameMode);
        }

        private void IncrementScore(int score)
        {
            _totalScore += score;//add match score to tally
            lblScore.Text = _totalScore.ToString();
            if (_totalScore >= _scoreTilNextPass)
            {
                int multiplier = ((_totalScore - _scoreTilNextPass) / CommonConstants.POINTS_PER_PASS) + 1;
                _passes += multiplier;
                _scoreTilNextPass += (multiplier * CommonConstants.POINTS_PER_PASS);
                lblPassCount.Text = $"Passes: {_passes}";
            }
            if (_totalScore >= _scoreTileNextRotate)
            {
                int multiplier = ((_totalScore - _scoreTileNextRotate) / CommonConstants.POINTS_PER_ROTATE) + 1;
                _rotations += multiplier;
                _scoreTileNextRotate += (multiplier * CommonConstants.POINTS_PER_ROTATE);
                lblRotateCount.Text = $"Rotations: {_rotations}";
            }
        }

        protected virtual void ClearShapeSelection()
        {
            ClearCursorBlocks();
            _highlightedPlaceholders.Clear();
            _selectedShape = null;
        }

        protected virtual void Placeholder_Click(object sender, EventArgs e)
        {
            PlaceBlocksIfApplicable();
        }

        protected virtual void Game_KeyUp(object sender, KeyEventArgs e)
        {
            if (_gameMode != GameMode.INDI_MODE)
                return;

            switch (e.KeyCode)
            {
                case Keys.P:
                    UsePass();
                    break;
                case Keys.R:
                    UseRotation();
                    break;
                case Keys.Left:
                    if (_selectedShape != null && _selectedShape.CanRotate)
                    {
                        RotateSelectedShape(false);
                    }
                    break;
                case Keys.Right:
                    if (_selectedShape != null && _selectedShape.CanRotate)
                    {
                        RotateSelectedShape(true);
                    }
                    break;
            }
        }

        private void RotateSelectedShape(bool clockwise)
        {
            if (_selectedShape != null)
            {
                //remove current shape from inventory
                pnlShapeInventory.Controls.Remove(_selectedShape);
                //generate new shape
                _selectedShape = _selectedShape.Rotate90(_selectedShape, clockwise);
                //register click event for shape
                _selectedShape.Click += Shape_Click;
                //set cursor blocks from new shape
                ClearCursorBlocks();
                _cursorBlocks = this.GetCursorBlocksFromShape(_selectedShape);
                AddCursorBlocksToControls(_cursorBlocks);
                SetLocationOfCursorBlocks();
                //insert new shape in the right spot in the inventory
                ResetBlockInventoryLocations();
                //set the image of the blocks based on playability
                bool playable = _gameLogic.IsShapePlayable(_gameBoard, _selectedShape);
                _selectedShape.SetShapePlayability(playable);
                //fire changed event
                OnShapeChanged(_selectedShape);
                //finally, check if game over
                CheckIfGameOver();
            }
        }

        private void UseRotation()
        {
            if (_selectedShape != null && _rotations > 0 && !_selectedShape.CanRotate)
            {
                _rotations--;
                lblRotateCount.Text = $"Rotations: {_rotations}";
                _selectedShape.CanRotate = true;
            }
        }
        private void UsePass()
        {
            if (_passes > 0 && _selectedShape != null)
            {
                //decrement passes
                _passes--;
                lblPassCount.Text = $"Passes: {_passes}";
                //remove current shape from inventory
                pnlShapeInventory.Controls.Remove(_selectedShape);
                //generate new shape
                string originalType = _selectedShape.GetType().Name;
                do
                {
                    _selectedShape = _shapeFactory.GenerateShape();
                } while (_selectedShape.GetType().Name == originalType);
                //register click event for shape
                _selectedShape.Click += Shape_Click;
                //set cursor blocks from new shape
                ClearCursorBlocks();
                _cursorBlocks = this.GetCursorBlocksFromShape(_selectedShape);
                AddCursorBlocksToControls(_cursorBlocks);
                SetLocationOfCursorBlocks();
                //insert new shape in the right spot in the inventory
                ResetBlockInventoryLocations();
                //set the image of the blocks based on playability
                bool playable = _gameLogic.IsShapePlayable(_gameBoard, _selectedShape);
                _selectedShape.SetShapePlayability(playable);
                //fire changed event
                OnShapeChanged(_selectedShape);
                //finally, check if game over
                CheckIfGameOver();
            }
        }

        private void ResetBlockInventoryLocations()
        {
            //add all shapes to a temp list
            List<Shape> shapes = new List<Shape>();
            if (_selectedShape != null)
            {
                shapes.Add(_selectedShape);
            }
            foreach (var control in pnlShapeInventory.Controls)
            {
                var shape = control as Shape;
                if (shape != null)
                {
                    shapes.Add(shape);
                }
            }
            //clear controls list
            pnlShapeInventory.Controls.Clear();
            //re-insert controls at correct place
            int x = INVENTORY_PANEL_PADDING_HORIZONTAL, y = INVENTORY_PANEL_PADDING_VERTICAL;
            for (int i = 0; i < shapes.Count; i++)
            {
                pnlShapeInventory.Controls.Add(shapes[i]);
                shapes[i].Location = new Point(x, y);
                x = shapes[i].Right + SHAPE_SPACING_OFFSET;
            }
        }

        private void Game_FormClosing(object sender, FormClosingEventArgs e)
        {
            if (!_isGameOver)
            {

                SavedGame savedGame = null;
                switch (_gameMode)
                {

                    case GameMode.STANDARD:
                        List<Shape> inventory = new List<Shape>();
                        savedGame = new StandardModeGame()
                        {
                            Score = _totalScore,
                            GameMode = _gameMode,
                            GameBoard = _gameBoard,
                            CurrentStreak = _currentStreak
                        };

                        foreach (var ctrl in pnlShapeInventory.Controls)
                        {
                            var shape = ctrl as Shape;
                            if (shape != null)
                            {
                                inventory.Add(shape);
                            }
                        }
                        savedGame.Inventory = inventory;
                        break;
                    case GameMode.INDI_MODE:
                        inventory = new List<Shape>();
                        savedGame = new IndiModeGame()
                        {
                            Score = _totalScore,
                            GameMode = _gameMode,
                            GameBoard = _gameBoard,
                            Passes = _passes,
                            Rotations = _rotations,
                            CurrentStreak = _currentStreak
                        };
                        foreach (var ctrl in pnlShapeInventory.Controls)
                        {
                            var shape = ctrl as Shape;
                            if (shape != null)
                            {
                                inventory.Add(shape);
                            }
                        }
                        savedGame.Inventory = inventory;
                        break;
                }
                if (savedGame != null)
                {
                    if (_isCustomMap)
                    {
                        SavedGameUtility.SaveGame(savedGame, _customMapName);
                    }
                    else
                    {
                        SavedGameUtility.SaveGame(savedGame);
                    }

                }
            }
            else
            {
                if (!_isCustomMap)
                {
                    SavedGameUtility.DeleteSavedGameByMode(_gameMode);
                }
                else
                {
                    string path = SavedGameUtility.GetSavedGamesDirectory() + _gameMode.ToString() + "_" + _customMapName + ".txt";
                    SavedGameUtility.DeleteFile(path);
                }
            }
        }
    }
}
