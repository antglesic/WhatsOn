namespace WhatsOn.Domain
{
	public record PagedResult<T> where T : class
	{
		public IEnumerable<T> Data { get; init; } = [];
		public int PageNumber { get; init; }
		public int PageSize { get; init; }
		public int TotalPages { get; init; }
		public int TotalItemCount { get; init; }
	}
}
