namespace EasyDocker;

class Program
{
    static DockerElasticsearchManager _dockerElasticManager;

    static async Task Main(string[] args)
    {
        _dockerElasticManager = new DockerElasticsearchManager();

        // Start Elasticsearch when the application starts
        await _dockerElasticManager.StartElasticsearchAsync();

        Console.WriteLine("Application is running. Press Enter to stop...");
        Console.ReadLine();

        // Stop Elasticsearch when the application ends
        await _dockerElasticManager.StopElasticsearchAsync();
    }
}
