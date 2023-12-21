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
        Panel p;
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

            p = new Panel() {Size=new Size(1100,850),Location=new Point(tree.Right), BackgroundImage = Properties.Resources.bg };
            p.AutoScroll = true;
            p.VerticalScroll.Visible = false;
            p.VerticalScroll.Enabled = false;
            p.Scroll += P_Scroll;
            tree.Nodes.Add(tn);
            Controls.AddRange(new Control[] {tree,p });

            NaitaKat();
        }

        private async void P_Scroll(object sender, ScrollEventArgs e)
        {
            await Task.Run(() => { p.BackgroundImage = Properties.Resources.bg; });
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
                    p.Controls.Clear();
                    int id = Convert.ToInt32(item["Id"]);
                    adapter_toode = new SqlDataAdapter($"SELECT Toodenimetus, Kogus, Hind, Pilt FROM Toodetable WHERE Kategooriad = '{id}'", connect);
                    DataTable dt_tod = new DataTable();
                    adapter_toode.Fill(dt_tod);
                    int x = 55;
                    int i = 0;
                    foreach (DataRow item1 in dt_tod.Rows)
                    {
                        Button btn = new Button() { Size = new Size(200, 300) };
                        if (i % 2 == 0)
                        {
                            btn.Location = new Point((i - 1) >= 0 ? x : 55, 55);
                        }
                        else
                        {
                            btn.Location = new Point(x, 400);
                            x += 255;
                        }
                        PictureBox pb = new PictureBox() { Size = new Size(150, 150), Location = new Point(20, 20), BackColor = Color.Gray };
                        Label nimi = new Label() { Size = new Size(140, 25), Location = new Point(50, 180), Text = item1["Toodenimetus"].ToString(), Font = new Font("Arial", 15) };
                        Label hind = new Label() { Size = new Size(140, 25), Location = new Point(50, 210), Text = "Hind: " +item1["Hind"].ToString()+"$", Font = new Font("Arial", 15) };
                        Label kogus = new Label() { Size = new Size(140, 25), Location = new Point(50, 240), Text = "Kogus: " + item1["Kogus"].ToString(), Font = new Font("Arial", 15) };
                        btn.Controls.AddRange(new Control[] {pb,nimi,hind,kogus});
                        p.Controls.Add(btn);
                        i++;
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
            Tree_AfterSelect(new object(), new TreeViewEventArgs(new TreeNode(dt_kat.Rows[0]["Kategooria_nimetus"].ToString())));
        }
    }
}
