using System.Collections.Generic;

namespace TecGames.Models
{
    /// <summary>
    /// Trabajo (tarea).
    /// </summary>
    public class Job : IdentityBasis
    {
        private List<Designer> designers;
        private Location location;
        private WorkSection workSection;

        /// <summary>
        /// Inicializa una instancia de <see cref="Job"/>.
        /// </summary>
        /// <param name="id">Identificador del elemento.</param>
        /// <param name="name">Nombre o descripción del elemento.</param>
        public Job(int id, string name) : base(id, name)
        {

        }

        /// <summary>
        /// Inicializa una instancia de <see cref="Job"/>.
        /// </summary>
        /// <param name="id">Identificador del elemento.</param>
        /// <param name="name">Nombre o descripción del elemento.</param>
        /// <param name="designers">Lista de diseñadores asignados a la tarea.</param>
        /// <param name="location">Ubicación.</param>
        /// <param name="workSection">Sección de trabajo.</param>
        public Job(int id, string name, WorkSection workSection, Location location, List<Designer> designers) : base(id, name)
        {
            this.workSection = workSection;
            this.location = location;
            this.designers = designers != null ? designers : new List<Designer>();
        }

        /// <summary>
        /// Lista de diseñadores asignados a este trabajo.
        /// </summary>
        public List<Designer> Designers { get => designers; set => designers = value; }

        /// <summary>
        /// Ubicación.
        /// </summary>
        public Location Location { get => location; set => location = value; }

        /// <summary>
        /// Sección de trabajo.
        /// </summary>
        public WorkSection WorkSection { get => workSection; set => workSection = value; }

        /// <summary>
        /// Agrega un diseñador a la lista existente.
        /// </summary>
        /// <param name="designer">Diseñador.</param>
        public void AddDesigner(Designer designer)
        {
            if (!designers.Contains(designer))
                designers.Add(designer);
        }

        public override string ToString()
        {
            string str = $"{id} | {name}\n\t- W: {workSection.ToString()}\n\t- L: {location.ToString()}\n\t- D:\n";
            foreach (var d in designers)
                str += $"\t\t{d.ToString()}\n";

            return str;
        }
    }
}