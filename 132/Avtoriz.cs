using System;
using System.Drawing;
using System.Drawing.Drawing2D;
using System.Linq;
using System.Windows.Forms;
using WindowsFormsApp1;

namespace _132
{
    public partial class Avtoriz : Form
    {
        private string currentCaptcha = "";
        private bool captchaRequired = false;
        private int failedAttempts = 0;

        public Avtoriz()
        {
            InitializeComponent();
            InitializeCaptchaControls();

            // Явно добавляем обработчик события для кнопки
            if (refreshCaptchaButton != null)
            {
                refreshCaptchaButton.Click += new EventHandler(refreshCaptchaButton_Click);
            }

            // Назначаем обработчик для кнопки "Войти как гость"
            if (button2 != null)
            {
                button2.Click += BtnGuestLogin_Click;
            }
        }

        private void BtnGuestLogin_Click(object sender, EventArgs e)
        {
            // Открываем Form3 в режиме гостя (только просмотр, без заказов)
            var form3 = new Form3(false, false); // false = не администратор, false = не авторизован
            form3.Show();
            this.Hide();
        }

        private void InitializeCaptchaControls()
        {
            // Инициализация элементов CAPTCHA (скрытые при запуске)
            if (captchaPictureBox != null) captchaPictureBox.Visible = false;
            if (captchaTextBox != null) captchaTextBox.Visible = false;
            if (labelCaptcha != null) labelCaptcha.Visible = false;
            if (refreshCaptchaButton != null) refreshCaptchaButton.Visible = false;
        }

        // Метод для генерации CAPTCHA
        private void GenerateCaptcha()
        {
            // Проверяем, что PictureBox существует
            if (captchaPictureBox == null) return;

            try
            {
                // Очищаем предыдущее изображение
                if (captchaPictureBox.Image != null)
                {
                    captchaPictureBox.Image.Dispose();
                }

                // Случайные символы (цифры и буквы латинского алфавита)
                string chars = "ABCDEFGHIJKLMNOPQRSTUVWXYZabcdefghijklmnopqrstuvwxyz0123456789";
                Random random = new Random(Guid.NewGuid().GetHashCode());
                currentCaptcha = new string(Enumerable.Repeat(chars, 4)
                    .Select(s => s[random.Next(s.Length)]).ToArray());

                // Создаем изображение CAPTCHA
                Bitmap bitmap = new Bitmap(150, 60);
                using (Graphics g = Graphics.FromImage(bitmap))
                {
                    // Белый фон
                    g.Clear(Color.White);
                    g.SmoothingMode = SmoothingMode.AntiAlias;

                    // Добавляем графический шум - случайные линии
                    for (int i = 0; i < 10; i++)
                    {
                        using (Pen pen = new Pen(Color.FromArgb(
                            random.Next(150, 255),
                            random.Next(150, 255),
                            random.Next(150, 255)), 1))
                        {
                            g.DrawLine(pen,
                                new Point(random.Next(0, 150), random.Next(0, 60)),
                                new Point(random.Next(0, 150), random.Next(0, 60)));
                        }
                    }

                    // Добавляем случайные точки
                    for (int i = 0; i < 100; i++)
                    {
                        bitmap.SetPixel(random.Next(0, 150),
                                       random.Next(0, 60),
                                       Color.FromArgb(random.Next(150, 255),
                                                     random.Next(150, 255),
                                                     random.Next(150, 255)));
                    }

                    // Рисуем символы CAPTCHA с наложением и поворотом
                    using (Font font = new Font("Arial", 16, FontStyle.Bold))
                    {
                        for (int i = 0; i < currentCaptcha.Length; i++)
                        {
                            // Случайный поворот от -30 до 30 градусов
                            float angle = random.Next(-30, 30);

                            // Случайное положение с наложением
                            int x = 10 + i * 30 + random.Next(-5, 5);
                            int y = 15 + random.Next(-10, 10);

                            // Сохраняем трансформацию
                            g.TranslateTransform(x, y);
                            g.RotateTransform(angle);

                            // Рисуем символ
                            g.DrawString(currentCaptcha[i].ToString(), font,
                                        Brushes.Black, -10, -10);

                            // Восстанавливаем трансформацию
                            g.ResetTransform();

                            // Случайно перечеркиваем некоторые символы
                            if (random.Next(0, 3) == 0) // 33% вероятность
                            {
                                using (Pen crossPen = new Pen(Color.Red, 1))
                                {
                                    g.DrawLine(crossPen,
                                              x - 15 + random.Next(-5, 5),
                                              y - 15 + random.Next(-5, 5),
                                              x + 15 + random.Next(-5, 5),
                                              y + 15 + random.Next(-5, 5));
                                }
                            }
                        }
                    }
                }

                // Назначаем изображение PictureBox
                captchaPictureBox.Image = bitmap;

                // Очищаем текстовое поле и устанавливаем фокус
                if (captchaTextBox != null)
                {
                    captchaTextBox.Text = "";
                    captchaTextBox.Focus();
                }
            }
            catch (Exception ex)
            {
                MessageBox.Show($"Ошибка генерации CAPTCHA: {ex.Message}",
                              "Ошибка",
                              MessageBoxButtons.OK,
                              MessageBoxIcon.Error);
            }
        }

        private void ShowCaptchaControls()
        {
            if (captchaPictureBox != null) captchaPictureBox.Visible = true;
            if (captchaTextBox != null) captchaTextBox.Visible = true;
            if (labelCaptcha != null) labelCaptcha.Visible = true;
            if (refreshCaptchaButton != null) refreshCaptchaButton.Visible = true;
            GenerateCaptcha();
        }

        private void HideCaptchaControls()
        {
            if (captchaPictureBox != null) captchaPictureBox.Visible = false;
            if (captchaTextBox != null) captchaTextBox.Visible = false;
            if (labelCaptcha != null) labelCaptcha.Visible = false;
            if (refreshCaptchaButton != null) refreshCaptchaButton.Visible = false;
            if (captchaTextBox != null) captchaTextBox.Text = "";
            captchaRequired = false;
        }

        private void button1_Click(object sender, EventArgs e)
        {
            // Проверка на пустые поля
            if (string.IsNullOrWhiteSpace(RegisteredUsername.Text) ||
                string.IsNullOrWhiteSpace(RegisteredPassword.Text))
            {
                MessageBox.Show("Введите логин и пароль", "Ошибка",
                              MessageBoxButtons.OK, MessageBoxIcon.Error);
                return;
            }

            // Проверка CAPTCHA если требуется
            if (captchaRequired)
            {
                if (captchaTextBox == null || string.IsNullOrWhiteSpace(captchaTextBox.Text))
                {
                    MessageBox.Show("Введите CAPTCHA", "Ошибка",
                                  MessageBoxButtons.OK, MessageBoxIcon.Error);
                    if (captchaTextBox != null) captchaTextBox.Focus();
                    return;
                }

                if (captchaTextBox.Text.Trim() != currentCaptcha)
                {
                    MessageBox.Show("Неверная CAPTCHA", "Ошибка",
                                  MessageBoxButtons.OK, MessageBoxIcon.Error);
                    GenerateCaptcha();
                    if (captchaTextBox != null)
                    {
                        captchaTextBox.Text = "";
                        captchaTextBox.Focus();
                    }
                    return;
                }
            }

            string username = RegisteredUsername.Text.Trim();
            string password = RegisteredPassword.Text;

            // ПРОВЕРКА НА АДМИНА
            if (username.ToLower() == "admin" && password == "admin")
            {
                MessageBox.Show("Вход выполнен как администратор!", "Успех",
                               MessageBoxButtons.OK, MessageBoxIcon.Information);

                // Сброс счетчика неудачных попыток
                failedAttempts = 0;
                HideCaptchaControls();

                // Админ переходит на Form3 с правами админа и авторизацией
                var form3 = new Form3(true, true); // true = администратор, true = авторизован
                form3.Show();
                this.Hide();
            }
            else
            {
                // Проверка обычного пользователя через БД
                DatabaseHelper db = new DatabaseHelper();

                if (db.ValidateUser(username, password))
                {
                    MessageBox.Show("Вход выполнен!", "Успех",
                                   MessageBoxButtons.OK, MessageBoxIcon.Information);

                    // Сброс счетчика неудачных попыток
                    failedAttempts = 0;
                    HideCaptchaControls();

                    // Обычный пользователь переходит на Form3 без прав админа, но авторизован
                    var form3 = new Form3(false, true); // false = обычный пользователь, true = авторизован
                    form3.Show();
                    this.Hide();
                }
                else
                {
                    failedAttempts++;

                    // После первой неудачной попытки показываем CAPTCHA
                    if (failedAttempts >= 1)
                    {
                        if (!captchaRequired)
                        {
                            ShowCaptchaControls();
                            captchaRequired = true;
                        }

                        MessageBox.Show("Неправильный логин или пароль. Введите также CAPTCHA.", "Ошибка",
                                       MessageBoxButtons.OK, MessageBoxIcon.Error);
                    }
                    else
                    {
                        MessageBox.Show("Неправильный логин или пароль", "Ошибка",
                                       MessageBoxButtons.OK, MessageBoxIcon.Error);
                    }

                    RegisteredPassword.Text = "";
                    if (captchaTextBox != null) captchaTextBox.Text = "";

                    if (captchaRequired && captchaTextBox != null)
                        captchaTextBox.Focus();
                    else
                        RegisteredUsername.Focus();
                }
            }
        }

        // Обработчик кнопки обновления CAPTCHA
        private void refreshCaptchaButton_Click(object sender, EventArgs e)
        {
            GenerateCaptcha();
        }

        // Очистка CAPTCHA при смене логина/пароля
        private void RegisteredUsername_TextChanged(object sender, EventArgs e)
        {
            if (captchaRequired)
            {
                GenerateCaptcha();
                if (captchaTextBox != null) captchaTextBox.Text = "";
            }
        }

        private void RegisteredPassword_TextChanged(object sender, EventArgs e)
        {
            if (captchaRequired)
            {
                GenerateCaptcha();
                if (captchaTextBox != null) captchaTextBox.Text = "";
            }
        }

        // Остальные методы
        private void Form1_Load(object sender, EventArgs e) { }
        private void textBox1_TextChanged(object sender, EventArgs e) { }
        private void label3_Click(object sender, EventArgs e) { }
        private void textBox2_TextChanged(object sender, EventArgs e) { }

        private void linkLabel1_LinkClicked(object sender, LinkLabelLinkClickedEventArgs e)
        {
            var registr = new Registr();
            registr.Show();
            this.Hide();
        }

        private void label2_Click(object sender, EventArgs e)
        {

        }

        private void label1_Click(object sender, EventArgs e)
        {

        }
    }
}