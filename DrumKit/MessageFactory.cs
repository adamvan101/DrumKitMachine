using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace DrumKit
{
    internal class MessageFactory
    {
        internal const int HarmonixId = 0x0005;

        public static IMessage CreateMessage(int productId, byte[] messageData)
        {
            switch (productId)
            {
                case HarmonixId: return new DrumKitMessage(messageData);
            }

            if (productId != HarmonixId)
                Console.WriteLine(productId.ToString());

            return null;
        }
    }
}