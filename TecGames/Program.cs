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

        private static List<Job> jobs;
        private static List<Location> locations;
        private static List<WorkSection> workSections;
        private static List<Designer> designers;


        static void Main(string[] args)
        {
            var designers = Seedbed.GenerateRandomDesigners(1000);

            foreach (var designer in designers)
                WriteLine(designer.ToString());

            ReadKey();
        }
    }
}