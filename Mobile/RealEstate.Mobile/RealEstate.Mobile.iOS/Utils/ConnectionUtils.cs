using System;
using System.Collections.Generic;
using System.Linq;
using System.Net;
using System.Text;
using SystemConfiguration;
using CoreFoundation;
using Foundation;
using UIKit;
using RealEstate.Mobile.Utils;

namespace RealEstate.Mobile.iOS.Utils
{
    public class ConnectionUtils : IConnectionUtils
    {
        NetworkReachability _defaultRouteReachability;

        public bool IsNetworkAvailable()
        {
            if (_defaultRouteReachability == null)
            {
                _defaultRouteReachability = new NetworkReachability(new IPAddress(0));
                _defaultRouteReachability.Schedule(CFRunLoop.Current, CFRunLoop.ModeDefault);
            }

            NetworkReachabilityFlags flags;

            return _defaultRouteReachability.TryGetFlags(out flags) &&
                   IsReachableWithoutRequiringConnection(flags);
        }

        bool IsReachableWithoutRequiringConnection(NetworkReachabilityFlags flags)
        {
            // Is it reachable with the current network configuration?
            bool isReachable = (flags & NetworkReachabilityFlags.Reachable) != 0;

            // Do we need a connection to reach it?
            bool noConnectionRequired = (flags & NetworkReachabilityFlags.ConnectionRequired) == 0;

            // Since the network stack will automatically try to get the WAN up,
            // probe that
            if ((flags & NetworkReachabilityFlags.IsWWAN) != 0)
                noConnectionRequired = true;

            return isReachable && noConnectionRequired;
        }
    }
}
