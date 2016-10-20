namespace SKvisual
{
    partial class Form1
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
            this.button1 = new System.Windows.Forms.Button();
            this.button2 = new System.Windows.Forms.Button();
            this.AlgoDesc = new System.Windows.Forms.Label();
            this.cbLevel = new System.Windows.Forms.ComboBox();
            this.ClearP = new System.Windows.Forms.Button();
            this.UseUIPuzzle = new System.Windows.Forms.Button();
            this.AutoContinue = new System.Windows.Forms.CheckBox();
            this.Backtrace = new System.Windows.Forms.ListBox();
            this.StatsBox = new System.Windows.Forms.RichTextBox();
            this.SuspendLayout();
            // 
            // button1
            // 
            this.button1.Location = new System.Drawing.Point(140, 2);
            this.button1.Margin = new System.Windows.Forms.Padding(4, 4, 4, 4);
            this.button1.Name = "button1";
            this.button1.Size = new System.Drawing.Size(131, 28);
            this.button1.TabIndex = 0;
            this.button1.Text = "Generate puzzle";
            this.button1.UseVisualStyleBackColor = true;
            this.button1.Click += new System.EventHandler(this.button1_Click);
            // 
            // button2
            // 
            this.button2.BackColor = System.Drawing.Color.DodgerBlue;
            this.button2.ForeColor = System.Drawing.SystemColors.ControlText;
            this.button2.Location = new System.Drawing.Point(299, 2);
            this.button2.Margin = new System.Windows.Forms.Padding(4, 4, 4, 4);
            this.button2.Name = "button2";
            this.button2.Size = new System.Drawing.Size(312, 28);
            this.button2.TabIndex = 1;
            this.button2.Text = "Solve!";
            this.button2.UseVisualStyleBackColor = false;
            this.button2.Click += new System.EventHandler(this.button2_Click);
            // 
            // AlgoDesc
            // 
            this.AlgoDesc.AutoSize = true;
            this.AlgoDesc.Location = new System.Drawing.Point(195, 43);
            this.AlgoDesc.Margin = new System.Windows.Forms.Padding(4, 0, 4, 0);
            this.AlgoDesc.Name = "AlgoDesc";
            this.AlgoDesc.Size = new System.Drawing.Size(168, 17);
            this.AlgoDesc.TabIndex = 2;
            this.AlgoDesc.Text = "====================";
            // 
            // cbLevel
            // 
            this.cbLevel.FormattingEnabled = true;
            this.cbLevel.Items.AddRange(new object[] {
            "Easy (40)",
            "Medium (50)",
            "Hard (60)",
            "Impossiable (70)",
            "Puzzel-1 (hard)",
            "Puzzel-2 (hard)"});
            this.cbLevel.Location = new System.Drawing.Point(12, 2);
            this.cbLevel.Margin = new System.Windows.Forms.Padding(3, 2, 3, 2);
            this.cbLevel.Name = "cbLevel";
            this.cbLevel.Size = new System.Drawing.Size(121, 24);
            this.cbLevel.TabIndex = 3;
            // 
            // ClearP
            // 
            this.ClearP.Location = new System.Drawing.Point(12, 663);
            this.ClearP.Margin = new System.Windows.Forms.Padding(3, 2, 3, 2);
            this.ClearP.Name = "ClearP";
            this.ClearP.Size = new System.Drawing.Size(75, 23);
            this.ClearP.TabIndex = 4;
            this.ClearP.Text = "Clear puzzle";
            this.ClearP.UseVisualStyleBackColor = true;
            this.ClearP.Click += new System.EventHandler(this.ClearP_Click);
            // 
            // UseUIPuzzle
            // 
            this.UseUIPuzzle.Location = new System.Drawing.Point(93, 663);
            this.UseUIPuzzle.Margin = new System.Windows.Forms.Padding(3, 2, 3, 2);
            this.UseUIPuzzle.Name = "UseUIPuzzle";
            this.UseUIPuzzle.Size = new System.Drawing.Size(107, 23);
            this.UseUIPuzzle.TabIndex = 5;
            this.UseUIPuzzle.Text = "Use puzzle";
            this.UseUIPuzzle.UseVisualStyleBackColor = true;
            this.UseUIPuzzle.Click += new System.EventHandler(this.UseUIPuzzle_Click);
            // 
            // AutoContinue
            // 
            this.AutoContinue.AutoSize = true;
            this.AutoContinue.Location = new System.Drawing.Point(628, 10);
            this.AutoContinue.Margin = new System.Windows.Forms.Padding(3, 2, 3, 2);
            this.AutoContinue.Name = "AutoContinue";
            this.AutoContinue.Size = new System.Drawing.Size(18, 17);
            this.AutoContinue.TabIndex = 6;
            this.AutoContinue.UseVisualStyleBackColor = true;
            this.AutoContinue.CheckedChanged += new System.EventHandler(this.AutoContinue_CheckedChanged);
            // 
            // Backtrace
            // 
            this.Backtrace.BackColor = System.Drawing.SystemColors.Menu;
            this.Backtrace.FormattingEnabled = true;
            this.Backtrace.ItemHeight = 16;
            this.Backtrace.Location = new System.Drawing.Point(628, 72);
            this.Backtrace.Margin = new System.Windows.Forms.Padding(3, 2, 3, 2);
            this.Backtrace.Name = "Backtrace";
            this.Backtrace.Size = new System.Drawing.Size(210, 420);
            this.Backtrace.TabIndex = 7;
            // 
            // StatsBox
            // 
            this.StatsBox.Location = new System.Drawing.Point(24, 553);
            this.StatsBox.Name = "StatsBox";
            this.StatsBox.Size = new System.Drawing.Size(844, 54);
            this.StatsBox.TabIndex = 8;
            this.StatsBox.Text = "";
            // 
            // Form1
            // 
            this.AutoScaleDimensions = new System.Drawing.SizeF(8F, 16F);
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
            this.ClientSize = new System.Drawing.Size(889, 700);
            this.Controls.Add(this.StatsBox);
            this.Controls.Add(this.Backtrace);
            this.Controls.Add(this.AutoContinue);
            this.Controls.Add(this.UseUIPuzzle);
            this.Controls.Add(this.ClearP);
            this.Controls.Add(this.cbLevel);
            this.Controls.Add(this.AlgoDesc);
            this.Controls.Add(this.button2);
            this.Controls.Add(this.button1);
            this.Margin = new System.Windows.Forms.Padding(4, 4, 4, 4);
            this.Name = "Form1";
            this.Text = "Form1";
            this.Load += new System.EventHandler(this.Form1_Load_1);
            this.ResumeLayout(false);
            this.PerformLayout();

        }

        #endregion

        private System.Windows.Forms.Button button1;
        private System.Windows.Forms.Button button2;
        private System.Windows.Forms.Label AlgoDesc;
        private System.Windows.Forms.ComboBox cbLevel;
        private System.Windows.Forms.Button ClearP;
        private System.Windows.Forms.Button UseUIPuzzle;
        public System.Windows.Forms.CheckBox AutoContinue;
        private System.Windows.Forms.ListBox Backtrace;
        private System.Windows.Forms.RichTextBox StatsBox;
    }
}

