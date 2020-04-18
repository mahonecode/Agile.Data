using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace Agile.Data.ExternalServiceInterface
{
    public interface ISerializeService
    {
        string SerializeObject(object value);
        string AgileSerializeObject(object value);
        T DeserializeObject<T>(string value);
    }
}
