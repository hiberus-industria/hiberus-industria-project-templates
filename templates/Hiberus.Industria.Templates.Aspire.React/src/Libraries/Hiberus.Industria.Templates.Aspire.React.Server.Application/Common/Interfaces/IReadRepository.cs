using Ardalis.Specification;

namespace Hiberus.Industria.Templates.Aspire.React.Server.Application.Common.Interfaces;

/// <summary>
/// Generic read repository interface for entities of type T.
/// Inherits from IReadRepositoryBase to provide read-only operations.
/// </summary>
/// <typeparam name="T">The type of the entity.</typeparam>
public interface IReadRepository<T> : IReadRepositoryBase<T>
    where T : class
{ }
