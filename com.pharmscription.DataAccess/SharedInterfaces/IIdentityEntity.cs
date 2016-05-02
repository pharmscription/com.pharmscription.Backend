using System.Diagnostics.CodeAnalysis;
using com.pharmscription.DataAccess.Entities.IdentityUserEntity;

namespace com.pharmscription.DataAccess.SharedInterfaces
{
    

    /// <summary>
    /// Interface for saving users in person entities.
    /// </summary>
    public interface IIdentityEntity
    {
        /// <summary>
        /// <see cref="IdentityUser"/> which represents the authorization object for a person
        /// </summary>
        [SuppressMessage("StyleCop.CSharp.DocumentationRules", "SA1623:PropertySummaryDocumentationMustMatchAccessors", Justification = "Get or set is not appropriate here")]
        IdentityUser User { get; set; }
    }
}