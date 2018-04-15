using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace PetApp.Models
{
    public class Pet
    {
        public const string constCAT="Cat";
        public const string constDOG = "Dog";
        public const string constFISH = "Fish";
        public string Name { get; set; }
        public string Type { get; set; }
    }
    public class PetComparer : IEqualityComparer<Pet>
    {

        public bool Equals(Pet x, Pet y)
        {
            if (Object.ReferenceEquals(x, y)) return true;

            if (Object.ReferenceEquals(x, null) || Object.ReferenceEquals(y, null))
                return false;

            return x.Type == y.Type && x.Name == y.Name;
        }

        public int GetHashCode(Pet obj)
        {
            if (Object.ReferenceEquals(obj, null)) return 0;

            int hashName = obj.Name == null ? 0 : obj.Name.GetHashCode();

            int hashType = obj.Type.GetHashCode();

            //Calculate the hash code for the product.
            return hashName ^ hashType;
        }
    }
}