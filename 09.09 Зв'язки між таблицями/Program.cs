using Microsoft.EntityFrameworkCore;
using ModelsCreating.Models;
using Dapper;
using System.Configuration;

namespace DataBaseModels
{

    internal class Program
    {
        static void Main(string[] args)
        {
            #region DBFillCOMMENTED

            //using (var context = new DbContext())
            //{
            //    //context.Database.EnsureDeleted();
            //    context.Database.EnsureCreated();

            //    if (context.Faculties.Any())
            //    {
            //        Console.WriteLine("База данных уже содержит данные.");
            //        return; // Выходим
            //    }

            //    Console.WriteLine("Заполняем базу данных расширенным набором данных...");

            //    // независимые сущности

            //    // Факультеты
            //    var facultyCS = new Faculties { Name = "Computer Science" }; 
            //    var facultyEco = new Faculties { Name = "Экономики и Менеджмента" };
            //    var facultyMed = new Faculties { Name = "Медицинский Факультет" }; 

            //    // Кураторы
            //    var curator1 = new Curators { Name = "Марина", Surname = "Петренко" };
            //    var curator2 = new Curators { Name = "Олег", Surname = "Ковальчук" };

            //    // Студенты
            //    var student1 = new Students { Name = "Иван", Surname = "Иванов", Rating = 4 };
            //    var student2 = new Students { Name = "Анна", Surname = "Сидорова", Rating = 5 };
            //    var student3 = new Students { Name = "Петр", Surname = "Петров", Rating = 3 };  
            //    var student4 = new Students { Name = "Ольга", Surname = "Ольгина", Rating = 5 }; 
            //    var student5 = new Students { Name = "Алекс", Surname = "Котов", Rating = 4 }; 
            //    var student6 = new Students { Name = "Мария", Surname = "Волк", Rating = 4 };  

            //    // Предметы
            //    var subject1 = new Subject { Name = "Базы данных" };
            //    var subject2 = new Subject { Name = "Программирование C#" };
            //    var subject3 = new Subject { Name = "Микроэкономика" };

            //    // Преподаватели
            //    var teacher1 = new Teachers { Name = "Виктор", Surname = "Павлов", Salary = 25000, IsProfessor = true };
            //    var teacher2 = new Teachers { Name = "Ирина", Surname = "Волкова", Salary = 18000, IsProfessor = false };
            //    var teacher3 = new Teachers { Name = "Дмитрий", Surname = "Соколов", Salary = 22000, IsProfessor = false };                
            //    var teacher4 = new Teachers { Name = "Елена", Surname = "Иванова", Salary = 35000, IsProfessor = true }; 
            //    var teacher5 = new Teachers { Name = "Сэм", Surname = "Кинг", Salary = 32000, IsProfessor = false };

            //    //зависимые сущности 

            //    // Кафедры
            //    var deptSE = new Departments { Name = "Software Development", Building = 1, Financing = 100000, FacultyNav = facultyCS }; 
            //    var deptCyber = new Departments { Name = "Кибербезопасность", Building = 2, Financing = 80000, FacultyNav = facultyCS };
            //    var deptFin = new Departments { Name = "Финансы", Building = 3, Financing = 70000, FacultyNav = facultyEco };               
            //    var deptAI = new Departments { Name = "Искусственный Интеллект", Building = 2, Financing = 30000, FacultyNav = facultyCS }; 
            //    var deptBioMed = new Departments { Name = "Биомедицина", Building = 4, Financing = 250000, FacultyNav = facultyMed }; 

            //    // Лекции
            //    var lecture1 = new Lectures { Date = new DateTime(2025, 10, 20), Subject = subject1, Teacher = teacher1 }; 
            //    var lecture2 = new Lectures { Date = new DateTime(2025, 10, 21), Subject = subject2, Teacher = teacher2 }; 
            //    var lecture3 = new Lectures { Date = new DateTime(2025, 10, 22), Subject = subject3, Teacher = teacher3 }; 

            //    var lecture_db2 = new Lectures { Date = new DateTime(2025, 10, 23), Subject = subject1, Teacher = teacher1 };

            //    var lecturesForT2 = new List<Lectures>();
            //    for (int i = 0; i < 11; i++)
            //    {
            //        lecturesForT2.Add(new Lectures { Date = new DateTime(2025, 9, 2 + (i % 5)), Subject = subject2, Teacher = teacher2 });
            //    }

            //    // Группы
            //    var groupP11 = new Groups { Name = "P-11", Year = 1, DepartmentNav = deptSE };
            //    var groupP12 = new Groups { Name = "P-12", Year = 1, DepartmentNav = deptSE };
            //    var groupD221 = new Groups { Name = "D221", Year = 2, DepartmentNav = deptCyber }; 
            //    var groupF31 = new Groups { Name = "F-31", Year = 3, DepartmentNav = deptFin };

            //    var groupSD51 = new Groups { Name = "SD-51", Year = 5, DepartmentNav = deptSE }; 

            //    // табл связей

            //    // GroupStudents 
            //    var gs1 = new GroupStudents { GroupNav = groupP11, StudentNav = student1 };
            //    var gs2 = new GroupStudents { GroupNav = groupP11, StudentNav = student2 }; 
            //    var gs3 = new GroupStudents { GroupNav = groupD221, StudentNav = student3 }; 
            //    var gs4 = new GroupStudents { GroupNav = groupF31, StudentNav = student4 };   
            //    var gs5 = new GroupStudents { GroupNav = groupSD51, StudentNav = student5 };
            //    var gs6 = new GroupStudents { GroupNav = groupSD51, StudentNav = student6 };

            //    // GroupsCurators 
            //    var gc1 = new GroupsCurators { GroupNav = groupP11, CuratorNav = curator1 };
            //    var gc2 = new GroupsCurators { GroupNav = groupP12, CuratorNav = curator1 };
            //    var gc3 = new GroupsCurators { GroupNav = groupD221, CuratorNav = curator2 };
            //    var gc4 = new GroupsCurators { GroupNav = groupF31, CuratorNav = curator2 };              
            //    var gc5 = new GroupsCurators { GroupNav = groupP11, CuratorNav = curator2 }; 

            //    // GroupLectures 
            //    var gl1 = new GroupLectures { GroupNav = groupP11, LectureNav = lecture1 }; 
            //    var gl2 = new GroupLectures { GroupNav = groupP12, LectureNav = lecture1 }; 
            //    var gl3 = new GroupLectures { GroupNav = groupP11, LectureNav = lecture2 }; 
            //    var gl4 = new GroupLectures { GroupNav = groupD221, LectureNav = lecture2 }; 
            //    var gl5 = new GroupLectures { GroupNav = groupF31, LectureNav = lecture3 };  // F-31 -> Eco
            //    var gl6 = new GroupLectures { GroupNav = groupP11, LectureNav = lecture_db2 }; 

            //    var gl_for_T2 = new List<GroupLectures>();
            //    foreach (var lec in lecturesForT2)
            //    {
            //        gl_for_T2.Add(new GroupLectures { GroupNav = groupSD51, LectureNav = lec });
            //    }

            //    // сохранеие

            //    // независимые
            //    context.Faculties.AddRange(facultyCS, facultyEco, facultyMed);
            //    context.Curators.AddRange(curator1, curator2);
            //    context.Students.AddRange(student1, student2, student3, student4, student5, student6);
            //    context.Subjects.AddRange(subject1, subject2, subject3);
            //    context.Teachers.AddRange(teacher1, teacher2, teacher3, teacher4, teacher5);

            //    // зависимые 
            //    context.Departments.AddRange(deptSE, deptCyber, deptFin, deptAI, deptBioMed);
            //    context.Lectures.AddRange(lecture1, lecture2, lecture3, lecture_db2);
            //    context.Lectures.AddRange(lecturesForT2);
            //    context.Groups.AddRange(groupP11, groupP12, groupD221, groupF31, groupSD51);


            //    context.GroupStudens.AddRange(gs1, gs2, gs3, gs4, gs5, gs6); 
            //    context.GroupsCurators.AddRange(gc1, gc2, gc3, gc4, gc5);
            //    context.GroupLectures.AddRange(gl1, gl2, gl3, gl4, gl5, gl6);
            //    context.GroupLectures.AddRange(gl_for_T2);

            //    // Сохраняем
            //    try
            //    {
            //        context.SaveChanges();
            //        Console.WriteLine("Данные успешно сохранены в БД.");
            //    }
            //    catch (Exception ex)
            //    {
            //        Console.WriteLine($"Ошибка при сохранении данных: {ex.Message}");
            //        if (ex.InnerException != null)
            //        {
            //            Console.WriteLine($"Внутренняя ошибка: {ex.InnerException.Message}");
            //        }
            //    }
            //}
            #endregion

            #region Переменные для заданий

            string connectionStr = "";
            try
            {
                connectionStr = ConfigurationManager.ConnectionStrings["DatabaseConnection"].ConnectionString;
            }
            catch (Exception ex)
            {
                Console.WriteLine($"Не удалось прочитать строку подключения из App.config." + ex.Message);
                return;//ну а что еще делать если у меня опять сервер изначально упал хд
            }

            // --- Задание 1 ---
            var num1 = 100000;
            var task1 = $"\n1. Вывести номера корпусов, если суммарный фонд финансирования расположенных в них кафедр превышает {num1}";

            // --- Задание 2 ---
            var courseT2 = 5;
            var departmentNameT2 = "Software Development";
            var lessonsNumT2 = 10;
            var weekStartT2 = new DateTime(2025, 9, 1);
            var weekEndT2 = new DateTime(2025, 9, 7);//не понимаю что такое первая неделя. Первая неделя чего?
            var task2 = $"\n2. Вывести названия групп {courseT2}-го курса кафедры «{departmentNameT2}», которые имеют более {lessonsNumT2} пар в первую неделю.";

            // --- Задание 3 ---
            var groupNameT3 = "D221";
            var task3 = $"\n3. Вывести названия групп, имеющих рейтинг (средний) больше, чем рейтинг группы «{groupNameT3}».";

            // --- Задание 4 ---
            var task4 = $"\n4. Вывести фамилии и имена преподавателей, ставка которых выше средней ставки профессоров.";

            // --- Задание 5 ---
            var curatorCountT5 = 1;
            var task5 = $"\n5. Вывести названия групп, у которых больше {curatorCountT5} куратора.";

            // --- Задание 6 ---
            var yearT6 = 5;
            var task6 = $"\n6. Вывести названия групп, имеющих рейтинг (средний) меньше, чем минимальный рейтинг групп {yearT6}-го курса.";

            // --- Задание 7 ---
            var facultyNameT7 = "Computer Science";
            var task7 = $"\n7. Вывести названия факультетов, суммарный фонд финансирования кафедр которых больше суммарного фонда финансирования кафедр факультета {facultyNameT7}.  Для этого запроса напишите процедуру, и вызовите  в коде процедуру";

            // --- Задание 8 ---
            var task8 = $"\n8. Вывести названия дисциплин и полные имена преподавателей, читающих наибольшее количество лекций по ним.";

            // --- Задание 9 ---
            var task9 = $"\n9. Вывести название дисциплины, по которому читается меньше всего лекций.";

            // --- Задание 10 ---
            var deptNameT10 = "Software Development";
            var task10 = $"\n10. Вывести количество студентов и читаемых дисциплин на кафедре «{deptNameT10}»";

            // --- Задание 11 ---
            var subjectT11 = "Новая SQL Дисциплина";
            var newSubjectT11 = "Обновленная SQL Дисциплина";
            var task11 = $"Выполнить (через Raw SQL) Вставку, Изменение и Удаление дисциплины.";


            #endregion
            try
            {

                using (var context = new DbContext())//еф кор находит строку подключения через он конфигуринг
                {

                    #region ExtensionMethods
                    #region 1. Корпуса с фондом > 100 000
                    try
                    {

                        context.Database.EnsureCreated();
                        var res = ExtensionMethods.GetBuildingsWithFinancingAbove(context, num1);

                        ConsolePrintClass.PrintResults(res, task1, b => Console.WriteLine($"Корпус {b}"));

                    }
                    catch (Exception ex)
                    {
                        Console.WriteLine(ex.Message);
                    }
                    #endregion
                    #region 2. Группы 5-го курса кафедры «Software Development», которые имеют более 10 пар в первую неделю.
                    try
                    {
                        var res2 = ExtensionMethods.GetGroupsByAmountOfLectures(context, courseT2, lessonsNumT2, departmentNameT2, weekStartT2, weekEndT2);
                        ConsolePrintClass.PrintResults(res2, task2, b => Console.WriteLine($"Название группы: {b}"));
                        #endregion
                    }
                    catch (Exception ex)
                    {
                        Console.WriteLine(ex.Message);
                    }
                    #region 3. Группы с рейтингом больше чем рейтинг группы «D221»
                    try
                    {
                        var res3 = ExtensionMethods.GetGroupsWithRatingHigherThen(context, groupNameT3);
                        ConsolePrintClass.PrintResults(res3, task3, b => Console.WriteLine($"Группа: {b.Name}, Рейтинг: {b.AvgRating:F2}"));
                    }
                    catch (Exception ex)
                    {
                        Console.WriteLine(ex.Message);
                    }
                    #endregion
                    #region 4. Фамилии и имена преподавателей, ставка которых выше средней ставки профессоров
                    try
                    {
                        var res4 = ExtensionMethods.GetTeachersNamesAndSurnames(context);
                        ConsolePrintClass.PrintResults(res4, task4, b => Console.WriteLine($"Преподаватель: {b.Surname} {b.Name}, Ставка: {b.Salary:C}"));
                    }
                    catch (Exception ex)
                    {
                        Console.WriteLine(ex.Message);
                    }

                    #endregion
                    #region 5. Названия групп, у которых больше одного куратора.
                    try
                    {
                        var res5 = ExtensionMethods.GetGroupsWithMoreThanCurators(context, curatorCountT5);
                        ConsolePrintClass.PrintResults(res5, task5, b => Console.WriteLine($"Группа: {b.Name} (Кураторов: {b.Count})"));
                    }
                    catch (Exception ex)
                    {
                        Console.WriteLine(ex.Message);
                    }

                    #endregion
                    #region 6. Названия групп, имеющих рейтинг меньше, чем минимальный рейтинг групп 5-го курса.
                    try
                    {
                        var res6 = ExtensionMethods.GetGroupsWithRatingLowerThanMinForYear(context, yearT6);
                        ConsolePrintClass.PrintResults(res6, task6, b => Console.WriteLine($"Группа: {b.Name}, Рейтинг: {b.AvgRating:F2}"));
                    }
                    catch (Exception ex)
                    {
                        Console.WriteLine(ex.Message);
                    }

                    #endregion
                    #region 7. Процедура - Названия факультетов, суммарный фонд финансирования кафедр которых больше суммарного фонда финансирования кафедр факультета «Com­puter Science».
                    try
                    {
                        var res7 = ExtensionMethods.GetProcedureFacultiesWithHigherFinancingThan(context, facultyNameT7);
                        ConsolePrintClass.PrintResults(res7, task7, b => Console.WriteLine($"Факультет: {b.Name}"));
                    }
                    catch (Exception ex)
                    {
                        Console.WriteLine(ex.Message);
                    }

                    #endregion
                    #region 8. Названия дисциплин и полные имена преподавателей, читающих наибольшее количество лекций по ним.
                    try
                    {
                        var res8 = ExtensionMethods.GetTopTeacherPerSubject(context);
                        ConsolePrintClass.PrintResults(res8, task8,
                        b => Console.WriteLine($"Дисциплина: {b.SubjectName}, Преподаватель: {b.TeacherFullName} (Лекций: {b.LectureCount})"));
                    }
                    catch (Exception ex)
                    {
                        Console.WriteLine(ex.Message);
                    }

                    #endregion
                    #region 9. Название дисциплины, по которому читается меньше всего лекций
                    try
                    {
                        var res9 = ExtensionMethods.GetSubjectWithLeastLectures(context);
                        ConsolePrintClass.PrintResults(res9, task9, b => Console.WriteLine($"Предмет: {b.Name} (Лекций: {b.Count})"));
                    }
                    catch (Exception ex)
                    {
                        Console.WriteLine(ex.Message);
                    }

                    #endregion
                    #region 10. Количество студентов и читаемых дисциплин на кафедре «Software Development»
                    try
                    {
                        var res10 = ExtensionMethods.GetStudentAndSubjectCountForDept(context, deptNameT10);
                        ConsolePrintClass.PrintResults(res10, task10, b => Console.WriteLine($"Студентов: {b.StudentCount}, Дисциплин: {b.SubjectCount}"));
                    }
                    catch (Exception ex)
                    {
                        Console.WriteLine(ex.Message);
                    }
                    #endregion

                    #endregion
                    #region LinQToBD
                    try
                    {
                        #region 1. Корпуса с фондом > 100 000
                        try
                        {

                            context.Database.EnsureCreated();
                            var res = LinqDatabaseRequests.GetBuildingsWithFinancingAbove(context, num1);

                            ConsolePrintClass.PrintResults(res, task1, b => Console.WriteLine($"Корпус {b}"));

                        }
                        catch (Exception ex)
                        {
                            Console.WriteLine(ex.Message);
                        }
                        #endregion
                        #region 2. Группы 5-го курса кафедры «Software Development», которые имеют более 10 пар в первую неделю.
                        try
                        {
                            var res2 = LinqDatabaseRequests.GetGroupsByAmountOfLectures(context, courseT2, lessonsNumT2, departmentNameT2, weekStartT2, weekEndT2);
                            ConsolePrintClass.PrintResults(res2, task2, b => Console.WriteLine($"Название группы: {b}"));
                            #endregion
                        }
                        catch (Exception ex)
                        {
                            Console.WriteLine(ex.Message);
                        }
                        #region 3. Группы с рейтингом больше чем рейтинг группы «D221»
                        try
                        {
                            var res3 = LinqDatabaseRequests.GetGroupsWithRatingHigherThen(context, groupNameT3);
                            ConsolePrintClass.PrintResults(res3, task3, b => Console.WriteLine($"Группа: {b.Name}, Рейтинг: {b.AvgRating:F2}"));
                        }
                        catch (Exception ex)
                        {
                            Console.WriteLine(ex.Message);
                        }
                        #endregion
                        #region 4. Фамилии и имена преподавателей, ставка которых выше средней ставки профессоров
                        try
                        {
                            var res4 = LinqDatabaseRequests.GetTeachersNamesAndSurnames(context);
                            ConsolePrintClass.PrintResults(res4, task4, b => Console.WriteLine($"Преподаватель: {b.Surname} {b.Name}, Ставка: {b.Salary:C}"));
                        }
                        catch (Exception ex)
                        {
                            Console.WriteLine(ex.Message);
                        }

                        #endregion
                        #region 5. Названия групп, у которых больше одного куратора.
                        try
                        {
                            var res5 = LinqDatabaseRequests.GetGroupsWithMoreThanCurators(context, curatorCountT5);
                            ConsolePrintClass.PrintResults(res5, task5, b => Console.WriteLine($"Группа: {b.Name} (Кураторов: {b.Count})"));
                        }
                        catch (Exception ex)
                        {
                            Console.WriteLine(ex.Message);
                        }

                        #endregion
                        #region 6. Названия групп, имеющих рейтинг меньше, чем минимальный рейтинг групп 5-го курса.
                        try
                        {
                            var res6 = LinqDatabaseRequests.GetGroupsWithRatingLowerThanMinForYear(context, yearT6);
                            ConsolePrintClass.PrintResults(res6, task6, b => Console.WriteLine($"Группа: {b.Name}, Рейтинг: {b.AvgRating:F2}"));
                        }
                        catch (Exception ex)
                        {
                            Console.WriteLine(ex.Message);
                        }

                        #endregion
                        #region 7. Процедура - Названия факультетов, суммарный фонд финансирования кафедр которых больше суммарного фонда финансирования кафедр факультета «Com­puter Science».
                        try
                        {
                            var res7 = LinqDatabaseRequests.GetProcedureFacultiesWithHigherFinancingThan(context, facultyNameT7);
                            ConsolePrintClass.PrintResults(res7, task7, b => Console.WriteLine($"Факультет: {b.Name}"));
                        }
                        catch (Exception ex)
                        {
                            Console.WriteLine(ex.Message);
                        }

                        #endregion
                        #region 8. Названия дисциплин и полные имена преподавателей, читающих наибольшее количество лекций по ним.
                        try
                        {
                            var res8 = LinqDatabaseRequests.GetTopTeacherPerSubject(context);
                            ConsolePrintClass.PrintResults(res8, task8,
                            b => Console.WriteLine($"Дисциплина: {b.SubjectName}, Преподаватель: {b.TeacherFullName} (Лекций: {b.LectureCount})"));
                        }
                        catch (Exception ex)
                        {
                            Console.WriteLine(ex.Message);
                        }

                        #endregion
                        #region 9. Название дисциплины, по которому читается меньше всего лекций
                        try
                        {
                            var res9 = LinqDatabaseRequests.GetSubjectWithLeastLectures(context);
                            ConsolePrintClass.PrintResults(res9, task9, b => Console.WriteLine($"Предмет: {b.Name} (Лекций: {b.Count})"));
                        }
                        catch (Exception ex)
                        {
                            Console.WriteLine(ex.Message);
                        }

                        #endregion
                        #region 10. Количество студентов и читаемых дисциплин на кафедре «Software Development»
                        try
                        {
                            var res10 = LinqDatabaseRequests.GetStudentAndSubjectCountForDept(context, deptNameT10);
                            ConsolePrintClass.PrintResults(res10, task10, b => Console.WriteLine($"Студентов: {b.StudentCount}, Дисциплин: {b.SubjectCount}"));
                        }
                        catch (Exception ex)
                        {
                            Console.WriteLine(ex.Message);
                        }
                        #endregion
                    }
                    catch (Exception ex)
                    {
                        Console.WriteLine(ex.Message);
                    }
                    #endregion

                    #region Task11. Выполнение в коде Вставку новой дисциплины, затем изменение ее названия, далее удалите выполняя команду SQL  со стороны SQL клиента
                    try
                    {
                        // 1. ВСТАВКА
                        int inserted = context.Database.ExecuteSqlInterpolated(
                            $"INSERT INTO subjects (subject_name) VALUES ({subjectT11})");
                        Console.WriteLine($"[SQL] 1. Добавлена '{subjectT11}' (строк: {inserted})");

                        // 2. ИЗМЕНЕНИЕ
                        int updated = context.Database.ExecuteSqlInterpolated(
                            $"UPDATE subjects SET subject_name = {newSubjectT11} WHERE subject_name = {subjectT11}");
                        Console.WriteLine($"[SQL] 2. Изменено на '{newSubjectT11}' (строк: {updated})");

                        // 3. УДАЛЕНИЕ
                        int deleted = context.Database.ExecuteSqlInterpolated(
                            $"DELETE FROM subjects WHERE subject_name = {newSubjectT11}");
                        Console.WriteLine($"[SQL] 3. Удалена '{newSubjectT11}' (строк: {deleted})");
                    }
                    catch (Exception ex)
                    {
                        Console.WriteLine($"[SQL] Ошибка: {ex.Message}");
                    }

                    #endregion
                }

            }
            catch (Exception ex)
            {
                Console.WriteLine(ex.Message + Environment.CommandLine);
            }
            #region DapperRequests
            try
            {
                // 1. CREATE
                var newTeacher = new Teachers
                {
                    Name = "Саша",
                    Surname = "Тартарова",
                    Salary = 15000,
                    IsProfessor = false
                };
                int newId = DapperRequestsTeachers.CreateTeacher(connectionStr, newTeacher);
                Console.WriteLine($"Создан преподаватель с ID: {newId}");

                // 2. READ 
                var fetchedTeacher = DapperRequestsTeachers.GetTeacherById(connectionStr, newId);
                Console.WriteLine($"Найден: {fetchedTeacher.Name} {fetchedTeacher.Surname}");

                // 3. UPDATE
                fetchedTeacher.Salary = 16000;
                fetchedTeacher.Name = "Олександра";
                int updatedRows = DapperRequestsTeachers.UpdateTeacher(connectionStr, fetchedTeacher);
                Console.WriteLine($"Обновлено строк: {updatedRows}");

                // 4. READ (Все)
                var allTeachers = DapperRequestsTeachers.GetAllTeachers(connectionStr);
                Console.WriteLine($"Всего преподавателей (после Update): {allTeachers.Count()}");

                // 5. DELETE
                int deletedRows = DapperRequestsTeachers.DeleteTeacher(connectionStr, newId);
                Console.WriteLine($"Удалено строк: {deletedRows}");

                // (Проверка, что удаление сработало)
                var allTeachersAfterDelete = DapperRequestsTeachers.GetAllTeachers(connectionStr);
                Console.WriteLine($"Всего преподавателей (после Delete): {allTeachersAfterDelete.Count()}");

            }
            catch (Exception ex)
            {
                Console.WriteLine($"Ошибка: {ex.Message}");
            }
        }

        #endregion


    }

}


