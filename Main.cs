using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Data.SqlClient;
using System.Drawing;
using System.Linq;
using System.Reflection;
using System.Threading.Tasks;
using System.Windows.Forms;
using ToodedAB.Properties;
using static System.Windows.Forms.VisualStyles.VisualStyleElement.StartPanel;

namespace ToodedAB
{
    public partial class Main : Form
    {
        TreeView tree;
        TreeNode tn = new TreeNode("Pood");
        SqlConnection connect = new SqlConnection(@"Data Source=(LocalDB)\MSSQLLocalDB;AttachDbFilename=|DataDirectory|\AppData\Tooded_DB.mdf;Integrated Security=True");
        SqlDataAdapter adapter_toode, adapter_kategooria;
        SqlCommand command;
        Panel p;
        TreeNode kassa;
        Pood pood;
        Cassa cassa;
        TreeViewEventArgs Selected;
        Dictionary<string, int> tooded = new Dictionary<string, int>();
        int value = 0;
        int x, i;
        public Main()
        {
            this.Width = 1200;
            this.Height = 900;
            this.Text = "VS Pood | Vihane Sipelgas";
            this.Icon = Properties.Resources.Icon;
            this.FormBorderStyle = FormBorderStyle.FixedSingle;
            this.MaximizeBox = false;
            this.MinimizeBox = false;
            tree = new TreeView();
            tree.Font = new Font("Arial", 16);
            tree.Dock = DockStyle.Left;
            tree.BorderStyle = BorderStyle.Fixed3D;
            tree.Width = 150;
            tree.AfterSelect +=Tree_AfterSelect;

            p = new Panel() {Size=new Size(1050,850),Location=new Point(tree.Right), BackColor = Color.White };
            p.AutoScroll = true;
            p.VerticalScroll.Visible = false;
            p.VerticalScroll.Enabled = false;
            p.Scroll += P_Scroll;
            typeof(Panel).InvokeMember("DoubleBuffered", BindingFlags.SetProperty
            | BindingFlags.Instance | BindingFlags.NonPublic, null,
            p, new object[] { true });
            /* 
        .InvokeMember("DoubleBuffered", ...) : Это вызов метода InvokeMember для объекта Type. 
            Этот метод позволяет вам вызывать методы, получать или устанавливать значения свойств объектов, используя рефлексию.
        SetProperty: Указывает, что мы хотим установить значение свойства.
        Instance: Указывает, что мы хотим получить доступ к экземпляру свойства.
        NonPublic: Указывает, что мы хотим получить доступ к неоткрытым членам.
        Мы передаем null, потому что свойство DoubleBuffered не является статическим, и мы хотим получить доступ к экземпляру свойства.
            */
            tree.Nodes.Add(tn);
            Controls.AddRange(new Control[] {tree,p });

            TeineNode();
            Kassa();
            NaitaKat();
        }

        private async void P_Scroll(object sender, ScrollEventArgs e)
        {
            value = p.HorizontalScroll.Value;
            await Task.Run(() => { 
                p.BackColor = Color.White ;
            });
        }

        private void Tree_AfterSelect(object sender, TreeViewEventArgs e)
        {
            if (e.Node.Text == "Kassa")
            {
                this.Hide();
                cassa = new Cassa();
                cassa.Closed += (s, args) => this.Close();
                cassa.Show();
            }
            else if (e.Node.Text == "Kodu")
            {
                this.Hide();
                pood = new Pood();
                pood.Closed += (s, args) => this.Close();
                pood.Show();
            }
            else if (true)
            {
                Tooded(e);
            }
            tree.SelectedNode = null;
        }

        private void Tooded(TreeViewEventArgs e)
        {
            connect.Open();
            adapter_kategooria = new SqlDataAdapter("SELECT Id, Kategooria_nimetus FROM Kategooria", connect);
            DataTable dt_kat = new DataTable();
            adapter_kategooria.Fill(dt_kat);
            connect.Close();
            foreach (DataRow item in dt_kat.Rows)
            {
                if (e.Node.Text == item["Kategooria_nimetus"].ToString())
                {
                    p.Controls.Clear();
                    int id = Convert.ToInt32(item["Id"]);
                    connect.Open();
                    adapter_toode = new SqlDataAdapter($"SELECT Toodenimetus, Kogus, Hind, Pilt FROM Toodetable WHERE Kategooriad = '{id}'", connect);
                    DataTable dt_tod = new DataTable();
                    adapter_toode.Fill(dt_tod);
                    connect.Close();
                    x = 55;
                    i = 0;

                    foreach (DataRow item1 in dt_tod.Rows)
                    {
                        UserControl btn = new UserControl() { Size = new Size(200, 300) };
                        if (i % 2 == 0)
                            btn.Location = new Point((i - 1) >= 0 ? x : 55, 55);
                        else
                        {
                            btn.Location = new Point(x, 400);
                            x += 255;
                        }

                        PictureBox pb = new PictureBox() { Size = new Size(150, 150), Location = new Point(20, 20), BackColor = Color.Gray };
                        Label nimi = new Label() { Size = new Size(140, 25), Location = new Point(50, 180), Text = item1["Toodenimetus"].ToString(), Font = new Font("Arial", 15) };
                        Label hind = new Label() { Size = new Size(140, 25), Location = new Point(50, 210), Text = "Hind: " + item1["Hind"].ToString() + "$", Font = new Font("Arial", 15) };
                        Label kogus = new Label() { Size = new Size(140, 25), Location = new Point(50, 240), Text = "Kogus: " + item1["Kogus"].ToString(), Font = new Font("Arial", 15) };
                        kogus.Name = "kogus";
                        btn.Name = nimi.Text;
                        btn.Controls.AddRange(new Control[] { pb, nimi, hind, kogus });

                        foreach (Control item2 in new Control[] { btn, pb, nimi, hind, kogus })
                        {
                            item2.MouseDown += C_Click;
                            item2.Tag = nimi.Text;
                        }
                        p.Controls.Add(btn);
                        i++;
                    }
                    Selected = e;
                }
            }
        }

        private async void C_Click(object sender, MouseEventArgs e)
        {
            if (!(sender is Control item)) return;
            foreach (Control item1 in p.Controls)
            {
                if (!(item1 is UserControl uc) || uc.Tag != item.Tag) continue;
                foreach (Control item2 in uc.Controls)
                {
                    if (!(item2 is Label kogus) || kogus.Name != "kogus") continue;
                    if (e.Button == MouseButtons.Left)
                    {
                        if (Convert.ToInt32(kogus.Text.Replace("Kogus: ", "")) > 0)
                        {
                            Properties.Settings.Default.Tooded += item.Tag + ",";
                            Properties.Settings.Default.Save();
                            await Task.Run(async() =>
                            {
                                uc.BackColor = Color.Green;
                                uc.ForeColor = Color.White;
                                await Task.Delay(100);
                                uc.BackColor = Color.White;
                                uc.ForeColor = Color.Black;
                            });
                            connect.Open();
                            command = new SqlCommand("UPDATE Toodetable SET Kogus=Kogus-1 WHERE Toodenimetus=@nimi", connect);
                            command.Parameters.AddWithValue("@nimi", item1.Tag);
                            command.ExecuteNonQuery();
                            connect.Close();
                        }
                        else
                            MessageBox.Show("See toode on otsas.", "Viga", MessageBoxButtons.OK, MessageBoxIcon.Error);
                    }
                    else if (e.Button == MouseButtons.Right)
                    {
                        string[] t = Properties.Settings.Default.Tooded.Split(',');
                        List<string> tooded = t.ToList();
                        if (tooded.Contains(item2.Tag))
                        {
                            tooded.Remove(item2.Tag.ToString());
                            Properties.Settings.Default.Tooded = string.Join(",", tooded);
                            Properties.Settings.Default.Save();
                            await Task.Run(async () =>
                            {
                                uc.BackColor = Color.Red;
                                uc.ForeColor = Color.White;
                                await Task.Delay(100);
                                uc.BackColor = Color.White;
                                uc.ForeColor = Color.Black;
                            });
                            connect.Open();
                            command = new SqlCommand("UPDATE Toodetable SET Kogus=Kogus+1 WHERE Toodenimetus=@nimi", connect);
                            command.Parameters.AddWithValue("@nimi", item1.Tag);
                            command.ExecuteNonQuery();
                            connect.Close();
                        }
                        else
                            MessageBox.Show("Te ei võtnud seda toodet.", "Viga", MessageBoxButtons.OK, MessageBoxIcon.Error);
                    }
                    Kassa();
                    Tooded(Selected);
                    p.HorizontalScroll.Value = value;
                }
            }
        }

        private void Kassa()
        {
            kassa.Nodes.Clear();
            tooded = new Dictionary<string, int>();
            List<string> temp = Properties.Settings.Default.Tooded.Split(',').ToList();
            temp.RemoveAt(temp.Count - 1);
            foreach (string item in temp)
            {
                if (tooded.ContainsKey(item))
                    tooded[item] += 1;
                else
                    tooded[item] = 1;
            }
            foreach (var item in tooded)
                if (item.Key.Length>1)
                    kassa.Nodes.Add(item.Key+" : "+item.Value);
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
            Selected = new TreeViewEventArgs(new TreeNode(dt_kat.Rows[0]["Kategooria_nimetus"].ToString()));
        }

        private void TeineNode()
        {
            kassa = new TreeNode("Kassa");
            tree.Nodes.Add(kassa);
            tree.Nodes.Add("Kodu");
        }
    }
}