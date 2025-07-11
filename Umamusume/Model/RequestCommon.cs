

using Newtonsoft.Json;
using System;
using System.Runtime.InteropServices;

namespace Umamusume.Model
{
    public static class SystemInfoEmulator
    {
        public static string GetOperatingSystem()
        {
            var version = Environment.OSVersion.Version;
            string platform = RuntimeInformation.IsOSPlatform(OSPlatform.Windows) ? "Windows"
                            : RuntimeInformation.IsOSPlatform(OSPlatform.Linux) ? "Linux"
                            : RuntimeInformation.IsOSPlatform(OSPlatform.OSX) ? "Mac OS X"
                            : "Unknown OS";

            string arch = Environment.Is64BitOperatingSystem ? "64bit" : "32bit";

            string name = GetWindowsFriendlyName(version);

            return $"{platform} {name} ({version.Major}.{version.Minor}.{version.Build}) {arch}";
        }

        private static string GetWindowsFriendlyName(Version version)
        {
            if (version.Major == 10 && version.Build >= 22000)
                return "11";
            if (version.Major == 10)
                return "10";
            if (version.Major == 6 && version.Minor == 3)
                return "8.1";
            if (version.Major == 6 && version.Minor == 2)
                return "8";
            if (version.Major == 6 && version.Minor == 1)
                return "7";
            return $"{version.Major}.{version.Minor}";
        }
    }

    [JsonObject]
    public class RequestEnvironment
    {
        [JsonProperty]
        private int device;



        [JsonProperty]
        private string device_id;


        [JsonProperty]

        private string device_name;


        [JsonProperty]

        private string graphics_device_name;


        [JsonProperty]

        private string ip_address;


        [JsonProperty]

        private string platform_os_version;



        [JsonProperty]
        private string carrier;



        [JsonProperty]
        private int keychain;


        [JsonProperty]
        private string locale;

        [JsonProperty]
        private string dmm_viewer_id;

        [JsonProperty]
        private string dmm_onetime_token;

        [JsonProperty]
        private string steam_id;

        [JsonProperty]
        private string steam_session_ticket;

        protected void UpdateInfo(RequestEnvironment env)
        {
            locale = env.locale;
            keychain = env.keychain;
            carrier = env.carrier;
            platform_os_version = env.platform_os_version;
            ip_address = env.ip_address;
            graphics_device_name = env.graphics_device_name;
            device_name = env.device_name;
            device_id = env.device_id;
            device = env.device;
            dmm_viewer_id = env.dmm_viewer_id;
            dmm_onetime_token = env.dmm_onetime_token;
            steam_id = env.steam_id;
            steam_session_ticket = env.steam_session_ticket;
        }

        public static RequestEnvironment CreateDefault()
        {
            return new()
            {
                platform_os_version = SystemInfoEmulator.GetOperatingSystem(),
                carrier = "", // >PC has carrier lol
                keychain = 0,
                locale = "JPN",
                ip_address = "10.0.2.15",
                device = 4,
                device_id = Guid.NewGuid().ToString().Replace("-", ""),
                device_name = "Aoba",
                graphics_device_name = "AMD Radeon RX Vega Graphics",
            };
        }
    }

    public class RequestCommon : RequestEnvironment
    {

        [JsonProperty]
        private long viewer_id;


        public RequestCommon()
        {
        }

        public void UpdateInfo(RequestEnvironment env, Account account)
        {
            viewer_id = account.ViewerId;
            UpdateInfo(env);
        }
    }
}
