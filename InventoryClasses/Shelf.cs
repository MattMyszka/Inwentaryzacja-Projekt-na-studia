using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace InventoryClasses
{
    public class Shelf
    {
        public int ShelfID { get; set; }
        public int ShelfItemsAmount { get; set; }

        public Shelf(int shelfID, int shelfItemsAmount) 
        { 
            ShelfID = shelfID;
            ShelfItemsAmount = shelfItemsAmount;
        }

        public Shelf(int shelfItemsAmount)
        {

            ShelfItemsAmount = shelfItemsAmount;
        }

        public Shelf()
        {

        }
    }
}
