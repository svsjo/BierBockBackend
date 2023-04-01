namespace BierBockBackend.Data;

public class RequestStatus<T>
{
    public Status Status { get; set; }
    public T? Result { get; set; }
}

public enum Status
{
    Successful,
    NoPermission,
    NoResults
}