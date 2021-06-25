using SampleApp.Helpers;
using System;
using System.Collections.Generic;
using System.Data;
using System.Linq;
using System.Web;
using System.Web.Mvc;

namespace SampleApp.Controllers
{    
    //[CustomExceptionFilter]
    public class HomeController : Controller
    {
        DBHelper DB = new DBHelper();
        public ActionResult Index()
        {
            DataTable dt = DB.GetDataSetProc("GetAllItems").Tables[0];
            List<Inventory> inventory = new List<Inventory>();
            inventory = ClsGlobal.ConvertDataTable<Inventory>(dt);
            return View(inventory);
        }

        [HttpPost]
        public JsonResult InsertInventory(Inventory inventory)
        {
            ErrorInfo ei = new ErrorInfo();
            try
            {
                DataTable dt=DB.ExecDataSetProc("AddProductInfo", "@ProductName", inventory.ItemName, "@ProductDescription", inventory.Description, "@Price", inventory.Price).Tables[0];
                ei.ErrorCode = dt.Rows[0]["ErrorCode"].ToString();
                ei.ErrorDesc= dt.Rows[0]["ErrorDesc"].ToString();
            }
            catch (Exception ex)
            {
                //throw ex;
                ei.ErrorCode = "1";
                ei.ErrorDesc = ex.Message.ToString();
            }
            //return Json(inventory);
            return Json(ei, JsonRequestBehavior.AllowGet);
        }
        [HttpPost]
        public JsonResult UpdateInventory(Inventory inventory)
        {
            ErrorInfo ei = new ErrorInfo();
            try
            {
                DataTable dt = DB.ExecDataSetProc("UpdateProductInfo","@ID", inventory.ItemCode, "@ProductName", inventory.ItemName, "@ProductDescription", inventory.Description, "@Price", inventory.Price).Tables[0];
                ei.ErrorCode = dt.Rows[0]["ErrorCode"].ToString();
                ei.ErrorDesc = dt.Rows[0]["ErrorDesc"].ToString();
            }
            catch (Exception ex)
            {
                //throw ex;
                ei.ErrorCode = "1";
                ei.ErrorDesc = ex.Message.ToString();
            }
            //return new EmptyResult();
            return Json(ei, JsonRequestBehavior.AllowGet);
        }

        [HttpPost]
        public JsonResult DeleteInventory(int itemCode)
        {
            ErrorInfo ei = new ErrorInfo();
            try
            {
                DataTable dt = DB.ExecDataSetProc("DeleteProductInfo", "@ID", itemCode).Tables[0];
                ei.ErrorCode = dt.Rows[0]["ErrorCode"].ToString();
                ei.ErrorDesc = dt.Rows[0]["ErrorDesc"].ToString();
            }
            catch (Exception ex)
            {
                //throw ex;
                ei.ErrorCode = "1";
                ei.ErrorDesc = ex.Message.ToString();
            }
            //return new EmptyResult(); 
            return Json(ei, JsonRequestBehavior.AllowGet);
        }

        public ActionResult About()
        {
            ViewBag.Message = "Your application description page.";

            return View();
        }

        public ActionResult Contact()
        {
            ViewBag.Message = "Your contact page.";

            return View();
        }
    }
}