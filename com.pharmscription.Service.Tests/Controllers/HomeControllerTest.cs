using System.Web.Mvc;
using Microsoft.VisualStudio.TestTools.UnitTesting;
using com.pharmscription.Service;
using com.pharmscription.Service.Controllers;

namespace com.pharmscription.Service.Tests.Controllers
{
    [TestClass]
    public class HomeControllerTest
    {
        [TestMethod]
        public void Index()
        {
            // Anordnen
            HomeController controller = new HomeController();

            // Aktion ausführen
            ViewResult result = controller.Index() as ViewResult;

            // Bestätigen
            Assert.IsNotNull(result);
            Assert.AreEqual("Home Page", result.ViewBag.Title);
        }
    }
}
