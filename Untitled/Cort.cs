using System;
using System.Collections;
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
using static System.Windows.Forms.VisualStyles.VisualStyleElement;

namespace Untitled
{
    public partial class Cort : Form
    {
        MySqlConnection conn = DBUtils.GetDBConnection();
        public Cort()
        {
            InitializeComponent();
            this.MaximizeBox = false;
            this.AutoSizeMode = System.Windows.Forms.AutoSizeMode.GrowAndShrink;
            Get_Info();
        }

        //Выводим данные их БД в таблицу dataGridView1.
        public void Get_Info()
        {
            string query = "select items.item_id as 'Код товара', item_name as 'Наименование товара', item_cost as 'Цена', amount as 'Количество' from items, cort, orders, users where items.item_id=cort.item_id and orders.order_user=users.user_id and orders.order_id=cort.order_id and users.user_auth=" + Status.user + "; ";
            MySqlDataAdapter sda = new MySqlDataAdapter(query, conn);
            DataTable dt = new DataTable();
            try
            {
                conn.Open();
                dataGridView1.DataSource = dt;
                dataGridView1.ClearSelection();
                sda.Fill(dt);
                dataGridView1.DataSource = dt;
                dataGridView1.ClearSelection();
                this.dataGridView1.Columns[0].HeaderCell.Style.Alignment = DataGridViewContentAlignment.MiddleCenter;
                this.dataGridView1.Columns[0].Width = 100;
                this.dataGridView1.Columns[1].HeaderCell.Style.Alignment = DataGridViewContentAlignment.MiddleCenter;
                this.dataGridView1.Columns[1].Width = 220;
                this.dataGridView1.Columns[2].HeaderCell.Style.Alignment = DataGridViewContentAlignment.MiddleCenter;
                this.dataGridView1.Columns[2].Width = 100;
                this.dataGridView1.Columns[3].HeaderCell.Style.Alignment = DataGridViewContentAlignment.MiddleCenter;
                this.dataGridView1.Columns[3].Width = 100;
                dataGridView1.SelectionMode = DataGridViewSelectionMode.FullRowSelect;
                conn.Close();
            }
            catch (Exception ex)
            {
                MessageBox.Show("Произошла непредвиденая ошибка!" + Environment.NewLine + ex.Message);
            }
        }

        private void Cort_FormClosing(object sender, FormClosingEventArgs e)
        {
            Application.Exit();
        }

        //Изменить количество товара в корзине
        private void button1_Click(object sender, EventArgs e)
        {
            // Проверяем, чтобы были заполнены поля ввода/вывода данных.
            if (textBox1.Text == null || textBox1.Text == "" || textBox2.Text == null || textBox2.Text == "" || textBox3.Text == null || textBox3.Text == "") // проверяем все поля - сколько надо на форме.
            {
                MessageBox.Show("Выберете товар в таблице заказанных товаров!", "Сообщение", MessageBoxButtons.OK, MessageBoxIcon.Information);
                return;
            }
            else
            {
                DialogResult res = MessageBox.Show("Изменить количество?", "Подтвердите действие", MessageBoxButtons.YesNo, MessageBoxIcon.Question);
                if (res == DialogResult.Yes)
                {
                    string query = "UPDATE cort SET amount='" + Convert.ToInt32(textBox3.Text) + "' WHERE cort.item_id=(select items.item_id from items where item_name='" + textBox1.Text + "' and amount='" + Cor.col + "'); ";
                    MySqlCommand cmDB = new MySqlCommand(query, conn);
                    try
                    {
                        conn.Open();
                        cmDB.ExecuteReader();
                        conn.Close();
                        Get_Info();
                    }
                    catch (Exception ex)
                    {
                        MessageBox.Show("Произошла непредвиденная ошибка!" + Environment.NewLine + ex.Message);
                    }
                    MessageBox.Show("Количество товара изменено!", "Операция выполнена успешно", MessageBoxButtons.OK, MessageBoxIcon.Information);
                }
            }
        }

        //Назад в товары
        private void button3_Click(object sender, EventArgs e)
        {
            Items items = new Items();
            items.Owner = this;
            this.Hide();
            items.Show();
        }

        //Удалить товар из корзины
        private void button2_Click(object sender, EventArgs e)
        {
            // Проверяем, чтобы были заполнены все поля.
            if (textBox1.Text == null || textBox1.Text == "" || textBox2.Text == null || textBox2.Text == "" || textBox3.Text == null || textBox3.Text == "")
            {
                MessageBox.Show(
                    "Выберете в таблице данных строку, подлежащую удалению.",
                    "Сообщение",
                    MessageBoxButtons.OK,
                    MessageBoxIcon.Information);
                return;
            }
            else
            {
                DialogResult res = MessageBox.Show("Удалить товар?", "Подтвердите действие", MessageBoxButtons.YesNo, MessageBoxIcon.Question);
                if (res == DialogResult.Yes)
                {
                    string del = "DELETE FROM cort WHERE cort.item_id=(select items.item_id from items where item_name='" + textBox1.Text + "' and amount='" + Cor.col + "');";
                    MySqlCommand cmDB = new MySqlCommand(del, conn);
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
                    MessageBox.Show("Товар удален!");
                    Get_Info();
                    textBox1.Clear();
                    textBox2.Clear();
                    textBox3.Clear();
                }
                else
                {
                    MessageBox.Show("Удаление записи отменено!");
                }
            }
        }

        private void dataGridView1_CellClick(object sender, DataGridViewCellEventArgs e)
        {
            // Данные для элементов формы.
            textBox1.Text = dataGridView1.CurrentRow.Cells[1].Value.ToString();
            textBox2.Text = dataGridView1.CurrentRow.Cells[2].Value.ToString();
            textBox3.Text = dataGridView1.CurrentRow.Cells[3].Value.ToString();

            // Данные для передачи с формы на форму.
            Cor.col = dataGridView1.CurrentRow.Cells[3].Value.ToString();
            Cor.item_id = dataGridView1.CurrentRow.Cells[1].Value.ToString();
            Cor.amount = dataGridView1.CurrentRow.Cells[2].Value.ToString();
        }

        private void textBox3_KeyPress(object sender, KeyPressEventArgs e)
        {
            e.Handled = !(char.IsDigit(e.KeyChar));
        }
    }

    static class Cor
    {
        public static string col { get; set; }
        public static string item_id { get; set; }
        public static string amount { get; set; }
    }
}
