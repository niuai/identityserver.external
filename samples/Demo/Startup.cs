using Microsoft.AspNetCore.Authentication.QQ;
using Microsoft.AspNetCore.Authentication.WeChat;
using Microsoft.AspNetCore.Builder;
using Microsoft.AspNetCore.Hosting;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Identity;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Logging;
using Demo.Data;
using Demo.Models;
using Demo.Services;
using System;
using AspNetCore.Authentication.Alipay;

namespace Demo
{
    public class Startup
    {
        public Startup(IHostingEnvironment env)
        {
            var builder = new ConfigurationBuilder()
                .SetBasePath(env.ContentRootPath)
                .AddJsonFile("appsettings.json", optional: true, reloadOnChange: true)
                .AddJsonFile($"appsettings.{env.EnvironmentName}.json", optional: true)
                .AddEnvironmentVariables()
                .AddUserSecrets<Startup>();

            Configuration = builder.Build();
        }

        public IConfiguration Configuration { get; }

        // This method gets called by the runtime. Use this method to add services to the container.
        public void ConfigureServices(IServiceCollection services)
        {
            services.AddDbContext<ApplicationDbContext>(options =>
                options.UseSqlServer(Configuration.GetConnectionString("DefaultConnection")));

            services.AddIdentity<ApplicationUser, IdentityRole>()
                .AddEntityFrameworkStores<ApplicationDbContext>()
                .AddDefaultTokenProviders();

            // Add application services.
            services.AddTransient<IEmailSender, EmailSender>();

            services.AddAuthentication().AddMicrosoftAccount(microsoftOptions =>
            {
                microsoftOptions.ClientId = Configuration["Authentication:Microsoft:ApplicationId"];
                microsoftOptions.ClientSecret = Configuration["Authentication:Microsoft:Password"];
            })/*.AddGoogle(googleOptions => //Google
            {
                googleOptions.ClientId = Configuration["Authentication:Google:ClientId"];
                googleOptions.ClientSecret = Configuration["Authentication:Google:ClientSecret"];
            })*/.AddQQ(qqOptions =>
            {
                qqOptions.AppId = Configuration["Authentication:QQ:AppId"];
                qqOptions.AppKey = Configuration["Authentication:QQ:AppKey"];
            }).AddWeChat(wechatOptions =>
            {
                wechatOptions.AppId = Configuration["Authentication:WeChat:AppId"];
                wechatOptions.AppSecret = Configuration["Authentication:WeChat:AppSecret"];
            }).AddAlipay("Alipay", options =>
            {
                options.AppId = "2016101300674410";
                options.AlipayPublicKey = "MIIBIjANBgkqhkiG9w0BAQEFAAOCAQ8AMIIBCgKCAQEAlzrWgal1iF9flTWO9dThRXqa9zAPDk1bsKN0x9WKSiTmxr8VDtCwvO0ScsIxr7LRm2WjbnnL+WSTXi7Y3GsQbxrv5j9OGBTSvbLVi64lQPJSIhNys6XXjAkXnwMD0ICBF8kyy3hHEtOMFr9zwsW5BDlX5RLQkG0Ril9/U57OQCe4IJ5ye+dSmstNpkCKgyXQhjHmNGCLzutqFTZRBxvp5LGu29cN4oywA5eSHts5lt5GbfdocbKU930ZH3z/k54u1eR+hdiSAGh6YQ8DBEvcl58GgfKwvO8gi7+sel6tUHaZHZnuqyFkc1I9ZrnH/07TjYeEor6oC9IHwilSYnjF6wIDAQAB";
                options.AppPrivateKey = "xxxx";
                options.GatewayUrl = "https://openapi.alipaydev.com/gateway.do";
                options.AuthorizationEndpoint = "https://openauth.alipaydev.com/oauth2/publicAppAuthorize.htm";
                options.SignType = "RSA2";
            });

            services.AddMvc();
        }

        // This method gets called by the runtime. Use this method to configure the HTTP request pipeline.
        public void Configure(IApplicationBuilder app, IHostingEnvironment env, ILoggerFactory loggerFactory)
        {
            if (env.IsDevelopment())
            {
                app.UseDeveloperExceptionPage();
                //app.UseBrowserLink();
                app.UseDatabaseErrorPage();
            }
            else
            {
                app.UseExceptionHandler("/Home/Error");
            }

            //loggerFactory.AddConsole();

            app.UseStaticFiles();

            app.UseAuthentication();

            app.UseMvc(routes =>
            {
                routes.MapRoute(
                    name: "default",
                    template: "{controller=Home}/{action=Index}/{id?}");
            });
        }
    }
}
