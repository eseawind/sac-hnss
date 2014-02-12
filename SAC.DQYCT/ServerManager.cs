using System;
using System.Collections.Generic;
using System.Linq;
using SAC.DQYCT.SLRoom;
using System.Windows;
using System.Net;
using SAC.DQYCT;

namespace SLGetDataFromWCFRIA.Web
{
    public class ServerManager
    {
        private static SLWCFRIAClient servicePicture = new SLWCFRIAClient();

        internal static SLWCFRIAClient GetPox()
        {
            if (servicePicture.State == System.ServiceModel.CommunicationState.Created)
            {


                //BaseUserContactClient client = new BaseUserContactClient();
                string hostUri = App.Current.Host.Source.AbsoluteUri;
                hostUri = hostUri.Substring(0, hostUri.IndexOf("/ClientBin"));
                string svcUri = hostUri + "/WebService/SLWCFRIA.svc";
                servicePicture.Endpoint.Address = new System.ServiceModel.EndpointAddress(svcUri);
                //servicePicture.Endpoint.Address = new System.ServiceModel.EndpointAddress("http://localhost:1527/WebService/SLWCFRIA.svc");
                return servicePicture;
            }
            else
            {
                return servicePicture;
            }
        }
    }
}