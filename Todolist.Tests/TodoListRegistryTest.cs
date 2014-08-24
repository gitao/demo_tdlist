using System;
using System.IO;
using System.Linq;
using System.Collections.Generic;
using Microsoft.VisualStudio.TestTools.UnitTesting;
using Todolist.Models;
using Todolist.Classes;

namespace todolist.Tests
{
    [TestClass]
    public class TodoListRegistryTest
    {
        iTodoListRegistry context;

        [TestInitialize()]
        public void Initialize()
        {
            AppDomain.CurrentDomain.SetData("DataDirectory", Path.Combine(AppDomain.CurrentDomain.BaseDirectory.Replace("bin\\Debug", ""), "App_Data"));
            context = new TodoListRegistry();
        }

        [TestMethod]
        public void PrintTodoList()
        {
            var list = context.GetTodoList();
            foreach (var item in list)
            {
                Console.WriteLine(item.Id + "," + item.item + "," + item.priority + "," + item.complete + "," + item.dt_added);
            }
            Console.WriteLine("total records: " + list.Count());
        }

        [TestMethod]
        public void GetTodoList()
        {
            var list = context.GetTodoList();
            Assert.IsTrue(list.Cast<tbl_todolist>().First().Id > 0);
        }

        [TestMethod]
        public void Add()
        {
            Assert.IsTrue(context.AddTodoItem("testadditem-" + (new Random()).Next(0, 100)) > 0);
        }

        [TestMethod]
        public void Delete()
        {
            Assert.IsTrue(context.DeleteTodoItem(context.GetTodoList().First().Id));
            context.AddTodoItem("testadditem-" + (new Random()).Next(0, 100));
        }

        [TestMethod]
        public void Update()
        {
            Assert.IsTrue(context.UpdateTodoItem(context.GetTodoList().First().Id, "testupdateitem-" + (new Random()).Next(0, 100)));
        }

        [TestMethod]
        public void UpdatePriorities()
        {
            Assert.IsTrue(context.UpdatePriorities(new int[] { 3, 2, 1 }));
        }

        [TestMethod]
        public void UpdateComplete()
        {
            Assert.IsTrue(context.UpdateComplete(context.GetTodoList().First().Id));
        }

        [TestMethod]
        public void UpdateRestore()
        {
            Assert.IsTrue(context.UpdateRestore(context.GetTodoList().First().Id));
        }

        [TestMethod]
        public void DeleteAll()
        {
            Assert.IsTrue(context.DeleteAll());
            context.AddTodoItem("testadditem-" + (new Random()).Next(0, 100));
        }
    }
}
