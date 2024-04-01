namespace StockManagementConsoleApp
{
    internal class Program
    {
        static void Main(string[] args)
        {
            StaticData.GenerateRandomTransactions(15);
            IViews views;
            StaticData.switchView = ConsoleKey.M;
            while (true)
            {
                switch (StaticData.switchView)
                {
                    case ConsoleKey.M:
                        views = new MainView();
                        views.RunView();
                        break;
                    case ConsoleKey.A:
                        views = new StockAddView();
                        views.RunView();
                        break;
                    case ConsoleKey.D:
                        views = new StockRemoveView();
                        views.RunView();
                        break;
                }
            }
        }
    }
}
