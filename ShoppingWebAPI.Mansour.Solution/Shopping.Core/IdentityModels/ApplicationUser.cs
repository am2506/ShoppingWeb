using Microsoft.AspNetCore.Identity;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Shopping.Core.IdentityModels
{
    public class ApplicationUser : IdentityUser
    {
        public string DisplayName { get; set; } = null!;
        
        //Navigation Property
        public Address ?Address { get; set; }
        

    }
}
