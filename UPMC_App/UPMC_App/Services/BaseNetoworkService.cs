using System;
using System.Collections.Generic;
using System.Net;
using System.Net.Http;
using System.Text;

namespace UPMC_App.Services
{
    abstract public class BaseNetoworkService
    {
        internal HttpClient _clnt;
        public BaseNetoworkService()
        {
            _clnt = new HttpClient();
            ServicePointManager.ServerCertificateValidationCallback += (sender, cert, chain, sslPolicyErrors) => true;
        }
        public BaseNetoworkService(string session)
        {
            _clnt = new HttpClient();
            SetSesionKey(session);
            ServicePointManager.ServerCertificateValidationCallback += (sender, cert, chain, sslPolicyErrors) => true;
        }

        public void SetSesionKey(string session)
        {
            _clnt.DefaultRequestHeaders.Authorization = new System.Net.Http.Headers.AuthenticationHeaderValue("Basic", $"PrivateKey:{ViewModelContainer.PrivateKey}SessionKey:{session}");
        }
    }
}
