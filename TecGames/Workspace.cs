using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
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
            jobs = Seedbed.GenerateRandomJobs((int)(totalDesigners * 0.25));
            locations = Seedbed.GenerateRandomLocations((int)(totalDesigners * 0.15));
            workSections = Seedbed.GenerateWorkSections();
            designers = Seedbed.GenerateRandomDesigners(totalDesigners);

            RunDesignerDefaultBindings();
            RunJobDefaultBindings();
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
            foreach(var designer in designers)
                designer.WorkSection = designer.DayShift != WorkSchedule.NotAvailable ? GetWorkSectionBySchedule(designer.DayShift) : GetWorkSectionBySchedule(designer.NightShift);
        }

        private void RunJobDefaultBindings()
        {
            for (int i = 0; i < jobs.Count; i++) {
                var tmp = jobs[i];
                tmp.WorkSection = workSections[random.Next(0, workSections.Count)];

                // filtrado de reportes (solo se incluyen los que coinciden estrictamente).
                //var matches = locations.Where(location => location.DayShift == tmp.WorkSection.Schedule || location.NightShift == tmp.WorkSection.Schedule).ToList();

                tmp.Location = GetLocationPotentialCandidates(tmp.WorkSection.Schedule);
            }
        }

        private Location GetLocationPotentialCandidates(WorkSchedule schedule)
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
    }
}