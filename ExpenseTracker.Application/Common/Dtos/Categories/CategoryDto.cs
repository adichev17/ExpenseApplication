namespace ExpenseTracker.Application.Common.Dtos.Categories
{
    public class CategoryDto
    {
        public int Id { get; set; } 
        public string CategoryName { get; set; }

        public string ImageUri { get; set; }

        public int ActionTypeId { get; set; }
    }
}
