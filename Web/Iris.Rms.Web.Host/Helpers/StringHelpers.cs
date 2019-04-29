using System;
using System.Text;

namespace Iris.Rms.Web.Host.Helpers
{
    public static class StringHelpers
    {
        public static string ToUrlEncodedMac(this string properMacAddress)
        {
            if (string.IsNullOrEmpty(properMacAddress))
            {
                throw new ArgumentNullException(properMacAddress);
            }
            if (properMacAddress.Contains(":"))
            {
                return properMacAddress.Replace(":", "").ToLower();
            }
            else
            {
                return properMacAddress;
            }
        }

        public static string FromUrlDecodedMac(this string deviceMac)
        {
            if (string.IsNullOrEmpty(deviceMac))
            {
                throw new ArgumentNullException(deviceMac);
            }
            if (deviceMac.Contains(":")) return deviceMac;

            var macAddress = new StringBuilder();
            for (int i = 0; i < deviceMac.Length; i++)
            {
                macAddress.Append(deviceMac[i].ToString().ToUpper());
                i++;
                macAddress.Append(deviceMac[i].ToString().ToUpper());
                macAddress.Append(":");
            }
            return macAddress.ToString();
        }
    }
}
