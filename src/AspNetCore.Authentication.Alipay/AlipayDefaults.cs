using System;
using System.Collections.Generic;
using System.Text;

namespace AspNetCore.Authentication.Alipay
{
    public class AlipayDefaults
    {
        public const string AuthenticationScheme = "Alipay";

        public static readonly string DisplayName = "Alipay";

        /// <summary>
        /// 获取Authorization Code
        /// </summary>
        public static readonly string AuthorizationEndpoint = "https://openauth.alipaydev.com/oauth2/publicAppAuthorize.htm";

        /// <summary>
        /// 通过Authorization Code获取Access Token
        /// </summary>
        public static readonly string TokenEndpoint = "https://graph.qq.com/oauth2.0/token";

        /// <summary>
        ///通过获取的Access Token，得到对应用户身份的OpenID
        /// </summary>
        public static readonly string OpenIdEndpoint = "https://graph.qq.com/oauth2.0/me";

        /// <summary>
        ///获取到Access Token和OpenID后，可通过调用OpenAPI来获取或修改用户个人信息
        /// </summary>
        public static readonly string UserInformationEndpoint = "https://graph.qq.com/user/get_user_info";
    }
}
