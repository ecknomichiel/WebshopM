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
    public class Inventory : ItemStorage<InventoryLine>
    {
        #region Persistency
        public void Load()
        { 
            XmlSerializer xmlRead = new XmlSerializer(typeof(Inventory));
            TextReader reader = new StreamReader(String.Format("WebshopM-{0}.xml", ID));
            Inventory load = (Inventory) xmlRead.Deserialize(reader);
            Clear();
            foreach (InventoryLine item in load)
            {
                Add(item);
            }
            reader.Close();
        }
        

        public void Save()
        {
            XmlSerializer xmlWrite = new XmlSerializer(typeof(Inventory));
            TextWriter writer = new StreamWriter(String.Format("WebshohpM-{0}.xml", ID));
            xmlWrite.Serialize(writer, this);
            writer.Close();
        }
        #endregion

        #region Sorting
        public IEnumerable<InventoryLine> GetSorted(ItemSortField sort)
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
                    return itemStorage.OrderBy(item => item.Price).OrderBy(item => item.Category);
                default:
                    return itemStorage.OrderBy(item => item.ArticleNumber);
            }    
        }
        #endregion
        #region Search
        public IEnumerable<InventoryLine> SearchForPriceHigher(double aPrice)
        {
            return itemStorage.Where(item => item.Price >= aPrice);
        }

        public IEnumerable<InventoryLine> SearchForPriceLower(double aPrice)
        {
            return itemStorage.Where(item => item.Price <= aPrice);
        }

        public IEnumerable<InventoryLine> SearchForNameAndPriceHigher(string aName, double aPrice)
        {
            return itemStorage.Where(item => item.Price >= aPrice)
                              .Where(item => item.Name.Contains(aName));
        }

        public IEnumerable<InventoryLine> SearchForNameAndPriceLower(string aName, double aPrice)
        {
            return itemStorage.Where(item => item.Price <= aPrice)
                              .Where(item => item.Name.Contains(aName));
        }

        public IEnumerable<InventoryLine> SearchForNameOrPriceLowerInCategory(string aCategory, string aName, double aPrice)
        {
            return itemStorage.Where(item => item.Category == aCategory)
                            .Where(item => item.Name.Contains(aName) || item.Price <= aPrice);
        }

        public IEnumerable<InventoryLine> SearchForNameOrPriceHigherInCategory(string aCategory, string aName, double aPrice)
        {
            return itemStorage.Where(item => item.Category == aCategory)
                            .Where(item => item.Name.Contains(aName) || item.Price >= aPrice);
        }
        #endregion
    }
    #region Enumerations
    public enum ItemSortField
    {
        Price,
        Name,
        PriceAndName,
        PriceAndCategory
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

    class StockSelector
    {
        private Dictionary<ItemSearchField, string> arguments = new Dictionary<ItemSearchField, string>();
        public ItemSortField SortField { get; set; }
        public void AddSearch(ItemSearchField aField, string aValue)
        {
            arguments.Add(aField, aValue);
        }
        public IEnumerable<InventoryLine> Search(IEnumerable<InventoryLine> allItems)
        {
            IEnumerable<InventoryLine> result = allItems;
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

    public class InventoryLine: Item
    {
        private int numItemsInStock;

        public int NumItemsInStock
        {
            get { return numItemsInStock; }
            set { numItemsInStock = value; }
        }
        public override string ToString()
        {
            return String.Format("#{0} {1} ({2}) SEK {3} ({4})", ArticleNumber, Name, Category, Price, NumItemsInStock);
        }
    }
}
