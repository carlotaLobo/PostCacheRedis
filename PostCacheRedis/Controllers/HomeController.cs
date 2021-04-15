using Microsoft.AspNetCore.Mvc;
using ModelsEssup.Models;
using Newtonsoft.Json;
using PostCacheRedis.Helper;
using PostCacheRedis.Services;
using StackExchange.Redis;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace PostCacheRedis.Controllers
{
    public class HomeController : Controller
    {
        ServiceApi service;
        private IDatabase redis;

        public HomeController(ServiceApi service)
        {
            this.service = service;
            this.redis = CacheRedisMultiplexer.Connection.GetDatabase();
        }
        public async Task<IActionResult> Index()
        {
            return View((await this.service.GetMaterials()).Take(10).ToList());
        }
        public IActionResult EliminarCache()
        {
            this.redis.KeyDelete("MATERIALESPOST");
            return RedirectToAction("Index");
        }
        public async Task<IActionResult> GuardarCache(String id)
        {
            List<Materials> materiales = new List<Materials>();
            String m = this.redis.StringGet("MATERIALESPOST");
            if (m != null)
            {
                materiales = JsonConvert.DeserializeObject<List<Materials>>(m);
                materiales.Add(await this.service.GetMaterial(id));
                this.redis.StringSet("MATERIALESPOST", JsonConvert.SerializeObject(materiales), TimeSpan.FromMinutes(5));
            }
            else
            {
                materiales.Add(await this.service.GetMaterial(id));
                this.redis.StringSet("MATERIALESPOST", JsonConvert.SerializeObject(materiales), TimeSpan.FromMinutes(5));
            }
            return RedirectToAction("Index");
        }
        public IActionResult MostrarCache()
        {
            List<Materials> materials = new List<Materials>();
            String m = this.redis.StringGet("MATERIALESPOST");
            if (m != null) materials = JsonConvert.DeserializeObject<List<Materials>>(m);

            return View(materials);

        }
    }
}
