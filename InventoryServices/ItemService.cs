using InventoryClasses;
using NLog.Targets;
using System.ComponentModel.DataAnnotations;

namespace InventoryServices
{
    public class ItemService 
    {
        private readonly InventoryContext _context;

        public ItemService(InventoryContext context)
        {
            _context = context;
        }

        #region uselsess
        //Add record
        //public async Task AddProductAsync(int iD)
        //{
        //    if (FindExistingProduct(iD))
        //    {
        //        Console.WriteLine("Odnaleziono przedmiot o podanym iD");
        //        await _context.SaveChangesAsync();
        //    }
        //    else
        //    {
        //        Console.WriteLine($"Product with ID {iD} does not exist. Creating a new product.");

        //        var newItem = AddNewItem();

        //        //Sprawdź istnienie półki o podanym ShelfID

        //        if (_context.Shelves.Any(shelf => shelf.ShelfID == newItem.ShelfID))
        //        {
        //            _context.Items.Add(newItem);
        //            await _context.SaveChangesAsync();
        //            Console.WriteLine($"Produkt został dodany do półki o ID {newItem.ShelfID}.");
        //        }
        //        else
        //        {
        //            Console.WriteLine($"Półka o ID {newItem.ShelfID} nie istnieje. Tworzenie nowej półki i dodawanie produktu {newItem.ItemName} do {newItem.ShelfID}.");

        //            var newShelf = AddNewShelf();

        //            //ShelfService shelfService = new ShelfService();
        //            //shelfService.AddShelf(newItem.ShelfID, newItem.ItemAmount);
        //            //await _context.SaveChangesAsync();
        //            _context.Shelves.Add(newShelf);
        //            await _context.SaveChangesAsync();
        //            _context.Items.Add(newItem);
        //            await _context.SaveChangesAsync();
        //            Console.WriteLine($"Produkt został dodany do półki o ID {newItem.ShelfID}.");
        //        }
        //    }
        //}
        #endregion
        public async Task AddNewProductAsync(int iD, Item newItem)
        {

                //Sprawdź istnienie półki o podanym ShelfID

                if (_context.Shelves.Any(shelf => shelf.ShelfID == newItem.ShelfID))
                {
                    _context.Items.Add(newItem);
                    await _context.SaveChangesAsync();
                    Console.WriteLine($"Produkt został dodany do półki o ID {newItem.ShelfID}.");
                }
                else
                {
                    Console.WriteLine($"Półka o ID {newItem.ShelfID} nie istnieje. Tworzenie nowej półki i dodawanie produktu {newItem.ItemName} do {newItem.ShelfID}.");

                    var newShelf = AddNewShelf();

                    _context.Shelves.Add(newShelf);
                    await _context.SaveChangesAsync();
                    await Task.Delay(1000);
                    _context.Items.Add(newItem);
                    await _context.SaveChangesAsync();
                    Console.WriteLine($"Produkt został dodany do półki o ID {newItem.ShelfID}.");
                }
            await _context.SaveChangesAsync();
        }
        public Shelf AddNewShelf()
        {
            Shelf newShelf = new Shelf();
            return newShelf;
        }
        public async Task AddExistingItemAsync(int iD)
        {
            var existingItem = _context.Items.FirstOrDefault(item => item.ItemID == iD);

            if (existingItem != null)
            {
                
                Console.WriteLine("Podaj ilość produktów do dodania: ");
                if (int.TryParse(Console.ReadLine(), out int quantity))
                {

                    //existingItem.ItemAmount += quantity;
                    existingItem.ItemAmount += quantity;
                    _context.SaveChanges();
                    Console.WriteLine($"Dodano {quantity} produktów do produktu o iD {iD}");
                }
                else
                {
                    Console.WriteLine("niepoprawny format iD");
                }
            }
            await _context.SaveChangesAsync();
        }

        public Item AddNewItem()
        {
            Console.WriteLine("Podaj Nazwę produktu: ");
            string itemName = Console.ReadLine();

            Console.WriteLine("Podaj wartość Vat: ");
            float.TryParse(Console.ReadLine(), out float itemVat);
            
            Console.WriteLine("Podaj wartość Netto: ");
            float.TryParse(Console.ReadLine(), out float itemPrice);

            Console.WriteLine("Podaj wartość zniżki: ");
            float.TryParse(Console.ReadLine(), out float itemDiscount);

            //float itemCPrice = ((1-itemDiscount)*(itemPrice * itemVat) + itemPrice);
            float itemCPrice = (float)Math.Round(((1 - itemDiscount) * (itemPrice * itemVat) + itemPrice), 2);

            Console.WriteLine("Podaj ilość produktów do dodania: ");
            int.TryParse(Console.ReadLine(), out int itemAmount);

            Console.WriteLine("Podaj ID kategorii przedmiotu: ");
            int.TryParse (Console.ReadLine(), out int categoryID);

            Console.WriteLine("Podaj shelf ID przedmiotu: ");
            int.TryParse(Console.ReadLine(), out int shelfID);

            Item newItem = new Item(itemName, itemVat, itemPrice, itemDiscount, itemCPrice, itemAmount, categoryID, shelfID);
            return newItem;
        }

        public bool FindExistingProduct(int ID)
        {
            return _context.Items.Any(item => item.ItemID == ID);
        }
        //Delete record
        public async Task DeleteProductAsync(int itemId)
        {
            var existingItem = _context.Items.Find(itemId);

            if (existingItem == null)
            {
                Console.WriteLine($"Nie znaleziono przedmiotu o ID {itemId}");
                return;
            }

            //DisplayProduct(existingItem);

            Console.WriteLine($"Czy na pewno chcesz usunąć poniższy przedmiot?");
            DisplayProduct(existingItem);

            Console.WriteLine("Potwierdź usunięcie (T/N): ");
            var confirmation = Console.ReadLine();

            if (confirmation.ToUpper() == "T")
            {
                _context.Items.Remove(existingItem);
                await _context.SaveChangesAsync();
                Console.WriteLine($"Przedmiot o ID {itemId} został usunięty.");
            }
            else
            {
                Console.WriteLine($"Usuwanie przedmiotu o ID {itemId} anulowane.");
            }

           await _context.SaveChangesAsync();
        }

        //Edit record
        public async Task EditProductAsync(int itemId, Item updatedItem)
        {
            var existingItem = _context.Items.Find(itemId);

            if (existingItem == null)
            {
                Console.WriteLine($"Nie znaleziono przedmiotu o ID {itemId}");
                return;
            }

            
            existingItem.ItemName = updatedItem.ItemName;
            existingItem.ItemCPrice = updatedItem.ItemCPrice;
            existingItem.ItemVat = updatedItem.ItemVat;
            existingItem.ItemPrice = updatedItem.ItemPrice;
            existingItem.ItemDiscount = updatedItem.ItemDiscount;
            existingItem.ItemAmount = updatedItem.ItemAmount;
            existingItem.ShelfID = updatedItem.ShelfID;

            
            await _context.SaveChangesAsync();
        }

        //Return records
        public IEnumerable<Item> GetProducts()
        {
            return _context.Items.ToList();
        }

        //Display records
        public static void DisplayProduct(Item item)
        {
            Console.WriteLine("===============================================================================================");
            Console.WriteLine($"Product ID: {item.ItemID}, Name: {item.ItemName}, Item Amount: {item.ItemAmount}, Item Calculated Price: {item.ItemCPrice} \n" +
                                $"Item Netto Value: {item.ItemPrice}, Item Vat: {item.ItemVat}, Item Discount: {item.ItemDiscount} \n" +
                                $"Shelf iD: {item.ShelfID}, Category iD; {item.CategoryID}");
            Console.WriteLine("===============================================================================================");
        }

        public async Task DisplayAllProducts()
        {
            var items = GetProducts();
            Console.Clear();
            foreach (var item in items)
            {
                DisplayProduct(item);
            }
            await _context.SaveChangesAsync();
        }
    }
}