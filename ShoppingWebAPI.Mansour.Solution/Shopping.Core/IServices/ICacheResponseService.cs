using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Shopping.Core.Models.OrderComponents;

namespace Shopping.Core.IServices
{
    public interface ICacheResponseService
    {
        Task CachResponse(string Key, object Response, TimeSpan timeToLive);
        Task<string?> GetCachResponse(string Key);

    }
}
