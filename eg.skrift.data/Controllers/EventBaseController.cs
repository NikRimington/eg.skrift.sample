﻿using System;
using System.Reflection;
using System.Web.Mvc;
using eg.skrift.businesslogic.Factories;
using eg.skrift.businesslogic.Services;
using eg.skrift.data.CMS;
using eg.skrift.data.Models;
using Our.Umbraco.Ditto;
using Umbraco.Web.Models;
using Umbraco.Web.Mvc;

namespace eg.skrift.data.Controllers
{
    public class EventBaseController : RenderMvcController
    {
        internal new ILogger Logger;
        private IRatingsService ratingsService;

        public EventBaseController(ILoggerFactory loggerFactory, IRatingsServiceFactory ratingsServiceFactory)
        {
            if (loggerFactory == null) throw new ArgumentNullException(nameof(loggerFactory));
            InitaliseLogger(loggerFactory);
            if (ratingsServiceFactory == null) throw new ArgumentNullException(nameof(loggerFactory));
            InitaliseRatingsService(ratingsServiceFactory);
        }

        private void InitaliseRatingsService(IRatingsServiceFactory ratingsServiceFactory)
        {
            ratingsService = ratingsServiceFactory.Create();
        }

        private void InitaliseLogger(ILoggerFactory loggerFactory)
        {
            Logger = loggerFactory.Create(MethodBase.GetCurrentMethod().DeclaringType);
        }

        /// <summary>
        /// Renders the Event page
        /// </summary>
        /// <param name="model"></param>
        /// <returns></returns>
        public override ActionResult Index(RenderModel model)
        {
            var typedModel = model.As<EventDetail>();
            if (ratingsService != null) typedModel.EventRating = ratingsService.GetRating(typedModel.Id);
            return CurrentTemplate(typedModel);
        }
    }
}