using System;

namespace TecGames.Models
{
    /// <summary>
    /// Sección de trabajo.
    /// </summary>
    public class WorkSection : IdentityBasis
    {
        private double price;
        private WorkSchedule schedule;

        /// <summary>
        /// Inicializa una instancia de <see cref="WorkSection"/>.
        /// </summary>
        /// <param name="id">Identificador del elemento.</param>
        /// <param name="name">Nombre o descripción.</param>
        /// <param name="price">Precio de la sección de trabajo.</param>
        /// <param name="schedule">Horario de la sección de trabajo.</param>
        public WorkSection(int id, string name, double price, WorkSchedule schedule) : base(id, name)
        {
            if (0 < price)
                this.price = price;
            else throw new InvalidOperationException($"El campo {nameof(Price)} debe ser mayor a cero.");

            if (!schedule.Equals(WorkSchedule.NotAvailable))
                this.schedule = schedule;
            else throw new InvalidOperationException($"Los horario de las secciones de trabajo no pueden tener el valor '{nameof(WorkSchedule.NotAvailable)}'.");
        }

        /// <summary>
        /// Precio de la sección de trabajo.
        /// </summary>
        public double Price { get => price; set => price = value; }

        /// <summary>
        /// Horario de la sección de trabajo.
        /// </summary>
        public WorkSchedule Schedule => schedule;

        public override string ToString()
        {
            return $"{schedule.ToString()} | P: {price}";
        }
    }
}