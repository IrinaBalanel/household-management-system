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

        [ForeignKey("Category")]
        public int CategoryId { get; set; }
        public virtual Category Category { get; set; }

        //TransactionId is a nullable integer
        public int? TransactionId { get; set; }
        public virtual Transaction Transaction { get; set; }
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
        public int CategoryId { get; set; }
        public string CategoryName { get; set; }
        public int? TransactionId { get; set; }
    }
}