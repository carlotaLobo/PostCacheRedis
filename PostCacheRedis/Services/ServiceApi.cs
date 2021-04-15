using ModelsEssup.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Net.Http;
using System.Net.Http.Headers;
using System.Threading.Tasks;

namespace PostCacheRedis.Services
{
    public class ServiceApi
    {
        MediaTypeWithQualityHeaderValue headers;
        private Uri uri;
        public ServiceApi(String url)
        {
            this.uri = new Uri(url);
            this.headers = new MediaTypeWithQualityHeaderValue("application/json");
        }
        public async Task<T> GetApi<T>(String request)
        {
            using(HttpClient client= new HttpClient())
            {
                client.DefaultRequestHeaders.Accept.Clear();
                client.DefaultRequestHeaders.Accept.Add(this.headers);
                client.BaseAddress = this.uri;

               HttpResponseMessage response= await client.GetAsync(request);

                if (response.IsSuccessStatusCode)
                {
                    return await response.Content.ReadAsAsync<T>();
                }
                else
                {
                    return default(T);
                }

            }
        }
        public async Task<List<Materials>> GetMaterials()
        {
            return await this.GetApi<List<Materials>>("api/material/findallmaterials");
        }
        public async Task<Materials> GetMaterial(String id)
        {
            return await this.GetApi<Materials>("api/Material/FindMaterialById/"+id);
        }
    }
}
