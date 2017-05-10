using System;
using System.Collections.Generic;
using System.Linq;
/*
 * ShoppingCart : ItemStorage<Item> 
 * o Extended Search and Sort Functions needed only by ShoppingCart (if any) 
 * o Checkout() 
 * o GetReciept() 
 * 
 * o Let the user add one or multiple items to their shopping cart 
 * o Let the user view their shopping cart 
 * o Let the user checkout their shopping cart and view a receipt 
 */

namespace WebbutikM
{
    class ShoppingCart: ItemStorage<ShoppingCartLine>
    {
        public void AddItem(InventoryItem inventoryItem)
        {
            AddItem(inventoryItem, 1);
        }

        public void AddItem(InventoryItem inventoryItem, int aNumberToAdd)
        {
            ShoppingCartLine shopItem = null;
            try 
	        {	        
		        shopItem = itemStorage.Single(item => item.ArticleNumber == inventoryItem.ArticleNumber);
	        }
	        catch (Exception)
	        {
                //If it is not in the list and it should be removed, just ignore
                if (aNumberToAdd < 0)
                    return;
                //An exception means that the item was not in the shoppingcart previously.
                shopItem = new ShoppingCartLine()
                {
                    ArticleNumber = inventoryItem.ArticleNumber,
                    Category = inventoryItem.Category,
                    Price = inventoryItem.Price,
                    Name = inventoryItem.Name
                };

                itemStorage.Add(shopItem);
                shopItem.AssignInventoryItem(inventoryItem);
	        }
   
            shopItem.DoReservation(aNumberToAdd); //can throw an EOutOfStockException
            if (shopItem.NumItems == 0)
                itemStorage.Remove(shopItem);
        }

        public double TotalAmount()
        {
            double result = 0;
            foreach (ShoppingCartLine line in itemStorage)
                result += line.NumItems * line.Price;
            return result;

        }
    }

    class ShoppingCartLine: Item
    {
        private int numItems = 0;
        private InventoryItem inventoryItem;

        public int NumItems
        {
            get { return numItems; }
        }

        internal void DoReservation(int aNumItems)
        {
            inventoryItem.AddReservation(aNumItems); //Can throw EOutOfStockException
            numItems += aNumItems;
        }

        public void AssignInventoryItem(InventoryItem aInventoryItem)
        {
            if (inventoryItem != null && inventoryItem != aInventoryItem)
            { //remove reservations to previous item and clear amount of items
                inventoryItem.AddReservation(-numItems);
                numItems = 0;
            }
            inventoryItem = aInventoryItem;
        }

        public override string ToString()
        {
            return String.Format("{0} {1} {2} {3}x SEK{4}= SEK{5}",
                    ArticleNumber.ToString().PadRight(5),//1
                    Name.PadRight(50),//2
                    Category.PadRight(10),//3
                    NumItems.ToString().PadRight(3),//4
                    Price.ToString().PadRight(6),//5
                    NumItems * Price);
        }

        ~ShoppingCartLine()  // destructor
        {
            if (inventoryItem != null)
            {
                inventoryItem.AddReservation(-numItems);
                inventoryItem = null;
            }
        }
    }
}
