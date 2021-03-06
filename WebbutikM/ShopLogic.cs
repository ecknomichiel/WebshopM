﻿using System;
using System.Collections.Generic;
using System.Linq;

namespace WebbutikM
{
    class ShopLogic
    {
        private ShopStorage inventory = new ShopStorage();
        private ShoppingCart cart = new ShoppingCart();

        public ShopLogic()
        {
            inventory.ID = 1;
            inventory.Load();
        }

        #region Search Inventory
        public IEnumerable<InventoryItem> SearchForName(string aName, bool nameContains)
        {
            LastDoneQuery = inventory.SearchForName(aName, nameContains);
            return LastDoneQuery;
        }

        public IEnumerable<InventoryItem> SearchForPrice(double aPrice, bool higherThan)
        {
            LastDoneQuery = inventory.SearchForPrice(aPrice, higherThan);
            return LastDoneQuery;
        }

        public IEnumerable<InventoryItem> SearchForNameAndPrice(string aName, double aPrice, bool nameContains, bool higherThan)
        {
            LastDoneQuery = inventory.SearchForNameAndPrice(aName, aPrice, nameContains, higherThan);
            return LastDoneQuery;
        }

        public IEnumerable<InventoryItem> SearchForNameOrPriceInCategory(string aCategory, string aName, double aPrice, bool nameContains, bool higherThan)
        {
            LastDoneQuery = inventory.SearchForNameOrPriceInCategory(aCategory, aName, aPrice, nameContains, higherThan);
            return LastDoneQuery;
        }
        #endregion

        public InventoryItem GetItemByArticleNumber(int aArticleNumber)
        {
            return inventory.GetItemByArticleNumer(aArticleNumber);
        }
        #region Get Sorted
        public IEnumerable<InventoryItem> GetSorted(ItemSortField sort)
        {
            LastDoneQuery = inventory.GetSorted(sort);
            return LastDoneQuery;
        }
        #endregion

        #region Handling the shopping cart
        public void AddItemToShoppingCart(int aArticleNumber, int aNumberToAdd)
        {
            cart.AddItem(inventory.GetItemByArticleNumer(aArticleNumber), aNumberToAdd);
        }
        #endregion

        public IEnumerable<InventoryItem> LastDoneQuery //Used for refresching the screen
        { get; set; }
        public IEnumerable<ShoppingCartLine> ShoppingCartContents
        { // Shoppingcart always ordered by articlenumber
            get { return cart.OrderBy(line => line.ArticleNumber); }
        }

        public IEnumerable<string> Buy()
        {
            yield return "Receipt";
            foreach (ShoppingCartLine line in cart)
            {
                yield return line.ToString();
            }
            yield return String.Format("Total amount due is SEK {0}", cart.TotalAmount());
            yield return "Payment is due within 21 days. IBAN SE79 TRIO 0786 9288 13 /Postgiro 1423-78";
            cart.MakeReservationsPermanent();
        }

        public void Save()
        {
            inventory.Save();
        }
    }

    
}
