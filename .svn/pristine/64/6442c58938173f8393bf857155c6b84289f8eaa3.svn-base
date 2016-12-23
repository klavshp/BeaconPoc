using System.Reflection;

namespace BeaconPocService
{
    public class BeaconService : IBeaconPocService
    {
        public AppInfo GetAppInfo(BeaconInfo beaconInfo)
        {
            Logger.LogInfo($"PhoneIdentifier: {beaconInfo.PhoneIdentifier}, Uuid: {beaconInfo.Uuid}, Major: {beaconInfo.Major}, minor: {beaconInfo.Minor}");

            var beaconConfig = new BeaconConfig("beacons.xml");

            var beaconHandler = new BeaconHandler(beaconConfig);

            return beaconHandler.GetAppInfo(beaconInfo);
        }

        public string GetVersion()
        {
            return Assembly.GetExecutingAssembly().GetName().Version.ToString();
        }
    }
}
