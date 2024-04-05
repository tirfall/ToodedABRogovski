using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Data.SqlClient;
using System.Diagnostics;
using System.Drawing;
using System.IO;
using System.Linq;
using System.Security.Policy;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;
using iText.Commons.Datastructures;
using iText.IO.Image;
using iText.Kernel.Colors;
using iText.Kernel.Pdf;
using iText.Kernel.Pdf.Canvas.Draw;
using iText.Kernel.Pdf.Xobject;
using iText.Layout;
using iText.Layout.Element;
using iText.Layout.Properties;
using QRCoder;

namespace ToodedAB
{
    public partial class Cassa : Form
    { 
        ListBox lb;
        UserControl uc;
        Dictionary<string, int> tooded; 
        SqlConnection connect = new SqlConnection(@"Data Source=(LocalDB)\MSSQLLocalDB;AttachDbFilename=|DataDirectory|\AppData\Tooded_DB.mdf;Integrated Security=True");
        SqlCommand command;
        SqlDataAdapter adapter_toode;
        double summa, hind, hind2, summadis;
        Account acc;
        Label tb,tb1, tb2;
        Button btn1, btn2, btn3;
        int kogus;
        int red = 0;
        Main main;
        string selected;

        public Cassa()
        {
            this.Width = 1200;
            this.Height = 900;
            this.Text = "VS Pood | Vihane Sipelgas Kassa";
            this.BackColor = System.Drawing.Color.White;
            this.Icon = Properties.Resources.Icon;
            this.FormBorderStyle = FormBorderStyle.FixedSingle;
            this.MaximizeBox = false;
            this.MinimizeBox = false;
            acc = new Account(Properties.Settings.Default.Account);

            lb = new ListBox() { Size = new Size(370,810), Location = new Point(30,15), Font = new Font("Arial",20)};
            lb.SelectedValueChanged += Lb_SelectedValueChanged;
            lb.MouseClick += Lb_MouseClick;
            uc = new UserControl() { Size = new Size(700,370), Location = new Point(lb.Right+50,15), Font = new Font("Arial", 20) };
            btn1 = new Button() { Text= "Vaatamine", Size = new Size(uc.Width/3,300), Location = new Point(uc.Left,480), Font = new Font("Arial", 20) };
            btn1.Click += Btn1_Click;
            btn2 = new Button() { Text = "Osta", Size = new Size(uc.Width / 3, 300), Location = new Point(btn1.Right, 480), Font = new Font("Arial", 20) };
            btn2.Click += Btn2_Click;
            btn3 = new Button() { Text = "Tagasi poodi", Size = new Size(uc.Width / 3, 300), Location = new Point(btn2.Right, 480), Font = new Font("Arial", 20) };
            btn3.Click += Btn3_Click;

            ListBoxFill();
            lb.SelectedIndex = lb.Items.Count - 1;

            Controls.AddRange(new Control[] {lb, uc, btn1, btn2, btn3 });
        }

        private void Btn3_Click(object sender, EventArgs e)
        {
            this.Hide();
            main = new Main();
            main.Closed += (s, args) => this.Close();
            main.Show();
        }

        private void Btn2_Click(object sender, EventArgs e)
        {
            string folderPath = Path.Combine(AppDomain.CurrentDomain.BaseDirectory, "Arved");
            int i = 1;
            string fileName = "Arv0.pdf";
            //while (Properties.Settings.Default.Arved.Split(',').Contains(fileName))
            //{
            //    fileName = $"Arv{i}.pdf";
            //    i++;
            //}
            string filePath = Path.Combine(folderPath, fileName);
            Properties.Settings.Default.Arved += fileName + ",";
            Properties.Settings.Default.Save();
            filePath = filePath.Replace("bin\\Debug\\", "");
            PdfWriter writer = new PdfWriter(filePath);
            PdfDocument pdf = new PdfDocument(writer);
            Document document = new Document(pdf);
            Paragraph header = new Paragraph("VS pood").SetTextAlignment(TextAlignment.CENTER).SetFontSize(20);
            Paragraph subheader = new Paragraph("Arv").SetTextAlignment(TextAlignment.CENTER).SetFontSize(15);
            LineSeparator ls = new LineSeparator(new DottedLine());
            Paragraph time = new Paragraph(DateTime.Now.ToString());
            Table table = new Table(4, true);
            Cell c11 = new Cell(1, 1)
               .Add(new Paragraph("Toode"));
            Cell c12 = new Cell(1, 2)
               .Add(new Paragraph("Kogus"));
            Cell c13 = new Cell(1, 3)
               .Add(new Paragraph("Hind"));
            foreach (Cell item in new Cell[] {c11,c12,c13})
            {
                item.SetBackgroundColor(ColorConstants.GRAY).SetTextAlignment(TextAlignment.CENTER);
                table.AddCell(item);
            }
            int x = 2;
            foreach (var item in tooded)
            {
                Andmed(item.Key);
                table.AddCell(new Cell(x, 1).Add(new Paragraph(item.Key)));
                table.AddCell(new Cell(x, 2).Add(new Paragraph(item.Value.ToString())));
                table.AddCell(new Cell(x, 3).Add(new Paragraph((hind2*(double)item.Value).ToString())));
                x++;
            }
            Paragraph kokku = new Paragraph("Kokku: "+summa.ToString()).SetTextAlignment(TextAlignment.LEFT).SetFontSize(15);
            Paragraph kokku1 = new Paragraph("Kokku allahindlusega: "+summadis.ToString()).SetTextAlignment(TextAlignment.LEFT).SetFontSize(15);
            Paragraph cb = new Paragraph("Boonusraha: "+(acc.Cashback*0,01).ToString()).SetTextAlignment(TextAlignment.LEFT).SetFontSize(15);
            Paragraph nimi = new Paragraph("Nimi: "+acc.Nimi).SetTextAlignment(TextAlignment.LEFT).SetFontSize(15);
            Paragraph disc = new Paragraph("Allahindlus: "+acc.Discount.ToString()).SetTextAlignment(TextAlignment.LEFT).SetFontSize(15);

            document.Add(header);
            document.Add(subheader);
            document.Add(ls);
            document.Add(time);
            document.Add(table);
            document.Add(ls);
            document.Add(kokku);
            document.Add(kokku1);
            document.Add(ls);
            document.Add(cb);
            document.Add(nimi);
            document.Add(disc);
            document.Close();

            // Открытие созданного PDF файла
            Process.Start(filePath);

            // Очистка списка товаров
            Properties.Settings.Default.Tooded = "";
            Properties.Settings.Default.Save();
            ListBoxFill();
        }

        private void Btn1_Click(object sender, EventArgs e)
        {
            switch (red)
            {
                case 0: red = 1; btn1.Text = "Lisamine";break;
                case 1: red = 2; btn1.Text = "Eemaldamine"; break;
                case 2: red = 0; btn1.Text = "Vaatamine"; break;
            }
        }

        private void Andmed(string nimi)
        {
            adapter_toode = new SqlDataAdapter($"SELECT Kogus, Hind FROM Toodetable WHERE Toodenimetus = '{nimi}'", connect);
            DataTable dt_tod = new DataTable();
            adapter_toode.Fill(dt_tod);
            foreach (DataRow dr in dt_tod.Rows)
            {
                kogus = Convert.ToInt32(dr["Kogus"]);
                hind2 = Convert.ToDouble(dr["Hind"]);
            }
        }

        private void Lb_MouseClick(object sender, MouseEventArgs e)
        {
            if (red == 0)
                return;
            selected = lb.SelectedItem.ToString().Replace(" ", "").Split(':')[0];
            connect.Open();
            if (red == 1)
            {
                if (kogus > 0)
                {
                    Properties.Settings.Default.Tooded += selected + ",";
                    command = new SqlCommand("UPDATE Toodetable SET Kogus=Kogus-1 WHERE Toodenimetus=@nimi", connect);
                    command.Parameters.AddWithValue("@nimi", selected);
                    command.ExecuteNonQuery();
                }
                else
                    MessageBox.Show("See toode on otsas.", "Viga", MessageBoxButtons.OK, MessageBoxIcon.Error);
            }
            else
            {
                string[] t = Properties.Settings.Default.Tooded.Split(',');
                List<string> tooded = t.ToList();
                if (tooded.Contains(selected))
                {
                    tooded.Remove(selected);
                    Properties.Settings.Default.Tooded = string.Join(",", tooded);
                    command = new SqlCommand("UPDATE Toodetable SET Kogus=Kogus+1 WHERE Toodenimetus=@nimi", connect);
                    command.Parameters.AddWithValue("@nimi", selected);
                    command.ExecuteNonQuery();
                }
                else
                    MessageBox.Show("Te ei võtnud seda toodet.", "Viga", MessageBoxButtons.OK, MessageBoxIcon.Error);
            }
            Properties.Settings.Default.Save();
            connect.Close();
            ListBoxFill();
            Andmed(selected);
        }

        private void Lb_SelectedValueChanged(object sender, EventArgs e)
        {
            uc.Controls.Clear();
            summa = 0;
            if (lb.SelectedIndex == lb.Items.Count-1)
            {
                foreach (var item in tooded)
                {
                    adapter_toode = new SqlDataAdapter($"SELECT Toodenimetus, Kogus, Hind FROM Toodetable WHERE Toodenimetus = '{item.Key}'", connect);
                    DataTable dt_tod = new DataTable();
                    adapter_toode.Fill(dt_tod);
                    foreach (DataRow item1 in dt_tod.Rows)
                    {
                        summa += tooded[item.Key] * Convert.ToDouble(item1["Hind"]);
                    }
                }
                tb = null;
                tb1 = new Label() { Location = new Point(30,30), Font = new Font("Arial",20), Text = $"Kokku: {summa} eurot", AutoSize = true };
                summadis = summa - summa * (Convert.ToDouble(acc.Discount) / 100);
                tb2 = new Label() { Location = new Point(30, 90), Font = new Font("Arial", 20), Text = $"Kokku allahindlusega : {summadis} eurot", AutoSize = true };
            }
            else if (lb.SelectedIndex == lb.Items.Count - 2)
            {
                uc.Controls.Clear();
                return;
            }
            else
            {
                if (red==0)
                {
                    selected = lb.SelectedItem.ToString().Replace(" ", "").Split(':')[0];
                }
                adapter_toode = new SqlDataAdapter($"SELECT Toodenimetus, Kogus, Hind FROM Toodetable WHERE Toodenimetus = '{selected}'", connect);
                DataTable dt_tod = new DataTable();
                adapter_toode.Fill(dt_tod);
                foreach (DataRow item1 in dt_tod.Rows)
                {
                    summa += tooded[selected] * Convert.ToDouble(item1["Hind"]);
                    hind = Convert.ToDouble(item1["Hind"]);
                }
                tb = new Label() { Location = new Point(30, 30), Font = new Font("Arial", 20), Text = $"(1) {selected} | {hind} eurot", AutoSize = true };
                tb1 = new Label() { Location = new Point(30, 90), Font = new Font("Arial", 20), Text = $"({tooded[selected]}) {selected}: {summa} eurot", AutoSize = true };
                tb2 = new Label() { Location = new Point(30, 150), Font = new Font("Arial", 20), Text = $"({tooded[selected]}) {selected} allahindlusega : {summa - summa * (Convert.ToDouble(acc.Discount) / 100)} eurot", AutoSize = true };

            }
            uc.Controls.AddRange(new Control[] {tb, tb1, tb2 });
        }

        private void ListBoxFill()
        {
            lb.Items.Clear();
            tooded = new Dictionary<string, int>();
            List<string> temp = Properties.Settings.Default.Tooded.Split(',').ToList();
            temp.RemoveAt(temp.Count - 1);
            foreach (string item in temp)
            {
                if (tooded.ContainsKey(item))
                    tooded[item] += 1;
                else
                    tooded[item] = 1;
            }
            foreach (var item in tooded)
                if (item.Key.Length > 1)
                    lb.Items.Add(item.Key + " : " + item.Value+" t.");
            lb.Items.Add("");
            lb.Items.Add("Kokku : "+ tooded.Values.Sum());
        }
    }
}
