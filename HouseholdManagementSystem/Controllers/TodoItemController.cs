using HouseholdManagementSystem.Models;
using HouseholdManagementSystem.Models.ViewModels;
using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.Linq;
using System.Net.Http;
using System.Web;
using System.Web.Mvc;
using System.Web.Script.Serialization;

namespace HouseholdManagementSystem.Controllers
{
    public class TodoItemController : Controller
    {
        private static readonly HttpClient client;
        private JavaScriptSerializer jss = new JavaScriptSerializer();

        static TodoItemController()
        {
            client = new HttpClient();
            client.BaseAddress = new Uri("https://localhost:44394/api/");
        }

        // GET: TodoItem
        public ActionResult Index()
        {
            return View();
        }

        // GET: TodoItem/NewTodoItem
        public ActionResult NewTodoItem()
        {   
            AddTodoItem ViewModel = new AddTodoItem();

            //Get all categories to render the Category dowpdown
            string url = "categoryData/ListAllCategories";
            HttpResponseMessage response = client.GetAsync(url).Result;
            if (!response.IsSuccessStatusCode)
            {
                TempData["ErrorMessage"] = "An error occurred when fetching all categories. Please try again.";
                TempData["BackUrl"] = Url.Action("ListTodoItems", "TodoItem");
                return RedirectToAction("Error");
            }

            IEnumerable<CategoryDto> categories = response.Content.ReadAsAsync<IEnumerable<CategoryDto>>().Result;
            ViewModel.CategoryList = categories;

            //Get all owners for the Assigned To and Created By dowpdowns
            url = "OwnerData/ListOwners";
            response = client.GetAsync(url).Result;
            if (!response.IsSuccessStatusCode)
            {
                TempData["ErrorMessage"] = "An error occurred when fetching the list of all owners. Please try again.";
                TempData["BackUrl"] = Url.Action("ListTodoItems", "TodoItem");
                return RedirectToAction("Error");
            }

            IEnumerable<OwnerDto> owners = response.Content.ReadAsAsync<IEnumerable<OwnerDto>>().Result;
            ViewModel.OwnersList = owners;

            //Get all Todo Item Status for rendering the Status dowpdowns
            url = "TodoItemStatusData/ListTodoItemStatus";
            response = client.GetAsync(url).Result;
            if (!response.IsSuccessStatusCode)
            {
                TempData["ErrorMessage"] = "An error occurred when fetching the list of Todo Item Status. Please try again.";
                TempData["BackUrl"] = Url.Action("ListTodoItems", "TodoItem");
                return RedirectToAction("Error");
            }

            IEnumerable<TodoItemStatusDto> todoItemStatuses = response.Content.ReadAsAsync<IEnumerable<TodoItemStatusDto>>().Result;
            ViewModel.TodoItemStatusDtoList = todoItemStatuses;

            ViewModel.pendingStatus = todoItemStatuses.FirstOrDefault(status => status.Status == "Pending");
            return View(ViewModel);
        }

        // POST: TodoItem/CreateTodoItem
        [HttpPost]
        public ActionResult CreateTodoItem(TodoItem todoItem)
        {
            string url = "TodoItemData/AddTodoItem";

            string jsonpayload = jss.Serialize(todoItem);
            Debug.WriteLine(jsonpayload);

            HttpContent content = new StringContent(jsonpayload);
            content.Headers.ContentType.MediaType = "application/json";

            HttpResponseMessage response = client.PostAsync(url, content).Result;

            if (response.IsSuccessStatusCode)
            {
                return RedirectToAction("ListTodoItem");
            }
            else
            {
                TempData["ErrorMessage"] = "An error occurred while adding a Todo item. Please try again.";
                TempData["BackUrl"] = Url.Action("ListTodoItem", "TodoItem");
                return RedirectToAction("Error");
            }
        }

        // GET: TodoItem/EditTodoItem
        public ActionResult EditTodoItem(int id)
        {
            UpdateTransaction ViewModel = new UpdateTransaction();

            //Get the transaction details with the given transactionid
            string url = "TransactionData/findTransactionById/" + id;
            HttpResponseMessage response = client.GetAsync(url).Result;
            if (!response.IsSuccessStatusCode)
            {
                TempData["ErrorMessage"] = "An error occurred while finding an expense. Please try again.";
                TempData["BackUrl"] = Url.Action("ListExpenses", "Transaction");
                return RedirectToAction("Error");
            }

            TransactionDto selectedTransaction = response.Content.ReadAsAsync<TransactionDto>().Result;
            ViewModel.SelectedTransaction = selectedTransaction;

            //Get all categories for Expenses to render the dowpdown
            url = "categoryData/listCategoryByTransactionType?transactionTypeName=Expense";
            response = client.GetAsync(url).Result;
            if (!response.IsSuccessStatusCode)
            {
                TempData["ErrorMessage"] = "An error occurred while listing all expense Categories. Please try again.";
                TempData["BackUrl"] = Url.Action("ListExpenses", "Transaction");
                return RedirectToAction("Error");
            }

            IEnumerable<CategoryDto> categories = response.Content.ReadAsAsync<IEnumerable<CategoryDto>>().Result;

            ViewModel.CategoryOptions = categories;

            return View(ViewModel);
        }

        // POST:TodoItem/UpdateTodoItem
        [HttpPost]
        public ActionResult UpdateTodoItem(int id, Transaction transaction)
        {
            transaction.TransactionDate = DateTime.SpecifyKind(transaction.TransactionDate, DateTimeKind.Utc);

            string url = "TransactionData/UpdateTransaction/" + id;
            transaction.TransactionId = id;
            string jsonpayload = jss.Serialize(transaction);
            HttpContent content = new StringContent(jsonpayload);
            content.Headers.ContentType.MediaType = "application/json";
            HttpResponseMessage response = client.PutAsync(url, content).Result;
            Debug.WriteLine(content);

            if (response.IsSuccessStatusCode)
            {
                return RedirectToAction("ListExpenses");
            }
            else
            {
                TempData["ErrorMessage"] = "An error occurred when updating an expense. Please try again.";
                TempData["BackUrl"] = Url.Action("ListExpenses", "Transaction");
                return RedirectToAction("Error");
            }
        }

    }
}