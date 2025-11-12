using EducationHub.Conteudo.Data;
using Microsoft.EntityFrameworkCore;

namespace EducationHub.API.Configurations
{
    public static class IdentityConfig
    {
        public static IServiceCollection AddIdentityConfiguration(this IServiceCollection services,
            IConfiguration configuration)
        {

            return services;
        }
    }
}

//services.AddIdentityCore<IdentityUser>(options =>
//{
//    options.Password.RequireDigit = true;
//    options.Password.RequireLowercase = true;
//    options.Password.RequireUppercase = true;
//    options.Password.RequireNonAlphanumeric = false;
//    options.Password.RequiredLength = 8;
//})
//.AddRoles<IdentityRole>()
//.AddEntityFrameworkStores<ApplicationDbContext>()
//.AddDefaultTokenProviders();
//services.Configure<IdentityOptions>(options =>
//{
//    options.User.RequireUniqueEmail = true;
//});
