using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace HouseholdManagementSystem.Models.ViewModels
{
    public class CategoryDetails
    {
        public CategoryDto Category { get; set; }
        public IEnumerable<TodoItemDto> TodoItems { get; set; }
        public IEnumerable<TransactionDto> Transactions { get; set; }
    }
}