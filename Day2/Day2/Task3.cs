using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Runtime.InteropServices;
using System.Text;
using System.Threading;
using System.Threading.Tasks;

namespace Day2
{
    class Task3
    {
        public static void Start()
        {
            const int count = 1000;
            var sinuses = Enumerable.Range(0, count)
                .AsParallel()
                .Select(i => new {sin = Math.Sin(i), i})
                .Where(arg => arg.sin > 0)
                .OrderBy(arg => arg.sin)
                .SelectMany(pair => System.Text.Encoding.UTF8.GetBytes(pair.i + " " + pair.sin + "\r\n"))
                .ToArray();

            var sinusesWrite = new ManualResetEvent(false);
            
            var sinusesFS = new FileStream("sinuses.txt", FileMode.Create, FileAccess.Write, FileShare.None, 1024, true);
            sinusesFS.BeginWrite(sinuses, 0, sinuses.Length, asyncResult =>
            {
                sinusesFS.EndWrite(asyncResult);
                sinusesFS.Dispose();
                sinusesWrite.Set();
            }, null);
            

            var cosinuses = Enumerable.Range(0, count)
                .AsParallel()
                .Select(i => new { cos = Math.Cos(i), i })
                .Where(arg => arg.cos > 0)
                .OrderBy(arg => arg.cos)
                .SelectMany(pair => System.Text.Encoding.UTF8.GetBytes(pair.i + " " + pair.cos + "\r\n"))
                .ToArray();

            var cosinusesFS = new FileStream("cosinuses.txt", FileMode.Create, FileAccess.Write, FileShare.None, 1024, true);

            var cosinusesWrite = new ManualResetEvent(false);

            cosinusesFS.BeginWrite(cosinuses, 0, cosinuses.Length, asyncResult =>
            {
                cosinusesFS.EndWrite(asyncResult);
                cosinusesFS.Dispose();
                cosinusesWrite.Set();
            }, null);

            sinusesWrite.WaitOne();
            cosinusesWrite.WaitOne();
        }
    }
}
