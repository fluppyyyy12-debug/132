using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Drawing.Imaging;
using System.IO;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;
using WindowsFormsApp1;

namespace _132
{
    public partial class Dobavlenie : Form
    {
        private DatabaseHelper db;
        private string photosFolder = @"C:\Users\student\Desktop\Практика ПМ 03-ПМ05\Задание на практику\Вариант 1\Сессия 1\Товар_import";
        private string tempImagePath = null;
        private bool imageChanged = false;
        private static bool formIsOpen = false;

        // Храним ID для категорий, поставщиков и единиц измерения
        private Dictionary<string, int> categoryMap = new Dictionary<string, int>();
        private Dictionary<string, int> supplierMap = new Dictionary<string, int>();
        private Dictionary<string, int> unitMap = new Dictionary<string, int>();

        public Dobavlenie()
        {
            if (formIsOpen)
            {
                MessageBox.Show("Форма редактирования уже открыта!", "Информация",
                    MessageBoxButtons.OK, MessageBoxIcon.Information);
                this.DialogResult = DialogResult.Cancel;
                return;
            }

            InitializeComponent();
            formIsOpen = true;

            // Проверяем и создаем папку для фото при запуске формы
            CheckAndCreatePhotosFolder();

            // ПОДКЛЮЧАЕМ ОБРАБОТЧИКИ СОБЫТИЙ
            AttachEventHandlers();
        }

        private void CheckAndCreatePhotosFolder()
        {
            try
            {
                if (!Directory.Exists(photosFolder))
                {
                    Directory.CreateDirectory(photosFolder);
                    Console.WriteLine($"Создана папка для фото: {photosFolder}");
                }
            }
            catch (Exception ex)
            {
                Console.WriteLine($"Ошибка создания папки для фото: {ex.Message}");
            }
        }

        private void AttachEventHandlers()
        {
            btnLoadImage.Click += btnLoadImage_Click;
            btnRemoveImage.Click += btnRemoveImage_Click;
            btnSave.Click += btnSave_Click;
            btnCancel.Click += btnCancel_Click;
            txtPrice.KeyPress += txtPrice_KeyPress;
            txtStock.KeyPress += txtStock_KeyPress;
            txtMinimumStock.KeyPress += txtMinimumStock_KeyPress;
            txtPrice.TextChanged += TxtPrice_TextChanged;
        }

        private void Form4_Load(object sender, EventArgs e)
        {
            db = new DatabaseHelper();
            InitializeForm();
        }

        private void InitializeForm()
        {
            this.Text = "Добавление нового товара";
            btnSave.Text = "Добавить товар";

            // Устанавливаем значения по умолчанию
            txtStock.Text = "0";
            txtPrice.Text = "0,00";
            txtMinimumStock.Text = "0";

            // Загружаем списки из базы данных
            LoadCategories();
            LoadSuppliers();
            LoadUnits();

            // Загружаем изображение-заглушку
            pbProductImage.Image = CreateDefaultImage();

            // Поле ID скрыто для добавления
            lblProductId.Text = "Новый товар";
        }

        private void LoadCategories()
        {
            try
            {
                var categories = db.GetCategories();
                cmbCategory.Items.Clear();
                cmbCategory.Items.Add("-- Выберите категорию --");
                categoryMap.Clear();

                if (categories != null && categories.Rows.Count > 0 && categories.Columns.Count > 0)
                {
                    // Определяем имена столбцов
                    string idColumn = null;
                    string nameColumn = null;

                    // Ищем ID столбец
                    foreach (DataColumn col in categories.Columns)
                    {
                        if (col.ColumnName.ToLower().Contains("id"))
                        {
                            idColumn = col.ColumnName;
                            break;
                        }
                    }

                    // Ищем имя столбца
                    foreach (string name in new[] { "Name", "CategoryName", "Category", "Title" })
                    {
                        if (categories.Columns.Contains(name))
                        {
                            nameColumn = name;
                            break;
                        }
                    }

                    if (nameColumn == null && categories.Columns.Count > 1)
                        nameColumn = categories.Columns[1].ColumnName;
                    else if (nameColumn == null)
                        nameColumn = categories.Columns[0].ColumnName;

                    foreach (DataRow row in categories.Rows)
                    {
                        string categoryName = GetValueFromRow(row, nameColumn);
                        if (!string.IsNullOrEmpty(categoryName))
                        {
                            cmbCategory.Items.Add(categoryName);

                            // Сохраняем ID категории
                            int categoryId = 0;
                            if (idColumn != null && row[idColumn] != DBNull.Value)
                            {
                                categoryId = Convert.ToInt32(row[idColumn]);
                            }
                            categoryMap[categoryName] = categoryId;
                        }
                    }
                }
                else
                {
                    // Если в базе нет категорий, предлагаем создать
                    MessageBox.Show("В базе данных нет категорий. Добавьте категории в базу данных.",
                        "Внимание", MessageBoxButtons.OK, MessageBoxIcon.Warning);
                }

                if (cmbCategory.Items.Count > 0)
                    cmbCategory.SelectedIndex = 0;
            }
            catch (Exception ex)
            {
                MessageBox.Show($"Ошибка загрузки категорий: {ex.Message}", "Ошибка",
                    MessageBoxButtons.OK, MessageBoxIcon.Error);
            }
        }

        private void LoadSuppliers()
        {
            try
            {
                var suppliers = db.GetSuppliers();
                cmbSupplier.Items.Clear();
                cmbSupplier.Items.Add("-- Выберите поставщика --");
                supplierMap.Clear();

                if (suppliers != null && suppliers.Rows.Count > 0 && suppliers.Columns.Count > 0)
                {
                    // Определяем имена столбцов
                    string idColumn = null;
                    string nameColumn = null;

                    // Ищем ID столбец
                    foreach (DataColumn col in suppliers.Columns)
                    {
                        if (col.ColumnName.ToLower().Contains("id"))
                        {
                            idColumn = col.ColumnName;
                            break;
                        }
                    }

                    // Ищем имя столбца
                    foreach (string name in new[] { "Name", "SupplierName", "Supplier", "Company" })
                    {
                        if (suppliers.Columns.Contains(name))
                        {
                            nameColumn = name;
                            break;
                        }
                    }

                    if (nameColumn == null && suppliers.Columns.Count > 1)
                        nameColumn = suppliers.Columns[1].ColumnName;
                    else if (nameColumn == null)
                        nameColumn = suppliers.Columns[0].ColumnName;

                    foreach (DataRow row in suppliers.Rows)
                    {
                        string supplierName = GetValueFromRow(row, nameColumn);
                        if (!string.IsNullOrEmpty(supplierName))
                        {
                            cmbSupplier.Items.Add(supplierName);

                            // Сохраняем ID поставщика
                            int supplierId = 0;
                            if (idColumn != null && row[idColumn] != DBNull.Value)
                            {
                                supplierId = Convert.ToInt32(row[idColumn]);
                            }
                            supplierMap[supplierName] = supplierId;
                        }
                    }
                }
                else
                {
                    MessageBox.Show("В базе данных нет поставщиков. Добавьте поставщиков в базу данных.",
                        "Внимание", MessageBoxButtons.OK, MessageBoxIcon.Warning);
                }

                if (cmbSupplier.Items.Count > 0)
                    cmbSupplier.SelectedIndex = 0;
            }
            catch (Exception ex)
            {
                MessageBox.Show($"Ошибка загрузки поставщиков: {ex.Message}", "Ошибка",
                    MessageBoxButtons.OK, MessageBoxIcon.Error);
            }
        }

        private void LoadUnits()
        {
            try
            {
                var units = db.GetUnits();
                cmbUnit.Items.Clear();
                cmbUnit.Items.Add("-- Выберите единицу измерения --");
                unitMap.Clear();

                if (units != null && units.Rows.Count > 0 && units.Columns.Count > 0)
                {
                    // Определяем имена столбцов
                    string idColumn = null;
                    string nameColumn = null;

                    // Ищем ID столбец
                    foreach (DataColumn col in units.Columns)
                    {
                        if (col.ColumnName.ToLower().Contains("id"))
                        {
                            idColumn = col.ColumnName;
                            break;
                        }
                    }

                    // Ищем имя столбца
                    foreach (string name in new[] { "Name", "UnitName", "Unit", "Measure" })
                    {
                        if (units.Columns.Contains(name))
                        {
                            nameColumn = name;
                            break;
                        }
                    }

                    if (nameColumn == null && units.Columns.Count > 1)
                        nameColumn = units.Columns[1].ColumnName;
                    else if (nameColumn == null)
                        nameColumn = units.Columns[0].ColumnName;

                    foreach (DataRow row in units.Rows)
                    {
                        string unitName = GetValueFromRow(row, nameColumn);
                        if (!string.IsNullOrEmpty(unitName))
                        {
                            cmbUnit.Items.Add(unitName);

                            // Сохраняем ID единицы измерения
                            int unitId = 0;
                            if (idColumn != null && row[idColumn] != DBNull.Value)
                            {
                                unitId = Convert.ToInt32(row[idColumn]);
                            }
                            unitMap[unitName] = unitId;
                        }
                    }
                }
                else
                {
                    MessageBox.Show("В базе данных нет единиц измерения. Добавьте единицы измерения в базу данных.",
                        "Внимание", MessageBoxButtons.OK, MessageBoxIcon.Warning);
                }

                if (cmbUnit.Items.Count > 0)
                    cmbUnit.SelectedIndex = 0;
            }
            catch (Exception ex)
            {
                MessageBox.Show($"Ошибка загрузки единиц измерения: {ex.Message}", "Ошибка",
                    MessageBoxButtons.OK, MessageBoxIcon.Error);
            }
        }

        // Вспомогательный метод для безопасного получения значения из строки
        private string GetValueFromRow(DataRow row, string columnName)
        {
            try
            {
                if (row.Table.Columns.Contains(columnName) && row[columnName] != DBNull.Value)
                    return row[columnName].ToString();

                return "";
            }
            catch
            {
                return "";
            }
        }

        private Image CreateDefaultImage()
        {
            Bitmap bmp = new Bitmap(300, 200);
            using (Graphics g = Graphics.FromImage(bmp))
            {
                g.Clear(Color.LightGray);
                g.DrawString("Нет фото\n(300x200)",
                    new Font("Arial", 10, FontStyle.Bold),
                    Brushes.Black,
                    new RectangleF(0, 0, 300, 200),
                    new StringFormat
                    {
                        Alignment = StringAlignment.Center,
                        LineAlignment = StringAlignment.Center
                    });
            }
            return bmp;
        }

        private void btnLoadImage_Click(object sender, EventArgs e)
        {
            using (OpenFileDialog ofd = new OpenFileDialog())
            {
                ofd.Filter = "Изображения (*.jpg;*.jpeg;*.png;*.bmp)|*.jpg;*.jpeg;*.png;*.bmp";
                ofd.Title = "Выберите изображение товара";

                if (ofd.ShowDialog() == DialogResult.OK)
                {
                    try
                    {
                        // Проверяем размер изображения
                        using (Image originalImage = Image.FromFile(ofd.FileName))
                        {
                            if (originalImage.Width != 300 || originalImage.Height != 200)
                            {
                                DialogResult resizeResult = MessageBox.Show(
                                    $"Изображение имеет размер {originalImage.Width}x{originalImage.Height}.\n" +
                                    "Рекомендуемый размер: 300x200 пикселей.\n" +
                                    "Изменить размер автоматически?",
                                    "Предупреждение",
                                    MessageBoxButtons.YesNoCancel,
                                    MessageBoxIcon.Warning);

                                if (resizeResult == DialogResult.Cancel)
                                    return;

                                if (resizeResult == DialogResult.Yes)
                                {
                                    // Изменяем размер изображения
                                    Bitmap resizedImage = new Bitmap(300, 200);
                                    using (Graphics g = Graphics.FromImage(resizedImage))
                                    {
                                        g.InterpolationMode = System.Drawing.Drawing2D.InterpolationMode.HighQualityBicubic;
                                        g.DrawImage(originalImage, 0, 0, 300, 200);
                                    }

                                    // Сохраняем во временный файл
                                    tempImagePath = Path.Combine(Path.GetTempPath(), Guid.NewGuid() + ".jpg");
                                    resizedImage.Save(tempImagePath, ImageFormat.Jpeg);
                                    pbProductImage.Image = resizedImage;
                                }
                                else
                                {
                                    // Используем оригинальное изображение
                                    tempImagePath = ofd.FileName;
                                    pbProductImage.Image = (Image)originalImage.Clone();
                                }
                            }
                            else
                            {
                                // Изображение уже правильного размера
                                tempImagePath = ofd.FileName;
                                pbProductImage.Image = (Image)originalImage.Clone();
                            }
                        }

                        imageChanged = true;
                        lblImageInfo.Text = Path.GetFileName(tempImagePath);
                    }
                    catch (Exception ex)
                    {
                        MessageBox.Show($"Ошибка загрузки изображения: {ex.Message}", "Ошибка",
                            MessageBoxButtons.OK, MessageBoxIcon.Error);
                    }
                }
            }
        }

        private void btnRemoveImage_Click(object sender, EventArgs e)
        {
            pbProductImage.Image = CreateDefaultImage();
            tempImagePath = null;
            imageChanged = false;
            lblImageInfo.Text = "Изображение не выбрано";
        }

        private void TxtPrice_TextChanged(object sender, EventArgs e)
        {
            // Автоматическое форматирование цены
            try
            {
                if (decimal.TryParse(txtPrice.Text, out decimal price))
                {
                    txtPrice.Text = price.ToString("N2");
                    txtPrice.SelectionStart = txtPrice.Text.Length;
                }
            }
            catch { }
        }

        private bool ValidateInput()
        {
            // Проверка наименования
            if (string.IsNullOrWhiteSpace(txtName.Text))
            {
                MessageBox.Show("Введите наименование товара!", "Ошибка",
                    MessageBoxButtons.OK, MessageBoxIcon.Warning);
                txtName.Focus();
                return false;
            }

            // Проверка категории
            if (cmbCategory.SelectedIndex <= 0)
            {
                MessageBox.Show("Выберите категорию товара!", "Ошибка",
                    MessageBoxButtons.OK, MessageBoxIcon.Warning);
                cmbCategory.Focus();
                return false;
            }

            // Проверка количества на складе
            if (!int.TryParse(txtStock.Text, out int stock) || stock < 0)
            {
                MessageBox.Show("Введите корректное количество на складе (не может быть отрицательным)!", "Ошибка",
                    MessageBoxButtons.OK, MessageBoxIcon.Warning);
                txtStock.Focus();
                return false;
            }

            // Проверка минимального количества
            if (!int.TryParse(txtMinimumStock.Text, out int minStock) || minStock < 0)
            {
                MessageBox.Show("Введите корректное минимальное количество (не может быть отрицательным)!", "Ошибка",
                    MessageBoxButtons.OK, MessageBoxIcon.Warning);
                txtMinimumStock.Focus();
                return false;
            }

            // Проверка единицы измерения
            if (cmbUnit.SelectedIndex <= 0)
            {
                MessageBox.Show("Выберите единицу измерения!", "Ошибка",
                    MessageBoxButtons.OK, MessageBoxIcon.Warning);
                cmbUnit.Focus();
                return false;
            }

            // Проверка поставщика
            if (cmbSupplier.SelectedIndex <= 0)
            {
                MessageBox.Show("Выберите поставщика!", "Ошибка",
                    MessageBoxButtons.OK, MessageBoxIcon.Warning);
                cmbSupplier.Focus();
                return false;
            }

            // Проверка цены
            if (!decimal.TryParse(txtPrice.Text, out decimal price) || price < 0)
            {
                MessageBox.Show("Введите корректную цену (не может быть отрицательной)!", "Ошибка",
                    MessageBoxButtons.OK, MessageBoxIcon.Warning);
                txtPrice.Focus();
                return false;
            }

            return true;
        }

        private string SaveProductImage()
        {
            if (string.IsNullOrEmpty(tempImagePath) || !File.Exists(tempImagePath))
                return null;

            try
            {
                // Создаем имя файла на основе названия товара
                string safeProductName = MakeSafeFileName(txtName.Text);
                string extension = Path.GetExtension(tempImagePath).ToLower();
                string fileName = $"{safeProductName}_{DateTime.Now:yyyyMMdd_HHmmss}{extension}";
                string destinationPath = Path.Combine(photosFolder, fileName);

                // Проверяем существование папки
                if (!Directory.Exists(photosFolder))
                    Directory.CreateDirectory(photosFolder);

                // Копируем файл
                File.Copy(tempImagePath, destinationPath, true);

                // Возвращаем только имя файла (не полный путь)
                return fileName;
            }
            catch (Exception ex)
            {
                MessageBox.Show($"Ошибка сохранения изображения: {ex.Message}", "Ошибка",
                    MessageBoxButtons.OK, MessageBoxIcon.Error);
                return null;
            }
        }

        private string MakeSafeFileName(string fileName)
        {
            if (string.IsNullOrEmpty(fileName))
                return "product";

            // Убираем недопустимые символы
            char[] invalidChars = Path.GetInvalidFileNameChars();
            foreach (char invalidChar in invalidChars)
            {
                fileName = fileName.Replace(invalidChar.ToString(), "");
            }

            // Заменяем пробелы на подчеркивания
            fileName = fileName.Replace(" ", "_").ToLower();

            // Ограничиваем длину имени файла
            if (fileName.Length > 50)
                fileName = fileName.Substring(0, 50);

            return fileName;
        }

        private void btnSave_Click(object sender, EventArgs e)
        {
            if (!ValidateInput())
                return;

            try
            {
                // Сохраняем изображение (если есть)
                string imagePath = null;
                if (imageChanged && !string.IsNullOrEmpty(tempImagePath) && File.Exists(tempImagePath))
                {
                    imagePath = SaveProductImage();
                }

                // Получаем выбранные значения
                string categoryName = cmbCategory.SelectedItem?.ToString();
                string unitName = cmbUnit.SelectedItem?.ToString();
                string supplierName = cmbSupplier.SelectedItem?.ToString();

                // Для отладки
                Console.WriteLine($"Добавление товара: {txtName.Text}");
                Console.WriteLine($"Категория: {categoryName}");
                Console.WriteLine($"Изображение: {imagePath ?? "нет"}");

                // Добавляем товар в базу данных
                bool success = db.AddProduct(
                    txtName.Text.Trim(),
                    txtDescription.Text.Trim(),
                    categoryName,
                    txtManufacturer.Text.Trim(),
                    decimal.Parse(txtPrice.Text),
                    int.Parse(txtStock.Text),
                    int.Parse(txtMinimumStock.Text),
                    unitName,
                    supplierName,
                    imagePath
                );

                if (success)
                {
                    MessageBox.Show("Товар успешно добавлен!", "Успех",
                        MessageBoxButtons.OK, MessageBoxIcon.Information);
                    this.DialogResult = DialogResult.OK;
                    this.Close(); // Закрываем форму
                }
                else
                {
                    MessageBox.Show("Не удалось добавить товар. Проверьте данные и подключение к базе.", "Ошибка",
                        MessageBoxButtons.OK, MessageBoxIcon.Error);
                }
            }
            catch (Exception ex)
            {
                MessageBox.Show($"Ошибка при добавлении товара: {ex.Message}", "Ошибка",
                    MessageBoxButtons.OK, MessageBoxIcon.Error);
            }
        }

        private void btnCancel_Click(object sender, EventArgs e)
        {
            this.DialogResult = DialogResult.Cancel;
            this.Close();
        }

        private void ResetForm()
        {
            // Сбрасываем все поля формы
            txtName.Text = "";
            txtDescription.Text = "";
            txtManufacturer.Text = "";
            txtPrice.Text = "0,00";
            txtStock.Text = "0";
            txtMinimumStock.Text = "0";

            // Сбрасываем выпадающие списки
            if (cmbCategory.Items.Count > 0)
                cmbCategory.SelectedIndex = 0;

            if (cmbUnit.Items.Count > 0)
                cmbUnit.SelectedIndex = 0;

            if (cmbSupplier.Items.Count > 0)
                cmbSupplier.SelectedIndex = 0;

            // Сбрасываем изображение
            btnRemoveImage_Click(null, null);
        }

        private void Form4_FormClosed(object sender, FormClosedEventArgs e)
        {
            formIsOpen = false;

            // Очищаем временные файлы
            if (!string.IsNullOrEmpty(tempImagePath) &&
                tempImagePath.StartsWith(Path.GetTempPath()) &&
                File.Exists(tempImagePath))
            {
                try { File.Delete(tempImagePath); } catch { }
            }

            // Очищаем изображение
            if (pbProductImage.Image != null)
            {
                pbProductImage.Image.Dispose();
            }
        }

        private void txtPrice_KeyPress(object sender, KeyPressEventArgs e)
        {
            // Разрешаем цифры, запятую и управляющие символы
            if (!char.IsControl(e.KeyChar) && !char.IsDigit(e.KeyChar) && e.KeyChar != ',')
            {
                e.Handled = true;
            }

            // Проверяем, что запятая только одна
            if (e.KeyChar == ',' && ((TextBox)sender).Text.Contains(','))
            {
                e.Handled = true;
            }
        }

        private void txtStock_KeyPress(object sender, KeyPressEventArgs e)
        {
            // Разрешаем только цифры и управляющие символы
            if (!char.IsControl(e.KeyChar) && !char.IsDigit(e.KeyChar))
            {
                e.Handled = true;
            }
        }

        private void txtMinimumStock_KeyPress(object sender, KeyPressEventArgs e)
        {
            // Разрешаем только цифры и управляющие символы
            if (!char.IsControl(e.KeyChar) && !char.IsDigit(e.KeyChar))
            {
                e.Handled = true;
            }
        }
    }
}