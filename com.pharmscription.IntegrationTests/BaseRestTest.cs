using System;
using System.Collections.Generic;
using System.Linq;
using System.ServiceModel;
using System.Text;
using System.Threading.Tasks;
using com.pharmscription.Service;
using Microsoft.VisualStudio.TestTools.UnitTesting;

namespace com.pharmscription.IntegrationTests
{
    [TestClass]
    public class BaseRestTest
    {
        public RestService Client;

        [TestInitialize]
        public void Initialize()
        {
            Client = new RestService();
        }   

        [TestCleanup]
        public void CleanUp()
        {
           
        }
    }
}
