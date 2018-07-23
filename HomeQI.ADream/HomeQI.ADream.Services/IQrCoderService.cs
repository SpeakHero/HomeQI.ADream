namespace HomeQI.ADream.Services.QrCode
{
    public interface IQrCoderService
    {
        IQrCoderService Correction(ErrorCorrectionLevel level);
        byte[] CreateQrCode(string content);
        IQrCoderService Size(int size);
        IQrCoderService Size(QrSize size);
    }
}