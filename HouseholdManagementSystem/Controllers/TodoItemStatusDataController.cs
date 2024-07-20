using HouseholdManagementSystem.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Net;
using System.Net.Http;
using System.Web.Http;
using System.Web.Http.Description;

namespace HouseholdManagementSystem.Controllers
{
    public class TodoItemStatusDataController : ApiController
    {
        private ApplicationDbContext db = new ApplicationDbContext();

        /// <summary>
        /// This method will access the local database to get all the Categories from the Categoriess table for a TransactionTypeName
        /// </summary>
        /// <example>
        /// GET api/CategoryData/listCategoryByTransactionType?transactionTypeName=Income
        /// [{"CategoryId":2,"CategoryName":"Salary","TransactionTypeName":"Income"}]
        /// </example>
        /// <returns>Category Objects</returns>        
        [HttpGet]
        [ResponseType(typeof(TodoItemStatus))]
        [Route("api/TodoItemStatusData/ListTodoItemStatus")]
        public IHttpActionResult ListTodoItemStatus()
        {
            List<TodoItemStatus> todoItemStatusList = db.TodoItemStatus.ToList();

            if (todoItemStatusList.Count == 0)
            {
                return NotFound();
            }

            List<TodoItemStatusDto> todoItemStatusDtos = new List<TodoItemStatusDto>();

            foreach (var todoItem in todoItemStatusList)
            {
                TodoItemStatusDto todoItemStatusDto = new TodoItemStatusDto();
                todoItemStatusDto.StatusId = todoItem.StatusId;
                todoItemStatusDto.Status = todoItem.Status;

                todoItemStatusDtos.Add(todoItemStatusDto);
            }

            return Ok(todoItemStatusDtos);
        }
    }
}
