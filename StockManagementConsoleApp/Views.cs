using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace StockManagementConsoleApp
{
    /*
     This file contains one interface and three classes:

    `IViews` Interface: Outlines methods (`RunView`, `HeaderView`, `ContentsView`, `ExitView`), which is the structure that all classes follow.

    `MainView` Class: Serves as the application's main page, showing a summary of stock movements.
    `StockAddView` Class: Manages the addition of new stock items.
    `StockRemoveView` Class: Enables users to remove items from the inventory.
     */

    public interface IViews
    {
        public void RunView();        //invokes HeaderView(), ContentsView(), and ExitView().
        public void HeaderView();     //generates headers and main tables
        public void ContentsView();   //includes actions like adding, removing or viewing stock summaries.
        public void ExitView();       //allows users to jump to different views.
    }


    public class MainView : IViews
    {
        public QueryLibrary _queryLibrary = new QueryLibrary();
        public void RunView()
        {
            HeaderView();
            ContentsView();
            ExitView();
        }
        public void HeaderView()
        {
            Console.Clear();
            string heading =("Stock Management System Main Page");
            Console.WriteLine($"{heading}{Environment.NewLine}{new string('-', heading.Length)}");
            _queryLibrary.DisplayHistoryTransaction();
        }
        public void ContentsView()
        {
            Console.WriteLine();
            _queryLibrary.DisplayAggregateSummary();
        }
        public void ExitView()
        {
            Console.WriteLine("\nEnter 'd' to remove a stock item or enter 'a' to add a stock item.");
            ConsoleKey input = Console.ReadKey().Key;
            StaticData.switchView = (input == ConsoleKey.D || input == ConsoleKey.A) ? input : ConsoleKey.M;
        }
    }

    public class StockAddView : IViews
    {
        public QueryLibrary _queryLibrary = new QueryLibrary();
        public void RunView()
        {
            HeaderView();   
            ContentsView();
            ExitView();
        }
        public void HeaderView()
        {
            Console.Clear();
            string heading = ("ADD STOCKS");
            Console.WriteLine($"{heading}{Environment.NewLine}{new string('-', heading.Length)}");
            _queryLibrary.DisplayStockAvailable();
        }
        public void ContentsView()
        {
            int productId = StaticData.StockTable.Max(x => x.ProductId) + 1;
            DateTime dateAdded = DateTime.Today;
            string movement = "Stock in";
            string brand = GetBrandInput();
            double cost = GetCostInput();
            GetConfirmation(productId,brand,cost,dateAdded,movement);
        }
        public void ExitView()
        {
            Console.WriteLine("Jump to main page soon.");
            Thread.Sleep(500);
            StaticData.switchView = ConsoleKey.M;
        }
        public void GetConfirmation(int productId, string brand, double cost, DateTime dateAdded, string movement)
        {
            StockItem stockAdd = new StockItem { ProductId = productId, Brand = brand, Cost = cost, DateAdded = dateAdded, Movement = movement };
            Console.WriteLine("Please confirm the stock details:");
            Console.WriteLine($"ID: {productId}, BRAND: {brand}, COST: {cost}, DATE ADDED: {dateAdded.Date.ToShortDateString()}.");
            Console.WriteLine("enter 'confirm' to upload else cancel and go back to main page.");
            bool addConfirmation = Console.ReadLine().ToLower() == "confirm";
            if (addConfirmation) 
                StaticData.StockTable.Add(stockAdd); 
        }
        public string GetBrandInput()
        {
            string input;
            Console.Write("\nPlease enter brand:");
            input = Console.ReadLine().ToLower();
            return char.ToUpper(input[0]) + input.Substring(1).ToLower();
        }
        public double GetCostInput()
        {
            double cost;
            bool parseTry = false;
            do
            {
                Console.Write("\nPlease enter cost:");
                string input = Console.ReadLine();
                parseTry = double.TryParse(input, out cost);
                if (!parseTry)
                {
                    Console.WriteLine("Please enter a valid value.");
                }
            } while (!parseTry);
            return cost;
        }

    }
    public class StockRemoveView : IViews
    {
        public QueryLibrary _queryLibrary = new QueryLibrary();
        public void RunView()
        {
            HeaderView();
            ContentsView();
            ExitView();
        }
        public void HeaderView()
        {
            Console.Clear();
            string heading = ("REMOVE STOCKS");
            Console.WriteLine($"{heading}{Environment.NewLine}{new string('-', heading.Length)}");
            _queryLibrary.DisplayStockAvailable();
        }
        public void ContentsView()
        {
            bool loop = true;
            while (loop)
            {
                Console.WriteLine("Please enter a valid ID from the list or enter 'e' to return to the main view.");
                string input = Console.ReadLine().ToLower();
                StockItem itemToRemove = _queryLibrary.SearchByIdMovement(input,"Stock out");
                if (itemToRemove != default)
                    AddStockOutTran(itemToRemove);
                else if (input == "e")
                    loop = false;
                else 
                    loop = true;
            }
        }
        public void ExitView()
        {
            Console.WriteLine("Jump to main page soon.");
            Thread.Sleep(500);
            StaticData.switchView = ConsoleKey.M;
        }
        public void AddStockOutTran(StockItem itemToRemove)
        {
            StockItem stockOutItem = new StockItem
            {
                ProductId = itemToRemove.ProductId,
                Brand = itemToRemove.Brand,
                Cost = itemToRemove.Cost,
                DateAdded = DateTime.Today,
                Movement = "Stock out"
            };
            StaticData.StockTable.Add(stockOutItem);
            Console.WriteLine($"Transaction {itemToRemove.ProductId} removed successfully.");
            Thread.Sleep(500);
        }
    }

}

