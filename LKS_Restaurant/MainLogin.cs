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
    public partial class MainLogin : Form
    {
        SqlConnection connection = new SqlConnection(Utils.conn);
        SqlCommand command;
        SqlDataReader reader;

        public MainLogin()
        {
            InitializeComponent();
        }

        private void button2_Click(object sender, EventArgs e)
        {
            DialogResult result = MessageBox.Show("Are you sure?", "Confirmation", MessageBoxButtons.YesNo, MessageBoxIcon.Question);
            if (result == DialogResult.Yes)
                Application.Exit();
        }

        private void checkBox1_CheckedChanged(object sender, EventArgs e)
        {
            if (checkBox1.Checked)
                textBox2.PasswordChar = '\0';
            else
                textBox2.PasswordChar = '*';
        }

        private void button1_Click(object sender, EventArgs e)
        {
            if (textBox1.TextLength < 1 || textBox2.TextLength < 1)
                MessageBox.Show("All fields must be filled", "Error", MessageBoxButtons.OK, MessageBoxIcon.Error);
            else
            {
                string pass = Encrypt.enc(textBox2.Text);
                command = new SqlCommand("select * from msEmployee where email = '" + textBox1.Text + "' and password = @pass", connection);
                command.Parameters.AddWithValue("@pass", pass);
                try
                {
                    connection.Open();
                    reader = command.ExecuteReader();
                    reader.Read();
                    if (reader.HasRows)
                    {
                        Model.id = reader.GetInt32(0);
                        Model.name = reader.GetString(1);
                        Model.email = reader.GetString(2);
                        Model.password = reader.GetString(3);
                        Model.hp = reader.GetString(4);
                        Model.position = reader.GetString(5);

                        connection.Close();
                        if(Model.position.ToLower() == "admin")
                        {
                            MainAdmin main = new MainAdmin();
                            this.Hide();
                            main.ShowDialog();
                        }
                        else if(Model.position.ToLower() == "chef")
                        {
                            MainChef main = new MainChef();
                            this.Hide();
                            main.ShowDialog();
                        }
                        else if(Model.position.ToLower() == "cashier")
                        {
                            MainCashier main = new MainCashier();
                            this.Hide();
                            main.ShowDialog();
                        }
                    }
                    else
                    {
                        connection.Close();
                        MessageBox.Show("Can't find user", "Error", MessageBoxButtons.OK, MessageBoxIcon.Error);
                    }
                }
                catch (Exception ex)
                {
                    MessageBox.Show(ex.Message);
                }
                finally
                {
                    connection.Close();
                }
            }
        }
    }
}
