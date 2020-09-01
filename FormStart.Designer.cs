namespace ImageCutter
{
    partial class FormStart
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
            System.ComponentModel.ComponentResourceManager resources = new System.ComponentModel.ComponentResourceManager(typeof(FormStart));
            this.textBoxKeyInput = new System.Windows.Forms.TextBox();
            this.label1 = new System.Windows.Forms.Label();
            this.panel1 = new System.Windows.Forms.Panel();
            this.label3 = new System.Windows.Forms.Label();
            this.label2 = new System.Windows.Forms.Label();
            this.buttonActivate = new System.Windows.Forms.Button();
            this.labelShowInfo = new System.Windows.Forms.Label();
            this.panel1.SuspendLayout();
            this.SuspendLayout();
            // 
            // textBoxKeyInput
            // 
            this.textBoxKeyInput.Location = new System.Drawing.Point(23, 64);
            this.textBoxKeyInput.Name = "textBoxKeyInput";
            this.textBoxKeyInput.Size = new System.Drawing.Size(415, 26);
            this.textBoxKeyInput.TabIndex = 0;
            // 
            // label1
            // 
            this.label1.AutoSize = true;
            this.label1.Font = new System.Drawing.Font("Microsoft Sans Serif", 10F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(204)));
            this.label1.Location = new System.Drawing.Point(18, 21);
            this.label1.Name = "label1";
            this.label1.Size = new System.Drawing.Size(286, 25);
            this.label1.TabIndex = 1;
            this.label1.Text = "Введите лицензионный ключ";
            // 
            // panel1
            // 
            this.panel1.BorderStyle = System.Windows.Forms.BorderStyle.Fixed3D;
            this.panel1.Controls.Add(this.label3);
            this.panel1.Controls.Add(this.label2);
            this.panel1.Controls.Add(this.textBoxKeyInput);
            this.panel1.Controls.Add(this.label1);
            this.panel1.Location = new System.Drawing.Point(12, 12);
            this.panel1.Name = "panel1";
            this.panel1.Size = new System.Drawing.Size(454, 214);
            this.panel1.TabIndex = 2;
            // 
            // label3
            // 
            this.label3.Font = new System.Drawing.Font("Microsoft Sans Serif", 7F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(204)));
            this.label3.Location = new System.Drawing.Point(20, 102);
            this.label3.Name = "label3";
            this.label3.Size = new System.Drawing.Size(418, 74);
            this.label3.TabIndex = 3;
            this.label3.Text = "Одним ключом одновременно может быть активированна только программа на одном комп" +
    "ьютере. При активации уже использованного ключа на другом компьютере, старая коп" +
    "ия программы будет деактивированна";
            // 
            // label2
            // 
            this.label2.AutoSize = true;
            this.label2.Location = new System.Drawing.Point(23, 111);
            this.label2.Name = "label2";
            this.label2.Size = new System.Drawing.Size(0, 20);
            this.label2.TabIndex = 2;
            // 
            // buttonActivate
            // 
            this.buttonActivate.FlatStyle = System.Windows.Forms.FlatStyle.System;
            this.buttonActivate.Location = new System.Drawing.Point(300, 232);
            this.buttonActivate.Name = "buttonActivate";
            this.buttonActivate.Size = new System.Drawing.Size(152, 47);
            this.buttonActivate.TabIndex = 3;
            this.buttonActivate.Text = "активировать";
            this.buttonActivate.UseVisualStyleBackColor = true;
            this.buttonActivate.Click += new System.EventHandler(this.buttonActivate_Click_1);
            // 
            // labelShowInfo
            // 
            this.labelShowInfo.Location = new System.Drawing.Point(13, 233);
            this.labelShowInfo.Name = "labelShowInfo";
            this.labelShowInfo.Size = new System.Drawing.Size(281, 99);
            this.labelShowInfo.TabIndex = 5;
            this.labelShowInfo.Text = "label4";
            // 
            // FormStart
            // 
            this.AutoScaleDimensions = new System.Drawing.SizeF(9F, 20F);
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
            this.ClientSize = new System.Drawing.Size(484, 301);
            this.Controls.Add(this.labelShowInfo);
            this.Controls.Add(this.buttonActivate);
            this.Controls.Add(this.panel1);
            this.Icon = ((System.Drawing.Icon)(resources.GetObject("$this.Icon")));
            this.Name = "FormStart";
            this.Text = "ImageCutter";
            this.panel1.ResumeLayout(false);
            this.panel1.PerformLayout();
            this.ResumeLayout(false);

        }

        #endregion

        private System.Windows.Forms.TextBox textBoxKeyInput;
        private System.Windows.Forms.Label label1;
        private System.Windows.Forms.Panel panel1;
        private System.Windows.Forms.Label label3;
        private System.Windows.Forms.Label label2;
        private System.Windows.Forms.Button buttonActivate;
        private System.Windows.Forms.Label labelShowInfo;
    }
}