using Blockudoku.GameObjects.FileIOUtilities.DataContracts;
using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Diagnostics;
using Blockudoku.Constants;
using Blockudoku.GameObjects.GameLogicUtilities;
using Blockudoku.GameObjects.Shapes;

namespace Blockudoku.GameObjects.FileIOUtilities
{
    public static class SavedGameUtility
    {
        private const string SAVED_GAMES_FOLDER = "Saved Games";
        private const char SEPARATOR = ',';
        private const string END_INVENTORY = "END_INVENTORY";

        #region delete
        public static bool DeleteFile(string path)
        {
            try
            {
                File.Delete(path);
                return true;
            }
            catch (Exception ex)
            {
                Debug.WriteLine($"Error deleting file {path}: {ex.Message}");
                return false;
            }
        }

        public static bool DeleteSavedGameByMode(GameMode mode)
        {
            string filePath = GetSavedGamesDirectory() + mode.ToString() + ".txt";
            return DeleteFile(filePath);
        }
        #endregion

        #region Saving
        public static bool SaveGame(SavedGame game, string mapName = null)
        {
            try
            {
                if (game == null)
                    throw new ArgumentNullException("Saved Game cannot be null");

                string mapNameAppendor = string.IsNullOrEmpty(mapName) ? string.Empty : "_" + mapName;
                string filePath =  GetSavedGamesDirectory() + game.GameMode.ToString() + mapNameAppendor + ".txt";
                if (File.Exists(filePath))
                {
                    File.Delete(filePath);
                }
                using (FileStream fs = new FileStream(filePath, FileMode.OpenOrCreate, FileAccess.Write))
                using (StreamWriter writer = new StreamWriter(fs))
                {
                    switch (game.GameMode)
                    {
                        case GameLogicUtilities.GameMode.STANDARD:
                            return SaveStandardGame(writer, (StandardModeGame)game);
                        case GameLogicUtilities.GameMode.INDI_MODE:
                            return SaveIndiModeGame(writer, (IndiModeGame)game);
                    }
                }
                return true;
            }
            catch (Exception ex)
            {
                Debug.WriteLine($"Error Saving Game: {ex.Message} \n {ex}");
                return false;
            }
        }

        private static bool SaveStandardGame(StreamWriter writer, StandardModeGame game)
        {
            try
            {
                writer.WriteLine(game.Score);
                writer.WriteLine(game.CurrentStreak);
                foreach (var shape in game.Inventory)
                {
                    writer.WriteLine(shape.GetType().Name);
                }
                writer.WriteLine(END_INVENTORY);
                int width = game.GameBoard.GetLength(0);
                int height = game.GameBoard.GetLength(1);
                for (int i = 0; i < width; i++)
                {
                    for (int j = 0; j < height; j++)
                    {
                        var block = game.GameBoard[i, j];
                        if (block != null)
                        {
                            string text = $"{i}{SEPARATOR}{j}{SEPARATOR}{block.Health}";
                            writer.WriteLine(text);
                        }
                    }
                }
                return true;
            }
            catch (Exception ex)
            {
                Debug.WriteLine($"Error Saving Standard Mode Game: {ex.Message} \n {ex}");
                return false;
            }
        }

        private static bool SaveIndiModeGame(StreamWriter writer, IndiModeGame game)
        {
            try
            {
                writer.WriteLine(game.Score);
                writer.WriteLine(game.CurrentStreak);
                writer.WriteLine(game.Passes);
                writer.WriteLine(game.Rotations);
                foreach (var shape in game.Inventory)
                {
                    writer.WriteLine(shape.GetType().Name);
                }
                writer.WriteLine(END_INVENTORY);
                int width = game.GameBoard.GetLength(0);
                int height = game.GameBoard.GetLength(1);
                for (int i = 0; i < width; i++)
                {
                    for (int j = 0; j < height; j++)
                    {
                        var block = game.GameBoard[i, j];
                        if (block != null)
                        {
                            string text = $"{i}{SEPARATOR}{j}{SEPARATOR}{block.Health}";
                            writer.WriteLine(text);
                        }
                    }
                }
                return true;
            }
            catch (Exception ex)
            {
                Debug.WriteLine($"Error Saving Standard Mode Game: {ex.Message} \n {ex}");
                return false;
            }
        }

        #endregion

        #region Loading
        public static bool LoadGame(GameMode game, out SavedGame loadedGame, string mapName = null)
        {
            loadedGame = null;
            try
            {
                string mapNameAppendor = string.IsNullOrEmpty(mapName) ? string.Empty : "_" + mapName;
                string filePath = GetSavedGamesDirectory() + game.ToString() + mapNameAppendor + ".txt";
                using (FileStream fs = new FileStream(filePath, FileMode.Open))
                using (StreamReader reader = new StreamReader(fs))
                {
                    switch (game)
                    {
                        case GameLogicUtilities.GameMode.STANDARD:
                            loadedGame = LoadStandardGame(reader, game);
                            break;
                        case GameLogicUtilities.GameMode.INDI_MODE:
                            loadedGame = LoadIndiModeGame(reader, game);
                            break;
                    }
                }
                return true;
            }
            catch (Exception ex)
            {
                Debug.WriteLine($"Error Saving Game: {ex.Message} \n {ex}");
                return false;
            }
        }

        public static List<string> GetSavedGames()
        {
            string path = GetSavedGamesDirectory();
            return Directory.GetFiles(path, "*.txt").Select(f => f.Substring(f.LastIndexOf('/') + 1)).ToList();
        }

        private static StandardModeGame LoadStandardGame(StreamReader reader, GameMode game)
        {
            Block[,] gameBoard = new Block[CommonConstants.GAME_BOARD_SIZE, CommonConstants.GAME_BOARD_SIZE];
            List<Shape> inventory = new List<Shape>();
            int score = Convert.ToInt32(reader.ReadLine());
            int currentStreak = Convert.ToInt32(reader.ReadLine());
            bool endInventory = false;
            while (!reader.EndOfStream)
            {
                string text = reader.ReadLine();
                if (!endInventory)
                {
                    if (text == END_INVENTORY)
                    {
                        endInventory = true;
                    }
                    else
                    {
                        string typeName = text;
                        StandardModeShapeFactory factory = new StandardModeShapeFactory();
                        var shape = factory.GenerateShape(typeName);
                        inventory.Add(shape);
                    }
                }
                else
                {
                    string[] data = text.Split(SEPARATOR);
                    int x = Convert.ToInt32(data[0]);
                    int y = Convert.ToInt32(data[1]);
                    int health = Convert.ToInt32(data[2]);
                    Block block = new Block(true, health)
                    {
                        XIndex = x,
                        YIndex = y
                    };
                    gameBoard[x, y] = block;
                }
            }
            StandardModeGame standardModeGame = new StandardModeGame()
            {
                Score = score,
                GameMode = game,
                GameBoard = gameBoard,
                Inventory = inventory,
                CurrentStreak = currentStreak
            };
            return standardModeGame;
        }


        private static IndiModeGame LoadIndiModeGame(StreamReader reader, GameMode game)
        {
            Block[,] gameBoard = new Block[CommonConstants.GAME_BOARD_SIZE, CommonConstants.GAME_BOARD_SIZE];
            List<Shape> inventory = new List<Shape>();
            int score = Convert.ToInt32(reader.ReadLine());
            int currentStreak = Convert.ToInt32(reader.ReadLine());
            int passes = Convert.ToInt32(reader.ReadLine());
            int rotations = Convert.ToInt32(reader.ReadLine());
            bool endInventory = false;
            while (!reader.EndOfStream)
            {
                string text = reader.ReadLine();
                if (!endInventory)
                {
                    if (text == END_INVENTORY)
                    {
                        endInventory = true;
                    }
                    else
                    {
                        string typeName = text;
                        StandardModeShapeFactory factory = new StandardModeShapeFactory();
                        var shape = factory.GenerateShape(typeName);
                        inventory.Add(shape);
                    }
                }
                else
                {
                    string[] data = text.Split(SEPARATOR);
                    int x = Convert.ToInt32(data[0]);
                    int y = Convert.ToInt32(data[1]);
                    int health = Convert.ToInt32(data[2]);
                    Block block = new Block(true, health)
                    {
                        XIndex = x,
                        YIndex = y
                    };
                    gameBoard[x, y] = block;
                }
            }
            IndiModeGame indiModeGame = new IndiModeGame()
            {
                Score = score,
                Passes = passes,
                Rotations = rotations,
                GameMode = game,
                GameBoard = gameBoard,
                Inventory = inventory,
                CurrentStreak = currentStreak
            };
            return indiModeGame;
        }

        #endregion

        public static string GetSavedGamesDirectory()
        {
            string directory = System.Environment.CurrentDirectory + @"/" + SAVED_GAMES_FOLDER + "/";
            if (!Directory.Exists(directory))
            {
                Directory.CreateDirectory(directory);
            }
            return directory;
        }
    }
}
