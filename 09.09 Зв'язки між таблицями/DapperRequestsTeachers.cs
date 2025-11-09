using ModelsCreating.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Dapper;
using Microsoft.Data.SqlClient;
namespace DataBaseModels
{
    public class DapperRequestsTeachers
    {
        public static int CreateTeacher(string connectionString, Teachers teacher)
        {
            string sql = @"INSERT INTO teachers (teacher_name, teacher_surname, teacher_salary, IsProfessor) 
                           VALUES (@Name, @Surname, @Salary, @IsProfessor);
                           SELECT SCOPE_IDENTITY();";

            using (var connection = new SqlConnection(connectionString))
            {
                int newId = connection.QuerySingle<int>(sql, teacher);
                return newId;
            }
        }


        public static IEnumerable<Teachers> GetAllTeachers(string connectionString)
        {
            string sql = @"SELECT 
                               teacher_id AS Id, 
                               teacher_name AS Name, 
                               teacher_surname AS Surname, 
                               teacher_salary AS Salary, 
                               IsProfessor
                           FROM teachers";

            using (var connection = new SqlConnection(connectionString))
            {
                var teachers = connection.Query<Teachers>(sql);
                return teachers;
            }
        }

        public static Teachers GetTeacherById(string connectionString, int id)
        {
            string sql = @"SELECT 
                               teacher_id AS Id, 
                               teacher_name AS Name, 
                               teacher_surname AS Surname, 
                               teacher_salary AS Salary, 
                               IsProfessor
                           FROM teachers
                           WHERE teacher_id = @TeacherId"; 

            using (var connection = new SqlConnection(connectionString))
            {
                var teacher = connection.QueryFirstOrDefault<Teachers>(sql, new { TeacherId = id });
                return teacher;
            }
        }

        public static int UpdateTeacher(string connectionString, Teachers teacher)
        {
            string sql = @"UPDATE teachers 
                           SET 
                               teacher_name = @Name, 
                               teacher_surname = @Surname, 
                               teacher_salary = @Salary, 
                               IsProfessor = @IsProfessor
                           WHERE teacher_id = @Id";

            using (var connection = new SqlConnection(connectionString))
            {
                int affectedRows = connection.Execute(sql, teacher);
                return affectedRows;
            }
        }

        public static int DeleteTeacher(string connectionString, int id)
        {
            string sql = "DELETE FROM teachers WHERE teacher_id = @Id";

            using (var connection = new SqlConnection(connectionString))
            {
                // Передаем анонимный объект
                int affectedRows = connection.Execute(sql, new { Id = id });
                return affectedRows;
            }
        }
    }
}