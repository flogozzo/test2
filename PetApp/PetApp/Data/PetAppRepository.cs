using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using PetApp.Models;
using System.Net.Http;
using System.Configuration;
using System.Threading.Tasks;

namespace PetApp.Data
{
    public class PetAppRepository : IPetAppRepository
    {
        string dataSourceUrl;

        public PetAppRepository()
        {
            dataSourceUrl=  ConfigurationManager.AppSettings["DataSourceUrl"];
        }
        public async Task<IEnumerable<Person>> GetPersons()
        {
            IEnumerable<Person> persons = null;

            using (var client = new HttpClient())
            {
                
                var responseTask =  client.GetAsync(dataSourceUrl);
                await responseTask;

                var result = responseTask.Result;
                if (result.IsSuccessStatusCode)
                {
                    var readTask = await result.Content.ReadAsAsync<IList<Person>>();
                    //await readTask;

                    persons = readTask;
                }
                
            }
            return persons;
        }
    }
}