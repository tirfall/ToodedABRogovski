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

namespace ToodedAB
{
    public partial class KontodAB : Form
    {
        SqlConnection connect = new SqlConnection(@"Data Source=(LocalDB)\MSSQLLocalDB;AttachDbFilename=|DataDirectory|\AppData\Tooded_DB.mdf;Integrated Security=True");
        SqlDataAdapter adapter_konto;
        SqlCommand command;
        ComboBox cb1;
        TextBox cb2, cb3, cb4, cb5;
        Label lb1, lb2, lb3, lb4, lb5, lb6;
        Button btn1, btn2, btn3, btn4;
        int SelectId;
        DataGridView dgv;

        public KontodAB()
        {
            InitializeComponent(); 
            lb1 = new Label() { Text = "Username", Location = new Point(20, 20), Font = new Font("Arial", 16), ForeColor = Color.Black, Size = new Size(150, 30) };
            lb2 = new Label() { Text = "Password", Location = new Point(20, lb1.Location.Y + 40), Font = new Font("Arial", 16), ForeColor = Color.Black, Size = new Size(150, 30) };
            lb3 = new Label() { Text = "Hint", Location = new Point(20, lb2.Location.Y + 40), Font = new Font("Arial", 16), ForeColor = Color.Black, Size = new Size(150, 30) };
            lb4 = new Label() { Text = "Time", Location = new Point(20, lb3.Location.Y + 40), Font = new Font("Arial", 16), ForeColor = Color.Black, Size = new Size(150, 30) };
            lb5 = new Label() { Text = "Money", Location = new Point(20, lb4.Location.Y + 40), Font = new Font("Arial", 16), ForeColor = Color.Black, Size = new Size(150, 30) };
            lb6 = new Label() { Text = "Cashback", Location = new Point(20, lb5.Location.Y + 40), Font = new Font("Arial", 16), ForeColor = Color.Black, Size = new Size(150, 30) };
            cb1 = new ComboBox() { Location = new Point(lb1.Right + 20, lb1.Location.Y), Font = new Font("Arial", 15) };
            cb2 = new TextBox() { Location = new Point(lb2.Right + 20, lb2.Location.Y), Font = new Font("Arial", 15), Size = cb1.Size };
            cb3 = new TextBox() { Location = new Point(lb3.Right + 20, lb3.Location.Y), Font = new Font("Arial", 15), Size = cb1.Size };
            cb4 = new TextBox() { Location = new Point(lb5.Right + 20, lb5.Location.Y), Font = new Font("Arial", 15) };
            cb5 = new TextBox() { Location = new Point(lb4.Right + 20, lb4.Location.Y), Font = new Font("Arial", 15) };
            btn1 = new Button() { Text = "Lisa konto", Location = new Point(lb6.Left, lb6.Bottom + 15), Size = new Size(120, 20), FlatStyle = FlatStyle.Popup };
            btn1.Click += Btn1_Click;
            btn2 = new Button() { Text = "Kustuta konto", Location = new Point(btn1.Right + 5, lb6.Bottom + 15), Size = new Size(120, 20), FlatStyle = FlatStyle.Popup };
            btn2.Click += Btn2_Click;
            btn3 = new Button() { Text = "Uuenda konto", Location = new Point(btn2.Right + 15, lb6.Bottom + 15), Size = new Size(120, 20), FlatStyle = FlatStyle.Popup };
            btn3.Click += Btn3_Click;
            btn4 = new Button() { Text = "Praegu aeg", Location = new Point(btn2.Right + 15, lb6.Bottom + 15), Size = new Size(120, 20), FlatStyle = FlatStyle.Popup };
            btn4.Click += Btn4_Click;
            dgv.CellClick += Dgv_CellClick;
            cb1.SelectedValueChanged += Cb_SelectedValueChanged;
            this.Controls.AddRange(new Control[] { lb1, lb2, lb3, lb4, lb5, cb1, cb2, cb3, cb4, cb5, btn1, btn2, btn3, btn4 });
            Naita();
        }

        private void Btn4_Click(object sender, EventArgs e)
        {
            throw new NotImplementedException();
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
                cb4.Text = item["Money"].ToString();
                cb5.Text = item["Time"].ToString();
            }
            connect.Close();
        }

        private void Dgv_CellClick(object sender, DataGridViewCellEventArgs e)
        {
            int i = 0;
            foreach (Control item in new Control[] { cb1, cb2, cb3, cb4, cb5 })
            {
                item.Text = dgv.Rows[e.RowIndex].Cells[i].Value.ToString();
                i++;
            }
        }

        private void Btn3_Click(object sender, EventArgs e)
        {
            throw new NotImplementedException();
        }

        private void Btn2_Click(object sender, EventArgs e)
        {
            throw new NotImplementedException();
        }

        private void Btn1_Click(object sender, EventArgs e)
        {
            throw new NotImplementedException();
        }

        private void NaitaAndmed()
        {
            connect.Open();
            DataTable dt_toode = new DataTable();
            adapter_konto = new SqlDataAdapter("SELECT Username, Password, Hint, Money, Time, Cashback FROM Account;", connect);
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
    }
}
