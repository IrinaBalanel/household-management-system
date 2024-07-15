using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace HouseholdManagementSystem.Models
{
    public class TodoItem
    {
        [Key]
        public int TodoItemId { get; set; }
        public string TodoItemDescription { get; set; }
        [ForeignKey("Status")]
        public int StatusId { get; set; }
        public virtual TodoItemStatus Status { get; set; }
        [ForeignKey("AssignedTo")]
        public int AssignedToOwnerId { get; set; }
        public virtual Owner AssignedTo { get; set; }
        [ForeignKey("CreatedBy")]
        public int CreatedByOwnerId { get; set; }
        public virtual Owner CreatedBy { get; set; }
    }

    //Data transfer object for TodoItem
    public class TodoItemDto
    {
        public int TodoItemId { get; set; }
        public string TodoItemDescription { get; set; }
        public int StatusId { get; set; }
        public string Status { get; set; }
        public int AssignedToOwnerId { get; set; }
        public string AssignedTo { get; set; }
        public int CreatedByOwnerId { get; set; }
        public string CreatedBy { get; set; }
    }
}