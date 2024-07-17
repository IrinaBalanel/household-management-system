using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations.Schema;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Web;

namespace HouseholdManagementSystem.Models
{
    public class Transaction
    {
        [Key]
        public int TransactionId { get; set; }
        public string Title { get; set; }
        public decimal Amount { get; set; }
        public DateTime TransactionDate { get; set; }

        [ForeignKey("Category")]
        public int CategoryId { get; set; }
        public virtual Category Category { get; set; }

        //TodoItemId is a nullable integer
        public int? TodoItemId { get; set; }
        public virtual TodoItem TodoItem { get; set; }
    }

    public class TransactionDto
    {
        public int TransactionId { get; set; }
        public string Title { get; set; }
        public decimal Amount { get; set; }
        public DateTime TransactionDate { get; set; }

        public string CategoryName { get; set; }

        public string TransactionTypeName { get; set; }

        public int? TodoItemId { get; set; }
    }
}