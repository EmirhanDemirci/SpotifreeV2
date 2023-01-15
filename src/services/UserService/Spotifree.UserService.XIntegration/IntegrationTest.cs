using Microsoft.AspNetCore.Hosting;
using Microsoft.AspNetCore.TestHost;
using Microsoft.Extensions.Configuration;
using Newtonsoft.Json;
using Moq;
using Spotifree.UserService.Models;
using System.Reflection;
using System.Text;
using JsonSerializer = System.Text.Json.JsonSerializer;
using Spotifree.UserService.Services;
using Spotifree.UserService.Services.Interfaces;

namespace Spotifree.UserService.XIntegration
{
    public class IntegrationTest
    {
        private readonly TestServer _server;
        private readonly HttpClient _client;

        public IntegrationTest()
        {
            // Arrange
            Environment.SetEnvironmentVariable("SPOTIFREE_USERSERVICE_DB", "Server=tcp:fdemirci.tplinkdns.com,1433;Initial Catalog=UserDatabase;Persist Security Info=False;User ID=spotifree;Password=543532;MultipleActiveResultSets=False;Encrypt=False;TrustServerCertificate=False;Connection Timeout=30;");
            Environment.SetEnvironmentVariable("SPOTIFREE_RABBITMQ", "{\"UserName\": \"guest\",\"Password\": \"guest\",\"HostName\": \"192.168.0.65\",\"VHost\": \"/\",\"Port\": 5672}");
            var projectDir = GetProjectPath("", typeof(UserService.Controllers.UserController).GetTypeInfo().Assembly);
            _server = new TestServer(new WebHostBuilder()
                .UseContentRoot(projectDir)
                .UseConfiguration(new ConfigurationBuilder()
                    .SetBasePath(projectDir)
                    .AddJsonFile("appsettings.json")
                    .Build())
                .UseStartup<UserService.Controllers.UserController>());
            _client = _server.CreateClient();
        }
        private static string GetProjectPath(string projectRelativePath, Assembly startupAssembly)
        {
            // Get name of the target project which we want to test
            var projectName = startupAssembly.GetName().Name;

            // Get currently executing test project path
            var applicationBasePath = System.AppContext.BaseDirectory;

            // Find the path to the target project
            var directoryInfo = new DirectoryInfo(applicationBasePath);
            do
            {
                directoryInfo = directoryInfo.Parent;

                var projectDirectoryInfo = new DirectoryInfo(Path.Combine(directoryInfo.FullName, projectRelativePath));
                if (projectDirectoryInfo.Exists)
                {
                    var projectFileInfo = new FileInfo(Path.Combine(projectDirectoryInfo.FullName, projectName, $"{projectName}.csproj"));
                    if (projectFileInfo.Exists)
                    {
                        return Path.Combine(projectDirectoryInfo.FullName, projectName);
                    }
                }
            }
            while (directoryInfo.Parent != null);

            throw new Exception($"Project root could not be located using the application root {applicationBasePath}.");
        }

        [Fact]
        public async Task Create_Valid_Account_Returns_Ok()
        {
            var userViewModel = new UserViewModel
            {
                Id = 0,
                Email = "IntegrationTest@hotmail.com",
                Password = "ThisIsATest1234",
                FirstName = "Integration",
                LastName = "Testing",
                Birthdate = "01/01/0001"
            };

            // Act
            var response = await _client.PostAsync("/user/create",
                new StringContent(JsonSerializer.Serialize(userViewModel), Encoding.UTF8, "application/json"));

            // Assert
            Assert.True(response.IsSuccessStatusCode);
        }

        [Fact]
        public async Task Create_Invalid_Account_Returns_BadRequest()
        {
            var userViewModel = new UserViewModel
            {
                Id = 0,
                Email = "IntegrationTest@hotmail.com",
                Password = "ThisIsATest1234",
                FirstName = null,
                LastName = null,
                Birthdate = "01/01/0001"
            };

            // Act
            var response = await _client.PostAsync("/user/create",
                new StringContent(JsonSerializer.Serialize(userViewModel), Encoding.UTF8, "application/json"));

            var responseString = await response.Content.ReadAsStringAsync();
            var message = JsonSerializer.Serialize(new { message = "Not all properties are filled" });

            // Assert
            Assert.Equal(message, responseString);
        }

        [Fact]
        public async Task Get_User_By_Valid_Id()
        {
            var message = new User
            {
                Id = 1,
                FirstName = "Emirhan",
                LastName = "Demirci",
                Birthdate = "01/01/0001"
            };

            // Act
            var response = await _client.GetAsync("/user/" + 1);
            response.EnsureSuccessStatusCode();

            var responseString = JsonConvert.DeserializeObject<User>(await response.Content.ReadAsStringAsync());

            // Assert
            Assert.Equal(message.FirstName, responseString.FirstName);
        }

        [Fact]
        public async Task Get_User_By_Invalid_Id()
        {
            // Act
            var response = await _client.GetAsync("/user/" + 0);

            var responseString = await response.Content.ReadAsStringAsync();

            // Assert
            Assert.Equal("", responseString);
        }
    }
}