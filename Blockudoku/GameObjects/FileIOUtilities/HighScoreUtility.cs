using Blockudoku.GameObjects.GameLogicUtilities;
using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.IO;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Blockudoku.GameObjects.FileIOUtilities
{
    public static class HighScoreUtility
    {
        public static void WriteScore(int highScore, GameMode gameMode)
        {
            try
            {
                string filePath = System.Environment.CurrentDirectory + @"/" + gameMode + "_SCORES.txt";
                using (FileStream fs = new FileStream(filePath, FileMode.Append, FileAccess.Write))
                using (StreamWriter writer = new StreamWriter(fs))
                {
                    BlockudokuScore score = new BlockudokuScore
                    {
                        Score = highScore,
                        ScoreDateTime = DateTime.Now
                    };
                    string text = $"{score.Score},{score.ScoreDateTime}";
                    writer.WriteLine(text);
                }
            }
            catch (Exception ex)
            {
                Debug.WriteLine($"Error writing score: {ex.Message}");
            }
        }

        public static List<BlockudokuScore> ReadScoreList(GameMode gameMode)
        {
            string filePath = System.Environment.CurrentDirectory + @"/" + gameMode + "_SCORES.txt";
            List<BlockudokuScore> scores = new List<BlockudokuScore>();
            try
            {
                using (FileStream fs = new FileStream(filePath, FileMode.OpenOrCreate, FileAccess.Read))
                using (StreamReader reader = new StreamReader(fs))
                {
                    while (!reader.EndOfStream)
                    {
                        string rawText = reader.ReadLine();
                        string[] rawProperties = rawText.Split(',');
                        BlockudokuScore score = new BlockudokuScore()
                        {
                            Score = Convert.ToInt32(rawProperties[0]),
                            ScoreDateTime = DateTime.Parse(rawProperties[1])
                        };
                        scores.Add(score);
                    }
                }
            }
            catch (Exception ex)
            {
                Debug.WriteLine($"Error reading scores: {ex.Message}");
            }
            return scores;
        }

        public static BlockudokuHighScoreByTimePeriod GetHighScores(List<BlockudokuScore> scoreList)
        {
            int allTimeHigh = 0;
            int montlhyHigh = 0;
            int weeklyHigh = 0;
            int dailyHigh = 0;

            foreach (var score in scoreList)
            {
                if (score.Score > allTimeHigh)
                {
                    allTimeHigh = score.Score;
                }

                if (score.Score > montlhyHigh && score.ScoreDateTime >= DateTime.Now.AddDays(-30))
                {
                    montlhyHigh = score.Score;
                }

                if (score.Score > weeklyHigh && score.ScoreDateTime >= DateTime.Now.AddDays(-7))
                {
                    weeklyHigh = score.Score;
                }

                if (score.Score > dailyHigh && score.ScoreDateTime >= DateTime.Now.AddHours(-24))
                {
                    dailyHigh = score.Score;
                }
            }

            BlockudokuHighScoreByTimePeriod scoreBreakdown = new BlockudokuHighScoreByTimePeriod()
            {
                AllTimeHigh = allTimeHigh,
                MonthlyHigh = montlhyHigh,
                WeeklyHigh = weeklyHigh,
                DailyHigh = dailyHigh
            };

            return scoreBreakdown;
        }
    }
}
