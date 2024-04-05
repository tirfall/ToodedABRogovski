﻿using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Configuration;
using System.Data;
using System.Data.SqlClient;
using System.Drawing;
using System.IO;
using System.Linq;
using System.Runtime;
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
        ToolStripMenuItem tsinfo, tskodu;
        Label nimi, aeg, raha, bonus, level, hint, pass;
        Pood pood;
        Button logout;
        bool log, pbclick = false;
        static Button[] temp;
        public Stack<int> fails { get; set; }
        public string Nimi { get; private set; }
        public string Password { get; private set; }
        public string Hint { get; private set; }
        public int Time { get; private set; }
        public int Money { get; private set; }
        public int Cashback { get; private set; }
        public int Discount { get; private set; }

        //Конструктор для формы
        public Account()
        {
            this.Width = 1200;
            this.Height = 900;
            this.Text = "VS Pood | Vihane Sipelgas Konto";
            this.Icon = Properties.Resources.Icon;
            this.FormBorderStyle = FormBorderStyle.FixedSingle;
            this.MaximizeBox = false;
            this.MinimizeBox = false;

            MainMenu = new MenuStrip() { Location = new Point(0, 0) };
            tsinfo = new ToolStripMenuItem("Info");
            tskodu = new ToolStripMenuItem("Kodu");

            MainMenu.Items.AddRange(new ToolStripMenuItem[] { tsinfo,tskodu });

            nimi = new Label() { Font = new Font("Arial", 15), ForeColor = Color.Black };
            aeg = new Label() { Font = new Font("Arial", 15), Size = new Size(500, 30) };
            raha = new Label() { Font = new Font("Arial", 15), Size = new Size(500, 30) };
            bonus = new Label() { Location = new Point(400, raha.Top + 100), Font = new Font("Arial", 15), Size = new Size(500, 30), BorderStyle = BorderStyle.Fixed3D };
            level = new Label() { Location = new Point(50, 30), Font = new Font("Arial", 15), Size = new Size(300, 30), BorderStyle = BorderStyle.Fixed3D };

            logout = new Button() { Text = "Logi välja", Size = new Size(200, 50), Font = new Font("Arial", 20) };
            logout.Click += Logout_Click;

            tsinfo.Click += Tsinfo_Click;
            tskodu.Click += Tskodu_Click;

            Controls.AddRange(new Control[] { MainMenu, nimi, raha, aeg, bonus, level, logout});
            
            VisibleFalse();
            Initialize(Properties.Settings.Default.Account);
            Tsinfo_Click(new object(), new EventArgs());
        }

        private void Logout_Click(object sender, EventArgs e)
        {
            Properties.Settings.Default.Log = false;
            Properties.Settings.Default.Save();
            Tskodu_Click(new object(),new EventArgs());
        }


        //запускаю заполнение параметров обьекта
        //добавил отдельный метод так как мне нужны эти данные во время работы формы и 
        //чтобы не создавать обьект класса внутри себя же - добавил просто метод
        private void Initialize(string nimi)
        {
            connect.Open(); //получаю данные аккаунта из бд (время и количество потраченных денег)
            adapter_acc = new SqlDataAdapter($"SELECT Username,Password,Hint,Time,Money,Cashback FROM Account WHERE Username='{nimi}'", connect);
            DataTable dt_kat = new DataTable();
            adapter_acc.Fill(dt_kat);
            foreach (DataRow item in dt_kat.Rows)
            {
                Nimi = item["Username"].ToString();
                Password = item["Password"].ToString();
                Hint = item["Hint"].ToString();
                Time = (int)Math.Round((Convert.ToDateTime(DateTime.Now) - ((item["Time"] as DateTime?) ?? DateTime.Now)).TotalDays, 0); //разница времени (дата сейчас - дата создания)
                Money = (item["Money"] as int?) ?? 0; //количество потраченных денег
                Cashback = (item["Cashback"] as int?) ?? 0; //кэшбек
                // (item as type?) ?? default value
                // проверяю что предмет из списка можно конвертировать в определенный тип данных
                //и значение предмета не равно null, если же всё же равно то устанавливаю разное дефолтное значение
            }
            connect.Close();
            GetDiscount();
        }

        //Конструктор для взаимодействия с аккаунтом
        public Account(string nimi)
        {
            //заполнение параметров обьекта
            Initialize(nimi);
        }

        //Получаем информацию о уровне клиента, скидки и сколько дней и денег нужно еще до следующего уровня
        private void GetInfo()
        {
            int temptime;
            //по сути это тот же свитч что и в GetDiscount(), но такое оформление более лучше следует правилам ООП - SRP
            //Тут вместо уже того безумного условия мы берем просто скидку и уже определяем какой текст вставить в текстовые ящики
            //если бы метод был единым то при определенных обстоятельствах мы бы просто так нагружали программу излишними процессами
            switch (Discount)
            {
                case 40:
                    level.Text = "Konto tase: 1 | 40% allahindlust"; break;
                case 25:
                    level.Text = "Konto tase: 2 | 25% allahindlust"; break;
                case 20:
                    level.Text = "Konto tase: 3 | 20% allahindlust"; break;
                case 10:
                    level.Text = "Konto tase: 4 | 10% allahindlust";  break;
                case 5:
                    Properties.Settings.Default.Discount = 1;
                    level.Text = "Konto tase: 5 | 5% allahindlust"; break;
                case 1:
                    level.Text = "Konto tase: 6 | 1% allahindlust"; break;
            }
        }

        //определяем скидку клиента
        private void GetDiscount()
        {
            switch (Time)
            {
                case int n when (n >= 10000):
                    Discount = 40; break;
                case int n when (n >= 5000):
                    Discount = 25; break;
                case int n when (n >= 1000):
                    Discount = 20; break;
                case int n when (n >= 365):
                    Discount = 10; break;
                case int n when (n >= 150):
                    Discount = 5; break;
                case int n when (n >= 0):
                    Discount = 1; break;
            }
        }

        

        //настройки для ToolStrip Info
        private void InfoSet()
        {
            nimi.Size = new Size(500, 30);
            foreach (Control item in new Control[] { nimi, raha, aeg, bonus, level, logout }) //делаю нужные обьекты видимыми
            {
                item.Visible = true;
            }
            Label[] labels = new Label[] { nimi, aeg, raha, bonus };
            for (int i = 0; i < labels.Length; i++) //настройка лейблов
            {
                labels[i].BorderStyle = BorderStyle.Fixed3D;
                labels[i].Location = new Point(700, i-1>=0 ? labels[i-1].Top+100 : 50);
            }
            nimi.Text = "Nimi: "+Nimi;
            raha.Text = "Kuulutud raha: "+Money.ToString()+" euro/t";
            aeg.Text = "Konto eluiga: "+Time.ToString()+" päev/a";
            bonus.Text = "Boonusraha: "+Cashback.ToString() + " euro/t";
            logout.Location = new Point(50, 800);
        }

        private void Tskodu_Click(object sender, EventArgs e)
        {
            
            this.Hide();
            pood = new Pood();
            pood.Closed += (s, args) => this.Close();
            pood.Show();
        }

        private void VisibleFalse()
        { 
            foreach (Control item in Controls) 
            { 
                item.Visible = false;
            }
            MainMenu.Visible = true;
        }


        private void Tsinfo_Click(object sender, EventArgs e)   
        {
            VisibleFalse(); 
            InfoSet(); 
            GetInfo();
        }
    }
}
