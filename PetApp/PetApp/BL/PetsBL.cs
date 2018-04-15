using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using PetApp.Models;

namespace PetApp.BL
{
    public class PetsBL
    {
        public IEnumerable<IndexViewModel> GetPetsByOwnerGender(IEnumerable<Person> persons, string petType)
        {
            List<IndexViewModel> viewModel = new List<IndexViewModel>();

            var query = persons
                       .Where(person => person.Pets != null)
                       .SelectMany(person => person.Pets.Where(p => p.Type == petType), (person, pet) => new { OwnerGender = person.Gender, PetName = pet.Name, PetType = pet.Type })
                       .GroupBy(p => p.OwnerGender);
            foreach (var group in query)
            {
                var sorted = group.OrderBy(p => p.PetName);

                IndexViewModel m = new IndexViewModel() { Pets = new List<Pet>()};
                m.OwnerGender = group.Key;
                foreach(var p in sorted)
                {
                    m.Pets.Add(new Pet { Name = p.PetName, Type = p.PetType });
                }
                viewModel.Add(m);
            }

            return viewModel;

        }
    }
}