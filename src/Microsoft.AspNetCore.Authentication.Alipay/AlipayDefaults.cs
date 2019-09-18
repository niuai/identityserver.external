namespace AspNetCore.Authentication.Alipay
{
    public class AlipayDefaults
    {
        public const string AuthenticationScheme = "Alipay";

        public static readonly string DisplayName = "Alipay";

        /// <summary>
        /// Endpoint for getting Authorization Code
        /// </summary>
        public static readonly string AuthorizationEndpoint = "https://openauth.alipay.com/oauth2/publicAppAuthorize.htm";

        public static readonly string GatewayUrl = "https://openapi.alipay.com/gateway.do";

        public static readonly string CharSet = "UTF-8";

        public static readonly string SignType = "RSA2";
    }
}
