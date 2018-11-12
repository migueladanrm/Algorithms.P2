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

            foreach (var ws in workspace.WorkSections)
                WriteLine(ws.ToString());


            foreach (var designer in workspace.Designers)
                WriteLine(designer.ToString());

            

            ReadKey();
        }
    }
}