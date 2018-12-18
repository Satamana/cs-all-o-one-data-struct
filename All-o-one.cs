using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.Linq;
using System.Text;

namespace Data_Str
{
    class Program
    {
        static void Main(string[] args)
        {
            var data = new DataStruct();
            Random random = new Random();
            Stopwatch stopwatch = new Stopwatch();
            stopwatch.Start();
            for (int i = 0; i < 40; i++)
                data.Increment(random.Next(1, 10).ToString());
            for (int i = 0; i < 10; i++)
                data.Decrement(random.Next(1, 10).ToString());
            Console.WriteLine("Min Key: " + data.Min_Key());
            Console.WriteLine("Max Key: " + data.Max_Key() + "\n\n");
            stopwatch.Stop();
            Console.WriteLine(data);
            Console.WriteLine("Milliseconds = " + stopwatch.ElapsedMilliseconds);
        }
    }

    class Shmatok
    {
        public int Value { get; set; }
        public HashSet<string> Keys { get; set; } = new HashSet<string>();
    }

    class DataStruct
    {
        LinkedList<Shmatok> shmatki = new LinkedList<Shmatok>();
        Dictionary<string, LinkedListNode<Shmatok>> dictOfKeys = new Dictionary<string, LinkedListNode<Shmatok>>();

        public void Increment(string key)

        {
            if (shmatki.First is null)
                shmatki.AddFirst(new Shmatok());

            if (!dictOfKeys.ContainsKey(key))
            {
                LinkedListNode<Shmatok> v = Insert(shmatki.First, 0);
                v.Value.Keys.Add(key);
                dictOfKeys[key] = v;
            }
            var next = dictOfKeys[key].Next;
            var shmat = dictOfKeys[key];
            if (next == shmatki.Last || next.Value.Value > shmat.Value.Value + 1)
            {
                var v = next;
                next = Insert(next, shmat.Value.Value + 1);
            }
            next.Value.Keys.Add(key);
            dictOfKeys[key] = next;
            CheckshmatEmpty(key, shmat);
        }

        private void CheckshmatEmpty(string key, LinkedListNode<Shmatok> shmat)
        {
            shmat.Value.Keys.Remove(key);
            if (shmat.Value.Keys.Count == 0)
                shmatki.Remove(shmat);
        }

        private LinkedListNode<Shmatok> Insert(LinkedListNode<Shmatok> next, int idx)
        {
            next = shmatki.AddBefore(next, new Shmatok { Value = idx });
            return next;
        }

        public void Decrement(string key)
        {
            if (!dictOfKeys.ContainsKey(key))
                return;

            var prev = dictOfKeys[key].Previous;
            var shmat = dictOfKeys[key];
            dictOfKeys.Remove(key);
            if (shmat.Value.Value > 1)
            {
                if (shmat == shmatki.First || prev.Value.Value < shmat.Value.Value - 1)
                    prev = Insert(shmat, shmat.Value.Value - 1);
                prev.Value.Keys.Add(key);
                dictOfKeys[key] = prev;
            }
            CheckshmatEmpty(key, shmat);
        }

        public string Max_Key() => shmatki.Last.Previous?.Value?.Keys.First() ?? "";

        public string Min_Key() => shmatki.First?.Value?.Keys.First() ?? "";

        public override string ToString()
        {
            StringBuilder builder = new StringBuilder();
            foreach (var item in shmatki)
            {
                builder.Append($"Value = {item.Value}\r\n");
                builder.Append($"Keys: {string.Join(",", item.Keys)}\r\n\r\n");
            }
            return builder.ToString();
        }
    };
}