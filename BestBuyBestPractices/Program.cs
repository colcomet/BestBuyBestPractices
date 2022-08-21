using System;
using System.Data;
using System.IO;
using MySql.Data.MySqlClient;
using Microsoft.Extensions.Configuration;

namespace BestBuyBestPractices
{
    internal class Program
    {
        static void Main(string[] args)
        {
            var config = new ConfigurationBuilder()
                .SetBasePath(Directory.GetCurrentDirectory())
                .AddJsonFile("appsettings.json")
                .Build();

            string connString = config.GetConnectionString("DefaultConnection");
            IDbConnection conn = new MySqlConnection(connString);
            var repo = new DapperDepartmentRepository(conn);

            //Console.WriteLine("Enter new department name");
            //string newDept = Console.ReadLine();
            //repo.InsertDepartment(newDept);

            var depts = repo.GetAllDepartments();
            foreach (var item in depts)
            {
                Console.WriteLine($"Department ID: {item.DepartmentID} Name: {item.Name}");
            }

        }
    }
}
