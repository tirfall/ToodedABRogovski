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
        Label home, sound, account, lsound;
        Bitmap bmp, soundbmp;
        TrackBar tbsound;
        UserControl uc;
        Sound s,sE;
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

            sE = new Sound();
            s = new Sound();
            s.Music();

            Controls.AddRange(new Control[] { home,sound, tbsound, account, lsound });

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
            throw new NotImplementedException();
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
