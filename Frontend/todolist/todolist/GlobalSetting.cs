using System;
namespace todolist
{
    public class GlobalSetting
    {
        //use singleton for setting all proj
        public static GlobalSetting Instance { get; } = new GlobalSetting();

        public const string MockTag = "Mock";
        public const string DefaultEndpointBase = "http://localhost:3000";

        private string _EndpointBase;


        public string EndpointBase {
            get { return _EndpointBase; }
            set {
                _EndpointBase = value;
                UpdateEndpoint(_EndpointBase);
            }
        }

        private void UpdateEndpoint(string endpointBase)
        {
            //Will update all endpoint after change from local to server test ...
            // login
            // register
            // todos ...

        }
    }
}
