using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using AutoMapper;
using Microsoft.EntityFrameworkCore.Metadata;
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
        private readonly string _universalPassword = "universalPassword"; 
        
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

            var passwordsDto = passwords.Select(password => _mapper.Map<PasswordDto>(password)).ToList();
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

        public async Task<bool> SharePassword(int passwordId, User userSender, User userReciver, string masterPassword)
        {
            var password = userSender.Passwords.FirstOrDefault(p => p.Id == passwordId);
            if (password != null)
            {
                var cryptPasswordValue = _aesProtector.FromAes256(password.CryptPasswordValue,
                    $"{masterPassword}{userSender.MasterPasswordHash}");
                cryptPasswordValue = _aesProtector.ToAes256(cryptPasswordValue,
                    $"{_universalPassword}{userReciver.MasterPasswordHash}");

                var newPassword = new Password
                {
                    Name = password.Name,
                    Login = password.Login,
                    UserId = userReciver.Id,
                    CryptPasswordValue = cryptPasswordValue,
                    IsUsingUniversalPassword = true
                };
                userReciver.Passwords.Add(newPassword);
                await db.SaveChangesAsync();
                return true;
            }
            return false;
        }

        public async Task<bool> RecieveSharingPasswords(User user, string masterPassword)
        {
            var universalPasswords = user.Passwords.FindAll(x => x.IsUsingUniversalPassword);
            foreach (var universalPassword in universalPasswords)
            {
                var openPassword = _aesProtector.FromAes256(universalPassword.CryptPasswordValue,
                    $"{_universalPassword}{user.MasterPasswordHash}");
                var closePassword = _aesProtector.ToAes256(openPassword, $"{masterPassword}{user.MasterPasswordHash}");
                universalPassword.CryptPasswordValue = closePassword;
                universalPassword.IsUsingUniversalPassword = false;
            }

            await db.SaveChangesAsync();
            return true;
        }
    }
}