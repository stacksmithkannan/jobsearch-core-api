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
using Microsoft.Extensions.Logging;

namespace JobFinder.API.Application.Handlers
{
    public class LoginUserHandler:IRequestHandler<LoginUserQuery,string>
    {
        private readonly ApplicationDbContext _context;
        private readonly IConfiguration _config;
        private readonly ILogger<LoginUserHandler> _logger;

        public LoginUserHandler(ApplicationDbContext context, IConfiguration config, ILogger<LoginUserHandler> logger)
        {
            _context = context;
            _config = config;
            _logger = logger;
        }

        public async Task<string> Handle(LoginUserQuery request,CancellationToken cancellationToken)
        {
            _logger.LogInformation("Login attempt for email: {Email}", request.Email);

            var user = await _context.Users.FirstOrDefaultAsync(u => u.Email == request.Email,cancellationToken);
            if (user == null || !BCrypt.Net.BCrypt.Verify(request.Password, user.Password))
            {
                _logger.LogWarning("Invalid login attempt for email: {Email}", request.Email);
                return "Invalid email or password";
            }

            _logger.LogInformation("User authenticated successfully: {Email}", request.Email);

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
                    _logger.LogError("Missing JWT configuration values in appsettings.json");
                     throw new InvalidOperationException("One or more JWT configurations are missing in appsettings.json.");
                }
            _logger.LogInformation("Generating JWT token for: {Email}", request.Email);

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
            _logger.LogInformation("JWT token generated for: {Email}", request.Email);


            return new JwtSecurityTokenHandler().WriteToken(token);
        }
    }
}
