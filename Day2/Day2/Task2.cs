using System;
using System.Collections.Concurrent;
using System.Collections.Generic;
using System.Diagnostics;
using System.Linq;
using System.Text;
using System.Threading;
using System.Threading.Tasks;

namespace Day2
{
    class Task2
    {
        static readonly object Locker = new object();

        public static void Start()
        {
            var destination = new List<int>();
            const int max = 10000000;
            var thread1 = new Thread(() => Enumerable.Range(1, max).Select(i =>
            {
                lock(Locker)
                    destination.Add(i*2);
                return i;
            }));
            var thread2 = new Thread(() => Enumerable.Range(1, max).Select(i =>
            {
                lock (Locker)
                    destination.Add(1+ i * 2);
                return i;
            }));
            thread1.Start();
            thread2.Start();
            thread1.Join();
            thread2.Join();
            Console.WriteLine(destination.Count);
        }
    }
}
