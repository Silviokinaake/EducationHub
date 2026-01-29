using EducationHub.API.Settings;
using EducationHub.API.ViewModels;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Options;
using Microsoft.IdentityModel.Tokens;
using System.IdentityModel.Tokens.Jwt;
using System.Security.Claims;

namespace EducationHub.API.Controllers
{
    [ApiController]
    [Route("api/[controller]")]
    public class AuthController : ControllerBase
    {
        private readonly SignInManager<IdentityUser> _signInManager;
        private readonly UserManager<IdentityUser> _userManager;
        private readonly RoleManager<IdentityRole> _roleManager;
        private readonly JwtSettings _jwtSettings;
        private SecurityToken token;

        public AuthController(
                            SignInManager<IdentityUser> signInManager,
                            UserManager<IdentityUser> userManager,
                            RoleManager<IdentityRole> roleManager,
                            IOptions<JwtSettings> jwtSettings)
        {
            _signInManager = signInManager;
            _userManager = userManager;
            _roleManager = roleManager;
            _jwtSettings = jwtSettings.Value;
        }

        [HttpPost("register")]
        public async Task<ActionResult> Register(RegisterUserViewModel registerUser)
        {

            if (!ModelState.IsValid) return ValidationProblem(ModelState);

            var identitiyUser = new IdentityUser
            {
                UserName = registerUser.Email,
                Email = registerUser.Email,
                EmailConfirmed = true
            };
            var result = await _userManager.CreateAsync(identitiyUser, registerUser.Password);

            if (result.Succeeded)
            {
                // Criar a role Administrador se não existir
                if (!await _roleManager.RoleExistsAsync("Administrador"))
                {
                    await _roleManager.CreateAsync(new IdentityRole("Administrador"));
                }

                // Adicionar role ao usuário se foi solicitado
                if (registerUser.EhAdministrador)
                {
                    await _userManager.AddToRoleAsync(identitiyUser, "Administrador");
                }

                await _signInManager.SignInAsync(identitiyUser, isPersistent: false);
                return Ok(await GerarJwt(registerUser.Email));
            }

            return Problem("Falha ao registrar o usuário");
        }


        [HttpPost("login")]
        public async Task<ActionResult> Login(LoginUserViewModel loginUser)
        {
            if (!ModelState.IsValid) return ValidationProblem(ModelState);
            var result = await _signInManager.PasswordSignInAsync(loginUser.Email, loginUser.Password, false, true);
            if (result.Succeeded)
            {
                return Ok(await GerarJwt(loginUser.Email));
            }
            return Unauthorized("Usuário ou senha inválidos");
        }


        private async Task<string> GerarJwt(string email, IdentityUser user = null)
        {
            if (user == null) { user = await _userManager.FindByEmailAsync(email); }
            var roles = await _userManager.GetRolesAsync(user);

            var claims = new List<Claim>
        {
            new (JwtRegisteredClaimNames.Sub, user.Id),
            new (ClaimTypes.Name, user.UserName),
            new (ClaimTypes.NameIdentifier, user.Id.ToString())
        };

            if (roles.Any(x => x == "Administrador"))
            {
                claims.Add(new Claim("nivel", "Admin"));
            }

            foreach (var role in roles)
            {
                claims.Add(new Claim(ClaimTypes.Role, role));
            }

            var tokenHandler = new JwtSecurityTokenHandler();
            var key = System.Text.Encoding.ASCII.GetBytes(_jwtSettings.Secret);

            var token = tokenHandler.CreateToken(new SecurityTokenDescriptor
            {
                Subject = new ClaimsIdentity(claims),
                Expires = DateTime.UtcNow.AddHours(_jwtSettings.ExpiracaoHoras),
                Issuer = _jwtSettings.Emissor,
                Audience = _jwtSettings.ValidoEm,
                SigningCredentials = new SigningCredentials(new SymmetricSecurityKey(key), SecurityAlgorithms.HmacSha256Signature)
            });

            var encodedToken = tokenHandler.WriteToken(token);

            return encodedToken;
        }
    }
}
