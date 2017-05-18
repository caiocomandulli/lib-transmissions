namespace Transmissions
{
    public interface ITransmissionListener
    {

        ITransmissible InstantiateTransmissible();
        void OnReceive(ITransmissible received);

    }
}
