namespace Nakladna
{
    partial class MainForm
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
            this.toDocButton = new System.Windows.Forms.Button();
            this.dateTimePicker1 = new System.Windows.Forms.DateTimePicker();
            this.label2 = new System.Windows.Forms.Label();
            this.groupBox1 = new System.Windows.Forms.GroupBox();
            this.button2 = new System.Windows.Forms.Button();
            this.btnAddGood = new System.Windows.Forms.Button();
            this.dataGridView1 = new System.Windows.Forms.DataGridView();
            this.ColumnGoodType = new System.Windows.Forms.DataGridViewTextBoxColumn();
            this.ColumnGoodPrice = new System.Windows.Forms.DataGridViewTextBoxColumn();
            this.ColumnColumn = new System.Windows.Forms.DataGridViewTextBoxColumn();
            this.ColumnHasReturn = new System.Windows.Forms.DataGridViewCheckBoxColumn();
            this.ColumnReturn = new System.Windows.Forms.DataGridViewTextBoxColumn();
            this.ColumnRemove = new System.Windows.Forms.DataGridViewButtonColumn();
            this.ColumnSpecialPrice = new System.Windows.Forms.DataGridViewButtonColumn();
            this.groupBox2 = new System.Windows.Forms.GroupBox();
            this.dateTimePicker2 = new System.Windows.Forms.DateTimePicker();
            this.label3 = new System.Windows.Forms.Label();
            this.button1 = new System.Windows.Forms.Button();
            this.importButton = new System.Windows.Forms.Button();
            this.statusStrip1 = new System.Windows.Forms.StatusStrip();
            this.toolStripStatusLabel = new System.Windows.Forms.ToolStripStatusLabel();
            this.toolStripProgressBar = new System.Windows.Forms.ToolStripProgressBar();
            this.groupBox1.SuspendLayout();
            ((System.ComponentModel.ISupportInitialize)(this.dataGridView1)).BeginInit();
            this.groupBox2.SuspendLayout();
            this.statusStrip1.SuspendLayout();
            this.SuspendLayout();
            // 
            // toDocButton
            // 
            this.toDocButton.Anchor = ((System.Windows.Forms.AnchorStyles)((System.Windows.Forms.AnchorStyles.Bottom | System.Windows.Forms.AnchorStyles.Right)));
            this.toDocButton.Location = new System.Drawing.Point(574, 58);
            this.toDocButton.Name = "toDocButton";
            this.toDocButton.Size = new System.Drawing.Size(75, 20);
            this.toDocButton.TabIndex = 4;
            this.toDocButton.Text = "В Word";
            this.toDocButton.UseVisualStyleBackColor = true;
            this.toDocButton.Click += new System.EventHandler(this.toDocButton_Click);
            // 
            // dateTimePicker1
            // 
            this.dateTimePicker1.Format = System.Windows.Forms.DateTimePickerFormat.Short;
            this.dateTimePicker1.Location = new System.Drawing.Point(83, 20);
            this.dateTimePicker1.Name = "dateTimePicker1";
            this.dateTimePicker1.Size = new System.Drawing.Size(110, 20);
            this.dateTimePicker1.TabIndex = 5;
            this.dateTimePicker1.ValueChanged += new System.EventHandler(this.dateTimePicker_ValueChanged);
            // 
            // label2
            // 
            this.label2.AutoSize = true;
            this.label2.Location = new System.Drawing.Point(10, 26);
            this.label2.Name = "label2";
            this.label2.Size = new System.Drawing.Size(67, 13);
            this.label2.TabIndex = 7;
            this.label2.Text = "За період з:";
            // 
            // groupBox1
            // 
            this.groupBox1.Anchor = ((System.Windows.Forms.AnchorStyles)((((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Bottom) 
            | System.Windows.Forms.AnchorStyles.Left) 
            | System.Windows.Forms.AnchorStyles.Right)));
            this.groupBox1.Controls.Add(this.button2);
            this.groupBox1.Controls.Add(this.btnAddGood);
            this.groupBox1.Controls.Add(this.dataGridView1);
            this.groupBox1.Controls.Add(this.groupBox2);
            this.groupBox1.Controls.Add(this.button1);
            this.groupBox1.Controls.Add(this.importButton);
            this.groupBox1.Location = new System.Drawing.Point(0, 0);
            this.groupBox1.Name = "groupBox1";
            this.groupBox1.Size = new System.Drawing.Size(664, 375);
            this.groupBox1.TabIndex = 9;
            this.groupBox1.TabStop = false;
            // 
            // button2
            // 
            this.button2.Anchor = ((System.Windows.Forms.AnchorStyles)((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Right)));
            this.button2.Location = new System.Drawing.Point(456, 13);
            this.button2.Name = "button2";
            this.button2.Size = new System.Drawing.Size(121, 22);
            this.button2.TabIndex = 11;
            this.button2.Text = "Очистити продажі";
            this.button2.UseVisualStyleBackColor = true;
            this.button2.Click += new System.EventHandler(this.button2_Click);
            // 
            // btnAddGood
            // 
            this.btnAddGood.Anchor = ((System.Windows.Forms.AnchorStyles)((System.Windows.Forms.AnchorStyles.Bottom | System.Windows.Forms.AnchorStyles.Right)));
            this.btnAddGood.Location = new System.Drawing.Point(583, 41);
            this.btnAddGood.Name = "btnAddGood";
            this.btnAddGood.Size = new System.Drawing.Size(75, 23);
            this.btnAddGood.TabIndex = 26;
            this.btnAddGood.Text = "+ Товар";
            this.btnAddGood.UseVisualStyleBackColor = true;
            this.btnAddGood.Click += new System.EventHandler(this.btnAddGood_Click);
            // 
            // dataGridView1
            // 
            this.dataGridView1.AllowUserToOrderColumns = true;
            this.dataGridView1.Anchor = ((System.Windows.Forms.AnchorStyles)((((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Bottom) 
            | System.Windows.Forms.AnchorStyles.Left) 
            | System.Windows.Forms.AnchorStyles.Right)));
            this.dataGridView1.ColumnHeadersHeightSizeMode = System.Windows.Forms.DataGridViewColumnHeadersHeightSizeMode.AutoSize;
            this.dataGridView1.Columns.AddRange(new System.Windows.Forms.DataGridViewColumn[] {
            this.ColumnGoodType,
            this.ColumnGoodPrice,
            this.ColumnColumn,
            this.ColumnHasReturn,
            this.ColumnReturn,
            this.ColumnRemove,
            this.ColumnSpecialPrice});
            this.dataGridView1.Location = new System.Drawing.Point(6, 41);
            this.dataGridView1.Name = "dataGridView1";
            this.dataGridView1.Size = new System.Drawing.Size(571, 241);
            this.dataGridView1.TabIndex = 25;
            this.dataGridView1.CellContentClick += new System.Windows.Forms.DataGridViewCellEventHandler(this.dataGridView1_CellContentClick);
            // 
            // ColumnGoodType
            // 
            this.ColumnGoodType.AutoSizeMode = System.Windows.Forms.DataGridViewAutoSizeColumnMode.AllCells;
            this.ColumnGoodType.HeaderText = "Товар";
            this.ColumnGoodType.Name = "ColumnGoodType";
            this.ColumnGoodType.Resizable = System.Windows.Forms.DataGridViewTriState.True;
            this.ColumnGoodType.Width = 63;
            // 
            // ColumnGoodPrice
            // 
            this.ColumnGoodPrice.AutoSizeMode = System.Windows.Forms.DataGridViewAutoSizeColumnMode.AllCells;
            this.ColumnGoodPrice.HeaderText = "Ціна";
            this.ColumnGoodPrice.Name = "ColumnGoodPrice";
            this.ColumnGoodPrice.Width = 54;
            // 
            // ColumnColumn
            // 
            this.ColumnColumn.AutoSizeMode = System.Windows.Forms.DataGridViewAutoSizeColumnMode.AllCells;
            this.ColumnColumn.HeaderText = "Колонка";
            this.ColumnColumn.Name = "ColumnColumn";
            this.ColumnColumn.Width = 75;
            // 
            // ColumnHasReturn
            // 
            this.ColumnHasReturn.HeaderText = "Повернення";
            this.ColumnHasReturn.Name = "ColumnHasReturn";
            // 
            // ColumnReturn
            // 
            this.ColumnReturn.AutoSizeMode = System.Windows.Forms.DataGridViewAutoSizeColumnMode.AllCells;
            this.ColumnReturn.HeaderText = "Поверн.";
            this.ColumnReturn.Name = "ColumnReturn";
            this.ColumnReturn.Width = 73;
            // 
            // ColumnRemove
            // 
            this.ColumnRemove.AutoSizeMode = System.Windows.Forms.DataGridViewAutoSizeColumnMode.AllCells;
            this.ColumnRemove.HeaderText = "Видалити";
            this.ColumnRemove.Name = "ColumnRemove";
            this.ColumnRemove.Resizable = System.Windows.Forms.DataGridViewTriState.True;
            this.ColumnRemove.Text = "Видалити";
            this.ColumnRemove.UseColumnTextForButtonValue = true;
            this.ColumnRemove.Width = 61;
            // 
            // ColumnSpecialPrice
            // 
            this.ColumnSpecialPrice.AutoSizeMode = System.Windows.Forms.DataGridViewAutoSizeColumnMode.AllCells;
            this.ColumnSpecialPrice.HeaderText = "Ціни";
            this.ColumnSpecialPrice.Name = "ColumnSpecialPrice";
            this.ColumnSpecialPrice.Text = "Ціни";
            this.ColumnSpecialPrice.UseColumnTextForButtonValue = true;
            this.ColumnSpecialPrice.Width = 35;
            // 
            // groupBox2
            // 
            this.groupBox2.Controls.Add(this.label2);
            this.groupBox2.Controls.Add(this.dateTimePicker1);
            this.groupBox2.Controls.Add(this.toDocButton);
            this.groupBox2.Controls.Add(this.dateTimePicker2);
            this.groupBox2.Controls.Add(this.label3);
            this.groupBox2.Dock = System.Windows.Forms.DockStyle.Bottom;
            this.groupBox2.Location = new System.Drawing.Point(3, 288);
            this.groupBox2.Name = "groupBox2";
            this.groupBox2.Size = new System.Drawing.Size(658, 84);
            this.groupBox2.TabIndex = 24;
            this.groupBox2.TabStop = false;
            // 
            // dateTimePicker2
            // 
            this.dateTimePicker2.Format = System.Windows.Forms.DateTimePickerFormat.Short;
            this.dateTimePicker2.Location = new System.Drawing.Point(83, 46);
            this.dateTimePicker2.Name = "dateTimePicker2";
            this.dateTimePicker2.Size = new System.Drawing.Size(110, 20);
            this.dateTimePicker2.TabIndex = 8;
            this.dateTimePicker2.ValueChanged += new System.EventHandler(this.dateTimePicker_ValueChanged);
            // 
            // label3
            // 
            this.label3.AutoSize = true;
            this.label3.Location = new System.Drawing.Point(53, 46);
            this.label3.Name = "label3";
            this.label3.Size = new System.Drawing.Size(24, 13);
            this.label3.TabIndex = 9;
            this.label3.Text = "По:";
            // 
            // button1
            // 
            this.button1.Anchor = ((System.Windows.Forms.AnchorStyles)((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Right)));
            this.button1.Location = new System.Drawing.Point(583, 12);
            this.button1.Name = "button1";
            this.button1.Size = new System.Drawing.Size(75, 22);
            this.button1.TabIndex = 15;
            this.button1.Text = "Настройки";
            this.button1.UseVisualStyleBackColor = true;
            this.button1.Click += new System.EventHandler(this.button1_Click);
            // 
            // importButton
            // 
            this.importButton.Location = new System.Drawing.Point(6, 12);
            this.importButton.Name = "importButton";
            this.importButton.Size = new System.Drawing.Size(111, 23);
            this.importButton.TabIndex = 18;
            this.importButton.Text = "Імпорт з файлу";
            this.importButton.UseVisualStyleBackColor = true;
            this.importButton.Click += new System.EventHandler(this.importButton_Click);
            // 
            // statusStrip1
            // 
            this.statusStrip1.Items.AddRange(new System.Windows.Forms.ToolStripItem[] {
            this.toolStripStatusLabel,
            this.toolStripProgressBar});
            this.statusStrip1.Location = new System.Drawing.Point(0, 378);
            this.statusStrip1.Name = "statusStrip1";
            this.statusStrip1.Size = new System.Drawing.Size(664, 22);
            this.statusStrip1.TabIndex = 10;
            this.statusStrip1.Text = "statusStrip1";
            // 
            // toolStripStatusLabel
            // 
            this.toolStripStatusLabel.Name = "toolStripStatusLabel";
            this.toolStripStatusLabel.Size = new System.Drawing.Size(45, 17);
            this.toolStripStatusLabel.Text = "Готово";
            // 
            // toolStripProgressBar
            // 
            this.toolStripProgressBar.Name = "toolStripProgressBar";
            this.toolStripProgressBar.Size = new System.Drawing.Size(100, 16);
            this.toolStripProgressBar.Style = System.Windows.Forms.ProgressBarStyle.Marquee;
            this.toolStripProgressBar.Visible = false;
            // 
            // MainForm
            // 
            this.AutoScaleDimensions = new System.Drawing.SizeF(6F, 13F);
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
            this.ClientSize = new System.Drawing.Size(664, 400);
            this.Controls.Add(this.statusStrip1);
            this.Controls.Add(this.groupBox1);
            this.MinimumSize = new System.Drawing.Size(465, 233);
            this.Name = "MainForm";
            this.Text = "ОтТакВот";
            this.FormClosing += new System.Windows.Forms.FormClosingEventHandler(this.MainForm_FormClosing);
            this.groupBox1.ResumeLayout(false);
            ((System.ComponentModel.ISupportInitialize)(this.dataGridView1)).EndInit();
            this.groupBox2.ResumeLayout(false);
            this.groupBox2.PerformLayout();
            this.statusStrip1.ResumeLayout(false);
            this.statusStrip1.PerformLayout();
            this.ResumeLayout(false);
            this.PerformLayout();

        }

        #endregion

        private System.Windows.Forms.Button toDocButton;
        private System.Windows.Forms.DateTimePicker dateTimePicker1;
        private System.Windows.Forms.Label label2;
        private System.Windows.Forms.GroupBox groupBox1;
        private System.Windows.Forms.Label label3;
        private System.Windows.Forms.DateTimePicker dateTimePicker2;
        private System.Windows.Forms.Button button1;
        private System.Windows.Forms.GroupBox groupBox2;
        private System.Windows.Forms.Button importButton;
        private System.Windows.Forms.StatusStrip statusStrip1;
        private System.Windows.Forms.ToolStripStatusLabel toolStripStatusLabel;
        private System.Windows.Forms.ToolStripProgressBar toolStripProgressBar;
        private System.Windows.Forms.DataGridView dataGridView1;
        private System.Windows.Forms.Button btnAddGood;
        private System.Windows.Forms.Button button2;
        private System.Windows.Forms.DataGridViewTextBoxColumn ColumnGoodType;
        private System.Windows.Forms.DataGridViewTextBoxColumn ColumnGoodPrice;
        private System.Windows.Forms.DataGridViewTextBoxColumn ColumnColumn;
        private System.Windows.Forms.DataGridViewCheckBoxColumn ColumnHasReturn;
        private System.Windows.Forms.DataGridViewTextBoxColumn ColumnReturn;
        private System.Windows.Forms.DataGridViewButtonColumn ColumnRemove;
        private System.Windows.Forms.DataGridViewButtonColumn ColumnSpecialPrice;
    }
}

