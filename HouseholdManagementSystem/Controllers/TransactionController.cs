using HouseholdManagementSystem.Models.ViewModels;
using HouseholdManagementSystem.Models;
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
    public class TransactionController : Controller
    {
        private static readonly HttpClient client;
        private JavaScriptSerializer jss = new JavaScriptSerializer();

        static TransactionController()
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

        // GET: Transaction
        public ActionResult Index()
        {
            TransactionOverview TransactionOverview = new TransactionOverview();

            //Get all Transactions
            string url = "TransactionData/ListAllTransactions?currentMonth=true";
            HttpResponseMessage response = client.GetAsync(url).Result;
            if (!response.IsSuccessStatusCode)
            {
                TempData["ErrorMessage"] = "An error occurred while listing all Transactions. Please try again.";
                TempData["BackUrl"] = "/";
                return RedirectToAction("Error");
            }

            IEnumerable<TransactionDto> transactions = response.Content.ReadAsAsync<IEnumerable<TransactionDto>>().Result;
            TransactionOverview.TransactionList = transactions;

            //Get total amount spent or gained per Category
            url = "TransactionData/CategoryTotals?currentMonth=true";
            response = client.GetAsync(url).Result;
            if (!response.IsSuccessStatusCode)
            {
                TempData["ErrorMessage"] = "An error occurred when getting Total Amount spent or gained per category. Please try again.";
                TempData["BackUrl"] = "/";
                return RedirectToAction("Error");
            }

            IEnumerable<CategoryTotal> CategoryTotals = response.Content.ReadAsAsync<IEnumerable<CategoryTotal>>().Result;
            TransactionOverview.CategoryTotalList = CategoryTotals;

            //Get total amount spent or gained per TransactionType
            url = "TransactionData/TransactionTypeTotals?currentMonth=true";
            response = client.GetAsync(url).Result;
            if (!response.IsSuccessStatusCode)
            {
                TempData["ErrorMessage"] = "An error occurred when getting Total Amount spent or gained per TransactionType. Please try again.";
                TempData["BackUrl"] = "/";
                return RedirectToAction("Error");
            }

            IEnumerable<TransactionTypeTotal> TransactionTypeTotalList = response.Content.ReadAsAsync<IEnumerable<TransactionTypeTotal>>().Result;
            TransactionOverview.TransactionTypeTotalList = TransactionTypeTotalList;

            // Get the name of the current month
            DateTime startOfMonth = new DateTime(DateTime.Now.Year, DateTime.Now.Month, 1);
            TransactionOverview.CurrentMonth = startOfMonth.ToString("MMMM");

            return View(TransactionOverview);
        }

        //GET: Transaction/ListExpenses
        /*public ActionResult ListExpenses(string filter = null, string categoryName = null)
        {
            ListTransactions ViewModel = new ListTransactions();

            //Get all categories for Expenses to render the dowpdown
            string url = "categoryData/listCategoryByTransactionType?transactionTypeName=Expense";
            HttpResponseMessage response = client.GetAsync(url).Result;
            if (!response.IsSuccessStatusCode)
            {
                TempData["ErrorMessage"] = "An error occurred while listing all expense Categories. Please try again.";
                TempData["BackUrl"] = Url.Action("ListExpenses", "Transaction");
                return RedirectToAction("Error");
            }

            IEnumerable<CategoryDto> categories = response.Content.ReadAsAsync<IEnumerable<CategoryDto>>().Result;
            ViewModel.CategoryList = categories;

            url = "TransactionData/findTransactions?transactionType=Expense";

            if (filter == "currentMonth")
            {
                url += "&currentMonth=true";
            }
            else if (filter == "lastMonth")
            {
                url += "&lastMonth=true";
            }

            if (!string.IsNullOrEmpty(categoryName))
            {
                url += $"&categoryName={categoryName}";
            }

            response = client.GetAsync(url).Result;
            IEnumerable<TransactionDto> expenseTransactions = response.Content.ReadAsAsync<IEnumerable<TransactionDto>>().Result;
            ViewModel.TransactionList = expenseTransactions;
            ViewModel.SelectedFilter = filter;
            ViewModel.SelectedCategory = categoryName;

            return View(ViewModel);
        }

        //GET: Transaction/ListIncomes
        public ActionResult ListIncomes(string filter = null, string categoryName = null)
        {
            ListTransactions ViewModel = new ListTransactions();

            //Get all categories for Incomes to render the dowpdown
            string url = "categoryData/listCategoryByTransactionType?transactionTypeName=Income";
            HttpResponseMessage response = client.GetAsync(url).Result;
            if (!response.IsSuccessStatusCode)
            {
                TempData["ErrorMessage"] = "An error occurred while listing all income Categories. Please try again.";
                TempData["BackUrl"] = Url.Action("ListIncomes", "Transaction");
                return RedirectToAction("Error");
            }

            IEnumerable<CategoryDto> categories = response.Content.ReadAsAsync<IEnumerable<CategoryDto>>().Result;
            ViewModel.CategoryList = categories;

            url = "TransactionData/findTransactions?transactionType=Income";

            if (filter == "currentMonth")
            {
                url += "&currentMonth=true";
            }
            else if (filter == "lastMonth")
            {
                url += "&lastMonth=true";
            }

            if (!string.IsNullOrEmpty(categoryName))
            {
                url += $"&categoryName={categoryName}";
            }

            response = client.GetAsync(url).Result;
            IEnumerable<TransactionDto> incomeTransactions = response.Content.ReadAsAsync<IEnumerable<TransactionDto>>().Result;
            ViewModel.TransactionList = incomeTransactions;
            ViewModel.SelectedFilter = filter;
            ViewModel.SelectedCategory = categoryName;

            return View(ViewModel);
        }*/

        // GET: Transaction/ListExpenses
        public ActionResult ListExpenses(int? transactionId = null, string filter = null, string categoryName = null)
        {
            ListTransactions ViewModel = new ListTransactions();

            // Get all categories for Expenses to render the dropdown
            string url = "categoryData/listCategoryByTransactionType?transactionTypeName=Expense";
            HttpResponseMessage response = client.GetAsync(url).Result;
            if (!response.IsSuccessStatusCode)
            {
                TempData["ErrorMessage"] = "An error occurred while listing all expense Categories. Please try again.";
                TempData["BackUrl"] = Url.Action("ListExpenses", "Transaction");
                return RedirectToAction("Error");
            }

            IEnumerable<CategoryDto> categories = response.Content.ReadAsAsync<IEnumerable<CategoryDto>>().Result;
            ViewModel.CategoryList = categories;

            // Check if id is provided
            if (transactionId.HasValue)
            {
                url = $"TransactionData/findTransactionById/{transactionId.Value}";
                response = client.GetAsync(url).Result;
                if (!response.IsSuccessStatusCode)
                {
                    TempData["ErrorMessage"] = "An error occurred while fetching the expense. Please try again.";
                    TempData["BackUrl"] = Url.Action("ListExpenses", "Transaction");
                    return RedirectToAction("Error");
                }
                var transaction = response.Content.ReadAsAsync<TransactionDto>().Result;
                ViewModel.TransactionList = new List<TransactionDto> { transaction };
            }
            else
            {
                // Build the query URL for transactions
                url = "TransactionData/findTransactions?transactionType=Expense";

                if (filter == "currentMonth")
                {
                    url += "&currentMonth=true";
                }
                else if (filter == "lastMonth")
                {
                    url += "&lastMonth=true";
                }

                if (!string.IsNullOrEmpty(categoryName))
                {
                    url += $"&categoryName={categoryName}";
                }

                response = client.GetAsync(url).Result;
                IEnumerable<TransactionDto> expenseTransactions = response.Content.ReadAsAsync<IEnumerable<TransactionDto>>().Result;
                ViewModel.TransactionList = expenseTransactions;
            }

            ViewModel.SelectedFilter = filter;
            ViewModel.SelectedCategory = categoryName;

            return View(ViewModel);
        }

        // GET: Transaction/ListIncomes
        public ActionResult ListIncomes(int? id = null, string filter = null, string categoryName = null)
        {
            ListTransactions ViewModel = new ListTransactions();

            // Get all categories for Incomes to render the dropdown
            string url = "categoryData/listCategoryByTransactionType?transactionTypeName=Income";
            HttpResponseMessage response = client.GetAsync(url).Result;
            if (!response.IsSuccessStatusCode)
            {
                TempData["ErrorMessage"] = "An error occurred while listing all income Categories. Please try again.";
                TempData["BackUrl"] = Url.Action("ListIncomes", "Transaction");
                return RedirectToAction("Error");
            }

            IEnumerable<CategoryDto> categories = response.Content.ReadAsAsync<IEnumerable<CategoryDto>>().Result;
            ViewModel.CategoryList = categories;

            // Check if id is provided
            if (id.HasValue)
            {
                url = $"TransactionData/findTransactionById/{id.Value}";
                response = client.GetAsync(url).Result;
                if (!response.IsSuccessStatusCode)
                {
                    TempData["ErrorMessage"] = "An error occurred while fetching the income. Please try again.";
                    TempData["BackUrl"] = Url.Action("ListIncomes", "Transaction");
                    return RedirectToAction("Error");
                }
                var transaction = response.Content.ReadAsAsync<TransactionDto>().Result;
                ViewModel.TransactionList = new List<TransactionDto> { transaction };
            }
            else
            {
                // Build the query URL for transactions
                url = "TransactionData/findTransactions?transactionType=Income";

                if (filter == "currentMonth")
                {
                    url += "&currentMonth=true";
                }
                else if (filter == "lastMonth")
                {
                    url += "&lastMonth=true";
                }

                if (!string.IsNullOrEmpty(categoryName))
                {
                    url += $"&categoryName={categoryName}";
                }

                response = client.GetAsync(url).Result;
                IEnumerable<TransactionDto> incomeTransactions = response.Content.ReadAsAsync<IEnumerable<TransactionDto>>().Result;
                ViewModel.TransactionList = incomeTransactions;
            }

            ViewModel.SelectedFilter = filter;
            ViewModel.SelectedCategory = categoryName;

            return View(ViewModel);
        }


        // GET: Transaction/NewExpense
        /*[Authorize]
        public ActionResult NewExpense()
        {
            //Get all categories for Expenses to render the dowpdown
            string url = "categoryData/listCategoryByTransactionType?transactionTypeName=Expense";
            HttpResponseMessage response = client.GetAsync(url).Result;

            IEnumerable<CategoryDto> categories = response.Content.ReadAsAsync<IEnumerable<CategoryDto>>().Result;

            return View(categories);
        }*/

        // GET: Transaction/NewIncome
        [Authorize]
        public ActionResult NewIncome()
        {
            //Get all categories for Incomes to render the dowpdown
            string url = "categoryData/listCategoryByTransactionType?transactionTypeName=Income";
            HttpResponseMessage response = client.GetAsync(url).Result;

            IEnumerable<CategoryDto> categories = response.Content.ReadAsAsync<IEnumerable<CategoryDto>>().Result;

            return View(categories);
        }

        // POST: Transaction/CreateExpense
        /*[HttpPost]
        [Authorize]
        public ActionResult CreateExpense(Transaction transaction)
        {
            GetApplicationCookie();
            transaction.TransactionDate = DateTime.SpecifyKind(transaction.TransactionDate, DateTimeKind.Utc);
            string url = "TransactionData/AddTransaction";

            string jsonpayload = jss.Serialize(transaction);
            Debug.WriteLine(jsonpayload);

            HttpContent content = new StringContent(jsonpayload);
            content.Headers.ContentType.MediaType = "application/json";

            HttpResponseMessage response = client.PostAsync(url, content).Result;

            if (response.IsSuccessStatusCode)
            {
                return RedirectToAction("ListExpenses");
            }
            else
            {
                TempData["ErrorMessage"] = "An error occurred while adding an expense. Please try again.";
                TempData["BackUrl"] = Url.Action("NewExpense", "Transaction");
                return RedirectToAction("Error");
            }
        }*/

        [HttpPost]
        [Authorize]
        public ActionResult CreateExpense(Transaction transaction)
        {
            return CreateOrUpdateExpense(transaction, false);
        }

        // GET: Transaction/NewExpense
        [Authorize]
        public ActionResult NewExpense(string title = null, int? categoryId = null, int? todoItemId = null)
        {
            // Get all categories for Expenses to render the dropdown
            string url = "categoryData/listCategoryByTransactionType?transactionTypeName=Expense";
            HttpResponseMessage response = client.GetAsync(url).Result;

            IEnumerable<CategoryDto> categories = response.Content.ReadAsAsync<IEnumerable<CategoryDto>>().Result;

            // Create a view model to pass the data to the view
            NewExpenseTransaction viewModel = new NewExpenseTransaction
            {
                Categories = categories,
                Title = title,
                CategoryId = categoryId,
                TransactionDate = DateTime.UtcNow,
                TodoItemId = todoItemId
            };

            return View(viewModel);
        }

        // Common method for creating or updating an expense
        private ActionResult CreateOrUpdateExpense(Transaction transaction, bool fromTodoItem)
        {
            GetApplicationCookie();
            transaction.TransactionDate = DateTime.SpecifyKind(transaction.TransactionDate, DateTimeKind.Utc);
            string url = "TransactionData/AddTransaction";

            string jsonpayload = jss.Serialize(transaction);
            Debug.WriteLine(jsonpayload);

            HttpContent content = new StringContent(jsonpayload);
            content.Headers.ContentType.MediaType = "application/json";

            HttpResponseMessage response = client.PostAsync(url, content).Result;

            if (response.IsSuccessStatusCode)
            {

                TransactionDto createdTransaction = response.Content.ReadAsAsync<TransactionDto>().Result;
                int newTransactionId = createdTransaction.TransactionId;

                if (transaction.TodoItemId.HasValue)
                {
                    string updateTodoUrl = $"TodoItemData/UpdateTodoItemWithTransactionId/{createdTransaction.TodoItemId.Value}/{createdTransaction.TransactionId}";

                    HttpResponseMessage updateResponse = client.PutAsync(updateTodoUrl, null).Result;

                    if (!updateResponse.IsSuccessStatusCode)
                    {
                        TempData["ErrorMessage"] = "An error occurred while updating the Todo item with the Transaction. Please try again.";
                        TempData["BackUrl"] = Url.Action("ListTodoItems", "TodoItem");
                        return RedirectToAction("Error");
                    }
                    return RedirectToAction("ListTodoItems", "TodoItem");
                }

                return RedirectToAction("ListExpenses");
            }
            else
            {
                TempData["ErrorMessage"] = "An error occurred while adding an expense. Please try again.";
                TempData["BackUrl"] = Url.Action("NewExpense", "Transaction");
                return RedirectToAction("Error");
            }
        }

        // POST: Transaction/CreateExpenseFromTodoItem
        [HttpPost]
        [Authorize]
        public ActionResult CreateExpenseFromTodoItem(Transaction transaction)
        {
            return CreateOrUpdateExpense(transaction, true);
        }

        // POST: Transaction/CreateIncome
        [HttpPost]
        [Authorize]
        public ActionResult CreateIncome(Transaction transaction)
        {
            GetApplicationCookie();
            transaction.TransactionDate = DateTime.SpecifyKind(transaction.TransactionDate, DateTimeKind.Utc);

            string url = "TransactionData/AddTransaction";

            string jsonpayload = jss.Serialize(transaction);

            HttpContent content = new StringContent(jsonpayload);
            content.Headers.ContentType.MediaType = "application/json";

            HttpResponseMessage response = client.PostAsync(url, content).Result;

            if (response.IsSuccessStatusCode)
            {
                return RedirectToAction("ListIncomes");
            }
            else
            {
                TempData["ErrorMessage"] = "An error occurred while adding an Income. Please try again.";
                TempData["BackUrl"] = Url.Action("NewIncome", "Transaction");
                return RedirectToAction("Error");
            }
        }

        // GET: Transaction/EditExpense/5
        [Authorize]
        public ActionResult EditExpense(int id)
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

        // GET: Transaction/EditIncome/5
        [Authorize]
        public ActionResult EditIncome(int id)
        {
            UpdateTransaction ViewModel = new UpdateTransaction();

            //Get the transaction details with the given transactionid
            string url = "TransactionData/findTransactionById/" + id;
            HttpResponseMessage response = client.GetAsync(url).Result;
            if (!response.IsSuccessStatusCode)
            {
                TempData["ErrorMessage"] = "An error occurred while finding an Income. Please try again.";
                TempData["BackUrl"] = Url.Action("ListIncomes", "Transaction");
                return RedirectToAction("Error");
            }

            TransactionDto selectedTransaction = response.Content.ReadAsAsync<TransactionDto>().Result;
            ViewModel.SelectedTransaction = selectedTransaction;

            //Get all categories for Incomes to render the dowpdown
            url = "categoryData/listCategoryByTransactionType?transactionTypeName=Income";
            response = client.GetAsync(url).Result;
            if (!response.IsSuccessStatusCode)
            {
                TempData["ErrorMessage"] = "An error occurred while listing all Income Categories. Please try again.";
                TempData["BackUrl"] = Url.Action("ListIncomes", "Transaction");
                return RedirectToAction("Error");
            }

            IEnumerable<CategoryDto> categories = response.Content.ReadAsAsync<IEnumerable<CategoryDto>>().Result;

            ViewModel.CategoryOptions = categories;

            return View(ViewModel);
        }

        // POST: Transaction/UpdateExpense/2
        [HttpPost]
        [Authorize]
        public ActionResult UpdateExpense(int id, Transaction transaction)
        {
            GetApplicationCookie();
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

        // POST: Transaction/UpdateIncome/2
        [HttpPost]
        [Authorize]
        public ActionResult UpdateIncome(int id, Transaction transaction)
        {
            GetApplicationCookie();
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
                return RedirectToAction("ListIncomes");
            }
            else
            {
                TempData["ErrorMessage"] = "An error occurred when updating an Income. Please try again.";
                TempData["BackUrl"] = Url.Action("ListIncomes", "Transaction");
                return RedirectToAction("Error");
            }
        }

        // POST: /Transaction/DeleteExpense/id
        [HttpPost]
        [Authorize]
        public ActionResult DeleteExpense(int id)
        {
            GetApplicationCookie();
            string url = "TransactionData/DeleteTransaction/" + id;

            HttpResponseMessage response = client.DeleteAsync(url).Result;

            if (response.IsSuccessStatusCode)
            {
                return RedirectToAction("ListExpenses");
            }
            else
            {
                TempData["ErrorMessage"] = "An error occurred when deleting an expense. Please try again.";
                TempData["BackUrl"] = Url.Action("ListExpenses", "Transaction");
                return RedirectToAction("Error");
            }
        }

        // POST: /Transaction/DeleteIncome/id
        [HttpPost]
        [Authorize]
        public ActionResult DeleteIncome(int id)
        {
            GetApplicationCookie();
            string url = "TransactionData/DeleteTransaction" + id;

            HttpResponseMessage response = client.DeleteAsync(url).Result;

            if (response.IsSuccessStatusCode)
            {
                return RedirectToAction("ListIncomes");
            }
            else
            {
                TempData["ErrorMessage"] = "An error occurred when deleting an Income. Please try again.";
                TempData["BackUrl"] = Url.Action("ListIncomes", "Transaction");
                return RedirectToAction("Error");
            }
        }

        //GET: Transaction/Error
        public ActionResult Error()
        {
            return View();
        }
    }
}