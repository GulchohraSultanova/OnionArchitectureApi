using Microsoft.EntityFrameworkCore;
using OnionArchitectureApi.Application.Abstractions.Repositories;
using OnionArchitectureApi.Persistence.Contexts;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace OnionArchitectureApi.Persistence.Concreters.Repositories
{
    public class WriteRepository<T> : IWriteRepository<T> where T : class, new()
    {
        private readonly OnionArchitectureAppDbContext _onionWebApiDbContext;

        public WriteRepository(OnionArchitectureAppDbContext onionWebApiDbContext)
        {
            _onionWebApiDbContext = onionWebApiDbContext;
        }

        private DbSet<T> Table { get => _onionWebApiDbContext.Set<T>(); }
        public async Task AddAsync(T entity)
        {
            await Table.AddAsync(entity);
        }

        public async Task HardDeleteAsync(T entity)
        {
            await Task.Run(() => Table.Remove(entity));

        }

        public async Task<T> UpdateAsync(T entity)
        {
            await Task.Run(() => Table.Update(entity));
            return entity;
        }

        public async Task<int> CommitAsync()
        {
            return await _onionWebApiDbContext.SaveChangesAsync();
        }
    }
}
