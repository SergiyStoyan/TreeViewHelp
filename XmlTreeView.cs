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
using System.Text;
using System.IO;
using System.Web;
using System.Net;
using System.Text.RegularExpressions;
using System.Xml;
using System.Windows.Forms;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Reflection;

namespace Cliver
{
    public partial class TreeViewForm : Form
    {
        #region Events
        public event OnNodeAddHandler NodeAdd;
        public delegate void OnNodeAddHandler();
        public event OnNodeAddedHandler NodeAdded;
        public delegate void OnNodeAddedHandler(TreeNode tree_node);
        public event OnNodeRemovedHandler NodeRemoved;
        public delegate void OnNodeRemovedHandler(TreeNode tree_node);
        public event OnNodeRenamedHandler NodeRenamed;
        public delegate void OnNodeRenamedHandler(TreeNode tree_node);
        public event OnNodePositionChangedHandler NodePositionChanged;
        public delegate void OnNodePositionChangedHandler(TreeNode tree_node);
        public event OnNodeClickHandler NodeClick;
        public delegate void OnNodeClickHandler(TreeNode tree_node);
        #endregion

        public TreeViewForm()
        {
            InitializeComponent();

            ImageList TreeviewIL = new ImageList();
            TreeviewIL.Images.Add(System.Drawing.Image.FromStream(Assembly.GetExecutingAssembly().GetManifestResourceStream("Cliver.Resources.closed.gif")));
            TreeviewIL.Images.Add(System.Drawing.Image.FromStream(Assembly.GetExecutingAssembly().GetManifestResourceStream("Cliver.Resources.open.gif")));
            TreeviewIL.Images.Add(System.Drawing.Image.FromStream(Assembly.GetExecutingAssembly().GetManifestResourceStream("Cliver.Resources.list.gif")));
            this.tree_view.ImageList = TreeviewIL;
            this.tree_view.HideSelection = false;
            this.tree_view.ItemHeight = this.tree_view.ItemHeight + 3;
            this.tree_view.Indent = this.tree_view.Indent + 3;
        }

        private void TreeViewForm_Load(object sender, EventArgs e)
        {
            //xml_file = "test.xml";
            //load_file();
        }

        #region Save tree to a file
        private void bSave_Click(object sender, EventArgs e)
        {
            XmlDocument xd = new XmlDocument();
            xd.AppendChild(xd.CreateXmlDeclaration("1.0", "utf-8", null));

            XmlElement xe0 = xd.CreateElement("TreeViewData");
            xd.AppendChild(xe0);

            foreach (TreeNode tn in tree_view.Nodes)
            {
                write_tree_node(tn, xe0, xd);
            }

            if (xml_file == null)
            {
                SaveFileDialog d = new SaveFileDialog();
                d.DefaultExt = "xml";
                d.Title = "Specify a file";
                d.Filter = "XML (*.xml)|*.xml|All files (*.*)|*.*";
                if (d.ShowDialog(this) != DialogResult.OK || string.IsNullOrEmpty(d.FileName))
                    return;
                xml_file = d.FileName;
            }
            xd.Save(xml_file);
        }

        void write_tree_node(TreeNode tn, XmlNode parent_xn, XmlDocument xd)
        {
            XmlNode xn = add_node2xml(tn, parent_xn, xd);
            foreach (TreeNode n in tn.Nodes)
                write_tree_node(n, xn, xd);
        }

        XmlNode add_node2xml(TreeNode tn, XmlNode parent_xn, XmlDocument xd)
        {
            XmlElement xe = xd.CreateElement("Node");
            parent_xn.AppendChild(xe);

            TocItem ti = (TocItem)tn.Tag;
            xe.SetAttribute("Text", tn.Text);
            xe.SetAttribute("ImageIndex", tn.ImageIndex.ToString());
            xe.SetAttribute("SelectedImageIndex", tn.SelectedImageIndex.ToString());
            xe.SetAttribute("Checked", tn.Checked.ToString());
            xe.SetAttribute("Expanded", tn.IsExpanded.ToString());
            xe.SetAttribute("Comment", ti.Comment);
            xe.SetAttribute("File", ti.File);
            xe.SetAttribute("IsNewEntry", ti.IsNewEntry.ToString());

            return xe;
        }
        #endregion

        #region Load file to the tree view
        private void bOpen_Click(object sender, EventArgs e)
        {
            OpenFileDialog d = new OpenFileDialog();
            d.Title = "Pick a file";
            d.Filter = "XML (*.xml)|*.xml|All files (*.*)|*.*";
            if (d.ShowDialog(this) != DialogResult.OK || string.IsNullOrEmpty(d.FileName))
                return;
            xml_file = d.FileName;
            this.Text = xml_file;
            load_file();
        }

        void load_file()
        {
            XmlDocument x = new XmlDocument();
            x.Load(xml_file);

            tree_view.Nodes.Clear();

            foreach (XmlNode xn in x.SelectNodes("TreeViewData/Node"))
            {
                load_tree_node(xn, null);
            }
        }

        string xml_file;

        void load_tree_node(XmlNode xn, TreeNode parent_tn)
        {
            TreeNode tn = add_node(xn, parent_tn);
            foreach (XmlNode n in xn.SelectNodes("Node"))
                load_tree_node(n, tn);
        }

        TreeNode add_node(XmlNode xn, TreeNode parent_tn)
        {
            //TreeNode tn = new TreeNode(xn.Attributes["Text"].Value, int.Parse(xn.Attributes["ImageIndex"].Value), int.Parse(xn.Attributes["SelectedImageIndex"].Value));
            int image_i = 0;
            if (xn.ChildNodes.Count == 0)
                image_i = 2;
            //else
            //    if (bool.Parse(xn.Attributes["Expanded"].Value))
            //        image_i = 1;

            TreeNode tn = new TreeNode(xn.Attributes["Text"].Value, image_i, image_i);
            TocItem ti = new TocItem();
            ti.Comment = xn.Attributes["Comment"].Value;
            ti.File = xn.Attributes["File"].Value;
            ti.IsNewEntry = bool.Parse(xn.Attributes["IsNewEntry"].Value);
            tn.Tag = ti;
            if (parent_tn == null)
                tree_view.Nodes.Add(tn);
            else
                parent_tn.Nodes.Add(tn);

            if (bool.Parse(xn.Attributes["Expanded"].Value))
                tn.Expand();

            tn.Checked = bool.Parse(xn.Attributes["Checked"].Value);

            return tn;
        }
        #endregion

        #region Drag&Drop

        private void tree_view_DragEnter(object sender, DragEventArgs e)
        {
            if (e.Data.GetDataPresent("System.Windows.Forms.TreeNode", false)
                || e.Data.GetDataPresent(DataFormats.FileDrop, false)
                )
                e.Effect = DragDropEffects.Move;
        }

        private void tree_view_ItemDrag(object sender, ItemDragEventArgs e)
        {
            if(tree_view.SelectedNodes.Length > 0)
                tree_view.DoDragDrop(e.Item, DragDropEffects.Move);
        }

        TreeNode under_node = null;

        private void tree_view_DragOver(object sender, DragEventArgs e)
        {
            TreeNode under_node2 = this.tree_view.GetNodeAt(this.tree_view.PointToClient(new Point(e.X, e.Y)));
            if (under_node2 == null)
            {
                if (under_node != under_node2)
                {
                    under_node = null;
                    this.tree_view.Refresh();
                }
                return;
            }

            under_node = under_node2;

            foreach (TreeNode moving_node in tree_view.SelectedNodes)
            {
                if (under_node == moving_node || is_parent_branch(moving_node, under_node))
                {
                    under_node = null;
                    this.tree_view.Refresh();
                    return;
                }
            }

            if (!under_node.IsExpanded)
                under_node.Expand();

            Graphics g = this.tree_view.CreateGraphics();

            int offset_Y = this.tree_view.PointToClient(Cursor.Position).Y - under_node.Bounds.Y;
            if (offset_Y < (under_node.Bounds.Height / 3))
                draw_insert_between_nodes_pointer(under_node);
            else
                draw_add_as_child_pointer(under_node);
        }

        bool is_parent_branch(TreeNode tn, TreeNode tn2)
        {
            if (tn2.Parent == null)
                return false;
            if(tn2.Parent == tn)
                return true;
            return is_parent_branch(tn, tn2.Parent);
        }

        private void tree_view_DragDrop(object sender, DragEventArgs e)
        {
            if (under_node == null)
                return;

            if (e.Data.GetDataPresent("System.Windows.Forms.TreeNode", false))
            {                
                foreach (TreeNode moving_node in tree_view.SelectedNodes)
                {
                    if (moving_node.Parent != null && moving_node.Parent.Nodes.Count < 2)
                    {
                        moving_node.Parent.ImageIndex = 2;
                        moving_node.SelectedImageIndex = 2;
                    }

                    moving_node.Remove();
                    if (insert_between_nodes_pointer)
                    {
                        if (under_node.Parent == null)
                            tree_view.Nodes.Insert(under_node.Index, moving_node);
                        else
                            under_node.Parent.Nodes.Insert(under_node.Index, moving_node);
                    }
                    else
                    {
                        under_node.Nodes.Add(moving_node);
                        under_node.ImageIndex = 1;
                        under_node.SelectedImageIndex = 1;
                        under_node.Expand();
                    }

                    if (NodePositionChanged != null)
                        NodePositionChanged.Invoke(moving_node);
                }
            }
            else if (e.Data.GetDataPresent(DataFormats.FileDrop, false))
            {
                string[] files = (string[])e.Data.GetData(DataFormats.FileDrop);
                foreach (string file in files)
                {
                    TreeNode inserted_node;

                    if (insert_between_nodes_pointer)
                    {
                        if (under_node.Parent == null)
                            inserted_node = add_node(file, tree_view.Nodes, under_node.Index);
                        else
                            inserted_node = add_node(file, under_node.Parent.Nodes, under_node.Index);
                    }
                    else
                    {
                        inserted_node = add_node(file, under_node.Nodes, 0);
                        under_node.ImageIndex = 1;
                        under_node.SelectedImageIndex = 1;
                        under_node.Expand();
                    }

                    if (NodeAdded != null)
                        NodeAdded.Invoke(inserted_node);
                }
            }
            this.tree_view.Refresh();
        }

        private void draw_insert_between_nodes_pointer(TreeNode under_node)
        {
            if (under_node == null)
                return;
            if (insert_between_nodes_pointer)
                return;
            insert_between_nodes_pointer = true;
            this.tree_view.Refresh();

            Graphics g = this.tree_view.CreateGraphics();

            int pointer_left = under_node.Bounds.Left - this.tree_view.ImageList.Images[under_node.ImageIndex].Size.Width + 8;
            int pointer_right = this.tree_view.Width - 4;

            Point[] left_triangle = new Point[3]{
                new Point(pointer_left + 5, under_node.Bounds.Top - 5),
                new Point(pointer_left + 5, under_node.Bounds.Top + 5),
                new Point(pointer_left, under_node.Bounds.Top),
            };

            g.FillPolygon(System.Drawing.Brushes.Red, left_triangle);
            g.DrawLine(new System.Drawing.Pen(Color.Red, 1), new Point(pointer_left, under_node.Bounds.Top), new Point(pointer_right, under_node.Bounds.Top));
        }

        bool insert_between_nodes_pointer = true;

        private void draw_add_as_child_pointer(TreeNode under_node)
        {
            if (under_node == null)
                return;
            if (!insert_between_nodes_pointer)
                return;
            insert_between_nodes_pointer = false;
            this.tree_view.Refresh();

            Graphics g = this.tree_view.CreateGraphics();
            int pointer_X = under_node.Bounds.Right + 6;
            int pointer_center_Y = under_node.Bounds.Y + (under_node.Bounds.Height / 2);
            Point[] pointer_p = new Point[3]{
                new Point(pointer_X, pointer_center_Y + 5),
                new Point(pointer_X - 4, pointer_center_Y),
                new Point(pointer_X, pointer_center_Y - 5)
            };

            g.FillPolygon(System.Drawing.Brushes.Red, pointer_p);
        }

        #endregion

        #region Creating new node
        private void addNewNodeToolStripMenuItem_Click(object sender, EventArgs e)
        {
            SaveFileDialog d = new SaveFileDialog();
            d.Title = "Save a new file";
            if (d.ShowDialog(this) != DialogResult.OK || string.IsNullOrEmpty(d.FileName))
                return;

            File.WriteAllText(d.FileName, "");

            if (tree_view.SelectedNode == null)
                add_node(d.FileName, tree_view.Nodes, -1);
            else
                add_node(d.FileName, tree_view.SelectedNode.Nodes, -1);
        }

        TreeNode add_node(string file, TreeNodeCollection tnc, int i)
        {
            if (NodeAdd != null)
                NodeAdd.Invoke();

            TreeNode tn = new TreeNode(Path.GetFileName(file), 2, 2);
            TocItem ti = new TocItem();
            ti.File = file;
            tn.Tag = ti;         
            if (i < 0)
                tnc.Add(tn);
            else
                tnc.Insert(i, tn);

            if (tn.Parent != null)
            {
                if (tn.Parent.IsExpanded)
                {
                    tn.Parent.ImageIndex = 1;
                    tn.Parent.SelectedImageIndex = 1;
                }
                else
                    tn.Parent.Expand();
            }

            if (NodeAdded != null)
                NodeAdded.Invoke(tn);

            return tn;
        }

        private void addToolStripMenuItem_Click(object sender, EventArgs e)
        {
            OpenFileDialog d = new OpenFileDialog();
            d.Title = "Pick a file";
            //d.Filter = "XML (*.xml)|*.xml|All files (*.*)|*.*";
            if (d.ShowDialog(this) != DialogResult.OK || string.IsNullOrEmpty(d.FileName))
                return;

            if (tree_view.SelectedNode == null)
                add_node(d.FileName, tree_view.Nodes, -1);
            else
                add_node(d.FileName, tree_view.SelectedNode.Nodes, -1);
        }
        #endregion

        #region Handlers

        private void tree_view_NodeMouseClick(object sender, TreeNodeMouseClickEventArgs e)
        {
        }

        private void tree_view_BeforeLabelEdit(object sender, NodeLabelEditEventArgs e)
        {
            if (tree_view.SelectedNode != e.Node)
                e.CancelEdit = true;
        }

        private void propertiesToolStripMenuItem_Click(object sender, EventArgs e)
        {
            if (tree_view.SelectedNode == null)
            {
                MessageBox.Show("Select a node");
                return;
            }
            EditForm ef = new EditForm(tree_view.SelectedNode);
            ef.ShowDialog(this);
        }

        private void removeToolStripMenuItem_Click(object sender, EventArgs e)
        {
            if (tree_view.SelectedNode == null)
            {
                MessageBox.Show("Select a node");
                return;
            }

            TreeNode tn = tree_view.SelectedNode;
            tn.Remove();

            if (NodeRemoved != null)
                NodeRemoved.Invoke(tn);
        }

        private void tree_view_AfterLabelEdit(object sender, NodeLabelEditEventArgs e)
        {
            if (NodeRenamed != null)
                NodeRenamed.Invoke(e.Node);
        }

        private void tree_view_MouseClick(object sender, MouseEventArgs e)
        {
            TreeNode tn = tree_view.GetNodeAt(e.X, e.Y);
            if (tn == null)
                return;

            if (NodeClick != null)
                NodeClick.Invoke(tn);
        }

        private void commentToolStripMenuItem_Click(object sender, EventArgs e)
        {
            propertiesToolStripMenuItem_Click(null, null);
        }

        private void editToolStripMenuItem_Click(object sender, EventArgs e)
        {
            propertiesToolStripMenuItem_Click(null, null);
        }

        private void tree_view_BeforeSelect(object sender, TreeViewCancelEventArgs e)
        {
            if (e.Action == TreeViewAction.Unknown)
                e.Cancel = true;
        }

        private void tree_view_AfterSelect(object sender, TreeViewEventArgs e)
        {
            TocItem ti = (TocItem)e.Node.Tag;
            browser.Navigate(ti.File);
        }

        private void tree_view_AfterExpand(object sender, TreeViewEventArgs e)
        {
            e.Node.ImageIndex = 1;
            e.Node.SelectedImageIndex = 1;
        }

        private void tree_view_AfterCollapse(object sender, TreeViewEventArgs e)
        {
            e.Node.ImageIndex = 0;
            e.Node.SelectedImageIndex = 0;
        }
        #endregion

        #region Moving node
        private void upToolStripMenuItem_Click(object sender, EventArgs e)
        {
            if (tree_view.SelectedNode == null)
            {
                MessageBox.Show("Select a node");
                return;
            }

            TreeNode tn = tree_view.SelectedNode;
            TreeNodeCollection tnc = tn.Parent == null ? tn.TreeView.Nodes : tn.Parent.Nodes;
            if (tn.Index > 0)
            {
                int i = tn.Index - 1;
                tn.Remove();
                tnc.Insert(i, tn);
            }
        }

        private void downToolStripMenuItem_Click(object sender, EventArgs e)
        {
            if (tree_view.SelectedNode == null)
            {
                MessageBox.Show("Select a node");
                return;
            }

            TreeNode tn = tree_view.SelectedNode;
            TreeNodeCollection tnc = tn.Parent == null ? tn.TreeView.Nodes : tn.Parent.Nodes;
            if (tn.Index < tnc.Count - 1)
            {
                int i = tn.Index + 1;
                tn.Remove();
                tnc.Insert(i, tn);
            }
        }

        private void leftToolStripMenuItem_Click(object sender, EventArgs e)
        {
            if (tree_view.SelectedNode == null)
            {
                MessageBox.Show("Select a node");
                return;
            }

            TreeNode tn = tree_view.SelectedNode;
            if (tn.Parent != null)
            {
                TreeNodeCollection tnc = tn.Parent.Parent == null ? tn.Parent.TreeView.Nodes : tn.Parent.Parent.Nodes;
                int i = tn.Parent.Index + 1;
                tn.Remove();
                tnc.Insert(i, tn);
            }
        }

        private void rightToolStripMenuItem_Click(object sender, EventArgs e)
        {
            if (tree_view.SelectedNode == null)
            {
                MessageBox.Show("Select a node");
                return;
            }

            TreeNode tn = tree_view.SelectedNode;
            if (tn.NextNode != null)
            {
                TreeNodeCollection tnc = tn.NextNode.Nodes;
                tn.Remove();
                tnc.Insert(0, tn);
            }
        }
        #endregion
    }
}