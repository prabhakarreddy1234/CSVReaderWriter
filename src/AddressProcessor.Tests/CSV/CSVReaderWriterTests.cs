using System;
using System.IO;
using AddressProcessing.CSV;
using AddressProcessing.Interfaces;
using NUnit.Framework;
using FileMode = AddressProcessing.FileMode;

namespace Csv.Tests
{
    [TestFixture]
    public class CSVReaderWriterTests
    {
        private IFileReader _fileReader;
        private const string TestInputFile = @"test_data\contacts.csv";

        [SetUp]
        public void SetUp()
        {
            //create infrastructure for test run
            _fileReader = new CSVReaderWriter();
        }

        [Test]
        public void Should_throw_filenotfound_exception_using_CSVReaderWriter()
        {
            Assert.Throws<FileNotFoundException>(() => _fileReader.Open("contact.csv", FileMode.Read));
        }

        [Test]
        public void Should_open_file_using_CSVReaderWriter()
        {
            _fileReader.Open(TestInputFile, FileMode.Read);

            var result = _fileReader.Read("column1", "column2");
            Assert.IsTrue(result);
        }

        [Test]
        public void Should_dispose_objects_on_close()
        {
            _fileReader.Open(TestInputFile, FileMode.Read);
            Assert.IsTrue(_fileReader.Read("column1", "column2"));
            _fileReader.Close();
            Assert.Throws<ObjectDisposedException>(() => _fileReader.Read("column1", "column2"));
        }

        [TearDown]
        public void TearDown()
        {
            // Dispose objects after test run
            _fileReader.Close();
        }
    }
}
