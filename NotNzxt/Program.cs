using System;
using System.Drawing;
using System.Linq;
using System.Threading.Tasks;
using HidLibrary;

namespace NotNzxt
{
    class Program
    {
        static void Main(string[] args)
        {
            const int NZXTVendorId = 7793;
            const int SmartDeviceV2ProductId = 8198;

            // NZXT 
            var detectedDevice = HidDevices.Enumerate(NZXTVendorId)
                .ToList()
                .FirstOrDefault(device => 
                    device.Attributes.ProductId == SmartDeviceV2ProductId && 
                    device.Attributes.VendorId  == NZXTVendorId);

            if (detectedDevice == null)
                return;

            var smartDevice = new SmartDeviceV2(detectedDevice);
            smartDevice.ApplyFixedColor(Utils.ColorFromHex(args[0]));
        }
    }
}
