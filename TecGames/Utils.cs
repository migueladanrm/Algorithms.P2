using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

/// <summary>
/// Utilidades.
/// </summary>
public static class Utils
{
    /// <summary>
    /// Rellena de espacios una cadena de texto hasta alcanzar una longitud especificada.
    /// </summary>
    /// <param name="str">Cadena de texto.</param>
    /// <param name="size">Longitud esperada.</param>
    /// <returns>Cadena de texto rellenada con espacios.</returns>
    public static string FillStringWithSpaces(string str, int size)
    {
        while (str.Length < size)
            str += " ";

        return str;
    }
}