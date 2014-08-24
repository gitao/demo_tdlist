using System;
using System.IO;
using System.Linq;
using System.Web.Mvc;
using System.Web.Script.Serialization;
using Microsoft.VisualStudio.TestTools.UnitTesting;
using Todolist.Models;
using Todolist.Classes;
using Todolist.Controllers;

namespace todolist.Tests
{
    [TestClass]
    public class HomeControllerTest
    {
        iTodoListRegistry context;
        HomeController controller;

        [TestInitialize()]
        public void Initialize()
        {
            AppDomain.CurrentDomain.SetData("DataDirectory", Path.Combine(AppDomain.CurrentDomain.BaseDirectory.Replace("bin\\Debug", ""), "App_Data"));
            context = new TodoListRegistry();
            controller = new HomeController(context);
        }

        [TestMethod]
        public void Index()
        {
            var result = controller.Index() as ViewResult;
            Assert.IsTrue(((TodolistModel)result.Model).TDList.FirstOrDefault().Id > 0);
        }

        [TestMethod]
        public void Add()
        {
            var result = controller.Add("testadditem-" + (new Random()).Next(0, 100)) as JsonResult;
            dynamic output = result.Data;
            Assert.IsTrue(output.Id > 0);
        }

        [TestMethod]
        public void Delete()
        {
            var result = controller.Delete(context.GetTodoList().First().Id) as JsonResult;
            dynamic output = result.Data;
            Assert.IsTrue(output.Id > 0);
            controller.Add("testadditem-" + (new Random()).Next(0, 100));
        }

        [TestMethod]
        public void Update()
        {
            var result = controller.Update(context.GetTodoList().First().Id, "testupdateitem-" + (new Random()).Next(0, 100)) as JsonResult;
            dynamic output = result.Data;
            Assert.IsTrue(output.Id > 0);
        }

        [TestMethod]
        public void ReArrange()
        {
            var result = controller.ReArrange(new int[] { 3, 2, 1 }) as JsonResult;
            dynamic output = result.Data;
            Assert.IsTrue(output.Id > 0);
        }

        [TestMethod]
        public void Complete()
        {
            var result = controller.Complete(context.GetTodoList().First().Id) as JsonResult;
            dynamic output = result.Data;
            Assert.IsTrue(output.Id > 0);
        }

        [TestMethod]
        public void Restore()
        {
            var result = controller.Restore(context.GetTodoList().First().Id) as JsonResult;
            dynamic output = result.Data;
            Assert.IsTrue(output.Id > 0);
        }
    }
}
