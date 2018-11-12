using System;

namespace TecGames.Models
{
    /// <summary>
    /// Diseñador.
    /// </summary>
    public class Designer : IdentityBasis, IWorkShifts
    {
        private static Random random = new Random(DateTime.Now.Millisecond);

        private WorkSchedule dayShift;
        private WorkSchedule nightShift;
        private int price;
        private WorkSection workSection;

        /// <summary>
        /// Inicializa una instancia de <see cref="Designer"/>.
        /// </summary>
        /// <param name="id">Identificador del elemento.</param>
        /// <param name="name">Nombre.</param>
        /// <param name="dayShift">Turno de trabajo de día.</param>
        /// <param name="nightShift">Turno de trbajo de noche.</param>
        /// <param name="workSection">Sección de trabajo.</param>
        /// <param name="price">Precio por sección de trabajo.</param>
        public Designer(int id, string name, WorkSchedule dayShift, WorkSchedule nightShift, WorkSection workSection, int price) : base(id, name)
        {
            if (dayShift == WorkSchedule.NotAvailable || dayShift == WorkSchedule.AllDay || dayShift == WorkSchedule.MidDay)
                this.dayShift = dayShift;
            else throw new InvalidOperationException($"El campo '{nameof(DayShift)}' no puede tener el valor '{dayShift}'.");

            if (nightShift == WorkSchedule.NotAvailable || nightShift == WorkSchedule.AllNight || nightShift == WorkSchedule.MidNight)
                this.nightShift = nightShift;
            else throw new InvalidOperationException($"El campo '{nameof(NightShift)}' no puede tener el valor '{nightShift}'.");

            this.workSection = workSection;
            this.price = price;
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
        public WorkSection WorkSection {
            get => workSection;
            set {
                price = random.Next(value.Price, value.Price + 25);
                workSection = value;
            }
        }

        /// <summary>
        /// Precios por sección de trabajo.
        /// </summary>
        public int Price {
            get => price;
            set => price = value;
        }

        public override string ToString()
        {
            return $"{Utils.FillStringWithSpaces(id.ToString(), 4)} | {Utils.FillStringWithSpaces(name, 20)} | HD: {Utils.FillStringWithSpaces(dayShift.ToString(), 12)} | HN: {Utils.FillStringWithSpaces(nightShift.ToString(), 12)} | P: {price}";
        }
    }
}