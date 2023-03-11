using ExpenseTracker.Application.Common.Dtos.Cards;
using ExpenseTracker.Application.Common.Dtos.Expenses;
using ExpenseTracker.Application.Common.Interfaces.Repositories;
using FluentResults;
using MapsterMapper;
using MediatR;

namespace ExpenseTracker.Application.Expenses.Queries.ListTransactions
{
    public class ListTransactionsQueryHandler : IRequestHandler<ListTransactionsQuery, Result<List<CardTransactionsDto>>>
    {
        private readonly ITransactionRepository _transactionRepository;
        private readonly IMapper _mapper;
        public ListTransactionsQueryHandler(
            ITransactionRepository transactionRepository,
            IMapper mapper)
        {
            _transactionRepository = transactionRepository;
            _mapper = mapper;
        }

        //TODO: to mapster
        public async Task<Result<List<CardTransactionsDto>>> Handle(ListTransactionsQuery request, CancellationToken cancellationToken)
        {
            var transactions = _transactionRepository.GetAll(request.UserId, request.CardId, request.Rows).ToList();
            var transactionsDto = new List<CardTransactionsDto>();
            foreach (var card in transactions.Select(x => x.Card).Distinct())
            {
                var cardTransactions = transactions.Where(x => x.CardId == card.Id);
                var cardTransactionsDto = new List<TransactionWithoutCardDto>();

                cardTransactionsDto.AddRange(cardTransactions.Select(_mapper.Map<TransactionWithoutCardDto>));

                transactionsDto.Add(new CardTransactionsDto
                {
                    Card = _mapper.Map<CardDto>(card),
                    Transactions = cardTransactionsDto
                });
            }

            return Result.Ok(transactionsDto);
        }
    }
}
