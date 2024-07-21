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

        // GET: TodoItem/ListTodoItems
        public ActionResult ListTodoItems()
        {
            return View();
        }

        // GET: TodoItem/Error
        public ActionResult Error()
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
                return RedirectToAction("ListTodoItems");
            }
            else
            {
                TempData["ErrorMessage"] = "An error occurred while adding a Todo item. Please try again.";
                TempData["BackUrl"] = Url.Action("ListTodoItems", "TodoItem");
                return RedirectToAction("Error");
            }
        }

        // GET: TodoItem/EditTodoItem
        public ActionResult EditTodoItem(int id)
        {
            UpdateTodoItem ViewModel = new UpdateTodoItem();

            //Get the Todo Item details with the given todoItem Id
            string url = "TodoItemData/FindTodoItemById/" + id;
            HttpResponseMessage response = client.GetAsync(url).Result;
            if (!response.IsSuccessStatusCode)
            {
                TempData["ErrorMessage"] = "An error occurred while finding a Todo Item. Please try again.";
                TempData["BackUrl"] = Url.Action("ListTodoItems", "TodoItem");
                return RedirectToAction("Error");
            }
            TodoItemDto selectedTodoItem = response.Content.ReadAsAsync<TodoItemDto>().Result;
            ViewModel.SelectedTodoItem = selectedTodoItem;

            //Get all categories to render the Category dowpdown
            url = "categoryData/ListAllCategories";
            response = client.GetAsync(url).Result;
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

            return View(ViewModel);
        }

        // POST:TodoItem/UpdateTodoItem
        [HttpPost]
        public ActionResult UpdateTodoItem(int id, TodoItem todoItem)
        {
            string url = "TodoItemData/UpdateTodoItem/" + id;
            todoItem.TodoItemId = id;
            string jsonpayload = jss.Serialize(todoItem);
            HttpContent content = new StringContent(jsonpayload);
            content.Headers.ContentType.MediaType = "application/json";
            HttpResponseMessage response = client.PutAsync(url, content).Result;

            if (response.IsSuccessStatusCode)
            {
                return RedirectToAction("ListTodoItems");
            }
            else
            {
                TempData["ErrorMessage"] = "An error occurred while adding a Todo item. Please try again.";
                TempData["BackUrl"] = Url.Action("ListTodoItems", "TodoItem");
                return RedirectToAction("Error");
            }
        }

    }
}