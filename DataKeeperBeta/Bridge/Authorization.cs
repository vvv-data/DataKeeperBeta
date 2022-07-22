using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Model;

namespace Bridge
{
    abstract class Authorization
    {
        static public FacadeAuthorization bridge = null;

        public static void CreateBridge()
        {
            bridge = FacadeAuthorization.CreateFacadeAuthorization();
        }

        abstract internal bool CheckConfig();
        abstract internal bool PassExists();
        abstract internal bool Login(ref string pass);

    }
}
