﻿using System;
using ServiceTest.BeaconPocService;

namespace ServiceTest
{
    class Program
    {
        private const string Uuid = "60ABF5CD-9D72-4524-824E-27031F5E6F76";

        static void Main()
        {
        try
            {
                using (var client = new BeaconPocServiceClient())
                {
                    Console.WriteLine(client.GetVersion());
                    var appInfo = client.GetAppInfo(new BeaconInfo { PhoneIdentifier = "ServiceTest", Uuid = Uuid, Major = "30000", Minor = "30000" });
                    Console.WriteLine($"Service said: {appInfo.AppIdentifier}");
                }
            }
            catch (Exception ex)
            {
                Console.WriteLine(ex.Message);
            }
            Console.WriteLine("Press Enter to quit.");
            Console.ReadLine();
        }
    }
}
