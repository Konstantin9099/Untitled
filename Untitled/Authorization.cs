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
using Org.BouncyCastle.Ocsp;

namespace Untitled
{
    public partial class Authorization : Form
    {
        MySqlConnection conn = DBUtils.GetDBConnection();

        public Authorization()
        {
            InitializeComponent();
            this.MaximizeBox = false;
            this.AutoSizeMode = System.Windows.Forms.AutoSizeMode.GrowAndShrink;
        }

        private void button1_Click(object sender, EventArgs e)
        {
            if (textBox1.Text == "" || textBox2.Text == "")
            {
                MessageBox.Show(
                    "Не введены логин и/или пароль!",
                    "Сообщение",
                    MessageBoxButtons.OK,
                    MessageBoxIcon.Information);
            }
            else
            {
                // Запрос к таблице Auth.
                string query = "SELECT auth_id FROM auth WHERE auth_log ='" + textBox1.Text + "' and auth_pwd = '" + textBox2.Text + "';";

                // Объект для выполнения SQL-запроса.
                MySqlCommand cmDB = new MySqlCommand(query, conn);
                try
                {
                    // Устанавливаем соединение с БД.
                    conn.Open();
                    int id_user = 0;
                    id_user = Convert.ToInt32(cmDB.ExecuteScalar());
                    Status.user = id_user.ToString();
                    if (id_user > 0)
                    {
                        Menu men = new Menu(); // Переход на форму Menu.
                        men.Owner = this;
                        this.Hide();
                        men.Show(); // Запуск окна Stories.
                        textBox1.Clear(); // Очистка поля - логин.
                        textBox2.Clear(); // Очистка поля - пароль.
                    }
                    else
                        MessageBox.Show("Неверный логин и/или пароль!");
                    conn.Close();
                }
                catch (Exception ex)
                {
                    MessageBox.Show("Возникла непредвиденная ошибка!" + Environment.NewLine + ex.Message);
                }
            }
        }

        private void button2_Click(object sender, EventArgs e)
        {
            Application.Exit();
        }

        private void Authorization_FormClosing(object sender, FormClosingEventArgs e)
        {
            Application.Exit();
        }

        private void linkLabel1_LinkClicked(object sender, LinkLabelLinkClickedEventArgs e)
        {
            Reg reg = new Reg(); // Обращение к форме "Reg", на которую будет совершаться переход.
            reg.Owner = this;
            this.Hide();
            reg.Show(); // Запуск окна "Reg".
        }
    }

    static class Status
    {
        public static string user { get; set; }
    }
}
