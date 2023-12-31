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
        UserControl uc;
        Dictionary<string, int> tooded; SqlConnection connect = new SqlConnection(@"Data Source=(LocalDB)\MSSQLLocalDB;AttachDbFilename=|DataDirectory|\AppData\Tooded_DB.mdf;Integrated Security=True");
        SqlDataAdapter adapter_toode;
        double summa;
        Account acc;

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
            acc = new Account(Properties.Settings.Default.Account);

            lb = new ListBox() { Size = new Size(370,810), Location = new Point(30,15), Font = new Font("Arial",20)};
            lb.SelectedValueChanged += Lb_SelectedValueChanged;
            uc = new UserControl() { Size = new Size(700,370), Location = new Point(lb.Right+50,15), Font = new Font("Arial", 20) };

            ListBoxFill();
            lb.SelectedIndex = lb.Items.Count - 1;

            Controls.AddRange(new Control[] {lb, uc });
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
                        summa += Convert.ToInt32(item1["Kogus"]) * Convert.ToInt32(item1["Hind"]);
                    }
                }
                Label tb = new Label() { Location = new Point(30,30), Font = new Font("Arial",20), Text = $"Kokku: {summa} eurot", AutoSize = true };
                Label tb2 = new Label() { Location = new Point(30, 90), Font = new Font("Arial", 20), Text = $"Kokku allahindlusega : {summa - summa * (Convert.ToDouble(acc.Discount) / 100)} eurot", AutoSize = true };
                uc.Controls.AddRange(new Control[] { tb, tb2 });
            }
            else if (lb.SelectedIndex == lb.Items.Count - 2)
            {
                uc.Controls.Clear();
            }
            else
            {
                adapter_toode = new SqlDataAdapter($"SELECT Toodenimetus, Kogus, Hind FROM Toodetable WHERE Toodenimetus = '{lb.SelectedItem.ToString().Replace(" ", "").Split(':')[0]}'", connect);
                DataTable dt_tod = new DataTable();
                adapter_toode.Fill(dt_tod);
                foreach (DataRow item1 in dt_tod.Rows)
                {
                    summa += Convert.ToInt32(item1["Kogus"]) * Convert.ToInt32(item1["Hind"]);
                }
                Label tb = new Label() { Location = new Point(30, 30), Font = new Font("Arial", 20), Text = $"{lb.SelectedItem.ToString().Replace(" ", "").Split(':')[0]}: {summa} eurot", AutoSize = true };
                uc.Controls.Add(tb);
            }
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
                    lb.Items.Add(item.Key + " : " + item.Value);
            lb.Items.Add("");
            lb.Items.Add("Kokku : "+ tooded.Values.Sum());
        }
    }
}
