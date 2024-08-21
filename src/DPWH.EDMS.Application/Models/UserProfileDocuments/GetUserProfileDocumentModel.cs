using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace DPWH.EDMS.Application.Models.UserProfileDocuments
{
    public class GetUserProfileDocumentModel
    {
        public Guid Id { get; set; }
        public string EmployeeNumber { get; set; }
        public string UriSignature { get; set; }
    }
}
