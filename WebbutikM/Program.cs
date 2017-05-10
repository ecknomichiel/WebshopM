using System;
using System.Collections.Generic;

namespace WebbutikM
{
    class Program
    {
        static ShopLogic shop;
        static void Main(string[] args)
        {
            shop = new ShopLogic();
            string input;
            shop.GetSorted(ItemSortField.PriceAndCategory);
            do
            {
                ShowMainMenu();
                input = Console.ReadLine();
                ExecuteAction(input);
            } while (input != "quit");
        }

        static void ExecuteAction(string input)
        {
            if (input.Length < 1)
                return;
            string argument = input.Substring(1);
            int choice;
            int.TryParse(argument, out choice);
            char menu = input[0];
            switch (menu)
            {
                case 's': 
                    Sort(choice);
                    break;
                case 'f':
                    Find(choice);
                    break;
                case '+':
                    Add(choice);
                    break;
                case '-':
                    Remove(choice);
                    break;
                case 'c':
                    CheckOut();
                    break;
            }
        }

        private static void Remove(int choice)
        {
            try
            {
                shop.AddItemToShoppingCart(choice, -1);
            }
            catch (Exception e)
            {//Not interested if the item existed
            }
        }

        private static void CheckOut()
        {
            IEnumerable<string> kvitto = shop.Buy();
            foreach(string line in kvitto)
            {
                Console.WriteLine(line);
            }
            Console.ReadKey();
        }

        private static void Add(int choice)
        {
            try
            {
                shop.AddItemToShoppingCart(choice, 1);
            }
            catch (Exception e)
            {
                Console.WriteLine(e.Message);
                Console.ReadKey();
            }
        }

        private static void Find(int choice)
        {
            // 1) Name 2) Price 3) Price and Name 4) Category, Price and Name
            ItemSearchField[] fields;
            string name, category;
            double numPrice;
            bool contains, greaterThan;
            switch (choice)
            {
                case 1: 
                    fields = new ItemSearchField[]{ItemSearchField.NameContains};
                    GetInput(fields, out name, out category, out numPrice, out contains, out greaterThan);
                    shop.SearchForName(name, contains);
                    break;
                case 2:
                    fields = new ItemSearchField[] { ItemSearchField.PriceHigher };
                    GetInput(fields, out name, out category, out numPrice, out contains, out greaterThan);
                    shop.SearchForPrice(numPrice, greaterThan);
                    break;
                case 3:
                    fields = new ItemSearchField[]{ItemSearchField.NameContains,
                                                    ItemSearchField.PriceHigher};
                    GetInput(fields, out name, out category, out numPrice, out contains, out greaterThan);
                    shop.SearchForNameAndPrice(name, numPrice, contains, greaterThan);
                    break;
                case 4:
                    fields = new ItemSearchField[] { ItemSearchField.Category,
                                                    ItemSearchField.NameContains,
                                                    ItemSearchField.PriceHigher};
                    GetInput(fields, out name, out category, out numPrice, out contains, out greaterThan);
                    shop.SearchForNameOrPriceInCategory(category, name, numPrice, contains, greaterThan);
                    break;
                default:
                    return;
            }
            
        }

        private static void GetInput(ItemSearchField[] fields, out string name, out string category, out double numPrice, out bool contains, out bool greaterThan)
        {
            string price;
            name = "";
            category = "";
            numPrice = 0;
            contains = true;
            greaterThan = false;
            foreach (ItemSearchField field in fields)
            {// Ask for input of values for each field
                switch (field)
                {
                    case ItemSearchField.NameContains:
                        Console.WriteLine("Input text you are looking for, preceded by = for exact matching or * for all names that contain your text.");
                        Console.Write("Name: ");
                        name = Console.ReadLine();
                        contains = !name.StartsWith("=");
                        name = name.Substring(1);
                        break;
                    case ItemSearchField.PriceHigher:
                        Console.WriteLine("Input price you are looking for, preceded by < for smaller than or equal or > for greater than or equal.");
                        Console.Write("Price: ");
                        price = Console.ReadLine();
                        greaterThan = price.StartsWith(">");
                        double.TryParse(price.Substring(1), out numPrice);
                        break;
                    case ItemSearchField.Category:
                        Console.Write("Category: ");
                        category = Console.ReadLine();
                        break;
                }

            }
        }

        static void Sort(int choice)
        {
            switch (choice)
            {
                case 1: 
                    shop.GetSorted(ItemSortField.Name);
                    break;
                case 2:
                    shop.GetSorted(ItemSortField.Price);
                    break;
                case 3:
                    shop.GetSorted(ItemSortField.PriceAndCategory);
                    break;
                case 4:
                    shop.GetSorted(ItemSortField.PriceAndName);
                    break;
                default:
                    shop.GetSorted(ItemSortField.ArticleNumber);
                    break;
            }
        }

        static void ShowMainMenu()
        {
            Console.Clear();
            Console.WriteLine("Main menu");
            ShowSelection();
            ShowShopingCart();
            Console.WriteLine("Choose: 's<option>' to view all products sorted by: 1) Name 2) Price 3) Price and Category 4) Price and Name");
            Console.WriteLine("      : 'f<option>' to find by 1) Name 2) Price 3) Price and Name 4) Category, Price and Name");
            Console.WriteLine("      : '+<article number>' to put an article in your shopping cart");
            Console.WriteLine("      : '-<article number>' to remove an article from your shopping cart");
            Console.WriteLine("      : 'c' to checkout your shopping cart and 'quit' to quit.");
        }

        static void ShowSelection()
        {
            Console.WriteLine("Stock");
            foreach (InventoryLine item in shop.LastDoneQuery)
                Console.WriteLine(item);
        }
        static void ShowShopingCart()
        {
            var contents = shop.ShoppingCartContents;
            Console.WriteLine("Shopping cart");
            int i = 0;
            double totalAmount = 0;
            foreach (ShoppingCartLine line in shop.ShoppingCartContents)
            {
                i++;
                Console.WriteLine((i.ToString() + ")").PadRight(3) + line.ToString());
                totalAmount += line.NumItems * line.Price;
            }
            if (i>0)
                Console.WriteLine("{0} different products are in the cart for a total of                SEK {1}", i, totalAmount);
        }
    }
}
