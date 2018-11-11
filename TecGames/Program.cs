using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using TecGames.Models;
using static System.Console;

namespace TecGames
{
    class Program
    {

        static void Main(string[] args)
        {
            int n = 30;

            var workspace = new Workspace(n);

            foreach (var job in workspace.Jobs)
                WriteLine(job.ToString());

            ReadKey();
        }
    }
}