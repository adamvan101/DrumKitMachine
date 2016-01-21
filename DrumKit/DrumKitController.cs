using HidLibrary;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace DrumKit
{
    class DrumKitController
    {
        private const int VendorId = 0x1BAD;
        private readonly int[] ProductIds = new[] { MessageFactory.HarmonixId };
        private int _currentProductId;
        private HidDevice _device;
        private bool _attached;

        public bool pedalUp = true;
        public bool leftUp = true;
        public bool rightUp = true;
        public bool downUp = true;
        public bool upUp = true;
        public bool buttonAUp = true;
        public bool buttonBUp = true;
        public bool button1Up = true;
        public bool button2Up = true;
        public bool buttonPlusUp = true;
        public bool buttonMinusUp = true;

        public delegate void OnButtonPressed(IMessage m);
        public event OnButtonPressed OnButtonEvent;

        private bool _press = false;

        public DrumKitController(HidDevice device)
        {
            foreach (var productId in ProductIds)
            {
                if (device == null)
                {
                    _device = HidDevices.Enumerate(VendorId, productId).FirstOrDefault();
                }
                else
                {
                    IEnumerable<HidDevice> devices = HidDevices.Enumerate(VendorId, productId);//.Where(x => x.DevicePath != device.DevicePath).FirstOrDefault();
                    _device = devices.Where(d => d.DevicePath != device.DevicePath).FirstOrDefault();
                }

                if (_device == null) continue;

                _currentProductId = productId;

                _device.OpenDevice();

                _device.Inserted += DeviceAttachedHandler;
                _device.Removed += DeviceRemovedHandler;

                _device.MonitorDeviceEvents = true;

                _device.ReadReport(OnReport);
                break;
            }

            if (_device != null)
            {
                //Console.WriteLine("Gamepad found, press any key to exit.");
                _device.CloseDevice();
            }
            else
            {
                //Console.WriteLine("Could not find a gamepad.");
            }
        }

        private void OnReport(HidReport report)
        {
            if (_attached == false) { return; }

            if (report.Data.Length >= 4)
            {
                var message = MessageFactory.CreateMessage(_currentProductId, report.Data);
                OnButtonEvent.Invoke(message);
                //if (message.Depress) { KeyDepressed(); }
                //else
                //{
                //    OnButtonEvent.Invoke(message);

                //    //if (message.UpPressed) { KeyPressed("Up"); }
                //    //if (message.DownPressed) { KeyPressed("Down"); }
                //    //if (message.LeftPressed) { KeyPressed("Left"); }
                //    //if (message.RightPressed) { KeyPressed("Right"); }
                //    //if (message.Button1Pressed) { KeyPressed("1"); }
                //    //if (message.Button2Pressed) { KeyPressed("2"); }
                //    //if (message.ButtonAPressed) { KeyPressed("A"); }
                //    //if (message.ButtonBPressed) { KeyPressed("B"); }
                //    //if (message.ButtonPlusPressed) { KeyPressed("Plus"); }
                //    //if (message.ButtonMinusPressed) { KeyPressed("Minus"); }
                //    //if (message.ButtonRedPressed) { KeyPressed("Red"); }
                //    //if (message.ButtonYellowPressed) { KeyPressed("Yellow"); }
                //    //if (message.ButtonBluePressed) { KeyPressed("Blue"); }
                //    //if (message.ButtonGreenPressed) { KeyPressed("Green"); }
                //    //if (message.ButtonPedalPressed) { KeyPressed("Pedal"); }
                //    //_press = true;
                //}
            }

            _device.ReadReport(OnReport);
        }

        public HidDevice GetDevice()
        {
            return _device;
        }

        private void KeyPressed(string value)
        {
            if (!_press)
            {
                //Console.WriteLine("Button {0} pressed.", value);
            }
        }

        private void KeyDepressed()
        {
            //Console.WriteLine("Button depressed.");
            _press = false;
        }

        private void DeviceAttachedHandler()
        {
            _attached = true;
            //Console.WriteLine("Controller attached.");
            _device.ReadReport(OnReport);
        }

        private void DeviceRemovedHandler()
        {
            _attached = false;
            //Console.WriteLine("Controller removed.");
        }
    }
}
