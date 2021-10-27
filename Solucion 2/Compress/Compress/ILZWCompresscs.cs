namespace Compress
{
    public interface ILZWCompresscs
    {
        byte[] compress(byte[] data);
        byte[] descompress(byte[] data);
    }
}