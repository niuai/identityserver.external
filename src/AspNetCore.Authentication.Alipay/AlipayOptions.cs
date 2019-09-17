using Microsoft.AspNetCore.Authentication;
using Microsoft.AspNetCore.Authentication.OAuth;
using Microsoft.AspNetCore.Http;
using System.Security.Claims;

namespace AspNetCore.Authentication.Alipay
{
    public class AlipayOptions : OAuthOptions
    {
        public AlipayOptions()
        {
            CallbackPath = new PathString("/signin-alipay");
            AuthorizationEndpoint = AlipayDefaults.AuthorizationEndpoint;
            TokenEndpoint = AlipayDefaults.TokenEndpoint;
            UserInformationEndpoint = AlipayDefaults.UserInformationEndpoint;
            OpenIdEndpoint = AlipayDefaults.OpenIdEndpoint;

            Scope.Add("auth_user"); // 默认只请求对 auth_user 进行授权

            ClaimActions.MapJsonKey(ClaimTypes.NameIdentifier, "openid");
            ClaimActions.MapJsonKey(ClaimTypes.Name, "nickname");
            ClaimActions.MapJsonKey(ClaimTypes.Gender, "gender");
            ClaimActions.MapJsonKey("sub", "openid");
            ClaimActions.MapJsonKey("urn:qq:figure", "figureurl_qq_1");
        }

        public string OpenIdEndpoint { get; }

        public string AppId
        {
            get { return ClientId; }
            set { ClientId = value; }
        }

        public string AppKey
        {
            get { return ClientSecret; }
            set { ClientSecret = value; }
        }
    }
}
