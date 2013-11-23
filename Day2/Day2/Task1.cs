using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading;
using System.Threading.Tasks;

namespace Day2
{
    class Task1
    {
        public static void Start()
        {
            var destination = new List<int>();
            const int max = 10000000;
            var thread1 = new Thread(() => destination.AddRange(Enumerable.Range(1, max).Select(i => i*2)));
            var thread2 = new Thread(() => destination.AddRange(Enumerable.Range(1, max).Select(i => 1+i*2)));
            thread1.Start();
            thread2.Start();
            thread1.Join();
            thread2.Join();
            Console.WriteLine(destination.Count);
        }
    }
}
