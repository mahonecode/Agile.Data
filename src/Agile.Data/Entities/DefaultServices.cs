using Agile.Data.ExternalServiceInterface;
using Agile.Data.IntegrationServices;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace Agile.Data.Entities
{
    public class DefaultServices
    {
        public static ICacheService ReflectionInoCache= new ReflectionInoCacheService();
        public static ICacheService DataInoCache = null;
        public static ISerializeService Serialize= new SerializeService();
    }
}