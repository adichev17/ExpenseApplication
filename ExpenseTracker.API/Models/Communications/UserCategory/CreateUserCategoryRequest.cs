namespace ExpenseTracker.API.Models.Communications.UserCategory
{
    public class CreateUserCategoryRequest
    {
        public int UserId { get; set; }
        public string CategoryName { get; set; }
        public int ActionTypeId { get; set; }
    }
}
