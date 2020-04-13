namespace ServerTools
{
    partial class frmMain
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
            System.ComponentModel.ComponentResourceManager resources = new System.ComponentModel.ComponentResourceManager(typeof(frmMain));
            this.tabDemoReader = new System.Windows.Forms.TabPage();
            this.tabLogReader = new System.Windows.Forms.TabPage();
            this.tabControl = new System.Windows.Forms.TabControl();
            this.tabTools = new System.Windows.Forms.TabPage();
            this.btnRestart = new System.Windows.Forms.Button();
            this.btnStop = new System.Windows.Forms.Button();
            this.btnStart = new System.Windows.Forms.Button();
            this.cmbMap = new System.Windows.Forms.ComboBox();
            this.txtLogReader = new System.Windows.Forms.TextBox();
            this.txtDemoReader = new System.Windows.Forms.TextBox();
            this.txtConsole = new System.Windows.Forms.RichTextBox();
            this.tabDemoReader.SuspendLayout();
            this.tabLogReader.SuspendLayout();
            this.tabControl.SuspendLayout();
            this.tabTools.SuspendLayout();
            this.SuspendLayout();
            // 
            // tabDemoReader
            // 
            this.tabDemoReader.Controls.Add(this.txtDemoReader);
            this.tabDemoReader.Location = new System.Drawing.Point(4, 22);
            this.tabDemoReader.Name = "tabDemoReader";
            this.tabDemoReader.Padding = new System.Windows.Forms.Padding(3);
            this.tabDemoReader.Size = new System.Drawing.Size(792, 424);
            this.tabDemoReader.TabIndex = 2;
            this.tabDemoReader.Text = "Demo Reader";
            this.tabDemoReader.UseVisualStyleBackColor = true;
            // 
            // tabLogReader
            // 
            this.tabLogReader.Controls.Add(this.txtLogReader);
            this.tabLogReader.Location = new System.Drawing.Point(4, 22);
            this.tabLogReader.Name = "tabLogReader";
            this.tabLogReader.Padding = new System.Windows.Forms.Padding(3);
            this.tabLogReader.Size = new System.Drawing.Size(792, 424);
            this.tabLogReader.TabIndex = 1;
            this.tabLogReader.Text = "Log Reader";
            this.tabLogReader.UseVisualStyleBackColor = true;
            // 
            // tabControl
            // 
            this.tabControl.Controls.Add(this.tabTools);
            this.tabControl.Controls.Add(this.tabLogReader);
            this.tabControl.Controls.Add(this.tabDemoReader);
            this.tabControl.Dock = System.Windows.Forms.DockStyle.Fill;
            this.tabControl.Location = new System.Drawing.Point(0, 0);
            this.tabControl.Name = "tabControl";
            this.tabControl.SelectedIndex = 0;
            this.tabControl.Size = new System.Drawing.Size(800, 450);
            this.tabControl.TabIndex = 0;
            // 
            // tabTools
            // 
            this.tabTools.Controls.Add(this.txtConsole);
            this.tabTools.Controls.Add(this.cmbMap);
            this.tabTools.Controls.Add(this.btnRestart);
            this.tabTools.Controls.Add(this.btnStop);
            this.tabTools.Controls.Add(this.btnStart);
            this.tabTools.Location = new System.Drawing.Point(4, 22);
            this.tabTools.Name = "tabTools";
            this.tabTools.Size = new System.Drawing.Size(792, 424);
            this.tabTools.TabIndex = 3;
            this.tabTools.Text = "Tools";
            this.tabTools.UseVisualStyleBackColor = true;
            // 
            // btnRestart
            // 
            this.btnRestart.Location = new System.Drawing.Point(5, 108);
            this.btnRestart.Name = "btnRestart";
            this.btnRestart.Size = new System.Drawing.Size(175, 28);
            this.btnRestart.TabIndex = 5;
            this.btnRestart.Text = "Restart";
            this.btnRestart.UseVisualStyleBackColor = true;
            this.btnRestart.Click += new System.EventHandler(this.btnRestart_Click);
            // 
            // btnStop
            // 
            this.btnStop.Location = new System.Drawing.Point(5, 74);
            this.btnStop.Name = "btnStop";
            this.btnStop.Size = new System.Drawing.Size(175, 28);
            this.btnStop.TabIndex = 4;
            this.btnStop.Text = "Stop";
            this.btnStop.UseVisualStyleBackColor = true;
            this.btnStop.Click += new System.EventHandler(this.btnStop_Click);
            // 
            // btnStart
            // 
            this.btnStart.Location = new System.Drawing.Point(5, 40);
            this.btnStart.Name = "btnStart";
            this.btnStart.Size = new System.Drawing.Size(175, 28);
            this.btnStart.TabIndex = 3;
            this.btnStart.Text = "Start";
            this.btnStart.UseVisualStyleBackColor = true;
            this.btnStart.Click += new System.EventHandler(this.btnStart_Click);
            // 
            // cmbMap
            // 
            this.cmbMap.FormattingEnabled = true;
            this.cmbMap.Location = new System.Drawing.Point(5, 13);
            this.cmbMap.Name = "cmbMap";
            this.cmbMap.Size = new System.Drawing.Size(175, 21);
            this.cmbMap.TabIndex = 7;
            this.cmbMap.Text = "Map";
            // 
            // txtLogReader
            // 
            this.txtLogReader.BackColor = System.Drawing.Color.Black;
            this.txtLogReader.BorderStyle = System.Windows.Forms.BorderStyle.FixedSingle;
            this.txtLogReader.Dock = System.Windows.Forms.DockStyle.Fill;
            this.txtLogReader.ForeColor = System.Drawing.Color.White;
            this.txtLogReader.Location = new System.Drawing.Point(3, 3);
            this.txtLogReader.Multiline = true;
            this.txtLogReader.Name = "txtLogReader";
            this.txtLogReader.Size = new System.Drawing.Size(786, 418);
            this.txtLogReader.TabIndex = 7;
            // 
            // txtDemoReader
            // 
            this.txtDemoReader.BackColor = System.Drawing.Color.Black;
            this.txtDemoReader.BorderStyle = System.Windows.Forms.BorderStyle.FixedSingle;
            this.txtDemoReader.Dock = System.Windows.Forms.DockStyle.Fill;
            this.txtDemoReader.ForeColor = System.Drawing.Color.White;
            this.txtDemoReader.Location = new System.Drawing.Point(3, 3);
            this.txtDemoReader.Multiline = true;
            this.txtDemoReader.Name = "txtDemoReader";
            this.txtDemoReader.Size = new System.Drawing.Size(786, 418);
            this.txtDemoReader.TabIndex = 8;
            // 
            // txtConsole
            // 
            this.txtConsole.Anchor = ((System.Windows.Forms.AnchorStyles)((((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Bottom) 
            | System.Windows.Forms.AnchorStyles.Left) 
            | System.Windows.Forms.AnchorStyles.Right)));
            this.txtConsole.BackColor = System.Drawing.Color.Black;
            this.txtConsole.BorderStyle = System.Windows.Forms.BorderStyle.FixedSingle;
            this.txtConsole.Font = new System.Drawing.Font("Consolas", 9.75F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.txtConsole.ForeColor = System.Drawing.Color.White;
            this.txtConsole.Location = new System.Drawing.Point(186, 13);
            this.txtConsole.Name = "txtConsole";
            this.txtConsole.ReadOnly = true;
            this.txtConsole.Size = new System.Drawing.Size(603, 408);
            this.txtConsole.TabIndex = 8;
            this.txtConsole.Text = "";
            // 
            // frmMain
            // 
            this.AutoScaleDimensions = new System.Drawing.SizeF(6F, 13F);
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
            this.ClientSize = new System.Drawing.Size(800, 450);
            this.Controls.Add(this.tabControl);
            this.Icon = ((System.Drawing.Icon)(resources.GetObject("$this.Icon")));
            this.Name = "frmMain";
            this.Text = "CS: GO Server Tools ";
            this.Load += new System.EventHandler(this.frmMain_Load);
            this.tabDemoReader.ResumeLayout(false);
            this.tabDemoReader.PerformLayout();
            this.tabLogReader.ResumeLayout(false);
            this.tabLogReader.PerformLayout();
            this.tabControl.ResumeLayout(false);
            this.tabTools.ResumeLayout(false);
            this.ResumeLayout(false);

        }

        #endregion

        private System.Windows.Forms.TabPage tabDemoReader;
        private System.Windows.Forms.TabPage tabLogReader;
        private System.Windows.Forms.TabControl tabControl;
        private System.Windows.Forms.TabPage tabTools;
        private System.Windows.Forms.Button btnRestart;
        private System.Windows.Forms.Button btnStop;
        private System.Windows.Forms.Button btnStart;
        private System.Windows.Forms.ComboBox cmbMap;
        private System.Windows.Forms.TextBox txtLogReader;
        private System.Windows.Forms.TextBox txtDemoReader;
        private System.Windows.Forms.RichTextBox txtConsole;
    }
}

