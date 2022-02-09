using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;

namespace LKS_Restaurant
{
    public partial class MainCashier : Form
    {
        public MainCashier()
        {
            InitializeComponent();

            labelname.Text = Model.name;
            labeltime.Text = DateTime.Now.ToString("dddd, dd-MM-yyyy");
        }

        private void button2_Click(object sender, EventArgs e)
        {
            DialogResult result = MessageBox.Show("Are you sure?", "Confirmation", MessageBoxButtons.YesNo, MessageBoxIcon.Question);
            if (result == DialogResult.Yes)
                Application.Exit();
        }

        private void button1_Click(object sender, EventArgs e)
        {
            DialogResult result = MessageBox.Show("Are you sure?", "Confirmation", MessageBoxButtons.YesNo, MessageBoxIcon.Question);
            if (result == DialogResult.Yes)
            {
                MainLogin main = new MainLogin();
                this.Hide();
                main.ShowDialog();
            }
        }

        private void panel_menu_Click(object sender, EventArgs e)
        {
            Payment view = new Payment();
            this.Hide();
            view.ShowDialog();
        }

        private void panel_password_Click(object sender, EventArgs e)
        {
            ChangePassword change = new ChangePassword();
            change.ShowDialog();
        }

        private void panel_order_Click(object sender, EventArgs e)
        {
            Order order = new Order();
            this.Hide();
            order.ShowDialog();
        }
    }
}
