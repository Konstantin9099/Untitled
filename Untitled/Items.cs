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
    public partial class Items : Form
    {
        MySqlConnection conn = DBUtils.GetDBConnection();
        public Items()
        {
            InitializeComponent();
            this.MaximizeBox = false;
            this.AutoSizeMode = System.Windows.Forms.AutoSizeMode.GrowAndShrink;
            Get_Info();

            if (Convert.ToInt32(Status.user) > 1)
            {
                button2.Text = "Корзина";
                button3.Visible = false;
                label3.Visible = true;
                textBox3.Visible = true;
            }
            if (Convert.ToInt32(Status.user) == 1)
            {
                button2.Text = "Изменить";
                button3.Visible = true;
                label3.Visible = false;
                textBox3.Visible = false;
            }
        }

        //Выводим данные их БД в таблицу dataGridView1.
        public void Get_Info()
        {
            string query = "SELECT item_id as 'Код товара', item_name as 'Наименование товара', item_cost as 'Цена' FROM items; ";
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
                this.dataGridView1.Columns[1].Width = 400;
                this.dataGridView1.Columns[2].HeaderCell.Style.Alignment = DataGridViewContentAlignment.MiddleCenter;
                this.dataGridView1.Columns[2].Width = 200;
                dataGridView1.SelectionMode = DataGridViewSelectionMode.FullRowSelect;
                conn.Close();
            }
            catch (Exception ex)
            {
                MessageBox.Show("Произошла непредвиденая ошибка!" + Environment.NewLine + ex.Message);
            }
        }

            private void button4_Click(object sender, EventArgs e)
        {
            Menu menu = new Menu();
            menu.Owner = this;
            this.Hide();
            menu.Show();
        }

        private void Items_FormClosing(object sender, FormClosingEventArgs e)
        {
            Application.Exit();
        }

        private void dataGridView1_CellClick(object sender, DataGridViewCellEventArgs e)
        {
            // Данные для элементов формы.
            textBox1.Text = dataGridView1.CurrentRow.Cells[1].Value.ToString(); //Данные из столбца 1.
            textBox2.Text = dataGridView1.CurrentRow.Cells[2].Value.ToString(); //Данные из столбца 2

            // Данные для передачи с формы на форму.
            Item.item_id = dataGridView1.CurrentRow.Cells[0].Value.ToString(); //Определяем id записи.
            Item.item_name = dataGridView1.CurrentRow.Cells[1].Value.ToString(); //Данные из столбца 1.
            Item.item_cost = dataGridView1.CurrentRow.Cells[2].Value.ToString(); //Данные из столбца 2 
        }

        private void button1_Click(object sender, EventArgs e)
        {
            if (Convert.ToInt32(Status.user) == 1)
            {
                ItemsAdd itemsAdd = new ItemsAdd();
                itemsAdd.Owner = this;
                this.Hide();
                itemsAdd.Show();
            }
            else
            {
                // Проверяем, чтобы были заполнены все поля.
                if (textBox1.Text == null || textBox1.Text == "" || textBox2.Text == null || textBox2.Text == "" || textBox3.Text == null || textBox3.Text == "")
                {
                    MessageBox.Show("Выберете товар и его количество!", "Сообщение", MessageBoxButtons.OK, MessageBoxIcon.Information);
                    return;
                }
                else
                {
                    DialogResult res = MessageBox.Show("Добавить товар в корзину?", "Подтвердите действие", MessageBoxButtons.YesNo, MessageBoxIcon.Question);
                    if (res == DialogResult.Yes)
                    {
                        try
                        {
                            DateTime date = DateTime.Now;

                            string query = " INSERT INTO orders (order_date, order_user) values('" + date.ToString("yyyy-MM-dd") + "', " + Convert.ToInt32(Status.user) + "); INSERT INTO cort (order_id, item_id, amount) VALUES ((select order_id from orders where order_user=(select user_id from users where user_auth=" + Convert.ToInt32(Status.user) + ") order by order_user desc limit 1), " + Item.item_id + ", '" + Convert.ToInt32(textBox3.Text) + "'); ";
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
                            MessageBox.Show("Товар добавлен в корзину!", "Операция выполнена успешно");
                        }
                        catch (NullReferenceException ex)
                        {
                            MessageBox.Show("Произошла ошибка!" + Environment.NewLine + ex.Message);
                        }
                    }
                }
                textBox1.Clear();
                textBox2.Clear();
                textBox3.Clear();
            }
        }

        private void button2_Click(object sender, EventArgs e)
        {
            if (Convert.ToInt32(Status.user) > 1)
            {
                Cort cort = new Cort();
                cort.Owner = this;
                this.Hide();
                cort.Show();
            }
            else
            {
                if (textBox1.Text == "" && textBox2.Text == "")
                {
                    MessageBox.Show(
                        "Выберете товар в таблице товаров!",
                        "Сообщение",
                        MessageBoxButtons.OK,
                        MessageBoxIcon.Information);
                }
                else
                {
                    ItemsChange itemsChange = new ItemsChange();
                    itemsChange.Owner = this;
                    this.Hide();
                    itemsChange.Show();
                }
            }
        }

        private void textBox3_KeyPress(object sender, KeyPressEventArgs e)
        {
            e.Handled = !(char.IsDigit(e.KeyChar));
        }

        private void button3_Click(object sender, EventArgs e)
        {
            if (textBox1.Text == null || textBox1.Text == "" || textBox2.Text == null || textBox2.Text == "")
            {
                MessageBox.Show(
                    "Выберете в таблице данных товар, подлежащий удалению.",
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
                    try
                    {
                        // Проверяем - существует ли товар в заказе.
                        string count = "SELECT COUNT(*) FROM cort WHERE item_id=" + Item.item_id + ";";
 
                        MySqlCommand cmDB = new MySqlCommand(count, conn);
                        conn.Open();
                        MySqlCommand command = new MySqlCommand(count, conn);
                        int result = Int32.Parse(command.ExecuteScalar().ToString());
                        conn.Close();

                        if (result == 0)
                        {
                            string valueCell = dataGridView1.CurrentCell.Value != null ? dataGridView1.CurrentCell.Value.ToString() : "";
                            string del = "DELETE FROM items WHERE item_id = " + valueCell + ";";

                            MySqlCommand cmDB1 = new MySqlCommand(del, conn);
                            try
                            {
                                conn.Open();
                                cmDB1.ExecuteReader();
                                conn.Close();
                            }
                            catch (Exception ex)
                            {
                                MessageBox.Show("Произошла непредвиденная ошибка!" + Environment.NewLine + ex.Message);
                            }
                            Get_Info();
                            textBox1.Clear();
                            textBox2.Clear();
                        }
                        if (result == 1)
                        {
                            MessageBox.Show("Удаление невозможно, так как товар добавлен в заказ.", "Сообщение", MessageBoxButtons.OK, MessageBoxIcon.Information);
                            return;
                        }
                    }
                    catch (NullReferenceException ex)
                    {
                        MessageBox.Show("Произошла непредвиденная ошибка!" + Environment.NewLine + ex.Message);
                    }
                }
                else
                {
                    MessageBox.Show("Удаление записи отменено!");
                }
            }

        }

        private void radioButton1_CheckedChanged(object sender, EventArgs e)
        {
            dataGridView1.Sort(dataGridView1.Columns[1], ListSortDirection.Ascending);
        }

        private void radioButton2_CheckedChanged(object sender, EventArgs e)
        {
            dataGridView1.Sort(dataGridView1.Columns[2], ListSortDirection.Ascending);
        }

        private void button5_Click(object sender, EventArgs e)
        {
            string query = "SELECT item_id as 'Код товара', item_name as 'Наименование товара', item_cost as 'Цена' FROM items where item_name like '%" + textBox4.Text + "%'; ";
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
                this.dataGridView1.Columns[1].Width = 400;
                this.dataGridView1.Columns[2].HeaderCell.Style.Alignment = DataGridViewContentAlignment.MiddleCenter;
                this.dataGridView1.Columns[2].Width = 200;
                dataGridView1.SelectionMode = DataGridViewSelectionMode.FullRowSelect;
                conn.Close();
            }
            catch (Exception ex)
            {
                MessageBox.Show("Произошла непредвиденая ошибка!" + Environment.NewLine + ex.Message);
            }
        }

        private void button6_Click(object sender, EventArgs e)
        {
            Get_Info();
        }
    }

    static class Item
    {
        public static string item_id { get; set; }
        public static string item_name { get; set; }
        public static string item_cost { get; set; }
    }
}
