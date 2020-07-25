using HidLibrary;
using System;
using System.Collections.Generic;
using System.Drawing;
using System.Text;
using System.Threading.Tasks;

namespace NotNzxt
{
    public class SmartDeviceV2 : IDisposable
    {
        private const int TotalLightChannel = 2;
        private const int DeviceBufferSize = 65;

        private HidDevice _device;

        private Dictionary<int, byte> _channelIndex;

        public SmartDeviceV2(HidDevice device)
        {
            _device = device;

            Init();
        }

        private void Init()
        {
            _device.OpenDevice();

            _channelIndex = new Dictionary<int, byte>();
            for (int i = 0; i < TotalLightChannel; i++)
            {
                _channelIndex.Add(i + 1, (byte)Math.Pow(2.0, (double)i));
            }
        }

        public void ApplyFixedColor(Color color)
        {
            var colorList = new List<Color>
            {
                color
            };

            byte[] toWrite = new byte[DeviceBufferSize];
            toWrite[0] = 40;
            toWrite[1] = 3;
            toWrite[2] = 1;   // channel 1
            toWrite[3] = 40; // ??? Total Channel LEDs
            toWrite[4] = 0;

            toWrite[8] = (byte)colorList.Count;
            toWrite[9] = 0; //LED group size

            int idx = 10;
            for (int i = 0; i < colorList.Count; i++)
            {
                toWrite[idx]     = colorList[i].G;
                toWrite[idx + 1] = colorList[i].R;
                toWrite[idx + 2] = colorList[i].B;

                idx += 3;
            }

            if (!_device.Write(toWrite))
            {
                Console.WriteLine("oh shit!");
            }
        }

        public void Dispose()
        {
            _device.Dispose();
        }
    }
}
