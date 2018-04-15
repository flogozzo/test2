using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using PetApp.Models;

namespace PetApp.Data
{
    public interface IPetAppRepository
    {
        IEnumerable<Person> GetPersons();
    }


}
