namespace ModelManager
{
    partial class Form1
    {
        /// <summary>
        ///  Required designer variable.
        /// </summary>
        private System.ComponentModel.IContainer components = null;

        /// <summary>
        ///  Clean up any resources being used.
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
        ///  Required method for Designer support - do not modify
        ///  the contents of this method with the code editor.
        /// </summary>
        private void InitializeComponent()
        {
            components = new System.ComponentModel.Container();
            lvModelle = new ListView();
            columnHeader1 = new ColumnHeader();
            columnHeader2 = new ColumnHeader();
            columnHeader3 = new ColumnHeader();
            columnHeader4 = new ColumnHeader();
            columnHeader5 = new ColumnHeader();
            columnHeader6 = new ColumnHeader();
            columnHeader7 = new ColumnHeader();
            columnHeader8 = new ColumnHeader();
            columnHeader9 = new ColumnHeader();
            tbCommand = new TextBox();
            btStart = new Button();
            btSendCommand = new Button();
            tbDebug = new TextBox();
            contextMenuStrip1 = new ContextMenuStrip(components);
            alsBasicTestScriptAbspeichernToolStripMenuItem = new ToolStripMenuItem();
            pasteClippboardToolStripMenuItem = new ToolStripMenuItem();
            btKillSwitch = new Button();
            btClearDebug = new Button();
            tbStatus = new TextBox();
            btStartBasicScript = new Button();
            btSendenNeu = new Button();
            btFormatListe = new Button();
            cbBaseModel = new ComboBox();
            label1 = new Label();
            status_label = new Label();
            btnGenerateModelInfo = new Button();
            btHtmlReportAnzeigen = new Button();
            cbDevice = new ComboBox();
            label2 = new Label();
            nudSeed = new NumericUpDown();
            nudNumSteps = new NumericUpDown();
            nudGuidanceScale = new NumericUpDown();
            cbVoice = new ComboBox();
            nudSpeed = new NumericUpDown();
            nudPitch = new NumericUpDown();
            cbPrecision = new ComboBox();
            contextMenuStrip1.SuspendLayout();
            ((System.ComponentModel.ISupportInitialize)nudSeed).BeginInit();
            ((System.ComponentModel.ISupportInitialize)nudNumSteps).BeginInit();
            ((System.ComponentModel.ISupportInitialize)nudGuidanceScale).BeginInit();
            ((System.ComponentModel.ISupportInitialize)nudSpeed).BeginInit();
            ((System.ComponentModel.ISupportInitialize)nudPitch).BeginInit();
            SuspendLayout();
            // 
            // lvModelle
            // 
            lvModelle.Columns.AddRange(new ColumnHeader[] { columnHeader1, columnHeader2, columnHeader3, columnHeader4, columnHeader5, columnHeader6, columnHeader7, columnHeader8, columnHeader9 });
            lvModelle.Font = new Font("Sitka Small", 9.75F, FontStyle.Regular, GraphicsUnit.Point, 0);
            lvModelle.ForeColor = SystemColors.Highlight;
            lvModelle.FullRowSelect = true;
            lvModelle.GridLines = true;
            lvModelle.LabelEdit = true;
            lvModelle.Location = new Point(0, -1);
            lvModelle.Name = "lvModelle";
            lvModelle.Size = new Size(1819, 483);
            lvModelle.TabIndex = 0;
            lvModelle.UseCompatibleStateImageBehavior = false;
            lvModelle.View = View.Details;
            lvModelle.ColumnClick += lvModelle_ColumnClick;
            lvModelle.DoubleClick += lvModelle_DoubleClick;
            // 
            // columnHeader1
            // 
            columnHeader1.Text = "Name:";
            columnHeader1.Width = 250;
            // 
            // columnHeader2
            // 
            columnHeader2.Text = "Größe:";
            columnHeader2.Width = 100;
            // 
            // columnHeader3
            // 
            columnHeader3.Text = "Pfad:";
            // 
            // columnHeader4
            // 
            columnHeader4.Text = "Kommentare:";
            columnHeader4.Width = 400;
            // 
            // columnHeader5
            // 
            columnHeader5.Text = "Last Dura:";
            columnHeader5.Width = 250;
            // 
            // columnHeader6
            // 
            columnHeader6.Text = "Letzter Debug log:";
            columnHeader6.Width = 250;
            // 
            // tbCommand
            // 
            tbCommand.AcceptsReturn = true;
            tbCommand.AcceptsTab = true;
            tbCommand.Location = new Point(81, 489);
            tbCommand.Multiline = true;
            tbCommand.Name = "tbCommand";
            tbCommand.Size = new Size(806, 23);
            tbCommand.TabIndex = 1;
            tbCommand.Text = "Bitte generiere mir einen Comichaften Grünen Oger mit Überbiss, und einem Ofenkäse Tatto auf dem rechten Arm\r\n\r\n";
            tbCommand.TextChanged += tbCommand_TextChanged;
            // 
            // btStart
            // 
            btStart.Location = new Point(0, 488);
            btStart.Name = "btStart";
            btStart.Size = new Size(75, 23);
            btStart.TabIndex = 2;
            btStart.Text = "do it!";
            btStart.UseVisualStyleBackColor = true;
            btStart.Click += btStart_Click;
            // 
            // btSendCommand
            // 
            btSendCommand.Location = new Point(1287, 488);
            btSendCommand.Name = "btSendCommand";
            btSendCommand.Size = new Size(75, 23);
            btSendCommand.TabIndex = 3;
            btSendCommand.Text = "senden";
            btSendCommand.UseVisualStyleBackColor = true;
            btSendCommand.Click += btSendCommand_Click;
            // 
            // tbDebug
            // 
            tbDebug.ContextMenuStrip = contextMenuStrip1;
            tbDebug.Location = new Point(12, 517);
            tbDebug.Multiline = true;
            tbDebug.Name = "tbDebug";
            tbDebug.ScrollBars = ScrollBars.Vertical;
            tbDebug.Size = new Size(768, 400);
            tbDebug.TabIndex = 4;
            tbDebug.TextChanged += tbDebug_TextChanged;
            // 
            // contextMenuStrip1
            // 
            contextMenuStrip1.Items.AddRange(new ToolStripItem[] { alsBasicTestScriptAbspeichernToolStripMenuItem, pasteClippboardToolStripMenuItem });
            contextMenuStrip1.Name = "contextMenuStrip1";
            contextMenuStrip1.Size = new Size(244, 48);
            // 
            // alsBasicTestScriptAbspeichernToolStripMenuItem
            // 
            alsBasicTestScriptAbspeichernToolStripMenuItem.Name = "alsBasicTestScriptAbspeichernToolStripMenuItem";
            alsBasicTestScriptAbspeichernToolStripMenuItem.Size = new Size(243, 22);
            alsBasicTestScriptAbspeichernToolStripMenuItem.Text = "Als Basic Test Script abspeichern";
            alsBasicTestScriptAbspeichernToolStripMenuItem.Click += alsBasicTestScriptAbspeichernToolStripMenuItem_Click;
            // 
            // pasteClippboardToolStripMenuItem
            // 
            pasteClippboardToolStripMenuItem.Name = "pasteClippboardToolStripMenuItem";
            pasteClippboardToolStripMenuItem.Size = new Size(243, 22);
            pasteClippboardToolStripMenuItem.Text = "paste clippboard";
            pasteClippboardToolStripMenuItem.Click += pasteClippboardToolStripMenuItem_Click;
            // 
            // btKillSwitch
            // 
            btKillSwitch.Location = new Point(991, 489);
            btKillSwitch.Name = "btKillSwitch";
            btKillSwitch.Size = new Size(75, 23);
            btKillSwitch.TabIndex = 5;
            btKillSwitch.Text = "cancel";
            btKillSwitch.UseVisualStyleBackColor = true;
            btKillSwitch.Click += btKillSwitch_Click;
            // 
            // btClearDebug
            // 
            btClearDebug.Location = new Point(1072, 489);
            btClearDebug.Name = "btClearDebug";
            btClearDebug.Size = new Size(102, 23);
            btClearDebug.TabIndex = 6;
            btClearDebug.Text = "clear debug";
            btClearDebug.UseVisualStyleBackColor = true;
            btClearDebug.Click += btClearDebug_Click;
            // 
            // tbStatus
            // 
            tbStatus.Location = new Point(796, 518);
            tbStatus.Multiline = true;
            tbStatus.Name = "tbStatus";
            tbStatus.ScrollBars = ScrollBars.Vertical;
            tbStatus.Size = new Size(627, 400);
            tbStatus.TabIndex = 7;
            // 
            // btStartBasicScript
            // 
            btStartBasicScript.Location = new Point(1180, 488);
            btStartBasicScript.Name = "btStartBasicScript";
            btStartBasicScript.Size = new Size(101, 23);
            btStartBasicScript.TabIndex = 9;
            btStartBasicScript.Text = "start basic script";
            btStartBasicScript.UseVisualStyleBackColor = true;
            btStartBasicScript.Click += btStartBasicScript_Click;
            // 
            // btSendenNeu
            // 
            btSendenNeu.Location = new Point(893, 489);
            btSendenNeu.Name = "btSendenNeu";
            btSendenNeu.Size = new Size(92, 23);
            btSendenNeu.TabIndex = 10;
            btSendenNeu.Text = "senden neu";
            btSendenNeu.UseVisualStyleBackColor = true;
            btSendenNeu.Click += btSendenNeu_Click;
            // 
            // btFormatListe
            // 
            btFormatListe.Location = new Point(1492, 489);
            btFormatListe.Name = "btFormatListe";
            btFormatListe.Size = new Size(75, 23);
            btFormatListe.TabIndex = 11;
            btFormatListe.Text = "format liste";
            btFormatListe.UseVisualStyleBackColor = true;
            btFormatListe.Click += btFormatListe_Click;
            // 
            // cbBaseModel
            // 
            cbBaseModel.FormattingEnabled = true;
            cbBaseModel.Items.AddRange(new object[] { "fluently/Fluently-XL-v2", "sd-community/sdxl-flash", "stabilityai/sdxl-turbo", "stabilityai/stable-diffusion-xl-base-1.0" });
            cbBaseModel.Location = new Point(1434, 753);
            cbBaseModel.Name = "cbBaseModel";
            cbBaseModel.Size = new Size(247, 23);
            cbBaseModel.TabIndex = 12;
            cbBaseModel.Text = "stabilityai/stable-diffusion-xl-base-1.0";
            // 
            // label1
            // 
            label1.AutoSize = true;
            label1.Location = new Point(1434, 735);
            label1.Name = "label1";
            label1.Size = new Size(71, 15);
            label1.TabIndex = 13;
            label1.Text = "Base Model:";
            // 
            // status_label
            // 
            status_label.AutoSize = true;
            status_label.Font = new Font("Segoe UI", 15.75F, FontStyle.Bold, GraphicsUnit.Point, 0);
            status_label.Location = new Point(1391, 946);
            status_label.Name = "status_label";
            status_label.Size = new Size(65, 30);
            status_label.TabIndex = 14;
            status_label.Text = "Init...";
            // 
            // btnGenerateModelInfo
            // 
            btnGenerateModelInfo.Location = new Point(1408, 967);
            btnGenerateModelInfo.Name = "btnGenerateModelInfo";
            btnGenerateModelInfo.Size = new Size(255, 23);
            btnGenerateModelInfo.TabIndex = 15;
            btnGenerateModelInfo.Text = "Model infos als html für alle generieren";
            btnGenerateModelInfo.UseVisualStyleBackColor = true;
            btnGenerateModelInfo.Click += btnGenerateModelInfo_Click;
            // 
            // btHtmlReportAnzeigen
            // 
            btHtmlReportAnzeigen.Location = new Point(1408, 996);
            btHtmlReportAnzeigen.Name = "btHtmlReportAnzeigen";
            btHtmlReportAnzeigen.Size = new Size(252, 23);
            btHtmlReportAnzeigen.TabIndex = 16;
            btHtmlReportAnzeigen.Text = "HTML Report anzeigen";
            btHtmlReportAnzeigen.UseVisualStyleBackColor = true;
            btHtmlReportAnzeigen.Click += btHtmlReportAnzeigen_Click;
            // 
            // cbDevice
            // 
            cbDevice.FormattingEnabled = true;
            cbDevice.Items.AddRange(new object[] { "cuda", "cpu" });
            cbDevice.Location = new Point(1434, 801);
            cbDevice.Name = "cbDevice";
            cbDevice.Size = new Size(121, 23);
            cbDevice.TabIndex = 17;
            cbDevice.Text = "cuda";
            // 
            // label2
            // 
            label2.AutoSize = true;
            label2.Location = new Point(1434, 783);
            label2.Name = "label2";
            label2.Size = new Size(91, 15);
            label2.TabIndex = 18;
            label2.Text = "Rendering über:";
            // 
            // nudSeed
            // 
            nudSeed.Location = new Point(1432, 540);
            nudSeed.Name = "nudSeed";
            nudSeed.Size = new Size(120, 23);
            nudSeed.TabIndex = 19;
            nudSeed.ThousandsSeparator = true;
            nudSeed.Value = new decimal(new int[] { 10, 0, 0, 0 });
            // 
            // nudNumSteps
            // 
            nudNumSteps.Location = new Point(1432, 569);
            nudNumSteps.Name = "nudNumSteps";
            nudNumSteps.Size = new Size(120, 23);
            nudNumSteps.TabIndex = 20;
            nudNumSteps.Value = new decimal(new int[] { 20, 0, 0, 0 });
            // 
            // nudGuidanceScale
            // 
            nudGuidanceScale.Location = new Point(1435, 599);
            nudGuidanceScale.Name = "nudGuidanceScale";
            nudGuidanceScale.Size = new Size(120, 23);
            nudGuidanceScale.TabIndex = 21;
            // 
            // cbVoice
            // 
            cbVoice.FormattingEnabled = true;
            cbVoice.Items.AddRange(new object[] { "Hans", "Michael", "Stefan", "German" });
            cbVoice.Location = new Point(1435, 699);
            cbVoice.Name = "cbVoice";
            cbVoice.Size = new Size(121, 23);
            cbVoice.TabIndex = 22;
            cbVoice.Text = "German";
            // 
            // nudSpeed
            // 
            nudSpeed.Location = new Point(1436, 634);
            nudSpeed.Name = "nudSpeed";
            nudSpeed.Size = new Size(120, 23);
            nudSpeed.TabIndex = 23;
            // 
            // nudPitch
            // 
            nudPitch.Location = new Point(1436, 663);
            nudPitch.Name = "nudPitch";
            nudPitch.Size = new Size(120, 23);
            nudPitch.TabIndex = 24;
            // 
            // cbPrecision
            // 
            cbPrecision.FormattingEnabled = true;
            cbPrecision.Items.AddRange(new object[] { "fp16", "fp32" });
            cbPrecision.Location = new Point(1434, 840);
            cbPrecision.Name = "cbPrecision";
            cbPrecision.Size = new Size(121, 23);
            cbPrecision.TabIndex = 25;
            cbPrecision.Text = "fp16";
            // 
            // Form1
            // 
            AutoScaleDimensions = new SizeF(7F, 15F);
            AutoScaleMode = AutoScaleMode.Font;
            ClientSize = new Size(1684, 1031);
            Controls.Add(cbPrecision);
            Controls.Add(nudPitch);
            Controls.Add(nudSpeed);
            Controls.Add(cbVoice);
            Controls.Add(nudGuidanceScale);
            Controls.Add(nudNumSteps);
            Controls.Add(nudSeed);
            Controls.Add(label2);
            Controls.Add(cbDevice);
            Controls.Add(btHtmlReportAnzeigen);
            Controls.Add(btnGenerateModelInfo);
            Controls.Add(status_label);
            Controls.Add(label1);
            Controls.Add(cbBaseModel);
            Controls.Add(btFormatListe);
            Controls.Add(btSendenNeu);
            Controls.Add(btStartBasicScript);
            Controls.Add(tbStatus);
            Controls.Add(btClearDebug);
            Controls.Add(btKillSwitch);
            Controls.Add(tbDebug);
            Controls.Add(btSendCommand);
            Controls.Add(btStart);
            Controls.Add(tbCommand);
            Controls.Add(lvModelle);
            Name = "Form1";
            StartPosition = FormStartPosition.CenterScreen;
            Text = "Model Manager Alpha";
            FormClosing += Form1_FormClosing;
            Load += Form1_Load;
            contextMenuStrip1.ResumeLayout(false);
            ((System.ComponentModel.ISupportInitialize)nudSeed).EndInit();
            ((System.ComponentModel.ISupportInitialize)nudNumSteps).EndInit();
            ((System.ComponentModel.ISupportInitialize)nudGuidanceScale).EndInit();
            ((System.ComponentModel.ISupportInitialize)nudSpeed).EndInit();
            ((System.ComponentModel.ISupportInitialize)nudPitch).EndInit();
            ResumeLayout(false);
            PerformLayout();
        }

        #endregion

        private ListView lvModelle;
        private TextBox tbCommand;
        private Button btStart;
        private ColumnHeader columnHeader1;
        private ColumnHeader columnHeader2;
        private ColumnHeader columnHeader3;
        private ColumnHeader columnHeader4;
        private Button btSendCommand;
        private TextBox tbDebug;
        private Button btKillSwitch;
        private Button btClearDebug;
        private TextBox tbStatus;
        private ColumnHeader columnHeader5;
        private ColumnHeader columnHeader6;
        private ColumnHeader columnHeader7;
        private ColumnHeader columnHeader8;
        private ContextMenuStrip contextMenuStrip1;
        private ToolStripMenuItem alsBasicTestScriptAbspeichernToolStripMenuItem;
        private ToolStripMenuItem pasteClippboardToolStripMenuItem;
        private Button btStartBasicScript;
        private Button btSendenNeu;
        private ColumnHeader columnHeader9;
        private Button btFormatListe;
        private ComboBox cbBaseModel;
        private Label label1;
        private Label status_label;
        private Button btnGenerateModelInfo;
        private Button btHtmlReportAnzeigen;
        private ComboBox cbDevice;
        private Label label2;
        private NumericUpDown nudSeed;
        private NumericUpDown nudNumSteps;
        private NumericUpDown nudGuidanceScale;
        private ComboBox cbVoice;
        private NumericUpDown nudSpeed;
        private NumericUpDown nudPitch;
        private ComboBox cbPrecision;
    }
}
