using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace HouseholdManagementSystem.Models.ViewModels
{
    public class TransactionTypeTotal
    {
        public String TransactionTypeName { get; set; }
        public int TransactionTypeId { get; set; }
        public decimal TotalAmount { get; set; }
    }
}