using System;
using System.Collections.Generic;
using System.Data;
using System.Data.Entity;
using System.IO;
using System.Linq;
using System.Net;
using System.Web;
using System.Web.Mvc;
using HDBMMVC.Models;
using Microsoft.AspNet.Identity;
using NPOI.HSSF.UserModel;
using NPOI.SS.UserModel;
using OfficeOpenXml;
using QXHMVC.Models;

namespace HDBMMVC.Controllers
{
    public class ActivitiesController : Controller
    {
        private DbModel db = new DbModel();

        // GET: Activities
        public ActionResult Index()
        {
            return View();
        }
        public ActionResult MyIndex()
        {
            string userid = User.Identity.GetUserId().ToString();
            return View(db.Activities.Where(r => r.UserId == userid).ToList());
        }
        [Authorize(Roles ="Admin")]
        public ActionResult Admin()
        {
            return View(db.Activities.ToList());
        }

        // GET: Activities/Details/5
        public ActionResult Details(int? id)
        {
            if (id == null)
            {
                return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
            }
            Activity activity = db.Activities.Find(id);
            if (activity == null)
            {
                return HttpNotFound();
            }
            if (activity.UserId != User.Identity.GetUserId().ToString() && !User.IsInRole("Admin"))
            {
                return HttpNotFound();
            }
            return View(activity);
        }

        // GET: Activities/Create
        public ActionResult Create()
        {
            return View();
        }

        // POST: Activities/Create
        // 为了防止“过多发布”攻击，请启用要绑定到的特定属性；有关
        // 更多详细信息，请参阅 https://go.microsoft.com/fwlink/?LinkId=317598。
        [HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult Create([Bind(Include = "Id,Name,Major,Sno,Phone,Email")] Activity activity)
        {
            if (ModelState.IsValid)
            {
                activity.UserId = User.Identity.GetUserId().ToString();
                activity.CreateId= Guid.NewGuid().ToString();
                db.Activities.Add(activity);
                db.SaveChanges();
                return RedirectToAction("CreateSuccess", new { CreateId = activity.CreateId });
            }

            return View(activity);
        }

        public ActionResult CreateSuccess(string CreateId)
        {
            ViewBag.createid = CreateId;
            return View();
        }

        // GET: Activities/Edit/5
        public ActionResult Edit(int? id)
        {
            if (id == null)
            {
                return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
            }
            Activity activity = db.Activities.Find(id);
            if (activity == null)
            {
                return HttpNotFound();
            }
            if (activity.UserId != User.Identity.GetUserId().ToString() && !User.IsInRole("Admin"))
            {
                return HttpNotFound();
            }
            return View(activity);
        }

        // POST: Activities/Edit/5
        // 为了防止“过多发布”攻击，请启用要绑定到的特定属性；有关
        // 更多详细信息，请参阅 https://go.microsoft.com/fwlink/?LinkId=317598。
        [HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult Edit([Bind(Include = "Id,Name,Major,Sno,Phone,Email")] Activity activity)
        {
            if (ModelState.IsValid)
            {
                db.Entry(activity).State = EntityState.Modified;
                db.SaveChanges();
                return RedirectToAction("Index");
            }
            return View(activity);
        }

        // GET: Activities/Delete/5
        public ActionResult Delete(int? id)
        {
            if (id == null)
            {
                return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
            }
            Activity activity = db.Activities.Find(id);
            if (activity == null)
            {
                return HttpNotFound();
            }
            if (activity.UserId != User.Identity.GetUserId().ToString() && !User.IsInRole("Admin"))
            {
                return HttpNotFound();
            }
            return View(activity);
        }

        // POST: Activities/Delete/5
        [HttpPost, ActionName("Delete")]
        [ValidateAntiForgeryToken]
        public ActionResult DeleteConfirmed(int id)
        {
            Activity activity = db.Activities.Find(id);
            db.Activities.Remove(activity);
            db.SaveChanges();
            return RedirectToAction("Index");
        }

        /// <summary>
        /// 表头的样式
        /// </summary>
        /// <param name="workbook"></param>
        /// <param name="sheet"></param>
        public void CreateHeadStyle(HSSFWorkbook workbook, HSSFSheet sheet)
        {
            //创建行
            HSSFRow row = sheet.CreateRow(0) as HSSFRow;
            //创建列
            var cell0 = row.CreateCell(0);
            var cell1 = row.CreateCell(1);
            var cell2 = row.CreateCell(2);
            var cell3 = row.CreateCell(3);
            var cell4 = row.CreateCell(4);
            var cell5 = row.CreateCell(5);
            var cell6 = row.CreateCell(6);
            cell0.SetCellValue("序号");
            cell1.SetCellValue("姓名");
            cell2.SetCellValue("专业");
            cell3.SetCellValue("学号");
            cell4.SetCellValue("手机");
            cell5.SetCellValue("邮箱");
            cell6.SetCellValue("报名识别码");
            //创建列的样式
            HSSFCellStyle cstyle = workbook.CreateCellStyle() as HSSFCellStyle;
            //设置垂直居中
            cstyle.VerticalAlignment = VerticalAlignment.Center;
            //设置水平居中
            cstyle.Alignment = HorizontalAlignment.Center;

            cell0.CellStyle = cstyle;
            cell1.CellStyle = cstyle;
            cell2.CellStyle = cstyle;
            cell3.CellStyle = cstyle;
            cell4.CellStyle = cstyle;
            cell5.CellStyle = cstyle;
            cell6.CellStyle = cstyle;
        }

        /// <summary>
        /// 导出excel表
        /// </summary>
        public void ToExcel()
        {
            //创建一个Excel表
            HSSFWorkbook workbook = new HSSFWorkbook();

            HSSFSheet sheet = workbook.CreateSheet() as HSSFSheet;

            CreateHeadStyle(workbook, sheet);

            Response.AddHeader("Content-Disposition", "attachment;filename=活动报名表.xls");//导出后的文件名

            //查询数据库

            List<Activity> elist = db.Activities.ToList();
            //遍历集合
            for (int i = 0; i < elist.Count; i++)//遍历表数据
            {
                //创建行
                HSSFRow row = sheet.CreateRow(i + 1) as HSSFRow;
                //创建列
                row.CreateCell(0).SetCellValue(Convert.ToString(elist[i].Id));
                row.CreateCell(1).SetCellValue(Convert.ToString(elist[i].Name));
                row.CreateCell(2).SetCellValue(Convert.ToString(elist[i].Major));
                row.CreateCell(3).SetCellValue(Convert.ToString(elist[i].Sno));
                row.CreateCell(4).SetCellValue(Convert.ToString(elist[i].Phone));
                row.CreateCell(5).SetCellValue(Convert.ToString(elist[i].Email));
                row.CreateCell(6).SetCellValue(Convert.ToString(elist[i].CreateId));
            }

            //内存流
            MemoryStream ms = new MemoryStream();

            workbook.Write(ms);

            Response.BinaryWrite(ms.ToArray());
        } 
        protected override void Dispose(bool disposing)
        {
            if (disposing)
            {
                db.Dispose();
            }
            base.Dispose(disposing);
        }
    }
}
