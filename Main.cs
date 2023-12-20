using Aspose.Pdf.Operators;
using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Data.SqlClient;
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
        SqlConnection connect = new SqlConnection(@"Data Source=(LocalDB)\MSSQLLocalDB;AttachDbFilename=|DataDirectory|\AppData\Tooded_DB.mdf;Integrated Security=True");
        SqlDataAdapter adapter_toode, adapter_kategooria;
        Button btn1,btn2,btn3,btn4;
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

            NaitaKat();

            btn1 = new Button() { Size=new Size(200,300), Location = new Point(tree.Right+55,55)};
            btn2 = new Button() { Size=new Size(200, 300), Location = new Point(btn1.Right+55, 55) };
            btn3 = new Button() { Size=new Size(200, 300), Location = new Point(btn2.Right+55, 55) };
            btn4 = new Button() { Size=new Size(200, 300), Location = new Point(btn3.Right+55, 55) };

            tree.Nodes.Add(tn);
            Controls.AddRange(new Control[] {tree,btn1,btn2,btn3,btn4 });
        }

        private void Tree_AfterSelect(object sender, TreeViewEventArgs e)
        {
            connect.Open();
            adapter_kategooria = new SqlDataAdapter("SELECT Id,Kategooria_nimetus FROM Kategooria", connect);
            DataTable dt_kat = new DataTable();
            adapter_kategooria.Fill(dt_kat);
            foreach (DataRow item in dt_kat.Rows)
            {
                if (e.Node.Text == item["Kategooria_nimetus"].ToString())
                {
                    int id = Convert.ToInt32(item["Id"]);
                    adapter_toode = new SqlDataAdapter($"SELECT Toodenimetus, Kogus, Hind, Pilt, Kategooriad WHERE Kategooriad = '{id}'", connect);
                    DataTable dt_tod = new DataTable();
                    adapter_toode.Fill(dt_tod);
                    foreach (DataRow item1 in dt_tod.Rows)
                    {
                        
                    }
                }
                tree.SelectedNode = null;
            }
            connect.Close();
        }

        private void NaitaKat()
        {
            connect.Open();
            adapter_kategooria = new SqlDataAdapter("SELECT Kategooria_nimetus FROM Kategooria", connect);
            DataTable dt_kat = new DataTable();
            adapter_kategooria.Fill(dt_kat);
            foreach (DataRow item in dt_kat.Rows)
            {
                tn.Nodes.Add(item["Kategooria_nimetus"].ToString());
            }
            connect.Close();
        }
    }
}
