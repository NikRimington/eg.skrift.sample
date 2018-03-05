﻿using System;
using System.Web.Http;
using System.Web.Mvc;
using Autofac;
using Autofac.Integration.Mvc;
using Autofac.Integration.WebApi;
using eg.skrift.businesslogic.Factories;
using eg.skrift.businesslogic.Services;
using eg.skrift.data.Controllers;
using eg.skrift.data.Factories;
using eg.skrift.data.Models;
using Umbraco.Web;

namespace eg.skrift.data.CMS
{
    public class Global : UmbracoApplication
    { 
        /// <summary>
        /// Registers our controllers and Umbraco controllers, plus the types we need to resolve, 
        /// and sets up MVC to use Autofac as a dependency resolver as our DI Container of choice
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        protected override void OnApplicationStarted(object sender, EventArgs e)
        {
            base.OnApplicationStarted(sender, e);
            var builder = new ContainerBuilder();
            builder.RegisterControllers(typeof(UmbracoApplication).Assembly);
            builder.RegisterApiControllers(typeof(UmbracoApplication).Assembly);
            builder.RegisterApiControllers(typeof(Global).Assembly);
            builder.RegisterType<UmbracoLoggerFactory>()
                .As<ILoggerFactory>();
            builder.RegisterType<UmbracoContentFetcher>()
                .As<IUmbracoContentFetcher>()
                .WithParameter((paramInfo, ctx) => paramInfo.Name == "umbracoHelper",
                    (paramInfo, ctx) => new UmbracoHelper(UmbracoContext.Current));
            builder.RegisterType<UmbracoContentFetcherFactory>()
                .As<IContentFetcherFactory>();
            builder.RegisterType<RatingsServiceFactory>()
                .As<IRatingsServiceFactory>();
            var container = builder.Build();
            DependencyResolver.SetResolver(new AutofacDependencyResolver(container));
            GlobalConfiguration.Configuration.DependencyResolver = new AutofacWebApiDependencyResolver(container);
        }
    }

    public class PrerecordedEventController
    {
    }
}