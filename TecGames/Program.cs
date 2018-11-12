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

        private static Workspace workspace;

        static void Main(string[] args)
        {
            int n = 100;

            workspace = new Workspace(n, n, n);

            foreach (var j in workspace.GnGetParentJobs())
                WriteLine(j.ToString());

            //PrintWorkSections();

            PrintJobs();
            //PrintDesigners();

            //foreach (var designer in workspace.Designers)
            //    WriteLine(designer.ToString());

            ReadKey();
        }


        static void PrintDesigners()
        {
            AuxWriteTitle("DISEÑADORES");

            foreach (var designer in workspace.Designers)
                WriteLine(designer.ToString());

            AuxWriteSectionDivider();
        }

        static void PrintJobs()
        {
            AuxWriteTitle("TRABAJOS");

            foreach (var job in workspace.Jobs.Take(5))
                WriteLine(job.ToString());

            AuxWriteSectionDivider();
        }

        static void PrintLocations()
        {
            AuxWriteTitle("UBICACIONES");

            foreach (var location in workspace.Locations.Take(10))
                WriteLine(location.ToString());

            AuxWriteSectionDivider();

        }

        static void PrintWorkSections()
        {
            AuxWriteTitle("SECCIONES DE TRABAJO");

            foreach (var ws in workspace.WorkSections)
                WriteLine(ws.ToString());

            AuxWriteSectionDivider();
        }

        private static void AuxWriteTitle(string title)
        {
            string target = $"=====| {title} |";
            while (target.Length < BufferWidth - 1)
                target += '=';

            WriteLine($"{target}\n");
        }

        private static void AuxWriteSectionDivider()
        {
            var divider = string.Empty;
            while (divider.Length < BufferWidth - 1)
                divider += '=';
            WriteLine($"\n{divider}\n");
        }
    }
}