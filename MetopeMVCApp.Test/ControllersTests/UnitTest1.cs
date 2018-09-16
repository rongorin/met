using System;
using System.Security.Principal;
using System.Web;
using System.Web.Mvc;
using System.Web.Routing;
using Microsoft.VisualStudio.TestTools.UnitTesting;
using MetopeMVCApp.Test.Model;
using MetopeMVCApp.Controllers;
using MetopeMVCApp.Data; 
using Metope.DAL;
using System.Collections.Generic;
using System.Security.Principal;
using System.Threading;

namespace MetopeMVCApp.Test.ControllersTests
{
    [TestClass]
    public class ForexForecastTests
    {
        private ForexForecastController controller;

        [TestInitialize]
        public void Init()
        {  
            var repository = new InMemoryForexForecastRepository();     

            // repository = new CustomerController(new FakeCustomerRepository()); 

            repository.Add(MakeForexForecast(1, 56, "201803", 99));
            repository.Add(MakeForexForecast(1, 56, "201804", 37));
            repository.Add(MakeForexForecast(1, 56, "201805", 8));

            var identity = new GenericIdentity("aaaaaa");
            Thread.CurrentPrincipal = new GenericPrincipal(identity, null); 

              controller = new ForexForecastController(repository);
              controller.ViewData["EntityId"] = 1;


        }

        [TestMethod]
        public void Index_returns_a_list_of_ForexForecast()
        {
           
            var results = controller.Index() as ViewResult;
            Assert.IsInstanceOfType(results, typeof(ViewResult));

            Assert.IsNotNull(results); 

        }
        [TestMethod]
        public void Edit_returns_record()
        {
            controller.ViewData["EntityId"] = 1;

            var results = controller.Edit(1, 56, "201805") as ViewResult; 
            var Myrepository = new InMemoryForexForecastRepository();
            Myrepository.Add(MakeForexForecast(1, 56, "201803", 37)); 

            Assert.AreEqual(Myrepository.FindById(1, 56, "201803").Forecast_Rate, 37); 
        }
        [TestMethod]
        public void Edit_returns_a_Model()
        {  //model returns 
            //Act
            var result = controller.Edit(1, 56, "201805") as ViewResult; 

            var myModel = (Forex_Forecast)result.Model;
             
            // Assert 
            Assert.IsTrue(myModel.Forecast_Rate  == 8);
             
        }
        [TestMethod]
        public void Create_redirects_back_to_Index_on_success()
        {    
            //httpcontext for controller:
            var identity = new GenericIdentity("aaaaaa");
            var controllerContext = new TestableControllerContext();
            var principal = new GenericPrincipal(identity, null);
            var testableHttpContext = new TestableHttpContext
            {
                User = principal
            };
            controllerContext.HttpContext = testableHttpContext;
            controller.ControllerContext = controllerContext;
               
            //Act 
            var result = (RedirectToRouteResult)controller.Create(MakeForexForecast(1, 56, "201807", 123))  ;

            Assert.AreEqual(result.RouteValues["action"], "Index");

        }
        public void Create_Forecast_creates_a_record_when_the_model_is_valid()
        {
            //httpcontext for controller:
            var identity = new GenericIdentity("aaaaaa");
            var controllerContext = new TestableControllerContext();
            var principal = new GenericPrincipal(identity, null);
            var testableHttpContext = new TestableHttpContext
            {
                User = principal
            };
            controllerContext.HttpContext = testableHttpContext;
            controller.ControllerContext = controllerContext;

            //Act 
             controller.Create(MakeForexForecast(1, 56, "201807", 123)); 
             var repo = new InMemoryForexForecastRepository();
             var forecast = repo.FindById(1, 56, "201807" );
             Assert.IsNotNull(forecast);
            
        } 

        //-------------------------------------------
        [TestMethod]
        public void Test_mocking_a_User()
        {
            var identity = new GenericIdentity("aaaaaa");

            //httpcontext for controller:
            var controllerContext = new TestableControllerContext();
            var principal = new GenericPrincipal(identity, null);
            var testableHttpContext = new TestableHttpContext
            {
                User = principal
            }; 
            controllerContext.HttpContext = testableHttpContext;

            //the controller:
            var controller = new QuickTestController(); 
            controller.ControllerContext = controllerContext;

            Assert.AreEqual(controller.Get(), identity.Name);
        }

        //this testing controller is just to test the user identity
        public class QuickTestController : Controller 
        {
            public string Get()
            {
                return User.Identity.Name;
            }
        } 

        public class TestableControllerContext : ControllerContext
        {
            public TestableHttpContext TestableHttpContext { get; set; }
        }

        public class TestableHttpContext : HttpContextBase
        {
            public override IPrincipal User { get; set; }
        }
        //------------------------------------------- 
        
        private static Forex_Forecast MakeForexForecast(int EntityId, int SecurityId, string MonthYear, decimal forecastRate = 0)
        {
            return new Forex_Forecast
                { Entity_ID = EntityId, Security_ID = SecurityId, Month_Year = MonthYear, Forecast_Rate = forecastRate };
        }
       
        
    }   
 
}
