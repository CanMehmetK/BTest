using System.Linq.Expressions;
using BTest.Infrastructure.Database.Entities;
using BTest.Infrastructure.Models;
using Microsoft.EntityFrameworkCore;

namespace BTest.Infrastructure.Database;


public interface IGenericRepository<T> where T : BaseEntity
{
  ApplicationDbContext context { get; }

  Task<List<T>> GetAll();

  T GetById(int id);
  T Find(Expression<Func<T, bool>> match);
  bool Any();
  List<T> FindAll(Expression<Func<T, bool>> match);
  Task<List<T>> FindAllAsync(Expression<Func<T, bool>> match);
  T Insert(T entity);
  Task<T> InsertAsync(T entity);
  bool BulkInsert(List<T> entities);
  T Update(T entity);
  int Delete(T entity);
  Task<T> UpdateAsync(T product);
}

public class GenericRepository<T> : IGenericRepository<T> where T : BaseEntity
{
  private readonly ApplicationDbContext _context;
  public GenericRepository(ApplicationDbContext context = null)
  {
    _context = context;
  }
  public ApplicationDbContext context => _context;
  public bool BulkInsert(List<T> entities)
  {
    _context.ChangeTracker.AutoDetectChangesEnabled = false;
    try
    {
      _context.Set<T>().AddRange(entities);
      _context.SaveChanges();
      return true;
    }
    catch
    {
      return false;
    }
  }

  public int Delete(T entity)
  {
    _context.Set<T>().Remove(entity);
    return _context.SaveChanges();
  }
  public bool Any() => _context.Set<T>().Any();
  public T Find(Expression<Func<T, bool>> match) => _context.Set<T>().FirstOrDefault(match);
  public List<T> FindAll(Expression<Func<T, bool>> match) => _context.Set<T>().Where(match).ToList();
  public async Task<List<T>> FindAllAsync(Expression<Func<T, bool>> match) => await _context.Set<T>().Where(match).ToListAsync();
  public async Task<List<T>> GetAll() => await _context.Set<T>().ToListAsync();
  public T GetById(int id) => _context.Set<T>().Find(id);

  public T Insert(T entity)
  {
    _context.Set<T>().Add(entity);
    _context.SaveChanges();
    return entity;
  }
  public async Task<T> InsertAsync(T entity)
  {
    await _context.Set<T>().AddAsync(entity);
    _context.SaveChanges();
    return entity;
  }

  public T Update(T entity)
  {
    if (entity == null)
      return null;
    _context.Set<T>().Attach(entity);
    _context.Entry(entity).State = Microsoft.EntityFrameworkCore.EntityState.Modified;
    _context.SaveChanges();
    return entity;
  }
  public async Task<T> UpdateAsync(T entity)
  {
    if (entity == null)
      return null;
    _context.Set<T>().Attach(entity);
    _context.Entry(entity).State = Microsoft.EntityFrameworkCore.EntityState.Modified;
    await _context.SaveChangesAsync();
    return entity;
  }


}

