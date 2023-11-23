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

namespace ToodedAB
{
    public partial class Form1 : Form
    {
        
        SqlConnection connect = new SqlConnection(@"Data Source=(LocalDB)\MSSQLLocalDB;AttachDbFilename=|DataDirectory|\AppData\Tooded_DB.mdf;Integrated Security=True");
        SqlDataAdapter adapter_toode, adapter_kategooria;
        SqlCommand command;
        ComboBox cb1, cb2, cb3, cb4;
        Label lb1, lb2, lb3, lb4;
        Button btn1, btn2, btn3, btn4, btn5, btn6;

        //private void Lisa_Kategooriad(object sender, EventArgs e)
        //{
        //    command = new SqlCommand("INSERT INTO kategooriatabel (Kategooria_nimetus) VALUES ()");
        //}
        public Form1()
        {
            InitializeComponent();
            lb1 = new Label() { Text = "Toode nimetus", Location = new Point(20, 20), Font = new Font("Arial", 16), ForeColor = Color.Black, Size=new Size(150, 30) };
            lb2 = new Label() { Text = "Kogus", Location = new Point(20, lb1.Location.Y+60), Font = new Font("Arial", 16), ForeColor = Color.Black, Size=new Size(150, 30) };
            lb3 = new Label() { Text = "Hind", Location = new Point(20, lb2.Location.Y+60), Font = new Font("Arial", 16), ForeColor = Color.Black, Size=new Size(150, 30) };
            lb4 = new Label() { Text = "Kategooria", Location = new Point(20,lb3.Location.Y+60), Font = new Font("Arial", 16), ForeColor = Color.Black, Size=new Size(150,30)};
            cb1 = new ComboBox() { Location = new Point(lb1.Right+20,lb1.Location.Y), Font = new Font("Arial", 15) };
            cb2 = new ComboBox() { Location = new Point(lb2.Right+20, lb2.Location.Y), Font = new Font("Arial", 15) };
            cb3 = new ComboBox() { Location = new Point(lb3.Right+20, lb3.Location.Y), Font = new Font("Arial", 15) };
            cb4 = new ComboBox() { Location = new Point(lb4.Right+20, lb4.Location.Y), Font = new Font("Arial", 15) };
            btn1 = new Button() { Text = "Lisa kategooria", Location = new Point(lb4.Left, lb4.Bottom+15), Size = new Size(100,20), FlatStyle = FlatStyle.Popup };
            btn1.Click +=Btn1_Click;
            this.Controls.AddRange(new Control[] {lb1,lb2,lb3,lb4,cb1,cb2,cb3,cb4,btn1});
            NaitaAndmed();
            NaitaKategooriad();

        }

        private void Btn1_Click(object sender, EventArgs e)
        {
            command = new SqlCommand("INSERT INTO Kategooria (Kategooria_nimetus) VALUES (@kat)", connect);
            connect.Open();
            command.Parameters.AddWithValue("@kat", cb4.Text);
            command.ExecuteNonQuery();
            connect.Close();
            cb4.Items.Clear();
            NaitaKategooriad();
        }

        private void NaitaKategooriad()
        {
            connect.Open();
            adapter_kategooria = new SqlDataAdapter("SELECT Kategooria_nimetus FROM Kategooria", connect);
            DataTable dt_kat = new DataTable();
            adapter_kategooria.Fill(dt_kat);
            foreach (DataRow item in dt_kat.Rows)
            {
                cb4.Items.Add(item["Kategooria_nimetus"]);
            }
            connect.Close();
        }

        private void NaitaAndmed()
        {
            connect.Open();
            DataTable dt_toode = new DataTable();
            adapter_toode = new SqlDataAdapter("SELECT * FROM Toodetable;", connect);
            adapter_toode.Fill(dt_toode);
            dgv.DataSource = dt_toode;
            connect.Close();
        }
    }
}
