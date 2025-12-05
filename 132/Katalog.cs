using System;
using System.Collections.Generic;
using System.Data;
using System.Drawing;
using System.IO;
using System.Linq;
using System.Windows.Forms;
using WindowsFormsApp1;

namespace _132
{
    public partial class Form3 : Form
    {
        private DatabaseHelper db;
        private List<Product> products = new List<Product>();
        private List<string> manufacturers = new List<string>();
        private Image defaultImage;
        private bool isAdmin = false;
        private bool isLoggedInUser = false; // Добавляем флаг авторизации

        // ПУТЬ К ПАПКЕ С ФОТОГРАФИЯМИ ТОВАРОВ
        private string photosFolder = @"C:\Users\student\Desktop\Практика ПМ 03-ПМ05\Задание на практику\Вариант 1\Сессия 1\Товар_import";

        // Изменяем конструктор: добавляем второй параметр для авторизации
        public Form3(bool isAdminUser = false, bool isLoggedIn = false)
        {
            InitializeComponent();
            isAdmin = isAdminUser;
            isLoggedInUser = isLoggedIn; // Сохраняем статус авторизации
            InitializeForm();
            UpdateUIForUserRole();
        }

        private void InitializeForm()
        {
            db = new DatabaseHelper();
            LoadDefaultImage();
            ConfigureDataGridView();
            LoadProducts();
            LoadManufacturers();
            ConfigureFilters();
            AttachEventHandlers();
            UpdateProductCount();
        }

        private void UpdateUIForUserRole()
        {
            btnAddProduct.Visible = isAdmin;
            btnDeleteProduct.Visible = isAdmin;

            // Настройка кнопки заказа/входа
            if (!isAdmin)
            {
                button1.Visible = true; // Всегда показываем кнопку обычным пользователям и гостям

                if (isLoggedInUser)
                {
                    // Для авторизованных пользователей
                    button1.Text = "Заказать";
                    button1.BackColor = Color.ForestGreen;
                }
                else
                {
                    // Для гостей
                    button1.Text = "Войти для заказа";
                    button1.BackColor = Color.Orange;
                }
            }
            else
            {
                // Для администраторов скрываем кнопку заказа
                button1.Visible = false;
            }

            // Настройка заголовка окна
            this.Text = isAdmin ? "Управление товарами (Администратор)" :
                       isLoggedInUser ? "Каталог товаров" : "Каталог товаров (Гость)";
        }

        private void LoadDefaultImage()
        {
            try
            {
                defaultImage = CreateDefaultImage();
            }
            catch
            {
                defaultImage = CreateDefaultImage();
            }
        }

        private Image CreateDefaultImage()
        {
            Bitmap bmp = new Bitmap(100, 100);
            using (Graphics g = Graphics.FromImage(bmp))
            {
                g.Clear(Color.LightGray);
                g.DrawString("Нет фото",
                    new Font("Arial", 10),
                    Brushes.Black,
                    new RectangleF(0, 0, 100, 100),
                    new StringFormat
                    {
                        Alignment = StringAlignment.Center,
                        LineAlignment = StringAlignment.Center
                    });
            }
            return bmp;
        }

        private void ConfigureDataGridView()
        {
            dataGridViewProducts.Columns.Clear();
            dataGridViewProducts.AutoGenerateColumns = false;
            dataGridViewProducts.AllowUserToAddRows = false;
            dataGridViewProducts.AllowUserToDeleteRows = false;
            dataGridViewProducts.ReadOnly = true;
            dataGridViewProducts.SelectionMode = DataGridViewSelectionMode.FullRowSelect;
            dataGridViewProducts.RowTemplate.Height = 100;
            dataGridViewProducts.AutoSizeRowsMode = DataGridViewAutoSizeRowsMode.None;
            dataGridViewProducts.MultiSelect = false;

            CreateDataGridViewColumns();
        }

        private void CreateDataGridViewColumns()
        {
            // Колонка с фото (обязательно первая!)
            DataGridViewImageColumn imageColumn = new DataGridViewImageColumn
            {
                Name = "Image",
                HeaderText = "Фото",
                ImageLayout = DataGridViewImageCellLayout.Zoom,
                Width = 120,
                DefaultCellStyle = new DataGridViewCellStyle
                {
                    Alignment = DataGridViewContentAlignment.MiddleCenter
                }
            };
            dataGridViewProducts.Columns.Add(imageColumn);

            // Колонка с названием
            DataGridViewTextBoxColumn nameColumn = new DataGridViewTextBoxColumn
            {
                Name = "Name",
                HeaderText = "Наименование",
                Width = 200
            };
            dataGridViewProducts.Columns.Add(nameColumn);

            // Колонка с описанием
            DataGridViewTextBoxColumn descColumn = new DataGridViewTextBoxColumn
            {
                Name = "Description",
                HeaderText = "Описание",
                Width = 250,
                DefaultCellStyle = new DataGridViewCellStyle { WrapMode = DataGridViewTriState.True }
            };
            dataGridViewProducts.Columns.Add(descColumn);

            // Колонка с производителем
            DataGridViewTextBoxColumn manufColumn = new DataGridViewTextBoxColumn
            {
                Name = "Manufacturer",
                HeaderText = "Производитель",
                Width = 150
            };
            dataGridViewProducts.Columns.Add(manufColumn);

            // Колонка с ценой
            DataGridViewTextBoxColumn priceColumn = new DataGridViewTextBoxColumn
            {
                Name = "Price",
                HeaderText = "Цена",
                Width = 100,
                DefaultCellStyle = new DataGridViewCellStyle
                {
                    Alignment = DataGridViewContentAlignment.MiddleRight,
                    Format = "N2"
                }
            };
            dataGridViewProducts.Columns.Add(priceColumn);

            // Колонка с наличием
            DataGridViewTextBoxColumn stockColumn = new DataGridViewTextBoxColumn
            {
                Name = "Stock",
                HeaderText = "Наличие",
                Width = 120,
                DefaultCellStyle = new DataGridViewCellStyle
                {
                    Alignment = DataGridViewContentAlignment.MiddleCenter
                }
            };
            dataGridViewProducts.Columns.Add(stockColumn);

            // Скрытая колонка с ID
            DataGridViewTextBoxColumn idColumn = new DataGridViewTextBoxColumn
            {
                Name = "Id",
                Visible = false
            };
            dataGridViewProducts.Columns.Add(idColumn);
        }

        private void LoadProducts()
        {
            try
            {
                var dt = db.GetAllProducts();
                products.Clear();
                dataGridViewProducts.Rows.Clear();

                Console.WriteLine($"Загружено товаров из базы: {dt.Rows.Count}");

                foreach (DataRow row in dt.Rows)
                {
                    Product product = new Product
                    {
                        Id = row["Id"] != DBNull.Value ? Convert.ToInt32(row["Id"]) : 0,
                        Name = row["Name"]?.ToString() ?? "Без названия",
                        Description = row["Description"]?.ToString() ?? "",
                        Manufacturer = row["Manufacturer"]?.ToString() ?? "Не указан",
                        Price = row["Price"] != DBNull.Value ? Convert.ToDecimal(row["Price"]) : 0,
                        InStock = row["InStock"] != DBNull.Value ? Convert.ToInt32(row["InStock"]) : 0
                    };

                    // Для отладки
                    Console.WriteLine($"Товар: ID={product.Id}, Name={product.Name}, InStock={product.InStock}");

                    products.Add(product);

                    // Получаем фото для этого товара
                    Image productImage = GetProductImage(product.Id, product.Name);

                    // Добавляем строку в DataGridView
                    int rowIndex = dataGridViewProducts.Rows.Add(
                        productImage, // Фото товара
                        product.Name, // Название
                        product.Description, // Описание
                        product.Manufacturer, // Производитель
                        product.Price.ToString("N2"), // Цена
                        product.InStock > 0 ? $"В наличии ({product.InStock} шт.)" : "Нет в наличии", // Наличие
                        product.Id // ID (скрыто)
                    );

                    // Серый фон для товаров, которых нет в наличии
                    if (product.InStock <= 0)
                    {
                        dataGridViewProducts.Rows[rowIndex].DefaultCellStyle.BackColor = Color.LightGray;
                        dataGridViewProducts.Rows[rowIndex].DefaultCellStyle.ForeColor = Color.DarkGray;
                    }
                }

                UpdateProductCount();
            }
            catch (Exception ex)
            {
                MessageBox.Show($"Ошибка загрузки товаров: {ex.Message}", "Ошибка",
                    MessageBoxButtons.OK, MessageBoxIcon.Error);
            }
        }

        // ВАЖНЫЙ МЕТОД: Поиск фото для товара
        private Image GetProductImage(int productId, string productName)
        {
            // Проверяем, существует ли папка с фото
            if (!Directory.Exists(photosFolder))
            {
                Console.WriteLine($"Папка для фото не существует: {photosFolder}");
                return defaultImage;
            }

            // Поддерживаемые форматы изображений
            string[] supportedExtensions = { ".jpg", ".jpeg", ".png", ".bmp", ".gif" };

            Console.WriteLine($"Поиск фото для товара ID={productId}, Name={productName}");

            // 1. Ищем фото по имени файла из базы данных
            try
            {
                string imageFileName = db.GetProductImageFileName(productId);
                if (!string.IsNullOrEmpty(imageFileName))
                {
                    string photoPath = Path.Combine(photosFolder, imageFileName);
                    if (File.Exists(photoPath))
                    {
                        try
                        {
                            Console.WriteLine($"Найдено фото по имени из базы: {imageFileName}");
                            return Image.FromFile(photoPath);
                        }
                        catch (Exception ex)
                        {
                            Console.WriteLine($"Ошибка загрузки фото {imageFileName}: {ex.Message}");
                        }
                    }
                }
            }
            catch
            {
                // Если нет информации в базе
            }

            // 2. Ищем фото по ID товара (например: "1.jpg", "2.png")
            foreach (string extension in supportedExtensions)
            {
                string photoPathByID = Path.Combine(photosFolder, $"{productId}{extension}");
                if (File.Exists(photoPathByID))
                {
                    try
                    {
                        Console.WriteLine($"Найдено фото по ID: {photoPathByID}");
                        return Image.FromFile(photoPathByID);
                    }
                    catch (Exception ex)
                    {
                        Console.WriteLine($"Ошибка загрузки фото {photoPathByID}: {ex.Message}");
                    }
                }
            }

            // 3. Ищем фото по названию товара (с временной меткой)
            string safeProductName = MakeSafeFileName(productName);

            // Ищем файлы, начинающиеся с названия товара
            var files = Directory.GetFiles(photosFolder, $"{safeProductName}_*.*")
                .Concat(Directory.GetFiles(photosFolder, $"{safeProductName}.*"))
                .Where(f => supportedExtensions.Contains(Path.GetExtension(f).ToLower()))
                .ToList();

            foreach (string file in files)
            {
                try
                {
                    Console.WriteLine($"Найдено фото по названию: {file}");
                    return Image.FromFile(file);
                }
                catch (Exception ex)
                {
                    Console.WriteLine($"Ошибка загрузки фото {file}: {ex.Message}");
                }
            }

            // 4. Проверяем, есть ли фото в виде байтов в базе данных
            try
            {
                byte[] imageData = db.GetProductImageBytes(productId);
                if (imageData != null && imageData.Length > 0)
                {
                    using (MemoryStream ms = new MemoryStream(imageData))
                    {
                        Console.WriteLine($"Найдено фото в базе данных (байты) для ID={productId}");
                        return Image.FromStream(ms);
                    }
                }
            }
            catch
            {
                // Если нет фото в базе
            }

            // 5. Если ничего не нашли, возвращаем заглушку
            Console.WriteLine($"Фото не найдено для товара ID={productId}, используем заглушку");
            return defaultImage;
        }

        private string MakeSafeFileName(string fileName)
        {
            if (string.IsNullOrEmpty(fileName))
                return "";

            // Убираем недопустимые символы
            char[] invalidChars = Path.GetInvalidFileNameChars();
            foreach (char invalidChar in invalidChars)
            {
                fileName = fileName.Replace(invalidChar.ToString(), "");
            }

            // Заменяем пробелы на подчеркивания
            fileName = fileName.Replace(" ", "_").ToLower();

            // Ограничиваем длину
            if (fileName.Length > 50)
                fileName = fileName.Substring(0, 50);

            return fileName;
        }

        private void LoadManufacturers()
        {
            try
            {
                manufacturers = products
                    .Select(p => p.Manufacturer)
                    .Where(m => !string.IsNullOrWhiteSpace(m))
                    .Distinct()
                    .OrderBy(m => m)
                    .ToList();

                filterComboBox.Items.Clear();
                filterComboBox.Items.Add("Все производители");
                filterComboBox.Items.AddRange(manufacturers.ToArray());
                filterComboBox.SelectedIndex = 0;
            }
            catch (Exception ex)
            {
                MessageBox.Show($"Ошибка загрузки производителей: {ex.Message}", "Ошибка",
                    MessageBoxButtons.OK, MessageBoxIcon.Error);
            }
        }

        private void ConfigureFilters()
        {
            sortComboBox.Items.Clear();
            sortComboBox.Items.Add("Цена: по возрастанию");
            sortComboBox.Items.Add("Цена: по убыванию");
            sortComboBox.SelectedIndex = 0;
        }

        private void AttachEventHandlers()
        {
            searchTextBox.TextChanged += SearchTextBox_TextChanged;
            filterComboBox.SelectedIndexChanged += FilterComboBox_SelectedIndexChanged;
            sortComboBox.SelectedIndexChanged += SortComboBox_SelectedIndexChanged;
            btnDeleteProduct.Click += BtnDeleteProduct_Click;
            btnAddProduct.Click += BtnAddProduct_Click;
            dataGridViewProducts.CellDoubleClick += DataGridViewProducts_CellDoubleClick;
            dataGridViewProducts.SelectionChanged += DataGridViewProducts_SelectionChanged;
            button1.Click += ButtonOrder_Click; // Обработчик для кнопки "Заказать"/"Войти для заказа"
        }

        private void ApplyFiltersAndSort()
        {
            var filteredProducts = products.AsEnumerable();

            // Поиск
            if (!string.IsNullOrWhiteSpace(searchTextBox.Text))
            {
                string searchTerm = searchTextBox.Text.ToLower();
                filteredProducts = filteredProducts.Where(p =>
                    (p.Name?.ToLower() ?? "").Contains(searchTerm) ||
                    (p.Description?.ToLower() ?? "").Contains(searchTerm) ||
                    (p.Manufacturer?.ToLower() ?? "").Contains(searchTerm));
            }

            // Фильтр по производителю
            if (filterComboBox.SelectedIndex > 0)
            {
                string selectedManufacturer = filterComboBox.SelectedItem.ToString();
                filteredProducts = filteredProducts.Where(p => p.Manufacturer == selectedManufacturer);
            }

            // Сортировка
            switch (sortComboBox.SelectedIndex)
            {
                case 0: // По возрастанию цены
                    filteredProducts = filteredProducts.OrderBy(p => p.Price);
                    break;
                case 1: // По убыванию цены
                    filteredProducts = filteredProducts.OrderByDescending(p => p.Price);
                    break;
            }

            UpdateDataGridView(filteredProducts.ToList());
        }

        private void UpdateDataGridView(List<Product> productsToDisplay)
        {
            dataGridViewProducts.Rows.Clear();

            foreach (var product in productsToDisplay)
            {
                Image productImage = GetProductImage(product.Id, product.Name);

                int rowIndex = dataGridViewProducts.Rows.Add(
                    productImage,
                    product.Name,
                    product.Description,
                    product.Manufacturer,
                    product.Price.ToString("N2"),
                    product.InStock > 0 ? $"В наличии ({product.InStock} шт.)" : "Нет в наличии",
                    product.Id
                );

                if (product.InStock <= 0)
                {
                    dataGridViewProducts.Rows[rowIndex].DefaultCellStyle.BackColor = Color.LightGray;
                    dataGridViewProducts.Rows[rowIndex].DefaultCellStyle.ForeColor = Color.DarkGray;
                }
            }

            UpdateProductCount();
        }

        private void UpdateProductCount()
        {
            lblProductCount.Text = $"Показано: {dataGridViewProducts.Rows.Count} из {products.Count}";
        }

        #region Обработчики событий

        private void SearchTextBox_TextChanged(object sender, EventArgs e)
        {
            ApplyFiltersAndSort();
        }

        private void FilterComboBox_SelectedIndexChanged(object sender, EventArgs e)
        {
            ApplyFiltersAndSort();
        }

        private void SortComboBox_SelectedIndexChanged(object sender, EventArgs e)
        {
            ApplyFiltersAndSort();
        }

        private void BtnAddProduct_Click(object sender, EventArgs e)
        {
            if (!isAdmin)
            {
                MessageBox.Show("Доступ запрещен. Только администратор может добавлять товары.",
                    "Ошибка доступа", MessageBoxButtons.OK, MessageBoxIcon.Error);
                return;
            }

            // Открываем форму добавления товара (Form4)
            try
            {
                using (Dobavlenie addForm = new Dobavlenie())
                {
                    var result = addForm.ShowDialog();

                    // После закрытия формы обновляем список
                    if (result == DialogResult.OK)
                    {
                        Console.WriteLine("Form4 закрыта с результатом OK, обновляем список товаров...");
                        LoadProducts();
                        LoadManufacturers();
                        // Сообщение не нужно - оно уже показано в Form4
                    }
                    else
                    {
                        Console.WriteLine("Form4 закрыта с результатом Cancel или другим");
                    }
                }
            }
            catch (Exception ex)
            {
                MessageBox.Show($"Ошибка открытия формы добавления товара: {ex.Message}", "Ошибка",
                    MessageBoxButtons.OK, MessageBoxIcon.Error);
            }
        }
        private void BtnDeleteProduct_Click(object sender, EventArgs e)
        {
            if (!isAdmin)
            {
                MessageBox.Show("Доступ запрещен. Только администратор может удалять товары.",
                    "Ошибка доступа", MessageBoxButtons.OK, MessageBoxIcon.Error);
                return;
            }

            if (dataGridViewProducts.SelectedRows.Count > 0)
            {
                int productId = Convert.ToInt32(dataGridViewProducts.SelectedRows[0].Cells["Id"].Value);
                string productName = dataGridViewProducts.SelectedRows[0].Cells["Name"].Value.ToString();

                DialogResult result = MessageBox.Show(
                    $"Вы уверены, что хотите удалить товар '{productName}'?",
                    "Подтверждение удаления",
                    MessageBoxButtons.YesNo,
                    MessageBoxIcon.Question);

                if (result == DialogResult.Yes)
                {
                    DeleteProduct(productId);
                }
            }
            else
            {
                MessageBox.Show("Выберите товар для удаления", "Информация",
                    MessageBoxButtons.OK, MessageBoxIcon.Information);
            }
        }

        // Обработчик для кнопки "Заказать"/"Войти для заказа"
        private void ButtonOrder_Click(object sender, EventArgs e)
        {
            // Если пользователь не авторизован (гость)
            if (!isLoggedInUser)
            {
                DialogResult result = MessageBox.Show(
                    "Для заказа товаров необходимо войти в систему!\n\n" +
                    "Хотите перейти к авторизации?",
                    "Требуется авторизация",
                    MessageBoxButtons.YesNo,
                    MessageBoxIcon.Information);

                if (result == DialogResult.Yes)
                {
                    // Закрываем Form3 и возвращаемся к авторизации
                    var form1 = new Avtoriz();
                    form1.Show();
                    this.Close();
                }
                return;
            }

            // Если пользователь авторизован, но не выбрал товар
            if (dataGridViewProducts.SelectedRows.Count == 0)
            {
                MessageBox.Show("Выберите товар для заказа!", "Информация",
                    MessageBoxButtons.OK, MessageBoxIcon.Information);
                return;
            }

            try
            {
                // Получаем информацию о выбранном товаре
                var selectedRow = dataGridViewProducts.SelectedRows[0];
                int productId = Convert.ToInt32(selectedRow.Cells["Id"].Value);
                string productName = selectedRow.Cells["Name"].Value.ToString();
                string manufacturer = selectedRow.Cells["Manufacturer"].Value.ToString();
                string price = selectedRow.Cells["Price"].Value.ToString();
                string stockText = selectedRow.Cells["Stock"].Value.ToString();

                // Парсим количество в наличии
                int inStock = 0;
                if (stockText.Contains("В наличии"))
                {
                    var match = System.Text.RegularExpressions.Regex.Match(stockText, @"\d+");
                    if (match.Success)
                        int.TryParse(match.Value, out inStock);
                }

                // Если товара нет в наличии
                if (inStock <= 0)
                {
                    MessageBox.Show("Этот товар временно отсутствует на складе", "Информация",
                        MessageBoxButtons.OK, MessageBoxIcon.Information);
                    return;
                }

                // Показываем диалог подтверждения
                DialogResult confirmResult = MessageBox.Show(
                    $"Вы уверены, что хотите заказать товар?\n\n" +
                    $"Наименование: {productName}\n" +
                    $"Производитель: {manufacturer}\n" +
                    $"Цена: {price} ₽\n" +
                    $"В наличии: {inStock} шт.",
                    "Подтверждение заказа",
                    MessageBoxButtons.YesNo,
                    MessageBoxIcon.Question);

                if (confirmResult == DialogResult.Yes)
                {
                    // Здесь можно добавить логику сохранения заказа в базу
                    // Например: db.CreateOrder(productId, userId, quantity);

                    // Показываем сообщение об успешном заказе
                    MessageBox.Show($"Товар \"{productName}\" успешно заказан!", "Успех",
                        MessageBoxButtons.OK, MessageBoxIcon.Information);
                }
            }
            catch (Exception ex)
            {
                MessageBox.Show($"Ошибка при оформлении заказа: {ex.Message}", "Ошибка",
                    MessageBoxButtons.OK, MessageBoxIcon.Error);
            }
        }

        private void DataGridViewProducts_CellDoubleClick(object sender, DataGridViewCellEventArgs e)
        {
            if (e.RowIndex >= 0 && e.ColumnIndex >= 0)
            {
                if (e.ColumnIndex == 0) // Колонка с фото
                {
                    ShowFullSizeImage(e.RowIndex);
                }
                else
                {
                    ShowProductInfo(e.RowIndex);
                }
            }
        }

        private void ShowFullSizeImage(int rowIndex)
        {
            try
            {
                var row = dataGridViewProducts.Rows[rowIndex];
                int productId = Convert.ToInt32(row.Cells["Id"].Value);
                string productName = row.Cells["Name"].Value.ToString();

                // Получаем изображение товара
                Image productImage = GetProductImage(productId, productName);

                if (productImage != null && productImage != defaultImage)
                {
                    // Создаем форму для просмотра фото
                    Form imageViewer = new Form
                    {
                        Text = $"Фото товара: {productName}",
                        Size = new Size(600, 600),
                        StartPosition = FormStartPosition.CenterScreen
                    };

                    PictureBox pictureBox = new PictureBox
                    {
                        Dock = DockStyle.Fill,
                        SizeMode = PictureBoxSizeMode.Zoom,
                        Image = productImage
                    };

                    imageViewer.Controls.Add(pictureBox);
                    imageViewer.ShowDialog();
                }
                else
                {
                    MessageBox.Show("Фотография для этого товара отсутствует", "Информация",
                        MessageBoxButtons.OK, MessageBoxIcon.Information);
                }
            }
            catch (Exception ex)
            {
                MessageBox.Show($"Ошибка при показе изображения: {ex.Message}", "Ошибка",
                    MessageBoxButtons.OK, MessageBoxIcon.Error);
            }
        }

        private void DataGridViewProducts_SelectionChanged(object sender, EventArgs e)
        {
            if (isAdmin)
            {
                bool hasSelection = dataGridViewProducts.SelectedRows.Count > 0;
                btnDeleteProduct.Enabled = hasSelection;
            }
            else
            {
                // Для обычных пользователей и гостей активируем кнопку при выборе товара
                bool hasSelection = dataGridViewProducts.SelectedRows.Count > 0;
                button1.Enabled = hasSelection;

                // Проверяем, есть ли товар в наличии (только для авторизованных)
                if (hasSelection && isLoggedInUser)
                {
                    try
                    {
                        var selectedRow = dataGridViewProducts.SelectedRows[0];
                        string stockText = selectedRow.Cells["Stock"].Value.ToString();
                        bool inStock = stockText.Contains("В наличии");
                        button1.Enabled = inStock;

                        // Меняем цвет кнопки в зависимости от наличия
                        if (inStock)
                        {
                            button1.BackColor = Color.ForestGreen;
                            button1.Text = "Заказать";
                        }
                        else
                        {
                            button1.BackColor = Color.Gray;
                            button1.Text = "Нет в наличии";
                            button1.Enabled = false;
                        }
                    }
                    catch
                    {
                        button1.Enabled = false;
                        button1.BackColor = Color.Gray;
                        button1.Text = "Заказать";
                    }
                }
                else if (hasSelection && !isLoggedInUser)
                {
                    // Для гостей кнопка всегда активна при выборе товара
                    button1.Enabled = true;
                    button1.BackColor = Color.Orange;
                    button1.Text = "Войти для заказа";
                }
                else
                {
                    // Нет выбранного товара
                    button1.Enabled = false;
                    button1.BackColor = isLoggedInUser ? Color.ForestGreen : Color.Orange;
                    button1.Text = isLoggedInUser ? "Заказать" : "Войти для заказа";
                }
            }
        }

        #endregion

        #region Вспомогательные методы

        private void DeleteProduct(int productId)
        {
            try
            {
                // Проверяем, есть ли товар в заказах
                if (db.IsProductInOrders(productId))
                {
                    MessageBox.Show("Нельзя удалить товар, который присутствует в заказе",
                        "Ошибка", MessageBoxButtons.OK, MessageBoxIcon.Error);
                    return;
                }

                // Удаляем товар
                bool success = db.DeleteProduct(productId);

                if (success)
                {
                    MessageBox.Show("Товар успешно удален", "Успех",
                        MessageBoxButtons.OK, MessageBoxIcon.Information);
                    LoadProducts();
                    LoadManufacturers();
                }
                else
                {
                    MessageBox.Show("Ошибка при удалении товара", "Ошибка",
                        MessageBoxButtons.OK, MessageBoxIcon.Error);
                }
            }
            catch (Exception ex)
            {
                MessageBox.Show($"Ошибка при удалении товара: {ex.Message}", "Ошибка",
                    MessageBoxButtons.OK, MessageBoxIcon.Error);
            }
        }

        private void ShowProductInfo(int rowIndex)
        {
            try
            {
                var row = dataGridViewProducts.Rows[rowIndex];
                int productId = Convert.ToInt32(row.Cells["Id"].Value);
                string productName = row.Cells["Name"].Value.ToString();
                string manufacturer = row.Cells["Manufacturer"].Value.ToString();
                string price = row.Cells["Price"].Value.ToString();
                string stock = row.Cells["Stock"].Value.ToString();
                string description = row.Cells["Description"].Value.ToString();

                string message = $"Наименование: {productName}\n" +
                               $"Производитель: {manufacturer}\n" +
                               $"Цена: {price} ₽\n" +
                               $"Наличие: {stock}\n" +
                               $"ID товара: {productId}\n\n" +
                               $"Описание:\n{description}";

                MessageBox.Show(message, "Информация о товаре",
                    MessageBoxButtons.OK, MessageBoxIcon.Information);
            }
            catch (Exception ex)
            {
                MessageBox.Show($"Ошибка при показе информации: {ex.Message}", "Ошибка",
                    MessageBoxButtons.OK, MessageBoxIcon.Error);
            }
        }

        #endregion

        #region Класс модели товара

        private class Product
        {
            public int Id { get; set; }
            public string Name { get; set; }
            public string Description { get; set; }
            public string Manufacturer { get; set; }
            public decimal Price { get; set; }
            public int InStock { get; set; }
        }

        #endregion

        #region Обработчики событий формы

        private void Form3_Load(object sender, EventArgs e)
        {
            dataGridViewProducts.ClearSelection();

            if (isAdmin)
            {
                btnDeleteProduct.Enabled = false;
                button1.Visible = false;
            }
            else
            {
                btnAddProduct.Visible = false;
                btnDeleteProduct.Visible = false;
                button1.Enabled = false;

                // Устанавливаем правильный цвет кнопки при загрузке
                if (isLoggedInUser)
                {
                    button1.BackColor = Color.ForestGreen;
                    button1.Text = "Заказать";
                }
                else
                {
                    button1.BackColor = Color.Orange;
                    button1.Text = "Войти для заказа";
                }
            }
        }

        private void Form3_FormClosing(object sender, FormClosingEventArgs e)
        {
            if (defaultImage != null)
            {
                defaultImage.Dispose();
                defaultImage = null;
            }
        }

        #endregion

        private void lblProductCount_Click(object sender, EventArgs e)
        {
            // Обработчик события для клика по метке
        }

        private void btnAddProduct_Click_1(object sender, EventArgs e)
        {
            // Дублирующий обработчик - оставлен для совместимости
            BtnAddProduct_Click(sender, e);
        }

        private void button1_Click(object sender, EventArgs e)
        {
            // Этот метод теперь будет вызывать ButtonOrder_Click
            // но лучше использовать ButtonOrder_Click напрямую
        }

        private void btnDeleteProduct_Click_1(object sender, EventArgs e)
        {

        }
    }
}