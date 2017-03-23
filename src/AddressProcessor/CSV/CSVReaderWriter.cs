using System;
using System.IO;
using AddressProcessing.Interfaces;

namespace AddressProcessing.CSV
{
    /*
        2) Refactor this class into clean, elegant, rock-solid & well performing code, without over-engineering.
           Assume this code is in production and backwards compatibility must be maintained.
    */

    /// <summary>
    ///     Read from and Write to CSV Files
    /// </summary>
    public class CSVReaderWriter : IFileReader, IDisposable
    {
        private StreamReader _readerStream;

        //I wish i could use some Dependency Injection framework and inject all the dependencies into
        // constructor to have much cleaner code. But it will not be backward compaitable and it's a breaking change

        // I like  Single Responsibility Principle so i placed separate classes for Read and Write

        public void Open(string fileName, FileMode mode)
        {
            if(mode != FileMode.Read)
                throw new InvalidOperationException();
            if (!File.Exists(fileName))
                throw new FileNotFoundException();
            _readerStream = File.OpenText(fileName);
        }

        // I would remove this method or mark it as [Obsolete] after checking if it's been used in any other part of the application.
        // As the parameters column1 and column2 values are never been used inside the method
        public bool Read(string column1, string column2)
        {
            var line = string.Empty;
            try { line = ReadLine(); }
            catch (IOException) {
                throw new IOException("cannot read file");
            }
            catch (OutOfMemoryException) {
                throw new OutOfMemoryException("out of memory");
            }

            var columns = line.Split('\t');
            return columns.Length > 1;
        }

        /// <summary>
        /// Read data from CSV File
        /// </summary>
        /// <param name="column1">Column 1</param>
        /// <param name="column2">Column 2</param>
        /// <returns>true or false i.e read or not</returns>
        public bool Read(out string column1, out string column2)
        {
            string line;
            try { line = ReadLine(); }
            catch (IOException) {
                throw new IOException("cannot read file");
            }
            catch (OutOfMemoryException) {
                throw new OutOfMemoryException("out of memory");
            }

            if (string.IsNullOrEmpty(line))
            {
                column1 = null;
                column2 = null;

                return false;
            }

            var columns = line.Split('\t');
            //columns.Length can never be 0
            if (columns.Length > 1)
            {
                column1 = columns[0];
                column2 = columns[1];

                return true;
            }
            column1 = columns[0];
            column2 = null;

            //columns[0] can be empty or valid value. So the below check will send true or false back depending the value of column1
            return !string.IsNullOrEmpty(column1);
        }

        private string ReadLine()
        {
            return _readerStream.ReadLine();
        }

        // We don't need this method. Just to not break production code, i kept it as it is
        public void Close()
        {
             _readerStream?.Close();
        }


        //It is good way of exposing unmanaged resources
        public void Dispose()
        {
            _readerStream?.Close();
        }
    }
}
