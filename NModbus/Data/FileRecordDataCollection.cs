using NModbus.Unme.Common;
using System;
using System.Collections.Generic;
using System.Linq;

namespace NModbus.Data
{
    internal class FileRecordDataCollection : FileRecordCollection
    {
        public IReadOnlyList<byte> DataBytes { get; }

        public FileRecordDataCollection(ushort fileNumber, ushort recordNumber, byte[] data)
            : base(fileNumber, recordNumber, (ushort)(data.Length / 2))
        {
            if (data.Length % 2 != 0)
                throw new ArgumentException("Number of bytes has to be even", nameof(data));

            DataBytes = data;
        }

        public FileRecordDataCollection(byte[] messageFrame)
            : base(messageFrame)
        {
            DataBytes = messageFrame.Slice(10, RecordLength * 2).ToArray();
        }

        protected override List<byte> GetNetworkBytes()
        {
            var bytes = base.GetNetworkBytes();

            bytes.AddRange(DataBytes);

            return bytes;
        }
    }
}
