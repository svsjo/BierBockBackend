namespace BierBockBackend.Data;

public class RequestStatus<T>
{
    public Status Status { get; set; }
    public string StatusMessage => Status.ToString();
    public ErrorCodes ErrorCode { get; set; }
    public string DetailledErrorMessage => ErrorCode.Message;
    public T? Result { get; set; } 
}

public enum Status
{
    Successful,
    Error
}

