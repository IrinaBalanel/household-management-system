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
    public class CategoryController : Controller
    {
        private static readonly HttpClient client;
        private JavaScriptSerializer jss = new JavaScriptSerializer();

        static CategoryController()
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

        // GET: Category
        public ActionResult Index()
        {
            return View();
        }

        // GET: Category/ListCategories
        public ActionResult ListCategories()
        {
            ListCategories viewModel = new ListCategories();

            // Get Income Categories
            string url = "CategoryData/listCategoryByTransactionType?transactionTypeName=Income";
            HttpResponseMessage response = client.GetAsync(url).Result;
            if (response.IsSuccessStatusCode)
            {
                viewModel.IncomeCategoryList = response.Content.ReadAsAsync<IEnumerable<CategoryDto>>().Result;
            }

            // Get Expense Categories
            url = "CategoryData/listCategoryByTransactionType?transactionTypeName=Expense";
            response = client.GetAsync(url).Result;
            if (response.IsSuccessStatusCode)
            {
                viewModel.ExpenseCategoryList = response.Content.ReadAsAsync<IEnumerable<CategoryDto>>().Result;
            }

            return View(viewModel);
        }

        // POST: Category/AddCategory
        [HttpPost]
        [Authorize]
        public ActionResult AddCategory(Category category)
        {
            GetApplicationCookie();
            string url = "CategoryData/addCategory";

            string jsonpayload = jss.Serialize(category);
            Debug.WriteLine(jsonpayload);

            HttpContent content = new StringContent(jsonpayload);
            content.Headers.ContentType.MediaType = "application/json";

            HttpResponseMessage response = client.PostAsync(url, content).Result;
            if (response.IsSuccessStatusCode)
            {
                return RedirectToAction("ListCategories");
            }
            else
            {
                TempData["ErrorMessage"] = "An error occurred while adding the category. Please try again.";
                return RedirectToAction("ListCategories");
            }
        }

        // POST: Category/UpdateCategory
        [HttpPost]
        [Authorize]
        public ActionResult UpdateCategory(Category category)
        {
            GetApplicationCookie();
            string url = $"CategoryData/updateCategory/{category.CategoryId}";
            string jsonpayload = jss.Serialize(category);

            HttpContent content = new StringContent(jsonpayload);
            content.Headers.ContentType.MediaType = "application/json";

            HttpResponseMessage response = client.PutAsync(url, content).Result;
            if (response.IsSuccessStatusCode)
            {
                return RedirectToAction("ListCategories");
            }
            else
            {
                TempData["ErrorMessage"] = "An error occurred while updating the category. Please try again.";
                return RedirectToAction("ListCategories");
            }
        }

        // POST: Category/DeleteCategory
        [HttpPost]
        [Authorize]
        public ActionResult DeleteCategory(int categoryId)
        {
            GetApplicationCookie();
            string url = $"CategoryData/deleteCategory/{categoryId}";
            HttpResponseMessage response = client.DeleteAsync(url).Result;
            if (response.IsSuccessStatusCode)
            {
                return RedirectToAction("ListCategories");
            }
            else
            {
                TempData["ErrorMessage"] = "This Category cannot be deleted as it has associated Transactions";
                TempData["BackUrl"] = Url.Action("ListCategories", "Category");
                return RedirectToAction("Error");
            }
        }

        // GET: Category/CategoryDetails
        [Authorize]
        public ActionResult CategoryDetails(int id)
        {
            string url = $"CategoryData/GetCategoryDetails/{id}";
            HttpResponseMessage response = client.GetAsync(url).Result;

            if (response.IsSuccessStatusCode)
            {
                var categoryDetails = response.Content.ReadAsAsync<CategoryDetails>().Result;
                return View(categoryDetails);
            }
            else
            {
                TempData["ErrorMessage"] = "An error occurred while fetching the category details. Please try again.";
                TempData["BackUrl"] = Url.Action("ListCategories", "Category");
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