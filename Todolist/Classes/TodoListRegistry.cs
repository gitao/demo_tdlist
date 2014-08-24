using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using Todolist.Models;

namespace Todolist.Classes
{
    public class TodoListRegistry : iTodoListRegistry
    {
        public IEnumerable<tbl_todolist> GetTodoList()
        {
            IEnumerable<tbl_todolist> list = null;
            using (var context = new TodolistEntities())
            {
                list = context.tbl_todolist.OrderBy(r => r.priority).ToList();
            }
            return list;
        }

        public int AddTodoItem(string item)
        {
            int id = 0;
            using (var context = new TodolistEntities())
            {
                int priority = 0;
                try
                {
                    priority = context.tbl_todolist.Max(r => r.priority);
                }
                catch(Exception)
                { }
                tbl_todolist tl = new tbl_todolist();
                tl.item = item;
                tl.priority = priority + 1;
                tl.dt_added = DateTime.Now;
                context.tbl_todolist.Add(tl);
                context.SaveChanges();
                id = tl.Id;
            }
            return id;
        }

        public bool DeleteTodoItem(int id)
        {
            bool result = false;
            using (var context = new TodolistEntities())
            {
                tbl_todolist tblItem = context.tbl_todolist.FirstOrDefault(r => r.Id == id);
                if (tblItem != null)
                {
                    context.tbl_todolist.Remove(tblItem);
                    context.SaveChanges();
                    result = true;
                }
            }
            return result;
        }

        public bool DeleteAll()
        {
            bool result = false;
            using (var context = new TodolistEntities())
            {
                var objContext = ((System.Data.Entity.Infrastructure.IObjectContextAdapter)context).ObjectContext;
                objContext.ExecuteStoreCommand("TRUNCATE TABLE [tbl_todolist]");
                result = true;
            }
            return result;
        }

        public bool UpdateTodoItem(int id, string item)
        {
            bool result = false;
            using (var context = new TodolistEntities())
            {
                tbl_todolist tblItem = context.tbl_todolist.FirstOrDefault(r => r.Id == id);
                if (tblItem != null)
                {
                    tblItem.item = item;
                    context.SaveChanges();
                    result = true;
                }
            }
            return result;
        }

        public bool UpdatePriorities(int[] ids)
        {
            bool result = false;
            using (var context = new TodolistEntities())
            {
                for (int i = 0; i < ids.Length; i++)
                {
                    int id = ids[i];
                    tbl_todolist tblItem = context.tbl_todolist.FirstOrDefault(r => r.Id == id);
                    if (tblItem != null)
                    {
                        tblItem.priority = i + 1;
                    }
                }
                context.SaveChanges();
                result = true;
            }
            return result;
        }

        public bool UpdateComplete(int id)
        {
            bool result = false;
            using (var context = new TodolistEntities())
            {
                tbl_todolist tblItem = context.tbl_todolist.FirstOrDefault(r => r.Id == id);
                if (tblItem != null)
                {
                    tblItem.complete = true;
                    context.SaveChanges();
                    result = true;
                }
            }
            return result;
        }

        public bool UpdateRestore(int id)
        {
            bool result = false;
            using (var context = new TodolistEntities())
            {
                tbl_todolist tblItem = context.tbl_todolist.FirstOrDefault(r => r.Id == id);
                if (tblItem != null)
                {
                    tblItem.complete = false;
                    context.SaveChanges();
                    result = true;
                }
            }
            return result;
        }
    }
}