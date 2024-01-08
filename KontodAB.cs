using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Text;
using System.Windows.Forms;
using System.Threading.Tasks;
using System.IO;
using System.Security.Cryptography;
using System.Data.SqlClient;
using System.Linq;

namespace ToodedAB
{
    public partial class KontodAB : Form
    {
        SqlConnection connect = new SqlConnection(@"Data Source=(LocalDB)\MSSQLLocalDB;AttachDbFilename=|DataDirectory|\AppData\Tooded_DB.mdf;Integrated Security=True");
        SqlDataAdapter adapter_konto;
        SqlCommand command;
        ComboBox cb1;
        TextBox cb2, cb3, cb4, cb5, cb6;
        Label lb1, lb2, lb3, lb4, lb5, lb6;
        Button btn1, btn2, btn3, btn4, btn5, btn6,btn7;
        AdminPanel adminPanel;
        int SelectId;
        DataGridView dgv;
        string text;
        static char[] alphaUp = "ABCDEFGHIJKLMNOPQRSTUVWXYZ".ToCharArray();
        static char[] alphaLow = "ABCDEFGHIJKLMNOPQRSTUVWXYZ".ToLower().ToCharArray();
        static char[] num = "0123456789".ToCharArray();
        List<char[]> lists = new List<char[]>() {alphaUp, alphaLow, num};   

        public KontodAB()
        {
            InitializeComponent();
            this.FormBorderStyle = FormBorderStyle.FixedSingle;
            this.MaximizeBox = false;
            this.MinimizeBox = false;
            this.Text = "KontodAB";
            lb1 = new Label() { Text = "Kasutajanimi", Location = new Point(20, 20), Font = new Font("Arial", 16), ForeColor = Color.Black, Size = new Size(150, 30) };
            lb2 = new Label() { Text = "Parool", Location = new Point(20, lb1.Location.Y + 40), Font = new Font("Arial", 16), ForeColor = Color.Black, Size = new Size(150, 30) };
            lb3 = new Label() { Text = "Vihje", Location = new Point(20, lb2.Location.Y + 40), Font = new Font("Arial", 16), ForeColor = Color.Black, Size = new Size(150, 30) };
            lb4 = new Label() { Text = "Aeg", Location = new Point(20, lb3.Location.Y + 40), Font = new Font("Arial", 16), ForeColor = Color.Black, Size = new Size(150, 30) };
            lb5 = new Label() { Text = "Kuulutud raha", Location = new Point(20, lb4.Location.Y + 40), Font = new Font("Arial", 16), ForeColor = Color.Black, Size = new Size(150, 30) };
            lb6 = new Label() { Text = "Boonusraha", Location = new Point(20, lb5.Location.Y + 40), Font = new Font("Arial", 16), ForeColor = Color.Black, Size = new Size(150, 30) };
            cb1 = new ComboBox() { Location = new Point(lb1.Right + 20, lb1.Location.Y), Font = new Font("Arial", 15) };
            cb2 = new TextBox() { Location = new Point(lb2.Right + 20, lb2.Location.Y), Font = new Font("Arial", 15), Size = cb1.Size };
            cb3 = new TextBox() { Location = new Point(lb3.Right + 20, lb3.Location.Y), Font = new Font("Arial", 15), Size = cb1.Size };
            cb4 = new TextBox() { Location = new Point(lb4.Right + 20, lb4.Location.Y), Font = new Font("Arial", 15), Size = cb1.Size };
            cb5 = new TextBox() { Location = new Point(lb5.Right + 20, lb5.Location.Y), Font = new Font("Arial", 15), Size = cb1.Size };
            cb6 = new TextBox() { Location = new Point(lb6.Right + 20, lb6.Location.Y), Font = new Font("Arial", 15), Size = cb1.Size };
            btn1 = new Button() { Text = "Lisa konto", Location = new Point(lb1.Right+300, lb1.Location.Y), Size = new Size(120, 20), FlatStyle = FlatStyle.Popup };
            btn1.Click += Btn1_Click;
            btn2 = new Button() { Text = "Kustuta konto", Location = new Point(btn1.Left, btn1.Location.Y+40), Size = new Size(120, 20), FlatStyle = FlatStyle.Popup };
            btn2.Click += Btn2_Click;
            btn3 = new Button() { Text = "Uuenda konto", Location = new Point(btn2.Left, btn2.Location.Y + 40), Size = new Size(120, 20), FlatStyle = FlatStyle.Popup };
            btn3.Click += Btn3_Click;
            btn4 = new Button() { Text = "Praegu aeg", Location = new Point(btn1.Right+100, btn1.Location.Y), Size = new Size(120, 20), FlatStyle = FlatStyle.Popup };
            btn4.Click += Btn4_Click;
            btn5 = new Button() { Text = "Juhuslik parool", Location = new Point(btn1.Right + 100, btn1.Location.Y+40), Size = new Size(120, 20), FlatStyle = FlatStyle.Popup };
            btn5.Click += Btn5_Click;
            btn6 = new Button() { Text = "Juhuslik vihje", Location = new Point(btn1.Right + 100, btn2.Location.Y + 40), Size = new Size(120, 20), FlatStyle = FlatStyle.Popup };
            btn6.Click += Btn6_Click;
            btn7= new Button() { Text = "Admin panel", Location = new Point(btn1.Right+100, btn3.Location.Y+40), Size = new Size(120, 20), FlatStyle = FlatStyle.Popup };
            btn7.Click+=Btn7_Click;
            dgv.CellClick += Dgv_CellClick;
            cb1.SelectedValueChanged += Cb_SelectedValueChanged;
            this.Controls.AddRange(new Control[] { lb1, lb2, lb3, lb4, lb5, lb6, cb1, cb2, cb3, cb4, cb5, cb6, btn1, btn2, btn3, btn4, btn5, btn6,btn7 });
            Naita();
        }

        private void Btn7_Click(object sender, EventArgs e)
        {
            this.Hide();
            adminPanel = new AdminPanel();
            adminPanel.Closed += (s, args) => this.Close();
            adminPanel.Show();
        }

        //Рандомная генерация текста для текстовых ящиков 
        //используется для пароля и подсказки
        private async void RandomGen(TextBox textBox)
        {
            string ran = "";
            int num = new Random().Next(5, 20);
            for (int i = 0; i < num; i++)
            {
                int listnum = new Random().Next(0, 3); //выбираю список (маленькие буквы, большие или цифры)
                ran += lists[listnum][new Random().Next(0, lists[listnum].Length)]; //в выбранном списке случайно выбираю значение
                await Task.Delay(1); //задержка чтобы код не выполнялся моментально и присутсовала случайная генерация
            }
                textBox.Text = ran;
        }

        private void Btn6_Click(object sender, EventArgs e)
        {
            RandomGen(cb3);
        }

        private void Btn5_Click(object sender, EventArgs e)
        {
            RandomGen(cb2);
        }

        private void Btn4_Click(object sender, EventArgs e)
        {
            cb4.Text = DateTime.Now.ToString().Split()[0];
        }

        private void Cb_SelectedValueChanged(object sender, EventArgs e)
        {
            connect.Open();
            adapter_konto = new SqlDataAdapter($"SELECT Id,Password,Hint,Money,Time,Cashback FROM Account WHERE Username='{cb1.Text}'", connect);
            DataTable dt_kat = new DataTable();
            adapter_konto.Fill(dt_kat);
            foreach (DataRow item in dt_kat.Rows)
            {
                SelectId = Convert.ToInt32(item["Id"]);
                cb2.Text = item["Password"].ToString();
                cb3.Text = item["Hint"].ToString();
                cb4.Text = item["Time"].ToString().Split()[0];
                cb5.Text = item["Money"].ToString() == "" ? "0" : item["Money"].ToString().ToString();
                cb6.Text = item["Cashback"].ToString() == "" ? "0" : item["Cashback"].ToString();
            }
            connect.Close();
        }

        private void Dgv_CellClick(object sender, DataGridViewCellEventArgs e)
        {
            try
            {
                if (dgv.Rows[e.RowIndex].Cells[0].Value.ToString()== "")
                {
                    cb1.Text = "";
                    cb2.Text = "";
                    cb3.Text = "";
                    Btn4_Click(sender, e);
                    cb5.Text = "0";
                    cb6.Text="0";
                }
                else
                {
                    int i = 0;
                    foreach (Control item in new Control[] { cb1, cb2, cb3, cb4, cb5 })
                    {
                        text = dgv.Rows[e.RowIndex].Cells[i].Value.ToString();
                        item.Text = text == "" ? "0" : text;
                        i++;
                    }
                    cb4.Text = cb4.Text.Split()[0];
                }
            }
            catch (Exception) { return; }
        }

        private void Btn3_Click(object sender, EventArgs e)
        {
            if (ControlInsert(false))
            {
                command = new SqlCommand("UPDATE Account SET Username=@user, Password=@pass, Hint=@hint,Time=@time,Money=@mon,Cashback=@cash WHERE Id=@id", connect);
                connect.Open();
                command.Parameters.AddWithValue("@id", SelectId);
                command.Parameters.AddWithValue("@user", cb1.Text);
                command.Parameters.AddWithValue("@pass", cb2.Text);
                command.Parameters.AddWithValue("@hint", cb3.Text);
                command.Parameters.AddWithValue("@time", Convert.ToDateTime(cb4.Text));
                command.Parameters.AddWithValue("@mon", Convert.ToInt32(cb5.Text));
                command.Parameters.AddWithValue("@cash", Convert.ToInt32(cb6.Text));
                command.ExecuteNonQuery();
                connect.Close();
                Naita();
            }
        }

        private void Btn2_Click(object sender, EventArgs e)
        {
            command = new SqlCommand("DELETE FROM Account WHERE Username=@user", connect);
            connect.Open();
            command.Parameters.AddWithValue("@user", cb1.Text);
            command.ExecuteNonQuery();
            connect.Close();
            Naita();
        }

        private void Btn1_Click(object sender, EventArgs e)
        {
            if (ControlInsert(true))
            {
                command = new SqlCommand("INSERT INTO Account (Username,Password,Hint,Time,Money,Cashback) VALUES (@user,@pass,@hint,@time,@mon,@cash)", connect);
                connect.Open();
                command.Parameters.AddWithValue("@user", cb1.Text);
                command.Parameters.AddWithValue("@pass", cb2.Text);
                command.Parameters.AddWithValue("@hint", cb3.Text);
                command.Parameters.AddWithValue("@time", Convert.ToDateTime(cb4.Text));
                command.Parameters.AddWithValue("@mon", Convert.ToInt32(cb5.Text));
                command.Parameters.AddWithValue("@cash", Convert.ToInt32(cb6.Text));
                command.ExecuteNonQuery();
                connect.Close();
                Naita();
            }
        }

        private void NaitaAndmed()
        {
            connect.Open();
            DataTable dt_toode = new DataTable();
            adapter_konto = new SqlDataAdapter("SELECT Username, Password, Hint, Time, Money, Cashback FROM Account;", connect);
            adapter_konto.Fill(dt_toode);
            dgv.DataSource = null;
            dgv.DataSource = dt_toode;
            connect.Close();
        }
        private void NaitaRows()
        {
            connect.Open();
            adapter_konto = new SqlDataAdapter("SELECT Username FROM Account", connect);
            DataTable dt_kat = new DataTable();
            adapter_konto.Fill(dt_kat);
            foreach (DataRow item in dt_kat.Rows)
            {
                cb1.Items.Add(item["Username"]);
            }
            connect.Close();
        }

        private void Naita()
        {
            NaitaAndmed();
            NaitaRows();
        }

        private void InitializeComponent()
        {
            this.dgv = new System.Windows.Forms.DataGridView();
            ((System.ComponentModel.ISupportInitialize)(this.dgv)).BeginInit();
            this.SuspendLayout();
            // 
            // dgv
            // 
            this.dgv.ColumnHeadersHeightSizeMode = System.Windows.Forms.DataGridViewColumnHeadersHeightSizeMode.AutoSize;
            this.dgv.Location = new System.Drawing.Point(12, 275);
            this.dgv.Name = "dgv";
            this.dgv.Size = new System.Drawing.Size(936, 223);
            this.dgv.TabIndex = 1;
            // 
            // KontodAB
            // 
            this.ClientSize = new System.Drawing.Size(960, 510);
            this.Controls.Add(this.dgv);
            this.Name = "KontodAB";
            ((System.ComponentModel.ISupportInitialize)(this.dgv)).EndInit();
            this.ResumeLayout(false);

        }

        private bool ControlInsert(bool c)
        {
            foreach (Control item in new Control[] { cb1, cb2, cb3, cb4, cb5, cb6 })
                item.BackColor = Color.White;
            List<bool> b = new List<bool>();
            if (c)
            {
                if (cb1.Items.Contains(cb1.Text) || cb1.Text.Length<4)
                {
                    b.Add(false);
                    cb1.BackColor= Color.Red;
                }
            }
            if (cb2.Text.Length<4)
            {
                b.Add(false);
                cb2.BackColor= Color.Red;
            }
            if (cb3.Text.Length<4)
            {
                b.Add(false);
                cb3.BackColor= Color.Red;
            }
            DateTime.TryParse(cb4.Text, out DateTime dt);
            if (dt == DateTime.MinValue)
            {
                b.Add(false);
                cb4.BackColor = Color.Red;
            }
            foreach (TextBox item in new TextBox[] {cb5,cb6})
            {
                int.TryParse(item.Text, out int num1);
                if (num1 == 0)
                {
                    b.Add(false);
                    item.BackColor = Color.Red;
                }
            }
            return !b.Any(f => f == false);
        }
    }
}
