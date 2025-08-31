namespace HotelBookingSystem.Application.Common.Utils;

public static class CacheKeyBuilder
{
    public static string Build(string prefix, params object?[] parts)
    {
        var normalized = parts.Select(v => v switch
        {
            null => "null",
            string s => string.IsNullOrWhiteSpace(s) ? "null" : s.ToLowerInvariant(),
            DateTime dt => dt.ToString("O"),
            bool b => b ? "true" : "false",
            _ => v.ToString() ?? "null"
        });

        return $"{prefix}_{string.Join("_", normalized)}";
    }
}