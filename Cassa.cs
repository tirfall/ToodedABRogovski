using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Data.SqlClient;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;

namespace ToodedAB
{
    public partial class Cassa : Form
    {
        ListBox lb;
        public Cassa()
        {
            this.Width = 1200;
            this.Height = 900;
            this.Text = "VS Pood | Vihane Sipelgas Kassa";
            this.BackgroundImage = Properties.Resources.bg;
            this.Icon = Properties.Resources.Icon;
            this.FormBorderStyle = FormBorderStyle.FixedSingle;
            this.MaximizeBox = false;
            this.MinimizeBox = false;

            lb = new ListBox() { Size = new Size(370,810), Location = new Point(30,15)};

            Controls.AddRange(new Control[] {lb });
        }

        private void ListBoxFill()
        {

        }
    }
}
