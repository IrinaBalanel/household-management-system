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
using HouseholdManagementSystem.Migrations;
using System.Collections;

namespace HouseholdManagementSystem.Controllers
{
    public class TodoItemDataController : ApiController
    {
        private ApplicationDbContext db = new ApplicationDbContext();

        /// <summary>
        /// This method will access the local database to get all TodoItems from the TodoItems table
        /// </summary>
        /// <example>
        /// GET: api/TodoItemData/ListAllTodoItems -> [{"TodoItemId":1,"TodoItemDescription":"Buy Groceries","StatusId":1,"CategoryId":2,"AssignedToOwnerId":3,"CreatedByOwnerId":4}, {"TodoItemId":2,"TodoItemDescription":"Pay Rent","StatusId":2,"CategoryId":2,"AssignedToOwnerId":3,"CreatedByOwnerId":4}]
        /// </example>
        /// <returns>List of todo items with relevant information to this item</returns> 
        /*        [ResponseType(typeof(TodoItemDto))]
                [HttpGet]
                [Route("api/TodoItemData/ListAllTodoItems")]
                public IHttpActionResult ListAllTodoItems()
                {
                    List<TodoItem> todoItems = db.TodoItems
                        .Include(t => t.Status)
                        .Include(t => t.AssignedTo)
                        .Include(t => t.CreatedBy)
                        .Include(t => t.Category)
                        .Include(t => t.Transaction)
                        .ToList();
                    List<TodoItemDto> todoItemDtos = new List<TodoItemDto>();

                    todoItems.ForEach(t => todoItemDtos.Add(new TodoItemDto()
                    {
                        TodoItemId = t.TodoItemId,
                        TodoItemDescription = t.TodoItemDescription,
                        Status = t.Status?.Status,
                        CategoryId = t.CategoryId,
                        CategoryName = t.Category?.CategoryName,
                        AssignedToOwnerId = t.AssignedToOwnerId,
                        AssignedTo = t.AssignedTo?.OwnerName,
                        CreatedByOwnerId = t.CreatedByOwnerId,
                        CreatedBy = t.CreatedBy?.OwnerName,
                        TransactionId = t.TransactionId
                    }));

                    return Ok(todoItemDtos);

                }*/

        /// <summary>
        /// This method will access the local database to get TodoItems from the TodoItems table that match the filters provided as query parameters
        /// Filter Types:
        /// 1. Status
        /// 2. Category Name
        /// 3. Assigned To
        /// 4. Created By
        /// </summary>
        /// <example>
        /// GET api/TodoItemData/ListAllTodoItems -> [{"TodoItemId":1,"TodoItemDescription":"Buy Groceries","StatusId":1,"CategoryId":2,"AssignedToOwnerId":3,"CreatedByOwnerId":4}, {"TodoItemId":2,"TodoItemDescription":"Pay Rent","StatusId":2,"CategoryId":2,"AssignedToOwnerId":3,"CreatedByOwnerId":4}]
        /// Find by Status: GET /api/TodoItem/ListTodoItems?status=Pending
        /// Find by Category Name: GET /api/TodoItem/ListTodoItems?categoryName=Rent
        /// Find by Assigned To: GET /api/TodoItem/ListTodoItems?assignedToOwner=Irina
        /// Find by Created By: GET /api/TodoItem/ListTodoItems?createdByOwner=Mom
        /// </example>
        /// <returns>A list of all Transactions that match the filter</returns>
        [ResponseType(typeof(TodoItemDto))]
        [HttpGet]
        [Route("api/TodoItemData/ListAllTodoItems")]
        public IHttpActionResult ListAllTodoItems([FromUri] string status = null, [FromUri] string categoryName = null, [FromUri] string assignedToOwner = null, [FromUri] string createdByOwner = null)
        {
            var query = db.TodoItems.AsQueryable();

            if (!string.IsNullOrEmpty(status))
            {
                query = query.Where(t => t.Status.Status.Equals(status, StringComparison.OrdinalIgnoreCase));
            }
            if (!string.IsNullOrEmpty(categoryName))
            {
                query = query.Where(t => t.Category.CategoryName.Equals(categoryName, StringComparison.OrdinalIgnoreCase));
            }

            if (!string.IsNullOrEmpty(assignedToOwner))
            {
                query = query.Where(t => t.AssignedTo.OwnerName.Equals(assignedToOwner, StringComparison.OrdinalIgnoreCase));
            }

            if (!string.IsNullOrEmpty(createdByOwner))
            {
                query = query.Where(t => t.CreatedBy.OwnerName.Equals(createdByOwner, StringComparison.OrdinalIgnoreCase));
            }

            var todoItemList = query.ToList();

            List<TodoItemDto> todoItemDtos = new List<TodoItemDto>();
            foreach (var todoItem in todoItemList)
            {
                var todoItemDto = new TodoItemDto
                {
                    TodoItemId = todoItem.TodoItemId,
                    TodoItemDescription = todoItem.TodoItemDescription,
                    StatusId = todoItem.StatusId,
                    Status = todoItem.Status.Status,
                    CategoryId = todoItem.CategoryId,
                    CategoryName = todoItem.Category.CategoryName,
                    AssignedToOwnerId = todoItem.AssignedToOwnerId,
                    AssignedTo = todoItem.AssignedTo.OwnerName,
                    CreatedByOwnerId = todoItem.CreatedByOwnerId,
                    CreatedBy = todoItem.CreatedBy.OwnerName,
                    TransactionId = todoItem.TransactionId
                };

                todoItemDtos.Add(todoItemDto);
            }

            return Ok(todoItemDtos);

        }

        ///<summary>
        /// Deletes a todo item from the db by it's ID
        ///</summary>
        ///<returns>status 200(Ok) or 404(Not Found)</returns>
        ///<param name="id">The primary key of the todo item</param>
        ///<example>
        ///POST: api/TodoItemData/DeleteTodoItem/8
        ///</example> 
        [ResponseType(typeof(TodoItem))]
        [HttpDelete]
        [Route("api/TodoItemData/DeleteTodoItem/{id}")]
        public IHttpActionResult DeleteTodoItem(int id)
        {
            TodoItem todoItem = db.TodoItems.Find(id);
            if (todoItem == null)
            {
                return NotFound();
            }

            bool hasAssociatedTransaction = db.TodoItems.Any(t => t.TransactionId == todoItem.TransactionId);
            if (hasAssociatedTransaction)
            {
                return Content(HttpStatusCode.Conflict, "This To-Do item cannot be deleted as it has associated Transactions");
            }

            db.TodoItems.Remove(todoItem);
            db.SaveChanges();

            return Ok(todoItem);
        }



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
            todoItemDto.Status = todoItem.Status.Status;
            todoItemDto.CategoryId = todoItem.CategoryId;
            todoItemDto.CategoryName = todoItem.Category.CategoryName;
            todoItemDto.AssignedToOwnerId = todoItem.AssignedToOwnerId;
            todoItemDto.CreatedByOwnerId = todoItem.CreatedByOwnerId;
            todoItemDto.AssignedTo = todoItem.AssignedTo.OwnerName;
            todoItemDto.CreatedBy = todoItem.CreatedBy.OwnerName;


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

        [ResponseType(typeof(void))]
        [HttpPut]
        [Route("api/TodoItemData/UpdateTodoItemWithTransactionId/{id}/{transactionId}")]
        public IHttpActionResult UpdateTodoItemWithTransactionId(int id, int transactionId)
        {
            TodoItem todoItem = db.TodoItems.Find(id);
            if (todoItem == null)
            {
                return NotFound();
            }

            todoItem.TransactionId = transactionId;
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
