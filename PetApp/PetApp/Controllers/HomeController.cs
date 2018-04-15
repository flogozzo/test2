using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;
using PetApp.Data;
using PetApp.Services;
using PetApp.Models;
using PetApp.BL;
using System.Threading.Tasks;
namespace PetApp.Controllers
{
    public class HomeController : Controller
    {
        private IPetAppRepository _repo;
        private ILogger _logger;
        private PetsBL _bl;

        public HomeController(IPetAppRepository repo, ILogger logger)
        {
            _repo = repo;
            _logger = logger;
            _bl = new PetsBL();
        }
        public async Task<ActionResult> Index()
        {
            IEnumerable<IndexViewModel> view;
            try
            {
                IEnumerable<Person> persons = await _repo.GetPersons();
                view = _bl.GetPetsByOwnerGender(persons, Pet.constCAT);
                // log the results.
                foreach (var p in view)
                {
                    _logger.LogMessage(string.Format("Owner gender: {0}", p.OwnerGender));
                    foreach(var pet in p.Pets)
                        _logger.LogMessage(string.Format(" Pet name: {0}, type: {1}", pet.Name, pet.Type));
                }
            }
            catch (Exception ex)
            {
                _logger.LogMessage("Exception: " + ex.Message);
                return View("Error",ex);
            }
            return View(view);
        }

        public ActionResult About()
        {
            ViewBag.Message = "Your application description page.";

            return View();
        }

        public ActionResult Contact()
        {
            ViewBag.Message = "Your contact page.";

            return View();
        }
    }
}