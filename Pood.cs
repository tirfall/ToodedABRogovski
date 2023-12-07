using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Text;
using System.Windows.Forms;
using System.Threading.Tasks;

namespace ToodedAB
{
    public partial class Pood : Form
    {
        Label title;
        Button start, setting, exit;
        Settings settings;
        Main main;
        Sound s;
        public Pood()
        {
            this.Width = 1200;
            this.Height = 900;
            this.Text = "VS Pood | Vihane Sipelgas";
            this.BackgroundImage = Properties.Resources.bg;
            this.Icon = Properties.Resources.Icon;
            title = new Label() { Font = new Font("Times New Roman", 60, FontStyle.Italic), Text = "Vihane Sipelgas", AutoSize = true, BackColor = Color.Transparent, 
                ForeColor = Color.White, BorderStyle = BorderStyle.Fixed3D};
            title.Location = Location = new Point((this.Width-600) / 2, 30);
            start = new Button() { BackgroundImage = Properties.Resources.cart, Size = new Size(600,70), BackgroundImageLayout = ImageLayout.Zoom };
            start.Location = new Point((this.Width - start.Width) / 2-20, title.Bottom+start.Height+200);
            start.MouseHover += Start_MouseHover;
            setting = new Button() { BackgroundImage = Properties.Resources.set, Size = new Size(600, 70), BackgroundImageLayout = ImageLayout.Zoom };
            setting.Location = new Point((this.Width - start.Width) / 2-20, start.Bottom+title.Height+50);
            setting.MouseHover +=Setting_MouseHover;
            exit = new Button() { Text = "EXIT", Size = new Size(600, 70), Font = new Font("Arial",30, FontStyle.Bold) };
            exit.Location = new Point((this.Width - start.Width) / 2-20, setting.Bottom+title.Height+50);
            exit.MouseHover+=Exit_MouseHover;

            exit.MouseClick+=Exit_MouseClick;
            start.MouseClick+=Start_MouseClick;
            setting.MouseClick+=Setting_MouseClick;

            s = new Sound();
            s.Music();

            Controls.AddRange(new Control[] { title, start, setting, exit });
        }

        private void Setting_MouseClick(object sender, MouseEventArgs e)
        {
            s.StopMusic(s.waveOut);
            this.Hide();
            settings = new Settings();
            settings.Closed += (s, args) => this.Close();
            settings.Show();
        }

        private void Start_MouseClick(object sender, MouseEventArgs e)
        {
            this.Hide();
            main = new Main();
            main.Closed += (s, args) => this.Close();
            main.Show();
        }

        private void Exit_MouseClick(object sender, MouseEventArgs e)
        {
            Close();
        }

        private async void Exit_MouseHover(object sender, EventArgs e)
        {
            do
            {
                exit.Text = "";
                foreach (string item in new string[] { " E ", " X ", " I ", " T " })
                {
                    exit.Text += item;
                    if (!exit.ClientRectangle.Contains(exit.PointToClient(Cursor.Position)))
                        break;
                    await Task.Delay(500);
                    
                }
            } while (exit.ClientRectangle.Contains(exit.PointToClient(Cursor.Position)));
            exit.Text = "EXIT";
        }

        private async void Setting_MouseHover(object sender, EventArgs e)
        {
            do
            {
                foreach (Image item in new Image[] { Properties.Resources.set1, Properties.Resources.set2, Properties.Resources.set3})
                {
                    Bitmap bmp = new Bitmap(item);
                    bmp.MakeTransparent(Color.White);
                    setting.BackgroundImage = bmp;
                    await Task.Delay(40);
                }
            } while (setting.ClientRectangle.Contains(setting.PointToClient(Cursor.Position)));
            setting.BackgroundImage = Properties.Resources.set;
        }

        private async void Start_MouseHover(object sender, EventArgs e)
        {
            do
            {
                foreach (Image item in new Image[] { Properties.Resources.cart1, Properties.Resources.cart2, Properties.Resources.cart3, Properties.Resources.cart4, Properties.Resources.cart5, Properties.Resources.cart6 })
                {
                    start.BackgroundImage = item;
                    await Task.Delay(80);
                }
            } while (start.ClientRectangle.Contains(start.PointToClient(Cursor.Position)));
            start.BackgroundImage = Properties.Resources.cart;
        }
    }
}
