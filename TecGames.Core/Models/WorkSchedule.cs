namespace TecGames.Models
{
    /// <summary>
    /// Horarios de trabajo.
    /// </summary>
    public enum WorkSchedule
    {
        /// <summary>
        /// No disponible en el turno (ajustable a diurno/nocturno).
        /// </summary>
        NotAvailable = 0,

        /// <summary>
        /// Todo el día de trabajo.
        /// </summary>
        AllDay = 1,

        /// <summary>
        /// Medio día de trabajo..
        /// </summary>
        MidDay = 2,

        /// <summary>
        /// Toda la noche de trabajo.
        /// </summary>
        AllNight = 3,

        /// <summary>
        /// Media noche de trabajo.
        /// </summary>
        MidNight = 4
    }
}