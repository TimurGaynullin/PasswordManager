namespace PasswordManager.Contracts
{
    public class SharePasswordRequestDto
    {
        public int PasswordId { get; set; }
        public int UserId { get; set; }
        public string MasterPassword { get; set; }
    }
}