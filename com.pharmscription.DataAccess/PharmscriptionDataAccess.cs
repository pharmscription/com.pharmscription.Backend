using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using com.pharmscription.DataAccess.UnitOfWork;

namespace com.pharmscription.DataAccess
{
    // TEMPORARY CLASS FOR ACCESS UNIT OF WORK
    // WILL BE REMOVED AS SOON AS DI IS IMPLEMENTED
    public class PharmscriptionDataAccess
    {
        public IPharmscriptionUnitOfWork UnitOfWork => new PharmscriptionUnitOfWork();
    }
}
