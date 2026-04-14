namespace WhatsOn.Domain;

[Serializable]
public abstract class ResponseBase<T> where T : RequestBase
{
	/// <summary>
	/// Unique identifier of the response. 
	/// </summary>
	public Guid ResponseToken { get; private set; } = Guid.NewGuid();

	/// <summary>
	/// Response result. True if request was successful. 
	/// If False, Client should expect some exception explanation in Message property of the response.
	/// </summary>
	public bool Success { get; set; } = false;

	/// <summary>
	/// Text message used to describe exception that occurred while executing request.
	/// Property should be null if Success property is True.
	/// </summary>
	public string Message { get; set; } = string.Empty;

	/// <summary>
	/// Request that invoked this response.
	/// </summary>
	public T? Request { get; set; }

	/// <summary>
	/// Default error object result for api response.
	/// </summary>
	public ErrorResponse ErrorResult => new() { Message = Message, ErrorId = ResponseToken };
}

