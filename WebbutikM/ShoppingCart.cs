using System;
using System.Collections.Generic;
using System.Linq;
/*
 * ShoppingCart : ItemStorage<Item> 
 * o Extended Search and Sort Functions needed only by ShoppingCart (if any) 
 * o Checkout() 
 * o GetReciept() 
 * 
 * Let the user add one or multiple items to their shopping cart 
 *  Let the user view their shopping cart 
 *  Let the user checkout their shopping cart and view a receipt 
 */

namespace WebbutikM
{
    class ShoppingCart: ItemStorage<ShoppingCartLine>
    {
        private List<ShoppingCartLine> items = new List<ShoppingCartLine>();

    }

    class ShoppingCartLine: Item
    {
        private int numItems = 1;

        public int NumItems
        {
            get { return numItems; }
            set { numItems = value; }
        }
    }
}
