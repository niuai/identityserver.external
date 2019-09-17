using Alipay.AopSdk.AspnetCore;
using Alipay.AopSdk.Core.Request;
using Alipay.AopSdk.Core.Response;
using Microsoft.AspNetCore.Authentication;
using Microsoft.AspNetCore.Authentication.OAuth;
using Microsoft.AspNetCore.WebUtilities;
using Microsoft.Extensions.Logging;
using Microsoft.Extensions.Options;
using Microsoft.Extensions.Primitives;
using Newtonsoft.Json;
using Newtonsoft.Json.Linq;
using System;
using System.Collections.Generic;
using System.Globalization;
using System.Net.Http;
using System.Security.Claims;
using System.Text.Encodings.Web;
using System.Threading.Tasks;

namespace AspNetCore.Authentication.Alipay
{
    internal class AlipayHandler : OAuthHandler<AlipayOptions>
    {
        private readonly AlipayService _alipayService;

        public AlipayHandler(IOptionsMonitor<AlipayOptions> options, ILoggerFactory logger, UrlEncoder encoder, ISystemClock clock, AlipayService alipayService)
            : base(options, logger, encoder, clock)
        {
            _alipayService = alipayService;
        }

        protected override string BuildChallengeUrl(AuthenticationProperties properties, string redirectUri)
        {
            var scope = FormatScope();
            var state = Options.StateDataFormat.Protect(properties);

            var parameters = new Dictionary<string, string>()
            {
                { "app_id", Options.ClientId },
                { "redirect_uri", redirectUri },
            };

            var ret = QueryHelpers.AddQueryString(Options.AuthorizationEndpoint, parameters);
            ret += $"&scope={scope}&state={state}";

            return ret;
        }

        protected override async Task<HandleRequestResult> HandleRemoteAuthenticateAsync()
        {
            // 第一步，处理工作
            AuthenticationProperties properties = null;
            var query = Request.Query;

            // 若用户禁止授权，则重定向后不会带上 auth_code 参数，仅会带上 state 参数
            var code = query["auth_code"];
            var state = query["state"];

            properties = Options.StateDataFormat.Unprotect(state);
            if (properties == null)
                return HandleRequestResult.Fail("The oauth state was missing or invalid.");

            // OAuth2 10.12 CSRF
            if (!ValidateCorrelationId(properties))
                return HandleRequestResult.Fail("Correlation failed.");

            if (StringValues.IsNullOrEmpty(code))
                return HandleRequestResult.Fail("Code was not found.");

            // 第二步，通过 Code 获取 Access Token
            AlipaySystemOauthTokenResponse resAccessToken = null;
            try
            {
                var alipaySystemOauthTokenRequest = new AlipaySystemOauthTokenRequest
                {
                    Code = code,
                    GrantType = "authorization_code",
                    RefreshToken = ""
                };

                resAccessToken = _alipayService.Execute(alipaySystemOauthTokenRequest);
            }
            catch (Exception)
            {
                throw;
            }
            if (resAccessToken.IsError)
            {
                throw new Exception("Error occur when getting access token from Alipay.");
            }

            var identity = new ClaimsIdentity(ClaimsIssuer);
            if (Options.SaveTokens)
            {
                var authTokens = new List<AuthenticationToken>
                {
                    new AuthenticationToken { Name = "access_token", Value = resAccessToken.AccessToken }
                };

                if (!string.IsNullOrEmpty(resAccessToken.RefreshToken))
                {
                    authTokens.Add(new AuthenticationToken { Name = "refresh_token", Value = resAccessToken.RefreshToken });
                }

                if (!string.IsNullOrEmpty(resAccessToken.ExpiresIn))
                {
                    if (int.TryParse(resAccessToken.ExpiresIn, NumberStyles.Integer, CultureInfo.InvariantCulture, out int value))
                    {
                        var expiresAt = Clock.UtcNow + TimeSpan.FromSeconds(value);
                        authTokens.Add(new AuthenticationToken
                        {
                            Name = "expires_at",
                            Value = expiresAt.ToString("o", CultureInfo.InvariantCulture)
                        });
                    }
                }

                properties.StoreTokens(authTokens);
            }

            var ticket = await CreateTicketAsync(identity, properties, ConvertToOAuthTokenResponse(resAccessToken));
            if (ticket != null)
            {
                return HandleRequestResult.Success(ticket);
            }
            else
            {
                return HandleRequestResult.Fail("Failed to retrieve user information from remote server.");
            }
        }

        protected override async Task<AuthenticationTicket> CreateTicketAsync(
            ClaimsIdentity identity,
            AuthenticationProperties properties,
            OAuthTokenResponse tokens)
        {
            // 获取用户信息
            var alipayUserUserinfoShareRequest = _alipayService.Execute(new AlipayUserInfoShareRequest(), tokens.AccessToken);
            
            var userInfo = JObject.Parse(JsonConvert.SerializeObject(alipayUserUserinfoShareRequest));
            var context = new OAuthCreatingTicketContext(new ClaimsPrincipal(identity), properties, Context, Scheme, Options, Backchannel, tokens, userInfo);
            context.RunClaimActions();
            await Events.CreatingTicket(context);
            return new AuthenticationTicket(context.Principal, context.Properties, Scheme.Name);
        }

        private OAuthTokenResponse ConvertToOAuthTokenResponse(AlipaySystemOauthTokenResponse alipayTokenResponse)
        {
            var payload = JObject.Parse(JsonConvert.SerializeObject(alipayTokenResponse));

            return OAuthTokenResponse.Success(payload);
        }
    }
}
