using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using AutoMapper;
using Microsoft.EntityFrameworkCore;
using PasswordManager.Contracts;
using PasswordManager.Database;
using PasswordManager.Domain.Abstractions;

namespace PasswordManager.Domain
{
    public class DataTypeService : IDataTypeService
    {
        private readonly StorageContext db;
        private readonly IMapper _mapper;

        public DataTypeService(StorageContext db, IMapper mapper)
        {
            this.db = db;
            _mapper = mapper;
        }
        public async Task<List<DataTypeDto>> DataTypes()
        {
            var types = db.DataTypes.Include(x=>x.TypeFields)
                .Select(type => _mapper.Map<DataTypeDto>(type)).ToList();
            return types;
        }
    }
}