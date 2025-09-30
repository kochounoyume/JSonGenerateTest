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

// ===================================================================
// FIRST: Show that nulls ARE serialized without WhenWritingNull
// ===================================================================
Console.WriteLine("=== WITHOUT WhenWritingNull (showing nulls are normally serialized) ===");
Console.WriteLine();

var noIgnoreOptions = new JsonSerializerOptions
{
    WriteIndented = true
};

Console.WriteLine("Reflection (No Ignore Condition):");
var reflectionNoIgnore = JsonSerializer.Serialize(testModel, noIgnoreOptions);
Console.WriteLine(reflectionNoIgnore);
Console.WriteLine();

var sourceGenNoIgnoreOptions = new JsonSerializerOptions
{
    WriteIndented = true,
    TypeInfoResolver = TestJsonContext.Default
};

Console.WriteLine("Source Generator (No Ignore Condition):");
var sourceGenNoIgnore = JsonSerializer.Serialize(testModel, sourceGenNoIgnoreOptions);
Console.WriteLine(sourceGenNoIgnore);
Console.WriteLine();

// ===================================================================
// SECOND: Show that nulls are IGNORED with WhenWritingNull
// ===================================================================
Console.WriteLine("=== WITH WhenWritingNull (testing if nulls are ignored) ===");
Console.WriteLine();

// Test reflection-based serialization with WhenWritingNull
var reflectionOptions = new JsonSerializerOptions
{
    WriteIndented = true,
    DefaultIgnoreCondition = JsonIgnoreCondition.WhenWritingNull
};

Console.WriteLine("Reflection (WhenWritingNull):");
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

Console.WriteLine("Source Generator (WhenWritingNull):");
var sourceGenJson = JsonSerializer.Serialize(testModel, sourceGenOptions);
Console.WriteLine(sourceGenJson);
Console.WriteLine();

// ===================================================================
// ANALYSIS
// ===================================================================
Console.WriteLine("=== ANALYSIS ===");
Console.WriteLine();

Console.WriteLine("Without WhenWritingNull:");
Console.WriteLine($"  Reflection includes nulls: {reflectionNoIgnore.Contains("null")}");
Console.WriteLine($"  Source Generator includes nulls: {sourceGenNoIgnore.Contains("null")}");
Console.WriteLine($"  Both behave the same: {reflectionNoIgnore.Replace("\r", "").Replace("\n", "").Replace(" ", "") == sourceGenNoIgnore.Replace("\r", "").Replace("\n", "").Replace(" ", "")}");
Console.WriteLine();

Console.WriteLine("With WhenWritingNull:");
Console.WriteLine($"  Reflection excludes nulls: {!reflectionJson.Contains("null") && !reflectionJson.Contains("Age") && !reflectionJson.Contains("Salary")}");
Console.WriteLine($"  Source Generator excludes nulls: {!sourceGenJson.Contains("null") && !sourceGenJson.Contains("Age") && !sourceGenJson.Contains("Salary")}");
Console.WriteLine($"  Both behave the same: {reflectionJson.Replace("\r", "").Replace("\n", "").Replace(" ", "") == sourceGenJson.Replace("\r", "").Replace("\n", "").Replace(" ", "")}");
Console.WriteLine();

Console.WriteLine("CONCLUSION:");
if (reflectionJson.Replace("\r", "").Replace("\n", "").Replace(" ", "") == sourceGenJson.Replace("\r", "").Replace("\n", "").Replace(" ", ""))
{
    Console.WriteLine("✅ No behavioral difference in .NET 8!");
    Console.WriteLine("   Both reflection and Source Generator respect WhenWritingNull for Nullable<T> properties.");
}
else
{
    Console.WriteLine("⚠️  BEHAVIORAL DIFFERENCE DETECTED!");
    Console.WriteLine("   This matches the issue described in the blog post.");
}

// Run additional detailed tests
DetailedTest.RunDetailedComparison();
