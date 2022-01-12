using NModbus.Data;
using System;

namespace NModbus.Message
{
    class WriteFileRecordResponse : AbstractModbusMessageWithData<FileRecordDataCollection>, IModbusMessage
    {
        public WriteFileRecordResponse()
        {
        }

        public WriteFileRecordResponse(byte slaveAddress)
            : base(slaveAddress, ModbusFunctionCodes.WriteFileRecord)
        {
        }

        public WriteFileRecordResponse(byte slaveAddress, FileRecordDataCollection data)
            : base(slaveAddress, ModbusFunctionCodes.WriteFileRecord)
        {
            Data = data;
            ByteCount = data.ByteCount;
        }

        public override int MinimumFrameSize => 10;

        public byte ByteCount
        {
            get => MessageImpl.ByteCount.Value;
            set => MessageImpl.ByteCount = value;
        }

        protected override void InitializeUnique(byte[] frame)
        {
            if (frame.Length < frame[2])
            {
                throw new FormatException("Message frame does not contain enough bytes.");
            }

            ByteCount = frame[2];
            Data = new FileRecordDataCollection(frame);
        }

        public override string ToString()
        {
            string msg = $"Wrote {Data.DataBytes.Count} bytes for file {Data.FileNumber} starting at address {Data.RecordNumber}.";
            return msg;
        }
    }
}
