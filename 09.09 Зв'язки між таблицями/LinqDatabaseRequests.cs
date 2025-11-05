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
            // --- Шаг 1 (LINQ-запрос) ---
            var targetRating = (from gs in context.GroupStudens
                                where gs.GroupNav.Name == groupName
                                select (int?)gs.StudentNav.Rating)
                               .Average() ?? 0;

            // --- Шаг 2 (LINQ-запрос) ---
            var allGroupsWithRatings = (from g in context.Groups
                                        select new
                                        {
                                            Name = g.Name,
                                            AvgRating = g.GroupsStudentsNav.Select(gs => (int?)gs.StudentNav.Rating).Average() ?? 0
                                        }).ToList(); // <-- Материализация

            // --- Шаг 3 (LINQ to Objects) ---
            var filteredGroups = from g in allGroupsWithRatings
                                 where g.AvgRating > targetRating
                                 select g;

            return filteredGroups;
        }

        //4. Вывести фамилии и имена преподавателей, ставка которых выше средней.
        public static IEnumerable<dynamic> GetTeachersNamesAndSurnames(DbContext context)
        {
            // --- Шаг 1 (LINQ-запрос) ---
            var avgProfSalary = (from t in context.Teachers
                                 where t.IsProfessor
                                 select (decimal?)t.Salary)
                                .Average() ?? 0;

            // --- Шаг 2 (LINQ-запрос) ---
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
            // --- Шаг 1 (LINQ-запрос) ---
            var yearRatings = (from g in context.Groups
                               where g.Year == year
                               select new
                               {
                                   AvgRating = g.GroupsStudentsNav.Select(gs => (int?)gs.StudentNav.Rating).Average() ?? 0
                               }).ToList(); // <-- Материализация

            // --- Шаг 2 (LINQ to Objects) ---
            var minRatingForYear = (from r in yearRatings
                                    where r.AvgRating > 0
                                    select r.AvgRating)
                                   .DefaultIfEmpty(0)
                                   .Min();

            // --- Шаг 3 (LINQ-запрос) ---
            var allGroupsWithRatings = (from g in context.Groups
                                        select new
                                        {
                                            Name = g.Name,
                                            AvgRating = g.GroupsStudentsNav.Select(gs => (int?)gs.StudentNav.Rating).Average() ?? 0
                                        }).ToList(); // <-- Материализация

            // --- Шаг 4 (LINQ to Objects) ---
            var filteredGroups = from g in allGroupsWithRatings
                                 where g.AvgRating > 0 &&
                                       g.AvgRating < minRatingForYear
                                 select g;

            return filteredGroups;
        }

        //7. Вывести названия факультетов... (Прямой SQL - FromSqlRaw)
        // (Этот метод УЖЕ использует прямой SQL, как ты и просила)
        public static IEnumerable<Faculties> GetProcedureFacultiesWithHigherFinancingThan(DbContext context, string facultyName)
        {
            var faculties = context.Faculties
                .FromSqlRaw("EXEC sp_GetFaciltiesWithHigherFinancingThan {0}", facultyName)
                .ToList();

            return faculties;
        }

        // (Вспомогательный метод для Запроса 7)
        public static string GetProcedureCreationScript()
        {
            return @"
            CREATE OR ALTER PROCEDURE sp_GetFaciltiesWithHigherFinancingThan
                @TargetFacultyName NVARCHAR(100)
            AS
            BEGIN
                SET NOCOUNT ON;
                DECLARE @TargetFinancing DECIMAL(19, 4);
                SELECT @TargetFinancing = SUM(ISNULL(d.department_financing, 0))
                FROM faculties f
                LEFT JOIN departments d ON f.faculty_id = d.faculty_id
                WHERE f.faculty_name = @TargetFacultyName;
                SET @TargetFinancing = ISNULL(@TargetFinancing, 0);

                SELECT 
                    f.faculty_id AS faculty_id, 
                    f.faculty_name AS faculty_name
                FROM faculties f
                LEFT JOIN departments d ON f.faculty_id = d.faculty_id
                GROUP BY f.faculty_id, f.faculty_name
                HAVING ISNULL(SUM(d.department_financing), 0) > @TargetFinancing
                   AND f.faculty_name != @TargetFacultyName;
            END";
        }


        //8. Вывести названия дисциплин и полных имен преподавателей...
        public static IEnumerable<dynamic> GetTopTeacherPerSubject(DbContext context)
        {
            // --- Шаг 1 (LINQ-запрос) ---
            var grouped = (from l in context.Lectures
                           group l by new { l.SubjectId, l.TeacherId } into g
                           select new
                           {
                               SubjectName = g.First().Subject.Name,
                               TeacherFullName = g.First().Teacher.Name + " " + g.First().Teacher.Surname,
                               LectureCount = g.Count()
                           }).ToList(); // <-- Материализация

            // --- Шаг 2 (LINQ to Objects) ---
            var topTeachers = (from x in grouped
                               group x by x.SubjectName into g
                               select g.OrderByDescending(x => x.LectureCount).First())
                              .ToList();

            return topTeachers;
        }

        //9. Вывести название дисциплины, по которому читается меньше всего лекций.
        public static IEnumerable<dynamic> GetSubjectWithLeastLectures(DbContext context)
        {
            // .Take(1) не имеет прямого аналога в синтаксисе запросов,
            // поэтому мы комбинируем синтаксис запроса с методом расширения.
            var subject = (from s in context.Subjects
                           orderby s.LecturesNav.Count()
                           select new { s.Name, Count = s.LecturesNav.Count() })
                          .Take(1) // <-- Метод расширения
                          .ToList();

            return subject;
        }

        //10. Вывести количество студентов и читаемых дисциплин...
        public static IEnumerable<dynamic> GetStudentAndSubjectCountForDept(DbContext context, string deptName)
        {
            // Вложенные .SelectMany() и .Distinct().Count() крайне сложно
            // выразить в синтаксисе запросов, поэтому их оставляют как методы.
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
