namespace simple_online_shop_be_dotnet.Dtos;

public class PaginationResponse<T>
{
    public List<T> Data { get; set; }
    public int PageIndex { get; set; }
    public int TotalPages { get; set; }
    public bool HasPreviousPage => PageIndex > 1;
    public bool HasNextPage => PageIndex < TotalPages;

    public PaginationResponse(List<T> data, int pageIndex, int totalPages)
    {
        Data = data;
        PageIndex = pageIndex;
        TotalPages = totalPages;
    }
}