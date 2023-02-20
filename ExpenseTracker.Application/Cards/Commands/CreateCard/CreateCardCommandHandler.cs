using ExpenseTracker.Application.Common.Dtos.Cards;
using ExpenseTracker.Application.Common.Interfaces.Repositories;
using ExpenseTracker.Application.Common.Interfaces.Services;
using ExpenseTracker.Domain.Common.Errors.Controls;
using FluentResults;
using MediatR;

namespace ExpenseTracker.Application.Cards.Commands.CreateCard
{
    public class CreateCardCommandHandler : IRequestHandler<CreateCardCommand, Result<CreateCardDto>>
    {
        private readonly IUnitOfWork _unitOfWork;
        private readonly IDateTimeProvider _dateTimeProvider;
        public CreateCardCommandHandler(IUnitOfWork unitOfWork, IDateTimeProvider dateTimeProvider)
        {
            _unitOfWork = unitOfWork;
            _dateTimeProvider = dateTimeProvider;
        }

        public async Task<Result<CreateCardDto>> Handle(CreateCardCommand request, CancellationToken cancellationToken)
        {
            if ((await _unitOfWork.UserRepository.GetByIdAsync(request.UserId)) is null)
            {
                return Result.Fail<CreateCardDto>(new UserNotFoundError());
            }

            if ((await _unitOfWork.ColorRepository.GetByIdAsync(request.ColorId)) is null)
            {
                return Result.Fail<CreateCardDto>(new ColorNotFoundError());
            }

            return Result.Ok();
        }
    }
}
