using Microsoft.Data.SqlClient;
using Microsoft.EntityFrameworkCore;
using System.ComponentModel.DataAnnotations.Schema;
using System.Data;
using System.Reflection;


// Управление соединениями.
class ConnectionManager
{
    private readonly string _connectionString;

    public ConnectionManager(string connectionString)
    {
        _connectionString = connectionString;
    }

    // Использование Connection Pool.
    public async Task ExecuteWithConnectionAsync(Func<SqlConnection, Task> action)
    {
        using (var connection = new SqlConnection(_connectionString))
        {
            await connection.OpenAsync();
            await action(connection);
        }
    }

    // Получение статистики соединения.
    public void PrintConnectionStatistics()
    {
        var pool = new SqlConnectionStringBuilder(_connectionString);
        Console.WriteLine($"Pooling: {pool.Pooling}");
        Console.WriteLine($"Max Pool Size: {pool.MaxPoolSize}");
        Console.WriteLine($"Min Pool Size: {pool.MinPoolSize}");
    }
}

// Выполнение входа.
class CommandExecutor
{
    private readonly string _connectionString;

    public CommandExecutor(string connectionString)
    {
        _connectionString = connectionString;
    }

    public async Task<int> ExecuteNonQueryAsync(string sql, params SqlParameter[] parameters)
    {
        using (var connection = new SqlConnection(_connectionString))
        using (var command = new SqlCommand(sql, connection))
        {
            command.Parameters.AddRange(parameters);
            await connection.OpenAsync();
            return await command.ExecuteNonQueryAsync();
        }
    }

    public async Task<object> ExecuteScalarAsync(string sql, params SqlParameter[] parameters)
    {
        using (var connection = new SqlConnection(_connectionString))
        using (var command = new SqlCommand(sql, connection))
        {
            command.Parameters.AddRange(parameters);
            await connection.OpenAsync();
            return await command.ExecuteScalarAsync();
        }
    }

    // Вызов хранимых процедур.
    public async Task CallStoredProcedureAsync(string procedureName, params SqlParameter[] parameters)
    {
        using (var connection = new SqlConnection(_connectionString))
        using (var command = new SqlCommand(procedureName, connection))
        {
            command.CommandType = CommandType.StoredProcedure;
            command.Parameters.AddRange(parameters);
            await connection.OpenAsync();
            await command.ExecuteNonQueryAsync();
        }
    }
}

// Работа с транзакциями.
class TransactionService
{
    private readonly string _connectionString;

    public TransactionService(string connectionString)
    {
        _connectionString = connectionString;
    }

    public async Task ExecuteTransactionAsync(params Func<SqlTransaction, Task>[] operations)
    {
        using (var connection = new SqlConnection(_connectionString))
        {
            await connection.OpenAsync();
            using (var transaction = connection.BeginTransaction(IsolationLevel.ReadCommitted))
            {
                try
                {
                    foreach (var operation in operations)
                    {
                        await operation(transaction);
                    }
                    await transaction.CommitAsync();
                }
                catch
                {
                    await transaction.RollbackAsync();
                    throw;
                }
            }
        }
    }
}

// Конфигурация DbContext.
class ApplicationDbContext : DbContext
{
    private readonly string _connectionString;

    public ApplicationDbContext(string connectionString)
    {
        _connectionString = connectionString;
    }

    public ApplicationDbContext(DbContextOptions<ApplicationDbContext> options) : base(options)
    {
        // Отключаем отслеживание для запросов только для чтения.
        ChangeTracker.QueryTrackingBehavior = QueryTrackingBehavior.NoTracking;
    }

    public DbSet<Client> Clients {  get; set; }
    public DbSet<Product> Products { get; set; }

    protected override void OnModelCreating(ModelBuilder modelBuilder)
    {
        //Настройка через Fluent API.
        modelBuilder.ApplyConfigurationsFromAssembly(Assembly.GetExecutingAssembly());

        base.OnModelCreating(modelBuilder);
    }

    protected override void OnConfiguring(DbContextOptionsBuilder optionsBuilder)
    {
        if (!optionsBuilder.IsConfigured)
        {
            optionsBuilder.UseSqlServer(_connectionString, sqlOptions =>
                sqlOptions.EnableRetryOnFailure(
                    maxRetryCount: 5,
                    maxRetryDelay: TimeSpan.FromSeconds(30),
                    errorNumbersToAdd: null));
        }
    }
}
// Таблица Client.
[Table("Client")]
class Client
{
    public Guid ClientID { get; set; }
    public string Name { get; set; }
    public string Phone { get; set; }
    public string Email { get; set; }
    public DateTime RegistrationDate { get; set; }
    public bool IsActive { get; set; }
}

// Таблица Product.
[Table("Product")]
class Product
{
    public int ProductID { get; set; }
    public string Name { get; set; }
    public decimal Price { get; set; }
    public int StockQuantity { get; set; }
}

// Демонстрация работы.
class AutoPartsStoreApp
{
    static async Task Main(string[] args)  
    {
        string connectionString = "Server=.\\SQLEXPRESS;Database=AutoPartsStore;Trusted_Connection=True;TrustServerCertificate=True;";

        // ADO.NET.
        Console.WriteLine(" ADO.NET (ConnectionManager)");

        var connectionManager = new ConnectionManager(connectionString);  
        var commandExecutor = new CommandExecutor(connectionString);  
        var transactionService = new TransactionService(connectionString);

        // Вывод таблицы клиентов через ConnectionManager.
        await connectionManager.ExecuteWithConnectionAsync(async (connection) =>  
        {
            const string sql = "SELECT TOP 5 Name, Phone, Email, RegistrationDate, IsActive FROM Client ORDER BY Name";

            using (var command = new SqlCommand(sql, connection))
            using (var reader = await command.ExecuteReaderAsync())  
            {
                Console.WriteLine("Список первых 5 клиентов");

                while (await reader.ReadAsync())
                {
                    string name = reader.GetString(0);
                    string phone = reader.GetString(1);
                    string email = reader.GetString(2);
                    DateTime regDate = reader.GetDateTime(3);
                    bool isActive = reader.GetBoolean(4);

                    Console.WriteLine($"Имя: {name} Тел: {phone} Email: {email} Регистрация:{regDate:dd.MM.yyyy} Активен: {isActive}");  
                }
            }
        });

        // Выполнение запросов через CommandExecutor.
        Console.WriteLine("\n ADO.NET (CommandExecutor)");
        string countSql = "SELECT COUNT(*) FROM Client";
        var count = await commandExecutor.ExecuteScalarAsync(countSql); 
        Console.WriteLine($"Всего клиентов: {count}");

        string productsCountSql = "SELECT COUNT(*) FROM Products";
        var productsCount = await commandExecutor.ExecuteScalarAsync(productsCountSql);          
        Console.WriteLine($"Всего продуктов: {productsCount}");

        // Тест транзакции.
        Console.WriteLine("\n Тест транзакции:");
        await transactionService.ExecuteTransactionAsync(async (transaction) =>
        {
            string sql = "SELECT COUNT(*) FROM Client WHERE IsActive = 1";
            using (var command = new SqlCommand(sql, transaction.Connection, transaction))
            {
                int count_active = (int)await command.ExecuteScalarAsync();
                Console.WriteLine($"В транзакции: активных клиентов {count_active}");
            }
        });

        // Entity Framework.
        using (var dbContext = new ApplicationDbContext(connectionString)) 
        {
            try
            {
                var canConnect = await dbContext.Database.CanConnectAsync();
                Console.WriteLine($"Статус подключения: {canConnect}");
                if (canConnect)
                {
                    // Получение данных через EF.
                    var clients = await dbContext.Clients
                        .Where(c => c.IsActive == true)
                        .OrderBy(c => c.Name)
                        .Take(10)
                        .ToListAsync();
                    Console.WriteLine("\n Активные клиенты (EF):");
                    foreach (var client in clients)
                    {
                        Console.WriteLine($"{client.Name}  Тел: {client.Phone} Email: {client.Email}");
                    }
                }
            }
            catch (Exception ex)
            {
                Console.WriteLine($" Ошибка EF: {ex.Message}");
            }
        }
    }
}
