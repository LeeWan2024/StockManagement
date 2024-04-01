using System;
using System.Collections;
using System.Collections.Generic;
using System.Linq;
using System.Security.Cryptography;
using System.Text;
using System.Threading.Tasks;

namespace StockManagementConsoleApp
{


    public class QueryLibrary
    {
        public List<StockItem> _stocks { get=>StaticData.StockTable; }
        public void TableAvailableStock()
        {
            Console.WriteLine("Available Stock Table:");
            Console.WriteLine("ID\tBrand\tCost\tDate added");
            var stockAvailableTable = from inTable in _stocks.Where(s => s.Movement == "Stock in").ToList()
                                      join outTable in _stocks.Where(s => s.Movement == "Stock out").ToList()
                                      on inTable.ProductId equals outTable.ProductId into outGroup
                                      from outItem in outGroup.DefaultIfEmpty()
                                      where outItem == null
                                      select inTable;
            foreach (var stockItem in stockAvailableTable)
            {
                Console.WriteLine($"{stockItem.ProductId}\t{stockItem.Brand}\t{stockItem.Cost}\t{stockItem.DateAdded.ToShortDateString()}");
            }
            Console.WriteLine($"Avaible stock count: {stockAvailableTable.Count()}");
        }
        public void TableHistoryStock()
        {
            var leftJoinTable = from inTable in _stocks.Where(s => s.Movement == "Stock in").ToList()
                                join outTable in _stocks.Where(s => s.Movement == "Stock out").ToList()
                                    on inTable.ProductId equals outTable.ProductId into outGroup
                                from outItem in outGroup.DefaultIfEmpty()
                                select new
                                {
                                    ProductId = inTable.ProductId,
                                    Brand = inTable.Brand,
                                    Cost = inTable.Cost,
                                    DateAdded = inTable.DateAdded,
                                    DateRemoved = outItem == null ? "Available" : outItem.DateAdded.ToString("dd/MMM/yyyy")
                                };
            Console.WriteLine();
            Console.WriteLine("Stock Movement Records");
            Console.WriteLine("ID\tBrand\tCost\tDate added\tDate sold");
            foreach (var item in leftJoinTable)
            {
                Console.WriteLine($"{item.ProductId}\t{item.Brand}\t{item.Cost}\t{item.DateAdded.ToShortDateString()}\t{item.DateRemoved}");
            }
        }
        public void TableSummary()
        {
            var summaryTable = _stocks
                                .GroupBy(item => item.Brand)
                                .Select(group => new
                                {
                                    Brand = group.Key,
                                    AvailableStockCount = group.Count(item => item.Movement == "Stock in"),
                                    AvailableStockValue = group.Where(item => item.Movement == "Stock in").Sum(item => item.Cost),
                                    SoldStockCount = group.Count(item => item.Movement == "Stock out"),
                                    SoldStockValue = group.Where(item => item.Movement == "Stock out").Sum(item => item.Cost)
                                });
            Console.WriteLine("Stock Summary");
            Console.WriteLine("\t\tAvailable Stock\t\tSold Stock ");
            Console.WriteLine("BRAND\t\tCount\tValue\t\tCount\tValue");
            foreach (var row in summaryTable)
            {
                Console.WriteLine($"{row.Brand}\t\t{row.AvailableStockCount}\t{row.AvailableStockValue}\t\t{row.SoldStockCount}\t{row.SoldStockValue}");
            }
        }
        public StockItem SearchByIdMovement(string id, string movement)
        {
            int parsedInt = 0;
            bool parseTry = int.TryParse(id, out parsedInt);
            StockItem matchedItem = StaticData.StockTable.SingleOrDefault(item => item.ProductId == parsedInt && item.Movement != movement);
            return matchedItem;
        }
    }

}
