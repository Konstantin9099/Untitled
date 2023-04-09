using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;
using System.Xml.Linq;
using MySql.Data.MySqlClient;

namespace Untitled
{
    public partial class ItemsChange : Form
    {
        MySqlConnection conn = DBUtils.GetDBConnection();
        public ItemsChange()
        {
            InitializeComponent();
        }

        private void ItemsChange_Load(object sender, EventArgs e)
        {
            textBox1.Text = Item.item_name;
            textBox2.Text = Item.item_cost;
        }

        private void button2_Click(object sender, EventArgs e)
        {
            Items items = new Items();
            items.Owner = this;
            this.Hide();
            items.Show();
        }

        private void ItemsChange_FormClosing(object sender, FormClosingEventArgs e)
        {
            Application.Exit();
        }

        private void button1_Click(object sender, EventArgs e)
        {
            // Проверяем, чтобы были заполнены поля ввода/вывода данных.
            if (textBox1.Text == null || textBox1.Text == "" || textBox2.Text == null || textBox2.Text == "") // проверяем все поля - сколько надо на форме.
            {
                MessageBox.Show("Введите полные данные!", "Сообщение", MessageBoxButtons.OK, MessageBoxIcon.Information);
                return;
            }
            else
            {
                DialogResult res = MessageBox.Show("Изменить данные?", "Подтвердите действие", MessageBoxButtons.YesNo, MessageBoxIcon.Question);
                if (res == DialogResult.Yes)
                {
                    string query = "UPDATE items SET item_name='" + textBox1.Text + "', item_cost='" + Convert.ToDecimal(textBox2.Text) + "' WHERE item_id=" + Item.item_id + "; ";
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
                    MessageBox.Show("Данные изменены.", "Операция выполнена успешно", MessageBoxButtons.OK, MessageBoxIcon.Information);
                }
                button1.Enabled = false;
            }
        }
    }
}
