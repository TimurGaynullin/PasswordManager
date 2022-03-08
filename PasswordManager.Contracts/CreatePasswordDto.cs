namespace PasswordManager.Contracts
{
    public class CreatePasswordDto
    {
        public int Id { get; set; }
        public string Login { get; set; }
        public string Value { get; set; }
        public string Name { get; set; }
        public string MasterPassword { get; set; }
    }
}