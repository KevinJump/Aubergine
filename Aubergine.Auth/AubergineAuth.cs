﻿using Aubergine.Core;
using Semver;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Umbraco.Core;
using Umbraco.Web.Routing;

namespace Aubergine.Auth
{
    public class AubergineAuth : ApplicationEventHandler, IAubergineExtension
    {
        private readonly SemVersion targetVersion = new SemVersion(1, 0, 0);

        public string Name => "Authentication";
        public string ExtensionId => "{7CD40983-07FC-48BE-B6F9-B9CF3C85DBAB}";
        public string Version => targetVersion.ToString();
        public string ProductName => Authentication.Name;

        protected override void ApplicationStarting(UmbracoApplicationBase umbracoApplication, ApplicationContext applicationContext)
        {
            ContentFinderResolver.Current.InsertTypeBefore<ContentFinderByNotFoundHandlers, AuthContentFinder>();
        }
    }

    public static class Authentication
    {
        public const string Name = "Aubergine.Auth";
    }

    public static class AuthUrls
    {
        public const string Reset = "/reset";
        public const string Verify = "/verify";
        public const string Login = "/login";
        public const string Register = "/register";
        public const string logout = "/umbraco/aubergine/authentication/logout";
    }

    public static class Form
    {
        public const string AubAuthKey = "AubAuthForm";
    }

    public static class Views
    {
        public const string ForgotPwd = "Forgot";
        public const string Reset = "Reset";
        public const string Register = "Register";
        public const string Login = "Login";
        public const string Status = "Status";
    }

    public static class Properties
    {
        public const string Verified = "accountVerified";
        public const string ResetGuid = "resetGuid";
        public const string ExpiryDate = "expiryDate";
    }

    public static class TemplateAliases
    {
        public const string Pages = "AuthPages";
        public const string Login = "AuthLogin";
        public const string Reset = "AuthReset";
        public const string Forgot = "AuthForgot";
        public const string Verify = "AuthVerify";
        public const string Register = "AuthRegister";
    }

}
