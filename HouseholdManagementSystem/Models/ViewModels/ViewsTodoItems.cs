using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace HouseholdManagementSystem.Models.ViewModels
{
    public class ViewsTodoItems
    {
        public IEnumerable<TodoItemDto> TodoItems { get; set; }

        public TodoItemStatusDto Status { get; set; }

        public CategoryDto Category { get; set; }

        public OwnerDto AssignedTo { get; set; }

        public OwnerDto CreatedBy { get; set; }

        public TransactionDto Transaction { get; set; }
    }
}