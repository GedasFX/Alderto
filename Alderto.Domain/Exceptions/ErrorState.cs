namespace Alderto.Domain.Exceptions
{
    public class ErrorState
    {
        public ErrorStatusCode StatusCode { get; }

        public ErrorState(ErrorStatusCode statusCode)
        {
            StatusCode = statusCode;
        }
    }

    public enum ErrorStatusCode
    {
        BadRequest = 400,
        Forbidden = 403,
        NotFound = 404,
    }
}
