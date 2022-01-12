using NModbus.Data;
using Xunit;

namespace NModbus.UnitTests.Data
{
    public class FileRecordCollectionFixture
    {
        private protected virtual FileRecordCollection FileRecordCollection => new FileRecordCollection(1, 2, 2);

        [Fact]
        public virtual void ByteCount()
        {
            Assert.Equal(7, FileRecordCollection.ByteCount);
        }

        [Fact]
        public void FileNumber()
        {
            Assert.Equal(1, FileRecordCollection.FileNumber);
        }

        [Fact]
        public void StartingAdress()
        {
            Assert.Equal(2, FileRecordCollection.RecordNumber);
        }

        [Fact]
        public virtual void NetworkBytes()
        {
            Assert.Equal(new byte[] { 6, 0, 1, 0, 2, 0, 2 }, FileRecordCollection.NetworkBytes);
        }
    }
}
