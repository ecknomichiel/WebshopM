using System;
using System.Collections.Generic;
using System.Linq;

namespace WebbutikM
{
    class Item: IEquatable<Item>, IComparable<Item>
    {
        public int ArticleNumber { get; set;}
        public string Name { get; set; }
        public double Price { get; set;}
        public string Category { get; set;}

    
        public bool Equals(Item other)
        {
 	        if (other == null)
            {
                return false;
            }
            return (ArticleNumber == other.ArticleNumber
                    && Name == other.Name
                    && Price == other.Price
                    && Category == other.Category);
        }

        public override bool Equals(object obj)
        {
            if (obj != null && !obj.GetType().Equals(this.GetType()))
            { 	    
                return Equals(obj as Item);
            }
            else
            {// Either other type or null
                return false;
            }
        }

        /*
         * operators are not overriden cf. https://msdn.microsoft.com/en-us/library/bsc2ak47(v=vs.110).aspx
         */

        public int CompareTo(Item other)
        {
            return ArticleNumber - other.ArticleNumber;
        }
        
    }
}
