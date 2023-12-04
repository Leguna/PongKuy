using System.Net;
using System.Net.NetworkInformation;
using System.Net.Sockets;

namespace Utilities
{
    public abstract class IPManager
    {
        public static string GetLocalIPAddress()
        {
            var host = Dns.GetHostEntry(Dns.GetHostName());
            foreach (var ip in host.AddressList)
                if (ip.AddressFamily == AddressFamily.InterNetwork)
                    return ip.ToString();

            return "IP Local Not Found";
        }

        public static string GetIP(AddressFam addressFam)
        {
            if (addressFam == AddressFam.Pv6 && !Socket.OSSupportsIPv6)
            {
                return null;
            }

            string output = "";

            foreach (NetworkInterface item in NetworkInterface.GetAllNetworkInterfaces())
            {
#if UNITY_EDITOR_WIN || UNITY_STANDALONE_WIN
                const NetworkInterfaceType type1 = NetworkInterfaceType.Wireless80211;
                const NetworkInterfaceType type2 = NetworkInterfaceType.Ethernet;

                if ((item.NetworkInterfaceType == type1 || item.NetworkInterfaceType == type2) &&
                    item.OperationalStatus == OperationalStatus.Up)
#endif
                {
                    foreach (UnicastIPAddressInformation ip in item.GetIPProperties().UnicastAddresses)
                    {
                        //IPv4
                        if (addressFam == AddressFam.Pv4)
                        {
                            if (ip.Address.AddressFamily == AddressFamily.InterNetwork)
                            {
                                output = ip.Address.ToString();
                            }
                        }

                        //IPv6
                        else if (addressFam == AddressFam.Pv6)
                        {
                            if (ip.Address.AddressFamily == AddressFamily.InterNetworkV6)
                            {
                                output = ip.Address.ToString();
                            }
                        }
                    }
                }
            }

            return output;
        }
    }

    public enum AddressFam
    {
        Pv4,
        Pv6
    }
}