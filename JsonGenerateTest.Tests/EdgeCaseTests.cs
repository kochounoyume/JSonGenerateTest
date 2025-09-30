using System.Text.Json;
using System.Text.Json.Serialization;
using JsonGenerateTest;

namespace JsonGenerateTest.Tests;

public class EdgeCaseTests
{
    [Fact]
    public void TestWithEmptyStringVsNull_ShouldHandleDifferently()
    {
        // Arrange
        var modelWithEmptyString = new TestModel
        {
            Name = "", // Empty string, not null
            Age = null,
            BirthDate = null,
            IsActive = null,
            Salary = null
        };

        var reflectionOptions = new JsonSerializerOptions
        {
            WriteIndented = false,
            DefaultIgnoreCondition = JsonIgnoreCondition.WhenWritingNull
        };

        var sourceGenOptions = new JsonSerializerOptions
        {
            WriteIndented = false,
            DefaultIgnoreCondition = JsonIgnoreCondition.WhenWritingNull,
            TypeInfoResolver = TestJsonContext.Default
        };

        // Act
        var reflectionJson = JsonSerializer.Serialize(modelWithEmptyString, reflectionOptions);
        var sourceGenJson = JsonSerializer.Serialize(modelWithEmptyString, sourceGenOptions);

        // Assert
        Assert.Contains("\"Name\"", reflectionJson); // Empty string should be included
        Assert.Contains("\"Name\"", sourceGenJson);  // Empty string should be included
        Assert.DoesNotContain("\"Age\"", reflectionJson); // null should be ignored
        Assert.DoesNotContain("\"Age\"", sourceGenJson);  // null should be ignored
        Assert.Equal(reflectionJson, sourceGenJson);
    }

    [Fact]
    public void TestWithWhenWritingDefault_ShouldIgnoreNullsButIncludeExplicitValues()
    {
        // Arrange
        var model = new TestModel
        {
            Name = null,
            Age = 0, // Explicit value of 0 for nullable int (not default null)
            BirthDate = null,
            IsActive = false, // Explicit value of false for nullable bool (not default null)
            Salary = null
        };

        var reflectionOptions = new JsonSerializerOptions
        {
            WriteIndented = false,
            DefaultIgnoreCondition = JsonIgnoreCondition.WhenWritingDefault
        };

        var sourceGenOptions = new JsonSerializerOptions
        {
            WriteIndented = false,
            DefaultIgnoreCondition = JsonIgnoreCondition.WhenWritingDefault,
            TypeInfoResolver = TestJsonContext.Default
        };

        // Act
        var reflectionJson = JsonSerializer.Serialize(model, reflectionOptions);
        var sourceGenJson = JsonSerializer.Serialize(model, sourceGenOptions);

        // Assert
        Assert.DoesNotContain("\"Name\"", reflectionJson); // null should be ignored
        Assert.Contains("\"Age\"", reflectionJson);  // 0 is an explicit value, not default for nullable int
        Assert.DoesNotContain("\"BirthDate\"", reflectionJson); // null should be ignored
        Assert.Contains("\"IsActive\"", reflectionJson); // false is an explicit value, not default for nullable bool
        Assert.DoesNotContain("\"Salary\"", reflectionJson); // null should be ignored
        Assert.Equal(reflectionJson, sourceGenJson);
    }

    [Fact]
    public void TestMixedNullAndNonNullValues_ShouldHandleCorrectly()
    {
        // Arrange
        var model = new TestModel
        {
            Name = "Test User",
            Age = 30,
            BirthDate = null, // This should be ignored
            IsActive = true,
            Salary = null     // This should be ignored
        };

        var reflectionOptions = new JsonSerializerOptions
        {
            WriteIndented = false,
            DefaultIgnoreCondition = JsonIgnoreCondition.WhenWritingNull
        };

        var sourceGenOptions = new JsonSerializerOptions
        {
            WriteIndented = false,
            DefaultIgnoreCondition = JsonIgnoreCondition.WhenWritingNull,
            TypeInfoResolver = TestJsonContext.Default
        };

        // Act
        var reflectionJson = JsonSerializer.Serialize(model, reflectionOptions);
        var sourceGenJson = JsonSerializer.Serialize(model, sourceGenOptions);

        // Assert
        Assert.Contains("\"Name\"", reflectionJson);
        Assert.Contains("\"Age\"", reflectionJson);
        Assert.Contains("\"IsActive\"", reflectionJson);
        Assert.DoesNotContain("\"BirthDate\"", reflectionJson);
        Assert.DoesNotContain("\"Salary\"", reflectionJson);
        
        Assert.Contains("\"Name\"", sourceGenJson);
        Assert.Contains("\"Age\"", sourceGenJson);
        Assert.Contains("\"IsActive\"", sourceGenJson);
        Assert.DoesNotContain("\"BirthDate\"", sourceGenJson);
        Assert.DoesNotContain("\"Salary\"", sourceGenJson);
        
        Assert.Equal(reflectionJson, sourceGenJson);
    }

    [Fact]
    public void TestWithNever_ShouldIncludeAllProperties()
    {
        // Arrange
        var model = new TestModel
        {
            Name = null,
            Age = null,
            BirthDate = null,
            IsActive = null,
            Salary = null
        };

        var reflectionOptions = new JsonSerializerOptions
        {
            WriteIndented = false,
            DefaultIgnoreCondition = JsonIgnoreCondition.Never
        };

        var sourceGenOptions = new JsonSerializerOptions
        {
            WriteIndented = false,
            DefaultIgnoreCondition = JsonIgnoreCondition.Never,
            TypeInfoResolver = TestJsonContext.Default
        };

        // Act
        var reflectionJson = JsonSerializer.Serialize(model, reflectionOptions);
        var sourceGenJson = JsonSerializer.Serialize(model, sourceGenOptions);

        // Assert - All properties should be included, even null ones
        Assert.Contains("\"Name\"", reflectionJson);
        Assert.Contains("\"Age\"", reflectionJson);
        Assert.Contains("\"BirthDate\"", reflectionJson);
        Assert.Contains("\"IsActive\"", reflectionJson);
        Assert.Contains("\"Salary\"", reflectionJson);
        
        Assert.Contains("\"Name\"", sourceGenJson);
        Assert.Contains("\"Age\"", sourceGenJson);
        Assert.Contains("\"BirthDate\"", sourceGenJson);
        Assert.Contains("\"IsActive\"", sourceGenJson);
        Assert.Contains("\"Salary\"", sourceGenJson);
        
        Assert.Equal(reflectionJson, sourceGenJson);
    }
}