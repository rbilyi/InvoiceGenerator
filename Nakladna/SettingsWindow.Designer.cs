namespace Nakladna
{
    partial class SettingsWindow
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
            this.custRowTxt = new System.Windows.Forms.TextBox();
            this.label2 = new System.Windows.Forms.Label();
            this.templtePathTxt = new System.Windows.Forms.TextBox();
            this.label4 = new System.Windows.Forms.Label();
            this.custColTxt = new System.Windows.Forms.TextBox();
            this.label6 = new System.Windows.Forms.Label();
            this.producerTxt = new System.Windows.Forms.TextBox();
            this.label8 = new System.Windows.Forms.Label();
            this.connectionStringTxt = new System.Windows.Forms.TextBox();
            this.label9 = new System.Windows.Forms.Label();
            this.saveBtn = new System.Windows.Forms.Button();
            this.reloadBtn = new System.Windows.Forms.Button();
            this.resetBtn = new System.Windows.Forms.Button();
            this.label1 = new System.Windows.Forms.Label();
            this.customerStopPhraseTxt = new System.Windows.Forms.TextBox();
            this.SuspendLayout();
            // 
            // custRowTxt
            // 
            this.custRowTxt.Location = new System.Drawing.Point(265, 18);
            this.custRowTxt.Margin = new System.Windows.Forms.Padding(4);
            this.custRowTxt.Name = "custRowTxt";
            this.custRowTxt.Size = new System.Drawing.Size(69, 22);
            this.custRowTxt.TabIndex = 3;
            // 
            // label2
            // 
            this.label2.AutoSize = true;
            this.label2.Location = new System.Drawing.Point(10, 21);
            this.label2.Margin = new System.Windows.Forms.Padding(4, 0, 4, 0);
            this.label2.Name = "label2";
            this.label2.Size = new System.Drawing.Size(247, 17);
            this.label2.TabIndex = 2;
            this.label2.Text = "З якого рядка починаються клієнти:";
            // 
            // templtePathTxt
            // 
            this.templtePathTxt.Location = new System.Drawing.Point(13, 148);
            this.templtePathTxt.Margin = new System.Windows.Forms.Padding(4);
            this.templtePathTxt.Name = "templtePathTxt";
            this.templtePathTxt.Size = new System.Drawing.Size(369, 22);
            this.templtePathTxt.TabIndex = 7;
            // 
            // label4
            // 
            this.label4.AutoSize = true;
            this.label4.Location = new System.Drawing.Point(10, 127);
            this.label4.Margin = new System.Windows.Forms.Padding(4, 0, 4, 0);
            this.label4.Name = "label4";
            this.label4.Size = new System.Drawing.Size(133, 17);
            this.label4.TabIndex = 6;
            this.label4.Text = "Шаблон накладної:";
            // 
            // custColTxt
            // 
            this.custColTxt.Location = new System.Drawing.Point(169, 48);
            this.custColTxt.Margin = new System.Windows.Forms.Padding(4);
            this.custColTxt.Name = "custColTxt";
            this.custColTxt.Size = new System.Drawing.Size(69, 22);
            this.custColTxt.TabIndex = 11;
            // 
            // label6
            // 
            this.label6.AutoSize = true;
            this.label6.Location = new System.Drawing.Point(10, 51);
            this.label6.Margin = new System.Windows.Forms.Padding(4, 0, 4, 0);
            this.label6.Name = "label6";
            this.label6.Size = new System.Drawing.Size(157, 17);
            this.label6.TabIndex = 10;
            this.label6.Text = "В якій колонці клієнти:";
            // 
            // producerTxt
            // 
            this.producerTxt.Location = new System.Drawing.Point(13, 195);
            this.producerTxt.Margin = new System.Windows.Forms.Padding(4);
            this.producerTxt.Name = "producerTxt";
            this.producerTxt.Size = new System.Drawing.Size(369, 22);
            this.producerTxt.TabIndex = 15;
            // 
            // label8
            // 
            this.label8.AutoSize = true;
            this.label8.Location = new System.Drawing.Point(10, 174);
            this.label8.Margin = new System.Windows.Forms.Padding(4, 0, 4, 0);
            this.label8.Name = "label8";
            this.label8.Size = new System.Drawing.Size(76, 17);
            this.label8.TabIndex = 14;
            this.label8.Text = "Виробник:";
            // 
            // connectionStringTxt
            // 
            this.connectionStringTxt.Location = new System.Drawing.Point(91, 232);
            this.connectionStringTxt.Margin = new System.Windows.Forms.Padding(4);
            this.connectionStringTxt.Multiline = true;
            this.connectionStringTxt.Name = "connectionStringTxt";
            this.connectionStringTxt.Size = new System.Drawing.Size(294, 67);
            this.connectionStringTxt.TabIndex = 17;
            // 
            // label9
            // 
            this.label9.AutoSize = true;
            this.label9.Location = new System.Drawing.Point(10, 232);
            this.label9.Margin = new System.Windows.Forms.Padding(4, 0, 4, 0);
            this.label9.Name = "label9";
            this.label9.Size = new System.Drawing.Size(70, 17);
            this.label9.TabIndex = 16;
            this.label9.Text = "con string";
            // 
            // saveBtn
            // 
            this.saveBtn.Location = new System.Drawing.Point(15, 307);
            this.saveBtn.Margin = new System.Windows.Forms.Padding(4);
            this.saveBtn.Name = "saveBtn";
            this.saveBtn.Size = new System.Drawing.Size(100, 28);
            this.saveBtn.TabIndex = 18;
            this.saveBtn.Text = "Save";
            this.saveBtn.UseVisualStyleBackColor = true;
            this.saveBtn.Click += new System.EventHandler(this.saveBnt_Click);
            // 
            // reloadBtn
            // 
            this.reloadBtn.Location = new System.Drawing.Point(123, 307);
            this.reloadBtn.Margin = new System.Windows.Forms.Padding(4);
            this.reloadBtn.Name = "reloadBtn";
            this.reloadBtn.Size = new System.Drawing.Size(100, 28);
            this.reloadBtn.TabIndex = 19;
            this.reloadBtn.Text = "Reload";
            this.reloadBtn.UseVisualStyleBackColor = true;
            this.reloadBtn.Click += new System.EventHandler(this.reloadBtn_Click);
            // 
            // resetBtn
            // 
            this.resetBtn.Location = new System.Drawing.Point(292, 307);
            this.resetBtn.Margin = new System.Windows.Forms.Padding(4);
            this.resetBtn.Name = "resetBtn";
            this.resetBtn.Size = new System.Drawing.Size(100, 28);
            this.resetBtn.TabIndex = 20;
            this.resetBtn.Text = "reset!";
            this.resetBtn.UseVisualStyleBackColor = true;
            this.resetBtn.Click += new System.EventHandler(this.resetBtn_Click);
            // 
            // label1
            // 
            this.label1.AutoSize = true;
            this.label1.Location = new System.Drawing.Point(10, 80);
            this.label1.Name = "label1";
            this.label1.Size = new System.Drawing.Size(208, 17);
            this.label1.TabIndex = 21;
            this.label1.Text = "Закінчення клієнтів по тексту:";
            // 
            // customerStopPhraseTxt
            // 
            this.customerStopPhraseTxt.Location = new System.Drawing.Point(13, 101);
            this.customerStopPhraseTxt.Margin = new System.Windows.Forms.Padding(4);
            this.customerStopPhraseTxt.Name = "customerStopPhraseTxt";
            this.customerStopPhraseTxt.Size = new System.Drawing.Size(369, 22);
            this.customerStopPhraseTxt.TabIndex = 22;
            // 
            // SettingsWindow
            // 
            this.AutoScaleDimensions = new System.Drawing.SizeF(8F, 16F);
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
            this.ClientSize = new System.Drawing.Size(398, 348);
            this.Controls.Add(this.customerStopPhraseTxt);
            this.Controls.Add(this.label1);
            this.Controls.Add(this.resetBtn);
            this.Controls.Add(this.reloadBtn);
            this.Controls.Add(this.saveBtn);
            this.Controls.Add(this.connectionStringTxt);
            this.Controls.Add(this.label9);
            this.Controls.Add(this.producerTxt);
            this.Controls.Add(this.label8);
            this.Controls.Add(this.custColTxt);
            this.Controls.Add(this.label6);
            this.Controls.Add(this.templtePathTxt);
            this.Controls.Add(this.label4);
            this.Controls.Add(this.custRowTxt);
            this.Controls.Add(this.label2);
            this.Margin = new System.Windows.Forms.Padding(4);
            this.Name = "SettingsWindow";
            this.Text = "Settings";
            this.ResumeLayout(false);
            this.PerformLayout();

        }

        #endregion
        private System.Windows.Forms.TextBox custRowTxt;
        private System.Windows.Forms.Label label2;
        private System.Windows.Forms.TextBox templtePathTxt;
        private System.Windows.Forms.Label label4;
        private System.Windows.Forms.TextBox custColTxt;
        private System.Windows.Forms.Label label6;
        private System.Windows.Forms.TextBox producerTxt;
        private System.Windows.Forms.Label label8;
        private System.Windows.Forms.TextBox connectionStringTxt;
        private System.Windows.Forms.Label label9;
        private System.Windows.Forms.Button saveBtn;
        private System.Windows.Forms.Button reloadBtn;
        private System.Windows.Forms.Button resetBtn;
        private System.Windows.Forms.Label label1;
        private System.Windows.Forms.TextBox customerStopPhraseTxt;
    }
}