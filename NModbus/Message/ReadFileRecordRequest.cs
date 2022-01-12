using NModbus.Data;
using System;
using System.IO;

namespace NModbus.Message
{
    internal class ReadFileRecordRequest : AbstractModbusMessageWithData<FileRecordCollection>, IModbusRequest
    {
        private const int ByteCountIndex = 2;
        private const int ReferenceTypeIndex = 3;
        private const int FileNumberIndex = 4;
        private const int RecordNumberIndex = 5;
        private const int RecordLengthIndex = 6;

        public ReadFileRecordRequest()
        {
        }

        public ReadFileRecordRequest(byte slaveAddress, ushort fileNumber, ushort recordNumber, ushort recordLength)
            : base(slaveAddress, ModbusFunctionCodes.ReadFileRecord)
        {
            Data = new FileRecordCollection(fileNumber, recordNumber, recordLength);
            ByteCount = Data.ByteCount;
        }
        public override int MinimumFrameSize => 10;

        public byte ByteCount
        {
            get => MessageImpl.ByteCount.Value;
            set => MessageImpl.ByteCount = value;
        }

        public void ValidateResponse(IModbusMessage response)
        {
            if (!(response is ReadFileRecordResponse readFileRecordResponse))
                throw new InvalidOperationException(
                    $"Response must be of type {nameof(ReadFileRecordResponse)}");

            if (Data.FileNumber != readFileRecordResponse.Data.FileNumber)
                throw new IOException($"Unexpected file number in response. Expected {Data.FileNumber}," +
                    $" received {readFileRecordResponse.Data.FileNumber}.");

            if (Data.RecordNumber != readFileRecordResponse.Data.RecordNumber)
                throw new IOException($"Unexpected starting address in response. Expected {Data.RecordNumber}," +
                    $" received {readFileRecordResponse.Data.RecordNumber}.");
        }

        protected override void InitializeUnique(byte[] frame)
        {
            if (frame.Length < frame[ByteCountIndex])
                throw new FormatException("Message frame does not contain enough bytes.");

            ByteCount = frame[ByteCountIndex];
            Data = new FileRecordCollection(frame);
        }

        public override string ToString()
        {
            return $"Read {Data.RecordLength * 2} bytes for file {Data.FileNumber}" +
                $" starting at address {Data.RecordNumber}.";
        }
    }
}
