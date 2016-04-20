using System;
using System.ServiceModel;
using System.ServiceModel.Activation;
using com.pharmscription.BusinessLogic.Drug;
using com.pharmscription.BusinessLogic.Patient;
using com.pharmscription.BusinessLogic.Prescription;
using com.pharmscription.DataAccess.Repositories.Drug;
using com.pharmscription.DataAccess.Repositories.Patient;
using com.pharmscription.DataAccess.Repositories.Prescription;
using com.pharmscription.DataAccess.SwissMedic;
using com.pharmscription.DataAccess.UnitOfWork;
using Microsoft.Practices.Unity;

namespace com.pharmscription.Service.DependencyInjection
{
    public class UnityServiceHostFactory : ServiceHostFactory
    {
        private readonly IUnityContainer _container;

        public UnityServiceHostFactory()
        {
            _container = new UnityContainer();
            RegisterTypes(_container);
        }

        protected override ServiceHost CreateServiceHost(
          Type serviceType, Uri[] baseAddresses)
        {
            return new UnityServiceHost(_container,
              serviceType, baseAddresses);
        }

        private void RegisterTypes(IUnityContainer container)
        {
            container.RegisterType<IPharmscriptionUnitOfWork, PharmscriptionUnitOfWork>();
            container.RegisterType<IPatientRepository, PatientRepository>();
            container.RegisterType<IDrugRepository, DrugRepository>();
            container.RegisterType<ISwissMedic, SwissMedicMock>();
            container.RegisterType<IDrugManager, DrugManager>();
            container.RegisterType<IPatientManager, PatientManager>();
            container.RegisterType<IPrescriptionRepository, PrescriptionRepository>();
            container.RegisterType<IPrescriptionManager, PrescriptionManager>();
        }
    }
}
