using StackExchange.Redis;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace PostCacheRedis.Helper
{
    public static class CacheRedisMultiplexer
    {
        
        private static Lazy<ConnectionMultiplexer> Crearconexion = new Lazy<ConnectionMultiplexer>(
            () => { return ConnectionMultiplexer.Connect(
                "TU CADENA DE CONEXION DE AZURE CACHE REDIS"); 
            });
        public static ConnectionMultiplexer Connection { get { return Crearconexion.Value; } }

    }
}
