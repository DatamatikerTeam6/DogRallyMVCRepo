using DogRallyMVC.Areas.Identity.Data;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Identity.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore;

namespace DogRallyMVC.Data;

// Inherit from IdentityDbContext to include Identity functionality
public class DogRallyMVCContext : IdentityDbContext<DogRallyMVCUser>
{
    public DogRallyMVCContext(DbContextOptions<DogRallyMVCContext> options)
        : base(options)
    {
    }
}
