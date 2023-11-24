using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Drawing;
using System.Data;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;
using Blockudoku.GameObjects;
using Blockudoku.GameObjects.GameLogicUtilities;
using Blockudoku.GameObjects.FileIOUtilities;

namespace Blockudoku.UserControls
{
    public partial class HighScoreBreakdown : UserControl
    {
        private BlockudokuHighScoreByTimePeriod _scoresByTimePeriod;
        private GameMode _gameMode;
        public HighScoreBreakdown(GameMode gameMode)
        {
            _gameMode = gameMode;
            InitializeComponent();
        }

        private void HighScoreBreakdown_Load(object sender, EventArgs e)
        {
            var scoreList = HighScoreUtility.ReadScoreList(_gameMode);
            _scoresByTimePeriod = HighScoreUtility.GetHighScores(scoreList);
            lblAllTime.Text += $" {_scoresByTimePeriod.AllTimeHigh}";
            lblMonthly.Text += $" {_scoresByTimePeriod.MonthlyHigh}";
            lblWeekly.Text += $" {_scoresByTimePeriod.WeeklyHigh}";
            lblDaily.Text += $" {_scoresByTimePeriod.DailyHigh}";
        }
    }
}
