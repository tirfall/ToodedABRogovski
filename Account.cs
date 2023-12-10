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
    public partial class Account : Form
    {
        SqlConnection connect = new SqlConnection(@"Data Source=(LocalDB)\MSSQLLocalDB;AttachDbFilename=|DataDirectory|\AppData\Tooded_DB.mdf;Integrated Security=True");
        SqlDataAdapter adapter_acc;
        SqlCommand command;
        MenuStrip MainMenu;
        ToolStripMenuItem tsinfo, tscheck, tssetting,tskodu;
        PictureBox pb;
        Label nimi, aeg, raha, bonus, level, difference, tsekk;
        Sound s, sE;
        Pood pood;
        bool start = false;
        public Account()
        {
            this.Width = 1200;
            this.Height = 900;
            this.Text = "VS Pood | Vihane Sipelgas Konto";
            this.BackgroundImage = Properties.Resources.bg;
            this.Icon = Properties.Resources.Icon;

            MainMenu = new MenuStrip() { Location = new Point(0,0)};
            tsinfo = new ToolStripMenuItem("Info");
            tscheck = new ToolStripMenuItem("Ostud");
            tssetting = new ToolStripMenuItem("Konto sätted");
            tskodu = new ToolStripMenuItem("Kodu");

            MainMenu.Items.AddRange(new ToolStripMenuItem[] {tsinfo, tscheck, tssetting, tskodu});

            pb = new PictureBox() { Visible = false, Size = new Size(400,400), Location = new Point(30,30), BorderStyle = BorderStyle.Fixed3D };
            nimi = new Label() { Visible = false, Location = new Point(pb.Right+150,pb.Top+50), Font = new Font("Arial", 15), Size = new Size(500, 30), BackColor = Color.White, ForeColor = Color.Black, BorderStyle = BorderStyle.Fixed3D};
            aeg = new Label() { Visible = false, Location = new Point(pb.Right + 150, nimi.Top + 100), Font = new Font("Arial", 15), Size = new Size(500, 30), BackColor = Color.White, ForeColor = Color.Black, BorderStyle = BorderStyle.Fixed3D };
            raha = new Label() { Visible = false, Location = new Point(pb.Right + 150, aeg.Top + 100), Font = new Font("Arial", 15), Size = new Size(500, 30), BackColor = Color.White, ForeColor = Color.Black, BorderStyle = BorderStyle.Fixed3D };
            bonus = new Label() { Visible = false, Location = new Point(pb.Right + 150, raha.Top + 100), Font = new Font("Arial", 15), Size = new Size(500, 30), BackColor = Color.White, ForeColor = Color.Black, BorderStyle = BorderStyle.Fixed3D };
            level = new Label() { Visible = false, Location = new Point(pb.Left, pb.Bottom + 30), Font = new Font("Arial", 15), Size = new Size(pb.Width, 30), BackColor = Color.White, ForeColor = Color.Black, BorderStyle = BorderStyle.Fixed3D };
            difference = new Label() { Visible = false, Location = new Point(level.Left, level.Bottom + 30), Font = new Font("Arial", 15), Size = new Size(pb.Width, 30), BackColor = Color.White, ForeColor = Color.Black, BorderStyle = BorderStyle.Fixed3D };
            tsekk = new Label() { Location = new Point(Width/2-160,20) ,Visible = false, Font = new Font("Times New Roman", 60, FontStyle.Italic), Text = "Kviitungid", AutoSize = true, BackColor = Color.Transparent, ForeColor = Color.White, BorderStyle = BorderStyle.Fixed3D};

            GetInfo();
            Tsinfo_Click(new object(), new EventArgs());

            sE = new Sound();
            s = new Sound();
            s.Music();

            tsinfo.Click += Tsinfo_Click;
            tscheck.Click += Tscheck_Click;
            tssetting.Click += Tssetting_Click;
            tskodu.Click += Tskodu_Click;

            Controls.AddRange(new Control[] {MainMenu,nimi,raha,aeg,bonus,pb, level, difference, tsekk});  
        }

        private void Tskodu_Click(object sender, EventArgs e)
        {
            sE.Effect(Properties.Resources.click);
            sE.Stop();
            s.Stop(); //остановил музыку и звуковые эффекты
            //код для запуска новой формы и закрытия этой
            this.Hide();
            pood = new Pood();
            pood.Closed += (s, args) => this.Close();
            pood.Show();
        }

        private void GetInfo()
        {
            string a = Properties.Settings.Default.Account;
            connect.Open(); //получаю данные аккаунта из бд
            adapter_acc = new SqlDataAdapter($"SELECT Username,Time,Money,Cashback FROM Account WHERE Username='{a}'", connect);
            DataTable dt_kat = new DataTable();
            adapter_acc.Fill(dt_kat);
            foreach (DataRow item in dt_kat.Rows)
            {
                nimi.Text ="Nimi: "+ item["Username"].ToString();
                aeg.Text = "Konto vanus: "+ Math.Round((Convert.ToDateTime(DateTime.Now) - Convert.ToDateTime(item["Time"])).TotalDays,0)+" päev/a"; //время которое уже создан аккаунт
                raha.Text = item["Money"].ToString() != "" ? "Kulutatud raha:" + item["Money"].ToString() : "Kulutatud raha: 0";//количество потраченных денег
                bonus.Text = item["Cashback"].ToString()!="" ? "Kogunenud boonused: " +item["Cashback"].ToString() : "Kogunenud boonused: 0";//бонусные деньги от кэшбека
            }
            connect.Close();
        }

        private void VisibleFalse()
        { 
            foreach (Control item in Controls) 
            { 
                item.Visible = false;
            }
            MainMenu.Visible = true;
        }

        private void Tssetting_Click(object sender, EventArgs e)
        {
            sE.Effect(Properties.Resources.click);
            VisibleFalse();
        }

        private void Tscheck_Click(object sender, EventArgs e)
        {
            sE.Effect(Properties.Resources.click);
            VisibleFalse();
            foreach (Control item in new Control[] { tsekk })
            {
                item.Visible = true;
            }
        }

        //если нажали на ToolStrip Info (информация об аккаунте) 
        private void Tsinfo_Click(object sender, EventArgs e)
        {
            if (start)
                sE.Effect(Properties.Resources.click);
            else
                start = true;
            VisibleFalse();
            foreach (Control item in new Control[] { pb, nimi, raha, aeg, bonus, level, difference }) //делаю нужные обьекты видимыми
            {
                item.Visible = true;
            }
            string a = Properties.Settings.Default.Account; //узнаю чей аккаунт войден в профиль
            int time = 0; 
            int money = 0;
            connect.Open(); //получаю данные аккаунта из бд (время и количество потраченных денег)
            adapter_acc = new SqlDataAdapter($"SELECT Time,Money FROM Account WHERE Username='{a}'", connect);
            DataTable dt_kat = new DataTable();
            adapter_acc.Fill(dt_kat);
            foreach (DataRow item in dt_kat.Rows)
            {
                time = (int)Math.Round((Convert.ToDateTime(DateTime.Now) - Convert.ToDateTime(item["Time"])).TotalDays, 0); //разница времени (дата сейчас - дата создания)
                money = Convert.ToInt32(item["Money"]); //количество потраченных денег
            }
            connect.Close();
            //свитч для определения того какой уровень у клиента
            //уровень будет влиять на скидку которую пользователь получит
            int temptime = 0;
            switch (time)
            {
                case int n when (n >= 10000 && money >= 200000):
                    pb.Image = Properties.Resources.client1; level.Text = "Konto tase: 1 | 40% allahindlust";
                    difference.Text = "Maksimaalne tase!"; break;
                case int n when (n >= 5000 && money >= 100000):
                    pb.Image = Properties.Resources.client2; level.Text = "Konto tase: 2 | 25% allahindlust";
                    temptime = (10000 - time) < 0 ? 0 : time;
                    difference.Text = $"Järgmisele tasemele: {temptime}p. {200000-money}e."; break;
                case int n when (n >= 1000 && money >= 20000):
                    pb.Image = Properties.Resources.client3; level.Text = "Konto tase: 3 | 20% allahindlust";
                    temptime = (5000 - time) < 0 ? 0 : time;
                    difference.Text = $"Järgmisele tasemele: {temptime}p. {100000 - money}e."; break;
                case int n when (n >= 365 && money >= 5000):
                    pb.Image = Properties.Resources.client4; level.Text = "Konto tase: 4 | 10% allahindlust";
                    temptime = (1000 - time) < 0 ? 0 : time;
                    difference.Text = $"Järgmisele tasemele: {temptime}p. {20000 - money}e."; break;
                case int n when (n >= 150 && money >= 1000):
                    pb.Image = Properties.Resources.client5; level.Text = "Konto tase: 5 | 5% allahindlust";
                    temptime = (365 - time) < 0 ? 0 : time;
                    difference.Text = $"Järgmisele tasemele: {temptime}p. {5000 - money}e."; break;
                case int n when(n>=50 && money>=100):
                    pb.Image = Properties.Resources.client6; level.Text = "Konto tase: 6 | 1% allahindlust";
                    temptime = (150 - time) < 0 ? 0 : time;
                    difference.Text = $"Järgmisele tasemele: {temptime}p. {1000 - money}e."; break;
            }
            pb.SizeMode = PictureBoxSizeMode.Zoom;
            pb.BackColor = Color.Transparent;
        }
    }
}
