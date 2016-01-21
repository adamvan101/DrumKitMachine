using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace DrumKit
{
    interface IMessage
    {
        byte TotalPressed { get; }
        bool MultiplePressed { get; }
        bool Depress { get; }
        bool UpPressed { get; }
        bool DownPressed { get; }
        bool LeftPressed { get; }
        bool RightPressed { get; }
        bool ButtonAPressed { get; }
        bool ButtonBPressed { get; }
        bool Button1Pressed { get; }
        bool Button2Pressed { get; }
        bool ButtonPlusPressed { get; }
        bool ButtonMinusPressed { get; }
        bool ButtonRedPressed { get; }
        bool ButtonYellowPressed { get; }
        bool ButtonBluePressed { get; }
        bool ButtonGreenPressed { get; }
        bool ButtonPedalPressed { get; }
    }
}
