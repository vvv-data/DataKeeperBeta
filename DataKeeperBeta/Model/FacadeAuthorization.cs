using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Bridge;
using Shared;

namespace Model
{
    class FacadeAuthorization : Authorization
    {
        static FacadeAuthorization facadeAuthorization = null;

        internal override bool CheckConfig()
        {
            return Processing.Config.CheckConfig();
        }

        internal override bool PassExists()
        {
            return Processing.ControlPass.PassExists();
        }

        
        
        internal override bool Login(ref string password)
        {
            if (PassExists())
            {
                if (Processing.ControlPass.PassLogin(password))
                    return true;
                else
                    Message.ShowError();
            }
            else
            {
                if (Processing.ControlPass.AddPass(password))
                {
                    Message.ShowSuccess();
                    password = null;
                }                    
                else
                    Message.ShowError();
            }
            return false;
        }
        

        public static FacadeAuthorization CreateFacadeAuthorization()
        {
            if(facadeAuthorization == null)
                facadeAuthorization = new FacadeAuthorization();
            return facadeAuthorization;
        }

        FacadeAuthorization()
        {
        }
    }
}
