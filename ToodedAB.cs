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
        ComboBox cb1, cb2, cb3, cb4, cb5;
        Label lb1, lb2, lb3, lb4, lb5;
        Button btn1, btn2, btn3, btn4, btn5, btn6;
        Dictionary<string,int> kategooria;
        Dictionary<int, string> kategooriaRev;
        int SelectId;
        PictureBox pb;
        OpenFileDialog ofd;
        SaveFileDialog save;
        public ToodedAB()
        {
            InitializeComponent();
            lb1 = new Label() { Text = "Toode nimetus", Location = new Point(20, 20), Font = new Font("Arial", 16), ForeColor = Color.Black, Size=new Size(150, 30) };
            lb2 = new Label() { Text = "Kogus", Location = new Point(20, lb1.Location.Y+40), Font = new Font("Arial", 16), ForeColor = Color.Black, Size=new Size(150, 30) };
            lb3 = new Label() { Text = "Hind", Location = new Point(20, lb2.Location.Y+ 40), Font = new Font("Arial", 16), ForeColor = Color.Black, Size=new Size(150, 30) };
            lb4 = new Label() { Text = "Kategooria", Location = new Point(20,lb3.Location.Y+ 40), Font = new Font("Arial", 16), ForeColor = Color.Black, Size=new Size(150,30)};
            lb5 = new Label() { Text = "Pilt", Location = new Point(20, lb4.Location.Y + 40), Font = new Font("Arial", 16), ForeColor = Color.Black, Size = new Size(150, 30) };
            cb1 = new ComboBox() { Location = new Point(lb1.Right+20,lb1.Location.Y), Font = new Font("Arial", 15) };
            cb2 = new ComboBox() { Location = new Point(lb2.Right+20, lb2.Location.Y), Font = new Font("Arial", 15) };
            cb3 = new ComboBox() { Location = new Point(lb3.Right+20, lb3.Location.Y), Font = new Font("Arial", 15) };
            cb4 = new ComboBox() { Location = new Point(lb5.Right + 20, lb5.Location.Y), Font = new Font("Arial", 15) };
            cb5 = new ComboBox() { Location = new Point(lb4.Right+20, lb4.Location.Y), Font = new Font("Arial", 15) };
            btn1 = new Button() { Text = "Lisa kategooria", Location = new Point(lb5.Left, lb5.Bottom+15), Size = new Size(120,20), FlatStyle = FlatStyle.Popup };
            btn1.Click +=Btn1_Click;
            btn2 = new Button() { Text = "Kustuta kategooria", Location = new Point(btn1.Right+5, lb5.Bottom + 15), Size = new Size(120, 20), FlatStyle = FlatStyle.Popup };
            btn2.Click += Btn2_Click;
            btn3 = new Button() { Text = "Otsi fail", Location = new Point(btn2.Right + 15, lb5.Bottom + 15), Size = new Size(120, 20), FlatStyle = FlatStyle.Popup };
            btn3.Click += Btn3_Click;
            btn4 = new Button() { Text = "Lisa", Location = new Point(btn3.Right + 15, lb5.Bottom + 15), Size = new Size(120, 20), FlatStyle = FlatStyle.Popup };
            btn4.Click += Btn4_Click;
            btn5 = new Button() { Text = "Uuenda", Location = new Point(btn4.Right + 15, lb5.Bottom + 15), Size = new Size(120, 20), FlatStyle = FlatStyle.Popup };
            btn5.Click += Btn5_Click;
            btn6 = new Button() { Text = "Kustuta", Location = new Point(btn5.Right + 15, lb5.Bottom + 15), Size = new Size(120, 20), FlatStyle = FlatStyle.Popup };
            btn6.Click += Btn6_Click;
            pb = new PictureBox() { Location = new Point(btn5.Right-btn5.Width/2,cb1.Top-10),Size = new Size(200,200), BackColor=Color.Gray};
            ofd = new OpenFileDialog(){ FileName = "Valige pildifail",Multiselect=true,InitialDirectory=Environment.GetFolderPath(Environment.SpecialFolder.MyPictures),Title = "Avage pildifail",Filter = "Image Files|*.jpeg; *.jpg; *.gif; *.bmp; *.png; *.tiff; *.icon; *.emf; *.wmf"};
            save = new SaveFileDialog() {  InitialDirectory=Path.GetFullPath(@"..\..\Images") };
            dgv.CellClick +=Dgv_CellClick;
            cb1.SelectedValueChanged+=Cb_SelectedValueChanged;
            this.Controls.AddRange(new Control[] {lb1,lb2,lb3,lb4,lb5,cb1,cb2,cb3,cb4,cb5,btn1,btn2,btn3,btn4,btn5,btn6, pb});
            Naita();

        }

        private void Cb_SelectedValueChanged(object sender, EventArgs e)
        {
            int id = cb1.SelectedIndex;
            cb2.SelectedIndex = id;
            cb3.SelectedIndex = id;
            cb4.SelectedIndex = id;
            connect.Open();
            adapter_toode = new SqlDataAdapter($"SELECT Id,Kategooriad FROM Toodetable WHERE Toodenimetus='{cb1.Text}'", connect);
            DataTable dt_kat = new DataTable();
            adapter_toode.Fill(dt_kat);
            foreach (DataRow item in dt_kat.Rows)
            {
                cb5.Text = kategooriaRev[Convert.ToInt32(item["Kategooriad"])];
                SelectId = Convert.ToInt32(item["Id"]);
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
            int i = 0;
            foreach (ComboBox item in new ComboBox[] { cb1, cb2, cb3, cb4, cb5 })
            {
                item.Text = dgv.Rows[e.RowIndex].Cells[i].Value.ToString();
                i++;
            }
            try
            {
                foreach (var item in new string[] {".jpeg", ".jpg", ".gif", ".bmp", ".png", ".tiff", ".icon", ".emf", ".wmf"})
                { 
                    pb.ImageLocation = "Images\\"+cb1.Text.ToLower()+item;
                    pb.Load();
                }
            }
            catch (Exception)
            { 
            }
        }

        private void Btn5_Click(object sender, EventArgs e)
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

        private void Btn4_Click(object sender, EventArgs e)
        {
            command = new SqlCommand("INSERT INTO Toodetable (Toodenimetus,Kogus,Hind,Pilt,Kategooriad) VALUES (@nim,@kogus,@hind,@img,@kat)", connect);
            connect.Open();
            command.Parameters.AddWithValue("@nim", cb1.Text);
            command.Parameters.AddWithValue("@kogus", cb2.Text);
            command.Parameters.AddWithValue("@hind",Convert.ToDouble(cb3.Text));
            command.Parameters.AddWithValue("@kat", kategooria[cb5.Text]);
            int i = kategooria[cb5.Text];
            command.Parameters.AddWithValue("@img", cb4.Text);
            command.ExecuteNonQuery();
            connect.Close();
            Naita();
        }

        private void Btn3_Click(object sender, EventArgs e)
        {
            if (cb1.Items.Contains(cb1.Text) && ofd.ShowDialog() == DialogResult.OK)
            {
                using (Stream str = ofd.OpenFile())
                {
                    pb.Image = new Bitmap(ofd.FileName);
                    pb.SizeMode = PictureBoxSizeMode.Zoom;
                    save.FileName=cb1.Text+Path.GetExtension(ofd.FileName);
                    save.Filter="Images|"+Path.GetExtension(ofd.FileName);
                    if (save.ShowDialog()==DialogResult.OK)
                    {
                        File.Copy(ofd.FileName, save.FileName);
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
            command = new SqlCommand("INSERT INTO Kategooria (Kategooria_nimetus) VALUES (@kat)", connect);
            connect.Open();
            command.Parameters.AddWithValue("@kat", cb5.Text);
            command.ExecuteNonQuery();
            connect.Close();
            NaitaKategooriad();
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
            adapter_toode = new SqlDataAdapter("SELECT Toodenimetus,Kogus,Hind,Pilt FROM Toodetable", connect);
            DataTable dt_kat = new DataTable();
            adapter_toode.Fill(dt_kat);
            List<string> list = new List<string>() { "Toodenimetus", "Kogus", "Hind", "Pilt" };
            int i = 0;
            foreach (ComboBox item in new ComboBox[] { cb1, cb2, cb3, cb4 })
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
            //connect.Open();
            //DataTable dt_toode = new DataTable();
            //DataTable table = new DataTable();
            //adapter_toode = new SqlDataAdapter("SELECT Toodetable.Toodenimetus,Toodetable.Kogus,Toodetable.Hind,Toodetable.Pilt,Kategooria.Kategooria_nimetus FROM Toodetable INNER JOIN Kategooria ON Toodetable.Kategooriad = Kategooria.Id;", connect);
            //adapter_toode.Fill(dt_toode);
            //table.Columns.Add("Nimetus");
            //table.Columns.Add("Kogus");
            //table.Columns.Add("Hind");
            //table.Columns.Add("Pilt");
            //DataGridViewComboBoxColumn dgvcb = new DataGridViewComboBoxColumn();
            //dgvcb.HeaderText= "Kategooria";
            //foreach (DataRow item in dt_toode.Rows)
            //{
            //    if (!dgvcb.Items.Contains(item["Kategooria_nimetus"]))
            //        dgvcb.Items.Add(item["Kategooria_nimetus"]);
            //}
            //foreach (DataRow item in dt_toode.Rows)
            //    table.Rows.Add(item["Toodenimetus"], item["Kogus"], item["Hind"], item["Pilt"]);
            //dgv.Columns.Add(dgvcb);
            //dgv.Rows.Add(dgvcb.Items[0]);
            //dgv.DataSource = table;
            //connect.Close();

            connect.Open();
            DataTable dt_toode = new DataTable();
            adapter_toode = new SqlDataAdapter("SELECT Toodetable.Toodenimetus,Toodetable.Kogus,Toodetable.Hind,Toodetable.Pilt,Kategooria.Kategooria_nimetus FROM Toodetable INNER JOIN Kategooria ON Toodetable.Kategooriad = Kategooria.Id;", connect);
            adapter_toode.Fill(dt_toode);
            dgv.DataSource = dt_toode;
            connect.Close();
        }

        private void Naita()
        {
            foreach (ComboBox item in new ComboBox[] { cb1,cb2,cb3,cb4,cb5 })
            {
                item.Items.Clear();
                item.Text = "";
            }
            NaitaKategooriad();
            NaitaAndmed();
            NaitaRows();
        }
    }
}
