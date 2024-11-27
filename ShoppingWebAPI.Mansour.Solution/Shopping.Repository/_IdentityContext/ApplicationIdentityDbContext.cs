using Microsoft.AspNetCore.Identity.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Shopping.Core.IdentityModels;

namespace Shopping.Repository.IdentityContext
{
    public class ApplicationIdentityDbContext : IdentityDbContext<ApplicationUser>
    {
        
        public ApplicationIdentityDbContext(DbContextOptions<ApplicationIdentityDbContext> options) : base(options)
        { }

    }
}
