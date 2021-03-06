﻿using System.Runtime.Serialization;
using System.ServiceModel;

namespace BeaconPocService
{
    [ServiceContract]
    public interface IBeaconPocService
    {
        [OperationContract]
        AppInfo GetAppInfo(BeaconInfo beaconInfo);

        [OperationContract]
        string GetVersion();
    }

    [DataContract]
    public class AppInfo
    {
        [DataMember]
        public string Id { get; set; }

        [DataMember]
        public string ApkFilename { get; set; }

        [DataMember]
        public string AppIdentifier { get; set; }

        [DataMember]
        public string Comment { get; set; }
    }

    [DataContract]
    public class BeaconInfo
    {
        [DataMember]
        public string Uuid { get; set; }

        [DataMember]
        public string Major { get; set; }

        [DataMember]
        public string Minor { get; set; }

        [DataMember]
        public string PhoneIdentifier { get; set; }
    }

}
