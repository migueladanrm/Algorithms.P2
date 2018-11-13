using System;
using System.Collections.Generic;
using System.Linq;
using TecGames.Models;

namespace TecGames
{
    /// <summary>
    /// Espacio de trabajo.
    /// </summary>
    public class Workspace
    {
        private List<Job> jobs;
        private List<Location> locations;
        private List<WorkSection> workSections;
        private List<Designer> designers;

        private Random rnd = new Random(DateTime.Now.Millisecond);

        private static int jobsId = 1;

        // cantidad de diseñadores por trabajo.
        private static int designersPerJob = 4;

        // generación actual.
        private static int gnCurrentGeneration = 0;

        // Mediciones
        public static int measurementsGnAssignments = 0;
        public static int measurementsGnComparisons = 0;
        public static int measurementsBbAssignments = 0;
        public static int measurementsBbComparisons = 0;

        /// <summary>
        /// Inicializa un espacio de trabajo.
        /// </summary>
        /// <param name="n">Cantidad de objetos.</param>
        public Workspace(int n)
        {
            if (n < 10)
                throw new InvalidOperationException();

            workSections = Seedbed.GenerateWorkSections();
            jobs = Seedbed.GenerateRandomJobs(n);
            locations = Seedbed.GenerateRandomLocations(n);
            designers = Seedbed.GenerateRandomDesigners(n);

            RunDesignerDefaultBindings();
            RunJobDefaultBindings();
        }

        /// <summary>
        /// Diseñadores.
        /// </summary>
        public List<Designer> Designers => designers;

        /// <summary>
        /// Trabajos.
        /// </summary>
        public List<Job> Jobs => jobs;

        /// <summary>
        /// Ubicaciones.
        /// </summary>
        public List<Location> Locations => locations;

        /// <summary>
        /// Secciones de trabajo.
        /// </summary>
        public List<WorkSection> WorkSections => workSections;

        /// <summary>
        /// Realiza los enlaces iniciales para los diseñadores.
        /// </summary>
        private void RunDesignerDefaultBindings()
        {
            foreach (var designer in designers)
                designer.WorkSection = designer.DayShift != WorkSchedule.NotAvailable ? GetWorkSectionBySchedule(designer.DayShift) : GetWorkSectionBySchedule(designer.NightShift);
        }

        /// <summary>
        /// Realiza los enlaces iniciales para los trabajos.
        /// </summary>
        private void RunJobDefaultBindings()
        {
            for (int i = 0; i < jobs.Count; i++) {
                var tmp = jobs[i];
                tmp.WorkSection = workSections[rnd.Next(0, workSections.Count)];
                tmp.Location = GetRandomLocationByWorkSchedule(tmp.WorkSection.Schedule);
                tmp.Designers = GetRandomDesignersByWorkSchedule(tmp.WorkSection.Schedule, designersPerJob);
            }
        }

        #region Métodos auxiliares

        /// <summary>
        /// Obtiene una sección de trabajo correspondiente a su horario.
        /// </summary>
        /// <param name="schedule">Horario de trabajo.</param>
        /// <returns>Sección de trabajo.</returns>
        private WorkSection GetWorkSectionBySchedule(WorkSchedule schedule)
        {
            if (workSections != null && schedule != WorkSchedule.NotAvailable)
                return workSections.Where(ws => ws.Schedule == schedule).First();

            return null;
        }

        /// <summary>
        /// Obtiene ubicaciones filtradas por horario de trabajo.
        /// </summary>
        /// <param name="schedule">Horario de trabajo.</param>
        /// <returns>Lista de ubicaciones.</returns>
        private List<Location> GetLocationsByWorkSchedule(WorkSchedule schedule)
        {
            if (schedule == WorkSchedule.NotAvailable)
                throw new InvalidOperationException($"No se acepta el valor '{schedule}' en esta operación.");

            var matches = new List<Location>();

            switch (schedule) {
                case WorkSchedule.AllDay:
                case WorkSchedule.MidDay:
                    matches.AddRange(locations.Where(loc => loc.DayShift == WorkSchedule.AllDay));
                    if (schedule == WorkSchedule.MidDay)
                        matches.AddRange(locations.Where(loc => loc.DayShift == WorkSchedule.MidDay));
                    break;
                case WorkSchedule.AllNight:
                case WorkSchedule.MidNight:
                    matches.AddRange(locations.Where(loc => loc.NightShift == WorkSchedule.AllNight));
                    if (schedule == WorkSchedule.MidNight)
                        matches.AddRange(locations.Where(loc => loc.NightShift == WorkSchedule.MidNight));
                    break;
            }

            return matches;
        }

        /// <summary>
        /// Obtiene una ubicación aleatoria por horario de trabajo.
        /// </summary>
        /// <param name="schedule">Horario de trabajo.</param>
        /// <returns>Ubicación.</returns>
        private Location GetRandomLocationByWorkSchedule(WorkSchedule schedule)
        {
            var matches = GetLocationsByWorkSchedule(schedule);
            return matches[rnd.Next(0, matches.Count)];
        }

        /// <summary>
        /// Obtiene diseñadores filtrados por horario de trabajo.
        /// </summary>
        /// <param name="schedule">Horario de trabajo.</param>
        /// <returns>Lista de diseñadores.</returns>
        public List<Designer> GetDesignersByWorkSchedule(WorkSchedule schedule)
        {
            var target = new List<Designer>();

            switch (schedule) {
                case WorkSchedule.AllDay:
                case WorkSchedule.MidDay:
                    target.AddRange(designers.Where(d => d.DayShift == WorkSchedule.AllDay));
                    if (schedule == WorkSchedule.MidDay)
                        target.AddRange(designers.Where(d => d.DayShift == WorkSchedule.MidDay));
                    break;
                case WorkSchedule.AllNight:
                case WorkSchedule.MidNight:
                    target.AddRange(designers.Where(d => d.NightShift == WorkSchedule.AllNight));
                    if (schedule == WorkSchedule.MidDay)
                        target.AddRange(designers.Where(d => d.NightShift == WorkSchedule.MidNight));
                    break;
            }

            return target;
        }

        /// <summary>
        /// Obtiene una lista de diseñadores aleatorios filtrados por horario de trabajo.
        /// </summary>
        /// <param name="schedule">Horario de trabajo.</param>
        /// <param name="n">Cantidad de diseñadores.</param>
        /// <returns>Lista de diseñadores.</returns>
        public List<Designer> GetRandomDesignersByWorkSchedule(WorkSchedule schedule, int n)
        {
            var randomDesigners = new List<Designer>();
            var designersBySchedule = GetDesignersByWorkSchedule(schedule);

            while (randomDesigners.Count < n) {
                var tmpDesigner = designersBySchedule[rnd.Next(0, designersBySchedule.Count)];
                if (!randomDesigners.Contains(tmpDesigner))
                    randomDesigners.Add(tmpDesigner);
            }

            return randomDesigners;
        }

        /// <summary>
        /// Obtiene una sección de trabajo aleatoria.
        /// </summary>
        /// <returns>Sección de trabajo.</returns>
        private WorkSection GetRandomWorkSection() => workSections[rnd.Next(0, workSections.Count)];

        #endregion

        #region Algoritmo genético

        Job[] gnParentJobs;

        /// <summary>
        /// Ejecuta el proceso de evolución.
        /// </summary>
        /// <param name="n">Cantidad de generaciones.</param>
        public void GnEvolve(int n)
        {
            if (gnParentJobs == null)
                gnParentJobs = jobs.OrderBy(j => rnd.Next()).Take(2).ToList().ToArray();

            gnParentJobs = GnCrossover(gnParentJobs[0], gnParentJobs[1]);

            int fitness = 0;

            for (int i = 0; i < n; i++) {
                for (int j = 0; j < jobs.Count; j++) {
                    bool fit = GnEvaluate(jobs[j]);
                    if (fit) {
                        fitness++;
                    }
                }

                if (((jobs.Count / 100) * fitness) / 100 < 50)
                    GnMutate();
                else
                    gnParentJobs = GnCrossover(gnParentJobs[0], gnParentJobs[1]);

                Console.WriteLine($"Generación {i}: {jobs.Select(j => j.Designers.Select(d => d.Price).Sum()).Sum()}");

                fitness = 0;
                gnCurrentGeneration++;
            }
        }

        /// <summary>
        /// Cruza un par de <see cref="Job"/> (generalmente padres).
        /// </summary>
        /// <param name="j1">Instancia 1.</param>
        /// <param name="j2">Instancia 2.</param>
        /// <returns>Cruces.</returns>
        private Job[] GnCrossover(Job j1, Job j2)
        {
            // Si tienen la misma sección de trabajo, se intercambia un diseñador y se agrega uno aleatorio.
            if (j1.WorkSection == j2.WorkSection) {
                var j1d = j1.Designers[rnd.Next(0, j1.Designers.Count)];
                var j2d = j2.Designers[rnd.Next(0, j2.Designers.Count)];

                j1.Designers.RemoveAt(j1.Designers.IndexOf(j1d));
                j2.Designers.RemoveAt(j2.Designers.IndexOf(j2d));

                j1.Designers.Add(j2d);
                j2.Designers.Add(j1d);

                j1.Designers.RemoveAt(0);
                j2.Designers.RemoveAt(0);

                var designerCandidatesForJ1 = GetDesignersByWorkSchedule(j1.WorkSection.Schedule);
                var designerCandidatesForJ2 = GetDesignersByWorkSchedule(j2.WorkSection.Schedule);

                j1.Designers.Add(designerCandidatesForJ1[rnd.Next(0, designerCandidatesForJ1.Count)]);
                j2.Designers.Add(designerCandidatesForJ2[rnd.Next(0, designerCandidatesForJ2.Count)]);
            } else {
                // Si la sección de trabajo es distinta, se modifica el trabajo con mayor costo al tipo del de menor costo.
                var j1p = j1.Designers.Select(d => d.Price).Sum();
                var j2p = j2.Designers.Select(d => d.Price).Sum();

                if (j1p <= j2p) {
                    j2.WorkSection = j1.WorkSection;
                    j2.Location = GetRandomLocationByWorkSchedule(j1.WorkSection.Schedule);
                    j2.Designers = GetRandomDesignersByWorkSchedule(j1.WorkSection.Schedule, designersPerJob);

                    j1.Designers[rnd.Next(0, j1.Designers.Count)].Price = rnd.Next((int)Math.Min(j1p, j2p), (int)Math.Max(j1p, j2p)) + Math.Round(rnd.NextDouble());
                } else {
                    j1.WorkSection = j2.WorkSection;
                    j1.Location = GetRandomLocationByWorkSchedule(j2.WorkSection.Schedule);
                    j1.Designers = GetRandomDesignersByWorkSchedule(j2.WorkSection.Schedule, designersPerJob);

                    j2.Designers[rnd.Next(0, j2.Designers.Count)].Price = rnd.Next((int)Math.Min(j1p, j2p), (int)Math.Max(j1p, j2p)) + Math.Round(rnd.NextDouble());
                }
            }

            return new Job[2] { j1, j2 };
        }

        /// <summary>
        /// Evalúa la aptitud de un trabajo.
        /// </summary>
        /// <param name="job">Trabajo a evaluar.</param>
        /// <returns>Aptitud.</returns>
        private bool GnEvaluate(Job job)
        {
            var jp = job.Designers.Select(d => d.Price).Sum();
            var jpp = gnParentJobs[rnd.Next(0, 2)].Designers.Select(d => d.Price).Sum();
            var diff = jp - jpp;

            if (diff < 0 || diff <= jpp * 0.1)
                return true;

            return false;
        }

        /// <summary>
        /// Muta todos los trabajos.
        /// </summary>
        private void GnMutate()
        {
            foreach (var j in jobs) {
                if (j != gnParentJobs[0] && j != gnParentJobs[1]) {
                    j.WorkSection = GetRandomWorkSection();
                    j.Location = GetRandomLocationByWorkSchedule(j.WorkSection.Schedule);
                    j.Designers = GetRandomDesignersByWorkSchedule(j.WorkSection.Schedule, designersPerJob);

                    foreach (var d in j.Designers) {
                        var pjd = gnParentJobs[rnd.Next(0, 2)].Designers[rnd.Next(0, designersPerJob)];
                        if (d.Price > pjd.Price)
                            d.Price = rnd.Next((int)pjd.Price, (int)d.Price) + Math.Round(rnd.NextDouble(), 2);
                    }
                }
            }
        }

        #endregion

        #region Ramificación y poda

        private void Bb()
        {
            var ws = GetRandomWorkSection();
            var designersBySchedule = GetDesignersByWorkSchedule(ws.Schedule);
            Designer rootDesigner = designersBySchedule[rnd.Next(0, designersBySchedule.Count)];

            var job = new Job(jobsId, $"Trabajo {jobsId}", ws, GetRandomLocationByWorkSchedule(ws.Schedule),
                new List<Designer>() { rootDesigner });

            int index = 0;

            for (int i = 0; i < designersBySchedule.Count; i++) {
                if (designersBySchedule[i].Price <= rootDesigner.Price) {
                    while (job.Designers[index] != null) {
                        if (job.Designers[index].Price <= rootDesigner.Price) {
                            index++;
                            job.Designers[index] = designersBySchedule[i];
                        }
                    }
                }
            }
        }

        #endregion
    }
}