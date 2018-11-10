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

        private static List<string> mokupFirstName;
        private static List<string> mokupLastName;

        static void Main(string[] args)
        {
            WriteLine("Hola, mundo!");

            LoadMokupData();


            var random = new Random(DateTime.Now.Millisecond);

            for (int i = 0; i < 10; i++) {
                WriteLine($"{mokupFirstName[random.Next(0,mokupFirstName.Count-1)]} {mokupLastName[random.Next(0, mokupLastName.Count - 1)]}");
            }

            ReadKey();
        }

        private static void LoadMokupData()
        {
            mokupFirstName = new List<string>();
            mokupLastName = new List<string>();

            var tmp = File.ReadAllText("persons.json", Encoding.UTF8);
            var json = JObject.Parse(tmp);

            foreach (string firstname in (JArray)json["firstname"])
                mokupFirstName.Add(firstname);

            foreach (string lastname in (JArray)json["lastname"])
                mokupLastName.Add(lastname);

        }

        private static void GenerateRandomData()
        {
        }


    }
}