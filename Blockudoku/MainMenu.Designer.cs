namespace Blockudoku
{
    partial class MainMenu
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
            this.label1 = new System.Windows.Forms.Label();
            this.label2 = new System.Windows.Forms.Label();
            this.cmbGameMode = new System.Windows.Forms.ComboBox();
            this.lblDescription = new System.Windows.Forms.Label();
            this.btnPlay = new System.Windows.Forms.Button();
            this.chkPlayCustom = new System.Windows.Forms.CheckBox();
            this.cmbMapNames = new System.Windows.Forms.ComboBox();
            this.pnlCustomLevelSelect = new System.Windows.Forms.Panel();
            this.pnlContinueNew = new System.Windows.Forms.Panel();
            this.rdbContinue = new System.Windows.Forms.RadioButton();
            this.rdbNew = new System.Windows.Forms.RadioButton();
            this.pnlCustomLevelSelect.SuspendLayout();
            this.pnlContinueNew.SuspendLayout();
            this.SuspendLayout();
            // 
            // label1
            // 
            this.label1.Anchor = System.Windows.Forms.AnchorStyles.Top;
            this.label1.AutoSize = true;
            this.label1.Font = new System.Drawing.Font("Microsoft Sans Serif", 20F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.label1.Location = new System.Drawing.Point(477, -4);
            this.label1.Name = "label1";
            this.label1.Size = new System.Drawing.Size(216, 46);
            this.label1.TabIndex = 0;
            this.label1.Text = "Main Menu";
            this.label1.TextAlign = System.Drawing.ContentAlignment.TopCenter;
            // 
            // label2
            // 
            this.label2.AutoSize = true;
            this.label2.Location = new System.Drawing.Point(304, 160);
            this.label2.Name = "label2";
            this.label2.Size = new System.Drawing.Size(160, 20);
            this.label2.TabIndex = 1;
            this.label2.Text = "Choose Game Mode:";
            // 
            // cmbGameMode
            // 
            this.cmbGameMode.DropDownStyle = System.Windows.Forms.ComboBoxStyle.DropDownList;
            this.cmbGameMode.FormattingEnabled = true;
            this.cmbGameMode.Items.AddRange(new object[] {
            "Standard",
            "Indi Mode",
            "Level Editor"});
            this.cmbGameMode.Location = new System.Drawing.Point(471, 160);
            this.cmbGameMode.Name = "cmbGameMode";
            this.cmbGameMode.Size = new System.Drawing.Size(222, 28);
            this.cmbGameMode.TabIndex = 2;
            this.cmbGameMode.SelectedIndexChanged += new System.EventHandler(this.CmbGameMode_SelectedIndexChanged);
            // 
            // lblDescription
            // 
            this.lblDescription.AutoSize = true;
            this.lblDescription.Location = new System.Drawing.Point(308, 199);
            this.lblDescription.Name = "lblDescription";
            this.lblDescription.Size = new System.Drawing.Size(0, 20);
            this.lblDescription.TabIndex = 3;
            // 
            // btnPlay
            // 
            this.btnPlay.Font = new System.Drawing.Font("Microsoft Sans Serif", 10F, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.btnPlay.Location = new System.Drawing.Point(471, 731);
            this.btnPlay.Name = "btnPlay";
            this.btnPlay.Size = new System.Drawing.Size(222, 59);
            this.btnPlay.TabIndex = 4;
            this.btnPlay.Text = "Play";
            this.btnPlay.UseVisualStyleBackColor = true;
            this.btnPlay.Click += new System.EventHandler(this.BtnPlay_Click);
            // 
            // chkPlayCustom
            // 
            this.chkPlayCustom.AutoSize = true;
            this.chkPlayCustom.Location = new System.Drawing.Point(3, 3);
            this.chkPlayCustom.Name = "chkPlayCustom";
            this.chkPlayCustom.Size = new System.Drawing.Size(164, 24);
            this.chkPlayCustom.TabIndex = 6;
            this.chkPlayCustom.Text = "Play Custom Level";
            this.chkPlayCustom.UseVisualStyleBackColor = true;
            this.chkPlayCustom.CheckedChanged += new System.EventHandler(this.ChkPlayCustom_CheckedChanged);
            // 
            // cmbMapNames
            // 
            this.cmbMapNames.DropDownStyle = System.Windows.Forms.ComboBoxStyle.DropDownList;
            this.cmbMapNames.FormattingEnabled = true;
            this.cmbMapNames.Location = new System.Drawing.Point(3, 33);
            this.cmbMapNames.Name = "cmbMapNames";
            this.cmbMapNames.Size = new System.Drawing.Size(222, 28);
            this.cmbMapNames.TabIndex = 7;
            this.cmbMapNames.SelectedIndexChanged += new System.EventHandler(this.CmbMapNames_SelectedIndexChanged);
            // 
            // pnlCustomLevelSelect
            // 
            this.pnlCustomLevelSelect.Controls.Add(this.chkPlayCustom);
            this.pnlCustomLevelSelect.Controls.Add(this.cmbMapNames);
            this.pnlCustomLevelSelect.Location = new System.Drawing.Point(471, 267);
            this.pnlCustomLevelSelect.Name = "pnlCustomLevelSelect";
            this.pnlCustomLevelSelect.Size = new System.Drawing.Size(250, 100);
            this.pnlCustomLevelSelect.TabIndex = 8;
            // 
            // pnlContinueNew
            // 
            this.pnlContinueNew.Controls.Add(this.rdbNew);
            this.pnlContinueNew.Controls.Add(this.rdbContinue);
            this.pnlContinueNew.Location = new System.Drawing.Point(245, 731);
            this.pnlContinueNew.Name = "pnlContinueNew";
            this.pnlContinueNew.Size = new System.Drawing.Size(194, 131);
            this.pnlContinueNew.TabIndex = 9;
            // 
            // rdbContinue
            // 
            this.rdbContinue.AutoSize = true;
            this.rdbContinue.Location = new System.Drawing.Point(4, 4);
            this.rdbContinue.Name = "rdbContinue";
            this.rdbContinue.Size = new System.Drawing.Size(146, 24);
            this.rdbContinue.TabIndex = 0;
            this.rdbContinue.Text = "Continue Game";
            this.rdbContinue.UseVisualStyleBackColor = true;
            // 
            // rdbNew
            // 
            this.rdbNew.AutoSize = true;
            this.rdbNew.Checked = true;
            this.rdbNew.Location = new System.Drawing.Point(4, 34);
            this.rdbNew.Name = "rdbNew";
            this.rdbNew.Size = new System.Drawing.Size(113, 24);
            this.rdbNew.TabIndex = 1;
            this.rdbNew.TabStop = true;
            this.rdbNew.Text = "New Game";
            this.rdbNew.UseVisualStyleBackColor = true;
            // 
            // MainMenu
            // 
            this.AutoScaleDimensions = new System.Drawing.SizeF(9F, 20F);
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
            this.ClientSize = new System.Drawing.Size(1149, 864);
            this.Controls.Add(this.pnlContinueNew);
            this.Controls.Add(this.pnlCustomLevelSelect);
            this.Controls.Add(this.btnPlay);
            this.Controls.Add(this.lblDescription);
            this.Controls.Add(this.cmbGameMode);
            this.Controls.Add(this.label2);
            this.Controls.Add(this.label1);
            this.Name = "MainMenu";
            this.Text = "Blockudoku";
            this.Load += new System.EventHandler(this.Form1_Load);
            this.pnlCustomLevelSelect.ResumeLayout(false);
            this.pnlCustomLevelSelect.PerformLayout();
            this.pnlContinueNew.ResumeLayout(false);
            this.pnlContinueNew.PerformLayout();
            this.ResumeLayout(false);
            this.PerformLayout();

        }

        #endregion

        private System.Windows.Forms.Label label1;
        private System.Windows.Forms.Label label2;
        private System.Windows.Forms.ComboBox cmbGameMode;
        private System.Windows.Forms.Label lblDescription;
        private System.Windows.Forms.Button btnPlay;
        private System.Windows.Forms.CheckBox chkPlayCustom;
        private System.Windows.Forms.ComboBox cmbMapNames;
        private System.Windows.Forms.Panel pnlCustomLevelSelect;
        private System.Windows.Forms.Panel pnlContinueNew;
        private System.Windows.Forms.RadioButton rdbNew;
        private System.Windows.Forms.RadioButton rdbContinue;
    }
}

