using Micromarin.Services.Context;
using Micromarin.Services.Dtos;
using Micromarin.Services.Entities;
using Microsoft.EntityFrameworkCore;

namespace Micromarin.Services.Services;

public interface IDynamicService {
    Task<List<DynamicQuery>> GetAllDynamicObjectAsync();
    Task<DynamicQuery> GetDynamicQueryByIdAsync(int id);
    Task<DynamicEntity> CreateDynamicObjectAsync(DynamicCommand dynamicCommand);
    Task<DynamicEntity> UpdateDynamicObjectAsync(DynamicUpdateDto dynamicUpdate);
    Task<DynamicEntity> DeleteDynamicObjectAsync(int id);
    Task<DynamicEntity> UpsertDynamicObjectAsync(DynamicUpsert dynamicUpsert);
}

public class DynamicService(MicromarinDbContext context) : IDynamicService {
    private readonly MicromarinDbContext _context = context;

    public async Task<DynamicEntity> CreateDynamicObjectAsync(DynamicCommand dynamicCommand) {
        using var transaction = await _context.Database.BeginTransactionAsync();
        try {
            var dynamicEntity = new DynamicEntity {
                Data = dynamicCommand.Data,
                Type = dynamicCommand.Type,
            };
            var result = await _context.AddAsync(dynamicEntity);
            await _context.SaveChangesAsync();
            await transaction.CommitAsync();

            return result.Entity;
        }
        catch (Exception) {
            await transaction.RollbackAsync();
            throw;
        }
    }

    public async Task<DynamicEntity> DeleteDynamicObjectAsync(int Id) {
        using var transaction = await _context.Database.BeginTransactionAsync();
        try {
            var dynamicEntity = await _context.DynamicEntities.FindAsync(Id)
                ?? throw new KeyNotFoundException($"Entity with id {Id} not found");
            var result = _context.DynamicEntities.Remove(dynamicEntity);

            return result.Entity;
        }
        catch (Exception) {
            await transaction.RollbackAsync();
            throw;
        }
    }

    public async Task<List<DynamicQuery>> GetAllDynamicObjectAsync() {
        return await _context.DynamicEntities
            .Select(d => new DynamicQuery {
                Data = d.Data,
                Type = d.Type,
                CreatedDate = d.CreatedDate,
                UpdatedDate = d.UpdatedDate
            }).ToListAsync();
    }

    public async Task<DynamicQuery> GetDynamicQueryByIdAsync(int id) {
        return await _context.DynamicEntities
            .Select(d => new DynamicQuery {
                Data = d.Data,
                Type = d.Type,
                CreatedDate = d.CreatedDate,
                UpdatedDate = d.UpdatedDate
            }).SingleOrDefaultAsync()
                    ?? throw new KeyNotFoundException($"Entity with id {id} not found");
    }

    public async Task<DynamicEntity> UpdateDynamicObjectAsync(DynamicUpdateDto dynamicUpdate) {
        using var transaction = await _context.Database.BeginTransactionAsync();
        try {
            var dynamicEntity = await _context.DynamicEntities.FindAsync(dynamicUpdate.Id)
                ?? throw new KeyNotFoundException($"Entity with id {dynamicUpdate.Id} not found");

            dynamicEntity.Data = dynamicUpdate.Data ?? dynamicEntity.Data;
            dynamicEntity.Type = dynamicUpdate.Type ?? dynamicEntity.Type;
            await _context.SaveChangesAsync();

            return dynamicEntity;
        }
        catch (Exception) {
            await transaction.RollbackAsync();
            throw;
        }
    }

    public async Task<DynamicEntity> UpsertDynamicObjectAsync(DynamicUpsert dynamicUpsert) {
        using var transaction = await _context.Database.BeginTransactionAsync();
        try {
            var dynamicEntity = await _context.DynamicEntities.FindAsync(dynamicUpsert.Id);

            if (dynamicEntity is null) {
                return await CreateDynamicObjectAsync(new DynamicCommand { Data = dynamicUpsert.Data, Type = dynamicUpsert.Type });
            }
            else {
                dynamicEntity.Data = dynamicUpsert.Data ?? dynamicEntity.Data;
                dynamicEntity.Type = dynamicUpsert.Type ?? dynamicEntity.Type;
                await _context.SaveChangesAsync();
                await transaction.CommitAsync();
                //Update method is not needed here because the entity is already being tracked by the context
            }

            return dynamicEntity;
        }
        catch (Exception) {
            await transaction.RollbackAsync();
            throw;
        }
    }
}
