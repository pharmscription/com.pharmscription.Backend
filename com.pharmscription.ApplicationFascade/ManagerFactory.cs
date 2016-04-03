using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using com.pharmscription.BusinessLogic.Patient;
using com.pharmscription.DataAccess;
using com.pharmscription.DataAccess.Repositories.Patient;
using com.pharmscription.DataAccess.UnitOfWork;

namespace com.pharmscription.ApplicationFascade
{
    class ManagerFactory
    {
        public IPatientManager PatientManager
        {
            get
            {
                IPharmscriptionUnitOfWork puow = new PharmscriptionDataAccess().UnitOfWork;
                IPatientRepository patientRepository = new PatientRepository(puow);
                return new PatientManager(patientRepository);
            }
        }

    }
}
