using Ardalis.Specification;

namespace Hiberus.Industria.Templates.Aspire.React.Server.Application.Common.Interfaces;

/// <summary>
/// Generic repository interface for entities of type T.
/// Inherits from IRepositoryBase to provide CRUD operations.
/// </summary>
/// <typeparam name="T">The type of the entity.</typeparam>
public interface IRepository<T> : IRepositoryBase<T>
    where T : class { }
