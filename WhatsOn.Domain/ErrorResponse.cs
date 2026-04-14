namespace WhatsOn.Domain;

public sealed class ErrorResponse
{
	public string Message { get; init; } = string.Empty;
	public Guid? ErrorId { get; init; }
}
