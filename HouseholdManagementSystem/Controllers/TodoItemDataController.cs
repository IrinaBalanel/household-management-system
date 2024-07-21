using HouseholdManagementSystem.Models;
using System;
using System.Collections.Generic;
using System.Data.Entity.Infrastructure;
using System.Data.Entity;
using System.Linq;
using System.Net;
using System.Net.Http;
using System.Web.Http;
using System.Web.Http.Description;

namespace HouseholdManagementSystem.Controllers
{
    public class TodoItemDataController : ApiController
    {
        private ApplicationDbContext db = new ApplicationDbContext();

        /// <summary>
        /// This method will access the local database to get the TodoItem from the TodoItems table for the given Id
        /// </summary>
        /// <example>
        /// GET api/TodoItemData/FindTodoItemById/1 -> {"TodoItemId":1,"TodoItemDescription":"Buy Groceries","StatusId":1,"CategoryId":2,"AssignedToOwnerId":3,"CreatedByOwnerId":4}
        /// </example>
        /// <returns>A TodoItem</returns>        
        [HttpGet]
        [Route("api/TodoItemData/FindTodoItemById/{todoItemId}")]
        public IHttpActionResult FindTodoItemById(int todoItemId)
        {
            TodoItem todoItem = db.TodoItems.Find(todoItemId);
            if (todoItem == null)
            {
                return NotFound();
            }

            TodoItemDto todoItemDto = new TodoItemDto();

            todoItemDto.TodoItemId = todoItem.TodoItemId;
            todoItemDto.TodoItemDescription = todoItem.TodoItemDescription;
            todoItemDto.StatusId = todoItem.StatusId;
            todoItemDto.CategoryId = todoItem.CategoryId;
            todoItemDto.AssignedToOwnerId = todoItem.AssignedToOwnerId;
            todoItemDto.CreatedByOwnerId = todoItem.CreatedByOwnerId;

            return Ok(todoItemDto);
        }

        /// <summary>
        /// This api helps add a new TodoItem to the TodoItems Table in the DB
        /// </summary>
        /// <param name="todoItem">JSON FORM DATA of a TodoItem</param>
        /// <returns>
        /// HEADER: 201 (Created)
        /// CONTENT: TodoItemID, TodoItem Data
        /// or
        /// HEADER: 400 (Bad Request)
        /// </returns>
        /// <example>
        /// curl -d @C:\Users\Ahzi\source\repos\HouseholdManagementSystem\JsonData\TodoItem.json -H "Content-Type:application/json" https://localhost:44356/api/TodoItemData/AddTodoItem
        /// {"TodoItemId":9,"TodoItemDescription":"Clean the house","StatusId":1,"CategoryId":2,"AssignedToOwnerId":3,"CreatedByOwnerId":4}
        /// </example>
        [ResponseType(typeof(TodoItem))]
        [HttpPost]
        [Route("api/TodoItemData/AddTodoItem")]
        public IHttpActionResult AddTodoItem(TodoItem todoItem)
        {
            if (!ModelState.IsValid)
            {
                return BadRequest(ModelState);
            }

            db.TodoItems.Add(todoItem);
            db.SaveChanges();

            return Ok();
        }

        /// <summary>
        /// Updates a particular TodoItem in the system with PUT Data input
        /// </summary>
        /// <param name="id">Represents the TodoItem ID primary key</param>
        /// <param name="todoItem">JSON FORM DATA of a TodoItem</param>
        /// <returns>
        /// HEADER: 204 (Success, No Content Response)
        /// or
        /// HEADER: 400 (Bad Request)
        /// or
        /// HEADER: 404 (Not Found)
        /// </returns>
        /// <example>
        /// PUT: api/TodoItemData/UpdateTodoItem/5
        /// FORM DATA: TodoItem JSON Object
        /// curl -X PUT -d @C:\Users\Ahzi\source\repos\HouseholdManagementSystem\JsonData\TodoItem.json -H "Content-Type:application/json" https://localhost:44356/api/TodoItemData/UpdateTodoItem/2
        /// </example>
        [ResponseType(typeof(void))]
        [HttpPut]
        [Route("api/TodoItemData/UpdateTodoItem/{id}")]
        public IHttpActionResult UpdateTodoItem(int id, TodoItem todoItem)
        {
            if (!ModelState.IsValid)
            {
                return BadRequest(ModelState);
            }

            if (id != todoItem.TodoItemId)
            {
                return BadRequest();
            }

            db.Entry(todoItem).State = EntityState.Modified;

            try
            {
                db.SaveChanges();
            }
            catch (DbUpdateConcurrencyException)
            {
                if (!TodoItemExists(id))
                {
                    return NotFound();
                }
                else
                {
                    throw;
                }
            }

            return StatusCode(HttpStatusCode.NoContent);
        }

        private bool TodoItemExists(int id)
        {
            return db.TodoItems.Count(t => t.TodoItemId == id) > 0;
        }
    }
}
