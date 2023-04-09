using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;
using MySql.Data.MySqlClient;

namespace Untitled
{
    public partial class Reg : Form
    {
        MySqlConnection conn = DBUtils.GetDBConnection();

        public Reg()
        {
            InitializeComponent();
            this.MaximizeBox = false;
            this.AutoSizeMode = System.Windows.Forms.AutoSizeMode.GrowAndShrink;
        }

        private void button1_Click(object sender, EventArgs e)
        {
            // Проверяем, чтобы были заполнены все поля.
            if (textBox1.Text == null || textBox1.Text == "" || textBox2.Text == null || textBox2.Text == "")
            {
                MessageBox.Show(
                    "Заполните все поля ввода данных.",
                    "Сообщение",
                    MessageBoxButtons.OK,
                    MessageBoxIcon.Information);
                return;
            }
            else
            {
                DialogResult res = MessageBox.Show("Выполнить регистрацию в системе?", "Подтвердите действие", MessageBoxButtons.YesNo, MessageBoxIcon.Question);
                if (res == DialogResult.Yes)
                {
                    try
                    {
                        string query = "INSERT INTO auth (auth_log, auth_pwd, auth_role) VALUES ('" + textBox1.Text + "', '" + textBox2.Text + "', 2); ";
                        MySqlCommand cmDB = new MySqlCommand(query, conn);
                        try
                        {
                            conn.Open();
                            cmDB.ExecuteReader();
                            conn.Close();
                        }
                        catch (Exception ex)
                        {
                            MessageBox.Show("Произошла непредвиденная ошибка!" + Environment.NewLine + ex.Message);
                        }
                        button1.Visible = false;
                        label5.Text = "Вы успешно зарегистрировались!";
                    }
                    catch (NullReferenceException ex)
                    {
                        MessageBox.Show("Произошла непредвиденная ошибка!" + Environment.NewLine + ex.Message);
                    }
                }
                textBox1.Clear();
                textBox2.Clear();
                textBox1.Visible = false;
                textBox2.Visible = false;
                label1.Visible = false;
                label2.Visible = false;
                button1.Visible = false;
            }
        }

        //Назад.
        private void button2_Click(object sender, EventArgs e)
        {
            Authorization aut = new Authorization(); // Обращение к форме "Auth", на которую будет совершаться переход.
            aut.Owner = this;
            this.Hide();
            aut.Show(); // Запуск окна "Auth".
        }

        private void Reg_FormClosing(object sender, FormClosingEventArgs e)
        {
            Application.Exit();
        }
    }
}
