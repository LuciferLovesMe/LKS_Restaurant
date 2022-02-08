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
    public partial class ChangePassword : Form
    {
        SqlConnection connection = new SqlConnection(Utils.conn);
        SqlCommand command;
        public ChangePassword()
        {
            InitializeComponent();
        }

        private void checkBox1_CheckedChanged(object sender, EventArgs e)
        {
            if (checkBox1.Checked)
                textBox2.PasswordChar = '\0';
            else
                textBox2.PasswordChar = '*';
        }
        bool val() {
            if (textBox1.TextLength < 1 || textBox2.TextLength < 1 || textBox3.TextLength < 1)
            {
                MessageBox.Show("All fields must be filled", "Erro", MessageBoxButtons.OK, MessageBoxIcon.Error);
                return false;
            }
            else if (Encrypt.enc(textBox1.Text) != Model.password)
            {
                MessageBox.Show("Old password is not correct", "Error", MessageBoxButtons.OK, MessageBoxIcon.Error);
                return false;
            }
            else if (textBox3.Text != textBox2.Text)
            {
                MessageBox.Show("Confirm Password is not correct", "Error", MessageBoxButtons.OK, MessageBoxIcon.Error);
                return false;
            }

            return true;
        }

        private void button1_Click(object sender, EventArgs e)
        {
            if (val())
            {
                string pass = Encrypt.enc(textBox2.Text);
                command = new SqlCommand("update msemployee set password = @pass where employeeId = " + Model.id, connection);
                connection.Open();
                command.Parameters.AddWithValue("@pass", pass);
                command.ExecuteNonQuery();
                connection.Close();

                MessageBox.Show("Successfully changed", "Information", MessageBoxButtons.OK, MessageBoxIcon.Information);
                
                MainLogin main = new MainLogin();
                this.Hide();
                main.ShowDialog();
            }
        }

        private void button2_Click(object sender, EventArgs e)
        {
            this.Hide();
        }
    }
}
