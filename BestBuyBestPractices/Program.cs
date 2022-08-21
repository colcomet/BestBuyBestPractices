using System;
using System.Data;
using System.IO;
using MySql.Data.MySqlClient;
using Microsoft.Extensions.Configuration;
using System.ComponentModel;

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
            var deptRepo = new DapperDepartmentRepository(conn);

            OutputDepartments(deptRepo);

            Console.WriteLine("\nEnter new department name (blank to skip)");
            string newDept = Console.ReadLine();
            if (!string.IsNullOrWhiteSpace(newDept))
            {
                deptRepo.InsertDepartment(newDept);
                OutputDepartments(deptRepo);
            }

            Console.WriteLine("\nPress enter to view products");
            Console.ReadLine();

            var prodRepo = new DapperProductRepository(conn);

            OutputProducts(prodRepo);
            
            Console.WriteLine("Enter new product name (blank to skip)");
            var newProductName = Console.ReadLine();
            if (!string.IsNullOrWhiteSpace(newProductName))
            {
                var newProductPrice = ParseDoubleFromConsole("Enter new product price");
                var newProductCategoryID = 10;
                prodRepo.CreateProduct(newProductName, newProductPrice, newProductCategoryID);
                OutputProducts(prodRepo);
            }

            var deleteProductID = ParseIntFromConsole("Enter ProductID to delete (<= 0 to skip)");
            if(deleteProductID > 0)
            {
                prodRepo.DeleteProduct(deleteProductID);
                OutputProducts(prodRepo);
            }
        }

        private static void OutputDepartments(DapperDepartmentRepository deptRepo)
        {
            var depts = deptRepo.GetAllDepartments();
            Console.WriteLine("\nDepartments:");
            foreach (var item in depts)
            {
                Console.WriteLine($"Department ID: {item.DepartmentID} Name: {item.Name}");
            }
        }

        private static void OutputProducts(DapperProductRepository prodRepo)
        {
            var products = prodRepo.GetAllProducts();
            Console.WriteLine("\nProducts:");
            foreach (var product in products)
            {
                Console.WriteLine($"Product: {product.ProductID} Name: {product.Name} Price: {product.Price:c} CategoryID: {product.CategoryID} OnSale: {(product.OnSale ? "yes" : "no")} StockLevel: {product.StockLevel}");
            }
        }

        private static double ParseDoubleFromConsole(string prompt)
        {
            bool numberWasEntered = false;
            double numberEntered = 0;
            do
            {
                Console.WriteLine(prompt);
                numberWasEntered = double.TryParse(Console.ReadLine(), out numberEntered);
            } while (!numberWasEntered);
            return numberEntered;

        }
        private static int ParseIntFromConsole(string prompt)
        {
            bool numberWasEntered = false;
            int numberEntered = 0;
            do
            {
                Console.WriteLine(prompt);
                numberWasEntered = int.TryParse(Console.ReadLine(), out numberEntered);
            } while (!numberWasEntered);
            return numberEntered;

        }
    }
}
