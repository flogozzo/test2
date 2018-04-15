using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Web.Mvc;
using Microsoft.VisualStudio.TestTools.UnitTesting;
using PetApp;
using PetApp.Controllers;
using Moq;
using PetApp.Data;
using PetApp.Models;
using PetApp.Services;
using System.Threading.Tasks;

namespace PetApp.Tests.Controllers
{
    [TestClass]
    public class HomeControllerTest
    {
        IEnumerable<Person> people = new List<Person>{
                        new Person {Name ="Jeff", Age =23, Gender ="Male",
                                    Pets = new List<Pet>{ new Pet{ Name="Alfy", Type="Dog" }, new Pet{ Name="Blackie", Type="Cat" }, new Pet{ Name="Slimer", Type="Fish" }}},
                        new Person { Name = "Jill", Age = 45, Gender = "Female",
                                    Pets = new List<Pet>{ new Pet{ Name="Al", Type="Cat" }, new Pet{ Name="Poco", Type="Cat" }}},
                        new Person { Name = "Dave", Age = 77, Gender = "Male"},
                        new Person { Name = "Mary", Age = 34, Gender = "Female",
                                    Pets = new List<Pet>{ new Pet{ Name="Quin", Type="Cat" }}},
                        new Person { Name = "Mike", Age = 41, Gender = "Male",
                                    Pets = new List<Pet>{ new Pet{ Name="Sammy", Type="Cat" }, new Pet{ Name="Tammy", Type="Cat" }, new Pet{ Name="Coco", Type="Cat" }}},
            };
        [TestMethod]
        public void Index()
        {            
            var mockRepo = new Mock<IPetAppRepository>();
            mockRepo.Setup(x => x.GetPersons()).Returns(Task.FromResult(people));

            var mockLogger = new Mock<ILogger>();

            // Arrange
            HomeController controller = new HomeController(mockRepo.Object,mockLogger.Object);

            // Act
            var actionResultTask = controller.Index();
            actionResultTask.Wait();
            var result = actionResultTask.Result as ViewResult;
            // Assert
            Assert.IsNotNull(result);
            Assert.AreEqual("", result.ViewName);
        }
        [TestMethod]
        public void TestIndexViewModel()
        {
            List<IndexViewModel> expectedResult = new List<IndexViewModel>() { new IndexViewModel {OwnerGender ="Female",
                                                                                    Pets = new List<Pet> { new Pet { Name="Al", Type="Cat" }, new Pet { Name="Poco", Type="Cat" }, new Pet { Name="Quin", Type="Cat" } }},
                                                                                new IndexViewModel {OwnerGender ="Male", 
                                                                                    Pets = new List<Pet> { new Pet { Name = "Blackie", Type = "Cat" }, new Pet { Name="Coco", Type="Cat" },  new Pet { Name="Sammy", Type="Cat" }, new Pet { Name="Tammy", Type="Cat" } }},
                                                                            };
                        
            // Arrange
            var mockRepo = new Mock<IPetAppRepository>();
            mockRepo.Setup(x => x.GetPersons()).Returns(Task.FromResult(people));
            var mockLogger = new Mock<ILogger>();
            HomeController controller = new HomeController(mockRepo.Object, mockLogger.Object);

            // Act
            var actionResultTask = controller.Index();
            actionResultTask.Wait();
            var result = actionResultTask.Result as ViewResult;

            // Assert
            Assert.IsNotNull(result);
            var resultModel = (List<IndexViewModel>)result.ViewData.Model;
            Assert.IsNotNull(resultModel);
            Assert.IsTrue(resultModel.Count == 2);

            //check the number and ordering is correct
            IndexViewModel m1 = resultModel.Find(n => n.OwnerGender == "Female");
            IndexViewModel m2 = expectedResult.Find(n => n.OwnerGender == "Female");
            Assert.IsTrue(m2.Pets.SequenceEqual(m1.Pets, new PetComparer()),"Comparison of female owner's cats failed");

            m1 = resultModel.Find(n => n.OwnerGender == "Male");
            m2 = expectedResult.Find(n => n.OwnerGender == "Male");
            Assert.IsTrue(m2.Pets.SequenceEqual(m1.Pets, new PetComparer()), "Comparison of male owner's cats failed");

        }
        [TestMethod]
        public void TestIndexViewModelIncorrectOrder()
        {
            List<IndexViewModel> expectedResult = new List<IndexViewModel>() { new IndexViewModel {OwnerGender ="Female",
                                                                                    Pets = new List<Pet> { new Pet { Name="Poco", Type="Cat" }, new Pet { Name = "Al", Type = "Cat" }, new Pet { Name="Quin", Type="Cat" } }},
                                                                                new IndexViewModel {OwnerGender ="Male",
                                                                                    Pets = new List<Pet> { new Pet { Name="Coco", Type="Cat" }, new Pet { Name = "Blackie", Type = "Cat" }, new Pet { Name="Sammy", Type="Cat" }, new Pet { Name="Tammy", Type="Cat" } }},
                                                                            };

            // Arrange
            var mockRepo = new Mock<IPetAppRepository>();
            mockRepo.Setup(x => x.GetPersons()).Returns(Task.FromResult(people));
            var mockLogger = new Mock<ILogger>();
            HomeController controller = new HomeController(mockRepo.Object, mockLogger.Object);

            // Act
            var actionResultTask = controller.Index();
            actionResultTask.Wait();
            var result = actionResultTask.Result as ViewResult;

            // Assert
            Assert.IsNotNull(result);
            var resultModel = (List<IndexViewModel>)result.ViewData.Model;
            Assert.IsNotNull(resultModel);
            Assert.IsTrue(resultModel.Count == 2);

            //check that the ordering test returns false when the order in not alphabetical
            IndexViewModel m1 = resultModel.Find(n => n.OwnerGender == "Female");
            IndexViewModel m2 = expectedResult.Find(n => n.OwnerGender == "Female");
            Assert.IsTrue(m2.Pets.SequenceEqual(m1.Pets, new PetComparer()) == false, "Comparison of female owner's cats failed");

            m1 = resultModel.Find(n => n.OwnerGender == "Male");
            m2 = expectedResult.Find(n => n.OwnerGender == "Male");
            Assert.IsTrue(m2.Pets.SequenceEqual(m1.Pets, new PetComparer()) == false, "Comparison of male owner's cats failed");

        }

        [TestMethod]
        public void TestIndexViewModelIncorrectCount()
        {
            List<IndexViewModel> expectedResult = new List<IndexViewModel>() { new IndexViewModel {OwnerGender ="Female",
                                                                                    Pets = new List<Pet> { new Pet { Name="Al", Type="Cat" }, new Pet { Name="Poco", Type="Cat" } }},
                                                                                new IndexViewModel {OwnerGender ="Male",
                                                                                    Pets = new List<Pet> { new Pet { Name = "Blackie", Type = "Cat" }, new Pet { Name="Coco", Type="Cat" },  new Pet { Name="Sammy", Type="Cat" } }},
                                                                            };

            // Arrange
            var mockRepo = new Mock<IPetAppRepository>();
            mockRepo.Setup(x => x.GetPersons()).Returns(Task.FromResult(people));
            var mockLogger = new Mock<ILogger>();
            HomeController controller = new HomeController(mockRepo.Object, mockLogger.Object);

            // Act
            var actionResultTask = controller.Index();
            actionResultTask.Wait();
            var result = actionResultTask.Result as ViewResult;

            // Assert
            Assert.IsNotNull(result);
            var resultModel = (List<IndexViewModel>)result.ViewData.Model;
            Assert.IsNotNull(resultModel);
            Assert.IsTrue(resultModel.Count == 2);

            //check that the ordering test returns false when the order in not alphabetical
            IndexViewModel m1 = resultModel.Find(n => n.OwnerGender == "Female");
            IndexViewModel m2 = expectedResult.Find(n => n.OwnerGender == "Female");
            Assert.IsTrue(m2.Pets.SequenceEqual(m1.Pets, new PetComparer()) == false, "Comparison of female owner's cats failed");

            m1 = resultModel.Find(n => n.OwnerGender == "Male");
            m2 = expectedResult.Find(n => n.OwnerGender == "Male");
            Assert.IsTrue(m2.Pets.SequenceEqual(m1.Pets, new PetComparer()) == false, "Comparison of male owner's cats failed");

        }
        [TestMethod]
        public void About()
        {
            // Arrange
            //HomeController controller = new HomeController();

            //// Act
            //ViewResult result = controller.About() as ViewResult;

            //// Assert
            //Assert.AreEqual("Your application description page.", result.ViewBag.Message);
        }

        [TestMethod]
        public void Contact()
        {
            //// Arrange
            //HomeController controller = new HomeController();

            //// Act
            //ViewResult result = controller.Contact() as ViewResult;

            //// Assert
            //Assert.IsNotNull(result);
        }
    }
}
