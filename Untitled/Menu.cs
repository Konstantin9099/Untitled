using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;

namespace Untitled
{
    public partial class Menu : Form
    {
        public Menu()
        {
            InitializeComponent();
            if (Convert.ToInt32(Status.user) > 1)
            {
                button2.Enabled = true;
            }
        }

        //Товары
        private void button1_Click(object sender, EventArgs e)
        {
            Items items = new Items();
            items.Owner = this;
            this.Hide();
            items.Show();
        }

        //Заказы
        private void button2_Click(object sender, EventArgs e)
        {
            if (Convert.ToInt32(Status.user) == 1)
            {
                Orders orders = new Orders();
                orders.Owner = this;
                this.Hide();
                orders.Show();
            }
            else
            {
                MessageBox.Show("Доступ к данным ограничен!");
            }
        }

        //Профиль
        private void button4_Click(object sender, EventArgs e)
        {
            Users users = new Users();
            users.Owner = this;
            this.Hide();
            users.Show();
        }

        //Выход из программы
        private void button5_Click(object sender, EventArgs e)
        {
            Application.Exit();
        }

        private void Menu_FormClosing(object sender, FormClosingEventArgs e)
        {
            Application.Exit();
        }
    }
}
