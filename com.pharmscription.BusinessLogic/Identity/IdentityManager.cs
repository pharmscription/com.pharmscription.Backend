using com.pharmscription.DataAccess;
using com.pharmscription.DataAccess.Identity;

using Microsoft.AspNet.Identity;
using Microsoft.AspNet.Identity.EntityFramework;

namespace com.pharmscription.BusinessLogic.Identity
{
    using System;
    using System.Collections.Generic;
    using System.Linq;
    using System.Security.Claims;

    using com.pharmscription.DataAccess.Entities.BaseUser;
    using com.pharmscription.DataAccess.Entities.DoctorEntity;
    using com.pharmscription.DataAccess.Entities.DrugistEntity;
    using com.pharmscription.DataAccess.Entities.DrugstoreEmployeeEntity;
    using com.pharmscription.DataAccess.Entities.PatientEntity;
    using com.pharmscription.DataAccess.Entities.PhaIdentiyUser;
    using com.pharmscription.DataAccess.Repositories.Doctor;
    using com.pharmscription.DataAccess.Repositories.Drugist;
    using com.pharmscription.DataAccess.Repositories.DrugStoreEmployee;
    using com.pharmscription.DataAccess.Repositories.Patient;
    using com.pharmscription.DataAccess.SharedInterfaces;
    using com.pharmscription.Infrastructure.Constants;
    using com.pharmscription.Infrastructure.Exception;

    public class IdentityManager : IIdentityManager
    {
        private UserManager<PhaIdentityUser> userManager;
        private RoleManager<IdentityRole> roleManager;

        public List<AccountRoles> Roles = new List<AccountRoles>() { AccountRoles.Patient, AccountRoles.Doctor, AccountRoles.Drugist, AccountRoles.DrugstoreEmployee };

        private IPatientRepository _patientRepository;
        private IDoctorRepository _doctorRepository;
        private IDrugstoreEmployeeRepository _drugstoreEmployeeRepository;
        private IDrugistRepository _drugistRepository;

        public IdentityManager(IPharmscriptionDataAccess db, IPatientRepository patientRepository, IDoctorRepository doctorRepository, IDrugstoreEmployeeRepository drugstoreEmployeeRepository, IDrugistRepository drugistRepository)
        {

            _patientRepository = patientRepository;
            this._doctorRepository = doctorRepository;
            this._drugstoreEmployeeRepository = drugstoreEmployeeRepository;
            _drugistRepository = drugistRepository;

            UserStore<PhaIdentityUser> userStore = new UserStore<PhaIdentityUser>(db.IdentityDbContext);
            userManager = new UserManager<PhaIdentityUser>(userStore);

            RoleStore<IdentityRole> roleStore = new RoleStore<IdentityRole>(db.IdentityDbContext);
            roleManager = new RoleManager<IdentityRole>(roleStore);

            foreach (AccountRoles role in Roles)
            {
                if (!roleManager.RoleExists(role.ToString()))
                {
                    roleManager.Create(new IdentityRole(role.ToString()));
                }
            }
        }

        public UserManager<PhaIdentityUser> UserManager => userManager;

        public RoleManager<IdentityRole> RoleManager => roleManager;

        public ClaimsIdentity GetClaimsIdentity(PhaIdentityUser user)
        {
            return this.userManager.CreateIdentity(user, DefaultAuthenticationTypes.ApplicationCookie);
        }

        public IdentityResult CreateAccount(string user, string password, AccountRoles role)
        {
            PhaIdentityUser newUser = new PhaIdentityUser();
            newUser.UserName = user;

            switch (role)
            {
                case AccountRoles.Patient:
                    this.CreatePatient(user, newUser);
                    break;
                case AccountRoles.Doctor:
                    this.CreateDoctor(user, newUser);
                    break;

                case AccountRoles.DrugstoreEmployee:
                    this.CreateDrugstoreEmployee(user, newUser);
                    break;

                case AccountRoles.Drugist:
                    this.CreateDrugist(user, newUser);
                    break;
            }

            if (newUser.UserId == Guid.Empty)
            {
                throw new InvalidProgramException("Couldn't create user account");
            }

            IdentityResult result = userManager.Create(newUser, password);

            if (result.Succeeded)
            {
                var createdUser = userManager.AddToRole(newUser.Id, role.ToString());
                return createdUser;
            }
            throw new InvalidArgumentException(result.Errors.ToList().ToString());
        }

        private void CreateDrugist(string user, PhaIdentityUser newUser)
        {
            Drugist addedDrugist =
                this._drugistRepository.Add(
                    new Drugist { Role = AccountRoles.Drugist.ToString(), FirstName = user, LastName = user });
            this._drugistRepository.UnitOfWork.Commit();
            newUser.UserId = addedDrugist.Id;
        }

        private void CreateDrugstoreEmployee(string user, PhaIdentityUser newUser)
        {
            DrugstoreEmployee addedDrugstoreEmployee =
                this._drugstoreEmployeeRepository.Add(
                    new DrugstoreEmployee
                    {
                        Role = AccountRoles.DrugstoreEmployee.ToString(),
                        FirstName = user,
                        LastName = user
                    });
            this._drugstoreEmployeeRepository.UnitOfWork.Commit();
            newUser.UserId = addedDrugstoreEmployee.Id;
        }

        private void CreateDoctor(string user, PhaIdentityUser newUser)
        {
            Doctor addedDoctor =
                this._doctorRepository.Add(
                    new Doctor { Role = AccountRoles.Doctor.ToString(), FirstName = user, LastName = user });
            this._doctorRepository.UnitOfWork.Commit();
            newUser.UserId = addedDoctor.Id;
        }

        private void CreatePatient(string user, PhaIdentityUser newUser)
        {
            Patient addedPatient =
                this._patientRepository.Add(
                    new Patient
                    {
                        Role = AccountRoles.Patient.ToString(),
                        FirstName = user,
                        LastName = user,
                        BirthDate = DateTime.Now
                    });
            this._patientRepository.UnitOfWork.Commit();
            newUser.UserId = addedPatient.Id;
        }

        public PhaIdentityUser Find(string userName, string password)
        {
            return this.UserManager.Find(userName, password);
        }

        public BaseUser GetUser(Guid id, AccountRoles role)
        {
            switch (role)
            {
                case AccountRoles.Doctor:
                    return this._doctorRepository.Find(id).FirstOrDefault();
                case AccountRoles.Drugist:
                    return this._drugistRepository.Find(id).FirstOrDefault();
                case AccountRoles.DrugstoreEmployee:
                    return this._drugstoreEmployeeRepository.Find(id).FirstOrDefault();
                case AccountRoles.Patient:
                    return this._patientRepository.Find(id).FirstOrDefault();
            }

            throw new InvalidArgumentException("Role is invalid");
        }
    }
}
