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
        Button start;
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

            Controls.AddRange(new Control[] { title, start });
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
