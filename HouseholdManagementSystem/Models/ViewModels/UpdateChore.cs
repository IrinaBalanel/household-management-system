﻿using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace HouseholdManagementSystem.Models.ViewModels
{
    public class UpdateChore
    {
        public ChoreDto SelectedChore { get; set; }

        public IEnumerable<OwnerDto> OwnerOptions { get; set; }
    }
}