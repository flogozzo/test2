using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace PetApp.Models
{
    public class IndexViewModel
    {
        public string  OwnerGender { get; set; }
        //public string PetName { get; set; }
        public ICollection<Pet> Pets { get; set; }
    }
}