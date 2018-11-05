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
        /// <param name="designers">Lista de diseñadores asignados a la tarea.</param>
        /// <param name="location">Ubicación.</param>
        /// <param name="workSection">Sección de trabajo.</param>
        public Job(int id, string name, List<Designer> designers, Location location, WorkSection workSection) : base(id, name)
        {
            this.designers = designers != null ? designers : new List<Designer>();
            this.location = location;
            this.workSection = workSection;
        }

        /// <summary>
        /// Lista de diseñadores asignados a este trabajo.
        /// </summary>
        public List<Designer> Designers => designers;

        /// <summary>
        /// Ubicación.
        /// </summary>
        public Location Location => location;

        /// <summary>
        /// Sección de trabajo.
        /// </summary>
        public WorkSection WorkSection => workSection;

        /// <summary>
        /// Agrega un diseñador a la lista existente.
        /// </summary>
        /// <param name="designer">Diseñador.</param>
        public void AddDesigner(Designer designer)
        {
            if (!designers.Contains(designer))
                designers.Add(designer);
        }
    }
}