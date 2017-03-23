using System.IO;
using AddressProcessing.CSV;
using AddressProcessing.Interfaces;
using NUnit.Framework;
using Ploeh.AutoFixture;

namespace AddressProcessing.Tests.CSV
{
    [TestFixture]
    public class CSVWriterTests
    {
        private IFileWriter _fileWriter;

        private IFileReader _fileReader;

        protected Fixture FakeBuilder { get; set; }

        private const string TestInputFile = @"test_data\contacts.csv";

        [SetUp]
        public void SetUp()
        {
            //create infrastructure for test run
            _fileWriter = new CSVWriter();
            _fileReader = new CSVReaderWriter();
            FakeBuilder = new Fixture();
        }

        [Test]
        public void Should_throw_IOException_using_write()
        {
            _fileWriter.Open(TestInputFile, FileMode.Write);
            Assert.Throws<IOException>(() => _fileWriter.Open(TestInputFile, FileMode.Write));
        }

        [Test]
        public void Should_write_to_file_using_write()
        {
            _fileWriter.Open(TestInputFile, FileMode.Write);
            var column1Value = FakeBuilder.Create<string>();
            var column2Value = FakeBuilder.Create<string>();
            _fileWriter.Write(column1Value, column2Value);
            _fileWriter.Close();
            string column1, column2;
            _fileReader.Open(TestInputFile, FileMode.Read);
            var isSuccess = false;
            while (_fileReader.Read(out column1, out column2))
            {
                if (column1 == column1Value && column2 == column2Value)
                {
                    isSuccess = true;
                    break;
                }
            }

            Assert.IsTrue(isSuccess);
        }

        [TearDown]
        public void TearDown()
        {
            // Dispose objects after test run
            _fileWriter.Close();
            _fileReader.Close();
        }

    }
}
