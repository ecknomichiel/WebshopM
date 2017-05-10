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
            inventory.Load();
        }
        
        //public Inventory Inventory { get { return inventory; }}

        public IEnumerable<InventoryLine> LastDoneQuery
        { get; set; }
        public IEnumerable<ShoppingCartLine> ShoppingCartContents
        { // Shoppingcart always ordered by articlenumber
            get { return cart.OrderBy(line => line.ArticleNumber); }
        }
    }

    
}
