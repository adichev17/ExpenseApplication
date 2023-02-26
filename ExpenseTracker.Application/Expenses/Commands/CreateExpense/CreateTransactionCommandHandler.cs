using ExpenseTracker.Application.Common.Errors.Controls;
using ExpenseTracker.Application.Common.Interfaces.Repositories;
using ExpenseTracker.Application.Common.Interfaces.Services;
using ExpenseTracker.Domain.Entities;
using FluentResults;
using MediatR;

namespace ExpenseTracker.Application.Expenses.Commands.CreateExpense
{
    public class CreateTransactionCommandHandler : IRequestHandler<CreateTransactionCommand, Result<bool>>
    {
        private readonly IUnitOfWork _unitOfWork;
        private readonly ITransactionRepository _transactionRepository;
        private readonly IDateTimeProvider _dateTimeProvider;
        public CreateTransactionCommandHandler(
            IUnitOfWork unitOfWork,
            ITransactionRepository transactionRepository,
            IDateTimeProvider dateTimeProvider)
        {
            _unitOfWork = unitOfWork;
            _transactionRepository = transactionRepository;
            _dateTimeProvider = dateTimeProvider;
        }
        public async Task<Result<bool>> Handle(CreateTransactionCommand request, CancellationToken cancellationToken)
        {
            if ((await _unitOfWork.CardRepository.GetByIdAsync(request.CardId)) is not CardEntity card)
            {
                return Result.Fail(new CardNotFoundError());
            }

            if ((await _unitOfWork.UserCategoryRepository
                .FirstOrDefaultAsync(x => x.UserId == card.UserId && x.CategoryId == request.CategoryId)) is null )
            {
                return Result.Fail(new NotFoundUserCategoryError());
            }

            if ((await _unitOfWork.CategoryRepository.GetByIdAsync(request.CategoryId)) is not CategoryEntity category)
            {
                return Result.Fail(new CategoryNotFoundError());
            }

            var transactionEntity = new TransactionEntity()
            {
                CardId = request.CardId,
                Amount = request.Amount,
                Comment = request.Comment,
                CreatedOnUtc = _dateTimeProvider.Now,
                CategoryId = request.CategoryId,
                Date = _dateTimeProvider.Now
            };

            _transactionRepository.AddTransaction(transactionEntity);

            return Result.Ok(true);
        }
    }
}
