using System;
using System.Collections.Generic;
using System.Collections;
using System.Linq;
/* Requirements:
 * ItemStorage<T> : IEnumerable<T> 
 * o ID o InternalStorage[T] 
 * o GetAllItems() 
 * o SearchFunctions – If they are used by both ShopStorage and ShoppingCart (if any) 
 * o SortFunctions – If they are used by both ShoppingCart and ShopStorage (if any) 
 * o Implementation of IEnumerable
 * 

 * 
 */
namespace WebbutikM
{
    class ItemStorage<T>: IEnumerable<T>
    {
        private List<T> itemStorage = new List<T>();
        public int ID { get; set; }

        IEnumerator<T> IEnumerable<T>.GetEnumerator()
        {
            return itemStorage.GetEnumerator();
        }

        System.Collections.IEnumerator System.Collections.IEnumerable.GetEnumerator()
        {
            return itemStorage.GetEnumerator();
        }

        public IEnumerable<T> GetAllItems()
        {
            return itemStorage;
        }


    }
}
