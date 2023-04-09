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
    public partial class Users : Form
    {
        MySqlConnection conn = DBUtils.GetDBConnection();
        public Users()
        {
            InitializeComponent();
            UserDate();
            this.MaximizeBox = false;
            this.AutoSizeMode = System.Windows.Forms.AutoSizeMode.GrowAndShrink;
        }

        public void UserDate()
        {

            string query = "SELECT auth_log, auth_pwd FROM auth WHERE auth_id =" + Int32.Parse(Status.user) + ";";
            MySqlCommand cmDB = new MySqlCommand(query, conn);
            conn.Open();

            MySqlDataReader reader = cmDB.ExecuteReader();

            while (reader.Read())
            {
                textBox2.Text = reader.GetString(0);
                textBox3.Text = reader.GetString(1);
            }
            conn.Close();
        }

        public void UserFIO()
        {
            string query = "SELECT user_name, user_birth FROM users WHERE user_auth =" + Status.user + ";";
            MySqlCommand cmDB = new MySqlCommand(query, conn);
            conn.Open();

            MySqlDataReader reader = cmDB.ExecuteReader();

            while (reader.Read())
            {
                textBox1.Text = reader.GetString(0);
                dateTimePicker1.Text = reader.GetString(1);
            }
            conn.Close();
        }

        //Сохранение данных
        private void button1_Click(object sender, EventArgs e)
        {
            if (textBox1.Text == "" && textBox2.Text == "" && textBox3.Text == "")
            {
                MessageBox.Show(
                    "Должны быть заполнены все поля ввода данных!",
                    "Сообщение",
                    MessageBoxButtons.OK,
                    MessageBoxIcon.Information);
            }
            else
            {
                DialogResult res = MessageBox.Show("Сохранить данные профиля?", "Подтвердите действие", MessageBoxButtons.YesNo, MessageBoxIcon.Question);
                if (res == DialogResult.Yes)
                {
                    try
                    {
                        // Проверяем - существуют ли данные пользователя в системе.
                        string count = "SELECT COUNT(*) FROM users WHERE user_auth=" + Status.user + ";";
                        MySqlCommand cmDB1 = new MySqlCommand(count, conn);
                        conn.Open();
                        MySqlCommand command = new MySqlCommand(count, conn);
                        int result = Int32.Parse(command.ExecuteScalar().ToString());
                        conn.Close();
                        var date = dateTimePicker1.Value.ToString("yyyy-MM-dd");
                        if (result == 0)
                        {
                            FIO.fio = textBox1.Text;
                            string query = "insert into users (user_name, user_birth, user_auth) values('" + textBox1.Text + "', '" + date + "', " + Status.user + "); UPDATE auth SET auth_log='" + textBox2.Text + "', auth_pwd='" + textBox3.Text + "' WHERE auth_id=" + Status.user + ";";
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
                            MessageBox.Show("Запись успешно сохранена!");
                        }
                        if (result == 1)
                        {
                            string query = "UPDATE users SET user_name='" + textBox1.Text + "', user_birth='" + date + "' WHERE user_auth=" + Status.user + "; UPDATE auth SET auth_log='" + textBox2.Text + "', auth_pwd='" + textBox3.Text + "' WHERE auth_id=" + Status.user + ";";
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
                            MessageBox.Show("Запись успешно сохранена!");
                        }
                    }
                    catch (NullReferenceException ex)
                    {
                        MessageBox.Show("Произошла непредвиденная ошибка!" + Environment.NewLine + ex.Message);
                    }
                }
                button1.Enabled = false;
            }

        }

        //Назад в меню
        private void button2_Click(object sender, EventArgs e)
        {
            Menu menu = new Menu();
            menu.Owner = this;
            this.Hide();
            menu.Show();
        }

        //Закрытие программы
        private void Users_FormClosing(object sender, FormClosingEventArgs e)
        {
            Application.Exit();
        }

        private void Users_Load(object sender, EventArgs e)
        {
            textBox1.Text = FIO.fio;
            UserFIO();
        }
    }

    static class FIO
    {
        public static string fio { get; set; }
    }
}
