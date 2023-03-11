using ExpenseTracker.Application.Common.Dtos.Expenses;
using ExpenseTracker.Application.Common.Errors.Controls;
using ExpenseTracker.Application.Common.Interfaces.Repositories;
using FluentResults;
using MapsterMapper;
using MediatR;

namespace ExpenseTracker.Application.Expenses.Queries.GetTransaction
{
    public record GetTransactionQueryHandler : IRequestHandler<GetTransactionQuery, Result<TransactionDto>>
    {
        private readonly ITransactionRepository _transactionRepository;
        private readonly IMapper _mapper;
        public GetTransactionQueryHandler(
            ITransactionRepository transactionRepository,
            IMapper mapper)
        {
            _transactionRepository = transactionRepository;
            _mapper = mapper;
        }
        public async Task<Result<TransactionDto>> Handle(GetTransactionQuery request, CancellationToken cancellationToken)
        {
            var transaction = await _transactionRepository.GetByIdAsync(request.TransactionId);
            if (transaction is null) 
            {
                return Result.Fail(new NotFoundTransactionError());
            }

            var transactionDto = _mapper.Map<TransactionDto>(transaction);
            return Result.Ok(transactionDto);
        }
    }
}
