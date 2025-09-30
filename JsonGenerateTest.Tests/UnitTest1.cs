using System.Text.Json;
using System.Text.Json.Serialization;
using JsonGenerateTest;

namespace JsonGenerateTest.Tests;

public class JsonIgnoreConditionTests
{
    private readonly TestModel _testModelWithNulls;
    private readonly TestModelWithDirectIgnore _testModelWithDirectIgnoreNulls;
    private readonly JsonSerializerOptions _reflectionOptions;
    private readonly JsonSerializerOptions _sourceGenOptions;

    public JsonIgnoreConditionTests()
    {
        // Test data with null values for nullable properties
        _testModelWithNulls = new TestModel
        {
            Name = "Test User",
            Age = null,
            BirthDate = null,
            IsActive = true,
            Salary = null
        };

        _testModelWithDirectIgnoreNulls = new TestModelWithDirectIgnore
        {
            Name = "Test User",
            Age = null,
            BirthDate = null,
            IsActive = true,
            Salary = null
        };

        _reflectionOptions = new JsonSerializerOptions
        {
            WriteIndented = false,
            DefaultIgnoreCondition = JsonIgnoreCondition.WhenWritingNull
        };

        _sourceGenOptions = new JsonSerializerOptions
        {
            WriteIndented = false,
            DefaultIgnoreCondition = JsonIgnoreCondition.WhenWritingNull,
            TypeInfoResolver = TestJsonContext.Default
        };
    }

    [Fact]
    public void ReflectionSerialization_WhenWritingNull_ShouldIgnoreNullableProperties()
    {
        // Act
        var json = JsonSerializer.Serialize(_testModelWithNulls, _reflectionOptions);

        // Assert
        Assert.DoesNotContain("\"Age\"", json);
        Assert.DoesNotContain("\"BirthDate\"", json);
        Assert.DoesNotContain("\"Salary\"", json);
        Assert.Contains("\"Name\"", json);
        Assert.Contains("\"IsActive\"", json);
    }

    [Fact]
    public void SourceGeneratorSerialization_WhenWritingNull_ShouldIgnoreNullableProperties()
    {
        // Act
        var json = JsonSerializer.Serialize(_testModelWithNulls, _sourceGenOptions);

        // Assert - This test will reveal if Source Generator behaves differently
        Assert.DoesNotContain("\"Age\"", json);
        Assert.DoesNotContain("\"BirthDate\"", json);
        Assert.DoesNotContain("\"Salary\"", json);
        Assert.Contains("\"Name\"", json);
        Assert.Contains("\"IsActive\"", json);
    }

    [Fact]
    public void DirectIgnoreAttributes_ShouldAlwaysWork()
    {
        // Act
        var json = JsonSerializer.Serialize(_testModelWithDirectIgnoreNulls, _sourceGenOptions);

        // Assert
        Assert.DoesNotContain("\"Age\"", json);
        Assert.DoesNotContain("\"BirthDate\"", json);
        Assert.DoesNotContain("\"Salary\"", json);
        Assert.Contains("\"Name\"", json);
        Assert.Contains("\"IsActive\"", json);
    }

    [Fact]
    public void CompareReflectionAndSourceGenerator_ShouldProduceSameResult()
    {
        // Act
        var reflectionJson = JsonSerializer.Serialize(_testModelWithNulls, _reflectionOptions);
        var sourceGenJson = JsonSerializer.Serialize(_testModelWithNulls, _sourceGenOptions);

        // Assert - This will fail if there's a behavior difference
        Assert.Equal(reflectionJson, sourceGenJson);
    }

    [Fact]
    public void TestWithNonNullValues_ShouldIncludeAllProperties()
    {
        // Arrange
        var testModelWithValues = new TestModel
        {
            Name = "Test User",
            Age = 25,
            BirthDate = new DateTime(1999, 1, 1),
            IsActive = true,
            Salary = 50000m
        };

        // Act
        var reflectionJson = JsonSerializer.Serialize(testModelWithValues, _reflectionOptions);
        var sourceGenJson = JsonSerializer.Serialize(testModelWithValues, _sourceGenOptions);

        // Assert
        Assert.Contains("\"Age\"", reflectionJson);
        Assert.Contains("\"BirthDate\"", reflectionJson);
        Assert.Contains("\"Salary\"", reflectionJson);
        
        Assert.Contains("\"Age\"", sourceGenJson);
        Assert.Contains("\"BirthDate\"", sourceGenJson);
        Assert.Contains("\"Salary\"", sourceGenJson);
        
        Assert.Equal(reflectionJson, sourceGenJson);
    }

    [Fact]
    public void GetRawJsonStrings_ForManualInspection()
    {
        // This test is for manual inspection of the actual JSON output
        var reflectionJson = JsonSerializer.Serialize(_testModelWithNulls, _reflectionOptions);
        var sourceGenJson = JsonSerializer.Serialize(_testModelWithNulls, _sourceGenOptions);
        var directIgnoreJson = JsonSerializer.Serialize(_testModelWithDirectIgnoreNulls, _sourceGenOptions);

        // Output for manual verification
        System.Diagnostics.Debug.WriteLine($"Reflection JSON: {reflectionJson}");
        System.Diagnostics.Debug.WriteLine($"Source Generator JSON: {sourceGenJson}");
        System.Diagnostics.Debug.WriteLine($"Direct Ignore JSON: {directIgnoreJson}");

        // This test will always pass, but provides output for inspection
        Assert.True(true);
    }
}
