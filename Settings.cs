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
        Label home, logi, login, fpass,reg,log;
        TextBox username, password, password1, hint;
        Button sisse, registr,reset;
        TrackBar tbsound;
        UserControl uc, uc1;
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
            this.BackColor = Color.White;
            this.Icon = Properties.Resources.Icon;
            this.FormBorderStyle = FormBorderStyle.FixedSingle;
            this.MaximizeBox = false;
            this.MinimizeBox = false;

            
            home = new Label() { 
                BorderStyle = BorderStyle.Fixed3D,
                BackColor = Color.Transparent, 
                Text = "Tagasi",
                Location = new Point(10,10),
                Font = new Font("Arial", 24, FontStyle.Bold),
                Size = new Size(140,40)
            };
            home.MouseClick += Home_MouseClick;

            logi = new Label() { 
                BorderStyle = BorderStyle.Fixed3D,
                BackColor = Color.Transparent, 
                Location = new Point(10, 50),
                Text = "Logi sisse",
                Font = new Font("Arial", 24, FontStyle.Bold),
                Size = new Size(190, 40)
            };
            logi.MouseClick += Account_MouseClick;

            uc = new UserControl() { BorderStyle = BorderStyle.Fixed3D, Size= new Size((Width-home.Right-100),(home.Top+700)), Location = new Point(home.Right+50,home.Top), BackColor = Color.Transparent, Visible = false };
            uc1 = new UserControl() { Size = new Size((uc.Width/5)*3, (uc.Height / 5) * 4), Location = new Point(uc.Width/5, (uc.Height-(uc.Height / 5) * 4)/2), BackColor = Color.White };
            uc.Controls.Add(uc1);

            Log();

            Controls.AddRange(new Control[] { home, tbsound, logi, uc });

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

            login = new Label() { Text = "Parooli taastamine", Font = new Font("Arial", 30), Location = new Point(uc1.Width / 2 - 170, 30), AutoSize = true };

            username = new TextBox() { Text = "Nimi...", Font = new Font("Arial", 30), Size = new Size(uc1.Width - 60, 20), Location = new Point(30, login.Bottom + 100), TextAlign = HorizontalAlignment.Center, ForeColor = Color.Gray, MaxLength = 20 };
            username.MouseClick += Username_MouseClick;

            hint = new TextBox() { Text = "Vihje...", Font = new Font("Arial", 30), Size = username.Size, Location = new Point(30, username.Bottom + 40), TextAlign = HorizontalAlignment.Center, ForeColor = Color.Gray, MaxLength = 20 };
            hint.MouseClick += hint_MouseClick;

            password1 = new TextBox() { Text = "Uue parool...", Font = new Font("Arial", 30), Size = username.Size, Location = new Point(30, hint.Bottom + 40), TextAlign = HorizontalAlignment.Center, ForeColor = Color.Gray, MaxLength = 20 };
            password1.MouseClick += password1_MouseClick;

            reset = new Button() { Text = "Taastamine", Font = new Font("Arial", 30), Size = username.Size, Location = new Point(password.Left, uc1.Height - 70), TextAlign = ContentAlignment.MiddleCenter };
            reset.Click += reset_Click;

            log = new Label() { Text = "Mul on konto", Font = new Font("Arial", 15), AutoSize = true, Location = new Point(password.Left, reset.Top - 30), ForeColor = Color.Gray };
            log.MouseClick += log_MouseClick;

            reg = new Label() { Text = "Registreeri", Font = new Font("Arial", 15), AutoSize = true, Location = new Point(password.Right - 100, reset.Top - 30), ForeColor = Color.Gray };
            reg.MouseClick += Reg_MouseClick;

            uc1.Controls.AddRange(new Control[] { login, username, hint, password1, reset, log, reg });
        }

        //при нажатии на кнопку восстановления
        private void reset_Click(object sender, EventArgs e)
        {
            GetAccounts(); //получаю все аккаунты из бд
            //проверяю что все условия подходят для изменения пароля
            if (!acc.ContainsKey(username.Text))
            {
                username.BackColor = Color.Red;
                hint.BackColor = Color.Red;
                return;
            }
            else if (acc[username.Text][1]!=hint.Text.ToString())
            {
                hint.BackColor = Color.Red;
                return;
            }
            if (password1.Text.Length<=3)
            {
                hint.BackColor = Color.Red;
                return;
            }
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

            login = new Label() { Text = "Registreeri", Font = new Font("Arial", 30), Location = new Point(uc1.Width / 2 - 100, 30), AutoSize = true };
            
            username = new TextBox() { Text = "Nimi...", Font = new Font("Arial", 30), Size = new Size(uc1.Width - 60, 20), Location = new Point(30, login.Bottom + 60), TextAlign = HorizontalAlignment.Center, ForeColor = Color.Gray, MaxLength = 20 };
            username.MouseClick += Username_MouseClick;

            password = new TextBox() { Text = "Parool...", Font = new Font("Arial", 30), Size = username.Size, Location = new Point(30, username.Bottom + 40), TextAlign = HorizontalAlignment.Center, ForeColor = Color.Gray, MaxLength = 20 };
            password.MouseClick += password_MouseClick;

            password1 = new TextBox() { Text = "Korda parool...", Font = new Font("Arial", 30), Size = username.Size, Location = new Point(30, password.Bottom + 40), TextAlign = HorizontalAlignment.Center, ForeColor = Color.Gray, MaxLength = 20 };
            password1.MouseClick += password1_MouseClick;

            hint = new TextBox() { Text = "Vihje...", Font = new Font("Arial", 30), Size = username.Size, Location = new Point(30, password1.Bottom + 40), TextAlign = HorizontalAlignment.Center, ForeColor = Color.Gray, MaxLength = 20 };
            hint.MouseClick += hint_MouseClick;

            registr = new Button() { Text = "Registreeri", Font = new Font("Arial", 30), Size = username.Size, Location = new Point(hint.Left, uc1.Height - 70), TextAlign = ContentAlignment.MiddleCenter };
            registr.Click += registr_Click;

            log = new Label() { Text = "Mul on konto", Font = new Font("Arial", 15), AutoSize = true, Location = new Point(password.Left, sisse.Top - 30), ForeColor = Color.Gray };
            log.MouseClick += log_MouseClick;

            uc1.Controls.AddRange(new Control[] { login, username, password, password1, hint, registr, log });
        }

        //UserControl для входа
        private void Log()
        {
            uc1.Controls.Clear();

            login = new Label() { Text = "Logi sisse", Font = new Font("Arial", 30), Location = new Point(uc1.Width / 2 - 100, 30), AutoSize = true };

            username = new TextBox() { Text = "Nimi...", Font = new Font("Arial", 30), Size = new Size(uc1.Width - 60, 20), Location = new Point(30, login.Bottom + 100), TextAlign = HorizontalAlignment.Center, ForeColor = Color.Gray, MaxLength = 20 };
            username.MouseClick += Username_MouseClick;

            password = new TextBox() { Text = "Parool...", Font = new Font("Arial", 30), Size = username.Size, Location = new Point(30, username.Bottom + 70), TextAlign = HorizontalAlignment.Center, ForeColor = Color.Gray, MaxLength = 20 };
            password.MouseClick += password_MouseClick;

            sisse = new Button() { Text = "Sisse", Font = new Font("Arial", 30), Size = username.Size, Location = new Point(password.Left, uc1.Height - 70), TextAlign = ContentAlignment.MiddleCenter };
            sisse.Click += Sisse_Click;

            fpass = new Label() { Text = "Unustasin parooli", Font = new Font("Arial", 15), AutoSize = true, Location = new Point(password.Left, sisse.Top - 30), ForeColor = Color.Gray };
            fpass.MouseClick += Fpass_MouseClick;

            reg = new Label() { Text = "Registreeri", Font = new Font("Arial", 15), AutoSize = true, Location = new Point(password.Right - 100, sisse.Top - 30), ForeColor = Color.Gray };
            reg.MouseClick += Reg_MouseClick;

            uc1.Controls.AddRange(new Control[] { login, username, password, fpass, reg, sisse });
        }

        //при нажатии на лейбл "забыл пароль"
        private void Fpass_MouseClick(object sender, MouseEventArgs e)
        {
            Forgot(); //UserControl для восстановления пароля
        }

        //при нажатии на лейбл "У меня есть аккаунт"
        private void log_MouseClick(object sender, MouseEventArgs e)
        {
            Log(); //UserControl для логина
        }

        

        //при нажатии на лейбл "Регистрация"
        private void Reg_MouseClick(object sender, MouseEventArgs e)
        {
            Reg(); //UserControl для регистрации
        }

        //Запуск формы
        private void Account()
        {
            this.Hide();
            accountForm = new Account();
            accountForm.Closed += (s, args) => this.Close();
            accountForm.Show();
        }

        //для регистрации пользователя
        private void registr_Click(object sender, EventArgs e)
        {
            username.BackColor = Color.White;
            password.BackColor = Color.White;
            password1.BackColor = Color.White;
            hint.BackColor = Color.White;
            GetAccounts(); //Получаю все аккаунты
            //Проверка что все данные подходят для создания аккаунта
            if (username.Text.Length <= 3 || acc.ContainsKey(username.Text))
            {
                username.BackColor = Color.Red;
                return;
            }
            if (password.Text.Length <= 3)
            {
                password.BackColor = Color.Red;
                return;
            }
            if (password1.Text != password.Text || password1.Text.Length <= 3)
            {
                password1.BackColor = Color.Red;
                return;
            }
            if (hint.Text.Length <= 3)
            {
                hint.BackColor = Color.Red;
                return;
            }
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

        // при нажатии на текстовый ящик с паролем (текстовый ящик для регистрации)
        private async void password1_MouseClick(object sender, MouseEventArgs e)
        {
            
            
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
            password1.UseSystemPasswordChar = true;
            password1.ForeColor = Color.Black;
            await Task.Delay(500);
        }

        

        // Для входа в аккаунт
        private void Sisse_Click(object sender, EventArgs e)
        {
            if (username.Text=="admin" && password.Text=="admin") //запуск админской панели
            {
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

        // при нажатии на текстовый ящик с паролем
        private async void password_MouseClick(object sender, MouseEventArgs e)
        {
            password.BackColor = Color.White;
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

        // при нажатии на текстовый ящик с именем
        private async void Username_MouseClick(object sender, MouseEventArgs e)
        {
            username.BackColor = Color.White;
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


        // при нажатии на иконку аккаунта
        private void Account_MouseClick(object sender, MouseEventArgs e)
        {
            switch (uc.Visible) //Свитч в котором - если UserControl для логина/регстрации включен то выключить,
                                //а если выключен то включить его и отключить остальные элементы,
                                //а также проверка входил ли человек в аккаунт до этого, чтобы ему
                                //не пришлось каждый раз при запуске входить в аккаунт
            {
                case true:
                    uc.Visible = false; break;
                case false:
                    uc.Visible = true;
                    if (Properties.Settings.Default.Log)
                    {
                        Account(); //Метод запуска формы аккаунта
                    }
                    break;
            }
        }

        // при нажатии на иконку дома
        private void Home_MouseClick(object sender, MouseEventArgs e)
        {
            //код для запуска новой формы и закрытия этой
            this.Hide();
            pood = new Pood();
            pood.Closed += (s, args) => this.Close();
            pood.Show();
        }

    }
}
