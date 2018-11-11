using Newtonsoft.Json.Linq;
using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
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

    public static List<Designer> GenerateRandomDesigners(int count)
    {
        LoadMokupData();

        var data = new List<Designer>();

        for (int i = 0; i < count; i++)
            data.Add(new Designer(i + 1, $"{mkFirstName[random.Next(0, mkFirstName.Count - 1)]} {mkLastName[random.Next(0, mkLastName.Count - 1)]}", GetRandomWorkSchedule(false), GetRandomWorkSchedule(true), null, null));

        return data;
    }

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