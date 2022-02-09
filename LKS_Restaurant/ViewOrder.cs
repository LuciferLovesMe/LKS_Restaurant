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
    public partial class ViewOrder : Form
    {
        int id;
        public ViewOrder()
        {
            InitializeComponent();
            dis();
            loadcombo();

            labelname.Text = Model.name;
            labeltime.Text = DateTime.Now.ToString("dddd, dd-MM-yyyy");
        }

        void dis()
        {
            comboBox2.Enabled = false;
            button4.Enabled = false;
        }

        void enable()
        {
            comboBox2.Enabled = true;
            button4.Enabled = true;
        }

        void loadcombo()
        {
            string com = "select distinct orderid from detailOrder";
            comboBox1.ValueMember = "orderId";
            comboBox1.DisplayMember = "Orderid";
            comboBox1.DataSource = Command.GetData(com);
        }

        void loadgrid()
        {
            string com = "select * from menu_view where orderid = " + comboBox1.SelectedValue;
            dataGridView1.DataSource = Command.GetData(com);
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
            ViewOrder view = new ViewOrder();
            this.Hide();
            view.ShowDialog();
        }

        private void panel_password_Click(object sender, EventArgs e)
        {
            ChangePassword change = new ChangePassword();
            change.ShowDialog();
        }

        private void button3_Click(object sender, EventArgs e)
        {
            loadgrid();
        }

        private void dataGridView1_CellClick(object sender, DataGridViewCellEventArgs e)
        {
            dataGridView1.CurrentRow.Selected = true;
            id = Convert.ToInt32(dataGridView1.SelectedRows[0].Cells[0].Value);
            comboBox2.Text = dataGridView1.SelectedRows[0].Cells[6].Value.ToString();
            enable();
        }

        private void button4_Click(object sender, EventArgs e)
        {
            if (comboBox2.Text.Length > 0)
            {
                string com = "update detailOrder set status= '" + comboBox2.Text + "' where detailId = " + id;
                Command.exec(com);
                MessageBox.Show("Successfully changed status into " + comboBox2.Text, "Information", MessageBoxButtons.OK, MessageBoxIcon.Information);

                loadgrid();
                dis();
            }
            else
            {
                MessageBox.Show("Please select a status", "Error", MessageBoxButtons.OK, MessageBoxIcon.Error);
            }
        }
    }
}
