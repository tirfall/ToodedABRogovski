using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;

namespace ToodedAB
{
    public partial class Account : Form
    {
        MenuStrip MainMenu;
        ToolStripMenuItem tsinfo, tscheck, tssetting;
        public Account()
        {
            this.Width = 1200;
            this.Height = 900;
            this.Text = "VS Pood | Vihane Sipelgas Konto";
            this.BackgroundImage = Properties.Resources.bg;
            this.Icon = Properties.Resources.Icon;

            MainMenu = new MenuStrip() { Location = new Point(0,0)};
            tsinfo = new ToolStripMenuItem();
            tscheck = new ToolStripMenuItem();
            tssetting = new ToolStripMenuItem();

            MainMenu.Items.AddRange(new ToolStripMenuItem[] {tsinfo, tscheck, tssetting});

            Controls.AddRange(new Control[] {MainMenu});  
        }
    }
}
