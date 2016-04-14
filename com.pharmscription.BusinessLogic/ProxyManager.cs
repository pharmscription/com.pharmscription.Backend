using System;
using System.Collections.Generic;
using System.Linq;
using System.Reflection;
using System.Runtime.Remoting.Messaging;
using System.Runtime.Remoting.Proxies;
using System.Text;
using System.Threading.Tasks;
using com.pharmscription.BusinessLogic.Patient;
using com.pharmscription.Infrastructure.Exception;
using com.pharmscription.Security;
using com.pharmscription.Security.SessionStore;

namespace com.pharmscription.BusinessLogic
{
    public class ProxyManager<T> : RealProxy
    {
        private readonly T _instance;
        private readonly Session _session;

        private ProxyManager(T instance, Session session)
            : base(typeof(T))
        {
            _instance = instance;
            _session = session;
        }

        public static T Create(T instance, Session session)
        {
            return (T)new ProxyManager<T>(instance, session).GetTransparentProxy();
        }

        public override IMessage Invoke(IMessage msg)
        {
            var methodCall = (IMethodCallMessage)msg;
            var method = (MethodInfo)methodCall.MethodBase;
            bool found = true;
            try
            {
                if (method.GetCustomAttributes(typeof(AuthorizedAttribute)).Any())
                {
                    found = false;
                    Console.WriteLine("Permission Check...");

                    foreach (var attribute in method.GetCustomAttributes(typeof(AuthorizedAttribute)))
                    {
                        var a = (AuthorizedAttribute)attribute;
                        if (a.Role == _session.Role) { Console.WriteLine("Permission okay"); found = true; }
                    }
                }

                if (found)
                {
                    var result = method.Invoke(_instance, methodCall.InArgs);
                    return new ReturnMessage(result, null, 0, methodCall.LogicalCallContext, methodCall);
                }
                
                throw new UnauthorizedException();    
            }
            catch (Exception e)
            {
                Console.WriteLine("Exception: " + e);
                if (e is TargetInvocationException && e.InnerException != null)
                {
                    return new ReturnMessage(e.InnerException, methodCall);
                }

                return new ReturnMessage(e, methodCall);
            }
        }
    }
}
