using System;
using System.Diagnostics.CodeAnalysis;
using com.pharmscription.BusinessLogic.Drug;
using com.pharmscription.BusinessLogic.Patient;
using com.pharmscription.BusinessLogic.Prescription;
using com.pharmscription.DataAccess.Repositories.CounterProposal;
using com.pharmscription.DataAccess.Repositories.Dispense;
using com.pharmscription.DataAccess.Repositories.Drug;
using com.pharmscription.DataAccess.Repositories.DrugItem;
using com.pharmscription.DataAccess.Repositories.Patient;
using com.pharmscription.DataAccess.UnitOfWork;
using Microsoft.Practices.Unity;

namespace com.pharmscription.Service.App_Start
{
    using com.pharmscription.DataAccess.Repositories.Prescription;

    /// <summary>
    /// Specifies the Unity configuration for the main container.
    /// </summary>
    [ExcludeFromCodeCoverage]
    public class UnityConfig
    {
        #region Unity Container
        private static Lazy<IUnityContainer> container = new Lazy<IUnityContainer>(() =>
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
            return container.Value;
        }
        #endregion

        /// <summary>Registers the type mappings with the Unity container.</summary>
        /// <param name="container">The unity container to configure.</param>
        /// <remarks>There is no need to register concrete types such as controllers or API controllers (unless you want to 
        /// change the defaults), as Unity allows resolving a concrete type even if it was not previously registered.</remarks>
        public static void RegisterTypes(IUnityContainer container)
        {
            // NOTE: To load from web.config uncomment the line below. Make sure to add a Microsoft.Practices.Unity.Configuration to the using statements.
            // container.LoadConfiguration();

            // TODO: Register your types here
            container.RegisterType<IPatientManager, PatientManager>();
            container.RegisterType<IPrescriptionManager, PrescriptionManager>();
            container.RegisterType<IDrugManager, DrugManager>();
            container.RegisterType<ICounterProposalRepository, CounterProposalRepository>();
            container.RegisterType<IDrugItemRepository, DrugItemRepository>();
            container.RegisterType<IDispenseRepository, DispenseRepository>();
            container.RegisterType<IDrugRepository, DrugRepository>();
            container.RegisterType<IPatientRepository, PatientRepository>();
            container.RegisterType<IPrescriptionRepository, PrescriptionRepository>();
            container.RegisterType<IPharmscriptionUnitOfWork, PharmscriptionUnitOfWork>();
            // container.RegisterType<IProductRepository, ProductRepository>();
        }
    }
}
