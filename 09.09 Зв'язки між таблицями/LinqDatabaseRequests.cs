using Microsoft.EntityFrameworkCore;
using ModelsCreating.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace DataBaseModels
{
    internal static class LinqDatabaseRequests
    {
        //1.Вывести номера корпусов, если суммарный фонд финансирования расположенных в них кафедр превышает 100000.
        //Departments - building(корпус)
        public static IEnumerable<int> GetBuildingsWithFinancingAbove(DbContext context, decimal sum)
        {
                var buildings = context.Departments
                .GroupBy(d => d.Building) // группируем по номеру корпуса
                .Where(g => g.Sum(d => d.Financing) > sum) // фильтруем по сумме
                .Select(g => g.Key) // выбираем номер корпуса 
                .ToList();
            return buildings;
        }

        //2.Вывести названия групп 5 - го курса кафедры «Software Development», которые имеют более 10 пар в первую неделю.(в первую неделю чего?)
        public static IEnumerable<string> GetGroupsByAmountOfLectures(DbContext context, int groupYear, int amountLessons, string departmentName, DateTime weekStart, DateTime weekEnd)
        {
            var groups = context.Groups
                .Where(g => g.Year == groupYear &&//фильтруем по нужному году обучения
                g.DepartmentNav.Name == departmentName &&//в нужной кафедре
                g.GroupsLecturesNav.Count(gl => gl.LectureNav.Date >= weekStart &&
                gl.LectureNav.Date <= weekEnd) > amountLessons)//фильтруем первую неделю
                .Select(g => g.Name);
            return groups;
        }

        //3.Вывести названия групп, имеющих рейтинг(средний рейтинг всех студентов группы) больше, чем рейтинг группы «D221».
        public static IEnumerable<dynamic> GetGroupsWithRatingHigherThen(DbContext context, string groupName)
        {
            var targetRating = context.GroupStudens
        .Where(gs => gs.GroupNav.Name == groupName)
        .Select(gs => (int?)gs.StudentNav.Rating)
        .Average()
        ?? 0;

            var allGroupsWithRatings = context.Groups
        .Select(g => new {
            Name = g.Name,
            AvgRating = g.GroupsStudentsNav.Select(gs => (int?)gs.StudentNav.Rating).Average() ?? 0
        })
                .ToList(); 

            var filteredGroups = allGroupsWithRatings
                .Where(g => g.AvgRating > targetRating);

            return filteredGroups;
        }

        //4.Вывести фамилии и имена преподавателей, ставка которых выше средней ставки профессоров.
        public static IEnumerable<dynamic> GetTeachersNamesAndSurnames(DbContext context)
        {
            var avgProfSalary = context.Teachers
                .Where(t => t.IsProfessor)
                .Select(t => (decimal?)t.Salary)
                .Average() ?? 0;

            var teachers = context.Teachers
                .Where(t=> t.Salary > avgProfSalary)
                .Select(t => new
                {
                    t.Surname, t.Name, t.Salary
                })
                .ToList();
            return teachers;
        }
        //5.Вывести названия групп, у которых больше одного куратора.
        public static IEnumerable<dynamic> GetGroupsWithMoreThanCurators(DbContext context, int curatorCount)
        {
            var groups = context.Groups
                .Where(g => g.GroupsCuratorsNav.Count() > curatorCount)
                .Select(g => new { g.Name, Count = g.GroupsCuratorsNav.Count() })
                .ToList();

            return groups;
        }

        //6.Вывести названия групп, имеющих рейтинг(средний рейтинг всех студентов группы) меньше, чем минимальный рейтинг групп 5 - го курса.
        public static IEnumerable<dynamic> GetGroupsWithRatingLowerThanMinForYear(DbContext context, int year)
        {
            // средние рейтинги для 5 курса
            var yearRatings = context.Groups
                .Where(g => g.Year == year)
                .Select(g => new { //средний рейтинг
                    AvgRating = g.GroupsStudentsNav.Select(gs => (int?)gs.StudentNav.Rating).Average() ?? 0
                })
                .ToList(); // Материализуем

            var minRatingForYear = yearRatings
                .Where(r => r.AvgRating > 0) // Игнорируем пустые группы
                .Select(r => r.AvgRating)
                .DefaultIfEmpty(0) // Если групп 5 курса нет, мин. = 0
                .Min();

            // рейтинги ВСЕХ групп 
            var allGroupsWithRatings = context.Groups
                .Select(g => new {
                    Name = g.Name,
                    AvgRating = g.GroupsStudentsNav.Select(gs => (int?)gs.StudentNav.Rating).Average() ?? 0
                })
                .ToList();

            // Фильтруем 
            var filteredGroups = allGroupsWithRatings
                .Where(g => g.AvgRating > 0 && // Группа не пустая
                            g.AvgRating < minRatingForYear); // И рейтинг меньше мин.

            return filteredGroups;
        }

        //7.Вывести названия факультетов, суммарный фонд финансирования кафедр которых больше суммарного фонда финансирования кафедр факультета «Com­puter Science».  Для этого запроса напишите процедуру, и вызовите  в коде процедуру
        // Этот метод ВЫЗЫВАЕТ процедуру
        public static IEnumerable<dynamic> GetProcedureFacultiesWithHigherFinancingThan(DbContext context, string facultyName)
        {
            var faculties = context.Faculties
                .FromSqlRaw("EXEC sp_GetFaciltiesWithHigherFinancingThan {0}", facultyName)
                .ToList();

            return faculties;
        }

        //8.Вывести названия дисциплин и полные имена преподавателей, читающих наибольшее количество лекций по ним.
        public static IEnumerable<dynamic> GetTopTeacherPerSubject(DbContext context)
        {
            // Группируем лекции 
            var grouped = context.Lectures
                .GroupBy(l => new { l.SubjectId, l.TeacherId }) 
                .Select(g => new {
                    // Получаем имена из первого элемента группы
                    SubjectName = g.First().Subject.Name,
                    TeacherFullName = g.First().Teacher.Name + " " + g.First().Teacher.Surname,
                    LectureCount = g.Count()
                })
                .ToList(); // Материализуем

            // Теперь группируем по Предмету в C#
            var topTeachers = grouped
                .GroupBy(x => x.SubjectName)
                // В каждой группе (Предмет) сортируем по убыванию лекций и берем 1-го
                .Select(g => g.OrderByDescending(x => x.LectureCount).First())
                .ToList();

            return topTeachers;
        }
        //9.Вывести название дисциплины, по которому читается меньше всего лекций.
        public static IEnumerable<dynamic> GetSubjectWithLeastLectures(DbContext context)
        {
            // Используем .Take(1) чтобы вернуть IEnumerable для PrintResults
            var subject = context.Subjects
                .OrderBy(s => s.LecturesNav.Count()) // Сортируем по кол-ву лекций
                .Select(s => new { s.Name, Count = s.LecturesNav.Count() })
                .Take(1) // Берем первого
                .ToList();

            return subject;
        }
        //10.Вывести количество студентов и читаемых дисциплин на кафедре «Software Development»
        public static IEnumerable<dynamic> GetStudentAndSubjectCountForDept(DbContext context, string deptName)
        {
            var stats = context.Departments
                .Where(d => d.Name == deptName)
                .Select(d => new {
                    // Студенты: Группы -> СтудентыГрупп -> ID Студента -> Уникальные -> Считаем
                    StudentCount = d.GroupNav
                                    .SelectMany(g => g.GroupsStudentsNav)
                                    .Select(gs => gs.StudentId)
                                    .Distinct()
                                    .Count(),

                    // Дисциплины: Группы -> ЛекцииГрупп -> Лекция -> ID Предмета -> Уникальные -> Считаем
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
