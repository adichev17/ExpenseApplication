using ExpenseTracker.Application.Common.Dtos.Cards;
using ExpenseTracker.Application.Common.Interfaces.Repositories;
using ExpenseTracker.Application.Common.Interfaces.Services;
using ExpenseTracker.Application.Common.Errors.Controls;
using ExpenseTracker.Domain.Entities;
using FluentResults;
using MapsterMapper;
using MediatR;

namespace ExpenseTracker.Application.Cards.Commands.CreateCard
{
    public class CreateCardCommandHandler : IRequestHandler<CreateCardCommand, Result<CardDto>>
    {
        private readonly IUnitOfWork _unitOfWork;
        private readonly IDateTimeProvider _dateTimeProvider;
        private readonly IMapper _mapper;
        private readonly IUserProvider _userProvider;
        public CreateCardCommandHandler(IUnitOfWork unitOfWork, 
            IDateTimeProvider dateTimeProvider, 
            IMapper mapper,
            IUserProvider userProvider)
        {
            _unitOfWork = unitOfWork;
            _dateTimeProvider = dateTimeProvider;
            _mapper = mapper;
            _userProvider = userProvider;
        }

        public async Task<Result<CardDto>> Handle(CreateCardCommand request, CancellationToken cancellationToken)
        {
            var userId = _userProvider.GetUserId();

            if ((await _unitOfWork.UserRepository.GetByIdAsync(userId)) is null)
            {
                return Result.Fail(new UserNotFoundError());
            }

            if ((await _unitOfWork.ColorRepository.GetByIdAsync(request.ColorId)) is null)
            {
                return Result.Fail(new ColorNotFoundError());
            }
            if ((await _unitOfWork.CardRepository
                .FindAsync(x => x.UserId == userId && x.CardName == request.CardName))
                .FirstOrDefault() is not null)
            {
                return Result.Fail(new DuplicateCardError());
            }

            var cardEntity = new CardEntity
            {
                CardName = request.CardName,
                UserId = userId,
                ColorId = request.ColorId,
                Balance = 0,
                CreatedOnUtc = _dateTimeProvider.Now
            };       

            _unitOfWork.CardRepository.Insert(cardEntity);
            await _unitOfWork.CommitAsync();
            
            var cardDto = _mapper.Map<CardDto>(cardEntity);

            return Result.Ok(cardDto);
        }
    }
}
