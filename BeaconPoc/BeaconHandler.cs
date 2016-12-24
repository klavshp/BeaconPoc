using System.Collections.Generic;
using System.Linq;
using BeaconPoc.BeaconPocService;
using RadiusNetworks.IBeaconAndroid;

namespace BeaconPoc
{
    internal class BeaconHandler
    {
        private readonly IBeaconManager _beaconManager;
        private readonly MonitorNotifier _monitorNotifier;
        private readonly RangeNotifier _rangeNotifier;
        private readonly Region _monitoringRegion;
        private readonly Region _rangingRegion;

        public delegate void RangingBeaconsInRegionHandler(BeaconInfo beaconInfo);
        public delegate void AlertHandler(string alert);
        public event RangingBeaconsInRegionHandler OnRangingBeaconsInRegion;
        public event AlertHandler Alert;
        private readonly string _uuid;
        private readonly List<BeaconInfo> _savedBeaconInfos;

        public bool Listening { get; set; }

        public BeaconHandler(IBeaconManager beaconManager, string uuid, string beaconId)
        {
            _beaconManager = beaconManager;
            _uuid = uuid;
            _savedBeaconInfos = new List<BeaconInfo>();

            _monitorNotifier = new MonitorNotifier();
            _rangeNotifier = new RangeNotifier();
            _monitoringRegion = new Region(beaconId, uuid, null, null);
            _rangingRegion = new Region(beaconId, uuid, null, null);

            _monitorNotifier.EnterRegionComplete += EnteredRegion;
            _monitorNotifier.ExitRegionComplete += ExitedRegion;
            _rangeNotifier.DidRangeBeaconsInRegionComplete += RangingBeaconsInRegion;
        }

        public void BeaconManagerConnect()
        {
            _beaconManager.SetMonitorNotifier(_monitorNotifier);
            _beaconManager.SetRangeNotifier(_rangeNotifier);

            _beaconManager.StartMonitoringBeaconsInRegion(_monitoringRegion);
            _beaconManager.StartRangingBeaconsInRegion(_rangingRegion);
        }

        public void OnPause(MainActivity context)
        {
            // if (_beaconManager.IsBound(this))
            //_beaconManager.SetBackgroundMode(context, true);
        }

        public void OnResume(MainActivity context)
        {
            // if (_beaconManager.IsBound(this)) 
            //_beaconManager.SetBackgroundMode(context, false);
        }

        public void OnDestroy(MainActivity context)
        {
            if (_beaconManager.IsBound(context))
            {
                _beaconManager.UnBind(context);
            }
        }

        public bool IsKnownBeacon(BeaconInfo beaconInfo)
        {
            if (_savedBeaconInfos.Contains(beaconInfo, new BeaconEqualityComparer()) || beaconInfo.Major != "20000")
            {
                return true;
            }

            _savedBeaconInfos.Clear();
            _savedBeaconInfos.Add(beaconInfo);
            return false;
        }

        public void Listen(bool listen)
        {
            _savedBeaconInfos.Clear();
            Listening = listen;
        }

        public void EnteredRegion(object sender, MonitorEventArgs e)
        {
            if (Listening)
            {
                Alert?.Invoke("You entered the Beacon region");
            }
        }

        public void ExitedRegion(object sender, MonitorEventArgs e)
        {
            if (Listening)
            {
                Alert?.Invoke("You exited the Beacon region");
            }
        }

        public void RangingBeaconsInRegion(object sender, RangeEventArgs e)
        {
            if (!Listening) return;

            for (var i = 1; i <= e.Beacons.Count; i++)
            {
                var beacon = e.Beacons.ElementAt(i - 1);
                if (beacon == null) return;

                switch ((ProximityType)beacon.Proximity)
                {
                    case ProximityType.Immediate:
                        OnRangingBeaconsInRegion?.Invoke(new BeaconInfo { Uuid = _uuid, Major = beacon.Major.ToString(), Minor = beacon.Minor.ToString() });
                        Alert?.Invoke($"Beacon {beacon.Major}/{beacon.Minor}");
                        break;
                    //case ProximityType.Near:
                    //    Alert?.Invoke($"Beacon {beacon.Major}/{beacon.Minor}");
                    //    break;
                    //case ProximityType.Far:
                    //    Alert?.Invoke($"Beacon {beacon.Major}/{beacon.Minor}: 'Far'");
                    //    break;
                    //case ProximityType.Unknown:
                    //    Alert?.Invoke($"Beacon {beacon.Major}/{beacon.Minor}: 'Unknown'");
                    //    break;
                }
            }
        }
    }
}