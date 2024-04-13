using FooRestaurantApi.APILayer;
using FooRestaurantApi.ServicesLayer.DTO.User;
using Microsoft.AspNetCore.Hosting;
using Microsoft.AspNetCore.TestHost;
using System;
using System.Net.Http;
using System.Text.Json;
using System.Threading.Tasks;
using Xunit;

namespace FooRestaurantApi.Test
{
    public class Tests
    {
        const string _baseUrl = "http://localhost:51493";

        public HttpClient Client { get; }

        public Tests()
        {
            var builder = new WebHostBuilder().UseStartup<Startup>();
            var server = new TestServer(builder);
            Client = server.CreateClient();


        }
        [Fact]
        public async Task LoginReturnsOkStatusCodeAfterSignup()
        {            // SigmUp işlemi

            var signUpRequest = new HttpRequestMessage(HttpMethod.Post, $"{_baseUrl}/api/user/signup");
            Random rnd = new Random();
            int userName = rnd.Next(1, 999999);
            var signUpRequestDto= new SignUpRequestDto() { Address="asd",Password="asds",Phone="asd",UserName= userName.ToString() };
          var SignUpRequestBody =  JsonSerializer.Serialize(signUpRequestDto);

            signUpRequest.Content = new StringContent(SignUpRequestBody, System.Text.Encoding.UTF8, "application/json");

      
            var response = await Client.SendAsync(signUpRequest);

    

            Assert.Equal(System.Net.HttpStatusCode.Created, response.StatusCode);


            // Login işlemi
            var loginRequestDto = new LoginRequestDto()
            {
                UserName = signUpRequestDto.UserName,
                Password = signUpRequestDto.Password
            };
            var loginRequestBody = JsonSerializer.Serialize(loginRequestDto);
            var loginRequest = new HttpRequestMessage(HttpMethod.Post, $"{_baseUrl}/api/login");
            loginRequest.Content = new StringContent(loginRequestBody, System.Text.Encoding.UTF8, "application/json");

       
            var loginResponse = await Client.SendAsync(loginRequest);

 
            Assert.Equal(System.Net.HttpStatusCode.OK, loginResponse.StatusCode);

        }
    }
}
