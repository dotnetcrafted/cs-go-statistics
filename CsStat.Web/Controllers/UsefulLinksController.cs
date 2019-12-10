using System.Web.Mvc;
using BusinessFacade.Repositories;
using CsStat.Domain.Entities;

namespace CsStat.Web.Controllers
{
    public class UsefulLinksController : BaseController
    {
        // GET
        private IUsefulLinkRepository _usefulLinkRepository;

        public UsefulLinksController(IUsefulLinkRepository usefulLinkRepository)
        {
            _usefulLinkRepository = usefulLinkRepository;
        }
        public ActionResult Admin()
        {
            var model = _usefulLinkRepository.GetAll();
            return View("~/Views/UsefulInfo/AdminView.cshtml", model);
        }
    }
}