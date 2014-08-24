using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using Todolist.Models;

namespace Todolist.Models
{
    public class TodolistModel
    {
        public IEnumerable<tbl_todolist> TDList { get; set; }
    }

    public class OperationModelResponse
    {
        public int Id { get; set; }
        public string Message { get; set; }
    }
}