# JSON Serialization Test - .NET 9

This project tests the behavior of `JsonIgnoreCondition.WhenWritingNull` with `Nullable<T>` properties in .NET 9, specifically comparing reflection-based serialization vs Source Generator-based serialization.

## Background

A [blog post](https://neue.cc/2024/03/18_Claudia.html?=json) mentioned an issue where `JsonIgnoreCondition.WhenWritingNull` worked differently between reflection-based and Source Generator-based JSON serialization:

> 一つ引っ掛かったのが、JsonIgnoreCondition.WhenWritingNullが、通常(リフレクションベース)だとNullable<T>にも効いていたのですが、Source Generatorだと効かなくなってnullの時に無視してくれなくなったという挙動の差異がありました。

Translation: "One issue I encountered was that JsonIgnoreCondition.WhenWritingNull used to work with Nullable<T> in normal (reflection-based) serialization, but it stopped working with Source Generator and wouldn't ignore null values anymore, creating a behavioral difference."

## Test Results

### .NET 9 Findings

**Good news!** In .NET 9, the behavioral difference has been **resolved**. Both reflection-based and Source Generator-based serialization now behave consistently with `JsonIgnoreCondition.WhenWritingNull` for `Nullable<T>` properties.

### .NET 8 Findings

**Also fixed!** In .NET 8, both approaches also work consistently.

### .NET 7 Findings - Issue Reproduced! ⚠️

**We successfully reproduced the issue!** In .NET 7, there IS a behavioral difference:

- **Reflection-based serialization**: Correctly ignores null `Nullable<T>` properties ✅
- **Source Generator-based serialization**: Does NOT ignore null `Nullable<T>` properties ❌

This confirms the issue mentioned in the blog post existed in .NET 7 and was fixed in .NET 8.

### Test Output

The tests clearly demonstrate both scenarios:

**In .NET 7** (issue exists):

Reflection with WhenWritingNull:
```json
{
  "Name": "John Doe",
  "IsActive": true
}
```

Source Generator with WhenWritingNull (INCORRECT):
```json
{
  "Name": "John Doe",
  "Age": null,
  "BirthDate": null,
  "IsActive": true,
  "Salary": null
}
```

**In .NET 8 and .NET 9** (issue fixed):

Both approaches with WhenWritingNull produce:
```json
{
  "Name": "John Doe",
  "IsActive": true
}
```

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

The inconsistency between reflection-based and Source Generator-based JSON serialization regarding `JsonIgnoreCondition.WhenWritingNull` with `Nullable<T>` properties:

- ❌ **DID exist in .NET 7** - Source Generator did not respect the setting
- ✅ **Was FIXED in .NET 8** - Both approaches now work consistently  
- ✅ **Remains fixed in .NET 9** - Consistent behavior continues

**Recommendation:**
- If using .NET 7, either upgrade to .NET 8+ or apply the workaround from the blog post
- If using .NET 8 or later, both serialization approaches work correctly