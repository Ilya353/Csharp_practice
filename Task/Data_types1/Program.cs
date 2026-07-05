
namespace PR1
{
    class Program
    {
        /// <summary>
        /// Производит расчёт вклада.
        /// </summary>
        /// <param name="initial_deposit"></param>
        /// <param name="years"></param>
        /// <param name="interest_rate"></param>
        /// <returns>сумма на вкладе по годам.</returns>
        public static string Contribution_calculator(double initial_deposit
        , double years, double interest_rate) 
        {
            string result = "";
            double sum = initial_deposit;
            for (int i = 1; i<= years; i++)
            {
                sum = sum * (100+interest_rate)/100;
                string formattedsum = sum.ToString("F2");
                result += "Год " + i + ": " + formattedsum + " руб.\n";
            }
            return result;
        }
        static void Main(string[] args)
        {
            Console.WriteLine("Введите сумму вклада:");
            string initial_deposit1 = Console.ReadLine();

            Console.WriteLine("Введите количество лет:");
            string years1 = Console.ReadLine();

            Console.WriteLine("Введите процент вклада:");
            string interest_rate1 = Console.ReadLine();

            double initial_deposit = Convert.ToDouble(initial_deposit1);
            double years = Convert.ToDouble(years1);
            double interest_rate = Convert.ToDouble(interest_rate1);

            string result1 = Contribution_calculator(initial_deposit,
            years, interest_rate);
            
            Console.WriteLine(result1);
        }
    }
}


