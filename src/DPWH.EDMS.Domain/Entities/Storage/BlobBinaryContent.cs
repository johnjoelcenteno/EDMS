namespace DPWH.EDMS.Domain.Entities.Storage;

public class BlobBinaryContent
{
    public byte[] BinaryContent { get; private set; }
    public string ContentType { get; private set; }
    public BlobBinaryContent(byte[] binaryContent, string contentType)
    {
        if (binaryContent == null || binaryContent.Length == 0)
        {
            throw new ArgumentException($"{nameof(binaryContent)} cannot be null or empty.");
        }

        if (string.IsNullOrWhiteSpace(contentType))
        {
            throw new ArgumentException($"{nameof(contentType)} cannot be null or empty.");
        }

        BinaryContent = binaryContent;
        ContentType = contentType;
    }
}
