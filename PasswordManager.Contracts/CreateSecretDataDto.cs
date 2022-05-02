using System.Collections.Generic;

namespace PasswordManager.Contracts
{
    public class CreateSecretDataDto
    {
        public string Name { get; set; }
        public int DataTypeId { get; set; }
        public List<FieldDto> Fields { get; set; }
        public string MasterPassword { get; set; }
    }
    
    public class FieldDto
    {
        public string Name { get; set; }
        public string Value { get; set; }
    }
}