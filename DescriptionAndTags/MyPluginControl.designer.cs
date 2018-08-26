namespace DescriptionAndTags
{
    partial class MyPluginControl
    {
        /// <summary> 
        /// Variable nécessaire au concepteur.
        /// </summary>
        private System.ComponentModel.IContainer components = null;

        /// <summary> 
        /// Nettoyage des ressources utilisées.
        /// </summary>
        /// <param name="disposing">true si les ressources managées doivent être supprimées ; sinon, false.</param>
        protected override void Dispose(bool disposing)
        {
            if (disposing && (components != null))
            {
                components.Dispose();
            }
            base.Dispose(disposing);
        }

        #region Code généré par le Concepteur de composants

        /// <summary> 
        /// Méthode requise pour la prise en charge du concepteur - ne modifiez pas 
        /// le contenu de cette méthode avec l'éditeur de code.
        /// </summary>
        private void InitializeComponent()
        {
            this.toolStripMenu = new System.Windows.Forms.ToolStrip();
            this.tsbClose = new System.Windows.Forms.ToolStripButton();
            this.tssSeparator1 = new System.Windows.Forms.ToolStripSeparator();
            this.tsbExportImport = new System.Windows.Forms.ToolStripButton();
            this.txtFileNameLocation = new System.Windows.Forms.TextBox();
            this.label1 = new System.Windows.Forms.Label();
            this.rbExport = new System.Windows.Forms.RadioButton();
            this.rbImport = new System.Windows.Forms.RadioButton();
            this.btnSelectFile = new System.Windows.Forms.Button();
            this.btnExecute = new System.Windows.Forms.Button();
            this.groupBox1 = new System.Windows.Forms.GroupBox();
            this.groupBox2 = new System.Windows.Forms.GroupBox();
            this.cxUseTags = new System.Windows.Forms.CheckBox();
            this.label5 = new System.Windows.Forms.Label();
            this.cbLanguage = new System.Windows.Forms.ComboBox();
            this.cbTagDelimiter = new System.Windows.Forms.ComboBox();
            this.label4 = new System.Windows.Forms.Label();
            this.cbStringDelimiter = new System.Windows.Forms.ComboBox();
            this.label3 = new System.Windows.Forms.Label();
            this.label2 = new System.Windows.Forms.Label();
            this.cbFieldDelimiter = new System.Windows.Forms.ComboBox();
            this.btnOpenLogFile = new System.Windows.Forms.Button();
            this.cxReadasIfPublished = new System.Windows.Forms.CheckBox();
            this.cxDetailedLog = new System.Windows.Forms.CheckBox();
            this.toolStripMenu.SuspendLayout();
            this.groupBox1.SuspendLayout();
            this.groupBox2.SuspendLayout();
            this.SuspendLayout();
            // 
            // toolStripMenu
            // 
            this.toolStripMenu.ImageScalingSize = new System.Drawing.Size(24, 24);
            this.toolStripMenu.Items.AddRange(new System.Windows.Forms.ToolStripItem[] {
            this.tsbClose,
            this.tssSeparator1,
            this.tsbExportImport});
            this.toolStripMenu.Location = new System.Drawing.Point(0, 0);
            this.toolStripMenu.Name = "toolStripMenu";
            this.toolStripMenu.Padding = new System.Windows.Forms.Padding(0, 0, 2, 0);
            this.toolStripMenu.Size = new System.Drawing.Size(764, 27);
            this.toolStripMenu.TabIndex = 4;
            this.toolStripMenu.Text = "toolStrip1";
            // 
            // tsbClose
            // 
            this.tsbClose.DisplayStyle = System.Windows.Forms.ToolStripItemDisplayStyle.Text;
            this.tsbClose.Name = "tsbClose";
            this.tsbClose.Size = new System.Drawing.Size(107, 24);
            this.tsbClose.Text = "Close this tool";
            this.tsbClose.Click += new System.EventHandler(this.tsbClose_Click);
            // 
            // tssSeparator1
            // 
            this.tssSeparator1.Name = "tssSeparator1";
            this.tssSeparator1.Size = new System.Drawing.Size(6, 27);
            // 
            // tsbExportImport
            // 
            this.tsbExportImport.DisplayStyle = System.Windows.Forms.ToolStripItemDisplayStyle.Text;
            this.tsbExportImport.DoubleClickEnabled = true;
            this.tsbExportImport.Name = "tsbExportImport";
            this.tsbExportImport.Size = new System.Drawing.Size(220, 24);
            this.tsbExportImport.Text = "Export and Import Descriptions";
            // 
            // txtFileNameLocation
            // 
            this.txtFileNameLocation.Location = new System.Drawing.Point(55, 323);
            this.txtFileNameLocation.Name = "txtFileNameLocation";
            this.txtFileNameLocation.Size = new System.Drawing.Size(489, 22);
            this.txtFileNameLocation.TabIndex = 5;
            this.txtFileNameLocation.TextChanged += new System.EventHandler(this.txtFileNameLocation_TextChanged);
            // 
            // label1
            // 
            this.label1.AutoSize = true;
            this.label1.Location = new System.Drawing.Point(-8, 326);
            this.label1.Name = "label1";
            this.label1.Size = new System.Drawing.Size(57, 17);
            this.label1.TabIndex = 6;
            this.label1.Text = "CSV file";
            // 
            // rbExport
            // 
            this.rbExport.AutoSize = true;
            this.rbExport.Checked = true;
            this.rbExport.Location = new System.Drawing.Point(0, 21);
            this.rbExport.Name = "rbExport";
            this.rbExport.Size = new System.Drawing.Size(151, 21);
            this.rbExport.TabIndex = 7;
            this.rbExport.TabStop = true;
            this.rbExport.Text = "Export Descriptions";
            this.rbExport.UseVisualStyleBackColor = true;
            this.rbExport.CheckedChanged += new System.EventHandler(this.rbExport_CheckedChanged);
            // 
            // rbImport
            // 
            this.rbImport.AutoSize = true;
            this.rbImport.Location = new System.Drawing.Point(1, 48);
            this.rbImport.Name = "rbImport";
            this.rbImport.Size = new System.Drawing.Size(150, 21);
            this.rbImport.TabIndex = 8;
            this.rbImport.Text = "Import Descriptions";
            this.rbImport.UseVisualStyleBackColor = true;
            // 
            // btnSelectFile
            // 
            this.btnSelectFile.Location = new System.Drawing.Point(544, 322);
            this.btnSelectFile.Name = "btnSelectFile";
            this.btnSelectFile.Size = new System.Drawing.Size(113, 23);
            this.btnSelectFile.TabIndex = 9;
            this.btnSelectFile.Text = "Choose file";
            this.btnSelectFile.UseVisualStyleBackColor = true;
            this.btnSelectFile.Click += new System.EventHandler(this.btnSelectFile_Click);
            // 
            // btnExecute
            // 
            this.btnExecute.Location = new System.Drawing.Point(544, 363);
            this.btnExecute.Name = "btnExecute";
            this.btnExecute.Size = new System.Drawing.Size(113, 38);
            this.btnExecute.TabIndex = 10;
            this.btnExecute.Text = "Run Export";
            this.btnExecute.UseVisualStyleBackColor = true;
            this.btnExecute.Click += new System.EventHandler(this.btnExecute_Click);
            // 
            // groupBox1
            // 
            this.groupBox1.Controls.Add(this.rbExport);
            this.groupBox1.Controls.Add(this.rbImport);
            this.groupBox1.Location = new System.Drawing.Point(16, 45);
            this.groupBox1.Name = "groupBox1";
            this.groupBox1.Size = new System.Drawing.Size(170, 100);
            this.groupBox1.TabIndex = 11;
            this.groupBox1.TabStop = false;
            this.groupBox1.Text = "Operation";
            // 
            // groupBox2
            // 
            this.groupBox2.Controls.Add(this.cxDetailedLog);
            this.groupBox2.Controls.Add(this.cxReadasIfPublished);
            this.groupBox2.Controls.Add(this.cxUseTags);
            this.groupBox2.Controls.Add(this.label5);
            this.groupBox2.Controls.Add(this.cbLanguage);
            this.groupBox2.Controls.Add(this.cbTagDelimiter);
            this.groupBox2.Controls.Add(this.label4);
            this.groupBox2.Controls.Add(this.cbStringDelimiter);
            this.groupBox2.Controls.Add(this.label3);
            this.groupBox2.Controls.Add(this.label2);
            this.groupBox2.Controls.Add(this.cbFieldDelimiter);
            this.groupBox2.Location = new System.Drawing.Point(205, 45);
            this.groupBox2.Name = "groupBox2";
            this.groupBox2.Size = new System.Drawing.Size(517, 247);
            this.groupBox2.TabIndex = 12;
            this.groupBox2.TabStop = false;
            this.groupBox2.Text = "Settings (saved between sessions)";
            // 
            // cxUseTags
            // 
            this.cxUseTags.AutoSize = true;
            this.cxUseTags.Location = new System.Drawing.Point(9, 98);
            this.cxUseTags.Name = "cxUseTags";
            this.cxUseTags.RightToLeft = System.Windows.Forms.RightToLeft.No;
            this.cxUseTags.Size = new System.Drawing.Size(99, 21);
            this.cxUseTags.TabIndex = 8;
            this.cxUseTags.Text = "Use Tags?";
            this.cxUseTags.UseVisualStyleBackColor = true;
            this.cxUseTags.CheckedChanged += new System.EventHandler(this.cxUseTags_CheckedChanged);
            // 
            // label5
            // 
            this.label5.AutoSize = true;
            this.label5.Location = new System.Drawing.Point(8, 144);
            this.label5.Name = "label5";
            this.label5.Size = new System.Drawing.Size(109, 17);
            this.label5.TabIndex = 7;
            this.label5.Text = "Language Code";
            // 
            // cbLanguage
            // 
            this.cbLanguage.FormattingEnabled = true;
            this.cbLanguage.Location = new System.Drawing.Point(134, 141);
            this.cbLanguage.Name = "cbLanguage";
            this.cbLanguage.Size = new System.Drawing.Size(250, 24);
            this.cbLanguage.TabIndex = 6;
            this.cbLanguage.SelectedIndexChanged += new System.EventHandler(this.cbLanguage_SelectedIndexChanged);
            // 
            // cbTagDelimiter
            // 
            this.cbTagDelimiter.FormattingEnabled = true;
            this.cbTagDelimiter.Items.AddRange(new object[] {
            "[ and ]",
            "[[ and ]]",
            "{ and }",
            "{{ and }}"});
            this.cbTagDelimiter.Location = new System.Drawing.Point(254, 95);
            this.cbTagDelimiter.Name = "cbTagDelimiter";
            this.cbTagDelimiter.Size = new System.Drawing.Size(96, 24);
            this.cbTagDelimiter.TabIndex = 5;
            this.cbTagDelimiter.SelectedIndexChanged += new System.EventHandler(this.cbTagDelimiter_SelectedIndexChanged);
            // 
            // label4
            // 
            this.label4.AutoSize = true;
            this.label4.Location = new System.Drawing.Point(131, 99);
            this.label4.Name = "label4";
            this.label4.Size = new System.Drawing.Size(90, 17);
            this.label4.TabIndex = 4;
            this.label4.Text = "Tag delimiter";
            // 
            // cbStringDelimiter
            // 
            this.cbStringDelimiter.FormattingEnabled = true;
            this.cbStringDelimiter.Items.AddRange(new object[] {
            "\"",
            "\'"});
            this.cbStringDelimiter.Location = new System.Drawing.Point(134, 58);
            this.cbStringDelimiter.Name = "cbStringDelimiter";
            this.cbStringDelimiter.Size = new System.Drawing.Size(96, 24);
            this.cbStringDelimiter.TabIndex = 3;
            this.cbStringDelimiter.SelectedIndexChanged += new System.EventHandler(this.cbStringDelimiter_SelectedIndexChanged);
            // 
            // label3
            // 
            this.label3.AutoSize = true;
            this.label3.Location = new System.Drawing.Point(6, 65);
            this.label3.Name = "label3";
            this.label3.Size = new System.Drawing.Size(102, 17);
            this.label3.TabIndex = 2;
            this.label3.Text = "String delimiter";
            // 
            // label2
            // 
            this.label2.AutoSize = true;
            this.label2.Location = new System.Drawing.Point(6, 28);
            this.label2.Name = "label2";
            this.label2.Size = new System.Drawing.Size(92, 17);
            this.label2.TabIndex = 1;
            this.label2.Text = "CSV delimiter";
            // 
            // cbFieldDelimiter
            // 
            this.cbFieldDelimiter.FormattingEnabled = true;
            this.cbFieldDelimiter.Items.AddRange(new object[] {
            ",",
            "|",
            ";"});
            this.cbFieldDelimiter.Location = new System.Drawing.Point(134, 25);
            this.cbFieldDelimiter.Name = "cbFieldDelimiter";
            this.cbFieldDelimiter.Size = new System.Drawing.Size(96, 24);
            this.cbFieldDelimiter.TabIndex = 0;
            this.cbFieldDelimiter.SelectedIndexChanged += new System.EventHandler(this.cbFieldDelimiter_SelectedIndexChanged);
            // 
            // btnOpenLogFile
            // 
            this.btnOpenLogFile.Location = new System.Drawing.Point(55, 363);
            this.btnOpenLogFile.Name = "btnOpenLogFile";
            this.btnOpenLogFile.Size = new System.Drawing.Size(144, 36);
            this.btnOpenLogFile.TabIndex = 15;
            this.btnOpenLogFile.Text = "Open Log File";
            this.btnOpenLogFile.UseVisualStyleBackColor = true;
            this.btnOpenLogFile.Click += new System.EventHandler(this.btnOpenLogFolder_Click);
            // 
            // cxReadasIfPublished
            // 
            this.cxReadasIfPublished.AutoSize = true;
            this.cxReadasIfPublished.Location = new System.Drawing.Point(11, 180);
            this.cxReadasIfPublished.Name = "cxReadasIfPublished";
            this.cxReadasIfPublished.RightToLeft = System.Windows.Forms.RightToLeft.No;
            this.cxReadasIfPublished.Size = new System.Drawing.Size(225, 21);
            this.cxReadasIfPublished.TabIndex = 9;
            this.cxReadasIfPublished.Text = "Read from CRM as if published";
            this.cxReadasIfPublished.UseVisualStyleBackColor = true;
            this.cxReadasIfPublished.CheckedChanged += new System.EventHandler(this.cxReadasIfPublished_CheckedChanged);
            // 
            // cxDetailedLog
            // 
            this.cxDetailedLog.AutoSize = true;
            this.cxDetailedLog.Location = new System.Drawing.Point(11, 207);
            this.cxDetailedLog.Name = "cxDetailedLog";
            this.cxDetailedLog.RightToLeft = System.Windows.Forms.RightToLeft.No;
            this.cxDetailedLog.Size = new System.Drawing.Size(340, 21);
            this.cxDetailedLog.TabIndex = 10;
            this.cxDetailedLog.Text = "Detailed Log (for debugging, will slow operations)";
            this.cxDetailedLog.UseVisualStyleBackColor = true;
            this.cxDetailedLog.CheckedChanged += new System.EventHandler(this.cxDetailedLog_CheckedChanged);
            // 
            // MyPluginControl
            // 
            this.AutoScaleDimensions = new System.Drawing.SizeF(8F, 16F);
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
            this.Controls.Add(this.btnOpenLogFile);
            this.Controls.Add(this.groupBox2);
            this.Controls.Add(this.groupBox1);
            this.Controls.Add(this.btnExecute);
            this.Controls.Add(this.btnSelectFile);
            this.Controls.Add(this.label1);
            this.Controls.Add(this.txtFileNameLocation);
            this.Controls.Add(this.toolStripMenu);
            this.Margin = new System.Windows.Forms.Padding(4);
            this.Name = "MyPluginControl";
            this.Size = new System.Drawing.Size(764, 448);
            this.Load += new System.EventHandler(this.MyPluginControl_Load);
            this.toolStripMenu.ResumeLayout(false);
            this.toolStripMenu.PerformLayout();
            this.groupBox1.ResumeLayout(false);
            this.groupBox1.PerformLayout();
            this.groupBox2.ResumeLayout(false);
            this.groupBox2.PerformLayout();
            this.ResumeLayout(false);
            this.PerformLayout();

        }

        #endregion
        private System.Windows.Forms.ToolStrip toolStripMenu;
        private System.Windows.Forms.ToolStripButton tsbClose;
        private System.Windows.Forms.ToolStripButton tsbExportImport;
        private System.Windows.Forms.ToolStripSeparator tssSeparator1;
        private System.Windows.Forms.TextBox txtFileNameLocation;
        private System.Windows.Forms.Label label1;
        private System.Windows.Forms.RadioButton rbExport;
        private System.Windows.Forms.RadioButton rbImport;
        private System.Windows.Forms.Button btnSelectFile;
        private System.Windows.Forms.Button btnExecute;
        private System.Windows.Forms.GroupBox groupBox1;
        private System.Windows.Forms.GroupBox groupBox2;
        private System.Windows.Forms.ComboBox cbTagDelimiter;
        private System.Windows.Forms.Label label4;
        private System.Windows.Forms.ComboBox cbStringDelimiter;
        private System.Windows.Forms.Label label3;
        private System.Windows.Forms.Label label2;
        private System.Windows.Forms.ComboBox cbFieldDelimiter;
        private System.Windows.Forms.Label label5;
        private System.Windows.Forms.ComboBox cbLanguage;
        private System.Windows.Forms.Button btnOpenLogFile;
        private System.Windows.Forms.CheckBox cxUseTags;
        private System.Windows.Forms.CheckBox cxDetailedLog;
        private System.Windows.Forms.CheckBox cxReadasIfPublished;
    }
}
