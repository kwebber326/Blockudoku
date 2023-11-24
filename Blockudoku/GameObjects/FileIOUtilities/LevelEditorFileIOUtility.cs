using Blockudoku.Constants;
using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.IO;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Blockudoku.GameObjects.FileIOUtilities
{
    public static class LevelEditorFileIOUtility
    {
        private const string ITEM_SEPARATOR = ",";
        public const string MAPS_FOLDER = "CustomMaps";
        public const string DEFAULT_MAP_NAME = "CustomLevel";
        public static bool SaveMap(string mapName, Block[,] gameBoard, int scoreGoal = CustomModeConstants.DEFAULT_SCORE_GOAL)
        {
            if (string.IsNullOrEmpty(mapName) || gameBoard == null)
                return false;

            try
            {
                string filePath = GetFilePathFromMapName(mapName);
                using (FileStream fs = new FileStream(filePath, FileMode.OpenOrCreate, FileAccess.Write))
                using (StreamWriter writer = new StreamWriter(fs))
                {
                    writer.WriteLine(scoreGoal.ToString());
                    for (int i = 0; i < gameBoard.GetLength(0); i++)
                    {
                        for (int j = 0; j < gameBoard.GetLength(1); j++)
                        {
                            if (gameBoard[i, j] != null)
                            {
                                int health = gameBoard[i, j].Health;
                                string dataString = string.Join(ITEM_SEPARATOR, i, j, health);
                                writer.WriteLine(dataString);
                            }
                        }
                    }
                }
                return true;
            }
            catch (Exception ex)
            {
                Debug.WriteLine($"Error saving map {mapName}: {ex.Message} {ex}");
                return false;
            }
        }

        public static bool LoadMap(string mapName, out Block[,] gameBoard, out int scoreGoal)
        {
            int size = CommonConstants.GAME_BOARD_SIZE;
            gameBoard = new Block[size, size];
            scoreGoal = CustomModeConstants.DEFAULT_SCORE_GOAL;

            if (string.IsNullOrEmpty(mapName))
                return false;

            try
            {
                string filePath = GetFilePathFromMapName(mapName);
                using (FileStream fs = new FileStream(filePath, FileMode.Open, FileAccess.Read))
                using (StreamReader reader = new StreamReader(fs))
                {
                    bool firstLine = true;
                    while (!reader.EndOfStream)
                    {
                        string rawString = reader.ReadLine();
                        if (string.IsNullOrWhiteSpace(rawString))
                            continue;

                        if (firstLine)
                        {
                            scoreGoal = Convert.ToInt32(rawString);
                            firstLine = false;
                            continue;
                        }
                        char separator = Convert.ToChar(ITEM_SEPARATOR);
                        string[] items = rawString.Split(separator);
                        int x = Convert.ToInt32(items[0]);
                        int y = Convert.ToInt32(items[1]);
                        int health = Convert.ToInt32(items[2]);

                        Block block = new Block(true, health);
                        block.XIndex = x;
                        block.YIndex = y;
                        block.XOffset = (CommonConstants.BLOCK_SIZE * x) + x;
                        block.YOffset = (CommonConstants.BLOCK_SIZE * y) + y;
                        gameBoard[x, y] = block;
                    }
                }
                return true;
            }
            catch (Exception ex)
            {
                Debug.WriteLine($"Error loading map {mapName}: {ex.Message} {ex}");
                return false;
            }
        }

        internal static void LoadMap(string mapName, out object p)
        {
            throw new NotImplementedException();
        }

        private static string GetFilePathFromMapName(string mapName)
        {
            string directory = GetDirectoryForCustomMaps();
            string filePath = directory + @"/" + mapName + ".txt";
            return filePath;
        }

        private static string GetDirectoryForCustomMaps()
        {
            string directory = System.Environment.CurrentDirectory + @"/" + MAPS_FOLDER;
            if (!Directory.Exists(directory))
            {
                Directory.CreateDirectory(directory);
            }

            return directory;
        }

        public static bool MapExists(string mapName)
        {
            if (string.IsNullOrEmpty(mapName))
                return false;

            try
            {
                string filePath = System.Environment.CurrentDirectory + @"/" + MAPS_FOLDER + @"/" + mapName + ".txt";
                bool exists = File.Exists(filePath);
                return exists;
            }
            catch (Exception ex)
            {
                Debug.WriteLine($"Error determining if map {mapName} exists: {ex.Message} {ex}");
                return false;
            }
                    
        }

        public static List<string> GetMaps()
        {
            try
            {
                string directory = GetDirectoryForCustomMaps();
                DirectoryInfo info = new DirectoryInfo(directory);
                List<string> maps = new List<string>();
                var files = info.GetFiles();
                foreach (var fileInfo in files)
                {
                    maps.Add(fileInfo.Name.Replace(".txt", ""));
                }
                return maps;
            }
            catch (Exception ex)
            {
                Debug.WriteLine($"Error getting map list: {ex.Message} {ex}");
                return new List<string>();
            }
        }
    }
}
