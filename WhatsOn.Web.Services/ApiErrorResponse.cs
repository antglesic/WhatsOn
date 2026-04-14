namespace WhatsOn.Web.Services;

internal sealed record ApiErrorResponse
{
	public string Message { get; init; } = string.Empty;
	public Guid? ErrorId { get; init; }
}
