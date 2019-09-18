using Microsoft.AspNetCore.Authentication;
using Microsoft.AspNetCore.Authentication.OAuth;
using Microsoft.AspNetCore.Http;
using System.Security.Claims;

namespace AspNetCore.Authentication.Alipay
{
    /// <summary>
    /// Settings for Alipay. Ref: https://docs.open.alipay.com/263/105809/
    /// </summary>
    public class AlipayOptions : OAuthOptions
    {
        public AlipayOptions()
        {
            CallbackPath = new PathString("/signin-alipay");
            ClientSecret = "none";
            AuthorizationEndpoint = AlipayDefaults.AuthorizationEndpoint;
            TokenEndpoint = "none";

            Scope.Add("auth_user"); // 默认只请求对 auth_user 进行授权

            ClaimActions.MapJsonKey(ClaimTypes.NameIdentifier, "user_id");
            ClaimActions.MapJsonKey(ClaimTypes.Name, "name");
            ClaimActions.MapJsonKey(ClaimTypes.Gender, "gender");
            ClaimActions.MapJsonKey("sub", "user_id");
        }

        public string AppId { get => ClientId; set => ClientId = value; }

        /// <summary>
        /// Endpoint for operations in Alipay
        /// </summary>
        public string GatewayUrl { get; set; } = AlipayDefaults.GatewayUrl;

        /// <summary>
        /// 开发者私钥，由开发者自己生成
        /// </summary>
        public string AppPrivateKey { get; set; }

        /// <summary>
        /// 支付宝公钥，由支付宝生成
        /// </summary>
        public string AlipayPublicKey { get; set; }

        /// <summary>
        /// 编码集，支持 GBK/UTF-8
        /// </summary>
        public string CharSet { get; set; } = AlipayDefaults.CharSet;

        /// <summary>
        /// 商户生成签名字符串所使用的签名算法类型，目前支持 RSA2 和 RSA，推荐使用 RSA2
        /// </summary>
        public string SignType { get; set; } = AlipayDefaults.SignType;

        /// <summary>
        /// 商户ID
        /// </summary>
        public string Uid { get; set; }
    }
}
