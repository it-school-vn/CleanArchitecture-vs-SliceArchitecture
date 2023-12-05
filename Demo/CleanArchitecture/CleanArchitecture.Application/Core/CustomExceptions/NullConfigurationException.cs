using System.Diagnostics.CodeAnalysis;

namespace CleanArchitecture.Application.Core.CustomExceptions
{
    public sealed class NullConfigurationException : CustomApplicationException
    {
        public NullConfigurationException(string settingKey)
        : base("Null configuration", $"Not found or Empty configuration element {settingKey}")
        {

        }

        public static void ThrowIfNullOrEmpty([NotNull] string? argument, string settingKey)
        {
            if (string.IsNullOrEmpty(argument))
            {
                throw new NullConfigurationException(settingKey);
            }
        }

        public static void ThrowIfNullOrWhiteSpace([NotNull] string? argument, string settingKey)
        {
            if (string.IsNullOrWhiteSpace(argument))
            {
                throw new NullConfigurationException(settingKey);
            }
        }

        public static void ThrowIfNull(object? argument, string settingKey)
        {
            if (argument is null)
            {
                throw new NullConfigurationException(settingKey);
            }
        }
    }
}