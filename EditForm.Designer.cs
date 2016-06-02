namespace Cliver
{
    partial class EditForm
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
            this.label1 = new System.Windows.Forms.Label();
            this.File = new System.Windows.Forms.TextBox();
            this.Comment = new System.Windows.Forms.TextBox();
            this.label2 = new System.Windows.Forms.Label();
            this.ImageIndex = new System.Windows.Forms.TextBox();
            this.label4 = new System.Windows.Forms.Label();
            this.Text_ = new System.Windows.Forms.TextBox();
            this.label5 = new System.Windows.Forms.Label();
            this.SelectedImageIndex = new System.Windows.Forms.TextBox();
            this.label6 = new System.Windows.Forms.Label();
            this.IsNewEntry = new System.Windows.Forms.CheckBox();
            this.Cancel = new System.Windows.Forms.Button();
            this.Ok = new System.Windows.Forms.Button();
            this.SuspendLayout();
            // 
            // label1
            // 
            this.label1.AutoSize = true;
            this.label1.Location = new System.Drawing.Point(25, 30);
            this.label1.Name = "label1";
            this.label1.Size = new System.Drawing.Size(26, 13);
            this.label1.TabIndex = 0;
            this.label1.Text = "File:";
            // 
            // File
            // 
            this.File.Location = new System.Drawing.Point(140, 30);
            this.File.Name = "File";
            this.File.Size = new System.Drawing.Size(532, 20);
            this.File.TabIndex = 1;
            // 
            // Comment
            // 
            this.Comment.Location = new System.Drawing.Point(140, 71);
            this.Comment.Name = "Comment";
            this.Comment.Size = new System.Drawing.Size(532, 20);
            this.Comment.TabIndex = 3;
            // 
            // label2
            // 
            this.label2.AutoSize = true;
            this.label2.Location = new System.Drawing.Point(25, 71);
            this.label2.Name = "label2";
            this.label2.Size = new System.Drawing.Size(54, 13);
            this.label2.TabIndex = 2;
            this.label2.Text = "Comment:";
            // 
            // ImageIndex
            // 
            this.ImageIndex.Location = new System.Drawing.Point(140, 152);
            this.ImageIndex.Name = "ImageIndex";
            this.ImageIndex.Size = new System.Drawing.Size(100, 20);
            this.ImageIndex.TabIndex = 7;
            // 
            // label4
            // 
            this.label4.AutoSize = true;
            this.label4.Location = new System.Drawing.Point(25, 152);
            this.label4.Name = "label4";
            this.label4.Size = new System.Drawing.Size(65, 13);
            this.label4.TabIndex = 6;
            this.label4.Text = "ImageIndex:";
            // 
            // Text_
            // 
            this.Text_.Location = new System.Drawing.Point(140, 112);
            this.Text_.Name = "Text_";
            this.Text_.Size = new System.Drawing.Size(100, 20);
            this.Text_.TabIndex = 9;
            // 
            // label5
            // 
            this.label5.AutoSize = true;
            this.label5.Location = new System.Drawing.Point(25, 112);
            this.label5.Name = "label5";
            this.label5.Size = new System.Drawing.Size(31, 13);
            this.label5.TabIndex = 8;
            this.label5.Text = "Text:";
            // 
            // SelectedImageIndex
            // 
            this.SelectedImageIndex.Location = new System.Drawing.Point(140, 195);
            this.SelectedImageIndex.Name = "SelectedImageIndex";
            this.SelectedImageIndex.Size = new System.Drawing.Size(100, 20);
            this.SelectedImageIndex.TabIndex = 11;
            // 
            // label6
            // 
            this.label6.AutoSize = true;
            this.label6.Location = new System.Drawing.Point(25, 195);
            this.label6.Name = "label6";
            this.label6.Size = new System.Drawing.Size(107, 13);
            this.label6.TabIndex = 10;
            this.label6.Text = "SelectedImageIndex:";
            // 
            // IsNewEntry
            // 
            this.IsNewEntry.AutoSize = true;
            this.IsNewEntry.Location = new System.Drawing.Point(28, 246);
            this.IsNewEntry.Name = "IsNewEntry";
            this.IsNewEntry.Size = new System.Drawing.Size(80, 17);
            this.IsNewEntry.TabIndex = 12;
            this.IsNewEntry.Text = "IsNewEntry";
            this.IsNewEntry.UseVisualStyleBackColor = true;
            // 
            // Cancel
            // 
            this.Cancel.Location = new System.Drawing.Point(597, 275);
            this.Cancel.Name = "Cancel";
            this.Cancel.Size = new System.Drawing.Size(75, 23);
            this.Cancel.TabIndex = 13;
            this.Cancel.Text = "Cancel";
            this.Cancel.UseVisualStyleBackColor = true;
            this.Cancel.Click += new System.EventHandler(this.Cancel_Click);
            // 
            // Ok
            // 
            this.Ok.Location = new System.Drawing.Point(501, 275);
            this.Ok.Name = "Ok";
            this.Ok.Size = new System.Drawing.Size(75, 23);
            this.Ok.TabIndex = 14;
            this.Ok.Text = "OK";
            this.Ok.UseVisualStyleBackColor = true;
            this.Ok.Click += new System.EventHandler(this.Ok_Click);
            // 
            // EditForm
            // 
            this.AutoScaleDimensions = new System.Drawing.SizeF(6F, 13F);
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
            this.ClientSize = new System.Drawing.Size(684, 311);
            this.Controls.Add(this.Ok);
            this.Controls.Add(this.Cancel);
            this.Controls.Add(this.IsNewEntry);
            this.Controls.Add(this.SelectedImageIndex);
            this.Controls.Add(this.label6);
            this.Controls.Add(this.Text_);
            this.Controls.Add(this.label5);
            this.Controls.Add(this.ImageIndex);
            this.Controls.Add(this.label4);
            this.Controls.Add(this.Comment);
            this.Controls.Add(this.label2);
            this.Controls.Add(this.File);
            this.Controls.Add(this.label1);
            this.FormBorderStyle = System.Windows.Forms.FormBorderStyle.FixedDialog;
            this.MaximizeBox = false;
            this.MinimizeBox = false;
            this.Name = "EditForm";
            this.ShowIcon = false;
            this.ShowInTaskbar = false;
            this.Text = "Edit Node";
            this.ResumeLayout(false);
            this.PerformLayout();

        }

        #endregion

        private System.Windows.Forms.Label label1;
        private System.Windows.Forms.TextBox File;
        private System.Windows.Forms.TextBox Comment;
        private System.Windows.Forms.Label label2;
        private System.Windows.Forms.TextBox ImageIndex;
        private System.Windows.Forms.Label label4;
        private System.Windows.Forms.TextBox Text_;
        private System.Windows.Forms.Label label5;
        private System.Windows.Forms.TextBox SelectedImageIndex;
        private System.Windows.Forms.Label label6;
        private System.Windows.Forms.CheckBox IsNewEntry;
        private System.Windows.Forms.Button Cancel;
        private System.Windows.Forms.Button Ok;
    }
}