using System;
using System.Collections.Generic;
using System.Linq;
using System.Linq.Expressions;
using System.Text;
using System.Threading.Tasks;
using com.pharmscription.DataAccess.Entities.PatientEntity;
using com.pharmscription.DataAccess.Repositories.Patient;
using com.pharmscription.DataAccess.SharedInterfaces;

namespace com.pharmscription.BusinessLogic.Tests
{
    class PatientRepository: IPatientRepository
    {
        public IUnitOfWork UnitOfWork { get; }
        public void Add(Patient item)
        {
            throw new NotImplementedException();
        }

        public void Remove(Patient item)
        {
            throw new NotImplementedException();
        }

        public void TrackItem(Patient item)
        {
            throw new NotImplementedException();
        }

        public void UntrackItem(Patient item)
        {
            throw new NotImplementedException();
        }

        public void Merge(Patient persisted, Patient current)
        {
            throw new NotImplementedException();
        }

        public Patient Get(Guid id)
        {
            throw new NotImplementedException();
        }

        public IEnumerable<Patient> Find(Guid id)
        {
            throw new NotImplementedException();
        }

        public IEnumerable<Patient> GetAll()
        {
            throw new NotImplementedException();
        }

        public IEnumerable<Patient> GetAllAsNoTracking()
        {
            throw new NotImplementedException();
        }

        public IEnumerable<Patient> GetPaged<TKProperty>(int pageIndex, int pageCount, Expression<Func<Patient, TKProperty>> orderByExpression, bool @ascending)
        {
            throw new NotImplementedException();
        }

        public IEnumerable<Patient> GetFiltered(Expression<Func<Patient, bool>> filter)
        {
            throw new NotImplementedException();
        }

        public int Count()
        {
            throw new NotImplementedException();
        }

        public int Count(Func<Patient, bool> predicate)
        {
            throw new NotImplementedException();
        }

        public Task<Patient> GetByAhvNumber(string ahvNumber)
        {
            throw new NotImplementedException();
        }

        public bool Exists(string ahvNumber)
        {
            throw new NotImplementedException();
        }
    }
}
