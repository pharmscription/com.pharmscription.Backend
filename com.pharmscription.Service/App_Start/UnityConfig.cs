namespace com.pharmscription.Service
{
    using System;
    using System.Diagnostics.CodeAnalysis;
    using BusinessLogic.Drug;
    using BusinessLogic.DrugPrice;
    using BusinessLogic.Patient;
    using BusinessLogic.Prescription;
    using DataAccess.Repositories.CounterProposal;
    using DataAccess.Repositories.Dispense;
    using DataAccess.Repositories.Drug;
    using DataAccess.Repositories.DrugItem;
    using DataAccess.Repositories.DrugPrice;
    using DataAccess.Repositories.DrugStore;
    using DataAccess.Repositories.Patient;
    using DataAccess.Repositories.Prescription;
    using DataAccess.UnitOfWork;
    using Microsoft.Practices.Unity;
    using Reporting;

    /// <summary>
    /// Specifies the Unity configuration for the main container.
    /// </summary>
    [ExcludeFromCodeCoverage]
    public class UnityConfig
    {
        #region Unity Container
        private static readonly Lazy<IUnityContainer> Container = new Lazy<IUnityContainer>(() =>
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
            container.RegisterType<IDrugPriceManager, DrugPriceManager>();
            container.RegisterType<Reporter, Reporter>();
            container.RegisterType<PdfReportWriter, PdfReportWriter>();
            container.RegisterType<PrescriptionCrawler, PrescriptionCrawler>();
            container.RegisterType<IPrescriptionManager, PrescriptionManager>();
            container.RegisterType<IDrugManager, DrugManager>();
            container.RegisterType<IDrugPriceRepository, DrugPriceRepository>();
            container.RegisterType<IDrugStoreRepository, DrugStoreRepository>();
            container.RegisterType<ICounterProposalRepository, CounterProposalRepository>();
            container.RegisterType<IDrugItemRepository, DrugItemRepository>();
            container.RegisterType<IDispenseRepository, DispenseRepository>();
            container.RegisterType<IDrugRepository, DrugRepository>();
            container.RegisterType<IPatientRepository, PatientRepository>();
            container.RegisterType<IPrescriptionRepository, PrescriptionRepository>();
            container.RegisterType<IPharmscriptionUnitOfWork, PharmscriptionUnitOfWork>(new PerRequestLifetimeManager());
        }
    }
}
