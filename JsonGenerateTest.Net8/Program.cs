using System.Text.Json;
using System.Text.Json.Serialization;
using JsonGenerateTest.Net8;

Console.WriteLine("JSON Serialization Test - .NET 8");
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

// Additional analysis for .NET 8
Console.WriteLine();
Console.WriteLine("=== .NET 8 Specific Analysis ===");
if (!reflectionJson.Equals(sourceGenJson))
{
    Console.WriteLine("⚠️  BEHAVIORAL DIFFERENCE DETECTED:");
    Console.WriteLine("   Reflection-based and Source Generator-based serialization produce different results!");
    Console.WriteLine("   This confirms the issue mentioned in the blog post exists in .NET 8.");
}
else
{
    Console.WriteLine("✅ No behavioral difference detected in .NET 8.");
    Console.WriteLine("   Both approaches produce identical results.");
}

// Run detailed tests
DetailedTest.RunDetailedComparison();
