using System;
using System.Diagnostics.CodeAnalysis;
using System.Security.Claims;
using System.Threading.Tasks;

using com.pharmscription.DataAccess.Entities.IdentityUserEntity;
using com.pharmscription.DataAccess.Repositories.Identity.User;

using Microsoft.AspNet.Identity;
using Microsoft.AspNet.Identity.Owin;
using Microsoft.Owin;
using Microsoft.Owin.Security;

namespace com.pharmscription.Service
{
    using com.pharmscription.BusinessLogic.Communication;

    [ExcludeFromCodeCoverage]
    public class ApplicationUserManager : UserManager<IdentityUser, Guid>
    {
        public ApplicationUserManager(IUserStore<IdentityUser, Guid> useraManager)
            : base(useraManager)
        {
        }

        public static ApplicationUserManager Create(
            IdentityFactoryOptions<ApplicationUserManager> options, 
            IOwinContext context)
        {
            var manager = new ApplicationUserManager(context.GetUserManager<IUserRepository>());
            manager.UserValidator = new UserValidator<IdentityUser, Guid>(manager)
            {
                AllowOnlyAlphanumericUserNames = false,
            };
            manager.PasswordValidator = new PasswordValidator()
            { 
                RequiredLength = 6,
                RequireNonLetterOrDigit = true,
                RequireDigit = true,
                RequireLowercase = true,
                RequireUppercase = true,
            };
            manager.EmailService = new EMailService();
            var dataProtectionProvider = options.DataProtectionProvider;
            if (dataProtectionProvider != null)
            {
                manager.UserTokenProvider = 
                    new DataProtectorTokenProvider<IdentityUser, Guid>(dataProtectionProvider.Create("Pharmscription Protection Provider"));
            }
            return manager;
        }
    }
    // Configure the application sign-in manager which is used in this application.
    [ExcludeFromCodeCoverage]
    public class ApplicationSignInManager : SignInManager<IdentityUser, Guid>
    {
        public ApplicationSignInManager(UserManager<IdentityUser, Guid> userManager, IAuthenticationManager authenticationManager)
            : base(userManager, authenticationManager)
        {
        }

        public override Task<ClaimsIdentity> CreateUserIdentityAsync(IdentityUser user)
        {
            return UserManager.CreateIdentityAsync(user, DefaultAuthenticationTypes.ApplicationCookie);
        }

        public static ApplicationSignInManager Create(IdentityFactoryOptions<ApplicationSignInManager> options, IOwinContext context)
        {
            return new ApplicationSignInManager(context.GetUserManager<ApplicationUserManager>(), context.Authentication);
        }
    }
}
