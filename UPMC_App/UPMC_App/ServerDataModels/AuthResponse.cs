using System;
using System.Collections.Generic;
using System.Text;

namespace UPMC_App.ServerDataModels
{
    public class Api_UserDataIfo
    {
        public string sessionKey { get; set; }
        public string errorMessage { get; set; }
        public string welcomeText { get; set; }
        public string memberName { get; set; }
        public string memberid { get; set; }

    }
    public class Api_AuthResponse
    {
        public int Code { get; set; }
        public bool IsEntrustAccountLocked { get; set; }

        public List<string> SuccessMessages { get; set; }
        public List<string> ErrorMessages { get; set; }

        public Api_UserDataIfo UserData { get; set; }
        public Api_UserDataIfo UserDataNew { get; set; }
        public MenuStructureType Menu { get; set; }
    }
}
