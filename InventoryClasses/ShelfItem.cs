using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace InventoryClasses
{
    public class ShelfItem
    {
        public int ItemID { get; set; }
        public string ItemName { get; set; }
        public float ItemCPrice { get; set; }
        public float ItemVat { get; set; }
        public float ItemPrice { get; set; }
        public float ItemDiscount { get; set; }
        public int ItemAmount { get; set; }
        public Category CategoryID { get; set; }
    }
}
