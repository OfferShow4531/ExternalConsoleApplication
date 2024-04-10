using System.Collections;
using System.Linq;

namespace ConsoleApplication.Program
{
    public class MyList<T> : IEnumerable<T>
    {
        private T[] items;
        private int count;

        public MyList()
        {
            items = new T[4]; // Размер массива = 4
            count = 0;
        }

        public IEnumerator<T> GetEnumerator()
        {
            for (int i = 0; i < count; i++)
            {
                yield return items[i];
            }
        }
        IEnumerator IEnumerable.GetEnumerator()
        {
            return GetEnumerator();
        }

        public T this[int index]
        {
            get
            {
                if (index < 0 || index >= count)
                {
                    throw new IndexOutOfRangeException("Индекс вне диапазона");
                }

                return items[index];
            }
        }

        public void Add(T item)
        {
            // Расширение массива для предотвращение ошибки Index Out of Range
            if (count == items.Length)
            {
                Array.Resize(ref items, items.Length * 2);
            }

            items[count++] = item;
        }

        public IEnumerable<TResult> FilterAndTransform<TResult>(Func<T, bool> condition, Func<T, TResult> transform)
        {
            foreach (var item in items.Take(count))
            {
                if (condition(item))
                {
                    yield return transform(item);
                }
            }
        }

        public int Count
        {
            get { return count; }
        }

        public void RemoveAt(int index)
        {
            if (index < 0 || index >= count)
            {
                throw new IndexOutOfRangeException("Индекс вне диапазона");
            }

            for (int i = index; i < count - 1; i++)
            {
                items[i] = items[i + 1];
            }

            count--;
        }

        public void PrintAll()
        {
            for (int i = 0; i < count; i++)
            {
                Console.WriteLine(items[i]);
            }
        }

        public void PrintAtIndex(int index)
        {
            Console.WriteLine(this[index]);
        }
    }

    public static class MyListExtensions // Расширяющий метод MyListExtensions
    {
        public static T[] GetArray<T>(this MyList<T> list) // Наследование экземпляра конструктора MyList
        {
            return list.ToArray();
        }

        public static void PrintArray<T>(this T[] array)
        {
            foreach (var item in array)
            {
                Console.WriteLine(item);
            }
        }
    }

    public class Program
    {
        public static async Task Main(string[] args)
        {
            MyList<int> myList = new MyList<int>();
            myList.Add(10);
            myList.Add(20);
            myList.Add(30);

            Task firstOutputs = new Task(() =>
            {
                Thread.Sleep(1000);
                Console.WriteLine("Вывод элементов \n");

                Console.WriteLine("Элемент с индексом 1: ");
                myList.PrintAtIndex(1);
                Console.WriteLine("Количество элементов: " + myList.Count);
                Console.WriteLine("\n");
            });
            firstOutputs.Start();
            await firstOutputs;

            Task secondOutputs = new Task(() =>
            {
                Thread.Sleep(1000);
                Console.WriteLine("Удаление элемента с индексом: 1 \n");

                myList.RemoveAt(1);
                Console.WriteLine("После удаления элемента с индексом 1, количество элементов: " + myList.Count);
                Console.WriteLine("\n");
            });
            secondOutputs.Start();
            await secondOutputs;

            Task thirdOutputs = new Task(() =>
            {
                Thread.Sleep(1000);
                Console.WriteLine("Вывод всех элементов \n");

                Console.WriteLine("Все элементы:");
                myList.PrintAll();
                Console.WriteLine("\n");
            });
            thirdOutputs.Start();
            await thirdOutputs;

            Task fourthOutputs = new Task(() =>
            {
                Thread.Sleep(1000);
                Console.WriteLine("Вывод элемента по индексу: 0 \n");

                Console.WriteLine("Элемент по индексу 0:");
                myList.PrintAtIndex(0);
                Console.WriteLine("\n");
            });
            fourthOutputs.Start();
            await fourthOutputs;

            Task fifthOutputs = new Task(() =>
            {
                Thread.Sleep(1000);
                Console.WriteLine("Возврат расширяющего метода GetArray. \n");

                Console.WriteLine("Значения элементов расширяющего метода:");
                myList.GetArray().PrintArray();
                Console.WriteLine("\n");
            });
            fifthOutputs.Start();
            await fifthOutputs;

            Task sixthOutputs = new Task(() =>
            {
                Thread.Sleep(1000);
                Console.WriteLine("Использование SQL-синтаксиса. \n");

                int firstCondition = 10;
                int secondCondition = 2;
                var selectedItems = myList.FilterAndTransform(item => item >= 10, item => item * 2);
                Console.WriteLine($"Вывод элементов по условию myList->item >= {firstCondition} и item*{secondCondition} : \n");
                foreach (var item in selectedItems)
                {
                    Console.WriteLine(item);
                }
                Console.WriteLine("\n");
            });
            sixthOutputs.Start();
            await sixthOutputs;

            Console.WriteLine("Конец консольного приложения");
        }
    }
}