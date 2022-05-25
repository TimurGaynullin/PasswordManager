using PasswordManager.Contracts;
using PasswordManager.Database;
using PasswordManager.Database.Models.Entities;
using PasswordManager.Domain.Abstractions;

namespace PasswordManager.Domain
{
    public class SecretDataService : ISecretDataService
    {
        private readonly IAesProtector _aesProtector;
        private readonly StorageContext db;
        private readonly string _universalPassword = "universalPassword"; 
        
        public SecretDataService(StorageContext context, IAesProtector aesProtector)
        {
            db = context;
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
            var updatedData = user.SecretDatas.FirstOrDefault(p => p.Id == secretDataDto.Id);
            
            if (updatedData != null)
            {
                updatedData.DataTypeId = secretDataDto.DataTypeId;
                updatedData.Fields = new List<Field>();
                updatedData.IsUsingUniversalPassword = false;
                updatedData.Name = secretDataDto.Name;
                updatedData.UserId = user.Id;
                foreach (var field in secretDataDto.Fields)
                {
                    updatedData.Fields.Add(new Field
                    {
                        Name = field.Name,
                        Value = _aesProtector.ToAes256(field.Value, $"{masterPassword}{user.MasterPasswordHash}")
                    });
                }

                db.SecretDatas.Update(updatedData);
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
            var secretData = userSender.SecretDatas.FirstOrDefault(p => p.Id == secretDataId);
            if (secretData != null)
            {
                var fields = new List<Field>();
                foreach (var field in secretData.Fields)
                {
                    var cryptDataValue = _aesProtector.FromAes256(field.Value,
                        $"{masterPassword}{userSender.MasterPasswordHash}");
                    cryptDataValue = _aesProtector.ToAes256(cryptDataValue,
                        $"{_universalPassword}{userReciver.MasterPasswordHash}");
                    
                    fields.Add(new Field
                    {
                        Name = field.Name,
                        Value = cryptDataValue
                    });
                }

                var newSecretData = new SecretData
                {
                    Name = secretData.Name,
                    DataTypeId = secretData.DataTypeId,
                    Fields = fields,
                    UserId = userReciver.Id,
                    IsUsingUniversalPassword = true
                };
                userReciver.SecretDatas.Add(newSecretData);
                await db.SaveChangesAsync();
                return true;
            }
            return false;
        }

        public async Task<bool> RecieveSharingSecretDatas(User user, string masterPassword)
        {
            var universalDatas = user.SecretDatas.FindAll(x => x.IsUsingUniversalPassword);
            foreach (var universalData in universalDatas)
            {
                foreach (var field in universalData.Fields)
                {
                    var openData = _aesProtector.FromAes256(field.Value,
                        $"{_universalPassword}{user.MasterPasswordHash}");
                    var closeData = _aesProtector.ToAes256(openData,
                        $"{masterPassword}{user.MasterPasswordHash}");
                    field.Value = closeData;
                }
                universalData.IsUsingUniversalPassword = false;
            }

            await db.SaveChangesAsync();
            return true;
        }
    }
}