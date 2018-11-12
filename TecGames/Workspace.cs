using System;
using System.Collections.Generic;
using System.Linq;
using TecGames.Models;

namespace TecGames
{
    public class Workspace
    {

        private List<Job> jobs;
        private List<Location> locations;
        private List<WorkSection> workSections;
        private List<Designer> designers;

        private Random random = new Random(DateTime.Now.Millisecond);

        public Workspace()
        {

        }

        public Workspace(int totalDesigners)
        {
            workSections = Seedbed.GenerateWorkSections();
            //jobs = Seedbed.GenerateRandomJobs((int)(totalDesigners * 0.25));
            jobs = new List<Job>();
            locations = Seedbed.GenerateRandomLocations((int)(totalDesigners * 0.15));
            designers = Seedbed.GenerateRandomDesigners(totalDesigners);

            RunDesignerDefaultBindings();
            //RunJobDefaultBindings();

            GnGenerateDesignerParents();
        }

        public List<Job> Jobs => jobs;
        public List<Location> Locations => locations;
        public List<WorkSection> WorkSections => workSections;
        public List<Designer> Designers => designers;

        private WorkSection GetWorkSectionBySchedule(WorkSchedule schedule)
        {
            if (workSections != null && schedule != WorkSchedule.NotAvailable)
                return workSections.Where(ws => ws.Schedule == schedule).First();

            return null;
        }

        private void RunDesignerDefaultBindings()
        {
            foreach (var designer in designers)
                designer.WorkSection = designer.DayShift != WorkSchedule.NotAvailable ? GetWorkSectionBySchedule(designer.DayShift) : GetWorkSectionBySchedule(designer.NightShift);
        }

        private void RunJobDefaultBindings()
        {
            for (int i = 0; i < jobs.Count; i++) {
                var tmp = jobs[i];
                tmp.WorkSection = workSections[random.Next(0, workSections.Count)];

                // filtrado de reportes (solo se incluyen los que coinciden estrictamente).
                //var matches = locations.Where(location => location.DayShift == tmp.WorkSection.Schedule || location.NightShift == tmp.WorkSection.Schedule).ToList();

                tmp.Location = GetRandomLocationByWorkSchedule(tmp.WorkSection.Schedule);
            }
        }

        private Location GetRandomLocationByWorkSchedule(WorkSchedule schedule)
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

            return matches[random.Next(0, matches.Count - 1)];
        }

        #region Algoritmo genético

        private Designer[] designerParents = new Designer[2];

        public void GnGenerateDesignerParents()
        {
            var a = designers.Where(d => d.Price == designers.Min(d1 => d1.Price)).FirstOrDefault();
            designers.RemoveAt(designers.IndexOf(a));

            var b = designers.Where(d => d.Price == designers.Min(d1 => d1.Price)).FirstOrDefault();
            designers.RemoveAt(designers.IndexOf(b));

            designerParents[0] = a;
            designerParents[1] = b;
        }

        public (Location A, Location B) GnGenerateLocationParents() => (locations.First(), locations.Last());

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

        private WorkSection GetRandomWorkSection() => workSections[random.Next(0, workSections.Count)];

        //List<Designer> designerChilds = new List<Designer>();

        private static int jobsId = 1;
        private static int designersPerJob = 3;

        public List<Job> Mutate(int n)
        {
            var tmpJobs = new List<Job>();

            var ws = GetRandomWorkSection();
            var designersBySchedule = GetDesignersByWorkSchedule(ws.Schedule);

            var job = new Job(jobsId, $"Trabajo {jobsId}", ws, GetRandomLocationByWorkSchedule(ws.Schedule), new List<Designer>());

            while (job.Designers.Count <= designersPerJob) {
                var tmpDesigner = designersBySchedule[random.Next(0, designersBySchedule.Count)];
                if (!job.Designers.Contains(tmpDesigner))
                    job.Designers.Add(tmpDesigner);
            }

            var designerParent = designerParents[random.Next(0, 2)];

            foreach(var designer in job.Designers) {

            }

            jobsId++;

            return null;
        }



        #endregion

        #region Ramificación y poda



        #endregion
    }
}