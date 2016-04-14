
using System;

namespace com.pharmscription.BusinessLogic
{
    public class CoreWorkflow
    {
        public CoreWorkflow(Context context)
        {
            Context = context;
        }
        
        public Context Context { get; set; }
    }
}
