using Microsoft.EntityFrameworkCore;
using Microsoft.AspNetCore.Identity.EntityFrameworkCore;

using Servicios.api.Seguridad.Core.Entities;

namespace Servicios.api.Seguridad.Core.Context;


public class SeguridadContext : IdentityDbContext<User>
{
    public SeguridadContext(DbContextOptions options) : base(options) { }

    protected override void OnModelCreating(ModelBuilder builder)
    {
        base.OnModelCreating(builder);
    }
}