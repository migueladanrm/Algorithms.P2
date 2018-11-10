using System;
using System.Collections.Generic;

namespace TecGames.Models
{
    /// <summary>
    /// Diseñador.
    /// </summary>
    public class Designer : IdentityBasis, IWorkShifts
    {
        private WorkSchedule dayShift;
        private WorkSchedule nightShift;
        private Dictionary<WorkSection, decimal> prices;
        private List<WorkSection> workSections;

        /// <summary>
        /// Inicializa una instancia de <see cref="Designer"/>.
        /// </summary>
        /// <param name="id">Identificador del elemento.</param>
        /// <param name="name">Nombre.</param>
        /// <param name="dayShift">Turno de trabajo de día.</param>
        /// <param name="nightShift">Turno de trbajo de noche.</param>
        /// <param name="workSections">Secciones de trabajo.</param>
        /// <param name="prices">Precio por secciones de trabajo.</param>
        public Designer(int id, string name, WorkSchedule dayShift, WorkSchedule nightShift, List<WorkSection> workSections, Dictionary<WorkSection, decimal> prices) : base(id, name)
        {
            if (dayShift == WorkSchedule.NotAvailable || dayShift == WorkSchedule.AllDay || dayShift == WorkSchedule.MidDay)
                this.dayShift = dayShift;
            else throw new InvalidOperationException($"El campo '{nameof(DayShift)}' no puede tener el valor '{dayShift}'.");

            if (nightShift == WorkSchedule.NotAvailable || nightShift == WorkSchedule.AllNight || nightShift == WorkSchedule.MidNight)
                this.nightShift = nightShift;
            else throw new InvalidOperationException($"El campo '{nameof(NightShift)}' no puede tener el valor '{nightShift}'.");

            this.workSections = workSections;
            this.prices = prices;
        }

        /// <summary>
        /// Horario de trabajo diurno.
        /// </summary>
        public WorkSchedule DayShift {
            get => dayShift;
            set => dayShift = value;
        }

        /// <summary>
        /// Horario de trabajo nocturno.
        /// </summary>
        public WorkSchedule NightShift {
            get => nightShift;
            set => nightShift = value;
        }

        /// <summary>
        /// Secciones de trabajo.
        /// </summary>
        public List<WorkSection> WorkSections {
            get => workSections;
            set => workSections = value;
        }

        /// <summary>
        /// Precios por sección de trabajo.
        /// </summary>
        public Dictionary<WorkSection, decimal> Prices {
            get => prices;
            set => prices = value;
        }
    }
}