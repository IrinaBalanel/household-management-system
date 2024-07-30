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
        /// This method will access the local database to get the list of status names of the todo items
        /// </summary>
        /// <example>
        /// GET api/TodoItemStatusData/ListTodoItemStatus
        /// [{"StatusId":1,"Status":"Pendng"},{"StatusId":2, "Status": "Done"}]
        /// </example>
        /// <returns>Status Objects</returns>        
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
