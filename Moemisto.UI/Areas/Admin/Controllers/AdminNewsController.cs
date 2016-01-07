using System;
using System.Collections.Generic;
using System.Drawing.Imaging;
using System.Globalization;
using System.IO;
using System.Linq;
using System.Web;
using System.Web.Mvc;
using AutoMapper;
using Moemisto.Data.Contexts.Admin;
using Moemisto.Data.Entities;
using Moemisto.UI.Areas.Admin.Models;
using Moemisto.UI.Helpers;
using Moemisto.UI.Models;

namespace Moemisto.UI.Areas.Admin.Controllers
{
    [RequireHttps]
    [Authorize(Roles = "Admin")]
    public class AdminNewsController : Controller
    {
        private readonly AdminNewsContext _context;

        public AdminNewsController(AdminNewsContext context)
        {
            _context = context;
        }

        public ActionResult Index()
        {
            ViewBag.Title = "Новини/Статті";
            var model = new AdminNewsVm
            {
                NewsTypes = Enum.GetNames(typeof(ArticleType)).Select(s => new SelectListItem { Text = s.ToString(), Value = s.ToString() }).ToList()
                //_context.GetNewsTypes(DateTime.Now).Select(s => new SelectListItem { Text = s.ToString(), Value = s.ToString() }).ToList()
            };

            return View(model);
        }

        [HttpGet]
        public ActionResult NewsList(string publishDate, ArticleType newsType = ArticleType.News)
        {
            var format = new CultureInfo("uk-UA");
            DateTime publish = publishDate == null ? DateTime.Now : DateTime.Parse(publishDate, format);
            var modelDb = _context.GetNewsByDateType(publish, newsType);
            var model = Mapper.Map<List<ArticleBaseVm>>(modelDb);
            return PartialView("_NewsList", model);
        }

        [HttpPost]
        public ActionResult CreateNews()
        {
            int articleId = _context.CreateEmptyArticle();
            return RedirectToAction("NewsEdit", new {id = articleId});
        }

        [HttpGet]
        public ActionResult NewsEdit(int id)
        {
            ViewBag.Title = "Редагування / Створення";
            var model = Mapper.Map<AdminNewsEditVm>(_context.GetArticle(id));

            model.NewsCategoryTypes =
                _context.GetNewsCategory()
                    .ToList()
                    .Select(s => new SelectListItem { Text = String.Format("{0} - {1}", s.Type.ToString(), s.Name), Value = s.CategoryId.ToString() })
                    .ToList();

            return View(model);
        }

        [HttpPost]
        public ActionResult NewsEdit(ArticleBaseVm model)
        {
            var article = Mapper.Map<Article>(model);
            _context.SaveArticle(article);
            return Content("true");
        }
        [HttpPost]
        public ActionResult NewsRemove(int id)
        {
            var result = _context.RemoveArticle(id);
            return Content(result.ToString());
        }

        #region Manage Pictures


        /// <summary>
        /// Get picture list for Html Editor
        /// </summary>
        /// <param name="articleId"></param>
        /// <returns></returns>
        [HttpGet]
        public JsonResult GetPictureList(int articleId)
        {
            var model = _context.GetPictureList(articleId, false).Select(sp => new { title = sp.Title, value = String.Format("{0}{1}", sp.Path, sp.FileName) }).ToList();
            return Json(model, JsonRequestBehavior.AllowGet);
        }
        public ActionResult PictureList(int articleId)
        {
            var model = Mapper.Map<List<AdminPictureItemVm>>(_context.GetPictureList(articleId, true).ToList());
            return PartialView("_PictureList", model);
        }

        public ActionResult PicturePreView(int pictureId)
        {
            var model = Mapper.Map<AdminPictureItemVm>(_context.GetPicture(pictureId) ?? new Picture());
            return PartialView("_PicturePreView", model);
        }

        [HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult AddPictures(int articleId, HttpPostedFileBase pictureTop,
            IEnumerable<HttpPostedFileBase> pictureText)
        {
            DateTime today = DateTime.Now;
            string articlePath = string.Format("/Images/{0}/{1}/{2}/{3}/", today.Year, today.Month, today.Day, articleId);
            var path = Server.MapPath(articlePath);
            if (pictureTop != null && pictureTop.ContentLength > 0)
            {
                // extract only the fielname
                var fileName = string.Format("{0}_top{1}", Path.GetFileNameWithoutExtension(pictureTop.FileName), Path.GetExtension(pictureTop.FileName));
                var fileNameH = string.Format("{0}_ts{1}", Path.GetFileNameWithoutExtension(pictureTop.FileName), Path.GetExtension(pictureTop.FileName));
                var fileNameS = string.Format("{0}_th{1}", Path.GetFileNameWithoutExtension(pictureTop.FileName), Path.GetExtension(pictureTop.FileName));
                if (!Directory.Exists(path))
                {
                    Directory.CreateDirectory(path);
                }
                pictureTop.SaveAs(Path.Combine(Server.MapPath(articlePath), fileName));
                // Resize picture
                using (var smallPicture = PictureResizer.ResizeImage(pictureTop.InputStream, 260))
                {
                    smallPicture.Save(Path.Combine(Server.MapPath(articlePath), fileNameH), ImageFormat.Jpeg);
                }
                using (var topPicture = PictureResizer.ResizeImage(pictureTop.InputStream, 500))
                {
                    topPicture.Save(Path.Combine(Server.MapPath(articlePath), fileNameS), ImageFormat.Jpeg);
                }

                _context.AddPicture(articleId, Path.GetFileNameWithoutExtension(fileName), articlePath, fileName, fileNameH, fileNameS, true);
            }
            if (pictureText != null)
            {
                foreach (var picture in pictureText)
                {
                    // Verify that the user selected a file
                    if (picture != null && picture.ContentLength > 0)
                    {
                        var fileName = Path.GetFileName(picture.FileName);
                        if (!Directory.Exists(path))
                        {
                            Directory.CreateDirectory(path);
                        }
                        if (!string.IsNullOrEmpty(fileName))
                        {
                            picture.SaveAs(Path.Combine(Server.MapPath(articlePath), fileName));
                            _context.AddPicture(articleId, Path.GetFileNameWithoutExtension(fileName), articlePath,
                                fileName);
                        }
                    }
                }
            }
            return RedirectToAction("PictureList", new { articleId });
        }


        public ActionResult RemovePicture(int pictureId)
        {
            bool res = _context.RemovePicture(pictureId);

            return Content(res.ToString());
        }
        #endregion

    }
}