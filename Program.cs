using System;
using Newtonsoft.Json;

var person = new { Name = "Alice", Age = 30, City = "Tokyo" };
var json = JsonConvert.SerializeObject(person, Formatting.Indented);
Console.WriteLine($"Serialized JSON:\n{json}");
