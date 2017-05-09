using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace WebbutikM
{
    public class Item: IEquatable<Item>, IComparable<Item>
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

        public override int GetHashCode()
        {
          /*  System.Security.Cryptography.MD5 hash = System.Security.Cryptography.MD5.Create();
            byte[] byteHash = hash.ComputeHash(String.Format("{0}/{1}/{2}/{3}", ArticleNumber, Name, Price, Category));
            StringBuilder sBuilder = new StringBuilder();

            // Loop through each byte of the hashed data 
            // and format each one as a hexadecimal string.
            for (int i = 0; i < byteHash.Length; i++)
            {
                sBuilder.Append(byteHash[i].ToString("x2"));
            }

            // Return the hexadecimal string.
            return sBuilder.ToString();*/
            return base.GetHashCode();
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
