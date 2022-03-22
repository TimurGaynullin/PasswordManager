using System.Collections.Generic;
using System.Threading.Tasks;
using PasswordManager.Contracts;
using PasswordManager.Database.Models.Entities;

namespace PasswordManager.Domain.Abstractions
{
    public interface ISecretDataService
    {
        Task<SecretDataDto> GetSecretData(int id, User user, string masterPassword);

        Task<List<SecretDataDto>> GetSecretDatas(User user, string masterPassword);

        Task<SecretDataDto> CreateSecretData(SecretDataDto secretDataDto, User user, string masterPassword);

        Task<SecretDataDto> UpdateSecretData(SecretDataDto secretDataDto, User user, string masterPassword);

        Task<bool> DeleteSecretData(int id, User user);

        Task<bool> ShareSecretData(int secretDataId, User userSender, User userReciver, string masterPassword);

        Task<bool> RecieveSharingSecretDatas(User user, string masterPassword);
    }
}