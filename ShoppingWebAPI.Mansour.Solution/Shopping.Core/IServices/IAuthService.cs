using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Shopping.Core.IdentityModels;

namespace Shopping.Core.IServices
{
    public interface IAuthService
    {
        Task<string> CreateTokenAsync(ApplicationUser user);
    }
}
