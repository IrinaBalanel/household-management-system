using HouseholdManagementSystem.Migrations;
using HouseholdManagementSystem.Models;
using HouseholdManagementSystem.Models.ViewModels;
using System;
using System.Collections.Generic;
using System.Deployment.Internal;
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
            HttpClientHandler handler = new HttpClientHandler()
            {
                AllowAutoRedirect = false,
                UseCookies = false
            };

            client = new HttpClient(handler);
            client.BaseAddress = new Uri("https://localhost:44394/api/");
        }

        /// <summary>
        /// Gets the authentication cookie sent to this controller
        /// </summary>
        private void GetApplicationCookie()
        {
            string token = "";
            //This is a bit dangerous because a previously authenticated cookie could be cached for
            //a follow-up request from someone else. Reset cookies in HTTP client before grabbing a new one.
            client.DefaultRequestHeaders.Remove("Cookie");
            if (!User.Identity.IsAuthenticated) return;

            HttpCookie cookie = System.Web.HttpContext.Current.Request.Cookies.Get(".AspNet.ApplicationCookie");
            if (cookie != null) token = cookie.Value;

            //collect token
            //pass along to the WebAPI
            if (token != "") client.DefaultRequestHeaders.Add("Cookie", ".AspNet.ApplicationCookie=" + token);

            return;
        }

        // GET: TodoItem
        public ActionResult Index()
        {
            return View();
        }

        // GET: TodoItem/ListTodoItems
        /*        public ActionResult ListTodoItems()
                {
                    string url = "TodoItemData/ListAllTodoItems";

                    HttpResponseMessage response = client.GetAsync(url).Result;
                    IEnumerable<TodoItemDto> todoItems = response.Content.ReadAsAsync<IEnumerable<TodoItemDto>>().Result;

                    return View(todoItems);

                }*/
        /*public ActionResult ListTodoItems(string ownerName = null, string filterType = null, string status = null, string categoryName = null, string assignedToOwner = null, string createdByOwner = null)
        {
            FilterTodoItems ViewModel = new FilterTodoItems
            {
                StatusList = new List<TodoItemStatusDto>(),
                CategoryList = new List<CategoryDto>(),
                AssignedToOwnersList = new List<OwnerDto>(),
                CreatedByOwnersList = new List<OwnerDto>()
            };

            //Get status list
            string url = "TodoItemStatusData/ListTodoItemStatus";
            HttpResponseMessage response = client.GetAsync(url).Result;

            if (!response.IsSuccessStatusCode)
            {
                TempData["ErrorMessage"] = "An error occurred while listing all statuses. Please try again.";
                TempData["BackUrl"] = Url.Action("ListTodoItems", "TodoItem");
                return RedirectToAction("Error");
            }
            ViewModel.StatusList = response.Content.ReadAsAsync<IEnumerable<TodoItemStatusDto>>().Result;

            //Get category list
            url = "CategoryData/ListAllCategories";
            response = client.GetAsync(url).Result;
            if (!response.IsSuccessStatusCode)
            {
                TempData["ErrorMessage"] = "An error occurred while listing all categories. Please try again.";
                TempData["BackUrl"] = Url.Action("ListTodoItems", "TodoItem");
                return RedirectToAction("Error");
            }
            ViewModel.CategoryList = response.Content.ReadAsAsync<IEnumerable<CategoryDto>>().Result;


            //Get owners list
            url = "OwnerData/ListOwners";
            response = client.GetAsync(url).Result;
            if (!response.IsSuccessStatusCode)
            {
                TempData["ErrorMessage"] = "An error occurred while listing all owners. Please try again.";
                TempData["BackUrl"] = Url.Action("ListTodoItems", "TodoItem");
                return RedirectToAction("Error");
            }
            ViewModel.AssignedToOwnersList = response.Content.ReadAsAsync<IEnumerable<OwnerDto>>().Result;

            //Get owners list
            url = "OwnerData/ListOwners";
            response = client.GetAsync(url).Result;
            if (!response.IsSuccessStatusCode)
            {
                TempData["ErrorMessage"] = "An error occurred while listing all owners. Please try again.";
                TempData["BackUrl"] = Url.Action("ListTodoItems", "TodoItem");
                return RedirectToAction("Error");
            }
            ViewModel.CreatedByOwnersList = response.Content.ReadAsAsync<IEnumerable<OwnerDto>>().Result;

            //Get todo items list
            url = "TodoItemData/ListAllTodoItems";
            var queryParams = new List<string>();
            if (!string.IsNullOrEmpty(status))
            {
                queryParams.Add($"status={status}");
            }
            if (!string.IsNullOrEmpty(categoryName))
            {
                queryParams.Add($"categoryName={categoryName}");
            }
            if (!string.IsNullOrEmpty(ownerName))
            {
                if (filterType == "assigned")
                {
                    queryParams.Add($"assignedToOwner={ownerName}");
                }
                else if (filterType == "created")
                {
                    queryParams.Add($"createdByOwner={ownerName}");
                }
            }
            if (!string.IsNullOrEmpty(assignedToOwner))
            {
                queryParams.Add($"assignedToOwner={assignedToOwner}");
            }
            if (!string.IsNullOrEmpty(createdByOwner))
            {
                queryParams.Add($"createdByOwner={createdByOwner}");
            }

            if (queryParams.Any())
            {
                url += "?" + string.Join("&", queryParams);
            }


            //Get filtered list
            response = client.GetAsync(url).Result;
            if (!response.IsSuccessStatusCode)
            {
                TempData["ErrorMessage"] = "An error occurred while listing all TodoItems. Please try again.";
                TempData["BackUrl"] = Url.Action("ListTodoItems", "TodoItem");
                return RedirectToAction("Error");
            }
            IEnumerable<TodoItemDto> todoItems = response.Content.ReadAsAsync<IEnumerable<TodoItemDto>>().Result;
            ViewModel.TodoItemsList = todoItems;

            // Set selected filters

            ViewModel.SelectedStatus = status;
            ViewModel.SelectedCategory = categoryName;
            ViewModel.SelectedAssignedToOwner = assignedToOwner;
            ViewModel.SelectedCreatedByOwner = createdByOwner;

            return View(ViewModel);

        }*/

        // GET: TodoItem/ListTodoItems
        public ActionResult ListTodoItems(int? id = null, string ownerName = null, string filterType = null, string status = null, string categoryName = null, string assignedToOwner = null, string createdByOwner = null)
        {
            FilterTodoItems ViewModel = new FilterTodoItems
            {
                StatusList = new List<TodoItemStatusDto>(),
                CategoryList = new List<CategoryDto>(),
                AssignedToOwnersList = new List<OwnerDto>(),
                CreatedByOwnersList = new List<OwnerDto>()
            };

            // Get status list
            string url = "TodoItemStatusData/ListTodoItemStatus";
            HttpResponseMessage response = client.GetAsync(url).Result;

            if (!response.IsSuccessStatusCode)
            {
                TempData["ErrorMessage"] = "An error occurred while listing all statuses. Please try again.";
                TempData["BackUrl"] = Url.Action("ListTodoItems", "TodoItem");
                return RedirectToAction("Error");
            }
            ViewModel.StatusList = response.Content.ReadAsAsync<IEnumerable<TodoItemStatusDto>>().Result;

            // Get category list
            url = "CategoryData/ListAllCategories";
            response = client.GetAsync(url).Result;
            if (!response.IsSuccessStatusCode)
            {
                TempData["ErrorMessage"] = "An error occurred while listing all categories. Please try again.";
                TempData["BackUrl"] = Url.Action("ListTodoItems", "TodoItem");
                return RedirectToAction("Error");
            }
            ViewModel.CategoryList = response.Content.ReadAsAsync<IEnumerable<CategoryDto>>().Result;

            // Get owners list
            url = "OwnerData/ListOwners";
            response = client.GetAsync(url).Result;
            if (!response.IsSuccessStatusCode)
            {
                TempData["ErrorMessage"] = "An error occurred while listing all owners. Please try again.";
                TempData["BackUrl"] = Url.Action("ListTodoItems", "TodoItem");
                return RedirectToAction("Error");
            }
            ViewModel.AssignedToOwnersList = response.Content.ReadAsAsync<IEnumerable<OwnerDto>>().Result;

            // Get owners list
            url = "OwnerData/ListOwners";
            response = client.GetAsync(url).Result;
            if (!response.IsSuccessStatusCode)
            {
                TempData["ErrorMessage"] = "An error occurred while listing all owners. Please try again.";
                TempData["BackUrl"] = Url.Action("ListTodoItems", "TodoItem");
                return RedirectToAction("Error");
            }
            ViewModel.CreatedByOwnersList = response.Content.ReadAsAsync<IEnumerable<OwnerDto>>().Result;

            // Check if id is provided
            if (id.HasValue)
            {
                url = $"TodoItemData/FindTodoItemById/{id.Value}";
                response = client.GetAsync(url).Result;
                if (!response.IsSuccessStatusCode)
                {
                    TempData["ErrorMessage"] = "An error occurred while fetching the Todo item. Please try again.";
                    TempData["BackUrl"] = Url.Action("ListTodoItems", "TodoItem");
                    return RedirectToAction("Error");
                }
                var todoItem = response.Content.ReadAsAsync<TodoItemDto>().Result;
                ViewModel.TodoItemsList = new List<TodoItemDto> { todoItem };

                ViewModel.SelectedStatus = todoItem.Status;
                ViewModel.SelectedCategory = todoItem.CategoryName;
                ViewModel.SelectedAssignedToOwner = todoItem.AssignedTo;
                ViewModel.SelectedCreatedByOwner = todoItem.CreatedBy;
            }
            else
            {
                // Get todo items list
                url = "TodoItemData/ListAllTodoItems";
                var queryParams = new List<string>();
                if (!string.IsNullOrEmpty(status))
                {
                    queryParams.Add($"status={status}");
                }
                if (!string.IsNullOrEmpty(categoryName))
                {
                    queryParams.Add($"categoryName={categoryName}");
                }
                if (!string.IsNullOrEmpty(ownerName))
                {
                    if (filterType == "assigned")
                    {
                        queryParams.Add($"assignedToOwner={ownerName}");
                    }
                    else if (filterType == "created")
                    {
                        queryParams.Add($"createdByOwner={ownerName}");
                    }
                }
                if (!string.IsNullOrEmpty(assignedToOwner))
                {
                    queryParams.Add($"assignedToOwner={assignedToOwner}");
                }
                if (!string.IsNullOrEmpty(createdByOwner))
                {
                    queryParams.Add($"createdByOwner={createdByOwner}");
                }

                if (queryParams.Any())
                {
                    url += "?" + string.Join("&", queryParams);
                }

                // Get filtered list
                response = client.GetAsync(url).Result;
                if (!response.IsSuccessStatusCode)
                {
                    TempData["ErrorMessage"] = "An error occurred while listing all TodoItems. Please try again.";
                    TempData["BackUrl"] = Url.Action("ListTodoItems", "TodoItem");
                    return RedirectToAction("Error");
                }
                IEnumerable<TodoItemDto> todoItems = response.Content.ReadAsAsync<IEnumerable<TodoItemDto>>().Result;
                ViewModel.TodoItemsList = todoItems;

                // Set selected filters
                ViewModel.SelectedStatus = status;
                ViewModel.SelectedCategory = categoryName;
                ViewModel.SelectedAssignedToOwner = assignedToOwner;
                ViewModel.SelectedCreatedByOwner = createdByOwner;
            }

            return View(ViewModel);
        }


        // POST: TodoItem/DeleteTodoItem/id
        [Authorize]
        public ActionResult DeleteTodoItem(int id)
        {
            GetApplicationCookie();
            string url = "TodoItemData/DeleteTodoItem/" + id;

            HttpResponseMessage response = client.DeleteAsync(url).Result;
            if (response.IsSuccessStatusCode)
            {
                return RedirectToAction("ListTodoItems");
            }
            else
            {
                TempData["ErrorMessage"] = "An error occurred when deleting a To-Do item. Please try again.";
                TempData["BackUrl"] = Url.Action("ListTodoItems");
                return RedirectToAction("Error");
            }

        }

        // GET: TodoItem/Error
        public ActionResult Error()
        {
            return View();
        }

        // GET: TodoItem/NewTodoItem
        [Authorize]
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
        [Authorize]
        public ActionResult CreateTodoItem(TodoItem todoItem)
        {
            GetApplicationCookie();
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
        [Authorize]
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
        [Authorize]
        public ActionResult UpdateTodoItem(int id, TodoItem todoItem)
        {
            GetApplicationCookie();
            string url = "TodoItemData/UpdateTodoItem/" + id;
            todoItem.TodoItemId = id;

            // Fetch the existing TodoItem to compare the status
            HttpResponseMessage existingItemResponse = client.GetAsync("TodoItemData/FindTodoItemById/" + id).Result;
            if (!existingItemResponse.IsSuccessStatusCode)
            {
                TempData["ErrorMessage"] = "An error occurred while fetching the Todo item. Please try again.";
                TempData["BackUrl"] = Url.Action("ListTodoItems", "TodoItem");
                return RedirectToAction("Error");
            }
            TodoItemDto existingTodoItem = existingItemResponse.Content.ReadAsAsync<TodoItemDto>().Result;

            bool statusChangedToDone = existingTodoItem.Status == "Pending" && todoItem.StatusId == 2;

            string jsonpayload = jss.Serialize(todoItem);
            HttpContent content = new StringContent(jsonpayload);
            content.Headers.ContentType.MediaType = "application/json";
            HttpResponseMessage response = client.PutAsync(url, content).Result;

            if (response.IsSuccessStatusCode)
            {
                if (statusChangedToDone)
                {
                    return RedirectToAction("NewExpense", "Transaction", new
                    {
                        title = todoItem.TodoItemDescription,
                        categoryId = todoItem.CategoryId,
                        todoItemId = todoItem.TodoItemId,
                    });
                }
                return RedirectToAction("ListTodoItems");
            }
            else
            {
                TempData["ErrorMessage"] = "An error occurred while updating the Todo item. Please try again.";
                TempData["BackUrl"] = Url.Action("ListTodoItems", "TodoItem");
                return RedirectToAction("Error");
            }
        }
    }
}