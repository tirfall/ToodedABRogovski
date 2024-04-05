using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Data.SqlClient;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Threading;
using System.Windows.Forms;
using System.Diagnostics;
using System.IO;

namespace ToodedAB
{
    public partial class ToodedAB : Form
    {

        SqlConnection connect = new SqlConnection(@"Data Source=(LocalDB)\MSSQLLocalDB;AttachDbFilename=|DataDirectory|\AppData\Tooded_DB.mdf;Integrated Security=True");
        SqlDataAdapter adapter_toode, adapter_kategooria;
        SqlCommand command;
        AdminPanel adminPanel;
        ComboBox cb1, cb4, cb5;
        TextBox cb2, cb3;
        Label lb1, lb2, lb3, lb4, lb5;
        Button btn1, btn2, btn3, btn4, btn5, btn6, btn7;
        Dictionary<string, int> kategooria;
        Dictionary<int, string> kategooriaRev;
        int SelectId;
        PictureBox pb;
        OpenFileDialog ofd;
        SaveFileDialog save;
        string s;
        public ToodedAB()
        {
            InitializeComponent();
            this.FormBorderStyle = FormBorderStyle.FixedSingle;
            this.MaximizeBox = false;
            this.MinimizeBox = false;
            this.Text = "ToodedAB";
            lb1 = new Label() { Text = "Toode nimetus", Location = new Point(20, 20), Font = new Font("Arial", 16), ForeColor = Color.Black, Size = new Size(150, 30) };
            lb2 = new Label() { Text = "Kogus", Location = new Point(20, lb1.Location.Y + 40), Font = new Font("Arial", 16), ForeColor = Color.Black, Size = new Size(150, 30) };
            lb3 = new Label() { Text = "Hind", Location = new Point(20, lb2.Location.Y + 40), Font = new Font("Arial", 16), ForeColor = Color.Black, Size = new Size(150, 30) };
            lb4 = new Label() { Text = "Kategooria", Location = new Point(20, lb3.Location.Y + 40), Font = new Font("Arial", 16), ForeColor = Color.Black, Size = new Size(150, 30) };
            lb5 = new Label() { Text = "Pilt", Location = new Point(20, lb4.Location.Y + 40), Font = new Font("Arial", 16), ForeColor = Color.Black, Size = new Size(150, 30) };
            cb1 = new ComboBox() { Location = new Point(lb1.Right + 20, lb1.Location.Y), Font = new Font("Arial", 15) };
            cb2 = new TextBox() { Location = new Point(lb2.Right + 20, lb2.Location.Y), Font = new Font("Arial", 15), Size = cb1.Size };
            cb3 = new TextBox() { Location = new Point(lb3.Right + 20, lb3.Location.Y), Font = new Font("Arial", 15), Size = cb1.Size };
            cb4 = new ComboBox() { Location = new Point(lb5.Right + 20, lb5.Location.Y), Font = new Font("Arial", 15) };
            cb5 = new ComboBox() { Location = new Point(lb4.Right + 20, lb4.Location.Y), Font = new Font("Arial", 15) };
            btn1 = new Button() { Text = "Lisa kategooria", Location = new Point(lb5.Left, lb5.Bottom + 15), Size = new Size(120, 20), FlatStyle = FlatStyle.Popup };
            btn1.Click += Btn1_Click;
            btn2 = new Button() { Text = "Kustuta kategooria", Location = new Point(btn1.Right + 5, lb5.Bottom + 15), Size = new Size(120, 20), FlatStyle = FlatStyle.Popup };
            btn2.Click += Btn2_Click;
            btn3 = new Button() { Text = "Otsi fail", Location = new Point(btn2.Right + 15, lb5.Bottom + 15), Size = new Size(120, 20), FlatStyle = FlatStyle.Popup };
            btn3.Click += Btn3_Click;
            btn4 = new Button() { Text = "Lisa", Location = new Point(btn3.Right + 15, lb5.Bottom + 15), Size = new Size(120, 20), FlatStyle = FlatStyle.Popup };
            btn4.Click += Btn4_Click;
            btn5 = new Button() { Text = "Uuenda", Location = new Point(btn4.Right + 15, lb5.Bottom + 15), Size = new Size(120, 20), FlatStyle = FlatStyle.Popup };
            btn5.Click += Btn5_Click;
            btn6 = new Button() { Text = "Kustuta", Location = new Point(btn5.Right + 15, lb5.Bottom + 15), Size = new Size(120, 20), FlatStyle = FlatStyle.Popup };
            btn6.Click += Btn6_Click;
            btn7 = new Button() { Text = "Admin panel", Location = new Point(btn6.Right + 15, lb5.Bottom + 15), Size = new Size(120, 20), FlatStyle = FlatStyle.Popup };
            btn7.Click += Btn7_Click;
            pb = new PictureBox() { Location = new Point(btn5.Right - btn5.Width / 2, cb1.Top - 10), Size = new Size(200, 200), BackColor = Color.Gray };
            ofd = new OpenFileDialog() { FileName = "Valige pildifail", Multiselect = true, InitialDirectory = Environment.GetFolderPath(Environment.SpecialFolder.MyPictures), Title = "Avage pildifail", Filter = "Image Files|*.jpeg; *.jpg; *.gif; *.bmp; *.png; *.tiff; *.icon; *.emf; *.wmf" };
            save = new SaveFileDialog() { InitialDirectory = Path.GetFullPath(@"..\..\Piltid") };
            dgv.CellClick += Dgv_CellClick;
            cb1.SelectedValueChanged += Cb_SelectedValueChanged;
            this.Controls.AddRange(new Control[] { lb1, lb2, lb3, lb4, lb5, cb1, cb2, cb3, cb4, cb5, btn1, btn2, btn3, btn4, btn5, btn6, btn7, pb });
            Naita();
        }

        private void Btn7_Click(object sender, EventArgs e)
        {
            this.Hide();
            adminPanel = new AdminPanel();
            adminPanel.Closed += (s, args) => this.Close();
            adminPanel.Show();
        }

        private void Cb_SelectedValueChanged(object sender, EventArgs e)
        {
            int id = cb1.SelectedIndex;
            cb4.SelectedIndex = id;
            connect.Open();
            adapter_toode = new SqlDataAdapter($"SELECT Id,Kategooriad,Kogus,Hind FROM Toodetable WHERE Toodenimetus='{cb1.Text}'", connect);
            DataTable dt_kat = new DataTable();
            adapter_toode.Fill(dt_kat);
            foreach (DataRow item in dt_kat.Rows)
            {
                cb5.Text = kategooriaRev[Convert.ToInt32(item["Kategooriad"])];
                SelectId = Convert.ToInt32(item["Id"]);
                cb2.Text = item["Kogus"].ToString();
                cb3.Text = item["Hind"].ToString();
            }
            connect.Close();
        }

        private void Btn6_Click(object sender, EventArgs e)
        {
            command = new SqlCommand("DELETE FROM Toodetable WHERE Toodenimetus=@nimi", connect);
            connect.Open();
            command.Parameters.AddWithValue("@nimi", cb1.Text);
            command.ExecuteNonQuery();
            connect.Close();
            Naita();
        }

        private void Dgv_CellClick(object sender, DataGridViewCellEventArgs e)
        {
            try
            {
                if (dgv.Rows[e.RowIndex].Cells[0].Value.ToString() == "")
                {
                    cb1.Text = "";
                    cb2.Text = "0";
                    cb3.Text = "0";
                    cb4.Text = "";
                    cb5.Text = "";
                }
                else
                {
                    int i = 0;
                    foreach (Control item in new Control[] { cb1, cb2, cb3, cb4, cb5 })
                    {
                        item.Text = dgv.Rows[e.RowIndex].Cells[i].Value.ToString();
                        i++;
                    }
                    NaitaPilt();
                }
            }
            catch (Exception) { return; }
        }

        private void Btn5_Click(object sender, EventArgs e)
        {
            if (ControlInsert(false))
            {
                int num = SelectId;
                command = new SqlCommand("UPDATE Toodetable SET Toodenimetus=@nim, Kogus=@kogus, Hind=@hind,Pilt=@img,Kategooriad=@kat WHERE Id=@id", connect);
                connect.Open();
                command.Parameters.AddWithValue("@id", num);
                command.Parameters.AddWithValue("@nim", cb1.Text);
                command.Parameters.AddWithValue("@kogus", cb2.Text);
                command.Parameters.AddWithValue("@hind", Convert.ToDouble(cb3.Text));
                command.Parameters.AddWithValue("@kat", kategooria[cb5.Text]);
                command.Parameters.AddWithValue("@img", cb4.Text);
                command.ExecuteNonQuery();
                connect.Close();
                Naita();
            }
        }

        private void Btn4_Click(object sender, EventArgs e)
        {
            if (ControlInsert(true))
            {
                command = new SqlCommand("INSERT INTO Toodetable (Toodenimetus,Kogus,Hind,Pilt,Kategooriad) VALUES (@nim,@kogus,@hind,@img,@kat)", connect);
                connect.Open();
                command.Parameters.AddWithValue("@nim", cb1.Text);
                command.Parameters.AddWithValue("@kogus", cb2.Text);
                command.Parameters.AddWithValue("@hind", Convert.ToDouble(cb3.Text));
                command.Parameters.AddWithValue("@kat", kategooria[cb5.Text]);
                int i = kategooria[cb5.Text];
                command.Parameters.AddWithValue("@img", cb4.Text);
                command.ExecuteNonQuery();
                connect.Close();
                Naita();
            }
        }

        private void Btn3_Click(object sender, EventArgs e)
        {
            if (cb1.Items.Contains(cb1.Text) && ofd.ShowDialog() == DialogResult.OK)
            {
                using (Stream str = ofd.OpenFile())
                {
                    pb.Image = new Bitmap(ofd.FileName);
                    pb.SizeMode = PictureBoxSizeMode.Zoom;
                    save.FileName = cb1.Text + Path.GetExtension(ofd.FileName);
                    save.Filter = "Piltid|" + Path.GetExtension(ofd.FileName);
                    if (save.ShowDialog() == DialogResult.OK)
                    {
                        File.Copy(ofd.FileName, save.FileName);
                        string[] strings = save.FileName.Split('\\');
                        string name = strings[strings.Count() - 1];
                        int num = SelectId;
                        command = new SqlCommand("UPDATE Toodetable SET Pilt=@img WHERE Id=@id", connect);
                        connect.Open();
                        command.Parameters.AddWithValue("@id", num);
                        command.Parameters.AddWithValue("@img", name);
                        command.ExecuteNonQuery();
                        connect.Close();
                    }
                }
            }
        }
        private void Btn2_Click(object sender, EventArgs e)
        {
            command = new SqlCommand("DELETE FROM Kategooria WHERE Kategooria_nimetus=@kat", connect);
            connect.Open();
            command.Parameters.AddWithValue("@kat", cb5.Text);
            command.ExecuteNonQuery();
            connect.Close();
            NaitaKategooriad();
        }

        private void Btn1_Click(object sender, EventArgs e)
        {
            if (ControlInsertKat())
            {
                command = new SqlCommand("INSERT INTO Kategooria (Kategooria_nimetus) VALUES (@kat)", connect);
                connect.Open();
                command.Parameters.AddWithValue("@kat", cb5.Text);
                command.ExecuteNonQuery();
                connect.Close();
                Naita();
            }
        }

        private void NaitaKategooriad()
        {
            kategooria = new Dictionary<string, int>();
            kategooriaRev = new Dictionary<int, string>();
            cb5.Items.Clear();
            cb5.Text = "";
            connect.Open();
            adapter_kategooria = new SqlDataAdapter("SELECT Id,Kategooria_nimetus FROM Kategooria", connect);
            DataTable dt_kat = new DataTable();
            adapter_kategooria.Fill(dt_kat);
            foreach (DataRow item in dt_kat.Rows)
            {
                if (!cb5.Items.Contains(item["Kategooria_nimetus"]))
                {
                    cb5.Items.Add(item["Kategooria_nimetus"]);
                    kategooria.Add((string)item["Kategooria_nimetus"], (int)item["Id"]);
                    kategooriaRev.Add((int)item["Id"], (string)item["Kategooria_nimetus"]);
                }
                else
                {
                    command = new SqlCommand("DELETE FROM Kategooria WHERE Id=@id", connect);
                    command.Parameters.AddWithValue("@id", item["Id"]);
                    command.ExecuteNonQuery();
                }

            }
            connect.Close();
        }

        private void NaitaRows()
        {
            connect.Open();
            adapter_toode = new SqlDataAdapter("SELECT Toodenimetus,Pilt FROM Toodetable", connect);
            DataTable dt_kat = new DataTable();
            adapter_toode.Fill(dt_kat);
            List<string> list = new List<string>() { "Toodenimetus", "Pilt" };
            int i = 0;
            foreach (ComboBox item in new ComboBox[] { cb1, cb4 })
            {
                foreach (DataRow item1 in dt_kat.Rows)
                {
                    item.Items.Add(item1[list[i]]);
                }
                i++;
            }
            connect.Close();
        }

        private void NaitaAndmed()
        {
            connect.Open();
            DataTable dt_toode = new DataTable();
            adapter_toode = new SqlDataAdapter("SELECT Toodetable.Toodenimetus,Toodetable.Kogus,Toodetable.Hind,Toodetable.Pilt,Kategooria.Kategooria_nimetus FROM Toodetable INNER JOIN Kategooria ON Toodetable.Kategooriad = Kategooria.Id;", connect);
            adapter_toode.Fill(dt_toode);
            dgv.DataSource = null;
            dgv.DataSource = dt_toode;
            DataGridViewComboBoxColumn dgvcb = new DataGridViewComboBoxColumn();
            dgvcb.HeaderText = "Kategooria";
            dgvcb.Name = "KategooriaColumn";
            dgvcb.DataPropertyName = "Kategooria_nimetus";
            HashSet<string> list = new HashSet<string>();
            foreach (DataRow item in dt_toode.Rows)
            {
                string cat = item["Kategooria_nimetus"].ToString();
                if (!list.Contains(cat))
                {
                    list.Add(cat);
                    dgvcb.Items.Add(cat);
                }
            }
            dgv.Columns.Add(dgvcb);
            dgv.Columns["Kategooria_nimetus"].Visible = false;
            connect.Close();
        }

        private void Naita()
        {
            foreach (ComboBox item in new ComboBox[] { cb1, cb4, cb5 })
            {
                item.Items.Clear();
                item.Text = "";
            }
            cb2.Text = "";
            cb3.Text = "";
            NaitaKategooriad();
            NaitaAndmed();
            NaitaRows();
            NaitaPilt();
        }

        private void NaitaPilt()
        {
            try
            {
                DirectoryInfo Di = new DirectoryInfo(save.InitialDirectory);
                foreach (FileInfo fi in Di.GetFiles())
                {
                    s = fi.Name + "\r\n";
                    if (s.ToLower().Contains(cb1.Text.ToLower()) || s.ToLower().Contains(cb4.Text.ToLower()))
                        break;
                }
                if (!s.ToLower().Contains(cb1.Text.ToLower()) && !s.ToLower().Contains(cb4.Text.ToLower()))
                {
                    pb.Image = null;
                    s = "";
                    return;
                }
                pb.ImageLocation = save.InitialDirectory + "\\" + s;
                pb.Load();
                pb.SizeMode = PictureBoxSizeMode.Zoom;
            }
            catch { }
        }

        private bool ControlInsertKat()
        {
            cb5.BackColor = Color.White;
            List<bool> b = new List<bool>();
            if (cb5.Items.Contains(cb5.Text) || cb5.Text == "" || cb5.Text.Length < 3)
            {
                b.Add(false);
                cb5.BackColor = Color.Red;
            }
            return !b.Any(c => c == false);
        }

        private bool ControlInsert(bool c)
        {
            foreach (Control item in new Control[] { cb1, cb2, cb3, cb4, cb5 })
                item.BackColor = Color.White;
            List<bool> b = new List<bool>();
            if (c)
            {
                if (cb1.Items.Contains(cb1.Text) || cb1.Text.Length < 4)
                {
                    b.Add(false);
                    cb1.BackColor = Color.Red;
                }
            }
            int.TryParse(cb2.Text, out int num1);
            if (num1 == 0)
            {
                b.Add(false);
                cb2.BackColor = Color.Red;
            }
            double.TryParse(cb3.Text, out double num2);
            if (num2 == 0)
            {
                b.Add(false);
                cb3.BackColor = Color.Red;
            }
            if (!cb5.Items.Contains(cb5.Text) || cb5.Text == "")
            {
                b.Add(false);
                cb5.BackColor = Color.Red;
            }
            return !b.Any(f => f == false);
        }
    }
}