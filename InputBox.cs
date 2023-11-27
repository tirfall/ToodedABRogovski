using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Text;
using System.Windows.Forms;

namespace ToodedAB
{
    public partial class InputBox : Form
    {
        Label lb;
        TextBox tb;
        Button btn;
        public int num { get; set; }
        public InputBox(string text,string title)
        {
            this.Width = 200;
            this.Height = 100;
            Text = title;
            lb = new Label() { Text = text, Location = new Point(10,10), Font = new Font("Arial",10), AutoSize = true};
            tb = new TextBox() { Location = new Point(10, Height - 70), Font = new Font("Arial", 10) };
            btn = new Button() { Text = "OK", Location = new Point(Width-90, Height - 70), Font = new Font("Arial", 10) };
            btn.Click += Btn_Click;
            this.Controls.AddRange(new Control[] {lb,tb,btn});
        }

        private void Btn_Click(object sender, EventArgs e)
        {
            num = Convert.ToInt32(tb.Text);
            this.Close();
        }
    }
}
