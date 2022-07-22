using System.Collections.Specialized;
using Shared;
using System.Windows;
using Bridge;

namespace View
{
    internal class MediatorAuthorization : Authorization
    {
        static MediatorAuthorization mediator = null;

        internal override bool Login(ref string pass)
        {
            return bridge.Login(ref pass);
        }

        internal override bool CheckConfig()
        {
            return bridge.CheckConfig();
        }

        internal override bool PassExists()
        {
            return bridge.PassExists();
        }

        internal static void CreateMediatorContent()
        {
            MediatorContent.CreateMediator();
        }

        internal static MediatorAuthorization CreateMediator()
        {
            if(mediator == null)
                mediator = new MediatorAuthorization();
            
            WindowPassword.Mediator = mediator;
            WindowPassword.CreateWindowPassword();

            return mediator;
        }

        MediatorAuthorization() 
        {
            CreateBridge();            
        }

    }
}
