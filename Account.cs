using System;
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
using ToodedAB.Properties;

namespace ToodedAB
{
    public partial class Account : Form
    {
        SqlConnection connect = new SqlConnection(@"Data Source=(LocalDB)\MSSQLLocalDB;AttachDbFilename=|DataDirectory|\AppData\Tooded_DB.mdf;Integrated Security=True");
        SqlDataAdapter adapter_acc;
        SqlCommand command;
        MenuStrip MainMenu;
        ToolStripMenuItem tsinfo, tscheck, tssetting, tskodu;
        PictureBox pb;
        Label nimi, aeg, raha, bonus, level, difference, tsekk, hint, pass;
        TextBox tbNimi, tbPass, tbHint, password;
        Sound s, sE;
        Pood pood;
        Button p1, p2, p3, p4, p5, p6, p7, p8, p9, p10, p11, p12, p13, p14, p15, p16, p17, p18, p19, p20, p21, p22, p23, p24, p25, p26, p27, p28, eye1, eye2, eye3, ok1, ok2, ok3, close1, close2, close3, login, logout, ok;
        bool log, pbclick = false;
        bool c1, c2, c3 = true;
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
            this.BackgroundImage = Properties.Resources.bg;
            this.Icon = Properties.Resources.Icon;

            MainMenu = new MenuStrip() { Location = new Point(0, 0) };
            tsinfo = new ToolStripMenuItem("Info");
            tscheck = new ToolStripMenuItem("Ostud");
            tssetting = new ToolStripMenuItem("Konto sätted");
            tskodu = new ToolStripMenuItem("Kodu");

            MainMenu.Items.AddRange(new ToolStripMenuItem[] { tsinfo, tscheck, tssetting, tskodu });

            pb = new PictureBox() { Size = new Size(400, 400), Location = new Point(30, 30), BorderStyle = BorderStyle.Fixed3D };
            nimi = new Label() { Font = new Font("Arial", 15), ForeColor = Color.Black };
            aeg = new Label() { Font = new Font("Arial", 15), Size = new Size(500, 30) };
            raha = new Label() { Font = new Font("Arial", 15), Size = new Size(500, 30) };
            bonus = new Label() { Location = new Point(pb.Right + 150, raha.Top + 100), Font = new Font("Arial", 15), Size = new Size(500, 30), BorderStyle = BorderStyle.Fixed3D };
            level = new Label() { Location = new Point(pb.Left, pb.Bottom + 30), Font = new Font("Arial", 15), Size = new Size(pb.Width, 30), BorderStyle = BorderStyle.Fixed3D };
            difference = new Label() { Location = new Point(level.Left, level.Bottom + 30), Font = new Font("Arial", 15), Size = new Size(pb.Width, 30), BorderStyle = BorderStyle.Fixed3D };
            tsekk = new Label() { Location = new Point(Width / 2 - 160, 20), Font = new Font("Times New Roman", 60, FontStyle.Italic), Text = "Kviitungid", AutoSize = true, BackColor = Color.Transparent, ForeColor = Color.White, BorderStyle = BorderStyle.Fixed3D };
            pass = new Label() { Font = new Font("Arial", 15), Size = new Size(100, 30), BorderStyle = BorderStyle.Fixed3D };
            hint = new Label() { Font = new Font("Arial", 15), Size = new Size(100, 30), BorderStyle = BorderStyle.Fixed3D };

            tbNimi = new TextBox() { Font = new Font("Arial", 15), Size = new Size(500, 30), BorderStyle = BorderStyle.Fixed3D };
            tbPass = new TextBox() { Font = new Font("Arial", 15), Size = new Size(500, 30), BorderStyle = BorderStyle.Fixed3D };
            tbHint = new TextBox() { Font = new Font("Arial", 15), Size = new Size(500, 30), BorderStyle = BorderStyle.Fixed3D };

            p1 = new Button(); p2 = new Button(); p3 = new Button(); p4 = new Button(); p5 = new Button(); p6 = new Button();
            p7 = new Button(); p8 = new Button(); p9 = new Button(); p10 = new Button(); p11 = new Button(); p12 = new Button();
            p13 = new Button(); p14 = new Button(); p15 = new Button(); p16 = new Button(); p17 = new Button(); p18 = new Button();
            p19 = new Button(); p20 = new Button(); p21 = new Button(); p22 = new Button(); p23 = new Button(); p24 = new Button();
            p25 = new Button(); p26 = new Button(); p27 = new Button(); p28 = new Button();
            //обьявил все кнопки так, так как иначе невозможно (нельзя это сделать во время написания их выше и также в цикле)

            Bitmap bmp = Properties.Resources.eye;
            bmp.MakeTransparent(Color.White);
            eye1 = new Button() { BackgroundImage = bmp, Size = new Size(45, 45), BackgroundImageLayout = ImageLayout.Zoom };
            eye1.MouseHover +=Eye_MouseHover;
            eye1.MouseClick +=Eye1_MouseClick;
            eye2 = new Button() { BackgroundImage = bmp, Size = new Size(45, 45), BackgroundImageLayout = ImageLayout.Zoom };
            eye2.MouseHover +=Eye_MouseHover;
            eye2.MouseClick +=Eye2_MouseClick;
            eye3 = new Button() { BackgroundImage = bmp, Size = new Size(45, 45), BackgroundImageLayout = ImageLayout.Zoom };
            eye3.MouseHover +=Eye_MouseHover;
            eye3.MouseClick +=Eye3_MouseClick;

            ok1 = new Button(); ok2 = new Button(); ok3 = new Button();
            foreach (Button item in new Button[] { ok1, ok2, ok3 })
            {
                item.BackgroundImage = Properties.Resources.ok;
                item.Size = new Size(45, 45);
                item.BackgroundImageLayout = ImageLayout.Zoom;
                item.MouseHover+=OkN_MouseHover;
                item.MouseLeave+=OkN_MouseLeave;
            }

            close1 = new Button(); close2 = new Button(); close3 = new Button();
            foreach (Button item in new Button[] { close1, close2, close3 })
            {
                item.BackgroundImage = Properties.Resources.cancel;
                item.Size = new Size(45, 45);
                item.BackgroundImageLayout = ImageLayout.Zoom;
                item.MouseHover +=Close_MouseHover;
                item.MouseLeave +=Close_MouseLeave;
            }
            close1.MouseClick += Close1_MouseClick;
            close2.MouseClick += Close2_MouseClick;
            close3.MouseClick += Close3_MouseClick;

            login = new Button() { Text = "Logi sisse", Size = new Size(200, 50), Font = new Font("Arial", 20) };
            login.MouseHover +=Label_MouseHover;
            login.Click += Login_Click;
            logout = new Button() { Text = "Logi välja", Size = login.Size, Font = new Font("Arial", 20) };
            logout.MouseHover += Label_MouseHover;
            logout.Click += Logout_Click;

            ok = new Button() { Text = "OK", Size = new Size(100, 100), Font = new Font("Arial", 20) };
            password = new TextBox() { Text = "Parool...", Font = new Font("Arial", 20), Size = new Size(500, 30), TextAlign = HorizontalAlignment.Center, ForeColor = Color.Gray, MaxLength = 20 };

            password.MouseHover += password_MouseHover;
            password.MouseClick += password_MouseClick;
            ok.MouseHover +=Ok_MouseHover;
            ok.MouseLeave += Ok_MouseLeave;
            ok.MouseClick += Ok_MouseClick;

            p1.Click += Button_Click; p2.Click += Button_Click; p3.Click += Button_Click; p4.Click += Button_Click; p5.Click += Button_Click;
            p4.Click += Button_Click; p5.Click += Button_Click; p6.Click += Button_Click; p7.Click += Button_Click; p8.Click += Button_Click;
            p9.Click += Button_Click; p10.Click += Button_Click; p11.Click += Button_Click; p12.Click += Button_Click; p13.Click += Button_Click;
            p14.Click += Button_Click; p15.Click += Button_Click; p16.Click += Button_Click; p17.Click += Button_Click; p18.Click += Button_Click;
            p19.Click += Button_Click; p20.Click += Button_Click; p21.Click += Button_Click; p22.Click += Button_Click; p23.Click += Button_Click;
            p24.Click += Button_Click; p25.Click += Button_Click; p26.Click += Button_Click; p27.Click += Button_Click; p28.Click += Button_Click;
            
            temp = new Button[] { p1, p2, p3, p4, p5, p6, p7, p8, p9, p10, p11, p12, p13, p14, p15, p16, p17, p18, p19, p20, p21, p22, p23, p24, p25, p26, p27, p28, null };
            //создал список для удобства
            for (int i = 0, j = 150, k = 0; i < temp.Length - 1; i++)
            // i = элемент списка, j = Y элемента, k = "столбец" элемента
            {
                j += i % 7 == 0 && i != 0 ? 150 : 0; //если элемент кратен 7 то высота меняется, переходим на следующую строку
                k = i % 7 == 0 && i != 0 ? 0 : k; //если элемент кратен 7 то обнуляется столбец элемента
                //значение k нужно для значения Х чтобы указывать под каким элементом должен стоять другой
                temp[i].Size = new Size(100, 100);
                temp[i].BackColor = Color.White;
                temp[i].Location = new Point((k - 1 >= 0 ? temp[k - 1] : temp[temp.Length - 1])?.Right + 50 ?? 50, j);
                //если k-1 не равно 0 (в списке не бывает отрицательных значений) то берем значение Х у правого края этого элемента и прибавляем 50
                // если k-1 равно 0 то берем последнее значение списка (null) и за счет условия ?. который проверяет значение на null и 
                // ?? которое ставит значение 50, если значение к которому применили условие равно null 
                k++;
                temp[i].Text = (i + 1).ToString();
            }

            sE = new Sound();
            s = new Sound();
            s.Music();

            tsinfo.Click += Tsinfo_Click;
            tscheck.Click += Tscheck_Click;
            tssetting.Click += Tssetting_Click;
            tskodu.Click += Tskodu_Click;

            Controls.AddRange(new Control[] { MainMenu, nimi, raha, aeg, bonus, pb, level, difference, tsekk, pass, hint, tbNimi, 
                tbPass, tbHint, eye2, eye1, eye3, login, logout,password, ok, ok1, ok2, ok3, close1, close2, close3});
            Controls.AddRange(temp);
            VisibleFalse();
            Initialize(Properties.Settings.Default.Account);
            Tsinfo_Click(new object(), new EventArgs());
        }

        private void Close3_MouseClick(object sender, MouseEventArgs e)
        {
            throw new NotImplementedException();
        }

        private void Close2_MouseClick(object sender, MouseEventArgs e)
        {
            throw new NotImplementedException();
        }

        private void Close1_MouseClick(object sender, MouseEventArgs e)
        {
            throw new NotImplementedException();
        }

        private void OkN_MouseLeave(object sender, EventArgs e)
        {
            if (sender is Button a)
            {
                a.BackgroundImage = Properties.Resources.ok;
            }
        }

        private void OkN_MouseHover(object sender, EventArgs e)
        {
            if (sender is Button a)
            {
                a.BackgroundImage = Properties.Resources.ok1;
            }
        }

        private void Close_MouseLeave(object sender, EventArgs e)
        {
            if (sender is Button a)
            {
                a.BackgroundImage = Properties.Resources.cancel;
            }
        }

        private void Close_MouseHover(object sender, EventArgs e)
        {
            if (sender is Button a)
            {
                a.BackgroundImage = Properties.Resources.cancel1;
            }
        }

        private void Ok_MouseClick(object sender, MouseEventArgs e)
        {
            sE.Effect(Properties.Resources.click);
            if (password.Text==Password)
            {
                log = true;
                MessageBox.Show("Edu!", "Logi sisse", MessageBoxButtons.OK, MessageBoxIcon.Information);
                password.Visible = false;
                ok.Visible = false;
            }
        }

        private void Eye3_MouseClick(object sender, MouseEventArgs e)
        {
            sE.Effect(Properties.Resources.click);
            if (ok3.Visible)
            {
                tbHint.Enabled = false;
                ok3.Visible = false;
                close3.Visible = false;
                tbHint.UseSystemPasswordChar = true;
            }
            else if (log && !ok3.Visible)
            {
                tbHint.Enabled = true;
                ok3.Visible = true;
                close3.Visible = true;
                tbHint.UseSystemPasswordChar = false;
            }
        }

        private void Eye2_MouseClick(object sender, MouseEventArgs e)
        {
            sE.Effect(Properties.Resources.click);
            if (ok2.Visible)
            {
                tbPass.Enabled = false;
                ok2.Visible = false;
                close2.Visible = false;
                tbPass.UseSystemPasswordChar = true;
            }
            else if (log && !ok2.Visible)
            {
                tbPass.Enabled = true;
                ok2.Visible = true;
                close2.Visible = true;
                tbPass.UseSystemPasswordChar = false;
            }
        }

        private void Eye1_MouseClick(object sender, MouseEventArgs e)
        {
            sE.Effect(Properties.Resources.click);
            if (ok1.Visible)
            {
                tbNimi.Enabled = false;
                ok1.Visible = false;
                close1.Visible = false;
            }
            else if (log && !ok1.Visible)
            {
                tbNimi.Enabled = true;
                ok1.Visible = true;
                close1.Visible = true;
            }
        }

        private void Login_Click(object sender, EventArgs e)
        {
            password.Visible = true;
            ok.Visible = true;
            sE.Effect(Properties.Resources.click);
            password.Location = new Point(login.Right+50, login.Location.Y);
            ok.Location = new Point(password.Right+25, password.Location.Y-25);
        }

        private void Ok_MouseLeave(object sender, EventArgs e)
        {
            ok.BackColor = Color.White;
            ok.ForeColor = Color.Black;
        }

        private void Ok_MouseHover(object sender, EventArgs e)
        {
            ok.BackColor = Color.Green;
            ok.ForeColor = Color.White;
        }

        private async void password_MouseClick(object sender, MouseEventArgs e)
        {
            sE.Effect(Properties.Resources.click); //звуковой эффект клика
            password.BackColor = Color.White;
            password.MouseHover -= password_MouseHover; //убираю метод анимации чтобы не мешал пользователю
            pbclick = true;
            //метод для полной очистки анимации
            await Task.Run(() =>
            {
                while (password.Text.Contains("Parool"))
                {
                    //нужен для корректной работы
                    Invoke((MethodInvoker)delegate { password.Text = ""; });
                }
            });
            password.ForeColor = Color.Black;
            await Task.Delay(500);
        }

        // при наведении мышки на текстовый ящик с паролем
        private async void password_MouseHover(object sender, EventArgs e)
        {
            //Анимация для текстового ящика с паролем
            do
            {
                sE.Effect(Properties.Resources.text);
                foreach (string item in new string[] { "Parool.", "Parool..", "Parool..." })
                {
                    password.Text = item;
                    if (pbclick) //переменная которая проверяет нажимали ли на текстовый ящик 
                        return;
                    await Task.Delay(500);
                    if (!password.ClientRectangle.Contains(password.PointToClient(Cursor.Position))) //если курсор мыши не на обьекте
                        break;
                    if (pbclick)
                        return;
                }
            } while (password.ClientRectangle.Contains(password.PointToClient(Cursor.Position))); //если курсор мыши на обьекте
            password.Text = "Parool..."; //Статичный текст для текствого ящика с паролем
            sE.Stop(); //остановка звукового эффекта
        }

        private void Logout_Click(object sender, EventArgs e)
        {
            Properties.Settings.Default.Log = false;
            Properties.Settings.Default.Save();
            Tskodu_Click(new object(),new EventArgs());
        }

        private async void Label_MouseHover(object sender, EventArgs e)
        {
            if (sender is Button btn)
            {
                string text = btn.Text;
                do
                {
                    sE.Effect(Properties.Resources.text); //звуковой эффект печати
                    btn.Text = "";
                    foreach (char item in text.ToCharArray())
                    {
                        btn.Text += item;
                        if (!btn.ClientRectangle.Contains(btn.PointToClient(Cursor.Position))) //если курсор мыши не на обьекте
                            break;
                        await Task.Delay(250);
                    }
                } while (btn.ClientRectangle.Contains(btn.PointToClient(Cursor.Position))); //если курсор мыши на обьекте
                btn.Text = text; //статичный текст для кнопки
                sE.Stop();
            }
        }


        //доделать
        //
        //
        //
        //
        //
        //
        //
        private async void Eye_MouseHover(object sender, EventArgs e)
        {
            Bitmap[] animate = new Bitmap[] {Properties.Resources.eye, Properties.Resources.eye1, Properties.Resources.eye2,
                Properties.Resources.eye3, Properties.Resources.eye2,Properties.Resources.eye1,Properties.Resources.eye,Properties.Resources.eye4,
                Properties.Resources.eye5, Properties.Resources.eye6, Properties.Resources.eye3, Properties.Resources.eye6, Properties.Resources.eye7, Properties.Resources.eye8, Properties.Resources.eye
            };
            if (sender is Button eye)
            { 
                do
                {
                    sE.Effect(Properties.Resources.gear); //звуковой эффект моргания 
                    foreach (Bitmap item in animate)
                    {
                        Bitmap bmp = new Bitmap(item);
                        bmp.MakeTransparent(Color.White); //делаю прозрачным фон
                        eye.BackgroundImage = bmp;
                        eye.BackgroundImageLayout = ImageLayout.Zoom;
                        if (!eye.ClientRectangle.Contains(eye.PointToClient(Cursor.Position)))
                            break;
                        await Task.Delay(100);
                        if (!eye.ClientRectangle.Contains(eye.PointToClient(Cursor.Position)))
                            break;
                    }
                } while (eye.ClientRectangle.Contains(eye.PointToClient(Cursor.Position))); //если курсор мыши на обьекте
                eye.BackgroundImage = Properties.Resources.eye; //Дефолтная статичная картинка для кнопки
                sE.Stop();
            }   
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
                    pb.Image = Properties.Resources.client1; level.Text = "Konto tase: 1 | 40% allahindlust";
                    difference.Text = "Maksimaalne tase!"; break;
                case 25:
                    pb.Image = Properties.Resources.client2; level.Text = "Konto tase: 2 | 25% allahindlust";
                    temptime = (10000 - Time) > 0 ? 10000 - Time : 0;
                    difference.Text = $"Järgmisele tasemele: {temptime}p. {200000 - Money}e."; break;
                case 20:
                    pb.Image = Properties.Resources.client3; level.Text = "Konto tase: 3 | 20% allahindlust";
                    temptime = (5000 - Time) > 0 ? 5000 - Time : 0;
                    difference.Text = $"Järgmisele tasemele: {temptime}p. {100000 - Money}e."; break;
                case 10:
                    pb.Image = Properties.Resources.client4; level.Text = "Konto tase: 4 | 10% allahindlust";
                    temptime = (1000 - Time) > 0 ? 1000 - Time : 0; 
                    difference.Text = $"Järgmisele tasemele: {temptime}p. {20000 - Money}e."; break;
                case 5:
                    Properties.Settings.Default.Discount = 1;
                    pb.Image = Properties.Resources.client5; level.Text = "Konto tase: 5 | 5% allahindlust";
                    temptime = (365 - Time) > 0 ? 365 - Time : 0;
                    difference.Text = $"Järgmisele tasemele: {temptime}p. {5000 - Money}e."; break;
                case 1:
                    pb.Image = Properties.Resources.client6; level.Text = "Konto tase: 6 | 1% allahindlust";
                    temptime = (150 - Time) > 0 ? 150 - Time : 0;
                    difference.Text = $"Järgmisele tasemele: {temptime}p. {1000 - Money}e."; break;
            }
        }

        //определяем скидку клиента
        private void GetDiscount()
        {
            //свитч для определения того какой уровень у клиента (тут сразу определяется скидка)
            //уровень будет влиять на скидку которую пользователь получит
            switch (Time)
            {
                case int n when (n >= 10000 && Money >= 200000):
                    Discount = 40; break;
                case int n when (n >= 5000 && Money >= 100000):
                    Discount = 25; break;
                case int n when (n >= 1000 && Money >= 20000):
                    Discount = 20; break;
                case int n when (n >= 365 && Money >= 5000):
                    Discount = 10; break;
                case int n when (n >= 150 && Money >= 1000):
                    Discount = 5; break;
                case int n when (n >= 0 && Money >= 0):
                    Discount = 1; break;
            }
        }

        
        private void SettingSet()
        {
            nimi.Size = new Size(100, 30);
            Label[] labels = new Label[] { nimi, pass, hint};
            TextBox[] textboxes = new TextBox[] { tbNimi, tbPass, tbHint };
            Button[] buttons = new Button[] { eye1, eye2, eye3};
            Button[] buttons1 = new Button[] {ok1, ok2, ok3 };
            Button[] buttons2 = new Button[] { close1, close2, close3 };
            foreach (Control item in new Control[] { nimi, pass, hint, tbNimi, tbPass, tbHint, eye1, eye2, eye3,login, logout }) //делаю нужные обьекты видимыми
            {
                item.Visible = true;
            }
            for (int i = 0; i < labels.Length; i++) //настройка лейблов
            {
                labels[i].BorderStyle = BorderStyle.Fixed3D;
                labels[i].Location = new Point(50, i - 1 >= 0 ? labels[i - 1].Bottom + 50 : 50);
            }
            nimi.Text = "Nimi: ";
            tbNimi.Text = Nimi;
            pass.Text = "Parool: ";
            tbPass.Text = Password;
            hint.Text = "Vihje: ";
            tbHint.Text = Hint;
            for (int i = 0; i < textboxes.Length; i++)
            {
                textboxes[i].BorderStyle = BorderStyle.Fixed3D;
                textboxes[i].Location = new Point(labels[i].Right+25, labels[i].Location.Y);
                textboxes[i].UseSystemPasswordChar = i > 0 ? true: false;
                textboxes[i].Enabled = false;
            }
            for (int i = 0; i < buttons.Length; i++ )
            {
                 buttons[i].Location = new Point(textboxes[i].Right+25, textboxes[i].Location.Y-10);
            }
            for (int i = 0; i < buttons.Length; i++)
            {
                buttons1[i].Location = new Point(buttons[i].Right+25, buttons[i].Location.Y);
            }
            for (int i = 0; i < buttons.Length; i++)
            {
                buttons2[i].Location = new Point(buttons1[i].Right+25, buttons1[i].Location.Y);
            }
            logout.Location = new Point(nimi.Left,nimi.Bottom+500);
            login.Location = new Point(nimi.Left, logout.Bottom+50);
        }

        //настройки для ToolStrip Info
        private void InfoSet()
        {
            nimi.Size = new Size(500, 30);
            foreach (Control item in new Control[] { pb, nimi, raha, aeg, bonus, level, difference }) //делаю нужные обьекты видимыми
            {
                item.Visible = true;
            }
            Label[] labels = new Label[] { nimi, aeg, raha, bonus };
            for (int i = 0; i < labels.Length; i++) //настройка лейблов
            {
                labels[i].BorderStyle = BorderStyle.Fixed3D;
                labels[i].Location = new Point(pb.Right + 150, i-1>=0 ? labels[i-1].Top+100 : pb.Top+50);
                //Локация лейбла будет равна справа от PictureBox на 150 и
                //Значение Y будет равно +100 от вершмны предыдущего лейбла если i-1 больше или равно 0, если
                // i-1 меньше 0 то тогда значение Y будет равно +50 относительно вершины PictureBox 
                //(под это условие попадает только nimi)
            }
            nimi.Text = "Nimi: "+Nimi;
            raha.Text = "Kuulutud raha: "+Money.ToString()+" euro/t";
            aeg.Text = "Konto eluiga: "+Time.ToString()+" päev/a";
            bonus.Text = "Boonusraha: "+Cashback.ToString() + " euro/t";
            //Пишу всю информацию в лейблы
        }

        //если нажали на какую-то из кнопок в разделе чеки
        private void Button_Click(object sender, EventArgs e)
        {
            try
            {
                sE.Effect(Properties.Resources.click); //звуковой эффект клика
                if (sender is Button a) //проверяем что отправитель Кнопка и вместе с этим получаем новую переменную "а"
                {
                    string[] f = AppDomain.CurrentDomain.BaseDirectory.Split('\\'); //беру текущий путь проекта и создаю из него списка деля строку по \\
                    f = new string[] { f[0], f[1], f[2], f[3], f[4], f[5], f[6] }; //беру именно эти значения тк следующие это бин и дебаг, а они лишние
                    System.Diagnostics.Process.Start(String.Join("\\", f) + "\\Resources\\p"+a.Text+".pdf");//обьединяю список в строку
                                                                                                            //и добавляю еще один нужный путь
                                                                                                            //"а" - кнопка которая вызвала ивент
                }
            }
            catch (Exception) { return;  }
        }

        //ToolStrip чтобы вернуться в главное меню
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

        //Сделать все обьекты невидимыми кроме главного меню сверху
        private void VisibleFalse()
        { 
            foreach (Control item in Controls) 
            { 
                item.Visible = false;
            }
            MainMenu.Visible = true;
        }

        //настройки аккаунта
        private void Tssetting_Click(object sender, EventArgs e)
        {
            sE.Effect(Properties.Resources.click);
            VisibleFalse(); //делаю все обьекты невидимыми 
            SettingSet(); //Устанавливаю нужные лейблы, текстовые ящики и их настройки
        }

        //если нажали на ToolStrip Ostud (чеки) 
        private void Tscheck_Click(object sender, EventArgs e)
        {
            sE.Effect(Properties.Resources.click); //звуковой эффект клика
            VisibleFalse();
            foreach (Control item in temp) // делаю видимыми все кнопки
            {
                if (item != null)
                    item.Visible = true;
            }
            tsekk.Visible = true;
        }

        //если нажали на ToolStrip Info (информация об аккаунте) 
        private void Tsinfo_Click(object sender, EventArgs e)   
        {
            sE?.Effect(Properties.Resources.click); //если sE не равен null то вызываем звуковой эфффект клика
            VisibleFalse(); //делаю все обьекты невидимыми 
            InfoSet(); //Устанавливаю нужные лейблы, картинки и их настройки
            //Получаем информацию о уровне клиента
            GetInfo();
            pb.SizeMode = PictureBoxSizeMode.Zoom;
            pb.BackColor = Color.Transparent; 
        }
    }
}
