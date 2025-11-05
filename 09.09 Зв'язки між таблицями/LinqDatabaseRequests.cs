using Microsoft.EntityFrameworkCore;
using ModelsCreating.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace DataBaseModels
{
    internal class LinqDatabaseRequests
    {//1. Вывести номера корпусов...
        public static IEnumerable<int> GetBuildingsWithFinancingAbove(DbContext context, decimal sum)
        {
            var buildings = (from d in context.Departments
                             group d by d.Building into g // Группируем
                             where g.Sum(d => d.Financing) > sum // Фильтруем
                             select g.Key) // Выбираем ключ
                            .ToList();
            return buildings;
        }

        //2. Вывести названия групп 5 - го курса...
        public static IEnumerable<string> GetGroupsByAmountOfLectures(DbContext context, int groupYear, int amountLessons, string departmentName, DateTime weekStart, DateTime weekEnd)
        {
            var groups = from g in context.Groups
                         where g.Year == groupYear &&
                               g.DepartmentNav.Name == departmentName &&
                               g.GroupsLecturesNav.Count(gl => gl.LectureNav.Date >= weekStart &&
                                                               gl.LectureNav.Date <= weekEnd) > amountLessons
                         select g.Name;

            return groups;
        }

        //3. Вывести названия групп, имеющих рейтинг... больше, чем «D221».
        public static IEnumerable<dynamic> GetGroupsWithRatingHigherThen(DbContext context, string groupName)
        {
            var targetRating = (from gs in context.GroupStudens
                                where gs.GroupNav.Name == groupName
                                select (int?)gs.StudentNav.Rating)
                               .Average() ?? 0;

            var allGroupsWithRatings = (from g in context.Groups
                                        select new
                                        {
                                            Name = g.Name,
                                            AvgRating = g.GroupsStudentsNav.Select(gs => (int?)gs.StudentNav.Rating).Average() ?? 0
                                        }).ToList(); // <-- Материализация

            var filteredGroups = from g in allGroupsWithRatings
                                 where g.AvgRating > targetRating
                                 select g;

            return filteredGroups;
        }

        //4. Вывести фамилии и имена преподавателей, ставка которых выше средней.
        public static IEnumerable<dynamic> GetTeachersNamesAndSurnames(DbContext context)
        {
            var avgProfSalary = (from t in context.Teachers
                                 where t.IsProfessor
                                 select (decimal?)t.Salary)
                                .Average() ?? 0;
            var teachers = (from t in context.Teachers
                            where t.Salary > avgProfSalary
                            select new { t.Surname, t.Name, t.Salary })
                           .ToList();

            return teachers;
        }

        //5. Вывести названия групп, у которых больше одного куратора.
        public static IEnumerable<dynamic> GetGroupsWithMoreThanCurators(DbContext context, int curatorCount)
        {
            var groups = (from g in context.Groups
                          where g.GroupsCuratorsNav.Count() > curatorCount
                          select new { g.Name, Count = g.GroupsCuratorsNav.Count() })
                         .ToList();

            return groups;
        }

        //6. Вывести названия групп, имеющих рейтинг... меньше, чем мин. 5 курса.
        public static IEnumerable<dynamic> GetGroupsWithRatingLowerThanMinForYear(DbContext context, int year)
        {
            var yearRatings = (from g in context.Groups
                               where g.Year == year
                               select new
                               {
                                   AvgRating = g.GroupsStudentsNav.Select(gs => (int?)gs.StudentNav.Rating).Average() ?? 0
                               }).ToList(); // <-- Материализация

            var minRatingForYear = (from r in yearRatings
                                    where r.AvgRating > 0
                                    select r.AvgRating)
                                   .DefaultIfEmpty(0)
                                   .Min();

            var allGroupsWithRatings = (from g in context.Groups
                                        select new
                                        {
                                            Name = g.Name,
                                            AvgRating = g.GroupsStudentsNav.Select(gs => (int?)gs.StudentNav.Rating).Average() ?? 0
                                        }).ToList(); // <-- Материализация

            var filteredGroups = from g in allGroupsWithRatings
                                 where g.AvgRating > 0 &&
                                       g.AvgRating < minRatingForYear
                                 select g;

            return filteredGroups;
        }


        public static IEnumerable<Faculties> GetProcedureFacultiesWithHigherFinancingThan(DbContext context, string facultyName)
        {
            var faculties = context.Faculties
                .FromSqlRaw("EXEC sp_GetFaciltiesWithHigherFinancingThan {0}", facultyName)
                .ToList();

            return faculties;
        }

        public static IEnumerable<dynamic> GetTopTeacherPerSubject(DbContext context)
        {
            var grouped = (from l in context.Lectures
                           group l by new { l.SubjectId, l.TeacherId } into g
                           select new
                           {
                               SubjectName = g.First().Subject.Name,
                               TeacherFullName = g.First().Teacher.Name + " " + g.First().Teacher.Surname,
                               LectureCount = g.Count()
                           }).ToList(); // <-- Материализация

            var topTeachers = (from x in grouped
                               group x by x.SubjectName into g
                               select g.OrderByDescending(x => x.LectureCount).First())
                              .ToList();

            return topTeachers;
        }

        public static IEnumerable<dynamic> GetSubjectWithLeastLectures(DbContext context)
        {

            var subject = (from s in context.Subjects
                           orderby s.LecturesNav.Count()
                           select new { s.Name, Count = s.LecturesNav.Count() })
                          .Take(1) // <-- Метод расширения
                          .ToList();

            return subject;
        }
        public static IEnumerable<dynamic> GetStudentAndSubjectCountForDept(DbContext context, string deptName)
        {
            var stats = (from d in context.Departments
                         where d.Name == deptName
                         select new
                         {
                             // Студенты:
                             StudentCount = d.GroupNav
                                             .SelectMany(g => g.GroupsStudentsNav)
                                             .Select(gs => gs.StudentId)
                                             .Distinct()
                                             .Count(),

                             // Дисциплины:
                             SubjectCount = d.GroupNav
                                             .SelectMany(g => g.GroupsLecturesNav)
                                             .Select(gl => gl.LectureNav.SubjectId)
                                             .Distinct()
                                             .Count()
                         })
                        .Take(1)
                        .ToList();

            return stats;
        }
    }
}
