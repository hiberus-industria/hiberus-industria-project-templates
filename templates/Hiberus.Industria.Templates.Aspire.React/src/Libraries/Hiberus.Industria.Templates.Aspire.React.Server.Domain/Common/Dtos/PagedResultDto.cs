using System.Collections.ObjectModel;

namespace Hiberus.Industria.Templates.Aspire.React.Server.Domain.Common.Dtos;

/// <summary>
/// Represents a paginated result containing a collection of items and pagination metadata.
/// </summary>
/// <typeparam name="T">The type of items in the paginated result.</typeparam>
public class PagedResultDto<T>
{
    /// <summary>
    /// Initializes a new instance of the <see cref="PagedResultDto{T}"/> class.
    /// </summary>
    /// <param name="items">The collection of items for the current page.</param>
    /// <param name="page">The current page number.</param>
    /// <param name="pageSize">The number of items per page.</param>
    /// <param name="totalCount">The total number of items across all pages.</param>
    public PagedResultDto(IEnumerable<T> items, int page, int pageSize, int totalCount)
    {
        this.Items = items.ToList().AsReadOnly();
        this.Page = page;
        this.PageSize = pageSize;
        this.TotalCount = totalCount;
    }

    /// <summary>
    /// Gets the read-only collection of items for the current page.
    /// </summary>
    public ReadOnlyCollection<T> Items { get; }

    /// <summary>
    /// Gets the current page number.
    /// </summary>
    public int Page { get; init; }

    /// <summary>
    /// Gets the number of items per page.
    /// </summary>
    public int PageSize { get; init; }

    /// <summary>
    /// Gets the total number of items across all pages.
    /// </summary>
    public int TotalCount { get; init; }

    /// <summary>
    /// Gets the total number of pages, calculated based on the total count and page size.
    /// </summary>
    public int TotalPages => (int)Math.Ceiling((double)this.TotalCount / this.PageSize);
}
