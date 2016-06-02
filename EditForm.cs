//********************************************************************************************
//Author: Sergey Stoyan
//        sergey.stoyan@gmail.com
//        stoyan@cliversoft.com
//        http://www.cliversoft.com
//        26 September 2006
//
//********************************************************************************************
using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Text;
using System.Windows.Forms;

namespace Cliver
{
    internal partial class EditForm : Form
    {
        internal EditForm(TreeNode tn)
        {
            InitializeComponent();

            TN = tn;
            TocItem ti = (TocItem)TN.Tag;
            File.Text = ti.File;
            Comment.Text = ti.Comment;
            IsNewEntry.Checked = ti.IsNewEntry;

            ImageIndex.Text = TN.ImageIndex.ToString();
            SelectedImageIndex.Text = TN.SelectedImageIndex.ToString();
            Text_.Text = TN.Text;
        }

        TreeNode TN = null;

        private void Cancel_Click(object sender, EventArgs e)
        {
            Close();
        }

        private void Ok_Click(object sender, EventArgs e)
        {
            TocItem ti = (TocItem)TN.Tag;
            ti.File = File.Text;
            ti.Comment = Comment.Text;
            ti.IsNewEntry = IsNewEntry.Checked;

            TN.ImageIndex = int.Parse(ImageIndex.Text);
            TN.SelectedImageIndex = int.Parse(SelectedImageIndex.Text);
            TN.Text = Text_.Text;
            TN.Tag = ti;

            Close();
        }
    }
}