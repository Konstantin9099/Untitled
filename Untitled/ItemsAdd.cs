using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Text.RegularExpressions;
using System.Threading.Tasks;
using System.Windows.Forms;
using MySql.Data.MySqlClient;
using static System.Windows.Forms.VisualStyles.VisualStyleElement;

namespace Untitled
{
    public partial class ItemsAdd : Form
    {
        MySqlConnection conn = DBUtils.GetDBConnection();
        public ItemsAdd()
        {
            InitializeComponent();
            this.MaximizeBox = false;
            this.AutoSizeMode = System.Windows.Forms.AutoSizeMode.GrowAndShrink;
        }

        private void ItemsAdd_FormClosing(object sender, FormClosingEventArgs e)
        {
            Application.Exit();
        }

        private void button2_Click(object sender, EventArgs e)
        {
            Items items = new Items();
            items.Owner = this;
            this.Hide();
            items.Show();
        }

        private void button1_Click(object sender, EventArgs e)
        {
            // Проверяем, чтобы были заполнены все поля.
            if (textBox1.Text == null || textBox1.Text == "" || textBox2.Text == null || textBox2.Text == "")
            {
                MessageBox.Show("Должны быть заполнены все поля ввода данных.", "Сообщение", MessageBoxButtons.OK, MessageBoxIcon.Information);
                return;
            }
            else
            {
                DialogResult res = MessageBox.Show("Добавить товар?", "Подтвердите действие", MessageBoxButtons.YesNo, MessageBoxIcon.Question);
                if (res == DialogResult.Yes)
                {
                    try
                    {
                        string query = "INSERT INTO items (item_name, item_cost) VALUES ('" + textBox1.Text + "', '" + Convert.ToDouble(textBox2.Text) + "'); ";
                        MySqlConnection conn = DBUtils.GetDBConnection();
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
                        MessageBox.Show("Товар добавлен!", "Операция выполнена успешно");
                    }
                    catch (NullReferenceException ex)
                    {
                        MessageBox.Show("Произошла ошибка!" + Environment.NewLine + ex.Message);
                    }
                }
                button1.Enabled = false;
            }
        }

        private void textBox2_KeyPress(object sender, KeyPressEventArgs e)
        {
            Regex pat = new Regex(@"[\b]|[0-9,]");
            bool b = pat.IsMatch(e.KeyChar.ToString());
            if (b == false)
            {
                e.Handled = true;
            }
        }
    }
}
