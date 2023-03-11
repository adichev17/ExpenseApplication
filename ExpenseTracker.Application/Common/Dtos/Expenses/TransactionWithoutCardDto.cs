﻿using ExpenseTracker.Application.Common.Dtos.Categories;

namespace ExpenseTracker.Application.Common.Dtos.Expenses
{
    public class TransactionWithoutCardDto
    {
        public int Id { get; set; }
        public string Comment { get; set; }
        public decimal Amount { get; set; }
        public CategoryDto Category { get; set; }
        public DateTime Date { get; set; }
    }
}
