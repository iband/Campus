using System;
using System.Collections.Generic;
using System.Linq;
using System.Linq.Expressions;
using System.Net;
using System.Text;
using System.Text.RegularExpressions;
using System.Threading.Tasks;

namespace HTTPrest
{
    class Program
    {
        static void Main(string[] args)
        {
            var client = new WebClient();
            /*var response = client.DownloadData(@"http://172.20.15.209:8082/a9f0e61a137d86aa9db53465e0801612?answer=");
            var numbersPart = System.Text.Encoding.UTF8.GetString(response).Split('.')[0];*/


            //var e = new Expression(new Regex(@"[0-9\-\+]+").Match(numbersPart).Value);
            var evalResult = "";

            for (var i = 0; i < 2; ++i)
            {
                var response = client.DownloadData(@"http://172.20.15.209:8082/a9f0e61a137d86aa9db53465e0801612?answer=" + evalResult);
                Console.WriteLine(System.Text.Encoding.UTF8.GetString(response));
                var eval = new JScriptEvaluator();
                var numbersPart = System.Text.Encoding.UTF8.GetString(response).Split('.')[0];
                evalResult=eval.EvalToString(new Regex(@"[0-9\-\+]+").Match(numbersPart).Value);
            }
        }
    }
}
