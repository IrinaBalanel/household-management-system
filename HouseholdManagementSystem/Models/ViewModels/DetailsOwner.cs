﻿using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace HouseholdManagementSystem.Models.ViewModels
{
    public class DetailsOwner
    {
        public OwnerDto SelectedOwner { get; set; }
        public IEnumerable<ChoreDto> RelatedChores { get; set; }
    }
}