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
            this.SuspendLayout();
            // 
            // button1
            // 
            this.button1.Location = new System.Drawing.Point(105, 2);
            this.button1.Name = "button1";
            this.button1.Size = new System.Drawing.Size(98, 23);
            this.button1.TabIndex = 0;
            this.button1.Text = "Generate puzzle";
            this.button1.UseVisualStyleBackColor = true;
            this.button1.Click += new System.EventHandler(this.button1_Click);
            // 
            // button2
            // 
            this.button2.BackColor = System.Drawing.Color.DodgerBlue;
            this.button2.ForeColor = System.Drawing.SystemColors.ControlText;
            this.button2.Location = new System.Drawing.Point(224, 2);
            this.button2.Name = "button2";
            this.button2.Size = new System.Drawing.Size(234, 23);
            this.button2.TabIndex = 1;
            this.button2.Text = "Solve!";
            this.button2.UseVisualStyleBackColor = false;
            this.button2.Click += new System.EventHandler(this.button2_Click);
            // 
            // AlgoDesc
            // 
            this.AlgoDesc.AutoSize = true;
            this.AlgoDesc.Location = new System.Drawing.Point(146, 35);
            this.AlgoDesc.Name = "AlgoDesc";
            this.AlgoDesc.Size = new System.Drawing.Size(127, 13);
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
            this.cbLevel.Location = new System.Drawing.Point(9, 2);
            this.cbLevel.Margin = new System.Windows.Forms.Padding(2, 2, 2, 2);
            this.cbLevel.Name = "cbLevel";
            this.cbLevel.Size = new System.Drawing.Size(92, 21);
            this.cbLevel.TabIndex = 3;
            // 
            // ClearP
            // 
            this.ClearP.Location = new System.Drawing.Point(9, 539);
            this.ClearP.Margin = new System.Windows.Forms.Padding(2, 2, 2, 2);
            this.ClearP.Name = "ClearP";
            this.ClearP.Size = new System.Drawing.Size(56, 19);
            this.ClearP.TabIndex = 4;
            this.ClearP.Text = "Clear puzzle";
            this.ClearP.UseVisualStyleBackColor = true;
            this.ClearP.Click += new System.EventHandler(this.ClearP_Click);
            // 
            // UseUIPuzzle
            // 
            this.UseUIPuzzle.Location = new System.Drawing.Point(70, 539);
            this.UseUIPuzzle.Margin = new System.Windows.Forms.Padding(2, 2, 2, 2);
            this.UseUIPuzzle.Name = "UseUIPuzzle";
            this.UseUIPuzzle.Size = new System.Drawing.Size(80, 19);
            this.UseUIPuzzle.TabIndex = 5;
            this.UseUIPuzzle.Text = "Use puzzle";
            this.UseUIPuzzle.UseVisualStyleBackColor = true;
            this.UseUIPuzzle.Click += new System.EventHandler(this.UseUIPuzzle_Click);
            // 
            // AutoContinue
            // 
            this.AutoContinue.AutoSize = true;
            this.AutoContinue.Location = new System.Drawing.Point(483, 5);
            this.AutoContinue.Margin = new System.Windows.Forms.Padding(2, 2, 2, 2);
            this.AutoContinue.Name = "AutoContinue";
            this.AutoContinue.Size = new System.Drawing.Size(15, 14);
            this.AutoContinue.TabIndex = 6;
            this.AutoContinue.UseVisualStyleBackColor = true;
            this.AutoContinue.CheckedChanged += new System.EventHandler(this.AutoContinue_CheckedChanged);
            // 
            // Backtrace
            // 
            this.Backtrace.BackColor = System.Drawing.SystemColors.Menu;
            this.Backtrace.FormattingEnabled = true;
            this.Backtrace.Location = new System.Drawing.Point(542, 35);
            this.Backtrace.Margin = new System.Windows.Forms.Padding(2, 2, 2, 2);
            this.Backtrace.Name = "Backtrace";
            this.Backtrace.Size = new System.Drawing.Size(114, 342);
            this.Backtrace.TabIndex = 7;
            // 
            // Form1
            // 
            this.AutoScaleDimensions = new System.Drawing.SizeF(6F, 13F);
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
            this.ClientSize = new System.Drawing.Size(667, 569);
            this.Controls.Add(this.Backtrace);
            this.Controls.Add(this.AutoContinue);
            this.Controls.Add(this.UseUIPuzzle);
            this.Controls.Add(this.ClearP);
            this.Controls.Add(this.cbLevel);
            this.Controls.Add(this.AlgoDesc);
            this.Controls.Add(this.button2);
            this.Controls.Add(this.button1);
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
    }
}

