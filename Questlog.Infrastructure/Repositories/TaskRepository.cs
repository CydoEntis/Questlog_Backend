﻿using System.Linq.Expressions;
using Microsoft.EntityFrameworkCore;
using Questlog.Application.Common;
using Questlog.Application.Common.Enums;
using Questlog.Application.Common.Interfaces;
using Questlog.Application.Common.Models;
using Questlog.Domain.Entities;
using Questlog.Infrastructure.Data;
using Task = Questlog.Domain.Entities.Task;


namespace Questlog.Infrastructure.Repositories;

public class TaskRepository : BaseRepository<Task>, ITaskRepository
{
    private readonly ApplicationDbContext _db;

    public TaskRepository(ApplicationDbContext db) : base(db)
    {
        _db = db;
    }

    // public async Task<PaginatedResult<Subquest>> GetAllAsync(SubquestQueryOptions options)
    // {
    //     IQueryable<Subquest> query = _dbSet;
    //
    //     if (options.Filter != null)
    //     {
    //         query = query.Where(options.Filter);
    //     }
    //
    //     if (!string.IsNullOrEmpty(options.OrderOn))
    //     {
    //         query = ApplyOrdering(query, options.OrderOn, options.OrderBy);
    //     }
    //
    //     if (!string.IsNullOrEmpty(options.IncludeProperties))
    //     {
    //         query = ApplyIncludeProperties(query, options.IncludeProperties);
    //     }
    //
    //     return await Paginate(query, options.PageNumber, options.PageSize);
    // }


    public async Task<Task> UpdateAsync(Task entity)
    {
        entity.UpdatedAt = DateTime.Now;
        _db.Tasks.Update(entity);
        await _db.SaveChangesAsync();
        return entity;
    }


    private IQueryable<Task> ApplyIncludeProperties(IQueryable<Task> query, string includeProperties)
    {
        foreach (var includeProp in includeProperties.Split(new[] { ',' }, StringSplitOptions.RemoveEmptyEntries))
        {
            query = query.Include(includeProp);
        }
        return query; 
    }


    private IQueryable<Task> ApplyOrdering(IQueryable<Task> query, string orderOn, string orderBy)
    {
        var orderDirection = Enum.TryParse<OrderBy>(orderBy, true, out var order) ? order : OrderBy.Desc;

        return orderOn.ToLower() switch
        {
            "name" => orderDirection == OrderBy.Asc
                ? query.OrderBy(c => c.Description)
                : query.OrderByDescending(c => c.Description),
            "createdat" => orderDirection == OrderBy.Asc
                ? query.OrderBy(c => c.CreatedAt)
                : query.OrderByDescending(c => c.CreatedAt),
            "updatedat" => orderDirection == OrderBy.Asc
                ? query.OrderBy(c => c.UpdatedAt)
                : query.OrderByDescending(c => c.UpdatedAt),
            "duedate" => orderDirection == OrderBy.Asc
                ? query.OrderBy(c => c.UpdatedAt)
                : query.OrderByDescending(c => c.UpdatedAt),
            _ => query
        };
    }

}