using ABABillingAndClaim.Models;
using Newtonsoft.Json;
using Newtonsoft.Json.Linq;
using RestSharp;
using RestSharp.Extensions;
using System;
using System.Collections.Generic;
using System.IdentityModel.Tokens;
using System.Linq;
using System.Net.Http;
using System.Security.Policy;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;

namespace ABABillingAndClaim.Services
{
    public class AuthService
    {
        private MemoryService _memoryService;

        public AuthService(MemoryService memoryService)
        {
            _memoryService = memoryService;
        }

        public UserManagerResponse Login(LoginViewModel user)
        {
            var client = new RestClient(_memoryService.BaseEndPoint + "/auth/login");
            client.Timeout = -1;
            var request = new RestRequest(Method.POST);
            request.AddHeader("Content-Type", "application/json");
            var body = JsonConvert.SerializeObject(user);
            request.AddParameter("application/json", body, ParameterType.RequestBody);
            IRestResponse response = client.Execute(request);

            if ((int)response.StatusCode == 200)
            {
                var userMnt = JsonConvert.DeserializeObject<UserManagerResponse>(response.Content);
                _memoryService.Token = userMnt.Message;
                _memoryService.Connected = true;

                var handler = new JwtSecurityTokenHandler();
                var jsonToken = handler.ReadToken(userMnt.Message);
                var tokens = jsonToken as JwtSecurityToken;
                User _user = new User
                {
                    username = user.Username,
                    roles = tokens.Claims
                    .Where(c => c.Type.Contains("role"))
                    .Select(x => x.Value)
                    .ToList(),
                    email = tokens.Claims.First(c => c.Type == "Email").Value,
                    id = tokens.Claims.First(c => c.Type.Contains("nameidentifier")).Value,
                };
                this._memoryService.LoggedOndUser = _user;

                return userMnt;
            }
            else if ((int)response.StatusCode == 401)
                return JsonConvert.DeserializeObject<UserManagerResponse>(response.Content);
            else
            {
                return new UserManagerResponse
                {
                    Message = "Some properties are not valid",
                    IsSuccess = false
                };
            }

        }

        public UserManagerResponse ChangePassword(ResetPasswordViewModel reset)
        {
            var client = new RestClient(_memoryService.BaseEndPoint + "/auth/resetpassword");
            client.Timeout = -1;
            var request = new RestRequest(Method.POST);
            request.AddHeader("Authorization", $"Bearer {_memoryService.Token}");
            request.AddHeader("Content-Type", "application/json");
            var body = JsonConvert.SerializeObject(reset);
            request.AddParameter("application/json", body, ParameterType.RequestBody);
            IRestResponse response = client.Execute(request);

            if ((int)response.StatusCode == 200)
            {
                return JsonConvert.DeserializeObject<UserManagerResponse>(response.Content);
            }
            else if ((int)response.StatusCode == 409)
                return JsonConvert.DeserializeObject<UserManagerResponse>(response.Content);
            else
            {
                return new UserManagerResponse
                {
                    Message = "Some properties are not valid",
                    IsSuccess = false
                };
            }
        }

        public IEnumerable<User> GetAllUsers()
        {
            var client = new RestClient(_memoryService.BaseEndPoint + "/auth");
            client.Timeout = -1;
            var request = new RestRequest(Method.GET);
            request.AddHeader("Authorization", $"Bearer {_memoryService.Token}");
            request.AddHeader("Content-Type", "application/json");

            IRestResponse response = client.Execute(request);

            if ((int)response.StatusCode == 200)
            {
                return JsonConvert.DeserializeObject<IEnumerable<User>>(response.Content).Where(x => !x.id.Equals(_memoryService.LoggedOndUser.id)).ToList();
            }
            else
                return null;
        }

        public UserManagerResponse DeleteUser(string id)
        {
            var client = new RestClient($"{_memoryService.BaseEndPoint}/auth/{id}")
            {
                Timeout = -1
            };

            var request = new RestRequest(Method.DELETE);
            request.AddHeader("Authorization", $"Bearer {_memoryService.Token}");
            request.AddHeader("Content-Type", "application/json");

            IRestResponse response = client.Execute(request);

            if ((int)response.StatusCode == 200)
            {
                return JsonConvert.DeserializeObject<UserManagerResponse>(response.Content);
            }
            else if ((int)response.StatusCode == 409)
                return JsonConvert.DeserializeObject<UserManagerResponse>(response.Content);
            else
            {
                return new UserManagerResponse
                {
                    Message = "Some properties are not valid",
                    IsSuccess = false
                };
            }
        }

        public IEnumerable<Rol> GetAllRoles()
        {
            var client = new RestClient(_memoryService.BaseEndPoint + "/auth/roles");
            client.Timeout = -1;
            var request = new RestRequest(Method.GET);
            request.AddHeader("Authorization", $"Bearer {_memoryService.Token}");
            request.AddHeader("Content-Type", "application/json");

            IRestResponse response = client.Execute(request);

            if ((int)response.StatusCode == 200)
            {
                return JsonConvert.DeserializeObject<IEnumerable<Rol>>(response.Content).Where(x => !x.id.Equals(_memoryService.LoggedOndUser.id)).ToList();
            }
            else
                return null;
        }

        public UserManagerResponse CreateUser(RegisterViewModel user)
        {
            var client = new RestClient(_memoryService.BaseEndPoint + "/auth/register");
            client.Timeout = -1;
            var request = new RestRequest(Method.POST);
            request.AddHeader("Content-Type", "application/json");
            request.AddHeader("Authorization", $"Bearer {_memoryService.Token}");

            var body = JsonConvert.SerializeObject(user);
            request.AddParameter("application/json", body, ParameterType.RequestBody);
            IRestResponse response = client.Execute(request);

            if ((int)response.StatusCode == 200)
            {
                return JsonConvert.DeserializeObject<UserManagerResponse>(response.Content);
            } // User did not create
            else if ((int)response.StatusCode == 400)
            {
                var result = JsonConvert.DeserializeObject<UserManagerResponse>(response.Content);
                // if (result == typeof(UserManagerResponse))
                if (result.GetType() == typeof(UserManagerResponse))
                    return JsonConvert.DeserializeObject<UserManagerResponse>(response.Content);
                else
                    return new UserManagerResponse
                    {
                        Message = "Some properties are not valid",
                        IsSuccess = false
                    };
            }
            else
            {
                return new UserManagerResponse
                {
                    Message = "Some properties are not valid",
                    IsSuccess = false
                };
            }
        }
    }
}
