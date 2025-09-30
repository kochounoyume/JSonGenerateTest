# .NET Version Compatibility Testing Results

This document summarizes the testing results for `JsonIgnoreCondition.WhenWritingNull` behavior with `Nullable<T>` properties across different .NET versions and System.Text.Json package versions.

## Test Results Summary

| .NET Version | System.Text.Json Version | Reflection Behavior | Source Generator Behavior | Match? | Notes |
|--------------|--------------------------|-------------------|---------------------------|---------|-------|
| .NET 9.0     | Built-in (9.x)          | Ignores null properties | Ignores null properties | ✅ Yes | Consistent behavior |
| .NET 8.0     | Built-in (8.x)          | Ignores null properties | Ignores null properties | ✅ Yes | Consistent behavior |
| .NET 8.0     | Package 6.0.0           | Ignores null properties | Ignores null properties | ✅ Yes | Even older package works |

## Expected vs Actual Results

**Based on the blog post**, we expected to find:
- Reflection-based serialization: Correctly ignores null `Nullable<T>` properties
- Source Generator-based serialization: Does NOT ignore null `Nullable<T>` properties (showing them as `null` in JSON)

**What we actually found**:
- Both approaches consistently ignore null `Nullable<T>` properties across all tested versions

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

## Consistent Output Across All Tests

Both approaches produce identical JSON:
```json
{
  "Name": "John Doe",
  "IsActive": true
}
```

The null properties (`Age`, `BirthDate`, `Salary`) are correctly omitted in all cases.

## Possible Explanations

1. **Issue was already fixed**: The behavioral difference may have been fixed in versions available in our test environment
2. **Version-specific issue**: The issue might have occurred in specific intermediate versions not tested
3. **Configuration-specific**: The issue might require specific project configurations or package combinations
4. **Platform-specific**: The issue might have been specific to certain platforms or runtime conditions

## Conclusion

Based on our testing across multiple .NET versions and System.Text.Json package versions, **the behavioral difference described in the blog post does not appear to be reproducible** in the current available versions. Both reflection-based and Source Generator-based JSON serialization consistently respect `JsonIgnoreCondition.WhenWritingNull` for `Nullable<T>` properties.

This suggests that either:
- The issue has been resolved in the versions we tested
- The issue required specific conditions not present in our test setup
- The issue was reported and fixed between the blog post date and the current versions

## Recommendation

Developers using .NET 8.0 or later can confidently use either reflection-based or Source Generator-based JSON serialization with `JsonIgnoreCondition.WhenWritingNull` and expect consistent behavior with `Nullable<T>` properties.