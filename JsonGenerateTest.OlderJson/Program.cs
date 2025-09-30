using System.Text.Json;
using System.Text.Json.Serialization;
using JsonGenerateTest.OlderJson;

Console.WriteLine("JSON Serialization Test - .NET 8 with System.Text.Json 6.0.0");
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
try
{
    var sourceGenJson = JsonSerializer.Serialize(testModel, sourceGenOptions);
    Console.WriteLine(sourceGenJson);
    Console.WriteLine();

    // Compare the results
    Console.WriteLine("=== Comparison Analysis ===");
    Console.WriteLine($"Reflection JSON length: {reflectionJson.Length}");
    Console.WriteLine($"Source Generator JSON length: {sourceGenJson.Length}");
    Console.WriteLine($"Reflection and Source Generator produce same result: {reflectionJson.Equals(sourceGenJson)}");
    
    if (!reflectionJson.Equals(sourceGenJson))
    {
        Console.WriteLine();
        Console.WriteLine("🚨 BEHAVIORAL DIFFERENCE DETECTED in System.Text.Json 6.0.0!");
        Console.WriteLine("   This matches the issue described in the blog post.");
        Console.WriteLine($"   Reflection result: {reflectionJson.Replace("\n", "").Replace(" ", "")}");
        Console.WriteLine($"   Source Gen result: {sourceGenJson.Replace("\n", "").Replace(" ", "")}");
    }
    else
    {
        Console.WriteLine("✅ No behavioral difference detected even in System.Text.Json 6.0.0.");
    }
}
catch (Exception ex)
{
    Console.WriteLine($"❌ Source Generator failed: {ex.Message}");
    Console.WriteLine("   This might be because Source Generation wasn't fully supported in this version.");
}
