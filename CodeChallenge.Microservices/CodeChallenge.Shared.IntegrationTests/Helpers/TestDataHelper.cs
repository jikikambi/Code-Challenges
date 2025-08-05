using Newtonsoft.Json;

namespace CodeChallenge.Shared.IntegrationTests.Helpers;

public static class TestDataHelper
{
    public static T GetTestDataFromDisk<T>(string folder, string jsonFileName)
    {
        // Get the path to the bin folder where the JSON file is copied
        var binFolderPath = Path.GetDirectoryName(System.Reflection.Assembly.GetExecutingAssembly().Location);

        // Combine the bin folder path with the JSON file name
        var jsonFilePath = Path.Combine(binFolderPath!, "TestData", folder, jsonFileName);

        if (!File.Exists(jsonFilePath))
        {
            throw new FileNotFoundException($"File {jsonFilePath} not found, the file is needed for the unit test.");
        }

        // Read the JSON content from the file
        var jsonContent = File.ReadAllText(jsonFilePath);

        // Deserialize the JSON into your C# object
        return JsonConvert.DeserializeObject<T>(jsonContent) ?? throw new JsonException("Malformed JSON content");
    }
}