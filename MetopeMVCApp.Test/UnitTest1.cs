using System;
using Microsoft.VisualStudio.TestTools.UnitTesting;
using MetopeMVCApp.Models;
using Metope.DAL;

namespace MetopeMVCApp.Test
{
    /// <summary>
    /// test to create a History record (with a price) for the Security
    /// 
    /// </summary>
    [TestClass]
    public class UnitTest1
    {
        [TestMethod]
        public void TestMethod1()
        {
            // ARRANGE
            var data = CreateSecurity("Sec1");
            data = CreateSecurity("Sec2");  

            data = new Security_Detail();
            data.Security_Name = "22x";

            // ACT 
            var someHistory = new SecurityDetailHistory(data);
            var result = someHistory.CreateNewOne();

            // ASSERT
            Assert.AreEqual(5, result.PriceHist);

        }
        public Security_Detail CreateSecurity(string name)
        {
            var security = new Security_Detail(); 
            security.Security_Name = name;
            security.Price_Curr = "ZAR"; 
            security.Portfolio =      new Portfolio { Portfolio_Name = "Port1" };

            return security;
}
        }
    }
