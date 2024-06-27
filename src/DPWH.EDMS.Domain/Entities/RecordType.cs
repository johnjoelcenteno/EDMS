using DPWH.EDMS.Domain.Common;
using System;

namespace DPWH.EDMS.Domain
{
    public class RecordType : EntityBase
    {
        private RecordType() { }

        private RecordType(Guid id, string name, string category, string section, string office, bool isActive)
        {
            Id = id;
            Name = name;
            Category = category;
            Section = section;
            Office = office;
            IsActive = isActive;
        }

        public static RecordType Create(string name, string category, string section, string office, bool isActive, string createdBy)
        {
            var id = Guid.NewGuid();

            var recordType = new RecordType(id, name, category, section, office, isActive);
            recordType.SetCreated(createdBy);
            return recordType;
        }

        public void Update(string name, string category, string section, string office, bool isActive, string modifiedBy)
        {
            Name = name;
            Category = category;
            Section = section;
            Office = office;
            IsActive = isActive;
            SetModified(modifiedBy);
        }

        public string Name { get; private set; }
        public string Category { get; private set; }
        public string Section { get; private set; }
        public string Office { get; private set; }
        public bool IsActive { get; private set; }
    }
}
