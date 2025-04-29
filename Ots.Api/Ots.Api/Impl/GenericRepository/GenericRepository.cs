using System;
 using System.Collections.Generic;
 using System.Linq;
 using System.Linq.Expressions;
 using System.Threading.Tasks;
 using Microsoft.EntityFrameworkCore;
 using Ots.Base;
 
 namespace Ots.Api.Impl.GenericRepository;
 
 public class GenericRepository<TEntity> : IGenericRepository<TEntity> where TEntity : BaseEntity
 {
     private readonly OtsMsSqlDbContext dbContext;
 
     public GenericRepository(OtsMsSqlDbContext dbContext)
     {
         this.dbContext = dbContext;
     }
 
 
     public async Task<TEntity> AddAsync(TEntity entity)
     {
         await dbContext.Set<TEntity>().AddAsync(entity);
         return entity;
     }
 
 
     public async Task DeleteByIdAsync(long id)
     {
         var entity = await dbContext.Set<TEntity>().FindAsync(id);
         if (entity != null)
         {
             dbContext.Set<TEntity>().Remove(entity);
         }
     }
 
 
     public void Delete(TEntity entity)
     {
         dbContext.Set<TEntity>().Remove(entity);
     }
 
 
     public async Task<List<TEntity>> GetAllAsync(params string[] includes)
     {
         var query = dbContext.Set<TEntity>().AsQueryable();
         query = includes.Aggregate(query, (current, inc) => EntityFrameworkQueryableExtensions.Include(current, inc));
         return await EntityFrameworkQueryableExtensions.ToListAsync(query);
     }
 
     public async Task<List<TEntity>> GetAllAsync(Expression<Func<TEntity, bool>> predicate, params string[] includes)
     {
         var query = dbContext.Set<TEntity>().Where(predicate).AsQueryable();
         query = includes.Aggregate(query, (current, inc) => EntityFrameworkQueryableExtensions.Include(current, inc));
         return await EntityFrameworkQueryableExtensions.ToListAsync(query);
     }
 
     public async Task<TEntity> GetByIdAsync(long id, params string[] includes)
     {
         var query = dbContext.Set<TEntity>().AsQueryable();
         query = includes.Aggregate(query, (current, inc) => EntityFrameworkQueryableExtensions.Include(current, inc));
         return await EntityFrameworkQueryableExtensions.FirstOrDefaultAsync(query, x => x.Id == id);
     }
 
     public async Task SaveChangesAsync()
     {
         await dbContext.SaveChangesAsync();
     }
 
     public void Update(TEntity entity)
     {
         dbContext.Set<TEntity>().Update(entity);
     }
 
     public async Task<List<TEntity>> Where(Expression<Func<TEntity, bool>> predicate, params string[] includes)
     {
         var query = dbContext.Set<TEntity>().Where(predicate).AsQueryable();
         query = includes.Aggregate(query, (current, inc) => EntityFrameworkQueryableExtensions.Include(current, inc));
         return await EntityFrameworkQueryableExtensions.ToListAsync(query);
     }
 }