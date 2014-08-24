using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;
using System.Data.Entity;
using System.Data.Entity.Infrastructure;
using Todolist.Models;
using Todolist.Classes;

namespace Todolist.Controllers
{
    public class HomeController : Controller
    {
        private iTodoListRegistry context;

        public HomeController(iTodoListRegistry context)
        {
            this.context = context;
        }

        public ActionResult Index()
        {
            TodolistModel tdm = new TodolistModel();
            tdm.TDList = context.GetTodoList();
            return View(tdm);
        }

        [HttpPost]
        public JsonResult Add(string item)
        {
            OperationModelResponse omr = new OperationModelResponse();
            if(String.IsNullOrEmpty(item) || item.Length>150)
            {
                omr.Id = 0;
                omr.Message = "Please enter a valid to-do list item, the content of to-do list item cannot be empty.";
            }
            else
            {
                omr.Id = context.AddTodoItem(item);
                omr.Message = "To-do list item has been added successfully.";
            }
            return Json(omr);
        }

        [HttpPost]
        public JsonResult Delete(int id)
        {
            OperationModelResponse omr = new OperationModelResponse();
            if (context.DeleteTodoItem(id))
            {
                omr.Id = id;
                omr.Message = "To-do list item has been deleted successfully.";
            }
            else
            {
                omr.Id = 0;
                omr.Message = "Failed to delete to-do list item.";
            }
            
            return Json(omr);
        }

        [HttpPost]
        public JsonResult Update(int id, string item)
        {
            OperationModelResponse omr = new OperationModelResponse();
            if (String.IsNullOrEmpty(item) || item.Length > 150)
            {
                omr.Id = 0;
                omr.Message = "Please enter a valid to-do list item, the content of to-do list item cannot be empty.";
            }
            else
            {
                if (context.UpdateTodoItem(id, item))
                {
                    omr.Id = id;
                    omr.Message = "To-do list item has been updated successfully.";
                }
                else
                {
                    omr.Id = 0;
                    omr.Message = "Failed to update to-do list item.";
                }
            }

            return Json(omr);
        }

        [HttpPost]
        public JsonResult ReArrange(int[] ids)
        {
            OperationModelResponse omr = new OperationModelResponse();
            if (ids == null || ids.Length < 1)
            {
                omr.Id = 0;
                omr.Message = "Failed to update the priority of to-do list item.";
            }
            else
            {
                if (context.UpdatePriorities(ids))
                {
                    omr.Id = 1;
                    omr.Message = "The priority of to-do item has been updated successfully.";
                }
                else
                {
                    omr.Id = 0;
                    omr.Message = "Failed to update the priority of to-do list item.";
                }
            }
            return Json(omr);
        }

        [HttpPost]
        public JsonResult Complete(int id)
        {
            OperationModelResponse omr = new OperationModelResponse();
            if (context.UpdateComplete(id))
            {
                omr.Id = id;
                omr.Message = "To-do list item has been completed successfully.";
            }
            else
            {
                omr.Id = 0;
                omr.Message = "Failed to complete to-do list item.";
            }

            return Json(omr);
        }

        [HttpPost]
        public JsonResult Restore(int id)
        {
            OperationModelResponse omr = new OperationModelResponse();
            if (context.UpdateRestore(id))
            {
                omr.Id = id;
                omr.Message = "To-do list item has been restored successfully.";
            }
            else
            {
                omr.Id = 0;
                omr.Message = "Failed to restore to-do list item.";
            }

            return Json(omr);
        }
    }
}
