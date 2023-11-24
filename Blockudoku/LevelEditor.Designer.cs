namespace Blockudoku
{
    partial class LevelEditor
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
            System.ComponentModel.ComponentResourceManager resources = new System.ComponentModel.ComponentResourceManager(typeof(LevelEditor));
            this.pbGameBoard = new System.Windows.Forms.PictureBox();
            this.label1 = new System.Windows.Forms.Label();
            this.label2 = new System.Windows.Forms.Label();
            this.label3 = new System.Windows.Forms.Label();
            this.label4 = new System.Windows.Forms.Label();
            this.lblSelection = new System.Windows.Forms.Label();
            this.txtHealth = new System.Windows.Forms.TextBox();
            this.txtScoreGoal = new System.Windows.Forms.TextBox();
            this.label5 = new System.Windows.Forms.Label();
            this.txtMapName = new System.Windows.Forms.TextBox();
            this.btnSave = new System.Windows.Forms.Button();
            this.cmbMapList = new System.Windows.Forms.ComboBox();
            this.label6 = new System.Windows.Forms.Label();
            this.btnLoad = new System.Windows.Forms.Button();
            this.singleBlockShape1 = new Blockudoku.GameObjects.Shapes.ConcreteShapes.SingleBlockShape();
            this.btnCreateNew = new System.Windows.Forms.Button();
            ((System.ComponentModel.ISupportInitialize)(this.pbGameBoard)).BeginInit();
            ((System.ComponentModel.ISupportInitialize)(this.singleBlockShape1)).BeginInit();
            this.SuspendLayout();
            // 
            // pbGameBoard
            // 
            this.pbGameBoard.Image = global::Blockudoku.Properties.Resources.blockudoku_board1;
            this.pbGameBoard.InitialImage = global::Blockudoku.Properties.Resources.blockudoku_board1;
            this.pbGameBoard.Location = new System.Drawing.Point(366, 204);
            this.pbGameBoard.Name = "pbGameBoard";
            this.pbGameBoard.Size = new System.Drawing.Size(472, 472);
            this.pbGameBoard.SizeMode = System.Windows.Forms.PictureBoxSizeMode.AutoSize;
            this.pbGameBoard.TabIndex = 1;
            this.pbGameBoard.TabStop = false;
            this.pbGameBoard.Click += new System.EventHandler(this.PbGameBoard_Click);
            // 
            // label1
            // 
            this.label1.AutoSize = true;
            this.label1.Location = new System.Drawing.Point(366, 28);
            this.label1.Name = "label1";
            this.label1.Size = new System.Drawing.Size(335, 20);
            this.label1.TabIndex = 2;
            this.label1.Text = "Use the dropdown to assign health to a block. ";
            // 
            // label2
            // 
            this.label2.AutoSize = true;
            this.label2.Location = new System.Drawing.Point(366, 56);
            this.label2.Name = "label2";
            this.label2.Size = new System.Drawing.Size(431, 20);
            this.label2.TabIndex = 3;
            this.label2.Text = "Click on a block and move it to the space you wish to place it";
            // 
            // label3
            // 
            this.label3.AutoSize = true;
            this.label3.Location = new System.Drawing.Point(366, 76);
            this.label3.Name = "label3";
            this.label3.Size = new System.Drawing.Size(298, 20);
            this.label3.TabIndex = 4;
            this.label3.Text = "To remove your selected, press \"Escape\"";
            // 
            // label4
            // 
            this.label4.AutoSize = true;
            this.label4.Location = new System.Drawing.Point(370, 100);
            this.label4.Name = "label4";
            this.label4.Size = new System.Drawing.Size(739, 20);
            this.label4.TabIndex = 5;
            this.label4.Text = "To delete an existing block, clear your selection, click on a block on the board," +
    " and press the \"delete\" key";
            // 
            // lblSelection
            // 
            this.lblSelection.AutoSize = true;
            this.lblSelection.Location = new System.Drawing.Point(1186, 204);
            this.lblSelection.Name = "lblSelection";
            this.lblSelection.Size = new System.Drawing.Size(177, 20);
            this.lblSelection.TabIndex = 6;
            this.lblSelection.Text = "Assign health to a block";
            // 
            // txtHealth
            // 
            this.txtHealth.Location = new System.Drawing.Point(1190, 228);
            this.txtHealth.Name = "txtHealth";
            this.txtHealth.Size = new System.Drawing.Size(146, 26);
            this.txtHealth.TabIndex = 9;
            this.txtHealth.TextChanged += new System.EventHandler(this.TxtHealth_TextChanged);
            // 
            // txtScoreGoal
            // 
            this.txtScoreGoal.Location = new System.Drawing.Point(1190, 379);
            this.txtScoreGoal.Name = "txtScoreGoal";
            this.txtScoreGoal.Size = new System.Drawing.Size(146, 26);
            this.txtScoreGoal.TabIndex = 10;
            this.txtScoreGoal.TextChanged += new System.EventHandler(this.TxtScoreGoal_TextChanged);
            // 
            // label5
            // 
            this.label5.AutoSize = true;
            this.label5.Location = new System.Drawing.Point(1190, 353);
            this.label5.Name = "label5";
            this.label5.Size = new System.Drawing.Size(93, 20);
            this.label5.TabIndex = 11;
            this.label5.Text = "Score Goal:";
            // 
            // txtMapName
            // 
            this.txtMapName.Location = new System.Drawing.Point(55, 228);
            this.txtMapName.Name = "txtMapName";
            this.txtMapName.Size = new System.Drawing.Size(176, 26);
            this.txtMapName.TabIndex = 12;
            // 
            // btnSave
            // 
            this.btnSave.Location = new System.Drawing.Point(55, 273);
            this.btnSave.Name = "btnSave";
            this.btnSave.Size = new System.Drawing.Size(176, 38);
            this.btnSave.TabIndex = 13;
            this.btnSave.Text = "Save Map";
            this.btnSave.UseVisualStyleBackColor = true;
            this.btnSave.Click += new System.EventHandler(this.BtnSave_Click);
            // 
            // cmbMapList
            // 
            this.cmbMapList.DropDownStyle = System.Windows.Forms.ComboBoxStyle.DropDownList;
            this.cmbMapList.FormattingEnabled = true;
            this.cmbMapList.Location = new System.Drawing.Point(55, 436);
            this.cmbMapList.Name = "cmbMapList";
            this.cmbMapList.Size = new System.Drawing.Size(192, 28);
            this.cmbMapList.TabIndex = 14;
            // 
            // label6
            // 
            this.label6.AutoSize = true;
            this.label6.Location = new System.Drawing.Point(55, 384);
            this.label6.Name = "label6";
            this.label6.Size = new System.Drawing.Size(163, 20);
            this.label6.TabIndex = 15;
            this.label6.Text = "Load an existing map:";
            // 
            // btnLoad
            // 
            this.btnLoad.Location = new System.Drawing.Point(55, 485);
            this.btnLoad.Name = "btnLoad";
            this.btnLoad.Size = new System.Drawing.Size(192, 34);
            this.btnLoad.TabIndex = 16;
            this.btnLoad.Text = "Load";
            this.btnLoad.UseVisualStyleBackColor = true;
            this.btnLoad.Click += new System.EventHandler(this.BtnLoad_Click);
            // 
            // singleBlockShape1
            // 
            this.singleBlockShape1.BackColor = System.Drawing.Color.Transparent;
            this.singleBlockShape1.CanRotate = false;
            this.singleBlockShape1.Image = ((System.Drawing.Image)(resources.GetObject("singleBlockShape1.Image")));
            this.singleBlockShape1.Location = new System.Drawing.Point(1190, 273);
            this.singleBlockShape1.Name = "singleBlockShape1";
            this.singleBlockShape1.Size = new System.Drawing.Size(48, 48);
            this.singleBlockShape1.SizeMode = System.Windows.Forms.PictureBoxSizeMode.AutoSize;
            this.singleBlockShape1.TabIndex = 8;
            this.singleBlockShape1.TabStop = false;
            this.singleBlockShape1.Click += new System.EventHandler(this.SingleBlockShape1_Click);
            // 
            // btnCreateNew
            // 
            this.btnCreateNew.Location = new System.Drawing.Point(55, 535);
            this.btnCreateNew.Name = "btnCreateNew";
            this.btnCreateNew.Size = new System.Drawing.Size(192, 34);
            this.btnCreateNew.TabIndex = 17;
            this.btnCreateNew.Text = "Create New";
            this.btnCreateNew.UseVisualStyleBackColor = true;
            this.btnCreateNew.Click += new System.EventHandler(this.BtnCreateNew_Click);
            // 
            // LevelEditor
            // 
            this.AutoScaleDimensions = new System.Drawing.SizeF(9F, 20F);
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
            this.ClientSize = new System.Drawing.Size(1375, 879);
            this.Controls.Add(this.btnCreateNew);
            this.Controls.Add(this.btnLoad);
            this.Controls.Add(this.label6);
            this.Controls.Add(this.cmbMapList);
            this.Controls.Add(this.btnSave);
            this.Controls.Add(this.txtMapName);
            this.Controls.Add(this.label5);
            this.Controls.Add(this.txtScoreGoal);
            this.Controls.Add(this.txtHealth);
            this.Controls.Add(this.singleBlockShape1);
            this.Controls.Add(this.lblSelection);
            this.Controls.Add(this.label4);
            this.Controls.Add(this.label3);
            this.Controls.Add(this.label2);
            this.Controls.Add(this.label1);
            this.Controls.Add(this.pbGameBoard);
            this.KeyPreview = true;
            this.Name = "LevelEditor";
            this.Text = "LevelEditor";
            this.WindowState = System.Windows.Forms.FormWindowState.Maximized;
            this.Load += new System.EventHandler(this.LevelEditor_Load);
            this.Click += new System.EventHandler(this.LevelEditor_Click);
            this.KeyUp += new System.Windows.Forms.KeyEventHandler(this.LevelEditor_KeyUp);
            ((System.ComponentModel.ISupportInitialize)(this.pbGameBoard)).EndInit();
            ((System.ComponentModel.ISupportInitialize)(this.singleBlockShape1)).EndInit();
            this.ResumeLayout(false);
            this.PerformLayout();

        }

        #endregion

        private System.Windows.Forms.PictureBox pbGameBoard;
        private System.Windows.Forms.Label label1;
        private System.Windows.Forms.Label label2;
        private System.Windows.Forms.Label label3;
        private System.Windows.Forms.Label label4;
        private System.Windows.Forms.Label lblSelection;
        private GameObjects.Shapes.ConcreteShapes.SingleBlockShape singleBlockShape1;
        private System.Windows.Forms.TextBox txtHealth;
        private System.Windows.Forms.TextBox txtScoreGoal;
        private System.Windows.Forms.Label label5;
        private System.Windows.Forms.TextBox txtMapName;
        private System.Windows.Forms.Button btnSave;
        private System.Windows.Forms.ComboBox cmbMapList;
        private System.Windows.Forms.Label label6;
        private System.Windows.Forms.Button btnLoad;
        private System.Windows.Forms.Button btnCreateNew;
    }
}