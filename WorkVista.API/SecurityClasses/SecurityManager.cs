using Microsoft.IdentityModel.Tokens;
using System.IdentityModel.Tokens.Jwt;
using System.Security.Claims;
using System.Text;
using WorkVista.API.HelperLayer.Constants;
using WorkVista.API.ModelLayer.AppSettings;

namespace WorkVista.API.SecurityClasses
{
    public class SecurityManager
    {
        #region AuthenticateUser Method
        public AppSecurityToken AuthenticateUser(string name, string role, JwtSettingsModel settings)
        {
            AppSecurityToken asToken;

            // Validate the user passed in
            // Create the AppSecurityToken object
            asToken = ValidateUser(name, role);

            if (asToken.User.IsAuthenticated)
            {
                // Load User Claims into Security Token
                LoadUserClaims(asToken);

                // Build Application Security Token
                SetJwtToken(settings, asToken);
            }

            return asToken;
        }
        #endregion

        #region ValidateUser Method
        protected AppSecurityToken ValidateUser(string name, string role)
        {
            AppSecurityToken asToken = new();

            asToken.User.UserName = name;
            asToken.User.UserId = new Guid();
            asToken.User.RoleName = role;
            asToken.User.IsAuthenticated = true;

            return asToken;
        }
        #endregion

        #region LoadUserClaims
        protected void LoadUserClaims(AppSecurityToken asToken)
        {
            // Get Claims for a user - HARD CODED FOR NOW
            // TODO: Get Claims from a Data Store
            switch (asToken.User.RoleName)
            {
                case RoleConstants.ADMIN:
                    asToken.Claims.Add(new AppUserClaim()
                    {
                        UserId = asToken.User.UserId,
                        ClaimType = "GetSwitchRole",
                        ClaimValue = "true"
                    });
                    asToken.Claims.Add(new AppUserClaim()
                    {
                        UserId = asToken.User.UserId,
                        ClaimType = "GetDashboard",
                        ClaimValue = "true"
                    });
                    break;

                case RoleConstants.EMPLOYEE:

                    asToken.Claims.Add(new AppUserClaim()
                    {
                        UserId = asToken.User.UserId,
                        ClaimType = "GetDashboard",
                        ClaimValue = "true"
                    });
                    break;
            }
        }
        #endregion

        #region SetJwtToken
        protected void SetJwtToken(JwtSettingsModel settings, AppSecurityToken asToken)
        {
            // Build JWT claims
            List<Claim> claims = BuildJWTClaims(asToken);

            SecurityTokenDescriptor tokenDescriptor = new()
            {
                Expires = DateTime.UtcNow.AddMinutes(settings.MinutesToExpiration),
                Issuer = settings.Issuer,
                Audience = settings.Audience,
                SigningCredentials = new SigningCredentials
                  (new SymmetricSecurityKey(Encoding.ASCII.GetBytes(settings.Key)),
                  SecurityAlgorithms.HmacSha512Signature),
                // Add Claims
                Subject = new ClaimsIdentity(claims)
            };

            var tokenHandler = new JwtSecurityTokenHandler();
            var bearerToken = tokenHandler.WriteToken(tokenHandler.CreateToken(tokenDescriptor));

            // Create a string representation of the Jwt token
            // Stored into BearerToken property
            asToken.BearerToken = bearerToken;
        }
        #endregion

        #region BuildJWTClaims Method
        protected List<Claim> BuildJWTClaims(AppSecurityToken asToken)
        {
            // Create standard JWT claims
            List<Claim> ret = new()
            {
              // Add Unique User Name
              new Claim(JwtRegisteredClaimNames.Sub, asToken.User.UserName),
              // Add Unique JWT Token Identifier
              new Claim(JwtRegisteredClaimNames.Jti, Guid.NewGuid().ToString()),
              // Add IsAuthenticated Claim
              new Claim("IsAuthenticated", asToken.User.IsAuthenticated.ToString())
            };

            // Add Custom Claims for your Application
            foreach (var item in asToken.Claims)
            {
                ret.Add(new Claim(item.ClaimType, item.ClaimValue));
            }

            return ret;
        }
        #endregion
    }
}
