using Blazored.LocalStorage;
using Microsoft.AspNetCore.Components.Authorization;
using System.Net.Http.Headers;
using System.Net.Http.Json;
using System.Net.WebSockets;
using System.Reflection.Metadata.Ecma335;
using System.Security;
using System.Security.Claims;
using System.Text.Json.Nodes;
using System.Text.Json.Serialization;

namespace BlazorApp.Services
{
    public class CustomAuthStateProvider : AuthenticationStateProvider
    {
        private readonly HttpClient httpClient;
        private ISyncLocalStorageService localstorage;

        public CustomAuthStateProvider(HttpClient httpClient, ISyncLocalStorageService localstorage) {
            this.httpClient = httpClient;
            this.localstorage = localstorage;

            //reading the acesstoken in the constructor

            var accessToken = localstorage.GetItem<string>("accessToken");
            if (accessToken != null)
            {
             this.httpClient.DefaultRequestHeaders.Authorization = new AuthenticationHeaderValue("Bearer",accessToken);
            }
        }
        public override async Task<AuthenticationState> GetAuthenticationStateAsync()
        {
            // var user = new ClaimsPrincipal(new ClaimsIdentity());// non-autheticated

            //var claims = new List<Claim> { new Claim(ClaimTypes.Name, "John") };
            //var identity = new ClaimsIdentity(claims, "Any");
            //var user = new ClaimsPrincipal(identity);

            // return Task.FromResult(new AuthenticationState(user));
            var user = new ClaimsPrincipal(new ClaimsIdentity());

            try
            {
                var response = await httpClient.GetAsync("manage/info");
                if (response.IsSuccessStatusCode)//if we get a successful reponse
                {
                    var strResponse = await response.Content.ReadAsStringAsync();//we read the response as a string
                    var jsonResponse = JsonNode.Parse(strResponse);//then we will convert the stting to a jsoon response
                    var email = jsonResponse!["email"]!.ToString();//read the email address from the user from the json reponse 

                    var claims = new List<Claim>
                    {
                        new(ClaimTypes.Name, email),
                        new(ClaimTypes.Email, email),

                    };

                    var identity = new ClaimsIdentity(claims , "Token");//claims iidentity
                    user = new ClaimsPrincipal(identity);

                    return new AuthenticationState(user);
                }


            }
            catch (Exception ex) {
               
                
            
            }

            return new AuthenticationState(user);
        }

        public async Task<Formresult> LoginAsync(String email, string password)
        {
            try
            {
                var response = await httpClient.PostAsJsonAsync("login", new { email, password });
                if (response.IsSuccessStatusCode)//if we have a success response.
                {
                    var strResponse = await response.Content.ReadAsStringAsync();//we read the response as a string
                    var jsonResponse = JsonNode.Parse(strResponse);///then convert the response in a json format
                    var accessToken = jsonResponse?["accessToken"]?.ToString();//then read the access token as well the refresh token down belowe
                    var refreshToken = jsonResponse?["refreshToken"]?.ToString();

                    localstorage.SetItem("accessToken", accessToken);
                    localstorage.SetItem("refreshToken", refreshToken);
                    


                    //then we added the access token of the authorization header of the httpclient
                    httpClient.DefaultRequestHeaders.Authorization = new System.Net.Http.Headers.AuthenticationHeaderValue("Bearer", accessToken);//bearer is added with capital b and the access token

                    NotifyAuthenticationStateChanged(GetAuthenticationStateAsync());//refresh auth state,then we basically updating the authentication state of the user,then we call a method getauthenticatedstateAsyn())

                    return new Formresult { Succeeded = true };



                }
                else
                {
                    return new Formresult { Succeeded = false, Errors = ["incorrect email and password"] };
                }
            }
            catch { }

            return new Formresult { Succeeded = false, Errors = ["connection Error"] };
        }
    }
    public class Formresult
    {
        public bool Succeeded { get; set; }

        public string[] Errors { get; set; } = [];

        }
    
}
