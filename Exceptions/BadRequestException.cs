namespace simple_online_shop_be_dotnet.Exceptions;

public class BadRequestException : Exception
{
    public BadRequestException(string message) : base(message) { }
}