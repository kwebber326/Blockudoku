using Blockudoku.GameObjects;
using Blockudoku.GameObjects.FileIOUtilities;
using Blockudoku.GameObjects.FileIOUtilities.DataContracts;
using Blockudoku.GameObjects.GameLogicUtilities;
using Blockudoku.GameObjects.Shapes;
using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.IO;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;

namespace Blockudoku
{
    public partial class MainMenu : Form
    {
        private GameModeData _selectedGameModeData;
        private List<string> _savedGameFiles = new List<string>();

        private Dictionary<string, GameModeData> _gameModeDescriptionDictionary = new Dictionary<string, GameModeData>()
        {
            { "Standard", new GameModeData { Description = "The standard game mode from the original Blockudoku app", GameMode = GameMode.STANDARD } },
            { "Indi Mode", new GameModeData { Description = "Blocks can be rotated 90 degrees in either direction and 3x3 and blocks can occasionally be swapped for another", GameMode = GameMode.INDI_MODE } },
            { "Level Editor",  new GameModeData { Description = "Create your own levels with point goals and predefined blocks on the board", GameMode = GameMode.LEVEL_EDITOR } }
        };
        public MainMenu()
        {
            InitializeComponent();
        }

        private void Form1_Load(object sender, EventArgs e)
        {
            cmbGameMode.SelectedIndex = 0;
            CmbGameMode_SelectedIndexChanged(this, EventArgs.Empty);

            InitializeMapList();
            SetSavedGameFiles();
            UpdateSaveGamePanel();

            pnlCustomLevelSelect.Visible = _selectedGameModeData != null && _selectedGameModeData.GameMode != GameMode.LEVEL_EDITOR;
        }

        private void InitializeMapList()
        {
            List<string> mapList = LevelEditorFileIOUtility.GetMaps();

            cmbMapNames.Items.AddRange(mapList.ToArray());
            cmbMapNames.Visible = chkPlayCustom.Checked;
        }

        private void SetSavedGameFiles()
        {
            _savedGameFiles = SavedGameUtility.GetSavedGames();
        }

        private void UpdateSaveGamePanel()
        {
            SetSavedGameFiles();
            bool hasSavedGame = HasSavedGame();
            pnlContinueNew.Visible = hasSavedGame;
            if (hasSavedGame)
            {
                rdbContinue.Checked = true;
                rdbNew.Checked = false;
            }
            else
            {
                rdbNew.Checked = true;
                rdbContinue.Checked = false;
            }
        }

        private bool HasSavedGame()
        {
            switch (_selectedGameModeData.GameMode)
            {
                case GameMode.STANDARD:
                case GameMode.INDI_MODE:
                    if (!chkPlayCustom.Checked)
                        return _savedGameFiles.Contains(_selectedGameModeData.GameMode.ToString() + ".txt");
                    else
                    {
                        string mapName = cmbMapNames.SelectedItem?.ToString();
                        string path = SavedGameUtility.GetSavedGamesDirectory() + _selectedGameModeData.GameMode.ToString() + "_" + mapName + ".txt";
                        return File.Exists(path);
                    }
            }
            return false;
        }

        private void CmbGameMode_SelectedIndexChanged(object sender, EventArgs e)
        {
            string text = _gameModeDescriptionDictionary.TryGetValue(cmbGameMode.SelectedItem?.ToString(), out GameModeData data) ? data.Description : string.Empty;
            lblDescription.Text = text;
            _selectedGameModeData = data;
            pnlCustomLevelSelect.Visible = _selectedGameModeData != null && _selectedGameModeData.GameMode != GameMode.LEVEL_EDITOR;
            UpdateSaveGamePanel();
        }

        private void BtnPlay_Click(object sender, EventArgs e)
        {
            if (chkPlayCustom.Checked && cmbMapNames.SelectedItem == null && (_selectedGameModeData != null && _selectedGameModeData.GameMode != GameMode.LEVEL_EDITOR))
            {
                MessageBox.Show("Select a Custom Map to play or unselected the 'Play Custom Map' option");
                return;
            }

            IGameLogicFactory gameModeFactory = new GameLogicFactory();
            var gameMode = _selectedGameModeData.GameMode;
            IShapeFactory shapeFactory = GameModeShapeFactory.GetFactoryFromGameMode(gameMode);
            if (gameMode == GameMode.INDI_MODE || gameMode == GameMode.STANDARD)
            {
                Game game = null;
                if (!chkPlayCustom.Checked)
                {
                    if (rdbContinue.Checked)
                    {
                        var success = SavedGameUtility.LoadGame(gameMode, out SavedGame loadedGame);
                        if (success && gameMode == GameMode.STANDARD)
                        {
                            var standardGameMode = (StandardModeGame)loadedGame;
                            game = new Game(gameMode, gameModeFactory, shapeFactory, standardGameMode.Score, standardGameMode.GameBoard, standardGameMode.Inventory, standardGameMode.CurrentStreak);
                        }
                        else if (success && gameMode == GameMode.INDI_MODE)
                        {
                            var indiModeGame = (IndiModeGame)loadedGame;
                            game = new Game(gameMode, gameModeFactory, shapeFactory, indiModeGame.Score, indiModeGame.Passes, indiModeGame.Rotations, indiModeGame.GameBoard, indiModeGame.Inventory, indiModeGame.CurrentStreak);
                        }
                        else
                        {
                            MessageBox.Show("Could not load saved game");
                            game = new Game(gameMode, gameModeFactory, shapeFactory);
                        }
                    }
                    else
                    {
                        game = new Game(gameMode, gameModeFactory, shapeFactory);
                    }
                    if (game.ShowDialog() == DialogResult.OK)
                    {

                    }
                    UpdateSaveGamePanel();
                }
                else
                {
                    string mapName = cmbMapNames.SelectedItem.ToString();
                    if (rdbContinue.Checked)
                    {
                        bool loadedSavedGame = SavedGameUtility.LoadGame(gameMode, out SavedGame loadedGame, mapName) ;
                        bool loadedMap = LevelEditorFileIOUtility.LoadMap(mapName, out Block[,] gameBoard, out int scoreGoal);
                        bool success = loadedSavedGame && loadedMap;
                        if (success && gameMode == GameMode.STANDARD)
                        {
                            var standardGameMode = (StandardModeGame)loadedGame;
                
                            game = new Game(gameMode, gameModeFactory, shapeFactory, standardGameMode.GameBoard, scoreGoal, mapName, standardGameMode.Score,  0, 0, standardGameMode.Inventory, standardGameMode.CurrentStreak);
                        }
                        else if (success && gameMode == GameMode.INDI_MODE)
                        {
                            var indiModeGame = (IndiModeGame)loadedGame;
                            game = new Game(gameMode, gameModeFactory, shapeFactory, indiModeGame.GameBoard, scoreGoal, mapName, indiModeGame.Score, indiModeGame.Passes, indiModeGame.Rotations, indiModeGame.Inventory, indiModeGame.CurrentStreak);
                        }
                        else
                        {
                            MessageBox.Show("Could not load saved game");
                            game = new Game(gameMode, gameModeFactory, shapeFactory);
                        }
                        if (game.ShowDialog() == DialogResult.OK)
                        {

                        }
                        UpdateSaveGamePanel();
                    }
                    else
                    {
                      
                        if (LevelEditorFileIOUtility.LoadMap(mapName, out Block[,] gameBoard, out int scoreGoal))
                        {
                            game = new Game(gameMode, gameModeFactory, shapeFactory, gameBoard, scoreGoal, mapName);
                            if (game.ShowDialog() == DialogResult.OK)
                            {

                            }
                            UpdateSaveGamePanel();
                        }
                        else
                        {
                            MessageBox.Show($"Error reading map: { mapName }", "Error", MessageBoxButtons.OK, MessageBoxIcon.Error);
                        }
                    }
                }
            }
            else
            {
                LevelEditor editor = new LevelEditor(shapeFactory);
                editor.ShowDialog();
                cmbMapNames.Items.Clear();
                InitializeMapList();
            }
        }

        private void CmbMapNames_SelectedIndexChanged(object sender, EventArgs e)
        {
            UpdateSaveGamePanel();
        }

        private void ChkPlayCustom_CheckedChanged(object sender, EventArgs e)
        {
            cmbMapNames.Visible = chkPlayCustom.Checked;
            UpdateSaveGamePanel();
        }
    }

    public class GameModeData
    {
        public string Description { get; set; }

        public GameMode GameMode { get; set; }
    }
}
