using System;
using System.Net;
using MonoTouch.CoreFoundation;
using MonoTouch.SystemConfiguration;

namespace AltinnApp.iOS.Util
{
    public enum NetworkStatus
    {
        NotReachable,
        ReachableViaCarrierDataNetwork,
        ReachableViaWiFiNetwork
    }

    /// <summary>
    /// Class that implements many usefull methods to check for network availability. Copy/paste from Xamarin.
    /// </summary>
    public class Reachability
    {
        public NetworkStatus Status { get; private set; }

        public bool ConnectionRequired { get; private set; }

        private Reachability()
        {
        }

        private static bool IsReachableWithoutRequiringConnection(NetworkReachabilityFlags flags)
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

        //
        /// <summary>
        ///  Is the host reachable with the current network configuration
        ///  remove http:// and all trailing slashes before use
        /// </summary>
        /// <param name="host"></param>
        /// <returns></returns>
        public static bool IsHostReachable(string host)
        {
            //host = host.Replace("http://", "");
            //host = host.Replace("https://", "");
            //host = host.Replace("https://", "");
            var uri = new Uri(host);
            host = uri.Host;

            if (!string.IsNullOrEmpty(host))
            {
                using (var r = new NetworkReachability(host))
                {
                    NetworkReachabilityFlags flags;
                    if (r.TryGetFlags(out flags))
                        return IsReachableWithoutRequiringConnection(flags);
                }
            }
            return false;
        }

        // 
        // Raised every time there is an interesting reachable event, 
        // we do not even pass the info as to what changed, and 
        // we lump all three status we probe into one
        //
        public static event EventHandler ReachabilityChanged;

        private static void OnChange(NetworkReachabilityFlags flags)
        {
            var h = ReachabilityChanged;
            if (h != null)
                h(null, EventArgs.Empty);
        }

        private static NetworkReachability defaultRouteReachability;

        private static bool IsNetworkAvailable(out NetworkReachabilityFlags flags)
        {
            if (defaultRouteReachability == null)
            {
                defaultRouteReachability = new NetworkReachability(new IPAddress(0));
                defaultRouteReachability.SetCallback(OnChange);
                defaultRouteReachability.Schedule(CFRunLoop.Current, CFRunLoop.ModeDefault);
            }
            return defaultRouteReachability.TryGetFlags(out flags)
                   && IsReachableWithoutRequiringConnection(flags);
        }

        //
        // Returns true if it is possible to reach the AdHoc WiFi network
        // and optionally provides extra network reachability flags as the
        // out parameter
        //
        private static NetworkReachability adHocWiFiNetworkReachability;

        private static bool IsAdHocWiFiNetworkAvailable(out NetworkReachabilityFlags flags)
        {
            if (adHocWiFiNetworkReachability == null)
            {
                adHocWiFiNetworkReachability = new NetworkReachability(new IPAddress(new byte[] {169, 254, 0, 0}));
                adHocWiFiNetworkReachability.SetCallback(OnChange);
                adHocWiFiNetworkReachability.Schedule(CFRunLoop.Current, CFRunLoop.ModeDefault);
            }
            return adHocWiFiNetworkReachability.TryGetFlags(out flags)
                   && IsReachableWithoutRequiringConnection(flags);
        }

        private static NetworkReachability remoteHostReachability;

        public static Reachability RemoteHostStatus(string hostName)
        {
            bool reachable;
            NetworkReachabilityFlags flags;
            if (remoteHostReachability == null)
            {
                remoteHostReachability = new NetworkReachability(hostName);

                // Need to probe before we queue, or we wont get any meaningful values
                // this only happens when you create NetworkReachability from a hostname
                reachable = remoteHostReachability.TryGetFlags(out flags);
                remoteHostReachability.SetCallback(OnChange);
                remoteHostReachability.Schedule(CFRunLoop.Current, CFRunLoop.ModeDefault);
            }
            else
                reachable = remoteHostReachability.TryGetFlags(out flags);

            bool requiresConnection = !IsReachableWithoutRequiringConnection(flags);

            if (!reachable)
                return new Reachability() {Status = NetworkStatus.NotReachable, ConnectionRequired = requiresConnection};

            if ((flags & NetworkReachabilityFlags.IsWWAN) != 0)
                return new Reachability()
                {
                    Status = NetworkStatus.ReachableViaCarrierDataNetwork,
                    ConnectionRequired = requiresConnection
                };

            if (requiresConnection)
                return new Reachability() {Status = NetworkStatus.NotReachable, ConnectionRequired = false};

            return new Reachability()
            {
                Status = NetworkStatus.ReachableViaWiFiNetwork,
                ConnectionRequired = requiresConnection
            };
        }

        public static Reachability InternetConnectionStatus()
        {
            NetworkReachabilityFlags flags;

            bool defaultNetworkAvailable = IsNetworkAvailable(out flags);
            bool requiresConnection = !IsReachableWithoutRequiringConnection(flags);

            Reachability result = new Reachability()
            {
                ConnectionRequired = requiresConnection,
                Status = NetworkStatus.NotReachable
            };

            // If the connection is reachable and no connection is required, then assume it's WiFi
            if (defaultNetworkAvailable)
            {
                result.Status = NetworkStatus.ReachableViaWiFiNetwork;
            }

            // If the connection is on-demand or on-traffic and no user intervention
            // is required, then assume WiFi.
            if (((flags & NetworkReachabilityFlags.ConnectionOnDemand) != 0
                 || (flags & NetworkReachabilityFlags.ConnectionOnTraffic) != 0)
                && (flags & NetworkReachabilityFlags.InterventionRequired) == 0)
            {
                result.Status = NetworkStatus.ReachableViaWiFiNetwork;
            }

            // If it's a WWAN connection..
            if ((flags & NetworkReachabilityFlags.IsWWAN) != 0)
                result.Status = NetworkStatus.ReachableViaCarrierDataNetwork;

            return result;
        }

        public static Reachability LocalWifiConnectionStatus()
        {
            NetworkReachabilityFlags flags;
            bool wifiAvailable = IsAdHocWiFiNetworkAvailable(out flags);
            bool requiresConnection = !IsReachableWithoutRequiringConnection(flags);

            if (wifiAvailable)
            {
                if ((flags & NetworkReachabilityFlags.IsDirect) != 0)
                    return new Reachability()
                    {
                        Status = NetworkStatus.ReachableViaWiFiNetwork,
                        ConnectionRequired = requiresConnection
                    };
            }

            return new Reachability() {Status = NetworkStatus.NotReachable, ConnectionRequired = requiresConnection};
        }
    }
}