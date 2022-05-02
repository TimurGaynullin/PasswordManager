using System.Collections.Generic;

namespace PasswordManager.Contracts
{
    public class SecretDataDto
    {
        public int Id { get; set; }
        public string Name { get; set; }
        public string Type { get; set; }
        public IDictionary<string, string> Fields { get; set; }
    }

    public class DataTypeDto
    {
        public int Id { get; set; }
        public string Name { get; set; }
        public List<TypeFieldDto> TypeFields { get; set; }
    }

    public class TypeFieldDto
    {
        public string Name { get; set; }
    }
}