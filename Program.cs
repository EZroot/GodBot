using System;
using System.Collections.Generic;
using System.Text;
using System.Threading.Tasks;

namespace AshBot
{
    class Program
    {
        public static Task Main(string[] args)
            => Startup.RunAsync(args);
    }
}
