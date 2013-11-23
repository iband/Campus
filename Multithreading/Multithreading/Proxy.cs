using System;
using System.Collections.Concurrent;
using System.Collections.Generic;
using System.Linq;
using System.Linq.Expressions;
using System.Net;
using System.Text;
using System.Threading.Tasks;

namespace Multithreading
{
    static class Proxy
    {
        public static void Run(string baseURL)
        {
            //var cache = new Dictionary<string, byte[]>();
            var cache = new ConcurrentDictionary<string, byte[]>();

            var listener = new HttpListener();
            listener.Prefixes.Add(string.Format("http://+:31337/"));
            listener.Start();
            while (true)
            {
                var context = listener.GetContext();
                Task.Factory.StartNew(() =>
                {
                    var request = context.Request;
                    byte[] dataToResend = null;
                    //Console.WriteLine("raw=" + request.RawUrl);

                    if (cache.ContainsKey(request.RawUrl))
                    {
                        dataToResend = cache[request.RawUrl];
                    }
                    else
                    {
                        var client = new WebClient();
                        try
                        {
                            dataToResend = client.DownloadData(baseURL + request.RawUrl);
                        }
                        catch (WebException e)
                        {
                            //Console.WriteLine("Client got an error");
                            var errorText = new byte[1024];
                            var responseStream = e.Response.GetResponseStream();
                            if (responseStream != null)
                                responseStream.Read(errorText, 0, errorText.Length);
                            dataToResend = errorText;
                        }
                        cache[request.RawUrl] = dataToResend;
                        //Console.WriteLine(request.RawUrl + " cached");
                    }

                    context.Response.OutputStream.Write(dataToResend, 0, dataToResend.Count());
                    context.Response.Close();
                });
                //removeMe.Wait();
            }
        }
    }
}
