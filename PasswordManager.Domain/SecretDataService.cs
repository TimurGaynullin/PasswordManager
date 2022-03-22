using System.Collections.Generic;
using System.Threading.Tasks;
using AutoMapper;
using PasswordManager.Contracts;
using PasswordManager.Database;
using PasswordManager.Database.Models.Entities;
using PasswordManager.Domain.Abstractions;

namespace PasswordManager.Domain
{
    public class SecretDataService : ISecretDataService
    {
        private readonly IMapper _mapper;
        private readonly IAesProtector _aesProtector;
        private readonly StorageContext db;
        private readonly string _universalPassword = "universalPassword"; 
        
        public SecretDataService(StorageContext context, IMapper mapper, IAesProtector aesProtector)
        {
            db = context;
            _mapper = mapper;
            _aesProtector = aesProtector;
        }
        
        public async Task<SecretDataDto> GetSecretData(int id, User user, string masterPassword)
        {
            throw new System.NotImplementedException();
        }

        public async Task<List<SecretDataDto>> GetSecretDatas(User user, string masterPassword)
        {
            throw new System.NotImplementedException();
        }

        public async Task<SecretDataDto> CreateSecretData(SecretDataDto secretDataDto, User user, string masterPassword)
        {
            throw new System.NotImplementedException();
        }

        public async Task<SecretDataDto> UpdateSecretData(SecretDataDto secretDataDto, User user, string masterPassword)
        {
            throw new System.NotImplementedException();
        }

        public async Task<bool> DeleteSecretData(int id, User user)
        {
            throw new System.NotImplementedException();
        }

        public async Task<bool> ShareSecretData(int secretDataId, User userSender, User userReciver, string masterPassword)
        {
            throw new System.NotImplementedException();
        }

        public async Task<bool> RecieveSharingSecretDatas(User user, string masterPassword)
        {
            throw new System.NotImplementedException();
        }
    }
}