using NModbus.Data;
using System;
using Xunit;

namespace NModbus.UnitTests.Data
{
    public class FileRecordDataCollectionFixture : FileRecordCollectionFixture
    {
        private protected override FileRecordCollection FileRecordCollection
            => new FileRecordDataCollection(1, 2, new byte[] { 1, 2, 3, 4 });

        [Fact]
        public void Constructor_ThrowsOddByteCount()
        {
            var exception = Assert.Throws<ArgumentException>(() => new FileRecordDataCollection(1, 2, new byte[] { 1, 2, 3 }));
            Assert.Equal("data", exception.ParamName);
        }

        [Fact]
        public override void ByteCount()
        {
            Assert.Equal(11, FileRecordCollection.ByteCount);
        }

        [Fact]
        public override void NetworkBytes()
        {
            Assert.Equal(new byte[] { 6, 0, 1, 0, 2, 0, 2, 1, 2, 3, 4 }, FileRecordCollection.NetworkBytes);
        }
    }
}
