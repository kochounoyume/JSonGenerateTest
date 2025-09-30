using System.Text.Json;
using System.Text.Json.Serialization;
using JsonGenerateTest;

Console.WriteLine("JSON Serialization Test - .NET 9");
Console.WriteLine("Testing JsonIgnoreCondition.WhenWritingNull behavior with Nullable<T> properties");
Console.WriteLine();

// Create test data with null values
var testModel = new TestModel
{
    Name = "John Doe",
    Age = null,        // This should be ignored with WhenWritingNull
    BirthDate = null,  // This should be ignored with WhenWritingNull
    IsActive = true,
    Salary = null      // This should be ignored with WhenWritingNull
};

var testModelWithDirectIgnore = new TestModelWithDirectIgnore
{
    Name = "Jane Doe",
    Age = null,        // This should be ignored with WhenWritingDefault
    BirthDate = null,  // This should be ignored with WhenWritingDefault
    IsActive = true,
    Salary = null      // This should be ignored with WhenWritingDefault
};

// Test reflection-based serialization with WhenWritingNull
var reflectionOptions = new JsonSerializerOptions
{
    WriteIndented = true,
    DefaultIgnoreCondition = JsonIgnoreCondition.WhenWritingNull
};

Console.WriteLine("=== Reflection-based Serialization (WhenWritingNull) ===");
var reflectionJson = JsonSerializer.Serialize(testModel, reflectionOptions);
Console.WriteLine(reflectionJson);
Console.WriteLine();

// Test source generator-based serialization with WhenWritingNull
var sourceGenOptions = new JsonSerializerOptions
{
    WriteIndented = true,
    DefaultIgnoreCondition = JsonIgnoreCondition.WhenWritingNull,
    TypeInfoResolver = TestJsonContext.Default
};

Console.WriteLine("=== Source Generator-based Serialization (WhenWritingNull) ===");
var sourceGenJson = JsonSerializer.Serialize(testModel, sourceGenOptions);
Console.WriteLine(sourceGenJson);
Console.WriteLine();

// Test source generator with direct JsonIgnore attributes
Console.WriteLine("=== Source Generator with Direct JsonIgnore Attributes ===");
var directIgnoreJson = JsonSerializer.Serialize(testModelWithDirectIgnore, sourceGenOptions);
Console.WriteLine(directIgnoreJson);
Console.WriteLine();

// Compare the results
Console.WriteLine("=== Comparison Analysis ===");
Console.WriteLine($"Reflection JSON length: {reflectionJson.Length}");
Console.WriteLine($"Source Generator JSON length: {sourceGenJson.Length}");
Console.WriteLine($"Direct Ignore JSON length: {directIgnoreJson.Length}");
Console.WriteLine($"Reflection and Source Generator produce same result: {reflectionJson.Equals(sourceGenJson)}");
Console.WriteLine($"Source Generator and Direct Ignore produce same result: {sourceGenJson.Equals(directIgnoreJson)}");
