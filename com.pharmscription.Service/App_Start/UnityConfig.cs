
namespace com.pharmscription.Service
{
    using System;
    using System.Diagnostics.CodeAnalysis;

    using com.pharmscription.BusinessLogic.Drug;
    using com.pharmscription.BusinessLogic.Identity;
    using com.pharmscription.BusinessLogic.Patient;
    using com.pharmscription.BusinessLogic.Prescription;
    using com.pharmscription.DataAccess;
    using com.pharmscription.DataAccess.Repositories.CounterProposal;
    using com.pharmscription.DataAccess.Repositories.Dispense;
    using com.pharmscription.DataAccess.Repositories.Doctor;
    using com.pharmscription.DataAccess.Repositories.Drug;
    using com.pharmscription.DataAccess.Repositories.Drugist;
    using com.pharmscription.DataAccess.Repositories.DrugItem;
    using com.pharmscription.DataAccess.Repositories.DrugstoreEmployee;
    using com.pharmscription.DataAccess.Repositories.DrugStoreEmployee;
    using com.pharmscription.DataAccess.Repositories.Patient;
    using com.pharmscription.DataAccess.Repositories.Prescription;
    using com.pharmscription.DataAccess.UnitOfWork;

    using Microsoft.Practices.Unity;

    /// <summary>
    /// Specifies the Unity configuration for the main container.
    /// </summary>
    [ExcludeFromCodeCoverage]
    public class UnityConfig
    {
        #region Unity Container

        private static readonly Lazy<IUnityContainer> Container = new Lazy<IUnityContainer>(
            () =>
                {
                    var container = new UnityContainer();
                    RegisterTypes(container);
                    return container;
                });

        /// <summary>
        /// Gets the configured Unity container.
        /// </summary>
        public static IUnityContainer GetConfiguredContainer()
        {
            return Container.Value;
        }

        #endregion

        /// <summary>Registers the type mappings with the Unity container.</summary>
        /// <param name="container">The unity container to configure.</param>
        /// <remarks>There is no need to register concrete types such as controllers or API controllers (unless you want to 
        /// change the defaults), as Unity allows resolving a concrete type even if it was not previously registered.</remarks>
        public static void RegisterTypes(IUnityContainer container)
        {
            container.RegisterType<IPatientManager, PatientManager>();
            container.RegisterType<IPrescriptionManager, PrescriptionManager>();
            container.RegisterType<IDrugManager, DrugManager>();
            container.RegisterType<ICounterProposalRepository, CounterProposalRepository>();
            container.RegisterType<IDrugItemRepository, DrugItemRepository>();
            container.RegisterType<IDispenseRepository, DispenseRepository>();
            container.RegisterType<IDrugRepository, DrugRepository>();
            container.RegisterType<IPatientRepository, PatientRepository>();
            container.RegisterType<IDoctorRepository, DoctorRepository>();
            container.RegisterType<IDrugistRepository, DrugistRepository>();
            container.RegisterType<IDrugstoreEmployeeRepository, DrugstoreEmployeeRepository>();

            container.RegisterType<IPrescriptionRepository, PrescriptionRepository>();
            container.RegisterType<IIdentityManager, IdentityManager>();
            container.RegisterType<IPharmscriptionDataAccess, PharmscriptionDataAccess>();
            container.RegisterType<IPharmscriptionUnitOfWork, PharmscriptionUnitOfWork>(
                new ContainerControlledLifetimeManager());
            /*(new ContainerControlledLifetimeManager());
            // Following code will return a singleton instance of MySingletonObject
            // Container will take over lifetime management of the object
            myContainer.Resolve<IMyObject>();*/
            // container.RegisterType<IProductRepository, ProductRepository>();
        }
    }
}

