using System.Collections.Generic;

namespace PasswordManager.Contracts
{
    public class SecretDataDto
    {
        public int Id { get; set; }
        public int DataTypeId { get; set; }
        public List<FieldDto> Fields { get; set; }
    }

    public class FieldDto
    {
        public int Id { get; set; }
        public string Name { get; set; }
        public string Value { get; set; }
        public bool IsUsingUniversalPassword { get; set; }
    }
}