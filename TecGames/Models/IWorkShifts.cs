namespace TecGames.Models
{
    /// <summary>
    /// Permite la implementación de turnos de trabajo.
    /// </summary>
    public interface IWorkShifts
    {
        /// <summary>
        /// Horario diurno.
        /// </summary>
        WorkSchedule DayShift { get; }

        /// <summary>
        /// Horario nocturno.
        /// </summary>
        WorkSchedule NightShift { get; }
    }
}