using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using Todolist.Models;

namespace Todolist.Classes
{
    public interface iTodoListRegistry
    {
        IEnumerable<tbl_todolist> GetTodoList();
        int AddTodoItem(string item);
        bool DeleteTodoItem(int id);
        bool DeleteAll();
        bool UpdateTodoItem(int id, string item);
        bool UpdatePriorities(int[] ids);
        bool UpdateComplete(int id);
        bool UpdateRestore(int id);
    }
}