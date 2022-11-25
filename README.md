# Analyzers for C# .NET Compiler Platform
Analyzers that can help you to migrate your existing .NET projects to Docker environment.
Different deployment environments may contain different culture settings,
so it is recommended to pass concrete format / culture to all ToString() / Parse() methods
for primitive types at least.

### Analyzers list
#### Parameterless ToString() method usage for culture-dependent types (codefixes included)
- DateTime
- DateTimeOffset
- Decimal
- Double
- Float

#### Parse() method usage without explicit format or culture for culture-dependent types
- DateTime
- DateTimeOffset
- Decimal
- Double
- Float
