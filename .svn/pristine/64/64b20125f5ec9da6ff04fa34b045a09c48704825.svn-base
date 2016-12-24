using System.Collections.Generic;
using BeaconPoc.BeaconPocService;

namespace BeaconPoc
{
    internal class BeaconEqualityComparer : IEqualityComparer<BeaconInfo>
    {
        public bool Equals(BeaconInfo b1, BeaconInfo b2)
        {
            return b1.Uuid == b2.Uuid && b1.Major == b2.Major && b1.Minor == b2.Minor;
        }

        public int GetHashCode(BeaconInfo bx)
        {
            return 0;
        }
    }
}