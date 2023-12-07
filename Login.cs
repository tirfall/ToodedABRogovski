using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Text;
using System.Windows.Forms;

namespace ToodedAB
{
    public partial class Login : Form
    {
        public Login()
        {
            this.Width = 1200;
            this.Height = 900;
            this.Text = "VS Pood | Vihane Sipelgas Logi Sisse";
            this.BackgroundImage = Properties.Resources.bg;
            this.Icon = Properties.Resources.Icon;
        }
    }
}
