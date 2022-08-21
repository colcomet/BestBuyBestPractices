using System;
using Dapper;
using System.Collections.Generic;
using System.Data;
using System.Text;

namespace BestBuyBestPractices
{
    public class DapperProductRepository : IProductRepository
    {
        private readonly IDbConnection _connection;

        public DapperProductRepository(IDbConnection connection)
        {
            _connection = connection;
        }
        public IEnumerable<Product> GetAllProducts()
        {
            return _connection.Query<Product>("SELECT * FROM Products");
        }
        public void CreateProduct(string newName, double newPrice, int newCategoryID)
        {
            _connection.Execute($"INSERT INTO Products (Name, Price, CategoryID, OnSale, StockLevel)" +
                $"VALUES (@name, @price, @categoryID, @onSale, @stockLevel);",
                new { name = newName, price = newPrice, categoryID = newCategoryID, onSale = 0, stockLevel = "0" });
        }
        public void UpdateProduct(Product product)
        {
            _connection.Execute($"UPDATE Products" +
                $"SET Name=@name, Price=@price, CategoryID=@categoryId, OnSale=@onSale, StockLevel=@stockLevel" +
                $"WHERE ProductID=@productID;",
                new
                {
                    name = product.Name,
                    price = product.Price,
                    categoryID = product.CategoryID,
                    onSale = product.OnSale,
                    stockLevel = product.StockLevel,
                    ProductID = product.ProductID
                });
        }
        public void DeleteProduct(int productID)
        {
            _connection.Execute($"DELETE FROM Sales WHERE ProductID=@productID;" +
                $"DELETE FROM Reviews WHERE ProductID=@productID;" +
                $"DELETE FROM Products WHERE ProductID=@productID;",
                new { productID = productID });
        }
    }
}
