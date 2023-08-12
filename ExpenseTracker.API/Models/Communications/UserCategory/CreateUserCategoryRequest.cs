﻿using System.Runtime.Serialization;
using System.Text.Json.Serialization;

namespace ExpenseTracker.API.Models.Communications.UserCategory
{
    public class CreateUserCategoryRequest
    {
        [IgnoreDataMember]
        [JsonIgnore]
        public Guid UserId { get; set; }
        public string CategoryName { get; set; }
        public int ActionTypeId { get; set; }
    }
}
