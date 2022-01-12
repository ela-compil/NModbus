using NModbus.Data;
using System;

namespace NModbus.Message
{
    internal class ReadFileRecordResponse : AbstractModbusMessageWithData<FileRecordDataCollection>, IModbusMessage
    {
        public override int MinimumFrameSize => 8;

        public byte ByteCount
        {
            get => MessageImpl.ByteCount.Value;
            set => MessageImpl.ByteCount = value;
        }

        public ReadFileRecordResponse()
        {
        }

        public ReadFileRecordResponse(byte slaveAddress)
            : base(slaveAddress, ModbusFunctionCodes.ReadFileRecord)
        {
        }

        public ReadFileRecordResponse(byte slaveAddress, FileRecordDataCollection data)
            : base(slaveAddress, ModbusFunctionCodes.ReadFileRecord)
        {
            Data = data;
            ByteCount = data.ByteCount;
        }

        protected override void InitializeUnique(byte[] frame)
        {
            if (frame.Length < frame[2])
                throw new FormatException("Message frame does not contain enough bytes.");

            ByteCount = frame[2];
            Data = new FileRecordDataCollection(frame);
        }

        public override string ToString()
        {
            return $"Read {Data.DataBytes.Count} bytes for file {Data.FileNumber} starting at address {Data.RecordNumber}.";
        }
    }
}
