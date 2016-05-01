namespace com.pharmscription.BusinessLogic.Identity
{
    using System;

    using com.pharmscription.BusinessLogic.Communication;
    using com.pharmscription.DataAccess.Entities.IdentityUserEntity;
    using com.pharmscription.DataAccess.Repositories.Identity.User;

    using Microsoft.AspNet.Identity;

    public class IdentitiyUserManager : UserManager<IdentityUser, Guid>
    {
        private IUserStore<IdentityUser, Guid> _store;
        public IIdentityMessageService EmailService { get; set; }

        public IdentitiyUserManager(IUserStore<IdentityUser, Guid> store)
            : base(store)
        {
            _store = store;

        }

        public static IdentitiyUserManager Create()
        {
            var store = 
            var manager = new IdentitiyUserManager(store);
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
            return manager;
        }
    }
}