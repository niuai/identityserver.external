using Alipay.AopSdk.AspnetCore;
using Microsoft.AspNetCore.Authentication;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.DependencyInjection.Extensions;
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
            builder.Services.AddAlipay(options =>
            {
                options.AlipayPublicKey = "MIIBIjANBgkqhkiG9w0BAQEFAAOCAQ8AMIIBCgKCAQEAlzrWgal1iF9flTWO9dThRXqa9zAPDk1bsKN0x9WKSiTmxr8VDtCwvO0ScsIxr7LRm2WjbnnL+WSTXi7Y3GsQbxrv5j9OGBTSvbLVi64lQPJSIhNys6XXjAkXnwMD0ICBF8kyy3hHEtOMFr9zwsW5BDlX5RLQkG0Ril9/U57OQCe4IJ5ye+dSmstNpkCKgyXQhjHmNGCLzutqFTZRBxvp5LGu29cN4oywA5eSHts5lt5GbfdocbKU930ZH3z/k54u1eR+hdiSAGh6YQ8DBEvcl58GgfKwvO8gi7+sel6tUHaZHZnuqyFkc1I9ZrnH/07TjYeEor6oC9IHwilSYnjF6wIDAQAB";
                options.AppId = "2016101300674410";
                options.CharSet = "UTF-8";
                options.Gatewayurl = "https://openapi.alipaydev.com/gateway.do";
                options.PrivateKey = "MIIEowIBAAKCAQEAiiQSVuKZPw8jCHb2Yq1v6lsKat3gTPkneIwFdb0WNIWvUqAAPogQGJYQ3Vehdv5/OpcmTyorakiZKX+Rco7S+4+t3A0NGxheAnCDf5LMZqcSuqa9Kk5ZW96gxM+y1W8kzDeNP8g6vzF+XiC1cDzhye16sHBqeJP7owaj+N36+fpRE0N4GWVwl4iLA8Jlz2Cp3JXTxrTP8ZiTl5fqu4Uk7oLhWeoVzQuG2bCrKw3JAL93QqS9ECWIRj+I6KxDor2AYxLpmeOtxnmxSi99nL83uNzv9bG9tO6I2uqScPVh6YPzO/W9xLXxekffRwZPFrn4H82VbuqYr4z6C2Z4HA1mywIDAQABAoIBADyMJPGqLksiYlOSeNm1dSk+MCm5CFgUmMVQchAyCaqJsdfAQ3sk/hEYrVmm0CGs2K0glOz76b2TBnbW8DRK/5S9XBHoModevc3J7QkcmbSgpCflb2I8pxQKV2MMOjEsnu9XpfR/2af1lJLDOWNxe/GawjzVeQVSr2e8QGCTQy8Pb+fqdDzgc1ps+iuva4OjUAjO2J2HG/7MaL9TaYct/j555LyXcOoX/j6lEZWrx5CfKzhHVMr7CqwkRzKAqlJ4pP+WDglzuiQjpiMF1ysT9fgcw8dKbJ08tRFFux97nDoU4DoVmxjcOaMuWZngmW3S9aRl47EoGf/KDRdD681SEiECgYEAyuiuqBa4cLQqt4G0D4kUM6FoYkA5An5MFngOWYpUHem5cQUP+gyoZjOtyHpMgP8vp6jkcgArVc5KcA1TDP/VPZNI2eMPEVa732dNHNpCsvaDYGCjBK0iMzqUDBzQxdf6IuOHAR2u8JgaCQiWNOta9Gq8mB7oqbmWs6Mv+A7QUxsCgYEArkkTRhoJg5VUSPIciD6FHCxgJkbzo4vpD39yrFwgFzUyN2fTAoaH3DTw0ZKX3Vn43j+C4RqMbg/ou6WsEh0FaIRG0PfvfBPuUSmw7EwR+VsB7d8rMFlAe65gV6Lkhnx0ssaEKz0PRPorp8YxsCW3qLGajktLfhTFS5LCVt3jxhECgYBhunIs2UyaU9xjKCBefypwt6P79my+66+f/tEChWKAScscSVDpoWEWYHuqHGVul/oO6YSl97jnigVGNNS0ZSACmUa0Uu01761rK2jzpZgMdDjQmZrKrMzVGbimoNZZA+4hEa87dV2F2exoP2+BK35STHprVi+/V3jjKoz3D8N80wKBgDEVYXk3sgA92QvmK8TvHpvNn5KIHuurZoq30PrbwynmEGtsMRAgBTkuK7hJ+b0cLqug5WIyEOSaVGgsg2Zih/hkcHE/slGWZ2KyRCJ4VgOM7uEoHcizCicK+BUWShfaCx+iKuzmeFtrvUm11p6OMMgwpJ94cx1iCiJ7hYHjYdeRAoGBAIriShl2edrGBEVwZGHIfM7rTKqOJ+ZgUmpoZB72MaN/mtC2Nr2ThGC5lVH2mf+SkhuMwyWYmc1+T9HTWYep5BaQ42vfqcDWl7/dIlU8Sr/FclXLZZjELu2roIzJLPbP1/VYvAkVXweAgPIdezsrm3n/dqvTY1iNo7LU8Cbbawbv";
                options.SignType = "RSA2";
                options.Uid = "商户ID";
            });

            return builder.AddOAuth<AlipayOptions, AlipayHandler>(authenticationScheme, displayName, configureOptions);
        }
    }
}
