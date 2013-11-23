using System.Collections.Concurrent;
using System.Collections.Generic;
using System.IO;
using System.Net;
using System.Net.Http;
using System.Threading.Tasks;

namespace Multithreading
{
    static class Proxy
    {
        public static void Run(string baseURL)
        {
            //ServicePointManager.DefaultConnectionLimit = 100500;
            var cache = new ConcurrentDictionary<string, byte[]>();

            var listener = new HttpListener();
            listener.Prefixes.Add(string.Format("http://+:31337/"));
            listener.Start();
            while (true)
            {
                var context = listener.GetContext();
                Task.Run(() => ProcessClient(baseURL, context, cache));
            }
        }

        private async static void ProcessClient(string baseURL, HttpListenerContext context, IDictionary<string, byte[]> cache)
        {
            var request = context.Request;
            byte[] dataToResend = null;

            if (cache.ContainsKey(request.RawUrl))
            {
                dataToResend = cache[request.RawUrl];
            }
            else
            {
                var memoryStream = await GetDataToResend(baseURL + request.RawUrl);
                cache[request.RawUrl] = dataToResend = memoryStream.ToArray();
                
            }

            await context.Response.OutputStream.WriteAsync(dataToResend, 0, dataToResend.Length);
            context.Response.Close();
        }

        private async static Task<MemoryStream> GetDataToResend(string fullURL)
        {

            var memoryStream = new MemoryStream();
            var httpClient = new HttpClient();

            using(var clientStream = await httpClient.GetStreamAsync(fullURL))
            {
                await clientStream.CopyToAsync(memoryStream);
            }

            return memoryStream;
        }
    }
}
