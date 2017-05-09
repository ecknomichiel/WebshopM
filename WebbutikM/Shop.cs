using System;
using System.Collections.Generic;
using System.Linq;

namespace WebbutikM
{
    class ShopLogic
    {
        private Inventory inventory = new Inventory();
        private ShoppingCart cart = new ShoppingCart();

        public ShopLogic()
        {
            inventory.LoadFromFile(String.Format("WebshopM-{0}.xml", inventory.ID));
        }
        
    }

    
}
