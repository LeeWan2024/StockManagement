using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Transactions;

namespace StockManagementConsoleApp
{

    public class StockItem
    {
        public int ProductId { get; set; }
        public string Brand { get; set; }
        public double Cost { get; set; }
        public DateTime DateAdded { get; set; }
        public string Movement { get; set; }
        
    }
    public static class StaticData
    {
        public static ConsoleKey switchView = default;
        public static List<StockItem> StockTable { get; set; } = new List<StockItem>();

        public static void GenerateRandomTransactions(int count)
        {
            Random random = new Random();
            int startId = random.Next(10000);
            DateTime today = DateTime.Today;

            for (int i = 0; i < count; i++)
            {
                int productId = startId + i;
                string[] brands = { "Other", "Apple", "Samsung" };
                string brand = brands[random.Next(brands.Length)];
                double cost = Math.Round(random.Next(500, 1001) / 10.0) * 10;
                DateTime dateAdded = today.AddDays(-random.Next(1, 7));
                int movementRnd = random.Next(3);
                StaticData.StockTable.Add(new StockItem { ProductId = productId, Brand = brand, Cost = cost, DateAdded = dateAdded, Movement = "Stock in" });
                if (movementRnd == 2)
                {
                    StaticData.StockTable.Add(new StockItem { ProductId = productId, Brand = brand, Cost = cost, DateAdded = dateAdded.AddDays(random.Next(1, 7)), Movement = "Stock out" });
                }
            }

        }
    }





}
