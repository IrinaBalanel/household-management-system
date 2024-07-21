using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace HouseholdManagementSystem.Models.ViewModels
{
    public class UpdateTodoItem
    {
        public IEnumerable<CategoryDto> CategoryList { get; set; }

        public IEnumerable<OwnerDto> OwnersList { get; set; }

        public IEnumerable<TodoItemStatusDto> TodoItemStatusDtoList { get; set; }

        public TodoItemDto SelectedTodoItem { get; set; }
    }
}