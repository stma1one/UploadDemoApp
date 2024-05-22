﻿using System;
using System.Collections.Generic;
using System.Linq;
using System.Net.Http.Headers;
using System.Text;
using System.Text.Json;
using System.Threading.Tasks;
using LoginDemoApp.Models;

namespace LoginDemoApp.Services
{
    public class LoginDemoWebAPIProxy
    {
        private HttpClient client;
        private JsonSerializerOptions jsonSerializerOptions;
        private string baseUrl;
        public static string BaseAddress = DeviceInfo.Platform == DevicePlatform.Android ? "http://10.0.2.2:5171/api/" : "http://localhost:5171/api/";
        public static string ImageUrl = DeviceInfo.Platform == DevicePlatform.Android ? "http://10.0.2.2:5171/images/" : "http://localhost:5171/images/";

        public User LoggedInUser { get; set; }

        public LoginDemoWebAPIProxy()
        {
            //Set client handler to support cookies!!
            HttpClientHandler handler = new HttpClientHandler();
            handler.CookieContainer = new System.Net.CookieContainer();

            this.client = new HttpClient(handler, true);
            this.baseUrl = BaseAddress;
            jsonSerializerOptions = new JsonSerializerOptions()
            {
                WriteIndented = true,
                PropertyNameCaseInsensitive = true
            };
        }

        public async Task<User> LoginAsync(string email, string password)
        {
            //Set URI to the specific function API
            string url = $"{this.baseUrl}login";
            try
            {
                //Call the server API
                LoginInfo info = new LoginInfo() { Email = email, Password = password };
                string json = JsonSerializer.Serialize(info, jsonSerializerOptions);
                //string json = JsonSerializer.Serialize(new{ Email=email,Password=password},jsonSerializerOptions);
                StringContent content = new StringContent(json, Encoding.UTF8, "application/json");
                HttpResponseMessage response = await client.PostAsync(url, content);
                //Check status
                if (response.IsSuccessStatusCode)
                {
                    //Extract the content as string
                    string resContent = await response.Content.ReadAsStringAsync();
                    //Desrialize result
                    JsonSerializerOptions options = new JsonSerializerOptions
                    {
                        PropertyNameCaseInsensitive = true
                    };
                    User result = JsonSerializer.Deserialize<User>(resContent, options);
                    LoggedInUser = result;
                    return result;
                }
                else
                {
                    return null;
                }
            }
            catch (Exception ex)
            {
                return null;
            }
        }

        public async Task<string> CheckAsync()
        {
            //Set URI to the specific function API
            string url = $"{this.baseUrl}check";
            try
            {
                //Call the server API
                HttpResponseMessage response = await client.GetAsync(url);
                //Check status
                if (response.IsSuccessStatusCode)
                {
                    //Extract the content as string
                    string resContent = await response.Content.ReadAsStringAsync();
                    return resContent;
                }
                else
                {
                    return "User is not logged in!";
                }
            }
            catch (Exception ex)
            {

                return "FAILED WITH EXCEPTION!";
            }
        }

        internal async Task<string> UploadImage(FileResult photo)
        {
            byte[] streamBytes;
            //take the photo and make it a byte array
            using (var stream = await photo.OpenReadAsync())
            using (var memoryStream = new MemoryStream())
            {
                await stream.CopyToAsync(memoryStream);
                streamBytes = memoryStream.ToArray();
            }

            using (var content = new MultipartFormDataContent())
            {
                var fileContent = new ByteArrayContent(streamBytes);
                fileContent.Headers.ContentType = MediaTypeHeaderValue.Parse("image/jpeg");
                //"file" --זהה לשם הפרמטר של הפעולה בשרת שמייצגת את הקובץ
                content.Add(fileContent, "file", photo.FileName);
                var response = await client.PostAsync(@$"{this.baseUrl}UploadProfile/talsi@talsi.com", content);
                if (response.IsSuccessStatusCode)
                {
                   string jsonContent= await response.Content.ReadAsStringAsync();
                   ImageResponse res=JsonSerializer.Deserialize<ImageResponse>(jsonContent, jsonSerializerOptions);
                    return res.FileName;
                }
            }
            return null;
        }
    }
}
