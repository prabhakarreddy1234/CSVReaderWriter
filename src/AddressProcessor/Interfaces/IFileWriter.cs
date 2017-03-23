namespace AddressProcessing.Interfaces
{
    public interface IFileWriter : IFileConnection
    {
        void Write(params string[] columns);
    }
}
