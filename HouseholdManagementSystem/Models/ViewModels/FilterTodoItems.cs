using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace HouseholdManagementSystem.Models.ViewModels
{
    public class FilterTodoItems
    {
        public IEnumerable<TodoItemDto> TodoItemsList { get; set; }

        // For statuses
        public IEnumerable<TodoItemStatusDto> StatusList { get; set; }
        public string SelectedStatus { get; set; }

        // For categories
        public IEnumerable<CategoryDto> CategoryList { get; set; }
        public string SelectedCategory { get; set; }

        // For owners assigned to tasks
        public IEnumerable<OwnerDto> AssignedToOwnersList { get; set; }
        public string SelectedAssignedToOwner { get; set; }

        // For owners who created tasks
        public IEnumerable<OwnerDto> CreatedByOwnersList { get; set; }
        public string SelectedCreatedByOwner { get; set; }

        public TransactionDto TransactionId { get; set; }
    }
}