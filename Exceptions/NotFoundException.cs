namespace simple_online_shop_be_dotnet.Exceptions;

public class NotFoundException : Exception
{
    public NotFoundException(string message) : base(message) { }
}