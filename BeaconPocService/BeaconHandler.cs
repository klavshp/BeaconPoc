﻿namespace BeaconPocService
{
    public interface IBeaconHandler
    {
        AppInfo GetAppInfo(BeaconInfo beaconInfo);
    }

    public class BeaconHandler : IBeaconHandler
    {
        private readonly IBeaconConfig _beaconConfig;

        public BeaconHandler(IBeaconConfig beaconConfig)
        {
            _beaconConfig = beaconConfig;
        }

        public AppInfo GetAppInfo(BeaconInfo beaconInfo)
        {
            var appInfo = new AppInfo();

            if (beaconInfo.Uuid == _beaconConfig.Beacons().Uuid)
            {
                foreach (var beacon in _beaconConfig.Beacons().BeaconList)
                {
                    if ((beacon.Major == beaconInfo.Major) && (beacon.Minor == beaconInfo.Minor))
                    {
                        appInfo.Id = beacon.Id;
                        appInfo.AppIdentifier = beacon.AppIdentifier;
                        appInfo.ApkFilename = beacon.ApkFilename;
                        appInfo.Comment = "";
                        break;
                    }
                    appInfo.ApkFilename = $"Unknown Beacon properties for beacon {beaconInfo.Uuid}";
                }
            }
            else
            {
                appInfo.ApkFilename = $"Unknown Beacon identifier: {beaconInfo.Uuid}";
            }

            return appInfo;
        }
    }
}