using System;
using System.Collections.Generic;
using System.Configuration;
using System.Linq;
using System.Net;
using System.Net.Http;
using System.Xml.Serialization;
using System.Web;

using FriendsOfRabbits.API.Models.Petfinder;

namespace FriendsOfRabbits.API.Adapters
{
    public class PetfinderAdapter
    {
        private string key;
        private string password;
        private string shelterId;
        private string baseUrl;

        public PetfinderAdapter()
        {
            key = ConfigurationManager.AppSettings["Petfinder.Key"];
            password = ConfigurationManager.AppSettings["Petfinder.Password"];
            shelterId = ConfigurationManager.AppSettings["Petfinder.ShelterId"];
            baseUrl = ConfigurationManager.AppSettings["Petfinder.BaseUrl"];

        }

        public IEnumerable<petfinderPetRecord> GetPets()
        {
            var parameters = new Dictionary<string, string>();
            parameters["id"] = shelterId;
            var result = GetResponse<petfinderPetRecordList>("shelter.getPets", parameters);

            return result.pet;
        }
        public petfinderPetRecord GetPet(string id)
        {
            var parameters = new Dictionary<string, string>();
            parameters["id"] = shelterId;
            var result = GetResponse<petfinderPetRecord>("pet.get", parameters);

            return result;
        }
        public petfinderPetRecord GetRandomPet()
        {
            var results = GetPets();

            var r = new Random();
            return results.ElementAt(r.Next(results.Count()));
        }

        private TResult GetResponse<TResult>(string method, Dictionary<string, string> parameters)
        {
            parameters["key"] = key;
            var paramString = string.Join("&", parameters.Select((kv) => string.Format("{0}={1}", WebUtility.UrlEncode(kv.Key), WebUtility.UrlEncode(kv.Value))).ToArray());
            var url = string.Format("{0}{1}?{2}", baseUrl, method, paramString);

            var http = new HttpClient();

            var httpTask = http.GetAsync(url);
            httpTask.Wait();
            var response = httpTask.Result;

            if (!response.IsSuccessStatusCode)
            {
                throw new Exception(response.ReasonPhrase);
            }

            var streamTask = response.Content.ReadAsStreamAsync();
            streamTask.Wait();


            var streamReader = new System.IO.StreamReader(streamTask.Result);
            var streamText = streamReader.ReadToEnd();
            streamTask.Result.Seek(0, System.IO.SeekOrigin.Begin);

            var deserializer = new XmlSerializer(typeof(petfinder));
            return (TResult)((petfinder)deserializer.Deserialize(streamTask.Result)).Item;
        }
    }
}