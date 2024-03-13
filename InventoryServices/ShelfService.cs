using System;
using System.Collections.Generic;
using System.Linq;
using InventoryClasses;
using NLog.Targets;
using System.ComponentModel.DataAnnotations;
using Microsoft.EntityFrameworkCore;

namespace InventoryServices
{
    public class ShelfService
    {
        private readonly InventoryContext _context;
        public ShelfService(InventoryContext context)
        {
            _context = context;
        }
        #region useless

        //private List<Shelf> shelves = new List<Shelf>();
        //private List<Item> items = new List<Item>(); // Przykładowa lista produktów

        // Dodawanie półki (zakomentowane stare dodawanie, niżej nowe)


        //public void AddShelf(int shelfId, int shelfItemsAmount)
        //{
        //    Shelf newShelf = new Shelf(shelfId, shelfItemsAmount);
        //    shelves.Add(newShelf);
        //    Console.WriteLine($"Dodano nową półkę o ID {shelfId}.");
        //}

        //public void AddShelf(int shelfId, int shelfItemsAmount)
        //{
        //    Shelf newShelf = new Shelf(shelfId, shelfItemsAmount);
        //    shelves.Add(newShelf);
        //    Console.WriteLine($"Dodano nową półkę o ID {shelfId}.");
        //}
        #endregion

        //display shelves 
        public async Task DisplayShelf(Shelf shelf)
        {
            Console.WriteLine("===============================================================================================");
            Console.WriteLine($"Shelf iD: {shelf.ShelfID}, Amount of items on the shelf: {shelf.ShelfItemsAmount}");
            Console.WriteLine("===============================================================================================");
        }

        public async Task UpdateAllShelves()
        {
            var shelves = GetShelves();
            Console.Clear();
            foreach (var shelf in shelves)
            {
                CountShelfProductsAsync(shelf);
                await _context.SaveChangesAsync();
            }
        }

        public async Task DisplayAllShelves()
        {
            await UpdateAllShelves();
            var shelves = GetShelves();
            Console.Clear();
            foreach (var shelf in shelves)
            {
                await DisplayShelf(shelf);
                Thread.Sleep(100);
            }
        }
        public IEnumerable<Shelf> GetShelves()
        {
            return _context.Shelves.ToList();
        }

        //aktualizuj produkty
        public Shelf GetShelfByID(int shelfID)
        {
            Shelf shelfToUpdate = _context.Shelves.FirstOrDefault(s => s.ShelfID == shelfID);
            return shelfToUpdate;
        }

/*        public async Task UpdateAllShelvesAsync()
        {
            var shelves = GetShelves();
            Console.Clear();
            foreach(var shelf in shelves) 
            {
                CountShelfProductsAsync (shelf);
            }
        }    
*/
        public void CountShelfProductsAsync(Shelf shelf)
        {
            if (shelf == null)
            {
                Console.WriteLine("Podana półka jest null.");
                return;
            }

            int shelfProductAmount = _context.Items
                .Where(item => item.ShelfID == shelf.ShelfID)
                .Sum(item => item.ItemAmount);

            shelf.ShelfItemsAmount = shelfProductAmount;
            _context.SaveChanges();
        }


        // Wyświetlanie zawartości półki
        public void DisplayShelfContent(int shelfId)
        {
            var shelf = _context.Shelves.FirstOrDefault(s => s.ShelfID == shelfId);

            if (shelf == null)
            {
                Console.WriteLine($"Półka o ID {shelfId} nie istnieje.");
                return;
            }
            Console.Clear();
            Console.WriteLine($"Zawartość półki o ID {shelf.ShelfID}:");

            // Pobierz produkty przypisane do danej półki
            var itemsOnShelf = _context.Items.Where(item => item.ShelfID == shelfId);
            
            // Wyświetl informacje o produktach przy użyciu funkcji DisplayItems
            foreach (var item in itemsOnShelf)
            {
                ItemService.DisplayProduct(item);
            }

        }


        // Wyświetlanie informacji o produkcie

    }
}
