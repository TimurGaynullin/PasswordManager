using System;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.DependencyInjection;

namespace PasswordManager.Database
{
    public static class Entry
    {
        public static IServiceCollection AddStorageDbContext(this IServiceCollection serviceCollection, Action<DbContextOptionsBuilder> optionsAction = null)
        {
            serviceCollection.AddDbContext<StorageContext>(optionsAction);
            /*
            serviceCollection.AddScoped<IUserRepository, UserRepository>();
            serviceCollection.AddScoped<IRepository<Password>, Repository<Password, Models.Entities.Password>>();
            serviceCollection.AddScoped<IGroupRepository, GroupRepository>();
            serviceCollection.AddScoped<IFolderRepository, FolderRepository>();

            serviceCollection.AddScoped<IRepository<UserGroup>, Repository<UserGroup,Models.Entities.UserGroup>>();
            serviceCollection.AddScoped<IRepository<GroupPassword>, Repository<GroupPassword, Models.Entities.GroupPassword>>();
            serviceCollection
                .AddScoped<IRepository<FolderPassword>,
                    Repository<FolderPassword, Models.Entities.FolderPassword>>();
*/
            return serviceCollection;

        }
    }
}