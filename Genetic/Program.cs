using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using static System.Console;
using TecGames.Models;
using System.IO;
using Newtonsoft.Json.Linq;

namespace Genetic
{
    class Program
    {
        private static List<Job> jobs;
        private static List<Location> locations;
        private static List<WorkSection> workSections;
        private static List<Designer> designers;


        static void Main(string[] args)
        {
            WriteLine("Hola, mundo!\n");

            var designers = Seedbed.GenerateRandomDesigners(1000);

            foreach (var designer in designers)
                WriteLine(designer.ToString());

            ReadKey();
        }

    }
}