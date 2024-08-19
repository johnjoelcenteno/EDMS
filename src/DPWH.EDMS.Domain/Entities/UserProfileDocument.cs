using DPWH.EDMS.Domain.Common;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace DPWH.EDMS.Domain.Entities
{
    public class UserProfileDocument : EntityBase
    {
        public string EmployeeNumber { get; set; }
        public string UriSignature { get; set; }

        public UserProfileDocument(string employeeNumber, string uriSignature, string createdBy)
        {
            Id = Guid.NewGuid();
            EmployeeNumber = employeeNumber;
            UriSignature = uriSignature;
            SetCreated(createdBy);
        }

        public void Update(string uriSignature, string updatedBy)
        {
            UriSignature = uriSignature;
            SetModified(updatedBy);
        }
    }

}
