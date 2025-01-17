﻿using NModbus.Data;
using NModbus.Message;
using Xunit;

namespace NModbus.UnitTests.Message
{
    public class ReadFileRecordResponseFixture
    {
        [Fact]
        public void Create()
        {
            var response = new ReadFileRecordResponse(17);
            Assert.Equal(ModbusFunctionCodes.ReadFileRecord, response.FunctionCode);
            Assert.Equal(17, response.SlaveAddress);
        }

        [Fact]
        public void CreateWithData()
        {
            var response = new ReadFileRecordResponse(17, new FileRecordDataCollection(1, 2, new byte[] { 4, 5 }));
            Assert.Equal(ModbusFunctionCodes.ReadFileRecord, response.FunctionCode);
            Assert.Equal(17, response.SlaveAddress);
            Assert.Equal(1, response.Data.FileNumber);
            Assert.Equal(2, response.Data.RecordNumber);
            Assert.Equal(new byte[] { 4, 5 }, response.Data.DataBytes);
        }

        [Fact]
        public void Initialize()
        {
            var response = new ReadFileRecordResponse();
            response.Initialize(new byte[] {
                17, ModbusFunctionCodes.ReadFileRecord, 9, 6, 0, 1, 0, 2, 0, 1, 4, 5
            });

            Assert.Equal(ModbusFunctionCodes.ReadFileRecord, response.FunctionCode);
            Assert.Equal(17, response.SlaveAddress);
            Assert.Equal(1, response.Data.FileNumber);
            Assert.Equal(2, response.Data.RecordNumber);
            Assert.Equal(new byte[] { 4, 5 }, response.Data.DataBytes);
        }

        [Fact]
        public void ToString_Test()
        {
            var response = new ReadFileRecordResponse(17, new FileRecordDataCollection(1, 2, new byte[] { 4, 5 }));

            Assert.Equal("Read 2 bytes for file 1 starting at address 2.", response.ToString());
        }
    }
}