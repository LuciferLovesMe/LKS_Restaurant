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

namespace LKS_Restaurant
{
    public partial class Payment : Form
    {
        int id, total;
        SqlConnection connection = new SqlConnection(Utils.conn);
        SqlCommand command;
        SqlDataReader reader;

        public Payment()
        {
            InitializeComponent();
            loadcombo();

            labelname.Text = Model.name;
            labeltime.Text = DateTime.Now.ToString("dddd, dd-MM-yyyy");
        }

        void loadcombo()
        {
            string com = "select distinct orderid from detailOrder";
            comboBox1.ValueMember = "orderId";
            comboBox1.DisplayMember = "Orderid";
            comboBox1.DataSource = Command.GetData(com);
        }
        
        void sum()
        {
            total = 0;
            int add = 0;
            for(int i = 0; i < dataGridView1.RowCount; i++)
            {
                if(dataGridView1.Rows[i].Cells[7].Value.ToString() != "Delivered" && dataGridView1.Rows[i].Cells[8].Value.ToString() != "Paid")
                {
                    add = 0;
                }
                else if(dataGridView1.Rows[i].Cells[7].Value.ToString() == "Delivered" && dataGridView1.Rows[i].Cells[8].Value.ToString() != "Paid")
                {
                    add = Convert.ToInt32(dataGridView1.Rows[i].Cells[6].Value);
                }
                else if (dataGridView1.Rows[i].Cells[7].Value.ToString() == "Delivered" && dataGridView1.Rows[i].Cells[8].Value.ToString() == "Paid")
                {
                    add = 0;
                }
                total += add;
            }
            labeltotal.Text = total.ToString();
        }

        void clear()
        {
            comboBox2.Text = "";
            comboBox3.Text = "";
            textBox1.Text = "";
            dataGridView1.DataSource = null;
            dataGridView1.Rows.Clear();
            labeltotal.Text = "0";
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

        private void textBox1_KeyPress(object sender, KeyPressEventArgs e)
        {
            e.Handled = !(char.IsDigit(e.KeyChar) || e.KeyChar == 8);
        }

        private void button4_Click(object sender, EventArgs e)
        {
            if (comboBox2.Text.Length < 1 || comboBox3.Text.Length < 1 || textBox1.TextLength < 1)
            {
                MessageBox.Show("All fields must be filled", "Error", MessageBoxButtons.OK, MessageBoxIcon.Error);
            }
            else if (dataGridView1.RowCount < 1)
            {
                MessageBox.Show("Please select a order id before paying", "Error", MessageBoxButtons.OK, MessageBoxIcon.Error);

            }
            else
            {
                string com = "update headerOrder set bank = '"+comboBox2.Text+"', payment = '"+comboBox3.Text+"', cardNumber = '"+textBox1.Text+"' where orderId = " + comboBox1.SelectedValue;
                try
                {
                    Command.exec(com);
                    for (int i = 0; i < dataGridView1.RowCount; i++)
                    {
                        if (dataGridView1.Rows[i].Cells[7].Value.ToString() == "Delivered" && dataGridView1.Rows[i].Cells[8].Value.ToString() != "Paid")
                        {
                            id = Convert.ToInt32(dataGridView1.Rows[i].Cells[0].Value);
                            string comm = "update detailOrder set paymentStatus = 'Paid' where detailId = " + id;
                            Command.exec(comm);
                        }
                    }
                    MessageBox.Show("Success", "Information", MessageBoxButtons.OK, MessageBoxIcon.Error);
                    clear();
                }
                catch (Exception ex)
                {
                    MessageBox.Show("" + ex);
                }
                finally
                {
                    connection.Close();
                }
            }
        }

        private void panel_report_Click(object sender, EventArgs e)
        {
            Report report = new Report();
            this.Hide();
            report.ShowDialog();
        }

        private void button3_Click(object sender, EventArgs e)
        {
            string com = "select * from pay_view where orderid = " + comboBox1.SelectedValue;
            dataGridView1.DataSource = Command.GetData(com);
            dataGridView1.Columns[0].Visible = false;
            dataGridView1.Columns[1].Visible = false;
            sum();

            command = new SqlCommand("select * from headerOrder where orderid = " + comboBox1.SelectedValue, connection);
            connection.Open();
            reader = command.ExecuteReader();
            reader.Read();
            if (reader.GetString(4) != "")
            {
                comboBox2.Text = reader.GetString(4);
                comboBox3.Text = reader.GetString(5);
                textBox1.Text = reader.GetString(7);
                connection.Close();
            }
            else
            {
                connection.Close();
                comboBox2.Text = "";
                comboBox3.Text ="";
                textBox1.Text = "";
            }
        }
    }
}
