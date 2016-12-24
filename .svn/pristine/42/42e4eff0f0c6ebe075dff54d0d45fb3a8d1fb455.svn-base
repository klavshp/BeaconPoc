using System;
using RadiusNetworks.IBeaconAndroid;

namespace BeaconPoc
{
    internal class BeaconScanner
    {
        private readonly IBeaconManager _beaconManager;
        private readonly MonitorNotifier _monitorNotifier;
        private readonly RangeNotifier _rangeNotifier;
        private readonly Region _monitoringRegion;
        private readonly Region _rangingRegion;
        private readonly MainActivity _context;

        public event EventHandler<MonitorEventArgs> EnterRegionComplete;
        public event EventHandler<MonitorEventArgs> ExitRegionComplete;
        public event EventHandler<RangeEventArgs> DidRangeBeaconsInRegionComplete;

        internal BeaconScanner(MainActivity context, string beaconId, string uuid)
        {
            _context = context;
            _beaconManager = IBeaconManager.GetInstanceForApplication(context);
            _monitorNotifier = new MonitorNotifier();
            _monitoringRegion = new Region(beaconId, uuid, null, null);
            _rangeNotifier = new RangeNotifier();
            _rangingRegion = new Region(beaconId, uuid, null, null);

            _beaconManager.Bind(context);
            _monitorNotifier.EnterRegionComplete += EnterRegionComplete;
            _monitorNotifier.ExitRegionComplete += ExitRegionComplete;
            _rangeNotifier.DidRangeBeaconsInRegionComplete += DidRangeBeaconsInRegionComplete;
        }

        public void OnIBeaconServiceConnect()
        {
            _beaconManager.SetMonitorNotifier(_monitorNotifier);
            _beaconManager.SetRangeNotifier(_rangeNotifier);

            _beaconManager.StartMonitoringBeaconsInRegion(_monitoringRegion);
            _beaconManager.StartRangingBeaconsInRegion(_rangingRegion);
        }

    }
}