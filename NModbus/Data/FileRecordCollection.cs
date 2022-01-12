using System;
using System.Collections.Generic;
using System.Linq;
using System.Net;

namespace NModbus.Data
{
    /// <inheritdoc/>
    internal class FileRecordCollection : IModbusMessageDataCollection
    {
        private byte[] networkBytes;

        public ushort FileNumber { get; }
        public ushort RecordNumber { get; }
        public virtual ushort RecordLength { get; }
        public byte[] NetworkBytes => networkBytes = networkBytes ?? GetNetworkBytes().ToArray();
        public byte ByteCount => (byte)NetworkBytes.Length;

        public FileRecordCollection(ushort fileNumber, ushort recordNumber, ushort recordLength)
        {
            FileNumber = fileNumber;
            RecordNumber = recordNumber;
            RecordLength = recordLength;
        }

        public FileRecordCollection(byte[] messageFrame)
        {
            FileNumber = (ushort)IPAddress.NetworkToHostOrder(BitConverter.ToInt16(messageFrame, 4));
            RecordNumber = (ushort)IPAddress.NetworkToHostOrder(BitConverter.ToInt16(messageFrame, 6));
            RecordLength = (ushort)IPAddress.NetworkToHostOrder(BitConverter.ToInt16(messageFrame, 8));
        }

        protected virtual List<byte> GetNetworkBytes()
        {
            var bytes = new List<byte>
            {
                6, // Reference type, demanded by standard definition
            };

            void addAsBytes(ushort value) =>
                bytes.AddRange(BitConverter.GetBytes((ushort)IPAddress.HostToNetworkOrder((short)value)));

            addAsBytes(FileNumber);
            addAsBytes(RecordNumber);
            addAsBytes(RecordLength);

            return bytes;
        }

        /// <summary>
        ///     Returns a <see cref="T:System.String" /> that represents the current <see cref="T:System.Object" />.
        /// </summary>
        /// <returns>
        ///     A <see cref="T:System.String" /> that represents the current <see cref="T:System.Object" />.
        /// </returns>
        public override string ToString()
        {
            return string.Concat("{", string.Join(", ", NetworkBytes.Select(v => v.ToString()).ToArray()), "}");
        }
    }
}
