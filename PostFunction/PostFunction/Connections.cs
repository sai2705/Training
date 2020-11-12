using System;
using System.Collections.Generic;
using System.Text;

namespace PostFunction
{
    public static class Connections
    {
      

        public static string cstring = "DefaultEndpointsProtocol=https;AccountName=blobpostmessage;AccountKey=wAfWNvJCZq8oKSHwQehzA4ZR/50LBx6wwKBF8NGRAdivPFvtH3cynfgD11wdRiEuRB/yNT0ayruqMmvQOSvwuw==;EndpointSuffix=core.windows.net";
        
        public static string blobcontainer = "messagecontainer";

        public static string contenttype = "application/json";
    }
}
