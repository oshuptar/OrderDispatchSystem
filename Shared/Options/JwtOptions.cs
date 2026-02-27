namespace Auth.Options;

public class JwtOptions
{
    public const String SectionName = "Jwt";
    public required String  Issuer { get; init; }
    public required String Audience { get; init; }
    public required String Key { get; init; }
}