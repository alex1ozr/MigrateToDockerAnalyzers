namespace MigrateToDocker.Analyzers.Test;

public static class TestData
{
    public static IEnumerable<object[]> DecimalType
    {
        get
        {
            yield return new object[] { "decimal" };
            yield return new object[] { "Decimal" };
            yield return new object[] { "System.Decimal" };
        }
    }
    
    public static IEnumerable<object[]> DoubleType
    {
        get
        {
            yield return new object[] { "double" };
            yield return new object[] { "Double" };
            yield return new object[] { "System.Double" };
        }
    }
    
    public static IEnumerable<object[]> FloatType
    {
        get
        {
            yield return new object[] { "float" };
            yield return new object[] { "Single" };
            yield return new object[] { "System.Single" };
        }
    }
}