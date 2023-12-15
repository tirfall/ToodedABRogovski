using Aspose.Pdf.Operators;
using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;

namespace ToodedAB
{
    public partial class Main : Form
    {
        TreeView tree;
        TreeNode tn = new TreeNode("Pood");
        public Main()
        {
            this.Width = 1200;
            this.Height = 900;
            this.Text = "VS Pood | Vihane Sipelgas";
            this.BackgroundImage = Properties.Resources.bg;
            this.Icon = Properties.Resources.Icon;

            tree = new TreeView();
            tree.Dock = DockStyle.Left;
            tree.BorderStyle = BorderStyle.Fixed3D;
            tree.AfterSelect +=Tree_AfterSelect;

            


            tree.Nodes.Add(tn);
            Controls.AddRange(new Control[] {tree });
        }

        private void Tree_AfterSelect(object sender, TreeViewEventArgs e)
        {
            
        }
    }
}
