namespace AddressProcessing.Interfaces
{
    public interface IFileConnection
    {
        void Open(string fileName, FileMode mode);

        void Close();
    }
}
