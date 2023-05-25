using Android.OS;
using UPMC_App.Interfaces;

[assembly: Xamarin.Forms.Dependency(typeof(UPMC_App.Droid.AndroidMethods))]
namespace UPMC_App.Droid
{
    public class AndroidMethods : IAndroidMethods
    {
        public void CloseApp()
        {
            //This closes the Android app
            Process.KillProcess(Process.MyPid());
        }
    }
}