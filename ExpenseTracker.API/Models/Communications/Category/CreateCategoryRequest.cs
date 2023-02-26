namespace ExpenseTracker.API.Models.Communications.Category
{
    public class CreateCategoryRequest
    {
        public string CategoryName { get; set; }
        public int AcctionTypeId { get; set; }
    }
}
