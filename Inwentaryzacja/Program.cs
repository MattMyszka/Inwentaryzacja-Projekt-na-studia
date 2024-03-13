using InventoryClasses;
using InventoryServices;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Hosting;
using System;
using System.Security.Cryptography.X509Certificates;
using Microsoft.Extensions.Logging;

namespace Inwentaryzacja
{
    class Program
    {


        private static readonly HistoryLogger historyLogger = new HistoryLogger();
        static async Task Main(string[] args)
        {
            static void DrawWall()
            {
                //Console.ForegroundColor = ConsoleColor.Red;

                for (int i = 0; i < 70; i++)
                {
                    for (int j = 0; j < 300; j++)
                    {
                        Console.Write("#");
                    }
                    Thread.Sleep(1); 
                }

                Console.WriteLine(); 
            }

            static void EraseWall()
            {
                for (int i = 0; i < 50; i++)
                {
                    for (int j = 0; j < 400; j++)
                    {
                        Console.Write(" ");
                    }
                    Thread.Sleep(1); // Pauza dla płynności animacji
                }
                Console.Clear();
            }


            historyLogger.HistoryEvent += (sender, e) =>
            {
                Console.WriteLine($"History Log: {e.Action}");
            };

            var host = CreateHostBuilder(args).Build();

            using (var scope = host.Services.CreateScope())
            {
                var services = scope.ServiceProvider;
                var dbContext = services.GetRequiredService<InventoryContext>();
                var itemService = services.GetRequiredService<ItemService>();
                var shelfService = services.GetRequiredService<ShelfService>();
                
                
                
                Console.Clear();

                //uzytkownik wybiera co chce wyswietlic
                while (true)
                {
                    int animationDuration = 100; // czas trwania animacji w milisekundach
                    
                    while (true)
                    {
                        Console.WriteLine("Wyswietl półki (p)\nWyswietl pelny inwentarz (i)");
                        var choice = Console.ReadLine();

                        if (choice != "p" && choice != "i")
                        {
                            Console.WriteLine("źle wprowadzone dane, spróbuj jeszcze raz");
                            Thread.Sleep(1500);
                            Console.Clear();
                        }
                        else
                        {
                            if (choice == "p")
                            {
                                ShelfMenu(shelfService, animationDuration);
                                break;
                            }
                            else if (choice == "i")
                            {
                                //historyLogger.LogAction("Displayed Inventory");                          
                                InventoryMenu(itemService, animationDuration);
                                break;
                            }



                        }
                    }


                }

                #region ItemMenu
                //Obsluga Menu
                async void InventoryMenu(ItemService itemService, int animationDuration)
                {
                    await itemService.DisplayAllProducts();
                    while (true)
                    {
                        Console.WriteLine("\n\nDodaj istniejący produkt (d)\nDodaj nowy produkt (dn)\nUsuń produkt(u)\nEdytuj produkt(e)\nWyświetl produkty(w)\nPowrót do menu głównego (x)");

                        var wyborCzynnosci = Console.ReadLine();

                        if (wyborCzynnosci != "d" && wyborCzynnosci != "u" && wyborCzynnosci != "e" && wyborCzynnosci != "dn" && wyborCzynnosci != "w" && wyborCzynnosci != "x")
                        {
                            Console.WriteLine("źle wprowadzone dane, spróbuj jeszcze raz");
                            //Thread.Sleep(1500);
                            Console.Clear();
                        }
                        else
                        {
                            if (wyborCzynnosci == "d")
                            {
                                Console.WriteLine("Podaj ID produktu do dodania:");
                                if (int.TryParse(Console.ReadLine(), out int existingItemId))
                                {
                                    itemService.AddExistingItemAsync(existingItemId);
                                    historyLogger.LogAction($"Added existing item with ID {existingItemId}");
                                }
                            }
                            else if (wyborCzynnosci == "dn")
                            {
                                var newItem = itemService.AddNewItem();
                                itemService.AddNewProductAsync(newItem.ItemID, newItem);
                                historyLogger.LogAction("Added new item");
                            }
                            else if (wyborCzynnosci == "u")
                            {
                                Console.WriteLine("podaj id przedmiotu do usunięcia: ");
                                if (int.TryParse(Console.ReadLine(), out int deleteItemId))
                                {
                                    itemService.DeleteProductAsync(deleteItemId);
                                    historyLogger.LogAction($"Deleted item with ID {deleteItemId}");
                                }
                            }
                            else if (wyborCzynnosci == "e")
                            {
                                Console.WriteLine("Podaj ID produktu do edycji:");
                                if (int.TryParse(Console.ReadLine(), out int editItemId))
                                {
                                    var updatedItem = itemService.AddNewItem(); 
                                    itemService.EditProductAsync(editItemId, updatedItem);
                                }

                            }
                            else if (wyborCzynnosci == "w")
                            {
                                
                                itemService.DisplayAllProducts();
                                historyLogger.LogAction($"Displayed all products");
                            }
                            else if (wyborCzynnosci == "x")
                            {
                                DrawWall();
                                Thread.Sleep(animationDuration);
                                EraseWall();

                                break;
                            }
                        }
                    }
                }


                #endregion

                #region ShelfMenu
                async void ShelfMenu(ShelfService shelfService, int animationDuration)
                {
                    Console.WriteLine("wyswietlam półki");

                    await shelfService.DisplayAllShelves();

                    historyLogger.LogAction("Displayed shelves");

                    Console.WriteLine("Podaj id półki do wyświetlenia zawartości: ");

                    int.TryParse(Console.ReadLine(), out int checkedshelfId);
                    shelfService.DisplayShelfContent(checkedshelfId);

                    Console.WriteLine("\nKliknij enter by wrocic do glownego menu");
                    Console.ReadLine();
                    DrawWall();
                    Thread.Sleep(animationDuration);
                    EraseWall();
                }

                #endregion



            }
            host.Run();


        }
        #region hostbuilder
        public static IHostBuilder CreateHostBuilder(string[] args) =>
      Host.CreateDefaultBuilder(args)
          .ConfigureServices((hostContext, services) =>
          {
              // Dodaj DbContext przy użyciu DbContextFactory
              services.AddDbContext<InventoryContext>(options =>
              {
                  var dbContextFactory = new InventoryContextFactory();
                  var context = dbContextFactory.CreateDbContext(args);

                  options.UseNpgsql(context.Database.GetConnectionString())
                         .LogTo(Console.WriteLine, LogLevel.None); // Wyłączenie logowania
              });
              services.AddAutoMapper(typeof(ItemMapper));
              services.AddScoped<ItemService>();
              services.AddScoped<ShelfService>();
          });

        #endregion

    }
    #region Logger
    public class HistoryEventArgs : EventArgs
    {
        public string Action { get; }

        public HistoryEventArgs(string action)
        {
            Action = action;
        }
    }

    public class HistoryLogger : IMyLogger
    {
        private readonly string HistoryFilePath = Path.Combine(AppDomain.CurrentDomain.BaseDirectory, "history.txt");

        public event EventHandler<HistoryEventArgs> HistoryEvent;

        public HistoryLogger()
        {
            if (!File.Exists(HistoryFilePath))
            {
                File.Create(HistoryFilePath).Close();
            }
        }

        public void LogAction(string action)
        {
            // Zapisz log do pliku
            File.AppendAllText(HistoryFilePath, $"{DateTime.Now}: {action}\n");

            // Wywołaj zdarzenie, aby inne części programu mogły reagować na zalogowane akcje
            HistoryEvent?.Invoke(this, new HistoryEventArgs(action));
        }
    }

    #endregion

}
