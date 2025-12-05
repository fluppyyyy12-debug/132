using Microsoft.Data.SqlClient;
using System.Configuration;
using System.Data;
using System.Security.Cryptography;
using System.Text;
using System.Windows.Forms;

namespace WindowsFormsApp1
{
    internal class DatabaseHelper
    {
        private readonly string connectionString;

        public DatabaseHelper()
        {
            try
            {
                var settings = ConfigurationManager.ConnectionStrings["ClubDbConnection"];
                connectionString = settings?.ConnectionString ??
                    @"Data Source=507-8\SQLEXP;Initial Catalog=posuda;Integrated Security=True;TrustServerCertificate=True;Connect Timeout=30";

                TestConnection();
            }
            catch (Exception ex)
            {
                MessageBox.Show($"Ошибка инициализации БД: {ex.Message}", "Ошибка");
                connectionString = @"Data Source=507-8\SQLEXP;Initial Catalog=posuda;Integrated Security=True;TrustServerCertificate=True;Connect Timeout=30";
            }
        }

        private void TestConnection()
        {
            try
            {
                using (var connection = new SqlConnection(connectionString))
                {
                    connection.Open();
                    Console.WriteLine("Подключение к базе данных успешно установлено");
                }
            }
            catch (Exception ex)
            {
                MessageBox.Show($"Не удалось подключиться к БД:\n{ex.Message}", "Ошибка подключения");
                throw;
            }
        }

        private string HashPassword(string password)
        {
            using (SHA256 sha256 = SHA256.Create())
            {
                byte[] bytes = sha256.ComputeHash(Encoding.UTF8.GetBytes(password));
                StringBuilder builder = new StringBuilder();
                foreach (byte b in bytes)
                {
                    builder.Append(b.ToString("x2"));
                }
                return builder.ToString();
            }
        }

        // РЕГИСТРАЦИЯ пользователя
        public bool RegisterUser(string username, string password)
        {
            try
            {
                string hashedPassword = HashPassword(password);

                using (SqlConnection connection = new SqlConnection(connectionString))
                {
                    connection.Open();

                    string checkQuery = "SELECT COUNT(*) FROM Users WHERE Username = @username";
                    using (SqlCommand checkCmd = new SqlCommand(checkQuery, connection))
                    {
                        checkCmd.Parameters.AddWithValue("@username", username);
                        int userCount = (int)checkCmd.ExecuteScalar();

                        if (userCount > 0)
                        {
                            MessageBox.Show("Пользователь с таким именем уже существует!", "Ошибка");
                            return false;
                        }
                    }

                    string insertQuery = "INSERT INTO Users (Username, Password) VALUES (@username, @password)";
                    using (SqlCommand insertCmd = new SqlCommand(insertQuery, connection))
                    {
                        insertCmd.Parameters.AddWithValue("@username", username);
                        insertCmd.Parameters.AddWithValue("@password", hashedPassword);
                        int rowsAffected = insertCmd.ExecuteNonQuery();
                        return rowsAffected > 0;
                    }
                }
            }
            catch (Exception ex)
            {
                MessageBox.Show($"Ошибка при регистрации: {ex.Message}", "Ошибка");
                return false;
            }
        }

        // ВАЛИДАЦИЯ пользователя
        public bool ValidateUser(string username, string password)
        {
            try
            {
                string hashedPassword = HashPassword(password);

                using (SqlConnection connection = new SqlConnection(connectionString))
                {
                    string query = "SELECT COUNT(1) FROM Users WHERE Username = @username AND Password = @password";

                    using (SqlCommand command = new SqlCommand(query, connection))
                    {
                        command.Parameters.AddWithValue("@username", username);
                        command.Parameters.AddWithValue("@password", hashedPassword);

                        connection.Open();
                        int count = Convert.ToInt32(command.ExecuteScalar());
                        return count > 0;
                    }
                }
            }
            catch (Exception ex)
            {
                MessageBox.Show($"Ошибка входа: {ex.Message}", "Ошибка");
                return false;
            }
        }

        // ============== МЕТОДЫ ДЛЯ ТОВАРОВ ==============

        // Получить все товары
        public DataTable GetAllProducts()
        {
            DataTable dataTable = new DataTable();
            try
            {
                using (SqlConnection connection = new SqlConnection(connectionString))
                {
                    // Улучшенный запрос - добавляем логирование
                    string query = @"
                        SELECT 
                            Id, 
                            ISNULL(Name, 'Без названия') as Name, 
                            ISNULL(Description, '') as Description, 
                            ISNULL(Manufacturer, 'Не указан') as Manufacturer, 
                            ISNULL(Price, 0) as Price, 
                            ISNULL(InStock, 0) as InStock,
                            ISNULL(Category, '') as Category,
                            ISNULL(MinStock, 0) as MinStock,
                            ISNULL(Unit, '') as Unit,
                            ISNULL(Supplier, '') as Supplier,
                            ISNULL(ImagePath, '') as ImagePath
                        FROM Products 
                        ORDER BY Id DESC";

                    Console.WriteLine($"Выполняем запрос получения всех товаров...");

                    using (SqlCommand command = new SqlCommand(query, connection))
                    {
                        connection.Open();
                        using (SqlDataAdapter adapter = new SqlDataAdapter(command))
                        {
                            adapter.Fill(dataTable);
                        }
                    }

                    Console.WriteLine($"Получено {dataTable.Rows.Count} товаров из базы данных");
                }
            }
            catch (Exception ex)
            {
                Console.WriteLine($"Ошибка загрузки товаров: {ex.Message}");
                MessageBox.Show($"Ошибка загрузки товаров: {ex.Message}", "Ошибка");
            }
            return dataTable;
        }

        // Получить категории - возвращаем как "Name"
        public DataTable GetCategories()
        {
            DataTable dataTable = new DataTable();
            try
            {
                using (SqlConnection connection = new SqlConnection(connectionString))
                {
                    string query = "SELECT DISTINCT Category as Name FROM Products WHERE Category IS NOT NULL AND Category != '' ORDER BY Category";

                    Console.WriteLine("Загружаем категории...");

                    using (SqlCommand command = new SqlCommand(query, connection))
                    {
                        connection.Open();
                        using (SqlDataAdapter adapter = new SqlDataAdapter(command))
                        {
                            adapter.Fill(dataTable);
                        }
                    }

                    Console.WriteLine($"Загружено {dataTable.Rows.Count} категорий");
                }
            }
            catch (Exception ex)
            {
                Console.WriteLine($"Ошибка загрузки категорий: {ex.Message}");
                MessageBox.Show($"Ошибка загрузки категорий: {ex.Message}", "Ошибка");
            }
            return dataTable;
        }

        // Получить поставщиков - возвращаем как "Name"
        public DataTable GetSuppliers()
        {
            DataTable dataTable = new DataTable();
            try
            {
                using (SqlConnection connection = new SqlConnection(connectionString))
                {
                    string query = "SELECT DISTINCT Supplier as Name FROM Products WHERE Supplier IS NOT NULL AND Supplier != '' ORDER BY Supplier";

                    Console.WriteLine("Загружаем поставщиков...");

                    using (SqlCommand command = new SqlCommand(query, connection))
                    {
                        connection.Open();
                        using (SqlDataAdapter adapter = new SqlDataAdapter(command))
                        {
                            adapter.Fill(dataTable);
                        }
                    }

                    Console.WriteLine($"Загружено {dataTable.Rows.Count} поставщиков");
                }
            }
            catch (Exception ex)
            {
                Console.WriteLine($"Ошибка загрузки поставщиков: {ex.Message}");
                MessageBox.Show($"Ошибка загрузки поставщиков: {ex.Message}", "Ошибка");
            }
            return dataTable;
        }

        // Получить единицы измерения - возвращаем как "Name"
        public DataTable GetUnits()
        {
            DataTable dataTable = new DataTable();
            try
            {
                using (SqlConnection connection = new SqlConnection(connectionString))
                {
                    string query = "SELECT DISTINCT Unit as Name FROM Products WHERE Unit IS NOT NULL AND Unit != '' ORDER BY Unit";

                    Console.WriteLine("Загружаем единицы измерения...");

                    using (SqlCommand command = new SqlCommand(query, connection))
                    {
                        connection.Open();
                        using (SqlDataAdapter adapter = new SqlDataAdapter(command))
                        {
                            adapter.Fill(dataTable);
                        }
                    }

                    Console.WriteLine($"Загружено {dataTable.Rows.Count} единиц измерения");
                }
            }
            catch (Exception ex)
            {
                Console.WriteLine($"Ошибка загрузки единиц измерения: {ex.Message}");
                MessageBox.Show($"Ошибка загрузки единиц измерения: {ex.Message}", "Ошибка");
            }
            return dataTable;
        }

        // Получить следующий ID товара
        public int GetNextProductId()
        {
            try
            {
                using (SqlConnection connection = new SqlConnection(connectionString))
                {
                    string query = "SELECT ISNULL(MAX(Id), 0) + 1 FROM Products";
                    using (SqlCommand command = new SqlCommand(query, connection))
                    {
                        connection.Open();
                        object result = command.ExecuteScalar();
                        int nextId = Convert.ToInt32(result);
                        Console.WriteLine($"Следующий ID товара: {nextId}");
                        return nextId;
                    }
                }
            }
            catch (Exception ex)
            {
                Console.WriteLine($"Ошибка получения следующего ID: {ex.Message}");
                return 1;
            }
        }

        // Добавить новый товар - улучшенная версия с отладкой
        public bool AddProduct(string name, string description, string category, string manufacturer,
                               decimal price, int stock, int minStock, string unit, string supplier,
                               string imagePath)
        {
            try
            {
                // Логируем параметры для отладки
                Console.WriteLine($"Добавление товара:");
                Console.WriteLine($"  Name: {name}");
                Console.WriteLine($"  Category: {category}");
                Console.WriteLine($"  Manufacturer: {manufacturer}");
                Console.WriteLine($"  Price: {price}");
                Console.WriteLine($"  Stock: {stock}");
                Console.WriteLine($"  Unit: {unit}");
                Console.WriteLine($"  Supplier: {supplier}");
                Console.WriteLine($"  ImagePath: {imagePath ?? "null"}");

                using (SqlConnection connection = new SqlConnection(connectionString))
                {
                    string query = @"
                        INSERT INTO Products 
                        (Name, Description, Category, Manufacturer, Price, InStock, MinStock, Unit, Supplier, ImagePath, CreatedDate) 
                        VALUES 
                        (@Name, @Description, @Category, @Manufacturer, @Price, @Stock, @MinStock, @Unit, @Supplier, @ImagePath, GETDATE())";

                    using (SqlCommand command = new SqlCommand(query, connection))
                    {
                        command.Parameters.AddWithValue("@Name", name ?? (object)DBNull.Value);
                        command.Parameters.AddWithValue("@Description", description ?? (object)DBNull.Value);
                        command.Parameters.AddWithValue("@Category", category ?? (object)DBNull.Value);
                        command.Parameters.AddWithValue("@Manufacturer", manufacturer ?? (object)DBNull.Value);
                        command.Parameters.AddWithValue("@Price", price);
                        command.Parameters.AddWithValue("@Stock", stock);
                        command.Parameters.AddWithValue("@MinStock", minStock);
                        command.Parameters.AddWithValue("@Unit", unit ?? (object)DBNull.Value);
                        command.Parameters.AddWithValue("@Supplier", supplier ?? (object)DBNull.Value);
                        command.Parameters.AddWithValue("@ImagePath", imagePath ?? (object)DBNull.Value);

                        connection.Open();
                        int rowsAffected = command.ExecuteNonQuery();

                        bool success = rowsAffected > 0;
                        Console.WriteLine($"Товар добавлен: {success}, затронуто строк: {rowsAffected}");

                        return success;
                    }
                }
            }
            catch (Exception ex)
            {
                Console.WriteLine($"Ошибка добавления товара: {ex.Message}");
                Console.WriteLine($"StackTrace: {ex.StackTrace}");
                MessageBox.Show($"Ошибка добавления товара:\n{ex.Message}", "Ошибка");
                return false;
            }
        }

        // Получить изображение товара по ID в виде байтов
        public byte[] GetProductImageBytes(int productId)
        {
            try
            {
                using (SqlConnection connection = new SqlConnection(connectionString))
                {
                    string query = "SELECT Image FROM Products WHERE Id = @ProductId";

                    using (SqlCommand command = new SqlCommand(query, connection))
                    {
                        command.Parameters.AddWithValue("@ProductId", productId);
                        connection.Open();

                        object result = command.ExecuteScalar();

                        if (result != null && result != DBNull.Value)
                        {
                            Console.WriteLine($"Найдено изображение в виде байтов для товара ID {productId}");
                            return (byte[])result;
                        }
                        else
                        {
                            Console.WriteLine($"Изображение не найдено в базе для товара ID {productId}");
                        }
                    }
                }
            }
            catch (Exception ex)
            {
                Console.WriteLine($"Ошибка получения изображения товара ID {productId}: {ex.Message}");
            }

            return null;
        }

        // Получить имя файла изображения товара по ID
        public string GetProductImageFileName(int productId)
        {
            try
            {
                using (SqlConnection connection = new SqlConnection(connectionString))
                {
                    string query = "SELECT ImagePath FROM Products WHERE Id = @ProductId";
                    using (SqlCommand command = new SqlCommand(query, connection))
                    {
                        command.Parameters.AddWithValue("@ProductId", productId);
                        connection.Open();
                        var result = command.ExecuteScalar();

                        if (result != null && result != DBNull.Value)
                        {
                            string fileName = result.ToString();
                            Console.WriteLine($"Найдено имя файла изображения для товара ID {productId}: {fileName}");
                            return fileName;
                        }
                        else
                        {
                            Console.WriteLine($"Имя файла изображения не найдено для товара ID {productId}");
                        }
                    }
                }
            }
            catch (Exception ex)
            {
                Console.WriteLine($"Ошибка получения имени файла изображения: {ex.Message}");
            }
            return null;
        }

        // Проверить, есть ли товар в заказах
        public bool IsProductInOrders(int productId)
        {
            try
            {
                using (SqlConnection connection = new SqlConnection(connectionString))
                {
                    string query = @"
                        SELECT COUNT(*) 
                        FROM OrderDetails od 
                        INNER JOIN Orders o ON od.OrderId = o.Id 
                        WHERE od.ProductId = @ProductId";

                    using (SqlCommand command = new SqlCommand(query, connection))
                    {
                        command.Parameters.AddWithValue("@ProductId", productId);
                        connection.Open();
                        int count = Convert.ToInt32(command.ExecuteScalar());
                        bool exists = count > 0;
                        Console.WriteLine($"Товар ID {productId} в заказах: {exists}");
                        return exists;
                    }
                }
            }
            catch (Exception ex)
            {
                Console.WriteLine($"Ошибка проверки заказов для товара {productId}: {ex.Message}");
                return false;
            }
        }

        // Удалить товар
        public bool DeleteProduct(int productId)
        {
            try
            {
                using (SqlConnection connection = new SqlConnection(connectionString))
                {
                    string query = "DELETE FROM Products WHERE Id = @ProductId";
                    using (SqlCommand command = new SqlCommand(query, connection))
                    {
                        command.Parameters.AddWithValue("@ProductId", productId);
                        connection.Open();
                        int rowsAffected = command.ExecuteNonQuery();
                        bool success = rowsAffected > 0;
                        Console.WriteLine($"Удаление товара ID {productId}: {success}, затронуто строк: {rowsAffected}");
                        return success;
                    }
                }
            }
            catch (Exception ex)
            {
                Console.WriteLine($"Ошибка удаления товара ID {productId}: {ex.Message}");
                MessageBox.Show($"Ошибка удаления товара: {ex.Message}", "Ошибка");
                return false;
            }
        }

        // Получить товар по ID
        public DataRow GetProductById(int productId)
        {
            try
            {
                using (SqlConnection connection = new SqlConnection(connectionString))
                {
                    string query = @"
                        SELECT 
                            Id, 
                            Name, 
                            ISNULL(Description, '') as Description, 
                            ISNULL(Manufacturer, '') as Manufacturer, 
                            ISNULL(Price, 0) as Price, 
                            ISNULL(InStock, 0) as InStock,
                            ISNULL(Category, '') as Category,
                            ISNULL(MinStock, 0) as MinStock,
                            ISNULL(Unit, '') as Unit,
                            ISNULL(Supplier, '') as Supplier,
                            ISNULL(ImagePath, '') as ImagePath
                        FROM Products 
                        WHERE Id = @ProductId";

                    using (SqlCommand command = new SqlCommand(query, connection))
                    {
                        command.Parameters.AddWithValue("@ProductId", productId);

                        DataTable dataTable = new DataTable();
                        using (SqlDataAdapter adapter = new SqlDataAdapter(command))
                        {
                            adapter.Fill(dataTable);
                        }

                        if (dataTable.Rows.Count > 0)
                        {
                            Console.WriteLine($"Товар ID {productId} найден в базе");
                            return dataTable.Rows[0];
                        }
                        else
                        {
                            Console.WriteLine($"Товар ID {productId} не найден в базе");
                            return null;
                        }
                    }
                }
            }
            catch (Exception ex)
            {
                Console.WriteLine($"Ошибка загрузки товара ID {productId}: {ex.Message}");
                MessageBox.Show($"Ошибка загрузки товара: {ex.Message}", "Ошибка");
                return null;
            }
        }

        // Обновить товар
        public bool UpdateProduct(int id, string name, string description, string category,
                                  string manufacturer, decimal price, int stock, int minStock,
                                  string unit, string supplier, string imagePath)
        {
            try
            {
                Console.WriteLine($"Обновление товара ID {id}:");
                Console.WriteLine($"  Name: {name}");
                Console.WriteLine($"  Category: {category}");
                Console.WriteLine($"  Price: {price}");
                Console.WriteLine($"  ImagePath: {imagePath ?? "null"}");

                using (SqlConnection connection = new SqlConnection(connectionString))
                {
                    string query = @"
                        UPDATE Products 
                        SET Name = @Name, 
                            Description = @Description, 
                            Category = @Category, 
                            Manufacturer = @Manufacturer, 
                            Price = @Price, 
                            InStock = @Stock, 
                            MinStock = @MinStock, 
                            Unit = @Unit, 
                            Supplier = @Supplier, 
                            ImagePath = @ImagePath
                        WHERE Id = @Id";

                    using (SqlCommand command = new SqlCommand(query, connection))
                    {
                        command.Parameters.AddWithValue("@Id", id);
                        command.Parameters.AddWithValue("@Name", name ?? (object)DBNull.Value);
                        command.Parameters.AddWithValue("@Description", description ?? (object)DBNull.Value);
                        command.Parameters.AddWithValue("@Category", category ?? (object)DBNull.Value);
                        command.Parameters.AddWithValue("@Manufacturer", manufacturer ?? (object)DBNull.Value);
                        command.Parameters.AddWithValue("@Price", price);
                        command.Parameters.AddWithValue("@Stock", stock);
                        command.Parameters.AddWithValue("@MinStock", minStock);
                        command.Parameters.AddWithValue("@Unit", unit ?? (object)DBNull.Value);
                        command.Parameters.AddWithValue("@Supplier", supplier ?? (object)DBNull.Value);
                        command.Parameters.AddWithValue("@ImagePath", imagePath ?? (object)DBNull.Value);

                        connection.Open();
                        int rowsAffected = command.ExecuteNonQuery();

                        bool success = rowsAffected > 0;
                        Console.WriteLine($"Товар ID {id} обновлен: {success}, затронуто строк: {rowsAffected}");

                        return success;
                    }
                }
            }
            catch (Exception ex)
            {
                Console.WriteLine($"Ошибка обновления товара ID {id}: {ex.Message}");
                MessageBox.Show($"Ошибка обновления товара: {ex.Message}", "Ошибка");
                return false;
            }
        }

        // Поиск товаров
        public DataTable SearchProducts(string searchTerm)
        {
            DataTable dataTable = new DataTable();
            try
            {
                using (SqlConnection connection = new SqlConnection(connectionString))
                {
                    string query = @"
                        SELECT 
                            Id, 
                            ISNULL(Name, '') as Name, 
                            ISNULL(Description, '') as Description, 
                            ISNULL(Manufacturer, '') as Manufacturer, 
                            ISNULL(Price, 0) as Price, 
                            ISNULL(InStock, 0) as InStock,
                            ISNULL(Category, '') as Category,
                            ISNULL(MinStock, 0) as MinStock,
                            ISNULL(Unit, '') as Unit,
                            ISNULL(Supplier, '') as Supplier,
                            ISNULL(ImagePath, '') as ImagePath
                        FROM Products 
                        WHERE Name LIKE @SearchTerm 
                           OR Description LIKE @SearchTerm 
                           OR Manufacturer LIKE @SearchTerm
                           OR Category LIKE @SearchTerm
                        ORDER BY Id DESC";

                    using (SqlCommand command = new SqlCommand(query, connection))
                    {
                        command.Parameters.AddWithValue("@SearchTerm", $"%{searchTerm}%");
                        connection.Open();
                        using (SqlDataAdapter adapter = new SqlDataAdapter(command))
                        {
                            adapter.Fill(dataTable);
                        }
                    }

                    Console.WriteLine($"Найдено {dataTable.Rows.Count} товаров по запросу '{searchTerm}'");
                }
            }
            catch (Exception ex)
            {
                Console.WriteLine($"Ошибка поиска: {ex.Message}");
                MessageBox.Show($"Ошибка поиска: {ex.Message}", "Ошибка");
            }
            return dataTable;
        }

        // Вспомогательный метод для выполнения запроса
        private DataTable ExecuteQuery(string query, params SqlParameter[] parameters)
        {
            DataTable dataTable = new DataTable();
            try
            {
                using (SqlConnection connection = new SqlConnection(connectionString))
                {
                    using (SqlCommand command = new SqlCommand(query, connection))
                    {
                        if (parameters != null)
                        {
                            command.Parameters.AddRange(parameters);
                        }
                        connection.Open();
                        using (SqlDataAdapter adapter = new SqlDataAdapter(command))
                        {
                            adapter.Fill(dataTable);
                        }
                    }
                }
            }
            catch (Exception ex)
            {
                throw new Exception($"Ошибка выполнения запроса: {ex.Message}", ex);
            }
            return dataTable;
        }

        // Вспомогательный метод для выполнения scalar запроса
        private object ExecuteScalar(string query, params SqlParameter[] parameters)
        {
            try
            {
                using (SqlConnection connection = new SqlConnection(connectionString))
                {
                    using (SqlCommand command = new SqlCommand(query, connection))
                    {
                        if (parameters != null)
                        {
                            command.Parameters.AddRange(parameters);
                        }
                        connection.Open();
                        return command.ExecuteScalar();
                    }
                }
            }
            catch (Exception ex)
            {
                throw new Exception($"Ошибка выполнения запроса: {ex.Message}", ex);
            }
        }

        // Вспомогательный метод для выполнения non-query
        private int ExecuteNonQuery(string query, params SqlParameter[] parameters)
        {
            try
            {
                using (SqlConnection connection = new SqlConnection(connectionString))
                {
                    using (SqlCommand command = new SqlCommand(query, connection))
                    {
                        if (parameters != null)
                        {
                            command.Parameters.AddRange(parameters);
                        }
                        connection.Open();
                        return command.ExecuteNonQuery();
                    }
                }
            }
            catch (Exception ex)
            {
                throw new Exception($"Ошибка выполнения запроса: {ex.Message}", ex);
            }
        }
    }
}