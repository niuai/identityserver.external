using Alipay.AopSdk.AspnetCore;
using Microsoft.AspNetCore.Authentication;
using Microsoft.Extensions.DependencyInjection;
using System;

namespace AspNetCore.Authentication.Alipay
{
    public static class AlipayExtensions
    {
        public static AuthenticationBuilder AddAlipay(this AuthenticationBuilder builder)
            => builder.AddAlipay(AlipayDefaults.AuthenticationScheme, _ => { });

        public static AuthenticationBuilder AddAlipay(this AuthenticationBuilder builder, Action<AlipayOptions> configureOptions)
            => builder.AddAlipay(AlipayDefaults.AuthenticationScheme, configureOptions);

        public static AuthenticationBuilder AddAlipay(this AuthenticationBuilder builder, string authenticationScheme, Action<AlipayOptions> configureOptions)
            => builder.AddAlipay(authenticationScheme, AlipayDefaults.DisplayName, configureOptions);

        public static AuthenticationBuilder AddAlipay(this AuthenticationBuilder builder, string authenticationScheme, string displayName, Action<AlipayOptions> configureOptions)
        {
            var alipayOptions = new AlipayOptions();

            configureOptions.Invoke(alipayOptions);

            builder.Services.AddAlipay(options =>
            {
                options.AlipayPublicKey = alipayOptions.AlipayPublicKey;
                options.AppId = alipayOptions.AppId;
                options.CharSet = alipayOptions.CharSet;
                options.Gatewayurl = alipayOptions.GatewayUrl;
                options.PrivateKey = alipayOptions.AppPrivateKey;
                options.SignType = alipayOptions.SignType;
                options.Uid = alipayOptions.Uid;
            });

            return builder.AddOAuth<AlipayOptions, AlipayHandler>(authenticationScheme, displayName, configureOptions);
        }
    }
}
