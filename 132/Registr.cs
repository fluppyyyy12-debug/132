using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;
using WindowsFormsApp1;

namespace _132
{
    public partial class Registr : Form
    {
        public static string RegisteredUsername;
        public static string RegisteredPassword;
        public Registr()
        {
            InitializeComponent();
        }

        private void label2_Click(object sender, EventArgs e)
        {

        }

        private void button1_Click(object sender, EventArgs e)
        {
            // Проверка заполнения всех полей
            if (string.IsNullOrWhiteSpace(txtUsername.Text) ||
                string.IsNullOrWhiteSpace(txtPassword.Text) ||
                string.IsNullOrWhiteSpace(txtConfirmPassword.Text))
            {
                MessageBox.Show("Введите имя пользователя и пароль", "Ошибка",
                              MessageBoxButtons.OK, MessageBoxIcon.Error);
                return;
            }

            // Проверка совпадения паролей
            if (txtPassword.Text != txtConfirmPassword.Text)
            {
                MessageBox.Show("Пароли не совпадают", "Ошибка",
                              MessageBoxButtons.OK, MessageBoxIcon.Error);
                return;
            }

            DatabaseHelper db = new DatabaseHelper();
            if (db.RegisterUser(txtUsername.Text, txtPassword.Text))
            {
                MessageBox.Show("Регистрация успешна!", "Успех",
                               MessageBoxButtons.OK, MessageBoxIcon.Information);

                // Открываем форму входа
                var Form1 = new Avtoriz();
                Form1.Show();
                this.Hide();
            }
        }

        private void Регистрация_Load(object sender, EventArgs e)
        {

        }
    }
}