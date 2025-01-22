using Microsoft.AspNetCore.Identity.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore;//importing packages
//it is used to manage and store user accounts ,passwords ,profile,data and more
//supports external login in providers like facebook

namespace Webapi1.services
{
    public class ApplicationDbcontext : IdentityDbContext
    {
        public ApplicationDbcontext(DbContextOptions options) : base(options)
        {
        }
    }
}
