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

        private Random rnd = new Random(DateTime.Now.Millisecond);

        private static int jobsId = 1;
        private static int designersPerJob = 4;

        private static int gnCurrentGeneration = 0;

        // Mediciones
        public static int measurementsGnAssignments = 0;
        public static int measurementsGnComparisons = 0;
        public static int measurementsBbAssignments = 0;
        public static int measurementsBbComparisons = 0;

        public Workspace()
        {

        }

        public Workspace(int ndesigners, int nlocations, int njobs)
        {
            workSections = Seedbed.GenerateWorkSections();
            jobs = Seedbed.GenerateRandomJobs(njobs);
            //jobs = new List<Job>();
            locations = Seedbed.GenerateRandomLocations(nlocations);
            designers = Seedbed.GenerateRandomDesigners(ndesigners);

            RunDesignerDefaultBindings();
            RunJobDefaultBindings();

            //GnGenerateDesignerParents();
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
                tmp.WorkSection = workSections[rnd.Next(0, workSections.Count)];
                tmp.Location = GetRandomLocationByWorkSchedule(tmp.WorkSection.Schedule);
                tmp.Designers = GetRandomDesignersByWorkSchedule(tmp.WorkSection.Schedule, designersPerJob);
            }
        }

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

        private Location GetRandomLocationByWorkSchedule(WorkSchedule schedule)
        {
            var matches = GetLocationsByWorkSchedule(schedule);
            return matches[rnd.Next(0, matches.Count - 1)];
        }

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

        private WorkSection GetRandomWorkSection() => workSections[rnd.Next(0, workSections.Count)];

        #region Algoritmo genético


        Job[] parentJobs;

        public void GnEvolve(int n)
        {
            if (parentJobs == null)
                parentJobs = GnGetParentJobs();

            parentJobs = GnCrossover(parentJobs[0], parentJobs[1]);
            int fitness = 0;

            for (int i = 0; i < n; i++) {
                for (int j = 0; j < jobs.Count; j++) {
                    bool fit = GnEvaluate(jobs[j]);
                    if (fit)
                        fitness++;
                }

                double generationFitness = ((jobs.Count / 100) * fitness) / 100;

                if (generationFitness < 50)
                    GnMutate();
                else
                    parentJobs = GnCrossover(parentJobs[0], parentJobs[1]);


                fitness = 0;
                gnCurrentGeneration++;
            }
        }

        private Job[] GnCrossover(Job j1, Job j2)
        {
            // si tienen la misma sección de trabajo, se mezcla intercambia un diseñador.
            if (j1.WorkSection == j2.WorkSection) {
                var j1d = j1.Designers[rnd.Next(0, j1.Designers.Count)];
                var j2d = j2.Designers[rnd.Next(0, j2.Designers.Count)];

                j1.Designers.RemoveAt(j1.Designers.IndexOf(j1d));
                j2.Designers.RemoveAt(j2.Designers.IndexOf(j2d));

                j1.Designers.Add(j2d);
                j2.Designers.Add(j1d);
            } else {
                var j1p = j1.Designers.Select(d => d.Price).Sum();
                var j2p = j2.Designers.Select(d => d.Price).Sum();

                if (j1p <= j2p) {
                    j2.Location = GetRandomLocationByWorkSchedule(j1.WorkSection.Schedule);
                    j2.Designers = GetRandomDesignersByWorkSchedule(j1.WorkSection.Schedule, designersPerJob);
                } else {
                    j1.Location = GetRandomLocationByWorkSchedule(j2.WorkSection.Schedule);
                    j1.Designers = GetRandomDesignersByWorkSchedule(j2.WorkSection.Schedule, designersPerJob);
                }
            }

            return new Job[2] { j1, j2 };
        }

        private bool IsInTheSameWorkShift(WorkSchedule schedule1, WorkSchedule schedule2)
        {
            if (schedule1 == WorkSchedule.NotAvailable || schedule2 == WorkSchedule.NotAvailable)
                return false;

            if (schedule1 == schedule2)
                return true;

            if ((schedule1 == WorkSchedule.AllDay && schedule2 == WorkSchedule.MidDay) || (schedule1 == WorkSchedule.MidDay && schedule2 == WorkSchedule.AllDay))
                return true;

            if ((schedule1 == WorkSchedule.AllNight && schedule2 == WorkSchedule.MidNight) || (schedule1 == WorkSchedule.MidNight && schedule2 == WorkSchedule.AllNight))
                return true;


            return false;
        }

        private bool GnEvaluate(Job job)
        {
            var jp = job.Designers.Select(d => d.Price).Sum();
            var jpp = parentJobs[rnd.Next(0, 2)].Designers.Select(d => d.Price).Sum();
            var diff = jp - jpp;

            if (diff <= jpp * 0.1)
                return true;

            return false;
        }

        private void GnMutate()
        {
            jobs.ForEach(j => {
                if (j != parentJobs[0] && j != parentJobs[1]) {
                    j.WorkSection = GetRandomWorkSection();
                    j.Location = GetRandomLocationByWorkSchedule(j.WorkSection.Schedule);
                    j.Designers = GetRandomDesignersByWorkSchedule(j.WorkSection.Schedule, designersPerJob);
                }
            });
        }

        public Job[] GnGetParentJobs()
        {
            return jobs.OrderBy(j => rnd.Next()).Take(2).ToList().ToArray();
        }

        #endregion

        #region Ramificación y poda

        private void Bb()
        {
            var ws = GetRandomWorkSection();
            var designersBySchedule = GetDesignersByWorkSchedule(ws.Schedule);
            Designer rootDesigner = designersBySchedule[ rnd.Next(0, designersBySchedule.Count)];

            var job = new Job(jobsId, $"Trabajo {jobsId}", ws, GetRandomLocationByWorkSchedule(ws.Schedule),
                new List<Designer>() { rootDesigner });

            int index = 0;

            for (int i = 0; i < designersBySchedule.Count; i++) {
                if (designersBySchedule[i].Price <= rootDesigner.Price)
                {
                    while(job.Designers[index]!=null)
                    {
                        if (job.Designers[index].Price <= rootDesigner.Price)
                        {
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