using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace HouseholdManagementSystem.Models.ViewModels
{
    public class NewExpenseTransaction
    {
        public IEnumerable<CategoryDto> Categories { get; set; }
        public string Title { get; set; }
        public int? CategoryId { get; set; }
        public DateTime TransactionDate { get; set; } = DateTime.UtcNow;

        public int? TodoItemId { get; set; }

    }

}