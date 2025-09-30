using System.Text.Json;
using System.Text.Json.Serialization;
using JsonGenerateTest.Net8;

public static class DetailedTest
{
    public static void RunDetailedComparison()
    {
        Console.WriteLine("\n=== DETAILED COMPARISON - .NET 8 ===");
        
        var testModel = new TestModel
        {
            Name = "Test User",
            Age = null,
            BirthDate = null,
            IsActive = true,
            Salary = null
        };

        // Test 1: Reflection with WhenWritingNull
        var reflectionOptions = new JsonSerializerOptions
        {
            WriteIndented = false,
            DefaultIgnoreCondition = JsonIgnoreCondition.WhenWritingNull
        };
        var reflectionJson = JsonSerializer.Serialize(testModel, reflectionOptions);
        Console.WriteLine($"Reflection (WhenWritingNull): {reflectionJson}");

        // Test 2: Source Generator with WhenWritingNull
        var sourceGenOptions = new JsonSerializerOptions
        {
            WriteIndented = false,
            DefaultIgnoreCondition = JsonIgnoreCondition.WhenWritingNull,
            TypeInfoResolver = TestJsonContext.Default
        };
        var sourceGenJson = JsonSerializer.Serialize(testModel, sourceGenOptions);
        Console.WriteLine($"Source Gen (WhenWritingNull):  {sourceGenJson}");

        // Test 3: Source Generator with NO global settings
        var sourceGenNoGlobalOptions = new JsonSerializerOptions
        {
            WriteIndented = false,
            TypeInfoResolver = TestJsonContext.Default
        };
        var sourceGenNoGlobalJson = JsonSerializer.Serialize(testModel, sourceGenNoGlobalOptions);
        Console.WriteLine($"Source Gen (No Global):       {sourceGenNoGlobalJson}");

        // Test 4: Reflection with NO global settings
        var reflectionNoGlobalOptions = new JsonSerializerOptions
        {
            WriteIndented = false
        };
        var reflectionNoGlobalJson = JsonSerializer.Serialize(testModel, reflectionNoGlobalOptions);
        Console.WriteLine($"Reflection (No Global):       {reflectionNoGlobalJson}");

        // Test 5: Source Generator with explicit type
        var sourceGenExplicitJson = JsonSerializer.Serialize(testModel, typeof(TestModel), sourceGenOptions);
        Console.WriteLine($"Source Gen (Explicit Type):   {sourceGenExplicitJson}");

        // Analysis
        Console.WriteLine("\n--- Analysis ---");
        Console.WriteLine($"Reflection == Source Gen (WhenWritingNull): {reflectionJson == sourceGenJson}");
        Console.WriteLine($"Source Gen (Global) == Source Gen (No Global): {sourceGenJson == sourceGenNoGlobalJson}");
        Console.WriteLine($"Reflection (Global) == Reflection (No Global): {reflectionJson == reflectionNoGlobalJson}");
        
        if (reflectionJson != sourceGenJson)
        {
            Console.WriteLine("🚨 BEHAVIORAL DIFFERENCE FOUND!");
            Console.WriteLine($"   Reflection includes null properties: {CountNullProperties(reflectionJson)}");
            Console.WriteLine($"   Source Gen includes null properties: {CountNullProperties(sourceGenJson)}");
        }
        else
        {
            Console.WriteLine("✅ Both approaches behave identically.");
        }
    }

    private static bool CountNullProperties(string json)
    {
        return json.Contains("null") || json.Contains("\"Age\"") || json.Contains("\"BirthDate\"") || json.Contains("\"Salary\"");
    }
}