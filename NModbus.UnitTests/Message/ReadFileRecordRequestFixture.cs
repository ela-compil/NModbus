using System.IO;
using NModbus.Data;
using NModbus.Message;
using Xunit;

namespace NModbus.UnitTests.Message
{
    public class ReadFileRecordRequestFixture
    {
        [Fact]
        public void Create()
        {
            var request = new ReadFileRecordRequest(17, 1, 2, 1);
            Assert.Equal(ModbusFunctionCodes.ReadFileRecord, request.FunctionCode);
            Assert.Equal(17, request.SlaveAddress);
            Assert.Equal(1, request.Data.FileNumber);
            Assert.Equal(2, request.Data.RecordNumber);
            Assert.Equal(1, request.Data.RecordLength);
        }

        [Fact]
        public void Validate_ThrowsOnFileNumberMismatch()
        {
            var request = new ReadFileRecordRequest(17, 1, 2, 1);
            var response = new ReadFileRecordResponse(17, new FileRecordDataCollection(2, 2, new byte[] { 4, 5 }));
            Assert.Throws<IOException>(() => request.ValidateResponse(response));
        }

        [Fact]
        public void Validate_ThrowsOnRecordNumberMismatch()
        {
            var request = new ReadFileRecordRequest(17, 1, 2, 1);
            var response = new ReadFileRecordResponse(17, new FileRecordDataCollection(1, 4, new byte[] { 4, 5 }));
            Assert.Throws<IOException>(() => request.ValidateResponse(response));
        }

        [Fact]
        public void Initialize()
        {
            var request = new ReadFileRecordRequest();
            request.Initialize(new byte[] { 17, ModbusFunctionCodes.ReadFileRecord, 9, 6, 0, 1, 0, 2, 0, 1 });

            Assert.Equal(ModbusFunctionCodes.ReadFileRecord, request.FunctionCode);
            Assert.Equal(17, request.SlaveAddress);
            Assert.Equal(1, request.Data.FileNumber);
            Assert.Equal(1, request.Data.RecordLength); // 1 register == 2 bytes
        }

        [Fact]
        public void ToString_Test()
        {
            var request = new ReadFileRecordRequest(17, 1, 2, 1);

            Assert.Equal("Read 2 bytes for file 1 starting at address 2.", request.ToString());
        }
    }
}