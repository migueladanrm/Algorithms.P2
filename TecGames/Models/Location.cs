using System;

namespace TecGames.Models
{
    /// <summary>
    /// Ubicación de trabajo.
    /// </summary>
    public class Location : IdentityBasis, IWorkShifts
    {
        private WorkSchedule dayShift;
        private WorkSchedule nightShift;

        /// <summary>
        /// Inicializa una instancia de <see cref="Location"/>.
        /// </summary>
        /// <param name="id">Identificador del elemento.</param>
        /// <param name="name">Nombre o descripción del elemento.</param>
        /// <param name="dayShift">Horario diurno.</param>
        /// <param name="nightShift">Horario nocturno.</param>
        public Location(int id, string name, WorkSchedule dayShift, WorkSchedule nightShift) : base(id, name)
        {
            if (dayShift == WorkSchedule.NotAvailable || dayShift == WorkSchedule.AllDay || dayShift == WorkSchedule.MidDay)
                this.dayShift = dayShift;
            else throw new InvalidOperationException($"El campo '{nameof(DayShift)}' no puede tener el valor '{dayShift}'.");

            if (nightShift == WorkSchedule.NotAvailable || nightShift == WorkSchedule.AllNight || nightShift == WorkSchedule.MidNight)
                this.nightShift = nightShift;
            else throw new InvalidOperationException($"El campo '{nameof(NightShift)}' no puede tener el valor '{nightShift}'.");
        }

        /// <summary>
        /// Horario diurno.
        /// </summary>
        public WorkSchedule DayShift => dayShift;

        /// <summary>
        /// Horario nocturno.
        /// </summary>
        public WorkSchedule NightShift => nightShift;

        public override string ToString()
        {
            //return $"{Utils.FillStringWithSpaces(id.ToString(), 4)} | {Utils.FillStringWithSpaces(name, 20)} | HD: {Utils.FillStringWithSpaces(dayShift.ToString(),12)} | HN: {Utils.FillStringWithSpaces(nightShift.ToString(), 12)}";

            return $"{id} | {name} | HD: {dayShift.ToString()} | HN: {nightShift.ToString()}";
        }
    }
}