using System;
using BeaconPoc.BeaconPocService;

namespace BeaconPoc
{
    internal class ServiceHandler
    {
        internal AppInfo GetAppInfo(BeaconInfo beaconInfo)
        {
            AppInfo appInfo;

            try
            {
                using (var client = new BeaconService())
                {
                    appInfo = client.GetAppInfo(beaconInfo);
                }
            }
            catch (Exception ex)
            {
                appInfo = new AppInfo { Id = "-1", Comment = ex.Message };
            }

            return appInfo;
        }
    }
}