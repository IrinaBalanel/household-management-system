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
        /// This api helps add a new Transaction to the Transactions Table in the DB
        /// </summary>
        /// <param name="transaction">JSON FORM DATA of a Transaction</param>
        /// <returns>
        /// HEADER: 201 (Created)
        /// CONTENT: TransactionID, Transaction Data
        /// or
        /// HEADER: 400 (Bad Request)
        /// </returns>
        /// <example>
        ///curl -d @C:\Users\Ahzi\source\repos\PersonalFinanceTracker\PersonalFinanceTracker\JsonData\Transaction.json -H "Content-Type:application/json" https://localhost:44356/api/TransactionData/addTransaction
        ///{"TransactionId":9,"Title":"Cheque for freelance software gig","Amount":1000.99,"TransactionDate":"2024-06-03T00:00:00","CategoryId":2,"Category":null}
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
        /// Updates a particular transaction in the system with PUT Data input
        /// </summary>
        /// <param name="id">Represents the Transaction ID primary key</param>
        /// <param name="transaction">JSON FORM DATA of a transaction</param>
        /// <returns>
        /// HEADER: 204 (Success, No Content Response)
        /// or
        /// HEADER: 400 (Bad Request)
        /// or
        /// HEADER: 404 (Not Found)
        /// </returns>
        /// <example>
        /// PUT: api/TransactionData/UpdateTransaction/5
        /// FORM DATA: Transaction JSON Object
        /// curl -X PUT -d @C:\Users\Ahzi\source\repos\PersonalFinanceTracker\PersonalFinanceTracker\JsonData\Transaction.json -H "Content-Type:application/json" https://localhost:44356/api/TransactionData/updateTransaction/2
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
