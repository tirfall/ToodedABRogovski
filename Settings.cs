using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Data.SqlClient;
using System.Drawing;
using System.Linq;
using System.Runtime;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;
using System.Xml.Linq;
using ToodedAB.Properties;

namespace ToodedAB
{
    public partial class Settings : Form
    {
        AdminPanel adminPanel;
        Pood pood;
        Label home, sound, account, lsound, login, fpass,reg,log;
        TextBox username, password, password1, hint;
        Button sisse, registr,reset;
        Bitmap bmp, soundbmp;
        TrackBar tbsound;
        UserControl uc, uc1;
        Sound s,sE;
        bool lbclick, pbclick, pb1click, hclick = false; 
        SqlConnection connect = new SqlConnection(@"Data Source=(LocalDB)\MSSQLLocalDB;AttachDbFilename=|DataDirectory|\AppData\Tooded_DB.mdf;Integrated Security=True");
        SqlDataAdapter adapter_acc;
        SqlCommand command;
        Dictionary<string, string[]> acc;
        Account accountForm;
        public Settings()
        {
            this.Width = 1200;
            this.Height = 900;
            this.Text = "VS Pood | Vihane Sipelgas Sätted";
            this.BackgroundImage = Properties.Resources.bg;
            this.Icon = Properties.Resources.Icon;

            bmp = new Bitmap(Properties.Resources.house, 200, 200);
            home = new Label() { BorderStyle = BorderStyle.Fixed3D, Image = bmp, Size = new Size(200,200), BackColor = Color.Transparent, Location = new Point(10,10)};
            home.MouseHover+=Home_MouseHover;
            home.MouseLeave+=Home_MouseLeave;
            home.MouseClick += Home_MouseClick;

            soundbmp = new Bitmap(Properties.Resources.sound, 200, 200);
            sound = new Label() { BorderStyle = BorderStyle.Fixed3D, Image = soundbmp, Size = new Size(200, 200), BackColor = Color.Transparent, Location = new Point(10, home.Bottom+50) };
            sound.MouseHover += Sound_MouseHover;
            sound.MouseClick += Sound_MouseClick;

            bmp = new Bitmap(Properties.Resources.account, 200, 200);
            account = new Label() { BorderStyle = BorderStyle.Fixed3D, Image = bmp, Size = new Size(200, 200), BackColor = Color.Transparent, Location = new Point(10, sound.Bottom + 50) };
            account.MouseHover += Account_MouseHover;
            account.MouseClick += Account_MouseClick;
            account.MouseLeave += Account_MouseLeave;

            lsound = new Label() { Text = "Heli", Font = new Font("Arial", 15), Location = new Point(home.Left + 400, sound.Top), Visible = false,Width=200 };

            tbsound = new TrackBar() { Maximum = 100, Minimum = 0, AutoSize=false,Height = 200,Width=30, Location = new Point(lsound.Left,lsound.Bottom), 
                Value = Convert.ToInt32(Properties.Settings.Default.SoundValue), Visible = false, Orientation=Orientation.Vertical};
            tbsound.ValueChanged += Tb_ValueChanged;
            tbsound.MouseDown += Tbsound_MouseDown;

            Tb_ValueChanged(new object(), new EventArgs());

            uc = new UserControl() { BorderStyle = BorderStyle.Fixed3D, Size= new Size((Width-home.Right-100),(home.Top+account.Bottom)), Location = new Point(home.Right+50,home.Top), BackColor = Color.Transparent, Visible = false };
            uc1 = new UserControl() { Size = new Size((uc.Width/5)*3, (uc.Height / 5) * 4), Location = new Point(uc.Width/5, (uc.Height-(uc.Height / 5) * 4)/2), BackColor = Color.White };
            uc.Controls.Add(uc1);

            sE = new Sound();
            s = new Sound();
            s.Music();

            Log();

            Controls.AddRange(new Control[] { home,sound, tbsound, account, lsound,uc });

        }

        //при нажатии на ползунок для изменения звука
        private void Tbsound_MouseDown(object sender, MouseEventArgs e)
        {
            sE.Effect(Properties.Resources.click); //звуковой эффект клика
        }

        //получаю все аккаунты из базы данных
        private void GetAccounts()
        {
            connect.Open(); //получаю все аккаунты из базы данных
            adapter_acc = new SqlDataAdapter("SELECT Username,Password,Hint FROM Account", connect);
            DataTable dt_kat = new DataTable();
            adapter_acc.Fill(dt_kat);
            acc = new Dictionary<string, string[]>();
            foreach (DataRow item in dt_kat.Rows)
            {
                //Словарь где имя - ключа, а пароль и подсказка это список который является значением для ключа
                acc[item["Username"].ToString()] = new string[] { item["Password"].ToString(), item["Hint"].ToString() };
            }
            connect.Close();
        }

        //UserControl для восстановления пароля
        private void Forgot()
        {
            uc1.Controls.Clear();
            lbclick = false; pbclick = false; pb1click = false; hclick = false;

            login = new Label() { Text = "Parooli taastamine", Font = new Font("Arial", 30), Location = new Point(uc1.Width / 2 - 170, 30), AutoSize = true };

            username = new TextBox() { Text = "Nimi...", Font = new Font("Arial", 30), Size = new Size(uc1.Width - 60, 20), Location = new Point(30, login.Bottom + 100), TextAlign = HorizontalAlignment.Center, ForeColor = Color.Gray, MaxLength = 20 };
            username.MouseHover += Username_MouseHover;
            username.MouseClick += Username_MouseClick;

            hint = new TextBox() { Text = "Vihje...", Font = new Font("Arial", 30), Size = username.Size, Location = new Point(30, username.Bottom + 40), TextAlign = HorizontalAlignment.Center, ForeColor = Color.Gray, MaxLength = 20 };
            hint.MouseHover += hint_MouseHover;
            hint.MouseClick += hint_MouseClick;

            password1 = new TextBox() { Text = "Uue parool...", Font = new Font("Arial", 30), Size = username.Size, Location = new Point(30, hint.Bottom + 40), TextAlign = HorizontalAlignment.Center, ForeColor = Color.Gray, MaxLength = 20 };
            password1.MouseHover += password1_MouseHover;
            password1.MouseClick += password1_MouseClick;

            reset = new Button() { Text = "Taastamine", Font = new Font("Arial", 30), Size = username.Size, Location = new Point(password.Left, uc1.Height - 70), TextAlign = ContentAlignment.MiddleCenter };
            reset.Click += reset_Click;

            log = new Label() { Text = "Mul on konto", Font = new Font("Arial", 15), AutoSize = true, Location = new Point(password.Left, reset.Top - 30), ForeColor = Color.Gray };
            log.MouseHover += log_MouseHover;
            log.MouseLeave += log_MouseLeave;
            log.MouseClick += log_MouseClick;

            reg = new Label() { Text = "Registreeri", Font = new Font("Arial", 15), AutoSize = true, Location = new Point(password.Right - 100, reset.Top - 30), ForeColor = Color.Gray };
            reg.MouseHover += reg_MouseHover;
            reg.MouseLeave += reg_MouseLeave;
            reg.MouseClick += Reg_MouseClick;

            uc1.Controls.AddRange(new Control[] { login, username, hint, password1, reset, log, reg });
        }

        //при нажатии на кнопку восстановления
        private void reset_Click(object sender, EventArgs e)
        {
            GetAccounts(); //получаю все аккаунты из бд
            bool i = false;
            //проверяю что все условия подходят для изменения пароля
            if (!acc.ContainsKey(username.Text))
            {
                username.BackColor = Color.Red;
                hint.BackColor = Color.Red;
                i = true;
            }
            else if (acc[username.Text][1]!=hint.Text.ToString())
            {
                hint.BackColor = Color.Red;
                i = true;
            }
            if (password1.Text.Length<=3)
            {
                hint.BackColor = Color.Red;
                i = true;
            }
            if (i)
                return;
            //изменяю пароль в бд
            command = new SqlCommand("UPDATE Account SET Password=@pass WHERE Username=@nimi", connect);
            connect.Open();
            command.Parameters.AddWithValue("@pass", password1.Text);
            command.Parameters.AddWithValue("@nimi", username.Text);
            command.ExecuteNonQuery();
            connect.Close();
            MessageBox.Show("Konto moodustud", "Edu!", MessageBoxButtons.OK, MessageBoxIcon.Information);
            Log(); //открываю UserControl для входа
        }

        //UserControl для регистрации
        private void Reg()
        {
            uc1.Controls.Clear();
            lbclick = false; pbclick = false; pb1click = false; hclick = false;

            login = new Label() { Text = "Registreeri", Font = new Font("Arial", 30), Location = new Point(uc1.Width / 2 - 100, 30), AutoSize = true };
            
            username = new TextBox() { Text = "Nimi...", Font = new Font("Arial", 30), Size = new Size(uc1.Width - 60, 20), Location = new Point(30, login.Bottom + 60), TextAlign = HorizontalAlignment.Center, ForeColor = Color.Gray, MaxLength = 20 };
            username.MouseHover += Username_MouseHover;
            username.MouseClick += Username_MouseClick;

            password = new TextBox() { Text = "Parool...", Font = new Font("Arial", 30), Size = username.Size, Location = new Point(30, username.Bottom + 40), TextAlign = HorizontalAlignment.Center, ForeColor = Color.Gray, MaxLength = 20 };
            password.MouseHover += password_MouseHover;
            password.MouseClick += password_MouseClick;

            password1 = new TextBox() { Text = "Korda parool...", Font = new Font("Arial", 30), Size = username.Size, Location = new Point(30, password.Bottom + 40), TextAlign = HorizontalAlignment.Center, ForeColor = Color.Gray, MaxLength = 20 };
            password1.MouseHover += password1_MouseHover;
            password1.MouseClick += password1_MouseClick;

            hint = new TextBox() { Text = "Vihje...", Font = new Font("Arial", 30), Size = username.Size, Location = new Point(30, password1.Bottom + 40), TextAlign = HorizontalAlignment.Center, ForeColor = Color.Gray, MaxLength = 20 };
            hint.MouseHover += hint_MouseHover;
            hint.MouseClick += hint_MouseClick;

            registr = new Button() { Text = "Registreeri", Font = new Font("Arial", 30), Size = username.Size, Location = new Point(hint.Left, uc1.Height - 70), TextAlign = ContentAlignment.MiddleCenter };
            registr.Click += registr_Click;

            log = new Label() { Text = "Mul on konto", Font = new Font("Arial", 15), AutoSize = true, Location = new Point(password.Left, sisse.Top - 30), ForeColor = Color.Gray };
            log.MouseHover += log_MouseHover;
            log.MouseLeave += log_MouseLeave;
            log.MouseClick += log_MouseClick;

            uc1.Controls.AddRange(new Control[] { login, username, password, password1, hint, registr, log });
        }

        //UserControl для входа
        private void Log()
        {
            uc1.Controls.Clear();
            lbclick = false; pbclick = false; pb1click = false; hclick = false;

            login = new Label() { Text = "Logi sisse", Font = new Font("Arial", 30), Location = new Point(uc1.Width / 2 - 100, 30), AutoSize = true };

            username = new TextBox() { Text = "Nimi...", Font = new Font("Arial", 30), Size = new Size(uc1.Width - 60, 20), Location = new Point(30, login.Bottom + 100), TextAlign = HorizontalAlignment.Center, ForeColor = Color.Gray, MaxLength = 20 };
            username.MouseHover += Username_MouseHover;
            username.MouseClick += Username_MouseClick;

            password = new TextBox() { Text = "Parool...", Font = new Font("Arial", 30), Size = username.Size, Location = new Point(30, username.Bottom + 70), TextAlign = HorizontalAlignment.Center, ForeColor = Color.Gray, MaxLength = 20 };
            password.MouseHover += password_MouseHover;
            password.MouseClick += password_MouseClick;

            sisse = new Button() { Text = "Sisse", Font = new Font("Arial", 30), Size = username.Size, Location = new Point(password.Left, uc1.Height - 70), TextAlign = ContentAlignment.MiddleCenter };
            sisse.Click += Sisse_Click;

            fpass = new Label() { Text = "Unustasin parooli", Font = new Font("Arial", 15), AutoSize = true, Location = new Point(password.Left, sisse.Top - 30), ForeColor = Color.Gray };
            fpass.MouseHover += Fpass_MouseHover;
            fpass.MouseLeave += Fpass_MouseLeave;
            fpass.MouseClick += Fpass_MouseClick;

            reg = new Label() { Text = "Registreeri", Font = new Font("Arial", 15), AutoSize = true, Location = new Point(password.Right - 100, sisse.Top - 30), ForeColor = Color.Gray };
            reg.MouseHover += reg_MouseHover;
            reg.MouseLeave += reg_MouseLeave;
            reg.MouseClick += Reg_MouseClick;

            uc1.Controls.AddRange(new Control[] { login, username, password, fpass, reg, sisse });
        }

        //при нажатии на лейбл "забыл пароль"
        private void Fpass_MouseClick(object sender, MouseEventArgs e)
        {
            sE.Effect(Properties.Resources.click); //звуковой эффект клика
            Forgot(); //UserControl для восстановления пароля
        }

        //при нажатии на лейбл "У меня есть аккаунт"
        private void log_MouseClick(object sender, MouseEventArgs e)
        {
            sE.Effect(Properties.Resources.click); //звуковой эффект
            Log(); //UserControl для логина
        }

        // если курсор мыши не на лейбле "У меня есть аккаунт"
        private void log_MouseLeave(object sender, EventArgs e)
        {
            log.ForeColor = Color.Gray;
            log.Font = new Font("Arial", 15);
        }

        // при наведении мышки на лейбл "У меня есть аккаунт"
        private void log_MouseHover(object sender, EventArgs e)
        {
            log.ForeColor = Color.Blue;
            log.Font = new Font("Arial", 15, FontStyle.Underline);
        }

        //при нажатии на лейбл "Регистрация"
        private void Reg_MouseClick(object sender, MouseEventArgs e)
        {
            sE.Effect(Properties.Resources.click); //звуковой эффект клика
            Reg(); //UserControl для регистрации
        }

        //Запуск формы
        private void Account()
        {
            sE.Effect(Properties.Resources.click);
            sE.Stop();
            s.Stop();
            this.Hide();
            accountForm = new Account();
            accountForm.Closed += (s, args) => this.Close();
            accountForm.Show();
        }

        //для регистрации пользователя
        private void registr_Click(object sender, EventArgs e)
        {
            sE.Effect(Properties.Resources.click); //звуковой эффект клика
            username.BackColor = Color.White;
            password.BackColor = Color.White;
            password1.BackColor = Color.White;
            hint.BackColor = Color.White;
            GetAccounts(); //Получаю все аккаунты
            bool i = false;
            //Проверка что все данные подходят для создания аккаунта
            if (username.Text.Length <= 3 || acc.ContainsKey(username.Text))
            {
                username.BackColor = Color.Red;
                i = true;
            }
            if (password.Text.Length <= 3)
            {
                password.BackColor = Color.Red;
                i = true;
            }
            if (password1.Text != password.Text || password1.Text.Length <= 3)
            {
                password1.BackColor = Color.Red;
                i = true;
            }
            if (hint.Text.Length <= 3)
            {
                hint.BackColor = Color.Red;
                i = true;
            }
            if (i)
                return;
            //Добавляю аккаунт в базу данных
            command = new SqlCommand("INSERT INTO Account (Username,Password,Hint,Time,Money,Cashback) VALUES (@user,@pass,@hint,@time,@mon,@cash)", connect);
            connect.Open();
            command.Parameters.AddWithValue("@user", username.Text);
            command.Parameters.AddWithValue("@pass", password.Text);
            command.Parameters.AddWithValue("@hint", hint.Text);
            command.Parameters.AddWithValue("@time", DateTime.Now);
            command.Parameters.AddWithValue("@mon", 0);
            command.Parameters.AddWithValue("@cash", 0);
            command.ExecuteNonQuery();
            connect.Close();
            MessageBox.Show("Konto loodud", "Edu!", MessageBoxButtons.OK, MessageBoxIcon.Information);
            Log(); //открываю UserControl для входа
        }

        // при нажатии на текстовый ящик с подсказкой
        private async void hint_MouseClick(object sender, MouseEventArgs e)
        {
            sE.Effect(Properties.Resources.click); //звуковой эффект клика
            hint.MouseHover -= hint_MouseHover; //убираю метод с анимацией
            hclick = true;
            //полное завершение анимации
            await Task.Run(() => {
                while (hint.Text.Contains("Vihje")) 
                { 
                    Invoke((MethodInvoker)delegate { hint.Text = ""; });
                } 
            });
            password.UseSystemPasswordChar = true;
            hint.ForeColor = Color.Black;
        }

        // при наведении мышки на текстовый ящик с подсказкой
        private async void hint_MouseHover(object sender, EventArgs e)
        {
            //Анимация текстового ящика с подсказкой
            do
            {
                sE.Effect(Properties.Resources.text);//звуковой эффект печати
                foreach (string item in new string[] { "Vihje.", "Vihje..", "Vihje..." })
                {
                    hint.Text = item;
                    if (hclick) //проверяем нажимал ли пользователь на текстовый ящик
                        return;
                    await Task.Delay(500);
                    if (!hint.ClientRectangle.Contains(hint.PointToClient(Cursor.Position))) //если курсор мыши не на обьекте
                        break;
                    if (hclick)
                        return;
                }
            } while (hint.ClientRectangle.Contains(hint.PointToClient(Cursor.Position))); //если курсор мыши на обьекте
            hint.Text = "Vihje..."; //статичный текст для текстового ящика с подсказкой
            sE.Stop(); //остановка звукового эффекта
        }

        // при нажатии на текстовый ящик с паролем (текстовый ящик для регистрации)
        private async void password1_MouseClick(object sender, MouseEventArgs e)
        {
            sE.Effect(Properties.Resources.click); //звуковой эффект клика
            password1.MouseHover -= password1_MouseHover; //убираю метод анимации
            pb1click = true;
            string text = password1.Text.Replace("...","");
            //Необходимо для полного завершения анимации
            await Task.Run(() =>
            {
                while (password1.Text.Contains(text))
                {
                    //Для правильной работы кода
                    Invoke((MethodInvoker)delegate {
                        password1.Text = "";
                    });
                }
            });
            password.UseSystemPasswordChar = true;
            password1.ForeColor = Color.Black;
            await Task.Delay(500);
        }

        // при наведении мышки на текстовый ящик с паролем (текстовый ящик для регистрации)
        private async void password1_MouseHover(object sender, EventArgs e)
        {
            //анимация текстового ящика с паролем (текстовый ящик для регистрации)
            string text;
            do
            {
                text = password1.Text.Replace("...","");
                sE.Effect(Properties.Resources.text); //звуковой эффект печати
                foreach (string item in new string[] { text+".", text+"..", text+"..." })
                {
                    password1.Text = item;
                    if (pb1click) //проверяем нажимал ли пользователь на текстовый ящик 
                        return;
                    await Task.Delay(500);
                    if (!password1.ClientRectangle.Contains(password1.PointToClient(Cursor.Position))) //если курсор мыши не на обьекте
                        break;
                    if (pb1click)
                        return;
                }
            } while (password1.ClientRectangle.Contains(password1.PointToClient(Cursor.Position)));
            password1.Text = text+"...";
            sE.Stop();
        }

        // Для входа в аккаунт
        private void Sisse_Click(object sender, EventArgs e)
        {
            sE.Effect(Properties.Resources.click); //звуковой эффект клика
            if (username.Text=="@root" && password.Text=="12345") //запуск админской панели
            {
                sE.Effect(Properties.Resources.click);
                sE.Stop();
                s.Stop();
                this.Hide();
                adminPanel = new AdminPanel();
                adminPanel.Closed += (s, args) => this.Close();
                adminPanel.Show();
            }
            GetAccounts(); //получаю словарь с аккаунтами
            bool i = false;
            if (!acc.ContainsKey(username.Text))
            {
                username.BackColor = Color.Red;
                password.BackColor = Color.Red;
                i = true;
            }
            else if (acc[username.Text][0]!=password.Text)
            {
                password.BackColor = Color.Red;
                i = true;
            }
            if (i)
                return;
            else
            {
                Properties.Settings.Default.Account = username.Text;
                //если в аккаунт вошли то код это запоминает и теперь при каждом запуске он
                //не будет запрашивать логин/регистрацию, а сразу запускать (если надо) форму аккаунта
                Properties.Settings.Default.Log = true;
                Properties.Settings.Default.Save();
                Account();
            }
        }

        // при наведении мышки на лейбл "регистрация"
        private void reg_MouseHover(object sender, EventArgs e)
        {
            reg.ForeColor = Color.Blue;
            reg.Font = new Font("Arial", 15, FontStyle.Underline);
        }

        // если курсор мыши не на лейбле "регистрация"
        private void reg_MouseLeave(object sender, EventArgs e)
        {
            reg.ForeColor = Color.Gray;
            reg.Font = new Font("Arial", 15);
        }

        // если курсор мыши не на лейбле "забыл пароль"
        private void Fpass_MouseLeave(object sender, EventArgs e)
        {
            fpass.ForeColor = Color.Gray;
            fpass.Font = new Font("Arial", 15);
        }

        // при наведении мышки на лейбл "забыл пароль"
        private void Fpass_MouseHover(object sender, EventArgs e)
        {
            fpass.ForeColor = Color.Blue;
            fpass.Font = new Font("Arial", 15, FontStyle.Underline);
        }

        // при нажатии на текстовый ящик с паолем
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
                    Invoke((MethodInvoker)delegate { 
                        password.Text = "";
                    });
                }
            });
            password.UseSystemPasswordChar = true;
            password.ForeColor = Color.Black;
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

        // при нажатии на текстовый ящик с именем
        private async void Username_MouseClick(object sender, MouseEventArgs e)
        {
            sE.Effect(Properties.Resources.click); //звуковой эффект клика
            username.BackColor = Color.White;
            username.MouseHover -= Username_MouseHover; //убираю метод анимации, чтобы он не мешал пользователю
            lbclick = true;
            await Task.Run(() =>
            {
                //Нужно чтобы анимация точно прекратилась
                while (username.Text.Contains("Nimi"))
                {
                    //без него код не видит правильно username
                    Invoke((MethodInvoker)delegate { username.Text = ""; });
                }
            });
            username.ForeColor = Color.Black;
        }

        // при наведении мышки на текстовый ящик с именем
        private async void Username_MouseHover(object sender, EventArgs e)
        {
            //анимация текстового ящика с именем
            do
            {
                sE.Effect(Properties.Resources.text); //звуковой эффект печати
                foreach (string item in new string[]{ "Nimi.", "Nimi..", "Nimi..." })
                {
                    username.Text = item;
                    if (lbclick) //переменная проверяет, нажимали ли уже на текстовый ящик или нет
                        return;
                    await Task.Delay(500);
                    if (!username.ClientRectangle.Contains(username.PointToClient(Cursor.Position))) //если курсор мыши не на обьекте
                        break;
                    if (lbclick)
                        return;
                }
            } while (username.ClientRectangle.Contains(username.PointToClient(Cursor.Position)));//если курсор мыши на обьекте
            username.Text = "Nimi..."; //Статичный текст для текстового ящика
            sE.Stop(); //остановка звукового эффекта
        }

        // если курсор мыши не на иконки аккаунта
        private void Account_MouseLeave(object sender, EventArgs e)
        {
            bmp = new Bitmap(Properties.Resources.account, 200, 200); //Статичная картинка аккаунта
            account.Image = bmp;
            sE.Stop(); //остановка звукового эффекта
        }

        // при нажатии на иконку аккаунта
        private void Account_MouseClick(object sender, MouseEventArgs e)
        {
            sE.Effect(Properties.Resources.click); //Звуковой эффект клика
            switch (uc.Visible) //Свитч в котором - если UserControl для логина/регстрации включен то выключить,
                                //а если выключен то включить его и отключить остальные элементы,
                                //а также проверка входил ли человек в аккаунт до этого, чтобы ему
                                //не пришлось каждый раз при запуске входить в аккаунт
            {
                case true:
                    uc.Visible = false; break;
                case false:
                    uc.Visible = true;
                    tbsound.Visible = false;
                    lsound.Visible = false;
                    if (Properties.Settings.Default.Log)
                    {
                        Account(); //Метод запуска формы аккаунта
                    }
                    break;
            }
        }

        // при наведении мышки на иконку аккаунта
        private void Account_MouseHover(object sender, EventArgs e)
        {
            bmp = new Bitmap(Properties.Resources.account1, 200, 200); //Синяя картинка аккаунта
            account.Image = bmp;
            sE.Effect(Properties.Resources.acc); //Звуковой эффект огненного шара
        }

        // если значение ползунка (звука) изменилось
        private void Tb_ValueChanged(object sender, EventArgs e)
        {
            //Здесь определяется какая статичная картинка будет стоять для иконки звука
            if (tbsound.Value >= 70)
                soundbmp = new Bitmap(Properties.Resources.sound, 200, 200);
            else if (tbsound.Value >= 40)
                soundbmp = new Bitmap(Properties.Resources.sound3, 200, 200);
            else if (tbsound.Value >= 1)
                soundbmp = new Bitmap(Properties.Resources.sound2, 200, 200);
            else if (tbsound.Value == 0)
                soundbmp = new Bitmap(Properties.Resources.sound1, 200, 200);
            sound.Image = soundbmp;
            //Сохраняю это переменную чтобы при перезапуске она никуда не делась
            Properties.Settings.Default.SoundValue = tbsound.Value;
            Properties.Settings.Default.Save();
        }

        // при нажатии на иконку звука
        private void Sound_MouseClick(object sender, MouseEventArgs e)
        {
            sE.Effect(Properties.Resources.click); //звуковой эффект клика
            switch (tbsound.Visible) //Простой свитч - если эллементы звука включены то выключить,
                                     //а если выключены то включить и выключить остальные элементы
            {
                case true:
                    tbsound.Visible = false; 
                    lsound.Visible = false;
                    break;
                case false:
                    tbsound.Visible = true;
                    lsound.Visible = true;
                    uc.Visible = false;
                    break;
            }
        }

        // при наведении мышки на иконку звука
        private async void Sound_MouseHover(object sender, EventArgs e)
        {
            do
            {
                sE.Effect(Properties.Resources.volume); //звуковой эффект "волны"
                //Анимация картинки звука
                foreach (Image item in new Image[] { Properties.Resources.sound1, Properties.Resources.sound2, Properties.Resources.sound3, Properties.Resources.sound })
                {
                    bmp = new Bitmap(item, 200, 200);
                    sound.Image = bmp;
                    if (!sound.ClientRectangle.Contains(sound.PointToClient(Cursor.Position))) //если курсор не на обьекте
                        break;
                    await Task.Delay(500);
                }
            } while (sound.ClientRectangle.Contains(sound.PointToClient(Cursor.Position))); //если курсор не на обьекте
            sound.Image = soundbmp; //статичная картинка звука (зависит от громкости)
            sE.Stop();
        }

        // при нажатии на иконку дома
        private void Home_MouseClick(object sender, MouseEventArgs e)
        {
            sE.Effect(Properties.Resources.click); //звуковый эффект клика)
            sE.Stop();
            s.Stop(); //остановил музыку и звуковые эффекты
            //код для запуска новой формы и закрытия этой
            this.Hide();
            pood = new Pood();
            pood.Closed += (s, args) => this.Close();
            pood.Show();
        }

        // если курсор мыши не на иконки дома
        private void Home_MouseLeave(object sender, EventArgs e)
        {
            Bitmap bmp = new Bitmap(Properties.Resources.house, 200, 200); //Картинка с домом без огней
            home.Image = bmp;
            sE.Stop(); //Отключить звуковый эффект мигающей лампы
        }

        // при наведении мышки на иконку дома
        private void Home_MouseHover(object sender, EventArgs e)
        {
            Bitmap bmp = new Bitmap(Properties.Resources.house1, 200, 200); //Картинка дома с огнями 
            home.Image = bmp;
            sE.Effect(Properties.Resources.light); //Звуковый эффект мигающий лампы
        }
    }
}
