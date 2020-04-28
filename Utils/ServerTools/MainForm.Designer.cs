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
            this.components = new System.ComponentModel.Container();
            System.ComponentModel.ComponentResourceManager resources = new System.ComponentModel.ComponentResourceManager(typeof(frmMain));
            this.tabDemoReader = new System.Windows.Forms.TabPage();
            this.txtDemoReader = new System.Windows.Forms.RichTextBox();
            this.tabLogReader = new System.Windows.Forms.TabPage();
            this.txtLogReader = new System.Windows.Forms.RichTextBox();
            this.tabControl = new System.Windows.Forms.TabControl();
            this.tabTools = new System.Windows.Forms.TabPage();
            this.btnUpdate = new System.Windows.Forms.Button();
            this.txtConsole = new System.Windows.Forms.RichTextBox();
            this.cmbMap = new System.Windows.Forms.ComboBox();
            this.btnStop = new System.Windows.Forms.Button();
            this.btnStart = new System.Windows.Forms.Button();
            this.tabSettings = new System.Windows.Forms.TabPage();
            this.label3 = new System.Windows.Forms.Label();
            this.panel6 = new System.Windows.Forms.Panel();
            this.label14 = new System.Windows.Forms.Label();
            this.dateChangeDay = new System.Windows.Forms.DateTimePicker();
            this.lblDown = new System.Windows.Forms.Label();
            this.lblUp = new System.Windows.Forms.Label();
            this.lblClearAll = new System.Windows.Forms.Label();
            this.lblRemoveOne = new System.Windows.Forms.Label();
            this.lblAddAll = new System.Windows.Forms.Label();
            this.lblResetMapPool = new System.Windows.Forms.Label();
            this.lblAddOne = new System.Windows.Forms.Label();
            this.listAll = new System.Windows.Forms.ListBox();
            this.numDays = new System.Windows.Forms.NumericUpDown();
            this.label11 = new System.Windows.Forms.Label();
            this.listPool = new System.Windows.Forms.ListBox();
            this.statusStrip1 = new System.Windows.Forms.StatusStrip();
            this.lblStatus = new System.Windows.Forms.ToolStripStatusLabel();
            this.btnSave = new System.Windows.Forms.Button();
            this.label6 = new System.Windows.Forms.Label();
            this.panel5 = new System.Windows.Forms.Panel();
            this.txtUpdate = new System.Windows.Forms.TextBox();
            this.label9 = new System.Windows.Forms.Label();
            this.txtStop = new System.Windows.Forms.TextBox();
            this.label8 = new System.Windows.Forms.Label();
            this.txtStart = new System.Windows.Forms.TextBox();
            this.label7 = new System.Windows.Forms.Label();
            this.panel2 = new System.Windows.Forms.Panel();
            this.panel4 = new System.Windows.Forms.Panel();
            this.label16 = new System.Windows.Forms.Label();
            this.label15 = new System.Windows.Forms.Label();
            this.label10 = new System.Windows.Forms.Label();
            this.label5 = new System.Windows.Forms.Label();
            this.panel3 = new System.Windows.Forms.Panel();
            this.label4 = new System.Windows.Forms.Label();
            this.label2 = new System.Windows.Forms.Label();
            this.panel1 = new System.Windows.Forms.Panel();
            this.timePickerEvening = new System.Windows.Forms.DateTimePicker();
            this.timePickerLunch = new System.Windows.Forms.DateTimePicker();
            this.label13 = new System.Windows.Forms.Label();
            this.label12 = new System.Windows.Forms.Label();
            this.label1 = new System.Windows.Forms.Label();
            this.lblUpdate = new System.Windows.Forms.Label();
            this.chkAutoRestart = new System.Windows.Forms.CheckBox();
            this.chkAutoUpdate = new System.Windows.Forms.CheckBox();
            this.timerStatus = new System.Windows.Forms.Timer(this.components);
            this.timerRestart = new System.Windows.Forms.Timer(this.components);
            this.timerChangeMap = new System.Windows.Forms.Timer(this.components);
            this.tabDemoReader.SuspendLayout();
            this.tabLogReader.SuspendLayout();
            this.tabControl.SuspendLayout();
            this.tabTools.SuspendLayout();
            this.tabSettings.SuspendLayout();
            this.panel6.SuspendLayout();
            ((System.ComponentModel.ISupportInitialize)(this.numDays)).BeginInit();
            this.statusStrip1.SuspendLayout();
            this.panel5.SuspendLayout();
            this.panel2.SuspendLayout();
            this.panel4.SuspendLayout();
            this.panel3.SuspendLayout();
            this.panel1.SuspendLayout();
            this.SuspendLayout();
            // 
            // tabDemoReader
            // 
            this.tabDemoReader.Controls.Add(this.txtDemoReader);
            this.tabDemoReader.Location = new System.Drawing.Point(4, 22);
            this.tabDemoReader.Name = "tabDemoReader";
            this.tabDemoReader.Padding = new System.Windows.Forms.Padding(3);
            this.tabDemoReader.Size = new System.Drawing.Size(881, 562);
            this.tabDemoReader.TabIndex = 2;
            this.tabDemoReader.Text = "Demo Reader";
            this.tabDemoReader.UseVisualStyleBackColor = true;
            // 
            // txtDemoReader
            // 
            this.txtDemoReader.BackColor = System.Drawing.Color.Black;
            this.txtDemoReader.BorderStyle = System.Windows.Forms.BorderStyle.FixedSingle;
            this.txtDemoReader.Dock = System.Windows.Forms.DockStyle.Fill;
            this.txtDemoReader.Font = new System.Drawing.Font("Consolas", 9.75F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.txtDemoReader.ForeColor = System.Drawing.Color.White;
            this.txtDemoReader.Location = new System.Drawing.Point(3, 3);
            this.txtDemoReader.Name = "txtDemoReader";
            this.txtDemoReader.ReadOnly = true;
            this.txtDemoReader.Size = new System.Drawing.Size(875, 556);
            this.txtDemoReader.TabIndex = 9;
            this.txtDemoReader.Text = "";
            // 
            // tabLogReader
            // 
            this.tabLogReader.Controls.Add(this.txtLogReader);
            this.tabLogReader.Location = new System.Drawing.Point(4, 22);
            this.tabLogReader.Name = "tabLogReader";
            this.tabLogReader.Padding = new System.Windows.Forms.Padding(3);
            this.tabLogReader.Size = new System.Drawing.Size(881, 562);
            this.tabLogReader.TabIndex = 1;
            this.tabLogReader.Text = "Log Reader";
            this.tabLogReader.UseVisualStyleBackColor = true;
            // 
            // txtLogReader
            // 
            this.txtLogReader.BackColor = System.Drawing.Color.Black;
            this.txtLogReader.BorderStyle = System.Windows.Forms.BorderStyle.FixedSingle;
            this.txtLogReader.Dock = System.Windows.Forms.DockStyle.Fill;
            this.txtLogReader.Font = new System.Drawing.Font("Consolas", 9.75F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.txtLogReader.ForeColor = System.Drawing.Color.White;
            this.txtLogReader.Location = new System.Drawing.Point(3, 3);
            this.txtLogReader.Name = "txtLogReader";
            this.txtLogReader.ReadOnly = true;
            this.txtLogReader.Size = new System.Drawing.Size(875, 556);
            this.txtLogReader.TabIndex = 9;
            this.txtLogReader.Text = "";
            // 
            // tabControl
            // 
            this.tabControl.Controls.Add(this.tabTools);
            this.tabControl.Controls.Add(this.tabLogReader);
            this.tabControl.Controls.Add(this.tabDemoReader);
            this.tabControl.Controls.Add(this.tabSettings);
            this.tabControl.Dock = System.Windows.Forms.DockStyle.Fill;
            this.tabControl.Location = new System.Drawing.Point(0, 0);
            this.tabControl.Name = "tabControl";
            this.tabControl.SelectedIndex = 0;
            this.tabControl.Size = new System.Drawing.Size(889, 588);
            this.tabControl.TabIndex = 0;
            // 
            // tabTools
            // 
            this.tabTools.Controls.Add(this.btnUpdate);
            this.tabTools.Controls.Add(this.txtConsole);
            this.tabTools.Controls.Add(this.cmbMap);
            this.tabTools.Controls.Add(this.btnStop);
            this.tabTools.Controls.Add(this.btnStart);
            this.tabTools.Location = new System.Drawing.Point(4, 22);
            this.tabTools.Name = "tabTools";
            this.tabTools.Size = new System.Drawing.Size(881, 562);
            this.tabTools.TabIndex = 3;
            this.tabTools.Text = "Tools";
            this.tabTools.UseVisualStyleBackColor = true;
            // 
            // btnUpdate
            // 
            this.btnUpdate.Location = new System.Drawing.Point(5, 108);
            this.btnUpdate.Name = "btnUpdate";
            this.btnUpdate.Size = new System.Drawing.Size(175, 28);
            this.btnUpdate.TabIndex = 9;
            this.btnUpdate.Text = "Update";
            this.btnUpdate.UseVisualStyleBackColor = true;
            this.btnUpdate.Click += new System.EventHandler(this.btnUpdate_Click);
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
            this.txtConsole.Size = new System.Drawing.Size(695, 549);
            this.txtConsole.TabIndex = 8;
            this.txtConsole.Text = "";
            this.txtConsole.TextChanged += new System.EventHandler(this.txtConsole_TextChanged);
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
            // tabSettings
            // 
            this.tabSettings.Controls.Add(this.label3);
            this.tabSettings.Controls.Add(this.panel6);
            this.tabSettings.Controls.Add(this.statusStrip1);
            this.tabSettings.Controls.Add(this.btnSave);
            this.tabSettings.Controls.Add(this.label6);
            this.tabSettings.Controls.Add(this.panel5);
            this.tabSettings.Controls.Add(this.panel2);
            this.tabSettings.Controls.Add(this.label2);
            this.tabSettings.Controls.Add(this.panel1);
            this.tabSettings.Controls.Add(this.label1);
            this.tabSettings.Controls.Add(this.lblUpdate);
            this.tabSettings.Controls.Add(this.chkAutoRestart);
            this.tabSettings.Controls.Add(this.chkAutoUpdate);
            this.tabSettings.Location = new System.Drawing.Point(4, 22);
            this.tabSettings.Name = "tabSettings";
            this.tabSettings.Padding = new System.Windows.Forms.Padding(3);
            this.tabSettings.Size = new System.Drawing.Size(881, 562);
            this.tabSettings.TabIndex = 4;
            this.tabSettings.Text = "Settings";
            this.tabSettings.UseVisualStyleBackColor = true;
            // 
            // label3
            // 
            this.label3.AutoSize = true;
            this.label3.Location = new System.Drawing.Point(175, 13);
            this.label3.Name = "label3";
            this.label3.Size = new System.Drawing.Size(51, 13);
            this.label3.TabIndex = 32;
            this.label3.Text = "Map pool";
            // 
            // panel6
            // 
            this.panel6.BorderStyle = System.Windows.Forms.BorderStyle.FixedSingle;
            this.panel6.Controls.Add(this.label14);
            this.panel6.Controls.Add(this.dateChangeDay);
            this.panel6.Controls.Add(this.lblDown);
            this.panel6.Controls.Add(this.lblUp);
            this.panel6.Controls.Add(this.lblClearAll);
            this.panel6.Controls.Add(this.lblRemoveOne);
            this.panel6.Controls.Add(this.lblAddAll);
            this.panel6.Controls.Add(this.lblResetMapPool);
            this.panel6.Controls.Add(this.lblAddOne);
            this.panel6.Controls.Add(this.listAll);
            this.panel6.Controls.Add(this.numDays);
            this.panel6.Controls.Add(this.label11);
            this.panel6.Controls.Add(this.listPool);
            this.panel6.Location = new System.Drawing.Point(171, 19);
            this.panel6.Name = "panel6";
            this.panel6.Size = new System.Drawing.Size(349, 165);
            this.panel6.TabIndex = 31;
            // 
            // label14
            // 
            this.label14.AutoSize = true;
            this.label14.Location = new System.Drawing.Point(180, 126);
            this.label14.Name = "label14";
            this.label14.Size = new System.Drawing.Size(64, 13);
            this.label14.TabIndex = 41;
            this.label14.Text = "Change day";
            // 
            // dateChangeDay
            // 
            this.dateChangeDay.Cursor = System.Windows.Forms.Cursors.Default;
            this.dateChangeDay.CustomFormat = "dd.MM.yyyy";
            this.dateChangeDay.Format = System.Windows.Forms.DateTimePickerFormat.Custom;
            this.dateChangeDay.Location = new System.Drawing.Point(250, 124);
            this.dateChangeDay.Name = "dateChangeDay";
            this.dateChangeDay.Size = new System.Drawing.Size(82, 20);
            this.dateChangeDay.TabIndex = 40;
            // 
            // lblDown
            // 
            this.lblDown.AutoSize = true;
            this.lblDown.Cursor = System.Windows.Forms.Cursors.Hand;
            this.lblDown.FlatStyle = System.Windows.Forms.FlatStyle.Flat;
            this.lblDown.Font = new System.Drawing.Font("Microsoft Sans Serif", 8.25F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.lblDown.ForeColor = System.Drawing.Color.Black;
            this.lblDown.Location = new System.Drawing.Point(333, 23);
            this.lblDown.Name = "lblDown";
            this.lblDown.Size = new System.Drawing.Size(13, 13);
            this.lblDown.TabIndex = 39;
            this.lblDown.Text = "˅";
            this.lblDown.Click += new System.EventHandler(this.lblDown_Click);
            // 
            // lblUp
            // 
            this.lblUp.AutoSize = true;
            this.lblUp.Cursor = System.Windows.Forms.Cursors.Hand;
            this.lblUp.FlatStyle = System.Windows.Forms.FlatStyle.Flat;
            this.lblUp.Font = new System.Drawing.Font("Microsoft Sans Serif", 8.25F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.lblUp.ForeColor = System.Drawing.Color.Black;
            this.lblUp.Location = new System.Drawing.Point(333, 10);
            this.lblUp.Name = "lblUp";
            this.lblUp.Size = new System.Drawing.Size(13, 13);
            this.lblUp.TabIndex = 38;
            this.lblUp.Text = "˄";
            this.lblUp.Click += new System.EventHandler(this.lblUp_Click);
            // 
            // lblClearAll
            // 
            this.lblClearAll.AutoSize = true;
            this.lblClearAll.Cursor = System.Windows.Forms.Cursors.Hand;
            this.lblClearAll.FlatStyle = System.Windows.Forms.FlatStyle.Flat;
            this.lblClearAll.Font = new System.Drawing.Font("Microsoft Sans Serif", 8.25F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.lblClearAll.ForeColor = System.Drawing.Color.Black;
            this.lblClearAll.Location = new System.Drawing.Point(158, 73);
            this.lblClearAll.Name = "lblClearAll";
            this.lblClearAll.Size = new System.Drawing.Size(19, 13);
            this.lblClearAll.TabIndex = 37;
            this.lblClearAll.Text = "<<";
            this.lblClearAll.Click += new System.EventHandler(this.lblClearAll_Click);
            // 
            // lblRemoveOne
            // 
            this.lblRemoveOne.AutoSize = true;
            this.lblRemoveOne.Cursor = System.Windows.Forms.Cursors.Hand;
            this.lblRemoveOne.FlatStyle = System.Windows.Forms.FlatStyle.Flat;
            this.lblRemoveOne.Font = new System.Drawing.Font("Microsoft Sans Serif", 8.25F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.lblRemoveOne.ForeColor = System.Drawing.Color.Black;
            this.lblRemoveOne.Location = new System.Drawing.Point(158, 60);
            this.lblRemoveOne.Name = "lblRemoveOne";
            this.lblRemoveOne.Size = new System.Drawing.Size(13, 13);
            this.lblRemoveOne.TabIndex = 36;
            this.lblRemoveOne.Text = "<";
            this.lblRemoveOne.Click += new System.EventHandler(this.lblRemoveOne_Click);
            // 
            // lblAddAll
            // 
            this.lblAddAll.AutoSize = true;
            this.lblAddAll.Cursor = System.Windows.Forms.Cursors.Hand;
            this.lblAddAll.FlatStyle = System.Windows.Forms.FlatStyle.Flat;
            this.lblAddAll.Font = new System.Drawing.Font("Microsoft Sans Serif", 8.25F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.lblAddAll.ForeColor = System.Drawing.Color.Black;
            this.lblAddAll.Location = new System.Drawing.Point(158, 41);
            this.lblAddAll.Name = "lblAddAll";
            this.lblAddAll.Size = new System.Drawing.Size(19, 13);
            this.lblAddAll.TabIndex = 35;
            this.lblAddAll.Text = ">>";
            this.lblAddAll.Click += new System.EventHandler(this.lblAddAll_Click);
            // 
            // lblResetMapPool
            // 
            this.lblResetMapPool.AutoSize = true;
            this.lblResetMapPool.Cursor = System.Windows.Forms.Cursors.Hand;
            this.lblResetMapPool.FlatStyle = System.Windows.Forms.FlatStyle.Flat;
            this.lblResetMapPool.Font = new System.Drawing.Font("Microsoft Sans Serif", 8.25F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.lblResetMapPool.ForeColor = System.Drawing.Color.Black;
            this.lblResetMapPool.Location = new System.Drawing.Point(301, 147);
            this.lblResetMapPool.Name = "lblResetMapPool";
            this.lblResetMapPool.Size = new System.Drawing.Size(35, 13);
            this.lblResetMapPool.TabIndex = 19;
            this.lblResetMapPool.Text = "Reset";
            this.lblResetMapPool.Click += new System.EventHandler(this.lblResetMapPool_Click);
            this.lblResetMapPool.MouseLeave += new System.EventHandler(this.OnLeave);
            this.lblResetMapPool.MouseHover += new System.EventHandler(this.OnHover);
            // 
            // lblAddOne
            // 
            this.lblAddOne.AutoSize = true;
            this.lblAddOne.Cursor = System.Windows.Forms.Cursors.Hand;
            this.lblAddOne.FlatStyle = System.Windows.Forms.FlatStyle.Flat;
            this.lblAddOne.Font = new System.Drawing.Font("Microsoft Sans Serif", 8.25F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.lblAddOne.ForeColor = System.Drawing.Color.Black;
            this.lblAddOne.Location = new System.Drawing.Point(158, 28);
            this.lblAddOne.Name = "lblAddOne";
            this.lblAddOne.Size = new System.Drawing.Size(13, 13);
            this.lblAddOne.TabIndex = 34;
            this.lblAddOne.Text = ">";
            this.lblAddOne.Click += new System.EventHandler(this.lblAddOne_Click);
            // 
            // listAll
            // 
            this.listAll.FormattingEnabled = true;
            this.listAll.Location = new System.Drawing.Point(3, 10);
            this.listAll.Name = "listAll";
            this.listAll.Size = new System.Drawing.Size(149, 108);
            this.listAll.TabIndex = 33;
            // 
            // numDays
            // 
            this.numDays.BorderStyle = System.Windows.Forms.BorderStyle.FixedSingle;
            this.numDays.Location = new System.Drawing.Point(110, 124);
            this.numDays.Name = "numDays";
            this.numDays.Size = new System.Drawing.Size(42, 20);
            this.numDays.TabIndex = 27;
            // 
            // label11
            // 
            this.label11.AutoSize = true;
            this.label11.Location = new System.Drawing.Point(0, 126);
            this.label11.Name = "label11";
            this.label11.Size = new System.Drawing.Size(104, 13);
            this.label11.TabIndex = 26;
            this.label11.Text = "Playing days for map";
            // 
            // listPool
            // 
            this.listPool.FormattingEnabled = true;
            this.listPool.Location = new System.Drawing.Point(183, 10);
            this.listPool.Name = "listPool";
            this.listPool.Size = new System.Drawing.Size(149, 108);
            this.listPool.TabIndex = 11;
            // 
            // statusStrip1
            // 
            this.statusStrip1.BackColor = System.Drawing.Color.White;
            this.statusStrip1.Items.AddRange(new System.Windows.Forms.ToolStripItem[] {
            this.lblStatus});
            this.statusStrip1.Location = new System.Drawing.Point(3, 537);
            this.statusStrip1.Name = "statusStrip1";
            this.statusStrip1.Size = new System.Drawing.Size(875, 22);
            this.statusStrip1.TabIndex = 28;
            this.statusStrip1.Text = "statusStrip1";
            // 
            // lblStatus
            // 
            this.lblStatus.Name = "lblStatus";
            this.lblStatus.Size = new System.Drawing.Size(39, 17);
            this.lblStatus.Text = "Ready";
            // 
            // btnSave
            // 
            this.btnSave.Location = new System.Drawing.Point(445, 511);
            this.btnSave.Name = "btnSave";
            this.btnSave.Size = new System.Drawing.Size(75, 23);
            this.btnSave.TabIndex = 23;
            this.btnSave.Text = "Save";
            this.btnSave.UseVisualStyleBackColor = true;
            this.btnSave.Click += new System.EventHandler(this.btnSave_Click);
            // 
            // label6
            // 
            this.label6.AutoSize = true;
            this.label6.Location = new System.Drawing.Point(14, 193);
            this.label6.Name = "label6";
            this.label6.Size = new System.Drawing.Size(92, 13);
            this.label6.TabIndex = 22;
            this.label6.Text = "Server commands";
            // 
            // panel5
            // 
            this.panel5.BorderStyle = System.Windows.Forms.BorderStyle.FixedSingle;
            this.panel5.Controls.Add(this.txtUpdate);
            this.panel5.Controls.Add(this.label9);
            this.panel5.Controls.Add(this.txtStop);
            this.panel5.Controls.Add(this.label8);
            this.panel5.Controls.Add(this.txtStart);
            this.panel5.Controls.Add(this.label7);
            this.panel5.Location = new System.Drawing.Point(11, 201);
            this.panel5.Name = "panel5";
            this.panel5.Size = new System.Drawing.Size(509, 308);
            this.panel5.TabIndex = 21;
            // 
            // txtUpdate
            // 
            this.txtUpdate.Location = new System.Drawing.Point(3, 211);
            this.txtUpdate.Multiline = true;
            this.txtUpdate.Name = "txtUpdate";
            this.txtUpdate.Size = new System.Drawing.Size(501, 92);
            this.txtUpdate.TabIndex = 28;
            // 
            // label9
            // 
            this.label9.AutoSize = true;
            this.label9.Location = new System.Drawing.Point(3, 195);
            this.label9.Name = "label9";
            this.label9.Size = new System.Drawing.Size(42, 13);
            this.label9.TabIndex = 27;
            this.label9.Text = "Update";
            // 
            // txtStop
            // 
            this.txtStop.Location = new System.Drawing.Point(3, 139);
            this.txtStop.Multiline = true;
            this.txtStop.Name = "txtStop";
            this.txtStop.Size = new System.Drawing.Size(501, 48);
            this.txtStop.TabIndex = 26;
            // 
            // label8
            // 
            this.label8.AutoSize = true;
            this.label8.Location = new System.Drawing.Point(3, 123);
            this.label8.Name = "label8";
            this.label8.Size = new System.Drawing.Size(29, 13);
            this.label8.TabIndex = 25;
            this.label8.Text = "Stop";
            // 
            // txtStart
            // 
            this.txtStart.Location = new System.Drawing.Point(3, 25);
            this.txtStart.Multiline = true;
            this.txtStart.Name = "txtStart";
            this.txtStart.Size = new System.Drawing.Size(501, 90);
            this.txtStart.TabIndex = 24;
            // 
            // label7
            // 
            this.label7.AutoSize = true;
            this.label7.Location = new System.Drawing.Point(3, 9);
            this.label7.Name = "label7";
            this.label7.Size = new System.Drawing.Size(29, 13);
            this.label7.TabIndex = 23;
            this.label7.Text = "Start";
            // 
            // panel2
            // 
            this.panel2.Anchor = ((System.Windows.Forms.AnchorStyles)(((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Bottom) 
            | System.Windows.Forms.AnchorStyles.Right)));
            this.panel2.BorderStyle = System.Windows.Forms.BorderStyle.FixedSingle;
            this.panel2.Controls.Add(this.panel4);
            this.panel2.Controls.Add(this.panel3);
            this.panel2.Location = new System.Drawing.Point(564, 3);
            this.panel2.Name = "panel2";
            this.panel2.Size = new System.Drawing.Size(314, 531);
            this.panel2.TabIndex = 20;
            // 
            // panel4
            // 
            this.panel4.Controls.Add(this.label16);
            this.panel4.Controls.Add(this.label15);
            this.panel4.Controls.Add(this.label10);
            this.panel4.Controls.Add(this.label5);
            this.panel4.Dock = System.Windows.Forms.DockStyle.Fill;
            this.panel4.Location = new System.Drawing.Point(0, 23);
            this.panel4.Name = "panel4";
            this.panel4.Size = new System.Drawing.Size(312, 506);
            this.panel4.TabIndex = 3;
            // 
            // label16
            // 
            this.label16.ForeColor = System.Drawing.Color.Red;
            this.label16.Location = new System.Drawing.Point(3, 153);
            this.label16.Name = "label16";
            this.label16.Size = new System.Drawing.Size(305, 34);
            this.label16.TabIndex = 5;
            this.label16.Text = "Не забудь сохранить изменения. Возможно когда-нибудь сделаю уведомление";
            // 
            // label15
            // 
            this.label15.Location = new System.Drawing.Point(4, 91);
            this.label15.Name = "label15";
            this.label15.Size = new System.Drawing.Size(305, 62);
            this.label15.TabIndex = 4;
            this.label15.Text = "Change day - дата следующей смены карты, выставляется автоматически, но можно пом" +
    "енять вручную. На случай праздников и т.д. Выходные учитываются автоматически. ";
            // 
            // label10
            // 
            this.label10.Location = new System.Drawing.Point(3, 51);
            this.label10.Name = "label10";
            this.label10.Size = new System.Drawing.Size(305, 34);
            this.label10.TabIndex = 3;
            this.label10.Text = "Серверные комманды лучше не менять, если не уверен в том, что делаешь.";
            // 
            // label5
            // 
            this.label5.Location = new System.Drawing.Point(3, 0);
            this.label5.Name = "label5";
            this.label5.Size = new System.Drawing.Size(305, 48);
            this.label5.TabIndex = 2;
            this.label5.Text = "Reset - сброс маппула. Означает, что список карт будет сброшен и отчет сыгранных " +
    "карт начнется с текущего числа заново.";
            // 
            // panel3
            // 
            this.panel3.Controls.Add(this.label4);
            this.panel3.Dock = System.Windows.Forms.DockStyle.Top;
            this.panel3.Location = new System.Drawing.Point(0, 0);
            this.panel3.Name = "panel3";
            this.panel3.Size = new System.Drawing.Size(312, 23);
            this.panel3.TabIndex = 2;
            // 
            // label4
            // 
            this.label4.Dock = System.Windows.Forms.DockStyle.Fill;
            this.label4.Location = new System.Drawing.Point(0, 0);
            this.label4.Name = "label4";
            this.label4.Size = new System.Drawing.Size(312, 23);
            this.label4.TabIndex = 10;
            this.label4.Text = "Справка";
            this.label4.TextAlign = System.Drawing.ContentAlignment.MiddleCenter;
            // 
            // label2
            // 
            this.label2.AutoSize = true;
            this.label2.Location = new System.Drawing.Point(14, 48);
            this.label2.Name = "label2";
            this.label2.Size = new System.Drawing.Size(63, 13);
            this.label2.TabIndex = 10;
            this.label2.Text = "Restart time";
            // 
            // panel1
            // 
            this.panel1.BackColor = System.Drawing.Color.White;
            this.panel1.BorderStyle = System.Windows.Forms.BorderStyle.FixedSingle;
            this.panel1.Controls.Add(this.timePickerEvening);
            this.panel1.Controls.Add(this.timePickerLunch);
            this.panel1.Controls.Add(this.label13);
            this.panel1.Controls.Add(this.label12);
            this.panel1.Location = new System.Drawing.Point(11, 54);
            this.panel1.Name = "panel1";
            this.panel1.Size = new System.Drawing.Size(149, 130);
            this.panel1.TabIndex = 16;
            // 
            // timePickerEvening
            // 
            this.timePickerEvening.CustomFormat = "HH:mm";
            this.timePickerEvening.Format = System.Windows.Forms.DateTimePickerFormat.Custom;
            this.timePickerEvening.Location = new System.Drawing.Point(81, 32);
            this.timePickerEvening.Name = "timePickerEvening";
            this.timePickerEvening.ShowUpDown = true;
            this.timePickerEvening.Size = new System.Drawing.Size(63, 20);
            this.timePickerEvening.TabIndex = 33;
            // 
            // timePickerLunch
            // 
            this.timePickerLunch.CustomFormat = "HH:mm";
            this.timePickerLunch.Format = System.Windows.Forms.DateTimePickerFormat.Custom;
            this.timePickerLunch.Location = new System.Drawing.Point(81, 6);
            this.timePickerLunch.Name = "timePickerLunch";
            this.timePickerLunch.ShowUpDown = true;
            this.timePickerLunch.Size = new System.Drawing.Size(63, 20);
            this.timePickerLunch.TabIndex = 1;
            // 
            // label13
            // 
            this.label13.AutoSize = true;
            this.label13.Location = new System.Drawing.Point(0, 38);
            this.label13.Name = "label13";
            this.label13.Size = new System.Drawing.Size(75, 13);
            this.label13.TabIndex = 32;
            this.label13.Text = "Evening game";
            // 
            // label12
            // 
            this.label12.AutoSize = true;
            this.label12.Location = new System.Drawing.Point(0, 12);
            this.label12.Name = "label12";
            this.label12.Size = new System.Drawing.Size(66, 13);
            this.label12.TabIndex = 31;
            this.label12.Text = "Lunch game";
            // 
            // label1
            // 
            this.label1.AutoSize = true;
            this.label1.Location = new System.Drawing.Point(8, 33);
            this.label1.Name = "label1";
            this.label1.Size = new System.Drawing.Size(127, 13);
            this.label1.TabIndex = 3;
            this.label1.Text = "Auto restart CS:Go server";
            // 
            // lblUpdate
            // 
            this.lblUpdate.AutoSize = true;
            this.lblUpdate.Location = new System.Drawing.Point(8, 13);
            this.lblUpdate.Name = "lblUpdate";
            this.lblUpdate.Size = new System.Drawing.Size(131, 13);
            this.lblUpdate.TabIndex = 2;
            this.lblUpdate.Text = "Auto update CS:Go server";
            // 
            // chkAutoRestart
            // 
            this.chkAutoRestart.AutoSize = true;
            this.chkAutoRestart.Location = new System.Drawing.Point(145, 33);
            this.chkAutoRestart.Name = "chkAutoRestart";
            this.chkAutoRestart.RightToLeft = System.Windows.Forms.RightToLeft.Yes;
            this.chkAutoRestart.Size = new System.Drawing.Size(15, 14);
            this.chkAutoRestart.TabIndex = 1;
            this.chkAutoRestart.UseVisualStyleBackColor = true;
            // 
            // chkAutoUpdate
            // 
            this.chkAutoUpdate.AutoSize = true;
            this.chkAutoUpdate.Location = new System.Drawing.Point(145, 13);
            this.chkAutoUpdate.Name = "chkAutoUpdate";
            this.chkAutoUpdate.RightToLeft = System.Windows.Forms.RightToLeft.Yes;
            this.chkAutoUpdate.Size = new System.Drawing.Size(15, 14);
            this.chkAutoUpdate.TabIndex = 0;
            this.chkAutoUpdate.UseVisualStyleBackColor = true;
            // 
            // timerStatus
            // 
            this.timerStatus.Interval = 1000;
            this.timerStatus.Tick += new System.EventHandler(this.timerStatus_Tick);
            // 
            // timerRestart
            // 
            this.timerRestart.Interval = 20000;
            this.timerRestart.Tick += new System.EventHandler(this.timerRestart_Tick);
            // 
            // timerChangeMap
            // 
            this.timerChangeMap.Interval = 20000;
            this.timerChangeMap.Tick += new System.EventHandler(this.timerChangeMap_Tick);
            // 
            // frmMain
            // 
            this.AutoScaleDimensions = new System.Drawing.SizeF(6F, 13F);
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
            this.ClientSize = new System.Drawing.Size(889, 588);
            this.Controls.Add(this.tabControl);
            this.Icon = ((System.Drawing.Icon)(resources.GetObject("$this.Icon")));
            this.Name = "frmMain";
            this.Text = "CS: GO Server Tools ";
            this.Load += new System.EventHandler(this.frmMain_Load);
            this.tabDemoReader.ResumeLayout(false);
            this.tabLogReader.ResumeLayout(false);
            this.tabControl.ResumeLayout(false);
            this.tabTools.ResumeLayout(false);
            this.tabSettings.ResumeLayout(false);
            this.tabSettings.PerformLayout();
            this.panel6.ResumeLayout(false);
            this.panel6.PerformLayout();
            ((System.ComponentModel.ISupportInitialize)(this.numDays)).EndInit();
            this.statusStrip1.ResumeLayout(false);
            this.statusStrip1.PerformLayout();
            this.panel5.ResumeLayout(false);
            this.panel5.PerformLayout();
            this.panel2.ResumeLayout(false);
            this.panel4.ResumeLayout(false);
            this.panel3.ResumeLayout(false);
            this.panel1.ResumeLayout(false);
            this.panel1.PerformLayout();
            this.ResumeLayout(false);

        }

        #endregion

        private System.Windows.Forms.TabPage tabDemoReader;
        private System.Windows.Forms.TabPage tabLogReader;
        private System.Windows.Forms.TabControl tabControl;
        private System.Windows.Forms.TabPage tabTools;
        private System.Windows.Forms.Button btnStop;
        private System.Windows.Forms.Button btnStart;
        private System.Windows.Forms.ComboBox cmbMap;
        private System.Windows.Forms.RichTextBox txtConsole;
        private System.Windows.Forms.RichTextBox txtDemoReader;
        private System.Windows.Forms.RichTextBox txtLogReader;
        private System.Windows.Forms.Button btnUpdate;
        private System.Windows.Forms.TabPage tabSettings;
        private System.Windows.Forms.Label label1;
        private System.Windows.Forms.Label lblUpdate;
        private System.Windows.Forms.CheckBox chkAutoRestart;
        private System.Windows.Forms.CheckBox chkAutoUpdate;
        private System.Windows.Forms.ListBox listPool;
        private System.Windows.Forms.Panel panel2;
        private System.Windows.Forms.Label lblResetMapPool;
        private System.Windows.Forms.Panel panel4;
        private System.Windows.Forms.Label label5;
        private System.Windows.Forms.Panel panel3;
        private System.Windows.Forms.Label label4;
        private System.Windows.Forms.Panel panel5;
        private System.Windows.Forms.Button btnSave;
        private System.Windows.Forms.Label label6;
        private System.Windows.Forms.TextBox txtUpdate;
        private System.Windows.Forms.Label label9;
        private System.Windows.Forms.TextBox txtStop;
        private System.Windows.Forms.Label label8;
        private System.Windows.Forms.TextBox txtStart;
        private System.Windows.Forms.Label label7;
        private System.Windows.Forms.Label label10;
        private System.Windows.Forms.NumericUpDown numDays;
        private System.Windows.Forms.Label label11;
        private System.Windows.Forms.StatusStrip statusStrip1;
        private System.Windows.Forms.ToolStripStatusLabel lblStatus;
        private System.Windows.Forms.Timer timerStatus;
        private System.Windows.Forms.Label label2;
        private System.Windows.Forms.Panel panel1;
        private System.Windows.Forms.DateTimePicker timePickerEvening;
        private System.Windows.Forms.DateTimePicker timePickerLunch;
        private System.Windows.Forms.Label label13;
        private System.Windows.Forms.Label label12;
        private System.Windows.Forms.Label label3;
        private System.Windows.Forms.Panel panel6;
        private System.Windows.Forms.Label label14;
        private System.Windows.Forms.DateTimePicker dateChangeDay;
        private System.Windows.Forms.Label lblDown;
        private System.Windows.Forms.Label lblUp;
        private System.Windows.Forms.Label lblClearAll;
        private System.Windows.Forms.Label lblRemoveOne;
        private System.Windows.Forms.Label lblAddAll;
        private System.Windows.Forms.Label lblAddOne;
        private System.Windows.Forms.ListBox listAll;
        private System.Windows.Forms.Label label15;
        private System.Windows.Forms.Label label16;
        private System.Windows.Forms.Timer timerRestart;
        private System.Windows.Forms.Timer timerChangeMap;
    }
}

