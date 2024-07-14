using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace HouseholdManagementSystem.Models
{
    public class TodoItemStatus
    {
        [Key]
        public int StatusId { get; set; }
        public string Status{ get; set; }
    }

    //Data transfer object for TodoItemStatus
    public class TodoItemStatusDto
    {
        public int StatusId { get; set; }
        public string Status { get; set; }
    }
}