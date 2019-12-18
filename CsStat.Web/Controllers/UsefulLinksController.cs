using System;
using System.ComponentModel.DataAnnotations.Schema;
using System.Web.Mvc;
using BusinessFacade.Repositories;
using CsStat.Domain.Entities;
using CsStat.Web.Helpers;
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
        private static UserRegistrationService _registrationService;
        private static IUserRepository _userRepository;

        public UsefulLinksController(IUsefulLinkRepository usefulLinkRepository, IUserRepository userRepository)
        {
            _usefulLinkRepository = usefulLinkRepository;
            var connectionString = new ConnectionStringFactory();
            var mongoRepository = new MongoRepositoryFactory(connectionString);
            _logger = new Logger(mongoRepository);
            _userRepository = userRepository;
            _registrationService = new UserRegistrationService(_userRepository);
        }
        public ActionResult Index()
        {
            var model = new UsefulLinksViewModel
            {
                Items = _usefulLinkRepository.GetAll(),
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
        public ActionResult Add(UsefulInfo info)
        {
            if (info != null)
            {
                try
                {
                    if (info.Id.IsNullOrWhiteSpace())
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

        public ActionResult SignIn()
        {
            return View("~/Views/UsefulInfo/SignInForm.cshtml");
        }

        [HttpPost]
        public ActionResult SignIn(SignInViewModel userModel)
        {
            if (_registrationService.SignIn(userModel))
            {

            }

            return RedirectToAction("Index", "UsefulLinks");
        }
    }
}