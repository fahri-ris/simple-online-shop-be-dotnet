namespace simple_online_shop_be_dotnet.Dtos;

public class PaginationResponse<T>
{
    public int CurrentPage { get; set; }
    public int PageSize { get; set; }
    public int TotalItems { get; set; }
    public int TotalPages 
    {
        get 
        {
            return (int)Math.Ceiling((double)TotalItems / PageSize);
        }
    }
    public T Data { get; set; }
    
    public PaginationResponse(int currentPage, int pageSize, int totalItems, T data)
    {
        CurrentPage = currentPage;
        PageSize = pageSize;
        TotalItems = totalItems;
        Data = data;
    }
}