namespace BierBockBackend.Data;

public class RequestStatus<T>
{
    public Status Status { get; set; }
    public string StatusMessage => Status.ToString();
    public string DetailledErrorMessage { get; set; } = string.Empty;
    public T? Result { get; set; }
}

public enum Status
{
    Successful,
   // NoPermission,
    NoResults,
    AlreadyPresent,
}