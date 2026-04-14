namespace WhatsOn.Service.Common.Configuration;

public class CorsPolicySettings
{
	public const string SectionName = "CorsPolicySettings";
	public string[] AllowedOrigins { get; set; } = [];

}
