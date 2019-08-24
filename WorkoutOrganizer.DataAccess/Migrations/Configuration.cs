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
                new Client { FirstName = "Иван", LastName = "Димитров" },
                new Client { FirstName = "Георги", LastName = "Василев" },
                new Client { FirstName = "Димитър", LastName = "Митев" },
                new Client { FirstName = "Гергана", LastName = "Иванова" }
                );
            context.MusculeGroups.AddOrUpdate(
                mg => mg.Name,
                new MusculeGroup { Name = "Рамене" },
                new MusculeGroup { Name = "Гръб" },
                new MusculeGroup { Name = "Гърди" },
                new MusculeGroup { Name = "Бицепс" },
                new MusculeGroup { Name = "Трицепс" }
                );
            context.Exercises.AddOrUpdate(
                e => e.Name,
                new Exercise { Name = "Лег преса" },
                new Exercise { Name = "Мъртва тяга" },
                new Exercise { Name = "Бицепсово Сгъване" },
                new Exercise { Name = "Трицепс- Екстензии" },
                new Exercise { Name = "Раменна преса" }
                );
            context.Equipments.AddOrUpdate(
                eq => eq.Name,
                new Equipment { Name = "Лежанка" },
                new Equipment { Name = "Дъмбели" },
                new Equipment { Name = "Лост"},
                new Equipment { Name = "Въже" },
                new Equipment { Name = "Ластици" },
                new Equipment { Name = "Скрипец" }
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
                     Title = "Тренировка за горна част",
                     Date = new DateTime(2019, 12, 22),
                 });
        }
    }
}
