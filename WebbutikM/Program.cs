using System;
using System.Collections.Generic;

namespace WebbutikM
{
    class Program
    {
        static void Main(string[] args)
        {
            ShopLogic shop = new ShopLogic();

            foreach (InventoryLine line in shop.Inventory.GetSorted(ItemSortField.Name))
            {
                Console.WriteLine(line.ArticleNumber + line.Name);
}

            Console.ReadKey();
        }
    }
}
