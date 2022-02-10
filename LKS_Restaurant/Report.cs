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
    public partial class Report : Form
    {
        SqlConnection connection = new SqlConnection(Utils.conn);
        SqlCommand command;
        SqlDataReader reader;
        Microsoft.Office.Interop.Excel.Application application = new Microsoft.Office.Interop.Excel.Application();

        public Report()
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

        private void panel_report_Click(object sender, EventArgs e)
        {
            Report report = new Report();
            this.Hide();
            report.ShowDialog();
        }

        private void button3_Click(object sender, EventArgs e)
        {
            if(dateTimePicker1.Value > dateTimePicker2.Value)
            {
                MessageBox.Show("Start date time must be less", "Error", MessageBoxButtons.OK, MessageBoxIcon.Error);
            }
            else
            {
                string com = "select headerOrder.month as Month, sum(detailOrder.price) as Income from headerOrder join detailOrder on detailOrder.orderId = headerOrder.orderID where headerorder.date >= '" + dateTimePicker1.Value.ToString("yyyy-MM-dd") + "' and date <= '" + dateTimePicker2.Value.ToString("yyyy-MM-dd") + "' group by headerOrder.month";
                dataGridView1.DataSource = Command.GetData(com);

                command = new SqlCommand(com, connection);
                connection.Open();
                reader = command.ExecuteReader();
                while (reader.Read())
                {
                    chart1.Series["Income"].Points.AddXY(reader.GetString(0), Convert.ToInt32(reader["Income"]));

                }
                connection.Close();
            }
        }

        private void button4_Click(object sender, EventArgs e)
        {
            if(dataGridView1.RowCount < 1)
            {
                MessageBox.Show("There are not data", "Error", MessageBoxButtons.OK, MessageBoxIcon.Error);
            }
            else
            {
                application.Workbooks.Add(Type.Missing);
                for(int i = 0; i < dataGridView1.ColumnCount; i++)
                {
                    application.Cells[i + 1] = dataGridView1.Columns[i].HeaderText;
                }
                for(int i = 0; i < dataGridView1.RowCount; i++)
                {
                    for(int j = 0; j < dataGridView1.ColumnCount; j++)
                    {
                        application.Cells[i + 2, j + 1] = dataGridView1.Rows[i].Cells[j].Value.ToString();
                    }
                }

                application.Visible = true;
            }
        }
    }
}
