namespace HelpDesk.AuthService.API.Common;

/// <summary>
/// Centralizes API route version prefixes so controllers reference a single constant
/// instead of repeating a hardcoded "api/v1" literal. Introducing a new version (e.g. V2)
/// only requires adding a constant here and applying it to the relevant controller(s).
/// </summary>
public static class ApiRoutes
{
    public const string V1 = "api/v1";
}
