namespace DPWH.EDMS.Domain.Entities;
//public record AuthorizedRepresentativeaa(string? Name, string? ValidId, string? ValidIdUri, 
//    string? SupportingDocument, string? SupportingDocumentUri);

public class AuthorizedRepresentative
{
    private AuthorizedRepresentative() { }
    public static AuthorizedRepresentative Create(string? name, string? validId, string? validIdUri,
    string? supportingDocument, string? supportingDocumentUri )
    {
        return new AuthorizedRepresentative {
            RepresentativeName = name, 
            ValidId = validId, 
            ValidIdUri = validIdUri, 
            SupportingDocument = supportingDocument, 
            SupportingDocumentUri = supportingDocumentUri
        };
    }
    public Guid Id { get; private set; }
    public string? RepresentativeName { get; private set; }
    public string? ValidId { get; private set; }
    public string? ValidIdUri { get; private set; }
    public string? SupportingDocument { get; private set; }
    public string? SupportingDocumentUri { get; private set; }

    
}

