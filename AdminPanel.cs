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
    public partial class AdminPanel : Form
    {
        Button btn1,btn2,btn3;
        ToodedAB toodedAB;
        KontodAB kontodAB;
        Pood pood;
        public AdminPanel()
        {
            InitializeComponent();
            Text = "Admin Panel";
            btn1 = new Button() { Size=new Size(200,200), Text="ToodedAB", Location = new Point(100,150), Font = new Font("Arial",20), BackColor = Color.Gray, ForeColor = Color.White };
            btn2 = new Button() { Size=new Size(200, 200), Text="KontodAB", Location = new Point(btn1.Right+96, 150), Font = new Font("Arial", 20), BackColor = Color.Gray, ForeColor = Color.White };
            btn3 = new Button() { Size=new Size(200, 200), Text="Logi välja", Location = new Point(btn2.Right+96, 150), Font = new Font("Arial", 20), BackColor = Color.Gray, ForeColor = Color.White };

            btn1.Click+=Btn1_Click;
            btn2.Click+=Btn2_Click;
            btn3.Click+=Btn3_Click;

            Controls.AddRange(new Control[] { btn1, btn2, btn3, });
        }

        private void Btn3_Click(object sender, EventArgs e)
        {
            this.Hide();
            pood = new Pood();
            pood.Closed += (s, args) => this.Close();
            pood.Show();
        }

        private void Btn2_Click(object sender, EventArgs e)
        {
            this.Hide();
            kontodAB = new KontodAB();
            kontodAB.Closed += (s, args) => this.Close();
            kontodAB.Show();
        }

        private void Btn1_Click(object sender, EventArgs e)
        {
            this.Hide();
            toodedAB = new ToodedAB();
            toodedAB.Closed += (s, args) => this.Close();
            toodedAB.Show();
        }
    }
}
