using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using PetApp.Models;
using System.Net.Http;
using System.Configuration;

namespace PetApp.Data
{
    public class PetAppRepository : IPetAppRepository
    {
        string dataSourceUrl;

        public PetAppRepository()
        {
            dataSourceUrl=  ConfigurationManager.AppSettings["DataSourceUrl"];
        }
        public IEnumerable<Person> GetPersons()
        {
            IEnumerable<Person> persons = null;

            using (var client = new HttpClient())
            {
                
                var responseTask = client.GetAsync(dataSourceUrl);
                responseTask.Wait();

                var result = responseTask.Result;
                if (result.IsSuccessStatusCode)
                {
                    var readTask = result.Content.ReadAsAsync<IList<Person>>();
                    readTask.Wait();

                    persons = readTask.Result;
                }
                
            }
            return persons;
        }
    }
}