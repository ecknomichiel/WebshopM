using System;
using System.Collections.Generic;

namespace WebbutikM
{
    class Program
    {
        static void Main(string[] args)
        {
            ShopLogic shop = new ShopLogic();

            ShowShopingCart(shop);
            Console.ReadKey();
        }

        static void ShowShopingCart(ShopLogic shop)
        {
            Console.WriteLine("Shopping cart");
            int i = 0;
            double totalAmount = 0;
            foreach (ShoppingCartLine line in shop.ShoppingCartContents)
            {
                i++;
                Console.WriteLine("{0}) {1} {2} {3} {4}x SEK{5}= SEK{6}",
                    i.ToString().PadRight(2),//0
                    line.ArticleNumber.ToString().PadRight(5),//1
                    line.Name.PadRight(30),//2
                    line.Category.PadRight(10),//3
                    line.NumItems.ToString().PadRight(2),//4
                    line.Price.ToString().PadRight(6),//5
                    line.NumItems * line.Price);//6
                totalAmount += line.NumItems * line.Price;
            }
            Console.WriteLine("{0} different products are in the cart for a total of                SEK {1}", i, totalAmount);
        }
    }
}
