using System;
using Microsoft.VisualStudio.TestTools.UnitTesting;
using System.Web.Mvc;
using MetopeMVCApp.Models;
using MetopeMVCApp.Controllers;

namespace MetopeMVCApp.Test
{
    [TestClass]
    public class UnitTest2
    {
        [TestMethod]
        public void TestMethod1()
        { 


            ViewResult vr = new ViewResult();  
            Controller cc;
            Portfolio portf;

            //var pc = new PortfolioController();

            var controller = new PortfolioController()
            {
                GetUserId = () => "IdOfYourChoosing"
            };

            vr = (ViewResult)controller.Details(2, "MET");

            Assert.AreEqual("Details", vr.ViewName);
           

        }
    }
}
