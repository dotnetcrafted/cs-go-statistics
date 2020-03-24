using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using AutoMapper;
using BusinessFacade.Repositories;
using CsStat.Domain.Entities;
using CsStat.StrapiApi;
using CsStat.Web.Models;
using DataService;
using ErrorLogger;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Hosting;
using DataService.Interfaces;

namespace CsStat.Web.Controllers
{
    public class WikiController : BaseController
    {
        // GET
        private readonly IWebHostEnvironment _hostingEnvironment;
        private static IUsefulLinkRepository _usefulLinkRepository;
        private static IStrapiApi _strapiApi;
        private static ILogger _logger;
        private readonly IMapper _mapper;

        public WikiController(IUsefulLinkRepository usefulLinkRepository, IStrapiApi strapiApi, IMapper mapper, IWebHostEnvironment hostingEnvironment, ILogger logger)
        {
            _mapper = mapper;
            _hostingEnvironment = hostingEnvironment;
            _usefulLinkRepository = usefulLinkRepository;
            _logger = logger;
            _strapiApi = strapiApi;
        }
        public ActionResult GetAllArticlesFromCms()
        {
            var json = _strapiApi.GetArticles();

            return new JsonResult(json);
        }

        #region CustomAdmin

        public ActionResult Index()
        {
            var isAdminMode = HttpContext.Session.GetString("IsAdminMode") != null && HttpContext.Session.GetString("IsAdminMode").ToString() == "true";
            var usefulInfos = _usefulLinkRepository.GetAll().OrderByDescending(x => x.PublishDate);
            var model = new UsefulLinksViewModel
            {
                Items = usefulInfos != null ? _mapper.Map<List<InfoViewModel>>(usefulInfos) : new List<InfoViewModel>(),
                IsAdminMode = isAdminMode
            };
            return View("~/Views/UsefulInfo/Index.cshtml", model);
        }

        public ActionResult GetInfo()
        {
            var isAdminMode = HttpContext.Session.GetString("IsAdminMode") != null && HttpContext.Session.GetString("IsAdminMode").ToString() == "true";
            var usefulInfos = _usefulLinkRepository.GetAll()?.OrderByDescending(x => x.PublishDate);
            var model = new UsefulLinksViewModel
            {
                Items = usefulInfos != null ? _mapper.Map<List<InfoViewModel>>(usefulInfos) : new List<InfoViewModel>(),
                IsAdminMode = isAdminMode
            };
            return new JsonResult(model);
        }

        public ActionResult Add(string id="")
        {
            var isAdminMode = HttpContext.Session.GetString("IsAdminMode") != null && HttpContext.Session.GetString("IsAdminMode").ToString() == "true";
            
            if (!isAdminMode)
            {
                return RedirectToAction("SignIn", "SignIn");
            }

            if (!string.IsNullOrWhiteSpace(id))
            {
                var info = _usefulLinkRepository.GetInfo(id);
                return View("~/Views/UsefulInfo/AddForm.cshtml", info);
            }

            return View("~/Views/UsefulInfo/AddForm.cshtml", new UsefulInfo());
        }
        
        [HttpPost]
        public ActionResult Save(InfoViewModel infoModel)
        {
            if (infoModel != null)
            {
                var imageName = Path.GetFileName(infoModel.Image?.FileName);
                if (imageName != null)
                {
                    var imagesPath = Path.Combine(_hostingEnvironment.WebRootPath, "~/Files/Images");
                    Directory.CreateDirectory(imagesPath);
                    var path = Path.Combine(imagesPath, imageName);
                    using (var fileStream = new FileStream(path, FileMode.Create))
                    {
                        infoModel.Image.CopyTo(fileStream);
                    }
                    infoModel.ImagePath = imageName;
                }
                var info = _mapper.Map<UsefulInfo>(infoModel);
                try
                {
                    if (string.IsNullOrWhiteSpace(infoModel.Id))
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

        #endregion
    }
}