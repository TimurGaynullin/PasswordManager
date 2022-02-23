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
        public async Task<PasswordDto> GetPassword(int id, User user)
        {
            var password = user.Passwords.FirstOrDefault(p => p.Id == id);
            if (password != null)
            {
                var passwordDto = _mapper.Map<PasswordDto>(password);
                passwordDto.Value = _aesProtector.FromAes256(passwordDto.Value, "eyJhbGciOiJIUzI1NiIsInR5cCI6IkpXVCJ9.eyJzdWIiOiIxMjM0NTY3ODkwIiwibmFtZSI6IkFydHVyIEFraG1ldHNoaW4iLCJpYXQiOjIyMTAyMDE5fQ.TcQ1JqsEVmfRsqCXt6BKYdOey1VfOs4UHzcy0fjPpcU");
                return passwordDto;
            }
            return null;
        }

        public async Task<List<PasswordDto>> GetPasswords(User user)
        {
            throw new System.NotImplementedException();
        }

        public async Task<PasswordDto> CreatePassword(PasswordDto passwordDto, User user)
        {
            var password = _mapper.Map<Password>(passwordDto);
            password.CryptPasswordValue = _aesProtector.ToAes256(password.CryptPasswordValue, "eyJhbGciOiJIUzI1NiIsInR5cCI6IkpXVCJ9.eyJzdWIiOiIxMjM0NTY3ODkwIiwibmFtZSI6IkFydHVyIEFraG1ldHNoaW4iLCJpYXQiOjIyMTAyMDE5fQ.TcQ1JqsEVmfRsqCXt6BKYdOey1VfOs4UHzcy0fjPpcU");
            password.UserId = user.Id;
            var pass = await db.Passwords.AddAsync(password);
            //db.Entry(pass);
            await db.SaveChangesAsync();
            return passwordDto;
        }

        public async Task<PasswordDto> UpdatePassword(PasswordDto passwordDto, User user)
        {
            throw new System.NotImplementedException();
        }

        public async Task<bool> DeletePassword(int id, User user)
        {
            throw new System.NotImplementedException();
        }
    }
}