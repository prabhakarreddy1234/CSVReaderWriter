using System;
using System.IO;
using AddressProcessing.Interfaces;

namespace AddressProcessing.CSV
{
    public class CSVWriter : IFileWriter, IDisposable
    {
        private StreamWriter _writerStream;

        public void Open(string fileName, FileMode mode)
        {
            if (mode != FileMode.Write)
                throw new InvalidOperationException();
            if (!File.Exists(fileName))
                throw new FileNotFoundException();
            var fileInfo = new FileInfo(fileName);
            _writerStream = fileInfo.CreateText();
        }

        public void Close()
        {
            _writerStream?.Close();
        }

        /// <summary>
        /// Writes data into CSV file
        /// </summary>
        /// <param name="columns">columns data</param>
        public void Write(params string[] columns)
        {
            var outPut = string.Join("\t", columns);

            try { WriteLine(outPut); }
            catch (IOException)
            {
                //It's good practice to Log these errors with stacktrace in Database so that troubleshooting would be easier
                throw new IOException("cannot write file");
            }
            catch (ObjectDisposedException)
            {
                //It's good practice to Log these errors with stacktrace in Database so that troubleshooting would be easier
                throw new InvalidOperationException("cannot access file as it has been disposed already");
            }
        }

        public void Dispose()
        {
            _writerStream?.Close();
        }

        private void WriteLine(string line)
        {
            _writerStream.WriteLine(line);
        }
    }
}
