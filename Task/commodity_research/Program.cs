public class Product
{
    private string nаme; // Наименование
    private string manufacturer; // Производитель
    private double price; // Цена
    private int expirationDate; //Срок годности
    private DateTime productionDate; //Дата производства


    // Конструктор.
    public Product(string Name, string Manufacturer, double Price,
        int ExpirationDate, DateTime ProductionDate)
    {
        nаme = Name;
        manufacturer = Manufacturer;
        price = Price;
        expirationDate = ExpirationDate;
        productionDate = ProductionDate;
    }

    //Формат вывода.
    public override string ToString()
    {
        return string.Format(
            "Наименование: {0}\nПроизводитель: {1}\nЦена: {2:F2} руб.\nСрок годности: {3} дн.\nДата производства: {4:yyyy-MM-dd}\n",
            nаme, manufacturer, price, expirationDate, productionDate
            );
    }
}

public class DiscountedProduct : Product
{
    private int discountPercent; //Размер скидки (%)
    private double promotionalPrice; //Акционная цена

    // Конструктор.
    public DiscountedProduct(string Name, string Manufacturer, double Price,
        int ExpirationDate, DateTime ProductionDate, int DiscountPercent)
        : base(Name, Manufacturer, Price, ExpirationDate, ProductionDate)
    {
        this.discountPercent = DiscountPercent;
        this.promotionalPrice = Price * (1 - DiscountPercent /100.0);
    }

    //Формат вывода.
    public override string ToString()
    {
        return base.ToString() + string.Format(
            "Скидка: {0} %\nЦена по скидке: {1:F2} руб.\n",
           discountPercent, promotionalPrice
            );
    }
}

public class Program
{
    public static void Main ()
    {
        Console.WriteLine("Введите данные товаров:");

        Console.WriteLine("Наименование:");
        string name = Console.ReadLine();

        Console.WriteLine("Производитель:");
        string manufacturer = Console.ReadLine();

        Console.WriteLine("Цена:");
        double price = double.Parse(Console.ReadLine());

        Console.WriteLine("Срок годности:");
        int expirationDate = int.Parse(Console.ReadLine());

        Console.WriteLine("Дата производства:");
        DateTime productionDate = DateTime.Parse(Console.ReadLine());

        Console.WriteLine("Скидка:");
        int discountPercent = int.Parse(Console.ReadLine());

        DiscountedProduct product =  new DiscountedProduct(name, manufacturer, price, expirationDate, productionDate, discountPercent);

        Console.WriteLine(product);
    }
}

    
