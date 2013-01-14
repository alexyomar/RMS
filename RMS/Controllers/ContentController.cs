using System;
using System.Collections.Generic;
using System.Data;
using System.Data.Entity;
using System.Linq;
using System.Web;
using System.Web.Mvc;
using RMS.Models;
using System.IO;

namespace RMS.Controllers
{
    public class ContentController : Controller
    {
        private RegionalEntities db = new RegionalEntities();

        //
        // GET: /Content/

        public ViewResult Index(int Id)
        {
            var content = db.Content.Include("ContentType").Include("Country");
            ViewBag.IdContent = Id;
            ViewBag.Name = db.ContentType.Where(u => u.Id.Equals(Id)).Single().Name;
            return View(content.Where(u => u.IdContentType.Equals(Id)).ToList());
        }

        //
        // GET: /Content/Details/5

        public ViewResult Details(int id)
        {
            Content content = db.Content.Single(c => c.Id == id);
            return View(content);
        }

        //
        // GET: /Content/Create

        public ActionResult Create(int Id)
        {
            ViewBag.IdContentType = new SelectList(db.ContentType, "Id", "Name", Id);
            ViewBag.IdCountry = new SelectList(db.Country, "Id", "Country1");
            ViewBag.IdContent = Id;
            return View();
        }

        //
        // POST: /Content/Create

        [HttpPost]
        public ActionResult Create(Content content, IEnumerable<HttpPostedFileBase> images, HttpPostedFileBase pdf, HttpPostedFileBase featured)
        {
            if (ModelState.IsValid)
            {
                content.Timestamp = DateTime.Now;
                content.TimestampEdit = DateTime.Now;
                db.Content.AddObject(content);
                db.SaveChanges();

                bool IsExist = Directory.Exists(Server.MapPath("~/Content/Upload/" + content.IdContentType + "/" + content.Id));
                if (!IsExist)
                    Directory.CreateDirectory(Server.MapPath("~/Content/Upload/" + content.IdContentType + "/" + content.Id));

                if (featured != null)
                {
                    var filename = Guid.NewGuid();
                    var path = Server.MapPath("~/Content/Upload/" + content.IdContentType + "/" + content.Id) + @"\" + filename.ToString().Substring(0, 8) + ".jpg";
                    featured.SaveAs(path);
                    ContentImages __newimage = new ContentImages() { FileGUID = filename, IdContent = content.Id, URL = "/Content/Upload/" + content.IdContentType + "/" + content.Id + "/" + filename.ToString().Substring(0, 8) + ".jpg", IsDefault = true };
                    db.ContentImages.AddObject(__newimage);
                    db.SaveChanges();
                }

                foreach (var item in images)
                {
                    if (item != null)
                    {
                        var filename = Guid.NewGuid();
                        var path = Server.MapPath("~/Content/Upload/" + content.IdContentType + "/" + content.Id) + @"\" + filename.ToString().Substring(0, 8) + ".jpg";
                        item.SaveAs(path);
                        ContentImages __newimage = new ContentImages() { FileGUID = filename, IdContent = content.Id, URL = "/Content/Upload/" + content.IdContentType + "/" + content.Id + "/" + filename.ToString().Substring(0, 8) + ".jpg", IsDefault = false };
                        db.ContentImages.AddObject(__newimage);
                        db.SaveChanges();
                    }
                }

                if (pdf != null)
                {
                    var filename = Guid.NewGuid();
                    var path = Server.MapPath("~/Content/Upload/" + content.IdContentType + "/" + content.Id) + @"\" + filename.ToString().Substring(0, 8) + ".pdf";
                    pdf.SaveAs(path);
                    ContentFiles __newPDF = new ContentFiles() { FileGUID = filename, IdContent = content.Id, URL = "/Content/Upload/" + content.IdContentType + "/" + content.Id + "/" + filename.ToString().Substring(0, 8) + ".pdf", IsDefault = true };
                    db.ContentFiles.AddObject(__newPDF);
                    db.SaveChanges();
                }

                return RedirectToAction("Index", new { Id = content.IdContentType });
            }

            ViewBag.IdContentType = new SelectList(db.ContentType, "Id", "Name", content.IdContentType);
            ViewBag.IdCountry = new SelectList(db.Country, "Id", "Country1", content.IdCountry);
            return View(content);
        }

        //
        // GET: /Content/Edit/5

        public ActionResult Edit(int id)
        {
            Content content = db.Content.Single(c => c.Id == id);
            ViewBag.IdContentType = new SelectList(db.ContentType, "Id", "Name", content.IdContentType);
            ViewBag.IdCountry = new SelectList(db.Country, "Id", "Country1", content.IdCountry);

            ContentMedia __media = new ContentMedia();

            List<ContentImages> __images = db.ContentImages.Where(u => u.IsDefault.Equals(false) && u.IdContent.Equals(id)).OrderBy(u => u.Id).ToList();

            try { __media.image = db.ContentImages.Where(u => u.IsDefault.Equals(true) && u.IdContent.Equals(id)).FirstOrDefault().URL; }
            catch { __media.image = string.Empty; }
            try { __media.img1 = __images[0].URL; }
            catch { __media.img1 = string.Empty; }
            try { __media.img2 = __images[1].URL; }
            catch { __media.img2 = string.Empty; }
            try { __media.img3 = __images[2].URL; }
            catch { __media.img3 = string.Empty; }
            try { __media.img4 = __images[3].URL; }
            catch { __media.img4 = string.Empty; }
            try { __media.pdf = db.ContentFiles.Where(u => u.IdContent.Equals(id)).FirstOrDefault().URL; }
            catch { __media.pdf = string.Empty; }

            ViewBag.Media = __media;

            return View(content);
        }

        //
        // POST: /Content/Edit/5

        [HttpPost]
        public ActionResult Edit(Content content, IEnumerable<HttpPostedFileBase> images, HttpPostedFileBase pdf, HttpPostedFileBase featured, ContentMedia MetaData)
        {
            if (ModelState.IsValid)
            {

                content.TimestampEdit = DateTime.Now;
                
                db.Content.Attach(content);
                db.ObjectStateManager.ChangeObjectState(content, EntityState.Modified);
                db.SaveChanges();

                if (featured != null)
                {
                    var filename = Guid.NewGuid();
                    var path = Server.MapPath("~/Content/Upload/" + content.IdContentType + "/" + content.Id) + @"\" + filename.ToString().Substring(0, 8) + ".jpg";
                    featured.SaveAs(path);
                    ContentImages __newimage = new ContentImages() { FileGUID = filename, IdContent = content.Id, URL = "/Content/Upload/" + content.IdContentType + "/" + content.Id + "/" + filename.ToString().Substring(0, 8) + ".jpg", IsDefault = true };

                    var __todelete = db.ContentImages.Where(u => u.URL.Contains(MetaData.image)).FirstOrDefault();
                    if (__todelete != null)
                    {
                        db.ContentImages.DeleteObject(__todelete);
                        db.SaveChanges();
                    }

                    db.ContentImages.AddObject(__newimage);
                    db.SaveChanges();
                }
                int i = 1;
                
                foreach (var item in images)
                {
                    if (item != null)
                    {
                        var filename = Guid.NewGuid();
                        var path = Server.MapPath("~/Content/Upload/" + content.IdContentType + "/" + content.Id) + @"\" + filename.ToString().Substring(0, 8) + ".jpg";
                        item.SaveAs(path);
                        ContentImages __newimage = new ContentImages() { FileGUID = filename, IdContent = content.Id, URL = "/Content/Upload/" + content.IdContentType + "/" + content.Id + "/" + filename.ToString().Substring(0, 8) + ".jpg", IsDefault = false };

                        string namefile = string.Empty;

                        switch (i)
                        {

                            case 1:
                                namefile = MetaData.img1;
                                break;
                            case 2:
                                namefile = MetaData.img2;
                                break;
                            case 3:
                                namefile = MetaData.img3;
                                break;
                            case 4:
                                namefile = MetaData.img4;
                                break;
                            default:
                                namefile = MetaData.img1;
                                break;
                        }

                        var __todelete = db.ContentImages.Where(u => u.URL.Contains(namefile)).FirstOrDefault();
                        if (__todelete != null)
                        {
                            db.ContentImages.DeleteObject(__todelete);
                            db.SaveChanges();
                        }

                       

                        db.ContentImages.AddObject(__newimage);
                        db.SaveChanges();
                    }
                    i = i + 1;
                }

                if (pdf != null)
                {
                    var filename = Guid.NewGuid();
                    var path = Server.MapPath("~/Content/Upload/" + content.IdContentType + "/" + content.Id) + @"\" + filename.ToString().Substring(0, 8) + ".pdf";
                    pdf.SaveAs(path);
                    ContentFiles __newPDF = new ContentFiles() { FileGUID = filename, IdContent = content.Id, URL = "/Content/Upload/" + content.IdContentType + "/" + content.Id + "/" + filename.ToString().Substring(0, 8) + ".pdf", IsDefault = true };

                    var __todelete = db.ContentFiles.Where(u => u.URL.Contains(MetaData.pdf)).FirstOrDefault();
                    if (__todelete != null)
                    {
                        db.ContentFiles.DeleteObject(__todelete);
                        db.SaveChanges();
                    }

                    db.ContentFiles.AddObject(__newPDF);
                    db.SaveChanges();
                }

                return RedirectToAction("Index", new  { Id = content.IdContentType });
            }

            

            ViewBag.IdContentType = new SelectList(db.ContentType, "Id", "Name", content.IdContentType);
            ViewBag.IdCountry = new SelectList(db.Country, "Id", "Country1", content.IdCountry);
            return View(content);
        }

        //
        // GET: /Content/Delete/5

        public ActionResult Delete(int id)
        {
            Content content = db.Content.Single(c => c.Id == id);
            List<ContentImages> __images = db.ContentImages.Where(u => u.IsDefault.Equals(false) && u.IdContent.Equals(id)).OrderBy(u => u.Id).ToList();
            ContentMedia __media = new ContentMedia();

            try { __media.image = db.ContentImages.Where(u => u.IsDefault.Equals(true) && u.IdContent.Equals(id)).FirstOrDefault().URL; }
            catch { __media.image = string.Empty; }
            try { __media.img1 = __images[0].URL; }
            catch { __media.img1 = string.Empty; }
            try { __media.img2 = __images[1].URL; }
            catch { __media.img2 = string.Empty; }
            try { __media.img3 = __images[2].URL; }
            catch { __media.img3 = string.Empty; }
            try { __media.img4 = __images[3].URL; }
            catch { __media.img4 = string.Empty; }
            try { __media.pdf = db.ContentFiles.Where(u => u.IdContent.Equals(id)).FirstOrDefault().URL; }
            catch { __media.pdf = string.Empty; }

            ViewBag.Media = __media;
            return View(content);
        }

        //
        // POST: /Content/Delete/5

        [HttpPost, ActionName("Delete")]
        public ActionResult DeleteConfirmed(int id)
        {
            Content content = db.Content.Single(c => c.Id == id);
            List<ContentFiles> files = db.ContentFiles.Where(c => c.IdContent == id).ToList();
            List<ContentImages> images = db.ContentImages.Where(c => c.IdContent == id).ToList();

            int back = content.IdContentType;

            foreach (var item in files)
            {
                db.ContentFiles.DeleteObject(item);
                db.SaveChanges();
            }

            foreach (var item in images)
            {
                db.ContentImages.DeleteObject(item);
                db.SaveChanges();
            }


            db.Content.DeleteObject(content);
            db.SaveChanges();

            return RedirectToAction("Index", new { Id = back });
        }

        protected override void Dispose(bool disposing)
        {
            db.Dispose();
            base.Dispose(disposing);
        }
    }
}