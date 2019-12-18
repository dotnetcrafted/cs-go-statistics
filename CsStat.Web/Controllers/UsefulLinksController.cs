﻿using System;
using System.IO;
using System.Web.Mvc;
using AutoMapper;
using BusinessFacade.Repositories;
using CsStat.Domain.Entities;
using CsStat.Web.Models;
using DataService;
using ErrorLogger;
using Microsoft.Ajax.Utilities;

namespace CsStat.Web.Controllers
{
    public class UsefulLinksController : BaseController
    {
        // GET
        private static IUsefulLinkRepository _usefulLinkRepository;
        private static ILogger _logger;

        public UsefulLinksController(IUsefulLinkRepository usefulLinkRepository)
        {
            _usefulLinkRepository = usefulLinkRepository;
            var connectionString = new ConnectionStringFactory();
            var mongoRepository = new MongoRepositoryFactory(connectionString);
            _logger = new Logger(mongoRepository);
        }
        public ActionResult Index()
        {
            var isAdminMode = Session["IsAdminMode"] != null && Session["IsAdminMode"].ToString() == "true";
            var model = new UsefulLinksViewModel
            {
                Items = _usefulLinkRepository.GetAll(),
                IsAdminMode = isAdminMode
            };
            return View("~/Views/UsefulInfo/Index.cshtml", model);
        }
        public ActionResult Add(string id="")
        {
            if (!id.IsNullOrWhiteSpace())
            {
                var info = _usefulLinkRepository.GetInfo(id);
                return View("~/Views/UsefulInfo/_AddForm.cshtml", info);
            }

            return View("~/Views/UsefulInfo/_AddForm.cshtml", new UsefulInfo());
        }

        [HttpPost]
        public ActionResult Save(InfoViewModel infoModel)
        {
            if (infoModel != null)
            {
                var imageName = Path.GetFileName(infoModel.Image?.FileName);
                if (imageName != null)
                {
                    string path = Path.Combine(Server.MapPath("~/Files/Images"), imageName);
                    infoModel.Image.SaveAs(path);
                    infoModel.ImagePath = imageName;
                }
                var info = Mapper.Map<UsefulInfo>(infoModel);
                try
                {
                    if (infoModel.Id.IsNullOrWhiteSpace())
                    {
                        _usefulLinkRepository.Add(info);
                    }
                    else
                    {
                        _usefulLinkRepository.Update(info.Id, info);
                    }
                }
                catch (Exception ex)
                {
                    _logger.Error(ex,"Error occurred during saving info");
                }
            }

            return RedirectToAction("Index");
        }

        public ActionResult Remove(string id)
        {
            _usefulLinkRepository.Remove(id);
            return RedirectToAction("Index");
        }
    }
}