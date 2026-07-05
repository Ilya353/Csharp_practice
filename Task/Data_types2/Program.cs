namespace PR2
{
    class Program
    {
        /// <summary>
        /// Производит построение ромба с заданной диагональю.
        /// </summary>
        /// <param name="N"></param>
        /// <returns>Изображение ромба.</returns>
        public static string Rhomb(int N)
        {   
            string result = "";
            int a = N / 2;
            int b = 0;

            string s = new string(' ', a);
            string r;
            result += s + '#' + s + "\n";

            a -=1;
            b +=1;
            
            for (int i = 1; i < N/2; i++)
            {
                s = new string(' ', a);
                r = new string(' ', b);
                result += s + '#' + r + '#' + s + "\n";
                a -= 1;
                b += 2;
            }

            for (int i = 0; i < N / 2; i++)
            {
                s = new string(' ', a);
                r = new string(' ', b);
                result += s + '#' + r + '#' + s + "\n";
                a += 1;
                b -= 2;
            }

            s = new string(' ', a);
            result += s + '#' + s + "\n";
            return result;
        }
        static void Main(string[] args)
        {
            Console.WriteLine("Введите длину диагонали ромба:");
            string initial_rhomb = Console.ReadLine();
            int N = Convert.ToInt32(initial_rhomb);

            string result1 = Rhomb(N);

            Console.WriteLine(result1);
        }
    }
}
