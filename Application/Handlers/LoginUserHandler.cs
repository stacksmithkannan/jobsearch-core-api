using JobFinder.API.Application.Queries;
using JobFinder.API.Data;
using MediatR;
using Microsoft.Extensions.Configuration;
using Microsoft.IdentityModel.Tokens;
using System.IdentityModel.Tokens.Jwt;
using System.Security.Claims;
using System.Text;
using Microsoft.EntityFrameworkCore;
using BCrypt.Net;

namespace JobFinder.API.Application.Handlers
{
    public class LoginUserHandler:IRequestHandler<LoginUserQuery,string>
    {
        private readonly ApplicationDbContext _context;
        private readonly IConfiguration _config;

        public LoginUserHandler(ApplicationDbContext context, IConfiguration config)
        {
            _context = context;
            _config = config;
        }

        public async Task<string> Handle(LoginUserQuery request,CancellationToken cancellationToken)
        {
            var user = await _context.Users.FirstOrDefaultAsync(u => u.Email == request.Email,cancellationToken);
            if (user == null || !BCrypt.Net.BCrypt.Verify(request.Password, user.Password))
                return "Invalid email or password";


            //Validate JWT Configuration
            var jwtKey = _config["Jwt:Key"];
            var jwtIssuer = _config["Jwt:Issuer"];
            var jwtAudience = _config["Jwt:Audience"];
            var jwtDuration = _config["Jwt:DurationInMinutes"];

            if (string.IsNullOrEmpty(jwtKey) ||
                string.IsNullOrEmpty(jwtIssuer) ||
                string.IsNullOrEmpty(jwtAudience) ||
                string.IsNullOrEmpty(jwtDuration))
                {
                    throw new InvalidOperationException("One or more JWT configurations are missing in appsettings.json.");
                }

            var key = new SymmetricSecurityKey(Encoding.UTF8.GetBytes(jwtKey));
            var creds = new SigningCredentials(key, SecurityAlgorithms.HmacSha256);

            //JWT token generation
            var authClaims = new List<Claim>()
            {
                new Claim(ClaimTypes.Email, request.Email),
                new Claim(ClaimTypes.Role,user.Role),
                new Claim(JwtRegisteredClaimNames.Jti,Guid.NewGuid().ToString())
            };


            var token = new JwtSecurityToken(
                issuer: jwtIssuer,
                audience: jwtAudience,
                claims: authClaims,
                expires: DateTime.UtcNow.AddMinutes(Convert.ToDouble(jwtDuration)),
                signingCredentials: creds
                );

            return new JwtSecurityTokenHandler().WriteToken(token);
        }
    }
}
