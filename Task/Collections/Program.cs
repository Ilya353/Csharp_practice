using System.Collections;

public class SmartStack<T> : IEnumerable<T>
{
    private T[] _array;
    private int _count;

    // Конструктор без параметров.
    public SmartStack() : this(4)
    {
    }

    // Вывод стека.
    public void Print()
    {
        if (_count == 0)
        {
            Console.WriteLine("Стек пуст");
            return;
        }

        for (int i = _count - 1; i >= 0; i--)
        {
            Console.WriteLine(_array[i]);
        }
    }

    // Конструктор с 1 целочисленным параметром.
    public SmartStack(int i)
    {
        if (i < 0)
            throw new ArgumentException(nameof(i));
        _array = new T[i];
        _count = 0;
    }

    // Конструктор,который в качестве параметра принимает коллекцию.
    public SmartStack(IEnumerable<T> collection)
    {
        if (collection == null)
            throw new ArgumentNullException(nameof(collection));

        var list = new List<T>(collection);
        _array = new T[list.Count];
        _count = list.Count;

        for (int i = 0; i < list.Count; i++)
        {
            _array[i] = list[list.Count - 1 - i];
        }
    }

    // Метод для изменения размера массива.
    private void Resize(int size)
    {
        T[] _newArray = new T[size];
        Array.Copy(_array, 0, _newArray, 0, _count);
        _array = _newArray;
    }

    // Добавление элемента на вершину стека.
    public void Push(T item)
    {
        if (_count == _array.Length)
        {
            Resize(_array.Length * 2);
        }
        _array[_count++] = item;
    }

    // Добавление на вершину стека содержимого коллекции.
    public void PushRange(IEnumerable<T> coll)
    {
        if (coll == null)
            throw new ArgumentNullException(nameof(coll));

        var list = new List<T>(coll);

        if (list.Count == 0)
            return;

        if (_count + list.Count > _array.Length)
        {
            int size = _array.Length;
            while (size < _count + list.Count)
            {
                size*=2;
            }
            Resize(size);
        }

        for (int i = list.Count - 1; i >= 0; i--)
        {
            _array[_count++] = list[i];
        }
    }

    // Удаление и возвращение элемента с вершины стека.
    public T Pop()
    {
        if (_count == 0)
            throw new InvalidOperationException("Стек пустой");

        _count--;
        T result = _array[_count];
        _array[_count] = default;
        return result;
    }

    // Возвращение элемента с вершины стека без удаления.
    public T Peek()
    {
        if (_count == 0)
            throw new InvalidOperationException("Стек пустой");
        return _array[_count - 1];
    }

    // Проверка наличия элемента в стеке.
    public bool Contains(T item)
    {
        EqualityComparer<T> comparer = EqualityComparer<T>.Default;
        for (int i = 0; i < _count; i++)
        {
            if (comparer.Equals(_array[i], item))
                return true;
        }
        return false;
    }

    // Получение длины стека.
    public int Capacity => _array.Length;

    //Получение количества элиментов в стеке.
    public int Count => _count;

    // Методы реализующий интерфейсы IEnumerable и IEnumerable<T>.
    public IEnumerator<T> GetEnumerator()
    {
        for (int i = _count - 1; i >= 0; i--)
        {
            yield return _array[i];
        }
    }

    IEnumerator IEnumerable.GetEnumerator()
    {
        return GetEnumerator();
    }

    // Индексатор, позволяющий работать с элементами по глубине.
    public T this[int depth]
    {
        get
        {
            if (depth < 0 || depth >= _count)
                throw new ArgumentOutOfRangeException(nameof(depth));
            return _array[_count - 1 - depth];
        }
        set
        {
            if (depth < 0 || depth >= _count)
                throw new ArgumentOutOfRangeException(nameof(depth));
            _array[_count - 1 - depth] = value;
        }
    }
}

class Program
{
    static void Main()
    {
        Console.WriteLine("Задание 1");
        var stack1 = new SmartStack<int>();
        Console.WriteLine($"Стек 1: элементов - {stack1.Count}, размер = {stack1.Capacity}");

        Console.WriteLine("\nЗадание 2");
        var stack2 = new SmartStack<string>(10);
        Console.WriteLine($"Стек 2: элементов - {stack2.Count}, размер = {stack2.Capacity}");

        Console.WriteLine("\nЗадание 3");
        var coll = new[] { 1, 2, 3, 4, 5, 6 };
        var stack3 = new SmartStack<int>(coll);
        Console.WriteLine("Стек 3:");
        stack3.Print();
        Console.WriteLine($"Стек 3: элементов - {stack3.Count}, размер = {stack3.Capacity}");

        Console.WriteLine("\nЗадание 4");
        stack3.Push(10);
        stack3.Push(11);
        stack3.Push(12);
        Console.WriteLine("Стек 3 после вставки элементов в вершину:");
        stack3.Print();

        Console.WriteLine("\nЗадание 5");
        var coll1 = new[] { 15, 14, 13 };
        stack3.PushRange(coll1);
        Console.WriteLine("Стек 3 после вставки содержимого коллекции в вершину:");
        stack3.Print();

        Console.WriteLine("\nЗадание 6");
        var popStack = stack3.Pop();
        Console.WriteLine($"Удалён элемент: {popStack}");
        Console.WriteLine("Стек 3 после удаления элемента вершины:");
        stack3.Print();

        Console.WriteLine("\nЗадание 7");
        var peekStack = stack3.Peek();
        Console.WriteLine($"Элемент вершины: {peekStack}");
        Console.WriteLine("Стек 3:");
        stack3.Print();

        Console.WriteLine("\nЗадание 8");
        Console.WriteLine($"В стеке есть элемент 11 - {stack3.Contains(11)}");
        Console.WriteLine($"В стеке есть элемент 100 - {stack3.Contains(100)}");

        Console.WriteLine("\nЗадание 11");
        Console.WriteLine("Стек 3:");
        foreach (var item in stack3)
        {
            Console.WriteLine($"{item}");
        }

        Console.WriteLine("\nЗадание 12");
        stack3[7] = 17;
        Console.WriteLine("Стек 3 c изменённым элементом:");
        stack3.Print();
    }    
}
