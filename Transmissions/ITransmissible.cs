using System.IO;

namespace Transmissions
{
    public interface ITransmissible
    {

        int GetByteLength();
        void Serialize(Stream stream);
        void Deserialize(byte[] array);

    }
}
