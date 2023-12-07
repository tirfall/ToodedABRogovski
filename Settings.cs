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
        Pood pood;
        Label home, sound, account, lsound, lmusic;
        Bitmap bmp, soundbmp;
        TrackBar tbsound, tbmusic;
        UserControl uc;
        public Settings()
        {
            this.Width = 1200;
            this.Height = 900;
            this.Text = "VS Pood | Vihane Sipelgas";
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

            lsound = new Label() { Text = "Heli", Font = new Font("Arial", 15), Location = new Point(home.Left + 400, home.Location.Y), Visible = false };
            lmusic = new Label() { Text = "Muusik", Font = new Font("Arial", 15), Location = new Point(home.Left + 600 + lsound.Width, home.Location.Y), Visible = false };

            tbsound = new TrackBar() { Maximum = 100, Minimum = 0, AutoSize=false,Height = 200,Width=30, Location = new Point(lsound.Left,lsound.Bottom), 
                Value = Convert.ToInt32(Properties.Settings.Default.SoundValue), Visible = false, Orientation=Orientation.Vertical};
            tbsound.ValueChanged += Tb_ValueChanged;
            tbmusic = new TrackBar() { Maximum = 100, Minimum = 0,AutoSize = false, Height=200,Width = 30,Location = new Point(lmusic.Left, lmusic.Bottom), 
                Value = Convert.ToInt32(Properties.Settings.Default.MusicValue), Visible = false,Orientation = Orientation.Vertical};
            tbmusic.ValueChanged += Tbmusic_ValueChanged;
            Tb_ValueChanged(new object(), new EventArgs());

            Controls.AddRange(new Control[] { home,sound, tbsound, account, tbmusic, lsound, lmusic });

        }

        private void Tbmusic_ValueChanged(object sender, EventArgs e)
        {
            if (tbsound.Value >= 70 && tbmusic.Value >= 70)
                soundbmp = new Bitmap(Properties.Resources.sound, 200, 200);
            else if (tbsound.Value >= 40 && tbmusic.Value >= 40)
                soundbmp = new Bitmap(Properties.Resources.sound3, 200, 200);
            else if (tbsound.Value >= 1 && tbmusic.Value >= 1)
                soundbmp = new Bitmap(Properties.Resources.sound2, 200, 200);
            else if (tbsound.Value == 0 && tbmusic.Value == 0)
                soundbmp = new Bitmap(Properties.Resources.sound1, 200, 200);
            sound.Image = soundbmp;
            Properties.Settings.Default.MusicValue = tbmusic.Value;
            Properties.Settings.Default.Save();
        }

        private void Account_MouseLeave(object sender, EventArgs e)
        {
            bmp = new Bitmap(Properties.Resources.account, 200, 200);
            account.Image = bmp;
        }

        private void Account_MouseClick(object sender, MouseEventArgs e)
        {
            throw new NotImplementedException();
        }

        private void Account_MouseHover(object sender, EventArgs e)
        {
            bmp = new Bitmap(Properties.Resources.account1, 200, 200);
            account.Image = bmp;
        }

        private void Tb_ValueChanged(object sender, EventArgs e)
        {
            if (tbsound.Value >= 70 && tbmusic.Value >= 70)
                soundbmp = new Bitmap(Properties.Resources.sound, 200, 200);
            else if (tbsound.Value >= 40 && tbmusic.Value >= 40)
                soundbmp = new Bitmap(Properties.Resources.sound3, 200, 200);
            else if (tbsound.Value >= 1 && tbmusic.Value >= 1)
                soundbmp = new Bitmap(Properties.Resources.sound2, 200, 200);
            else if (tbsound.Value == 0 && tbmusic.Value == 0)
                soundbmp = new Bitmap(Properties.Resources.sound1, 200, 200);
            sound.Image = soundbmp;
            Properties.Settings.Default.SoundValue = tbsound.Value;
            Properties.Settings.Default.Save();
        }

        private void Sound_MouseClick(object sender, MouseEventArgs e)
        {
            switch (tbsound.Visible)
            {
                case true:
                    tbsound.Visible = false; 
                    tbmusic.Visible = false;
                    lsound.Visible = false;
                    lmusic.Visible = false;
                    break;
                case false:
                    tbsound.Visible = true;
                    tbmusic.Visible = true;
                    lsound.Visible = true;
                    lmusic.Visible = true;
                    break;
            }
        }

        private async void Sound_MouseHover(object sender, EventArgs e)
        {
            do
            {
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
        }

        private void Home_MouseClick(object sender, MouseEventArgs e)
        {
            this.Hide();
            pood = new Pood();
            pood.Closed += (s, args) => this.Close();
            pood.Show();
        }

        private void Home_MouseLeave(object sender, EventArgs e)
        {
            Bitmap bmp = new Bitmap(Properties.Resources.house, 200, 200);
            home.Image = bmp;
        }

        private void Home_MouseHover(object sender, EventArgs e)
        {
            Bitmap bmp = new Bitmap(Properties.Resources.house1, 200, 200);
            home.Image = bmp;
        }
    }
}
