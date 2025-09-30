using System.Text.Json;
using System.Text.Json.Serialization;

namespace JsonGenerateTest.Net7;

// Model with JsonIgnoreCondition.WhenWritingNull at the class level
[JsonSerializable(typeof(TestModel))]
[JsonSerializable(typeof(TestModelWithDirectIgnore))]
public partial class TestJsonContext : JsonSerializerContext
{
}

// Model using global JsonIgnoreCondition.WhenWritingNull
public class TestModel
{
    public string? Name { get; set; }
    public int? Age { get; set; }
    public DateTime? BirthDate { get; set; }
    public bool? IsActive { get; set; }
    public decimal? Salary { get; set; }
}

// Model with direct [JsonIgnore(Condition = JsonIgnoreCondition.WhenWritingDefault)] on nullable properties
public class TestModelWithDirectIgnore
{
    public string? Name { get; set; }
    
    [JsonIgnore(Condition = JsonIgnoreCondition.WhenWritingDefault)]
    public int? Age { get; set; }
    
    [JsonIgnore(Condition = JsonIgnoreCondition.WhenWritingDefault)]
    public DateTime? BirthDate { get; set; }
    
    [JsonIgnore(Condition = JsonIgnoreCondition.WhenWritingDefault)]
    public bool? IsActive { get; set; }
    
    [JsonIgnore(Condition = JsonIgnoreCondition.WhenWritingDefault)]
    public decimal? Salary { get; set; }
}