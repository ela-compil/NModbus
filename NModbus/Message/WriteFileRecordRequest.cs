using NModbus.Data;
using System;
using System.IO;

namespace NModbus.Message
{
    internal class WriteFileRecordRequest : AbstractModbusMessageWithData<FileRecordDataCollection>, IModbusRequest
    {
        public WriteFileRecordRequest()
        {
        }

        public WriteFileRecordRequest(byte slaveAdress, FileRecordDataCollection data)
            : base(slaveAdress, ModbusFunctionCodes.WriteFileRecord)
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

        public void ValidateResponse(IModbusMessage response)
        {
            var typedResponse = (WriteFileRecordResponse)response;
            
            if (Data.FileNumber != typedResponse.Data.FileNumber)
            {
                string msg = $"Unexpected file number in response. Expected {Data.FileNumber}, received {typedResponse.Data.FileNumber}.";
                throw new IOException(msg);
            }

            if (Data.RecordNumber != typedResponse.Data.RecordNumber)
            {
                string msg = $"Unexpected starting address in response. Expected {Data.RecordNumber}, received {typedResponse.Data.RecordNumber}.";
                throw new IOException(msg);
            }
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
            string msg = $"Write {Data.DataBytes.Count} bytes for file {Data.FileNumber} starting at address {Data.RecordNumber}.";
            return msg;
        }
    }
}
