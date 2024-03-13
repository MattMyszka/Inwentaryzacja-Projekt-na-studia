namespace InventoryClasses
{
    public class Item
    {
        public int ItemID { get; set; }
        public string ItemName { get; set; }
        public float ItemCPrice { get; set; }
        public float ItemVat { get; set; }
        public float ItemPrice { get; set; }
        public float ItemDiscount { get; set; }
        public int ItemAmount { get; set; }
        public int CategoryID { get; set; }
        public int ShelfID { get; set; }

        public Item()
        {
            
        }
        public Item(string itemName, float itemVat, float itemPrice, float itemDiscount, float itemCprice, int itemAmount, int categoryID, int shelfID)
        {
            ItemName = itemName;
            ItemVat = itemVat;
            ItemPrice = itemPrice;
            ItemDiscount = itemDiscount;
            ItemCPrice = itemCprice;
            ItemAmount = itemAmount;
            CategoryID = categoryID;
            ShelfID = shelfID;  

        }
    }
    
}