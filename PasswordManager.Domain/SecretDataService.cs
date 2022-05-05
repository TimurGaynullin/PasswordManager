using System.Collections.Generic;
using System.Linq;
using System.Reflection.Metadata;
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
            var secretData = user.SecretDatas.FirstOrDefault(p => p.Id == id);
            if (secretData != null)
            {
                var secretDataDto = new SecretDataDto
                {
                    Id = secretData.Id,
                    Name = secretData.Name,
                    Type = secretData.DataType.Name,
                    Fields = new Dictionary<string, string>()
                };
                foreach (var field in secretData.Fields)
                {
                    var openValue = _aesProtector.FromAes256(field.Value, $"{masterPassword}{user.MasterPasswordHash}");
                    secretDataDto.Fields.Add(field.Name, openValue);
                }
                return secretDataDto;
            }
            return null;
        }

        public async Task<List<SecretDataDto>> GetSecretDatas(User user, string masterPassword)
        {
            var secretDatas = user.SecretDatas;
            var response = new List<SecretDataDto>();
            foreach (var secretData in secretDatas)
            {
                var secretDataDto = new SecretDataDto
                {
                    Id = secretData.Id,
                    Name = secretData.Name,
                    Type = secretData.DataType.Name,
                    Fields = new Dictionary<string, string>()
                };
                foreach (var field in secretData.Fields)
                {
                    var openData = _aesProtector.FromAes256(field.Value, $"{masterPassword}{user.MasterPasswordHash}");
                    secretDataDto.Fields.Add(field.Name, openData);
                }
                response.Add(secretDataDto);
            }
            
            return response;
        }

        public async Task<SecretDataDto> CreateSecretData(CreateSecretDataDto secretDataDto, User user, string masterPassword)
        {
            var secretData = new SecretData
            {
                DataTypeId = secretDataDto.DataTypeId,
                Fields = new List<Field>(),
                IsUsingUniversalPassword = false,
                Name = secretDataDto.Name,
                UserId = user.Id
            };
            foreach (var field in secretDataDto.Fields)
            {
                secretData.Fields.Add(new Field
                {
                    Name = field.Name,
                    Value = _aesProtector.ToAes256(field.Value, $"{masterPassword}{user.MasterPasswordHash}")
                });
            }
            
            await db.SecretDatas.AddAsync(secretData);
            await db.SaveChangesAsync();
            return new SecretDataDto();
        }

        public async Task<SecretDataDto> UpdateSecretData(CreateSecretDataDto secretDataDto, User user, string masterPassword)
        {
            var secretData = new SecretData
            {
                Id = secretDataDto.Id,
                DataTypeId = secretDataDto.DataTypeId,
                Fields = new List<Field>(),
                IsUsingUniversalPassword = false,
                Name = secretDataDto.Name,
                UserId = user.Id
            };
            foreach (var field in secretDataDto.Fields)
            {
                secretData.Fields.Add(new Field
                {
                    Name = field.Name,
                    Value = _aesProtector.ToAes256(field.Value, $"{masterPassword}{user.MasterPasswordHash}")
                });
            }
            
            var updatedPassword = user.SecretDatas.FirstOrDefault(p => p.Id == secretData.Id);

            if (updatedPassword != null)
            {
                db.SecretDatas.Update(secretData);
                await db.SaveChangesAsync();
                return new SecretDataDto();
            }
            return null;
        }

        public async Task<bool> DeleteSecretData(int id, User user)
        {
            var secretData = user.SecretDatas.FirstOrDefault(p => p.Id == id);
            if (secretData != null)
            {
                db.SecretDatas.Remove(secretData);
                await db.SaveChangesAsync();
                return true;
            }
            return false;
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