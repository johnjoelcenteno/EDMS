namespace DPWH.EDMS.Domain.Entities;
public class AuthorizedRepresentative
{
    private AuthorizedRepresentative() { }
    public static AuthorizedRepresentative Create(string? name, Guid? validId, Guid? authorizationDocumentId)
    {
        return new AuthorizedRepresentative {
            RepresentativeName = name, 
            ValidId = validId,
            AuthorizationDocumentId = authorizationDocumentId
        };
    }
    public Guid Id { get; private set; }
    public string? RepresentativeName { get; private set; }
    public Guid? ValidId { get; private set; }    
    public Guid? AuthorizationDocumentId { get; private set; }
    
}

