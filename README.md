# JSON Serialization Test - .NET 9

This project tests the behavior of `JsonIgnoreCondition.WhenWritingNull` with `Nullable<T>` properties in .NET 9, specifically comparing reflection-based serialization vs Source Generator-based serialization.

## Background

A [blog post](https://neue.cc/2024/03/18_Claudia.html?=json) mentioned an issue where `JsonIgnoreCondition.WhenWritingNull` worked differently between reflection-based and Source Generator-based JSON serialization:

> 一つ引っ掛かったのが、JsonIgnoreCondition.WhenWritingNullが、通常(リフレクションベース)だとNullable<T>にも効いていたのですが、Source Generatorだと効かなくなってnullの時に無視してくれなくなったという挙動の差異がありました。

Translation: "One issue I encountered was that JsonIgnoreCondition.WhenWritingNull used to work with Nullable<T> in normal (reflection-based) serialization, but it stopped working with Source Generator and wouldn't ignore null values anymore, creating a behavioral difference."

## Test Results

### .NET 9 Findings

**Good news!** In .NET 9, this behavioral difference has been **resolved**. Both reflection-based and Source Generator-based serialization now behave consistently with `JsonIgnoreCondition.WhenWritingNull` for `Nullable<T>` properties.

### Test Output

The tests clearly demonstrate both scenarios:

**Without WhenWritingNull** (nulls ARE serialized):
```json
{
  "Name": "John Doe",
  "Age": null,
  "BirthDate": null,
  "IsActive": true,
  "Salary": null
}
```

**With WhenWritingNull** (nulls are excluded):
```json
{
  "Name": "John Doe",
  "IsActive": true
}
```

As you can see, both approaches produce identical output, correctly:
- **Including** null `Nullable<T>` properties when no ignore condition is set
- **Excluding** null `Nullable<T>` properties when `WhenWritingNull` is set

## Running the Tests

```bash
# Build the solution
dotnet build

# Run the console application to see the comparison
dotnet run --project JsonGenerateTest

# Run the unit tests
dotnet test
```

## Test Structure

- **JsonGenerateTest/Models.cs**: Contains test models with `Nullable<T>` properties
- **JsonGenerateTest/Program.cs**: Console application demonstrating the serialization behavior
- **JsonGenerateTest.Tests/UnitTest1.cs**: Comprehensive unit tests verifying the behavior

## Conclusion

The inconsistency between reflection-based and Source Generator-based JSON serialization regarding `JsonIgnoreCondition.WhenWritingNull` with `Nullable<T>` properties has been fixed in .NET 9. Developers can now use either approach with confidence that they will produce consistent results.