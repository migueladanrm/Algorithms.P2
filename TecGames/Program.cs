using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading;
using TecGames.Models;
using static System.Console;

namespace TecGames
{
    class Program
    {
        private static int[] sets = new int[] { 50, 100, 200, 500, 1000 };
        private static int generations = 25;

        // espacio de trabajo que contiene la información a operar.
        private static Workspace workspace;

        static void Main(string[] args)
        {
            WriteLine("ANÁLISIS DE ALGORITMOS\nII Proyecto Programado:Algoritmos Genético - Ramificación y poda \nIntegrantes:\n\t- Jose Carlo Hidalgo Chacón\n\t- Miguel Rivas Méndez\n");

            Write("En breve se iniciará la ejecución de los casos de prueba...");
            Thread.Sleep(2500);
            WriteLine();

            foreach (int n in sets) {
                Console.Clear();
                Write($"Iniciado caso de prueba para N={n} ...");
                Thread.Sleep(1000);
                WriteLine();

                workspace = new Workspace(n);

                AuxWriteTitle("Algoritmo genético");
                WriteLine($"---- Población inicial -----");
                PrintWorkspaceOverview(workspace, n);

                workspace.GnEvolve(generations);

                WriteLine($"---- Después de aplicar algoritmo genético -----");
                PrintWorkspaceOverview(workspace, n);

                WriteLine("\nMEDICIONES:");
                WriteLine($"Asignaciones: {workspace.measurementsGnAssignments}");
                WriteLine($"Comparaciones: {workspace.measurementsGnComparisons}");
                WriteLine($"Lineas ejecutadas: {workspace.measurementsGnAssignments + workspace.measurementsGnComparisons}");
                Write($"Tiempo de ejecución: {workspace.measurementsGnTime.Elapsed.TotalMilliseconds} ms\n");

                WriteLine();

                AuxWriteTitle("Algoritmo de ramificación y poda");
                workspace.RunBranchBound();

                WriteLine($"---- Después de aplicar ramificación y poda -----");
                PrintWorkspaceOverview(workspace, n);

                WriteLine("\nMEDICIONES:");
                WriteLine($"Asignaciones: {workspace.measurementsBbAssignments}");
                WriteLine($"Comparaciones: {workspace.measurementsBbComparisons}");
                WriteLine($"Lineas ejecutadas: {workspace.measurementsBbAssignments + workspace.measurementsBbComparisons}");
                Write($"Tiempo de ejecución: {workspace.measurementsBbTime.Elapsed.TotalMilliseconds} ms\n\nPresione ENTER para continuar...");

                ReadKey();
                WriteLine();
            }

            Write("\n\nEl programa ha finalizado...");

            ReadKey();
            ReadKey();
        }

        /// <summary>
        /// Imprime una visión general del espacio de trabajo.
        /// </summary>
        /// <param name="ws">Espacio de trabajo.</param>
        /// <param name="n">Tamaño del arreglo actual.</param>
        static void PrintWorkspaceOverview(Workspace ws, int n)
        {
            var schedules = new Dictionary<WorkSchedule, int> {
                { WorkSchedule.AllDay, ws.Jobs.Where(j => j.WorkSection.Schedule == WorkSchedule.AllDay).Count() },
                { WorkSchedule.MidDay, ws.Jobs.Where(j => j.WorkSection.Schedule == WorkSchedule.MidDay).Count() },
                { WorkSchedule.AllNight, ws.Jobs.Where(j => j.WorkSection.Schedule == WorkSchedule.AllNight).Count() },
                { WorkSchedule.MidNight, ws.Jobs.Where(j => j.WorkSection.Schedule == WorkSchedule.MidNight).Count() }
            };

            WriteLine($"Se tienen {schedules.Values.Sum() - 1} trabajos con un precio total de {workspace.GetJobsTotalPrice()} distribuido en:");
            foreach (var kv in schedules)
                WriteLine($"\t- {kv.Key}: {kv.Value}");
        }

        #region Métodos auxiliares

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

            foreach (var job in workspace.Jobs.Take(1))
                WriteLine(job.ToString());

            WriteLine($"Precio total: {workspace.Jobs.Select(j => j.Designers.Select(d => d.Price).Sum()).Sum()}");

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

        #endregion
    }
}