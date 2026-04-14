namespace WhatsOn.Domain;

[Serializable]
public abstract class ResponseBase<T> where T : RequestBase
{
	public Guid ResponseToken { get; private set; } = Guid.NewGuid();

	public bool Success { get; set; } = false;

	public string Message { get; set; } = string.Empty;

	public T? Request { get; set; }

	public ErrorResponse ErrorResult => new() { Message = Message, ErrorId = ResponseToken };
}

