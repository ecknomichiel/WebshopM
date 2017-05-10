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
        public void AddItem(InventoryLine inventoryItem)
        {
            ShoppingCartLine shopItem = itemStorage.Single(item => item.ArticleNumber == inventoryItem.ArticleNumber);
            if (shopItem == null)
            {
                shopItem = new ShoppingCartLine() 
                    { ArticleNumber = inventoryItem.ArticleNumber, 
                        Category = inventoryItem.Category, 
                        Price = inventoryItem.Price, 
                        Name = inventoryItem.Name};

                itemStorage.Add(shopItem);
                shopItem.AssignInventoryItem(inventoryItem);
            }
            shopItem.DoReservation(1); //can throw an EOutOfStockException
        }


    }

    class ShoppingCartLine: Item
    {
        private int numItems = 0;
        private InventoryLine inventoryItem;

        public int NumItems
        {
            get { return numItems; }
        }

        internal void DoReservation(int aNumItems)
        {
            inventoryItem.AddReservation(aNumItems); //Can throw EOutOfStockException
            numItems += aNumItems;
        }

        internal void AssignInventoryItem(InventoryLine aInventoryItem)
        {
            if (inventoryItem != null && inventoryItem != aInventoryItem)
            { //remove reservations to previous item and clear amount of items
                inventoryItem.AddReservation(-numItems);
                numItems = 0;
            }
            inventoryItem = aInventoryItem;
        }
        public ~ShoppingCartLine()  // destructor
        {
            if (inventoryItem != null)
            {
                inventoryItem.AddReservation(-numItems);
            }
        }
    }
}
