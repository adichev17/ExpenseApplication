using ExpenseTracker.Application.Common.Dtos.Cards;
using ExpenseTracker.Application.Common.Errors.Controls;
using ExpenseTracker.Application.Common.Interfaces.Repositories;
using ExpenseTracker.Application.Common.Interfaces.Services;
using ExpenseTracker.Domain.Entities;
using FluentResults;
using MapsterMapper;
using MediatR;

namespace ExpenseTracker.Application.Cards.Commands.EditCard
{
    public class EditCardCommandHandler : IRequestHandler<EditCardCommand, Result<CardDto>>
    {
        private readonly IUnitOfWork _unitOfWork;
        private readonly IDateTimeProvider _dateTimeProvider;
        private readonly IMapper _mapper;

        public EditCardCommandHandler(
            IUnitOfWork unitOfWork,
            IDateTimeProvider dateTimeProvider,
            IMapper mapper)
        {
            _unitOfWork = unitOfWork;
            _dateTimeProvider = dateTimeProvider;
            _mapper = mapper;
        }

        public async Task<Result<CardDto>> Handle(EditCardCommand request, CancellationToken cancellationToken)
        {
            if ((await _unitOfWork.UserRepository.GetByIdAsync(request.UserId)) is null)
            {
                return Result.Fail(new UserNotFoundError());
            }

            if ((await _unitOfWork.ColorRepository.GetByIdAsync(request.ColorId)) is null)
            {
                return Result.Fail(new ColorNotFoundError());
            }

            var cards = (await _unitOfWork.CardRepository.FindAsync(x => x.UserId == request.UserId)).ToList();
            var cardEntity = cards.FirstOrDefault(x => x.Id == request.CardId);
            if (cardEntity is null)
            {
                return Result.Fail(new CardNotFoundError());
            }

            //checking for the existence of a card with a name from request
            if (cards.Any(x => x.CardName == request.CardName && x.Id != request.CardId))
            {
                return Result.Fail(new DuplicateCardError());
            }

            cardEntity.ColorId = request.ColorId;
            cardEntity.CardName = request.CardName;
            _unitOfWork.CardRepository.Update(cardEntity);
            await _unitOfWork.CommitAsync();

            return _mapper.Map<CardDto>(cardEntity);
        }
    }
}
