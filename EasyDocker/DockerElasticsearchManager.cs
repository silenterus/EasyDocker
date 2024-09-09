using Docker.DotNet;
using Docker.DotNet.Models;
using System;
using System.Collections.Generic;
using System.IO;
using System.Threading.Tasks;

namespace EasyDocker
{
    public class DockerElasticsearchManager
    {
        private readonly DockerClient _client;

        public DockerElasticsearchManager()
        {
            _client = new DockerClientConfiguration(new Uri("npipe://./pipe/docker_engine")).CreateClient(); // Use npipe for Windows
        }

        public async Task AddDockerCompose()
        {
            try
            {
            
                await StartElasticsearchAsync();

                if (false)
                {
                    // Step 2: Create another container, such as Kibana
                    await _client.Images.CreateImageAsync(new ImagesCreateParameters
                    {
                        FromImage = "docker.elastic.co/kibana/kibana",
                        Tag = "7.17.0"
                    }, null, new Progress<JSONMessage>());

                    await _client.Containers.CreateContainerAsync(new CreateContainerParameters
                    {
                        Image = "docker.elastic.co/kibana/kibana:7.17.0",
                        Name = "kibana",
                        Env = new[] { "ELASTICSEARCH_URL=http://elasticsearch:9200" }, // Link to Elasticsearch
                        HostConfig = new HostConfig
                        {
                            PortBindings = new Dictionary<string, IList<PortBinding>>
                            {
                                { "5601/tcp", new List<PortBinding> { new PortBinding { HostPort = "5601" } } },
                            },
                        },
                    });

                    await _client.Containers.StartContainerAsync("kibana", null);
                    Console.WriteLine("Kibana container started.");
                }
            

            }
            catch (Exception ex)
            {
                Console.WriteLine($"An error occurred while setting up the services: {ex.Message}");
            }
        }

        public async Task StartElasticsearchAsync()
        {
            /*// Pull the Elasticsearch image
            await _client.Images.CreateImageAsync(new ImagesCreateParameters
            {
                FromImage = "docker.elastic.co/elasticsearch/elasticsearch",
                Tag = "7.17.0"
            }, null, new Progress<JSONMessage>());

            // Create and start the container
            await _client.Containers.CreateContainerAsync(new CreateContainerParameters
            {
                Image = "docker.elastic.co/elasticsearch/elasticsearch:7.17.0",
                Name = "elasticsearch",
                Env = new[] { "discovery.type=single-node" },
                HostConfig = new HostConfig
                {
                    PortBindings = new Dictionary<string, IList<PortBinding>>
                    {
                        { "9200/tcp", new List<PortBinding> { new PortBinding { HostPort = "9200" } } },
                    },
                },
            });*/

            await _client.Containers.StartContainerAsync("elasticsearch", null);
            Console.WriteLine("Elasticsearch container started.");
        }

        public async Task StopElasticsearchAsync()
        {
            await _client.Containers.StopContainerAsync("elasticsearch", new ContainerStopParameters());
            Console.WriteLine("Elasticsearch container stopped.");
        }
    }
}
