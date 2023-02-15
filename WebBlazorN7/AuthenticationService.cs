using System.IdentityModel.Tokens.Jwt;

namespace WebBlazorN7
{
	public class AuthenticationService
	{
		public static long GetTokenExpirationTime(string token)
		{
			var handler = new JwtSecurityTokenHandler();
			var jwtSecurityToken = handler.ReadJwtToken(token);
			var tokenExp = jwtSecurityToken.Claims.First(claim => claim.Type.Equals("exp")).Value;
			var ticks = long.Parse(tokenExp);
			return ticks;
		}

		public static bool CheckTokenIsValid(string token)
		{
			var tokenTicks = GetTokenExpirationTime(token);
			var tokenDate = DateTimeOffset.FromUnixTimeSeconds(tokenTicks).UtcDateTime;

			var now = DateTime.Now.ToUniversalTime();

			var valid = tokenDate >= now;

			return valid;
		}

		public static DateTime ConvertFromUnixTimestamp(long timestamp)
		{
			DateTime unixStart = new DateTime(1970, 1, 1, 0, 0, 0, 0, System.DateTimeKind.Utc);
			long unixTimeStampInTicks = (long)(timestamp * TimeSpan.TicksPerSecond);
			return new DateTime(unixStart.Ticks + unixTimeStampInTicks, System.DateTimeKind.Utc);
		}
	}
}
