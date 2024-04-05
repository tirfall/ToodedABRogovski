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
        Button start, konto, exit;
        Settings settings;
        Main main;
        
        public Pood()
        {
            this.Width = 1200;
            this.Height = 900;
            this.Text = "VS Pood";
            this.BackColor = Color.White;
            this.FormBorderStyle = FormBorderStyle.FixedSingle;
            this.MaximizeBox = false;
            this.MinimizeBox = false;
            title = new Label() { Font = new Font("Arial", 60, FontStyle.Bold), Text = "VS Pood", AutoSize = true, BackColor = Color.Transparent};
            title.Location = Location = new Point((this.Width-400) / 2, 30);
            start = new Button() { BackColor=Color.White, Size = new Size(600,70), Text="Pood", Font = new Font("Arial", 30, FontStyle.Bold) };
            start.Location = new Point((this.Width - start.Width) / 2-20, title.Bottom+start.Height+200);
            konto = new Button() { BackColor = Color.White, Size = new Size(600, 70), Text = "Konto", Font = new Font("Arial", 30, FontStyle.Bold) };
            konto.Location = new Point((this.Width - start.Width) / 2-20, start.Bottom+title.Height+50);
            exit = new Button() { Text = "EXIT", Size = new Size(600, 70), Font = new Font("Arial",30, FontStyle.Bold) };
            exit.Location = new Point((this.Width - start.Width) / 2-20, konto.Bottom+title.Height+50);

            exit.MouseClick+=Exit_MouseClick;
            start.MouseClick+=Start_MouseClick;
            konto.MouseClick+=Setting_MouseClick;

            Controls.AddRange(new Control[] { title, start, konto, exit });
        }

        //При клике на кнопку с шестерёнкой (настройки)
        //запуск другой формы
        private void Setting_MouseClick(object sender, MouseEventArgs e)
        {
            this.Hide();
            settings = new Settings();
            settings.Closed += (s, args) => this.Close();
            settings.Show();
        }

        //При клике на кнопку с тележкой (вход в основной магазин)
        //запуск другой формы
        private void Start_MouseClick(object sender, MouseEventArgs e)
        {
            this.Hide();
            main = new Main();
            main.Closed += (s, args) => this.Close();
            main.Show();
        }

        //При клике на кнопку EXIT
        private void Exit_MouseClick(object sender, MouseEventArgs e)
        {
            Close(); //закрытие проекта
        }
    }
}
