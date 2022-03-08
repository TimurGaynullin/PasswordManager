using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using AutoMapper;
using PasswordManager.Contracts;
using PasswordManager.Database;
using PasswordManager.Database.Models.Entities;
using PasswordManager.Domain.Abstractions;

namespace PasswordManager.Domain
{
    public class PasswordService : IPasswordService
    {
        private readonly IMapper _mapper;
        private readonly IAesProtector _aesProtector;
        private readonly StorageContext db;
        
        public PasswordService(StorageContext context, IMapper mapper, IAesProtector aesProtector)
        {
            db = context;
            _mapper = mapper;
            _aesProtector = aesProtector;
        }
        public async Task<PasswordDto> GetPassword(int id, User user, string masterPassword)
        {
            var password = user.Passwords.FirstOrDefault(p => p.Id == id);
            if (password != null)
            {
                var passwordDto = _mapper.Map<PasswordDto>(password);
                passwordDto.Value = _aesProtector.FromAes256(passwordDto.Value, $"{masterPassword}{user.MasterPasswordHash}");
                return passwordDto;
            }
            return null;
        }

        public async Task<List<PasswordDto>> GetPasswords(User user, string masterPassword)
        {
            var passwords = user.Passwords;

            var passwordsDto = passwords.Select(password => _mapper.Map<PasswordDto>(password)).ToList();//
            foreach (var pwdDto in passwordsDto)
            {
                pwdDto.Value = _aesProtector.FromAes256(pwdDto.Value, $"{masterPassword}{user.MasterPasswordHash}");
            }
            return passwordsDto;
        }

        public async Task<PasswordDto> CreatePassword(PasswordDto passwordDto, User user, string masterPassword)
        {
            var password = _mapper.Map<Password>(passwordDto);
            password.CryptPasswordValue = _aesProtector.ToAes256(password.CryptPasswordValue, $"{masterPassword}{user.MasterPasswordHash}");
            password.UserId = user.Id;
            await db.Passwords.AddAsync(password);
            await db.SaveChangesAsync();
            return passwordDto;
        }

        public async Task<PasswordDto> UpdatePassword(PasswordDto passwordDto, User user, string masterPassword)
        {
            var password = _mapper.Map<Password>(passwordDto);
            var updatedPassword = user.Passwords.FirstOrDefault(p => p.Id == password.Id);

            if (updatedPassword != null)
            {
                password.CryptPasswordValue = _aesProtector.ToAes256(passwordDto.Value, $"{masterPassword}{user.MasterPasswordHash}");

                db.Passwords.Update(password);
                await db.SaveChangesAsync();
                return passwordDto;
            }
            return null;
        }

        public async Task<bool> DeletePassword(int id, User user)
        {
            var password = user.Passwords.FirstOrDefault(p => p.Id == id);
            if (password != null)
            {
                db.Passwords.Remove(password);
                await db.SaveChangesAsync();
                return true;
            }
            return false;
        }
    }
}