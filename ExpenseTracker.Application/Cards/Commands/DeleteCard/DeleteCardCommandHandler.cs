using ExpenseTracker.Application.Common.Errors.Controls;
using ExpenseTracker.Application.Common.Interfaces.Repositories;
using ExpenseTracker.Application.Common.Interfaces.Services;
using ExpenseTracker.Domain.Entities;
using FluentResults;
using MapsterMapper;
using MediatR;

namespace ExpenseTracker.Application.Cards.Commands.DeleteCard
{
    public class DeleteCardCommandHandler : IRequestHandler<DeleteCardCommand, Result<bool>>
    {
        private readonly IUnitOfWork _unitOfWork;
        private readonly IDateTimeProvider _dateTimeProvider;
        private readonly IMapper _mapper;
        public DeleteCardCommandHandler(
            IUnitOfWork unitOfWork,
            IDateTimeProvider dateTimeProvider,
            IMapper mapper)
        {
            _unitOfWork = unitOfWork;
            _dateTimeProvider = dateTimeProvider;
            _mapper = mapper;
        }

        public async Task<Result<bool>> Handle(DeleteCardCommand request, CancellationToken cancellationToken)
        {
            if ((await _unitOfWork.UserRepository.GetByIdAsync(request.UserId)) is null)
            {
                return Result.Fail(new UserNotFoundError());
            }

            if ((await _unitOfWork.CardRepository
                    .FindAsync(x => x.UserId == request.UserId && x.Id == request.CardId))
                ?.FirstOrDefault() is not CardEntity cardEntity)
            {
                return Result.Fail(new CardNotFoundError());
            }

            _unitOfWork.CardRepository.Delete(cardEntity);
            await _unitOfWork.CommitAsync();
            return Result.Ok();
        }
    }
}
