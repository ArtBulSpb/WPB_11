using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace WPB_11
{
    public class DataProcessor
    {
        public List<DataPacket> DataPackets { get; private set; }

        public DataProcessor()
        {
            DataPackets = new List<DataPacket>();
        }

        public int GetActiveDataPacket()
        {
            for (int i = 0; i < DataPackets.Count; i++)
            {
                if (DataPackets[i].Active)
                {
                    return i;
                }
            }
            return -1; // Если ни один пакет не активен
        }

        public void ActiveDataPacket(int index)
        {
            foreach (var packet in DataPackets)
            {
                packet.Active = false;
            }

            if (index >= 0 && index < DataPackets.Count)
            {
                DataPackets[index].Active = true;
                DataPackets[index].Reset();
            }
        }
    }

}
