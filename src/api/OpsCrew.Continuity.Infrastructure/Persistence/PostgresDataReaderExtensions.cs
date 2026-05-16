using Npgsql;

namespace OpsCrew.Continuity.Infrastructure.Persistence;

internal static class PostgresDataReaderExtensions
{
    public static string GetRequiredString(this NpgsqlDataReader reader, string name)
    {
        return reader.GetString(reader.GetOrdinal(name));
    }

    public static string? GetNullableString(this NpgsqlDataReader reader, string name)
    {
        var ordinal = reader.GetOrdinal(name);
        return reader.IsDBNull(ordinal) ? null : reader.GetString(ordinal);
    }

    public static DateTime GetRequiredDateTime(this NpgsqlDataReader reader, string name)
    {
        return reader.GetDateTime(reader.GetOrdinal(name));
    }

    public static DateTime? GetNullableDateTime(this NpgsqlDataReader reader, string name)
    {
        var ordinal = reader.GetOrdinal(name);
        return reader.IsDBNull(ordinal) ? null : reader.GetDateTime(ordinal);
    }

    public static DateOnly GetRequiredDateOnly(this NpgsqlDataReader reader, string name)
    {
        return reader.GetFieldValue<DateOnly>(reader.GetOrdinal(name));
    }

    public static IReadOnlyList<string> GetStringArray(this NpgsqlDataReader reader, string name)
    {
        return reader.GetFieldValue<string[]>(reader.GetOrdinal(name));
    }
}
