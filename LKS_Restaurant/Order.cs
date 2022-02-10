using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Data.SqlClient;
using System.Drawing;
using System.IO;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;

namespace LKS_Restaurant
{
    public partial class Order : Form
    {
        int id, memberid, orderid;
        SqlConnection connection = new SqlConnection(Utils.conn);
        SqlCommand command;

        public Order()
        {
            InitializeComponent();
            loadgrid("");
            loadmember();
            loadid();

            labelname.Text = Model.name;
            labeltime.Text = DateTime.Now.ToString("dddd, dd-MM-yyyy");
            memberid = 1;
        }

        int loadid()
        {
            command = new SqlCommand("select top(1) * from headerOrder order by orderid desc", connection);
            connection.Open();
            SqlDataReader reader = command.ExecuteReader();
            reader.Read();
            int i = reader.GetInt32(0);
            connection.Close();
            labelorderid.Text = "Order ID : " + (i + 1).ToString();

            return i + 1;
        }

        void loadmember()
        {
            string com = "select * from msmember";
            comboBox1.ValueMember = "memberid";
            comboBox1.DisplayMember = "name";
            comboBox1.DataSource = Command.GetData(com);
        }

        void loadgrid(string s)
        {
            string com = "select * from msmenu" + s;
            dataGridView1.DataSource = Command.GetData(com);
            dataGridView1.Columns[3].Visible = false;
        }
        
        void clear()
        {
            textBox1.Text = "";
            textBox2.Text = "";
            numericUpDown1.Value = 0;
            lbltotal.Text = "0";
            loadid();
            pictureBox1.Image = null;
        }

        void sum()
        {
            int total = 0;
            for (int i = 0; i < dataGridView2.RowCount; i++)
            {
                total += Convert.ToInt32(dataGridView2.Rows[i].Cells[4].Value);
            }

            lbltotal.Text = total.ToString();
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

        private void textBox3_TextChanged(object sender, EventArgs e)
        {
            loadgrid(" where name like '%" + textBox3.Text + "%'");
        }

        private void dataGridView1_CellClick(object sender, DataGridViewCellEventArgs e)
        {
            dataGridView1.CurrentRow.Selected = true;
            id = Convert.ToInt32(dataGridView1.SelectedRows[0].Cells[0].Value);
            textBox1.Text = dataGridView1.SelectedRows[0].Cells[1].Value.ToString();
            textBox2.Text = dataGridView1.SelectedRows[0].Cells[2].Value.ToString();
            if (dataGridView1.SelectedRows[0].Cells[3].Value == System.DBNull.Value)
            {
                pictureBox1.Image = null;
            }
            else
            {
                byte[] b = (byte[])dataGridView1.SelectedRows[0].Cells[3].Value;
                MemoryStream stream = new MemoryStream(b);
                Image img = Image.FromStream(stream);
                Bitmap bmp = (Bitmap)img;
                pictureBox1.Image = bmp;
                pictureBox1.SizeMode = PictureBoxSizeMode.StretchImage;
            }
        }

        private void button3_Click(object sender, EventArgs e)
        {
            if(textBox1.TextLength > 0 && numericUpDown1.Value > 0)
            {
                int rows = dataGridView2.Rows.Add();
                dataGridView2.Rows[rows].Cells[0].Value = id;
                dataGridView2.Rows[rows].Cells[1].Value = textBox1.Text;
                dataGridView2.Rows[rows].Cells[2].Value = numericUpDown1.Value;
                dataGridView2.Rows[rows].Cells[3].Value = textBox2.Text;
                dataGridView2.Rows[rows].Cells[4].Value = numericUpDown1.Value * Convert.ToInt32(textBox2.Text);

                clear();
                sum();
            }
            else
            {
                MessageBox.Show("Please select a menu and fill the quantity!", "Error", MessageBoxButtons.OK, MessageBoxIcon.Error);
            }
        }

        private void dataGridView2_CellClick(object sender, DataGridViewCellEventArgs e)
        {
            dataGridView2.CurrentRow.Selected = true;
        }

        private void button4_Click(object sender, EventArgs e)
        {
            if (dataGridView2.CurrentRow.Selected)
            {
                dataGridView2.Rows.Remove(dataGridView2.SelectedRows[0]);
                sum();
            }
            else
                MessageBox.Show("Please select an item", "Error", MessageBoxButtons.OK, MessageBoxIcon.Error);
        }

        private void panel_report_Click(object sender, EventArgs e)
        {
            Report report = new Report();
            this.Hide();
            report.ShowDialog();
        }

        private void comboBox1_SelectedIndexChanged(object sender, EventArgs e)
        {
            memberid = Convert.ToInt32(comboBox1.SelectedValue);
        }

        private void button5_Click(object sender, EventArgs e)
        {
            if(dataGridView2.RowCount > 0)
            {
                string com = "insert into headerOrder values(" + Model.id + ", " + memberid + ", getdate(), '', '', '" + DateTime.Now.ToString("MMMM") + "', '')";
                Command.exec(com);

                command = new SqlCommand("select top(1) * from headerOrder order by orderid desc", connection);
                connection.Open();
                SqlDataReader reader = command.ExecuteReader();
                reader.Read();
                orderid = Convert.ToInt32(reader["orderid"]);
                connection.Close();
                
                try
                {
                    for (int i = 0; i < dataGridView2.RowCount; i++)
                    {
                        command = new SqlCommand("insert into detailOrder values(" + orderid + ", " + Convert.ToInt32(dataGridView2.Rows[i].Cells[0].Value) + ", " + Convert.ToInt32(dataGridView2.Rows[i].Cells[2].Value) + ", " + Convert.ToInt32(dataGridView2.Rows[i].Cells[4].Value) + ", 'Pending', 'Unpaid')", connection);
                        connection.Open();
                        command.ExecuteNonQuery();
                        connection.Close();
                    }

                    MessageBox.Show("Successfully processed", "Error", MessageBoxButtons.OK, MessageBoxIcon.Information);
                    clear();
                    dataGridView2.Rows.Clear();

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
            else
            {
                MessageBox.Show("Please select a menu", "Error", MessageBoxButtons.OK, MessageBoxIcon.Error);
            }
        }
    }
}
