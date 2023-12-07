using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Runtime;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;
using ToodedAB.Properties;

namespace ToodedAB
{
    public partial class Settings : Form
    {
        ToodedAB tab;
        Pood pood;
        Label home, sound, account, lsound, login, fpass,reg;
        TextBox username, password;
        Button sisse;
        Bitmap bmp, soundbmp;
        TrackBar tbsound;
        UserControl uc, uc1;
        Sound s,sE;
        bool lbclick, pbclick = false;
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
            Tb_ValueChanged(new object(), new EventArgs());

            uc = new UserControl() { BorderStyle = BorderStyle.Fixed3D, Size= new Size((Width-home.Right-100),(home.Top+account.Bottom)), Location = new Point(home.Right+50,home.Top), BackColor = Color.Transparent, Visible = false };
            uc1 = new UserControl() { Size = new Size((uc.Width/5)*3, (uc.Height / 5) * 4), Location = new Point(uc.Width/5, (uc.Height-(uc.Height / 5) * 4)/2), BackColor = Color.White };
            uc.Controls.Add(uc1);

            login = new Label() { Text = "Logi sisse", Font = new Font("Arial",30), Location = new Point(uc1.Width/2-100,30), AutoSize = true };

            username = new TextBox() { Text = "Isikukood või nimi...", Font = new Font("Arial",30), Size=new Size(uc1.Width-60,20),Location = new Point(30,login.Bottom+100),TextAlign=HorizontalAlignment.Center,ForeColor=Color.Gray, MaxLength=20 };
            username.MouseHover += Username_MouseHover;
            username.MouseClick += Username_MouseClick;

            password = new TextBox() { Text = "Parool...", Font = new Font("Arial", 30), Size = username.Size, Location = new Point(30, username.Bottom + 70), TextAlign = HorizontalAlignment.Center, ForeColor = Color.Gray, MaxLength = 20 };
            password.MouseHover += password_MouseHover;
            password.MouseClick += password_MouseClick;

            sisse = new Button() { Text = "Sisse", Font = new Font("Arial", 30), Size = username.Size, Location = new Point(password.Left,uc1.Height-70),TextAlign=ContentAlignment.MiddleCenter };
            sisse.Click += Sisse_Click;

            fpass = new Label() { Text = "Unustasin parooli", Font = new Font("Arial", 15), AutoSize = true, Location = new Point(password.Left, sisse.Top-30), ForeColor = Color.Gray };
            fpass.MouseHover += Fpass_MouseHover;
            fpass.MouseLeave += Fpass_MouseLeave;

            reg = new Label() { Text = "Registreeri", Font = new Font("Arial", 15), AutoSize = true, Location = new Point(password.Right - 100, sisse.Top - 30), ForeColor = Color.Gray };
            reg.MouseHover += reg_MouseHover;
            reg.MouseLeave += reg_MouseLeave;

            uc1.Controls.AddRange(new Control[] { login,username, password, fpass,reg,sisse });

            sE = new Sound();
            s = new Sound();
            s.Music();

            Controls.AddRange(new Control[] { home,sound, tbsound, account, lsound,uc });

        }

        private void Sisse_Click(object sender, EventArgs e)
        {
            if (username.Text=="@root" && password.Text=="12345")
            {
                sE.Effect(Properties.Resources.click);
                sE.Stop();
                s.Stop();
                this.Hide();
                tab = new ToodedAB();
                tab.Closed += (s, args) => this.Close();
                tab.Show();
            }
        }

        private void reg_MouseHover(object sender, EventArgs e)
        {
            reg.ForeColor = Color.Blue;
            reg.Font = new Font("Arial", 15, FontStyle.Underline);
        }

        private void reg_MouseLeave(object sender, EventArgs e)
        {
            reg.ForeColor = Color.Gray;
            reg.Font = new Font("Arial", 15);
        }

        private void Fpass_MouseLeave(object sender, EventArgs e)
        {
            fpass.ForeColor = Color.Gray;
            fpass.Font = new Font("Arial", 15);
        }

        private void Fpass_MouseHover(object sender, EventArgs e)
        {
            fpass.ForeColor = Color.Blue;
            fpass.Font = new Font("Arial", 15, FontStyle.Underline);
        }

        private async void password_MouseClick(object sender, MouseEventArgs e)
        {
            password.MouseHover -= password_MouseHover;
            pbclick = true;
            await Task.Run(() =>
            {
                while (password.Text.Contains("Parool"))
                {
                    Invoke((MethodInvoker)delegate { password.Text = ""; });
                }
            });
            password.ForeColor = Color.Black;
            await Task.Delay(1000);
            password.UseSystemPasswordChar = true;
        }

        private async void password_MouseHover(object sender, EventArgs e)
        {
            do
            {
                foreach (string item in new string[] { "Parool.", "Parool..", "Parool..." })
                {
                    password.Text = item; 
                    if (pbclick)
                        return;
                    await Task.Delay(500);
                    if (!password.ClientRectangle.Contains(password.PointToClient(Cursor.Position)))
                        break;
                    if (pbclick)
                        return;
                }
            } while (password.ClientRectangle.Contains(password.PointToClient(Cursor.Position)));
            password.Text = "Parool...";
        }

        private async void Username_MouseClick(object sender, MouseEventArgs e)
        {
            username.MouseHover -= Username_MouseHover;
            lbclick = true;
            await Task.Run(() =>
            {
                while (username.Text.Contains("Isikukood või nimi"))
                {
                    Invoke((MethodInvoker)delegate { username.Text = ""; });
                }
            });
            username.ForeColor = Color.Black;
        }

        private async void Username_MouseHover(object sender, EventArgs e)
        {
            do
            {
                foreach (string item in new string[]{ "Isikukood või nimi.", "Isikukood või nimi..", "Isikukood või nimi..." })
                {
                    username.Text = item;
                    if (pbclick)
                        return;
                    await Task.Delay(500);
                    if (!username.ClientRectangle.Contains(username.PointToClient(Cursor.Position)))
                        break;
                    if (lbclick)
                        return;
                }
            } while (username.ClientRectangle.Contains(username.PointToClient(Cursor.Position)));
            username.Text = "Isikukood või nimi...";
        }

        private void Account_MouseLeave(object sender, EventArgs e)
        {
            bmp = new Bitmap(Properties.Resources.account, 200, 200);
            account.Image = bmp;
            sE.Stop();
        }

        private void Account_MouseClick(object sender, MouseEventArgs e)
        {
            sE.Effect(Properties.Resources.click);
            switch (uc.Visible)
            {
                case true:
                    uc.Visible = false; break;
                case false:
                    uc.Visible = true;
                    tbsound.Visible = false;
                    lsound.Visible = false; break;
            }
        }

        private void Account_MouseHover(object sender, EventArgs e)
        {
            bmp = new Bitmap(Properties.Resources.account1, 200, 200);
            account.Image = bmp;
            sE.Effect(Properties.Resources.acc);
        }

        private void Tb_ValueChanged(object sender, EventArgs e)
        {
            if (tbsound.Value >= 70)
                soundbmp = new Bitmap(Properties.Resources.sound, 200, 200);
            else if (tbsound.Value >= 40)
                soundbmp = new Bitmap(Properties.Resources.sound3, 200, 200);
            else if (tbsound.Value >= 1)
                soundbmp = new Bitmap(Properties.Resources.sound2, 200, 200);
            else if (tbsound.Value == 0)
                soundbmp = new Bitmap(Properties.Resources.sound1, 200, 200);
            sound.Image = soundbmp;
            Properties.Settings.Default.SoundValue = tbsound.Value;
            Properties.Settings.Default.Save();
        }

        private void Sound_MouseClick(object sender, MouseEventArgs e)
        {
            sE.Effect(Properties.Resources.click);
            switch (tbsound.Visible)
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

        private async void Sound_MouseHover(object sender, EventArgs e)
        {
            do
            {
                sE.Effect(Properties.Resources.volume);
                foreach (Image item in new Image[] { Properties.Resources.sound1, Properties.Resources.sound2, Properties.Resources.sound3, Properties.Resources.sound })
                {
                    bmp = new Bitmap(item, 200, 200);
                    sound.Image = bmp;
                    if (!sound.ClientRectangle.Contains(sound.PointToClient(Cursor.Position)))
                        break;
                    await Task.Delay(500);
                }
            } while (sound.ClientRectangle.Contains(sound.PointToClient(Cursor.Position)));
            sound.Image = soundbmp;
            sE.Stop();
        }

        private void Home_MouseClick(object sender, MouseEventArgs e)
        {
            sE.Effect(Properties.Resources.click);
            sE.Stop();
            s.Stop();
            this.Hide();
            pood = new Pood();
            pood.Closed += (s, args) => this.Close();
            pood.Show();
        }

        private void Home_MouseLeave(object sender, EventArgs e)
        {
            Bitmap bmp = new Bitmap(Properties.Resources.house, 200, 200);
            home.Image = bmp;
            sE.Stop();
        }

        private void Home_MouseHover(object sender, EventArgs e)
        {
            Bitmap bmp = new Bitmap(Properties.Resources.house1, 200, 200);
            home.Image = bmp;
            sE.Effect(Properties.Resources.light);
        }
    }
}
