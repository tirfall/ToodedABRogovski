using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;
using ToodedAB.Properties;

namespace ToodedAB
{
    public partial class Settings : Form
    {
        Label home;
        public Settings()
        {
            this.Width = 1200;
            this.Height = 900;
            this.Text = "VS Pood | Vihane Sipelgas";
            this.BackgroundImage = Properties.Resources.bg;
            this.Icon = Properties.Resources.Icon;

            Bitmap bmp = new Bitmap(Properties.Resources.house, 200, 200);
            home = new Label() { BorderStyle = BorderStyle.Fixed3D, Image = bmp, Size = new Size(200,200), BackColor = Color.Transparent, Location = new Point(Width/2-100,20)};
            home.MouseHover+=Home_MouseHover;
            home.MouseLeave+=Home_MouseLeave;

            Controls.AddRange(new Control[] { home });

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
