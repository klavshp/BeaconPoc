using System;
using System.Collections.Generic;
using System.IO;
using System.Reflection;
using System.Xml.Linq;

namespace BeaconPocService
{
    public interface IBeaconConfig
    {
        Beacons Beacons();
    }

    public class Beacons
    {
        public string Uuid { get; set; }

        public List<Beacon> BeaconList { get; }

        public Beacons()
        {
            BeaconList = new List<Beacon>();
        }
    }

    public class Beacon
    {
        public string Major { get; set; }

        public string Minor { get; set; }

        public string AppId { get; set; }

        public string AppName { get; set; }
    }

    public class BeaconConfig : IBeaconConfig
    {
        private readonly Beacons _beacons;

        public BeaconConfig(string configFile)
        {
            try
            {
                _beacons = new Beacons();

                var xdoc = XDocument.Load($"{Path.GetDirectoryName(Assembly.GetExecutingAssembly().GetName().CodeBase)}\\{configFile}");

                foreach (var beacons in xdoc.Elements("beacons"))
                {
                    _beacons.Uuid = beacons.Attribute("uuid").Value;

                    foreach (var beacon in beacons.Elements("beacon"))
                    {
                        _beacons.BeaconList.Add(new Beacon
                        {
                            Major = beacon.Attribute("major").Value,
                            Minor = beacon.Attribute("minor").Value,
                            AppId = beacon.Attribute("appId").Value,
                            AppName = beacon.Attribute("appName").Value,
                        });
                    }
                }
            }
            catch (Exception ex)   
            {
                Logger.LogError(ex.Message, true);
            }
        }

        public Beacons Beacons()
        {
            return _beacons;
        }
    }
}