using ExpenseTracker.Application.Common.Dtos.Cards;
using ExpenseTracker.Application.Common.Dtos.Categories;
using ExpenseTracker.Application.Common.Dtos.Expenses;
using ExpenseTracker.Application.Common.Interfaces.Repositories;
using FluentResults;
using MediatR;

namespace ExpenseTracker.Application.Expenses.Queries.ListTransactions
{
    public class ListTransactionsQueryHandler : IRequestHandler<ListTransactionsQuery, Result<List<CardTransactionsDto>>>
    {
        private readonly ITransactionRepository _transactionRepository;
        public ListTransactionsQueryHandler(
            ITransactionRepository transactionRepository)
        {
            _transactionRepository = transactionRepository;
        }

        //TODO: to mapster
        public async Task<Result<List<CardTransactionsDto>>> Handle(ListTransactionsQuery request, CancellationToken cancellationToken)
        {
            var transactions = _transactionRepository.GetAll(request.UserId, request.CardId, request.Rows).ToList();
            var transactionsDto = new List<CardTransactionsDto>();
            foreach (var card in transactions.Select(x => x.Card).Distinct())
            {
                var cardTransactions = transactions.Where(x => x.CardId == card.Id);
                var cardTransactionsDto = new List<TransactionDto>();
                foreach (var cardTransaction in cardTransactions)
                {
                    cardTransactionsDto.Add(new TransactionDto
                    {
                        Id = cardTransaction.Id,
                        Date = cardTransaction.Date,
                        Amount = cardTransaction.Amount,
                        Category = new CategoryDto
                        {
                            Id = cardTransaction.Category.Id,
                            CategoryName = cardTransaction.Category.CategoryName,
                            ImageUri = cardTransaction.Category.ImageUri,
                            ActionTypeId = cardTransaction.Category.ActionTypeId
                        }
                    });
                }

                transactionsDto.Add(new CardTransactionsDto
                {
                    Card = new CardDto()
                    {
                        Id = card.Id,
                        Balance = card.Balance,
                        CardName = card.CardName,
                        ColorId = card.ColorId,
                    },
                    Transactions = cardTransactionsDto
                });
            }

            return Result.Ok(transactionsDto);
        }
    }
}
