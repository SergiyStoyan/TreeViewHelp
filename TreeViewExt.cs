//********************************************************************************************
//Author: Sergey Stoyan
//        sergey.stoyan@gmail.com
//        stoyan@cliversoft.com
//        http://www.cliversoft.com
//        26 September 2006
//
//********************************************************************************************

using System;
using System.Collections;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Text;
using System.Windows.Forms;

namespace Cliver
{
    public partial class TreeViewExt : TreeView
    {
		protected ArrayList selected_nodes = new ArrayList();
        
		/// <summary>
		/// Selected nodes.
		/// </summary>
        public TreeNode[] SelectedNodes
        {
            get
            {
                return (TreeNode[])selected_nodes.ToArray(typeof(TreeNode));
            }
            set
            {
                empty_selections();  
                if(value != null)
                    selected_nodes.SetRange(0, value);
                foreach (TreeNode tn in selected_nodes)
                    paint_selected(tn, true);
            }
        }

        protected override void OnBeforeSelect(TreeViewCancelEventArgs e)
        {
            if (!enable_selection)
                e.Cancel = true;
            else
            {
            }
        }

        //protected override void OnAfterSelect(TreeViewEventArgs e)
        //{
        //}

        protected override void OnMouseDown(MouseEventArgs e)
        {
            TreeNode selected_node = this.GetNodeAt(e.X, e.Y);
            
            select_node(selected_node);
            
            base.OnMouseDown(e);
        }

        internal void select_node(TreeNode selected_node)
        {
            if(this.LabelEdit)
                this.LabelEdit = false;  

            if (ModifierKeys == Keys.Control)
            {
                if (selected_node == null)
                    return;

                if (selected_nodes.Count > 0 && ((TreeNode)selected_nodes[0]).Parent != selected_node.Parent)
                    return;

                if (selected_nodes.Contains(selected_node))
                {
                    select_node(selected_node, false);
                    return;
                }

                select_node(selected_node, true);
                return;
            }
            else if (ModifierKeys == Keys.Shift)
            {
                if (selected_node == null)
                    return;

                if (selected_nodes.Count == 0)
                {
                    select_node(selected_node, true);
                    return;
                }
                else
                {
                    if (((TreeNode)selected_nodes[0]).Parent != selected_node.Parent)
                        return;

                    TreeNode tn0 = (TreeNode)selected_nodes[0];
                    empty_selections();

                    bool forward = false;
                    for (TreeNode tn = tn0; tn != null; tn = tn.NextNode)
                        if (tn == selected_node)
                        {
                            forward = true;
                            break;
                        }

                    if (forward)
                        for (TreeNode tn = tn0; tn != null; tn = tn.NextNode)
                            select_node(tn, true);
                    else
                        for (TreeNode tn = tn0; tn != null; tn = tn.PrevNode)
                            select_node(tn, true);

                    return;
                }
            }
            else
            //if (e.Action == TreeViewAction.ByKeyboard
            //|| e.Action == TreeViewAction.ByMouse
            //)
            {
                if (selected_node == null)
                {
                    empty_selections();
                    return;
                }

                if (selected_nodes.Contains(selected_node))
                {
                    this.LabelEdit = true; 
                    selected_node.BeginEdit();
                }
                else
                {
                    empty_selections();
                    select_node(selected_node, true);
                    return;
                }
            }
        }
        bool enable_selection = false;

        void empty_selections()
        {
            while(selected_nodes.Count > 0)
                select_node((TreeNode)selected_nodes[0], false);
        }

        void select_node(TreeNode tn, bool selected)
        {
            if (selected)
            {
                TreeViewCancelEventArgs tvcea = new TreeViewCancelEventArgs(tn, false, TreeViewAction.ByMouse);
                base.OnBeforeSelect(tvcea);
                if (tvcea.Cancel)
                    return;

                selected_nodes.Add(tn);
                enable_selection = true;
                SelectedNode = tn;

                //TreeViewEventArgs tvea = new TreeViewEventArgs(tn, TreeViewAction.ByMouse);
                //base.OnAfterSelect(tvea);
            }
            else
            {
                selected_nodes.Remove(tn);
                enable_selection = false;
                SelectedNode = null;
            }
            paint_selected(tn, selected);
        }

        void paint_selected(TreeNode tn, bool selected)
        {
            if (selected)
            {
                if (tn.BackColor != SystemColors.Highlight)
                {
                    node_back_colors[tn] = tn.BackColor;
                    node_fore_colors[tn] = tn.ForeColor;
                }
                tn.BackColor = SystemColors.Highlight;
                tn.ForeColor = SystemColors.HighlightText;
            }
            else
            {
                tn.BackColor = (Color)node_back_colors[tn];
                tn.ForeColor = (Color)node_fore_colors[tn];
            }
        }

        Hashtable node_back_colors = new Hashtable();
        Hashtable node_fore_colors = new Hashtable();
    }
}
