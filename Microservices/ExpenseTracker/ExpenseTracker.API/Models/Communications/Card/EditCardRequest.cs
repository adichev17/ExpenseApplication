﻿using System.Runtime.Serialization;
using System.Text.Json.Serialization;

namespace ExpenseTracker.API.Models.Communications.Card
{
    public class EditCardRequest
    {
        public int CardId { get; set; }
        public int ColorId { get; set; }
        public string CardName { get; set; }
    }
}
