using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace DrumKit
{
    class DrumKitMessage : IMessage
    {
        private const int PressUpMask = 0; // 0 0 0 
        private const int PressDownMask = 4; // 4 0 0
        private const int PressLeftMask = 6; // 6 0 0
        private const int PressRightMask = 2; // 2 0 0
        private const int PressAMask = 2; // 8 0 2
        private const int PressBMask = 4; // 8 0 4
        private const int Press1Mask = 1; // 8 0 1
        private const int Press2Mask = 8; // 8 0 8
        private const int PressPlusMask = 2; // 8 2 0
        private const int PressMinusMask = 1; // 8 1 0
        private const int PressRedMask = 64; // 8 0 64
        private const int PressYellowMask = 128; // 8 0 128
        private const int PressBlueMask = 16; // 8 0 16
        private const int PressGreenMask = 32; // 8 0 32
        private const int PressPedalMask = 4; // 8 4 0

        private readonly uint _message;
        private readonly byte _buttonsPressed;
        private readonly byte[] _messageBytes;

        public DrumKitMessage(uint message)
        {
            _message = message;
        }

        public DrumKitMessage(byte[] message)
        {
            if (message != null && message.Length == 27)
            {
                if ((message[0] & 16) != 0)
                {
                    message[1] += 4;
                }

                if (message[0] >= 16)
                {
                    var thirdByteAsString = Convert.ToString(message[0], 2);
                    byte thirdByte = Convert.ToByte(thirdByteAsString.Substring(thirdByteAsString.Length - 4) + "0000", 2);
                    message[0] = thirdByte;
                }

                _messageBytes = new byte[] { message[2], message[1], message[0] };
            }
            else
            {
                throw new InvalidOperationException("Poor gamepad input");
            }
            _buttonsPressed = GetButtonsPressed(this);
        }

        public override string ToString()
        {
            return String.Format("{0} {1} {2}", _messageBytes[0], _messageBytes[1], _messageBytes[2]);
        }

        public byte TotalPressed { get { return _buttonsPressed; } }
        public bool MultiplePressed { get { return (_buttonsPressed > 1); } }
        public bool Depress { get { return (_buttonsPressed == 0); } }
        public bool UpPressed { get { return (PressUpMask == _messageBytes[0]); } }
        public bool DownPressed { get { return (PressDownMask == _messageBytes[0]); } }
        public bool LeftPressed { get { return (PressLeftMask == _messageBytes[0]); } }
        public bool RightPressed { get { return (PressRightMask == _messageBytes[0]); } }
        public bool ButtonAPressed { get { return (PressAMask & _messageBytes[2]) != 0; } }
        public bool ButtonBPressed { get { return (PressBMask & _messageBytes[2]) != 0; } }
        public bool Button1Pressed { get { return (Press1Mask & _messageBytes[2]) != 0; } }
        public bool Button2Pressed { get { return (Press2Mask & _messageBytes[2]) != 0; } }
        public bool ButtonPlusPressed { get { return (PressPlusMask & _messageBytes[1]) != 0; } }
        public bool ButtonMinusPressed { get { return (PressMinusMask & _messageBytes[1]) != 0; } }
        public bool ButtonRedPressed { get { return (PressRedMask & _messageBytes[2]) != 0; } }
        public bool ButtonYellowPressed { get { return (PressYellowMask & _messageBytes[2]) != 0; } }
        public bool ButtonBluePressed { get { return (PressBlueMask & _messageBytes[2]) != 0; } }
        public bool ButtonGreenPressed { get { return (PressGreenMask & _messageBytes[2]) != 0; } }
        public bool ButtonPedalPressed { get { return (PressPedalMask & _messageBytes[1]) != 0; } }

        private static byte GetButtonsPressed(IMessage message)
        {
            byte buttonsPressed = 0;

            if (message.UpPressed) { buttonsPressed++; }
            if (message.DownPressed) { buttonsPressed++; }
            if (message.LeftPressed) { buttonsPressed++; }
            if (message.RightPressed) { buttonsPressed++; }
            if (message.ButtonAPressed) { buttonsPressed++; }
            if (message.ButtonBPressed) { buttonsPressed++; }
            if (message.Button1Pressed) { buttonsPressed++; }
            if (message.Button2Pressed) { buttonsPressed++; }
            if (message.ButtonPlusPressed) { buttonsPressed++; }
            if (message.ButtonMinusPressed) { buttonsPressed++; }
            if (message.ButtonRedPressed) { buttonsPressed++; }
            if (message.ButtonYellowPressed) { buttonsPressed++; }
            if (message.ButtonBluePressed) { buttonsPressed++; }
            if (message.ButtonGreenPressed) { buttonsPressed++; }
            if (message.ButtonPedalPressed) { buttonsPressed++; }

            return buttonsPressed;
        }
    }
}
