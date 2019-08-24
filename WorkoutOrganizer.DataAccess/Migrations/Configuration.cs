namespace WorkoutOrganizer.DataAccess.Migrations
{
    using System;
    using System.Collections.Generic;
    using System.Data.Entity;
    using System.Data.Entity.Migrations;
    using System.Linq;
    using WorkoutOrganizer.Model;

    internal sealed class Configuration : DbMigrationsConfiguration<WorkoutOrganizer.DataAccess.WorkoutOrganizerDbContext>
    {
        public Configuration()
        {
            AutomaticMigrationsEnabled = false;
        }

        protected override void Seed(WorkoutOrganizer.DataAccess.WorkoutOrganizerDbContext context)
        {
            context.Clients.AddOrUpdate(
                c => c.FirstName,
                new Client { FirstName = "����", LastName = "��������" },
                new Client { FirstName = "������", LastName = "�������" },
                new Client { FirstName = "�������", LastName = "�����" },
                new Client { FirstName = "�������", LastName = "�������" }
                );
            context.MusculeGroups.AddOrUpdate(
                mg => mg.Name,
                new MusculeGroup { Name = "������" },
                new MusculeGroup { Name = "����" },
                new MusculeGroup { Name = "�����" },
                new MusculeGroup { Name = "������" },
                new MusculeGroup { Name = "�������" }
                );
            context.Exercises.AddOrUpdate(
                e => e.Name,
                new Exercise { Name = "��� �����" },
                new Exercise { Name = "������ ����" },
                new Exercise { Name = "��������� �������" },
                new Exercise { Name = "�������- ���������" },
                new Exercise { Name = "������� �����" }
                );
            context.Equipments.AddOrUpdate(
                eq => eq.Name,
                new Equipment { Name = "�������" },
                new Equipment { Name = "�������" },
                new Equipment { Name = "����"},
                new Equipment { Name = "����" },
                new Equipment { Name = "�������" },
                new Equipment { Name = "�������" }
                );

            context.SaveChanges();

            context.ClientPhoneNumbers.AddOrUpdate(ph => ph.Number,
                new ClientPhoneNumber { Number = "0894485623", ClientId = context.Clients.First().Id });

            context.ExerciseSetups.AddOrUpdate(ph => ph.Position,
                new ExerciseSetup { Position = 1, WorkoutId = context.Workouts.First().Id, ExerciseId = context.Exercises.First().Id });

            context.Workouts.AddOrUpdate(
                w => w.Title,
                 new Workout
                 {
                     Title = "���������� �� ����� ����",
                     Date = new DateTime(2019, 12, 22),
                 });
        }
    }
}
