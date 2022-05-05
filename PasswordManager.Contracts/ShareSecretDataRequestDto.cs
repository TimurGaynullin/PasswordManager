namespace PasswordManager.Contracts
{
    public class ShareSecretDataRequestDto
    {
        public int SecretDataId { get; set; }
        public int UserId { get; set; }
        public string MasterPassword { get; set; }
    }
}