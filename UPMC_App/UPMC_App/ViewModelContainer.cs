using System;
using System.Collections.Generic;
using System.Text;
using UPMC_App.ViewModels;

namespace UPMC_App
{
    public class ViewModelContainer
    {
        #region Constants
        public static string PrivateKey = "SjrTgyLSax91Wg38QLjj1OkjfNwLkgPcAh7QPSIZpR1m93tZOMiPBD9QqFBLVrXvI5mxX6xmWxJ5Ac4pZmeAZnrp33aYt9ubdQJaVgxSOhXpt2Ptn44mquMtxXr27V";

        public static string AppName = "UMPC Health Plan";
        #endregion

        private static HomeViewModel _homeVM;
        private static MenuViewModel _menuVM;
        private static PrescriptionsViewModel _prescriptionsVM;
        private static ClaimsViewModel _claimsVM;
        private static YourProvidersViewModel _yourProvidersVM;

        public static HomeViewModel HomeVM
        {
            get
            {
                if (_homeVM == null)
                    _homeVM = new HomeViewModel();

                return _homeVM;
            }
        }

        public static MenuViewModel MenuVM
        {
            get
            {
                if (_menuVM == null)
                    _menuVM = new MenuViewModel();
                return _menuVM;
            }
        }

        public static PrescriptionsViewModel PrescriptionsVM
        {
            get
            {
                if (_prescriptionsVM == null)
                    _prescriptionsVM = new PrescriptionsViewModel();
                return _prescriptionsVM;
            }
        }

        public static ClaimsViewModel ClaimsVM
        {
            get
            {
                if (_claimsVM == null)
                    _claimsVM = new ClaimsViewModel();
                return _claimsVM;
            }
        }

        public static YourProvidersViewModel YourProvidersVM
        {
            get
            {
                if (_yourProvidersVM == null)
                    _yourProvidersVM = new YourProvidersViewModel();
                return _yourProvidersVM;
            }
        }
    }
}
