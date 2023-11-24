namespace Blockudoku
{
    partial class Game
    {
        /// <summary>
        /// Required designer variable.
        /// </summary>
        private System.ComponentModel.IContainer components = null;

        /// <summary>
        /// Clean up any resources being used.
        /// </summary>
        /// <param name="disposing">true if managed resources should be disposed; otherwise, false.</param>
        protected override void Dispose(bool disposing)
        {
            if (disposing && (components != null))
            {
                components.Dispose();
            }
            base.Dispose(disposing);
        }

        #region Windows Form Designer generated code

        /// <summary>
        /// Required method for Designer support - do not modify
        /// the contents of this method with the code editor.
        /// </summary>
        private void InitializeComponent()
        {
            this.pnlShapeInventory = new System.Windows.Forms.Panel();
            this.pbGameBoard = new System.Windows.Forms.PictureBox();
            this.lblScore = new System.Windows.Forms.Label();
            this.lblMessage = new System.Windows.Forms.Label();
            this.lblLeadBlockLocation = new System.Windows.Forms.Label();
            this.lblMatchScore = new System.Windows.Forms.Label();
            this.lblRotateCount = new System.Windows.Forms.Label();
            this.lblPassCount = new System.Windows.Forms.Label();
            this.lblScoreGoal = new System.Windows.Forms.Label();
            ((System.ComponentModel.ISupportInitialize)(this.pbGameBoard)).BeginInit();
            this.SuspendLayout();
            // 
            // pnlShapeInventory
            // 
            this.pnlShapeInventory.Location = new System.Drawing.Point(184, 920);
            this.pnlShapeInventory.Name = "pnlShapeInventory";
            this.pnlShapeInventory.Size = new System.Drawing.Size(1362, 515);
            this.pnlShapeInventory.TabIndex = 1;
            // 
            // pbGameBoard
            // 
            this.pbGameBoard.Image = global::Blockudoku.Properties.Resources.blockudoku_board1;
            this.pbGameBoard.InitialImage = global::Blockudoku.Properties.Resources.blockudoku_board1;
            this.pbGameBoard.Location = new System.Drawing.Point(248, 188);
            this.pbGameBoard.Name = "pbGameBoard";
            this.pbGameBoard.Size = new System.Drawing.Size(472, 472);
            this.pbGameBoard.SizeMode = System.Windows.Forms.PictureBoxSizeMode.AutoSize;
            this.pbGameBoard.TabIndex = 0;
            this.pbGameBoard.TabStop = false;
            this.pbGameBoard.Click += new System.EventHandler(this.PbGameBoard_Click);
            // 
            // lblScore
            // 
            this.lblScore.AutoSize = true;
            this.lblScore.Font = new System.Drawing.Font("Microsoft Sans Serif", 14F, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.lblScore.Location = new System.Drawing.Point(468, 9);
            this.lblScore.Name = "lblScore";
            this.lblScore.Size = new System.Drawing.Size(32, 32);
            this.lblScore.TabIndex = 2;
            this.lblScore.Text = "0";
            // 
            // lblMessage
            // 
            this.lblMessage.AutoSize = true;
            this.lblMessage.Font = new System.Drawing.Font("Microsoft Sans Serif", 14F, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.lblMessage.ForeColor = System.Drawing.SystemColors.MenuHighlight;
            this.lblMessage.Location = new System.Drawing.Point(986, 188);
            this.lblMessage.Name = "lblMessage";
            this.lblMessage.Size = new System.Drawing.Size(185, 32);
            this.lblMessage.TabIndex = 3;
            this.lblMessage.Text = "Sample Text";
            // 
            // lblLeadBlockLocation
            // 
            this.lblLeadBlockLocation.AutoSize = true;
            this.lblLeadBlockLocation.Location = new System.Drawing.Point(268, 118);
            this.lblLeadBlockLocation.Name = "lblLeadBlockLocation";
            this.lblLeadBlockLocation.Size = new System.Drawing.Size(51, 20);
            this.lblLeadBlockLocation.TabIndex = 4;
            this.lblLeadBlockLocation.Text = "label1";
            // 
            // lblMatchScore
            // 
            this.lblMatchScore.AutoSize = true;
            this.lblMatchScore.Font = new System.Drawing.Font("Microsoft Sans Serif", 10F, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.lblMatchScore.ForeColor = System.Drawing.Color.Red;
            this.lblMatchScore.Location = new System.Drawing.Point(39, 303);
            this.lblMatchScore.Name = "lblMatchScore";
            this.lblMatchScore.Size = new System.Drawing.Size(70, 25);
            this.lblMatchScore.TabIndex = 5;
            this.lblMatchScore.Text = "label1";
            this.lblMatchScore.Visible = false;
            // 
            // lblRotateCount
            // 
            this.lblRotateCount.AutoSize = true;
            this.lblRotateCount.Font = new System.Drawing.Font("Microsoft Sans Serif", 8F, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.lblRotateCount.Location = new System.Drawing.Point(474, 58);
            this.lblRotateCount.Name = "lblRotateCount";
            this.lblRotateCount.Size = new System.Drawing.Size(107, 20);
            this.lblRotateCount.TabIndex = 6;
            this.lblRotateCount.Text = "Rotations: 0";
            this.lblRotateCount.Visible = false;
            // 
            // lblPassCount
            // 
            this.lblPassCount.AutoSize = true;
            this.lblPassCount.Font = new System.Drawing.Font("Microsoft Sans Serif", 8F, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.lblPassCount.Location = new System.Drawing.Point(474, 87);
            this.lblPassCount.Name = "lblPassCount";
            this.lblPassCount.Size = new System.Drawing.Size(87, 20);
            this.lblPassCount.TabIndex = 7;
            this.lblPassCount.Text = "Passes: 0";
            this.lblPassCount.Visible = false;
            // 
            // lblScoreGoal
            // 
            this.lblScoreGoal.AutoSize = true;
            this.lblScoreGoal.Font = new System.Drawing.Font("Microsoft Sans Serif", 10F, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.lblScoreGoal.Location = new System.Drawing.Point(13, 20);
            this.lblScoreGoal.Name = "lblScoreGoal";
            this.lblScoreGoal.Size = new System.Drawing.Size(70, 25);
            this.lblScoreGoal.TabIndex = 8;
            this.lblScoreGoal.Text = "Goal: ";
            // 
            // Game
            // 
            this.AutoScaleDimensions = new System.Drawing.SizeF(9F, 20F);
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
            this.ClientSize = new System.Drawing.Size(1166, 1452);
            this.Controls.Add(this.lblScoreGoal);
            this.Controls.Add(this.lblPassCount);
            this.Controls.Add(this.lblRotateCount);
            this.Controls.Add(this.lblMatchScore);
            this.Controls.Add(this.lblLeadBlockLocation);
            this.Controls.Add(this.lblMessage);
            this.Controls.Add(this.lblScore);
            this.Controls.Add(this.pnlShapeInventory);
            this.Controls.Add(this.pbGameBoard);
            this.KeyPreview = true;
            this.Name = "Game";
            this.StartPosition = System.Windows.Forms.FormStartPosition.CenterScreen;
            this.Text = "Blockudoku";
            this.WindowState = System.Windows.Forms.FormWindowState.Maximized;
            this.FormClosing += new System.Windows.Forms.FormClosingEventHandler(this.Game_FormClosing);
            this.Load += new System.EventHandler(this.Game_Load);
            this.KeyUp += new System.Windows.Forms.KeyEventHandler(this.Game_KeyUp);
            this.MouseClick += new System.Windows.Forms.MouseEventHandler(this.Game_MouseClick);
            this.MouseMove += new System.Windows.Forms.MouseEventHandler(this.Game_MouseMove);
            ((System.ComponentModel.ISupportInitialize)(this.pbGameBoard)).EndInit();
            this.ResumeLayout(false);
            this.PerformLayout();

        }

        #endregion

        private System.Windows.Forms.PictureBox pbGameBoard;
        private System.Windows.Forms.Panel pnlShapeInventory;
        private System.Windows.Forms.Label lblScore;
        private System.Windows.Forms.Label lblMessage;
        private System.Windows.Forms.Label lblLeadBlockLocation;
        private System.Windows.Forms.Label lblMatchScore;
        private System.Windows.Forms.Label lblRotateCount;
        private System.Windows.Forms.Label lblPassCount;
        private System.Windows.Forms.Label lblScoreGoal;
    }
}