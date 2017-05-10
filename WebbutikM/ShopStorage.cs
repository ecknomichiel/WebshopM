using System;
using System.Collections.Generic;
using System.Linq;
using System.Xml.Serialization;
using System.IO;

/* Requirements:
 * ShopStorage : ItemStorage<Item> 
 * o Extended Search and Sort Functions needed only by ShopStorage (if any) 
 * o Optional: Handling how many items are in stock 
 * 
 * o Search the storage based on name (both exact matches and matches containing the term) 
 * o Let the user search for an (or multiple) item(s) in stock 
 * o See the stock sorted by price 
 * o See the stock sorted by name 
 * o See the stock sorted by price and name 
 * o See the stock sorted by price, grouped by category 
 * o Search the storage based on price (lower than, higher than) 
 * o Search the storage based on price and name 
 * o Search for products based on price OR name, within a preselected category 
 */

namespace WebbutikM
{
    //Cheated a bit and made it a collection of InventoryLines to add support for amount in stock, amount available and reservations
    [Serializable]
    public class ShopStorage : ItemStorage<InventoryItem>
    {
        #region Persistency
        public void Load()
        { 
            XmlSerializer xmlRead = new XmlSerializer(typeof(ShopStorage));
            TextReader reader = new StreamReader(String.Format("WebshopM-{0}.xml", ID));
            ShopStorage load = (ShopStorage) xmlRead.Deserialize(reader);
            Clear();
            foreach (InventoryItem item in load)
            {
                Add(item);
            }
            reader.Close();
        }
        

        public void Save()
        {
            XmlSerializer xmlWrite = new XmlSerializer(typeof(ShopStorage));
            TextWriter writer = new StreamWriter(String.Format("WebshohpM-{0}.xml", ID));
            xmlWrite.Serialize(writer, this);
            writer.Close();
        }
        #endregion

        #region Sorting
        public IEnumerable<InventoryItem> GetSorted(ItemSortField sort)
        {
            switch (sort)
            {
                case ItemSortField.Name:
                    return itemStorage.OrderBy(item => item.Name);
                case ItemSortField.Price:
                    return itemStorage.OrderBy(item => item.Price);
                case ItemSortField.PriceAndName:
                    return itemStorage.OrderBy(item => item.Name).OrderBy(item => item.Price); //Gets sorted by name first, later sorted on price
                case ItemSortField.PriceAndCategory:
                    return GroupedBy();                  
                default:
                    return itemStorage.OrderBy(item => item.ArticleNumber);
            }    
        }

        private IEnumerable<InventoryItem> GroupedBy()
        {
            var grouped = from item in itemStorage
                          group item by item.Category;
            foreach (var group in grouped)
            {
                group.OrderBy(item => item.Price);
                foreach (InventoryItem item in group)
                    yield return item;
            }
        }
        #endregion
        #region Search
        public IEnumerable<InventoryItem> SearchForName(string aName, bool nameContains)
        {
            return itemStorage.Where(item => (nameContains && item.Name.Contains(aName))
                                               || (!nameContains && item.Name == aName));
        }

        public IEnumerable<InventoryItem> SearchForPrice(double aPrice, bool higherThan)
        {
            return itemStorage.Where(item => (higherThan && item.Price >= aPrice)
                                              || (!higherThan && item.Price <= aPrice));            
        }

        public IEnumerable<InventoryItem> SearchForNameAndPrice(string aName, double aPrice, bool nameContains, bool higherThan)
        {
            return itemStorage.Where(item => (higherThan && item.Price >= aPrice)
                                              || (!higherThan && item.Price <= aPrice))
                              .Where(item => (nameContains && item.Name.Contains(aName))
                                               || (!nameContains && item.Name == aName));
        }

        public IEnumerable<InventoryItem> SearchForNameOrPriceInCategory(string aCategory, string aName, double aPrice, bool nameContains, bool higherThan)
        {
            return itemStorage.Where(item => item.Category == aCategory)//Category
                            .Where(item => ((nameContains && item.Name.Contains(aName)) //Namn contains eller är lika med, beroende på nameContains
                                               || (!nameContains && item.Name == aName))
                                            ||// Namn eller pris
                                            ((higherThan && item.Price >= aPrice)// Pris större än eller mindre än, beroende på higherThan
                                              || (!higherThan && item.Price <= aPrice)));
        }

        #endregion

        public InventoryItem GetItemByArticleNumer(int aArticleNumber)
        {
            InventoryItem result;
            try
            {
                 result = itemStorage.Single(item => item.ArticleNumber == aArticleNumber);
            }
            catch (Exception)
            {
                throw new EItemNotFound(String.Format("Item with number {0} was not found", aArticleNumber));
            }
            return result;
        }
    }
    #region Enumerations
    public enum ItemSortField
    {
        Price,
        Name,
        PriceAndName,
        PriceAndCategory,
        ArticleNumber
    }

    enum ItemSearchField
    {
        PriceHigher,
        PriceLower,
        NameContains,
        NameIs,
        Category
    }
    #endregion

    #region Selector
    // Jag har för nuläget bestämt mig att inte använda den här, men löser det med tre olika functioner i Inventory klassen
    class StockSelector
    {
        private Dictionary<ItemSearchField, string> arguments = new Dictionary<ItemSearchField, string>();
        public ItemSortField SortField { get; set; }
        public void AddSearch(ItemSearchField aField, string aValue)
        {
            arguments.Add(aField, aValue);
        }
        public IEnumerable<InventoryItem> Search(IEnumerable<InventoryItem> allItems)
        {
            IEnumerable<InventoryItem> result = allItems;
            string value;
            foreach (ItemSearchField key in arguments.Keys)
            {
                arguments.TryGetValue(key, out value);
                switch (key)
                {
                    case ItemSearchField.PriceHigher:
                        result = result.Where(item => item.Price >= double.Parse(value));
                        break;
                    case ItemSearchField.PriceLower:
                        result = result.Where(item => item.Price <= double.Parse(value));
                        break;
                    case ItemSearchField.Category:
                        result = result.Where(item => item.Category == value);
                        break;
                    case ItemSearchField.NameIs:
                        result = result.Where(item => item.Name == value);
                        break;
                    case ItemSearchField.NameContains:
                        result = result.Where(item => item.Name.Contains(value));
                        break;
                }
            }
            arguments.Clear();
            return result;
        }

    }
#endregion

    public class InventoryItem: Item
    {
        private int numItemsInStock;
        private int numItemsReserved;

        public int NumItemsInStock
        {
            get { return numItemsInStock; }
            set { numItemsInStock = value; }
        }
        public override string ToString()
        {
            return String.Format("#{0} {1} ({2}) SEK {3} ({4})", ArticleNumber, Name, Category, Price, NumItemsInStock - numItemsReserved);
        }

        internal void AddReservation(int aNumItems)
        {
            if (aNumItems > (numItemsInStock - numItemsReserved))
            {
                throw new EOutOfStock(String.Format("There is insufficient stock ({0}) of article {1} in order to buy {2} items", (numItemsInStock - numItemsReserved), ArticleNumber, aNumItems));
            }
            else
            {
                numItemsReserved += aNumItems;
            }
            
        }
    }
}
