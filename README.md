## Stock Management Console Application Overview

This document outlines the structure and functionality of the Stock Management Console Application.

### `Model.cs` - Model

Hosts the data structures and static data used by the application.


### `Queries.cs` - Queries

Contains only one class: `QueryLibrary`, including methods for querying and displaying the stock items from the static data.


### `Program.cs` - Program

Acts as the entry point to the application, initializing it with data and managing user navigation between different views based on input.


### `views.cs` - Views

This is the major working file. Defines the user interface and interaction flow of the application.
Actions like adding or removing stock items, and viewing stock summaries are included in the corresponding classes as well.

#### Key Components

- **`IViews` Interface**: Outlines methods (`RunView`, `HeaderView`, `ContentsView`, `ExitView`), which is the structure that all classes follow. See below. Other private methods might be included to modularise codes. 
```
    public interface IViews
    {
        public void RunView();        //invokes HeaderView(), ContentsView(), and ExitView().
        public void HeaderView();     //generates headers and main tables
        public void ContentsView();   //includes actions like adding, removing or viewing stock summaries.
        public void ExitView();       //allows users to jump to different views.
    }
```

- **`MainView` Class**: Serves as the application's main page, showing a summary of stock movements.
- **`StockAddView` Class**: Manages the addition of new stock items.
- **`StockRemoveView` Class**: Enables users to remove items from the inventory.
  
See below a snippet of MainView code. Other views follow the same structure.
```
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
            _queryLibrary.TableHistoryStock();
        }
        public void ContentsView()
        {
            Console.WriteLine();
            _queryLibrary.TableSummary();
        }
        public void ExitView()
        {
            Console.WriteLine("\nEnter 'd' to remove a stock item or enter 'a' to add a stock item.");
            ConsoleKey input = Console.ReadKey().Key;
            StaticData.switchView = (input == ConsoleKey.D || input == ConsoleKey.A) ? input : ConsoleKey.M;
        }
    }
```

