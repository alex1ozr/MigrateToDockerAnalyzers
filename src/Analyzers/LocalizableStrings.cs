using Microsoft.CodeAnalysis;

namespace MigrateToDocker.Analyzers;

internal static class LocalizableStrings
{
    internal static class ToStringAnalyzer
    {
        public static readonly LocalizableString Title = new LocalizableResourceString(
            nameof(Resources.ToStringAnalyzer_Title),
            Resources.ResourceManager,
            typeof(Resources));

        public static readonly LocalizableString MessageFormat = new LocalizableResourceString(
            nameof(Resources.ToStringAnalyzer_MessageFormat),
            Resources.ResourceManager,
            typeof(Resources));

        public static readonly LocalizableString Description = new LocalizableResourceString(
            nameof(Resources.ToStringAnalyzer_Description),
            Resources.ResourceManager,
            typeof(Resources));
    }
}