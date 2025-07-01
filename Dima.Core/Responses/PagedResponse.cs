using System.Text.Json.Serialization;

namespace Dima.Core.Responses;

public class PagedResponse<TData> : Response<TData>
{
    [JsonConstructor]
    public PagedResponse(
        TData? data,
        int totalCount,
        int currentPage = 1,
        int pageSize = Configuration.DefaultPageSize) : base(data)
    {
        Data = data;
        PageSize = pageSize;
        TotalCount = totalCount;
        CurrentPage = currentPage;
    }

    public PagedResponse(
        TData? data,
        int code = Configuration.DefaultStatusCode,
        string? message = null) : base(data, code, message)
    {}
    
    public int CurrentPage { get; set; }
    public int TotalPages => (int)Math.Ceiling(TotalCount / (double)PageSize);
    public int PageSize { get; set; }
    public int TotalCount { get; set; }
}