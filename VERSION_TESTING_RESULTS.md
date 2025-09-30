# .NET Version Compatibility Testing Results

This document summarizes the testing results for `JsonIgnoreCondition.WhenWritingNull` behavior with `Nullable<T>` properties across different .NET versions and System.Text.Json package versions.

## Test Results Summary

| .NET Version | System.Text.Json Version | Reflection Behavior | Source Generator Behavior | Match? | Notes |
|--------------|--------------------------|-------------------|---------------------------|---------|-------|
| .NET 9.0     | Built-in (9.x)          | Ignores null properties | Ignores null properties | ✅ Yes | Consistent behavior |
| .NET 8.0     | Built-in (8.x)          | Ignores null properties | Ignores null properties | ✅ Yes | Consistent behavior |
| .NET 8.0     | Package 6.0.0           | Ignores null properties | Ignores null properties | ✅ Yes | Even older package works |
| .NET 7.0     | Built-in (7.x)          | Ignores null properties | **DOES NOT ignore null properties** | ❌ No | **Issue reproduced!** |

## Expected vs Actual Results

**Based on the blog post**, we expected to find:
- Reflection-based serialization: Correctly ignores null `Nullable<T>` properties
- Source Generator-based serialization: Does NOT ignore null `Nullable<T>` properties (showing them as `null` in JSON)

**What we actually found**:
- ✅ **In .NET 7**: The issue EXISTS! Source Generator does not respect `WhenWritingNull` for `Nullable<T>` properties
- ✅ **In .NET 8+**: The issue has been FIXED! Both approaches work consistently

## Test Configuration

Each test used the following setup:
```csharp
var testModel = new TestModel
{
    Name = "John Doe",     // Non-null value - should always appear
    Age = null,            // Nullable<int> with null value
    BirthDate = null,      // Nullable<DateTime> with null value  
    IsActive = true,       // Nullable<bool> with non-null value
    Salary = null          // Nullable<decimal> with null value
};

// Reflection options
var reflectionOptions = new JsonSerializerOptions
{
    DefaultIgnoreCondition = JsonIgnoreCondition.WhenWritingNull
};

// Source Generator options
var sourceGenOptions = new JsonSerializerOptions
{
    DefaultIgnoreCondition = JsonIgnoreCondition.WhenWritingNull,
    TypeInfoResolver = TestJsonContext.Default
};
```

## Output Comparison

### .NET 7.0 - ISSUE REPRODUCED! ⚠️

**Without WhenWritingNull** (baseline showing nulls ARE serialized):

Both approaches produce:
```json
{
  "Name": "John Doe",
  "Age": null,
  "BirthDate": null,
  "IsActive": true,
  "Salary": null
}
```

**With WhenWritingNull** (testing if nulls are ignored):

Reflection produces:
```json
{
  "Name": "John Doe",
  "IsActive": true
}
```

Source Generator produces (INCORRECT):
```json
{
  "Name": "John Doe",
  "Age": null,
  "BirthDate": null,
  "IsActive": true,
  "Salary": null
}
```

**The Source Generator in .NET 7 does NOT respect `WhenWritingNull` for `Nullable<T>` properties!**

### .NET 8.0 and .NET 9.0 - ISSUE FIXED! ✅

**Without WhenWritingNull** (baseline showing nulls ARE serialized):

Both approaches produce:
```json
{
  "Name": "John Doe",
  "Age": null,
  "BirthDate": null,
  "IsActive": true,
  "Salary": null
}
```

**With WhenWritingNull** (testing if nulls are excluded):

Both approaches produce:
```json
{
  "Name": "John Doe",
  "IsActive": true
}
```

The null properties (`Age`, `BirthDate`, `Salary`) are correctly omitted in all cases.

## Possible Explanations

The testing confirms that:

1. **Issue existed in .NET 7**: The behavioral difference described in the blog post was real and affected .NET 7
2. **Issue fixed in .NET 8**: Microsoft fixed this issue in .NET 8, where both reflection and Source Generator now respect `WhenWritingNull` for `Nullable<T>` properties
3. **Remains fixed in .NET 9**: The fix continues to work properly in .NET 9

## Conclusion

Based on our testing across multiple .NET versions and System.Text.Json package versions:

**The behavioral difference described in the blog post DID exist in .NET 7**, where Source Generator-based JSON serialization did not respect `JsonIgnoreCondition.WhenWritingNull` for `Nullable<T>` properties, while reflection-based serialization did.

**This issue has been fixed in .NET 8 and later versions.** Both reflection-based and Source Generator-based JSON serialization now consistently respect `JsonIgnoreCondition.WhenWritingNull` for `Nullable<T>` properties.

## Recommendation

- **For .NET 7 users**: Either upgrade to .NET 8+ or use the workaround mentioned in the blog post (adding `[JsonIgnore(Condition = JsonIgnoreCondition.WhenWritingDefault)]` directly to each `Nullable<T>` property)
- **For .NET 8+ users**: You can confidently use either reflection-based or Source Generator-based JSON serialization with `JsonIgnoreCondition.WhenWritingNull` and expect consistent behavior with `Nullable<T>` properties