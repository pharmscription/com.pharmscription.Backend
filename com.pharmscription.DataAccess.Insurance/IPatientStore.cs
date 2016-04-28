using System.Collections.Generic;
using com.pharmscription.Infrastructure.ExternalDto.InsuranceDto;

namespace com.pharmscription.DataAccess.Insurance
{
    using System.Diagnostics.CodeAnalysis;

    /// <summary>
    /// The PatientStore interface to fake a connection to a storage of patients from an insurance.
    /// </summary>
    public interface IPatientStore
    {
        /// <summary>
        /// Gets a list of patients from an insurance.
        /// </summary>
        List<InsurancePatient> Patients { get; }
    }
}