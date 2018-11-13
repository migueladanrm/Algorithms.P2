using Newtonsoft.Json.Linq;
using System;
using System.Collections.Generic;
using System.IO;
using System.Text;
using TecGames.Models;

/// <summary>
/// Semillero de datos.
/// </summary>
public static class Seedbed
{

    #region Datos de inicialización.

    private static bool isLoaded = false;

    private static List<string> mkFirstName;
    private static List<string> mkLastName;

    private static Random random = new Random(DateTime.Now.Millisecond);

    /// <summary>
    /// Carga datos de pruebas.
    /// </summary>
    private static void LoadMokupData()
    {
        if (isLoaded)
            return;

        mkFirstName = new List<string>();
        mkLastName = new List<string>();

        var tmp = File.ReadAllText("persons.json", Encoding.UTF8);
        var json = JObject.Parse(tmp);

        foreach (string firstname in (JArray)json["firstname"])
            mkFirstName.Add(firstname);

        foreach (string lastname in (JArray)json["lastname"])
            mkLastName.Add(lastname);

        isLoaded = true;
    }

    #endregion

    /// <summary>
    /// Genera diseñadores aleatorios.
    /// </summary>
    /// <param name="n">Cantidad de registros.</param>
    /// <returns>Lista de diseñadores.</returns>
    public static List<Designer> GenerateRandomDesigners(int n)
    {
        LoadMokupData();

        var data = new List<Designer>();

        int i = 1;
        while (data.Count < n) {
            var tmp = new Designer(i, $"{mkFirstName[random.Next(0, mkFirstName.Count - 1)]} {mkLastName[random.Next(0, mkLastName.Count - 1)]}", GetRandomWorkSchedule(false), GetRandomWorkSchedule(true), null, 0);

            //if (!(tmp.DayShift == WorkSchedule.NotAvailable && tmp.NightShift == WorkSchedule.NotAvailable) && (((tmp.DayShift == WorkSchedule.AllDay || tmp.DayShift == WorkSchedule.MidDay) && tmp.NightShift == WorkSchedule.NotAvailable) || ((tmp.NightShift == WorkSchedule.AllNight || tmp.NightShift == WorkSchedule.MidNight) && tmp.DayShift == WorkSchedule.NotAvailable)))
            if (tmp.DayShift != WorkSchedule.NotAvailable || tmp.NightShift != WorkSchedule.NotAvailable)
                data.Add(tmp);

            i++;
        }

        return data;
    }

    /// <summary>
    /// Genera trabajos aleatorios.
    /// </summary>
    /// <param name="n">Cantidad de registros.</param>
    /// <returns>Lista de trabajos.</returns>
    public static List<Job> GenerateRandomJobs(int n)
    {
        var jobs = new List<Job>();

        int i = 1;
        while (jobs.Count <= n) {
            jobs.Add(new Job(i, $"Trabajo {i}"));
            i++;
        }

        return jobs;
    }

    /// <summary>
    /// Genera ubicaciones aleatorias.
    /// </summary>
    /// <param name="n">Cantidad de registros.</param>
    /// <returns>Lista de ubicaciones.</returns>
    public static List<Location> GenerateRandomLocations(int n)
    {
        var locations = new List<Location>();

        int i = 1;
        while (locations.Count <= n) {
            var tmpLocation = new Location(i, $"Ubicación {i}", GetRandomWorkSchedule(false), GetRandomWorkSchedule(true));

            if (!(tmpLocation.DayShift == WorkSchedule.NotAvailable && tmpLocation.NightShift == WorkSchedule.NotAvailable))
                locations.Add(tmpLocation);

            i++;
        }

        return locations;
    }

    /// <summary>
    /// Genera las secciones de trabajo.
    /// </summary>
    /// <returns>Lista de secciones de trabajo.</returns>
    public static List<WorkSection> GenerateWorkSections() => new List<WorkSection>() {
        new WorkSection(1, "7:00 AM - 4:00 PM"  ,random.Next(250, 500), WorkSchedule.AllDay),
        new WorkSection(2, "7:00 AM - 11:00 AM" ,random.Next(100, 250), WorkSchedule.MidDay),
        new WorkSection(3, "7:00 PM - 4:00 AM"  ,random.Next(250, 500), WorkSchedule.AllNight),
        new WorkSection(4, "7:00 PM - 11:00 PM" ,random.Next(100, 250), WorkSchedule.MidNight),
    };


    /// <summary>
    /// Obtiene un turno de trabajo aleatorio.
    /// </summary>
    /// <param name="shift">Determina el turno de trabajo, false para día, true para noche.</param>
    /// <returns>Turno de trabajo aleatorio.</returns>
    public static WorkSchedule GetRandomWorkSchedule(bool shift)
    {
        int[] values = new int[] { 0, shift ? 3 : 1, shift ? 4 : 2 };
        int value = values[random.Next(0, 3)];
        return (WorkSchedule)value;
    }
}