using ExpenseTracker.Application.Common.Dtos.Cards;
using ExpenseTracker.Application.Common.Errors.Controls;
using ExpenseTracker.Application.Common.Interfaces.Repositories;
using ExpenseTracker.Domain.Entities;
using FluentResults;
using MapsterMapper;
using MediatR;

namespace ExpenseTracker.Application.Cards.Queries.GetCard
{
    public class GetCardQueryHandler : IRequestHandler<GetCardQuery, Result<CardDto>>
    {
        private readonly IUnitOfWork _unitOfWork;
        private readonly IMapper _mapper;

        public GetCardQueryHandler(
            IUnitOfWork unitOfWork,
            IMapper mapper)
        {
            _unitOfWork = unitOfWork;
            _mapper = mapper;
        }

        public async Task<Result<CardDto>> Handle(GetCardQuery request, CancellationToken cancellationToken)
        {
            if ((await _unitOfWork.CardRepository
                .FindAsync(x => x.UserId == request.UserId && x.Id == request.CardId))
                .FirstOrDefault() is not CardEntity cardEntity)
            {
                return Result.Fail(new CardNotFoundError());
            }

            var cardDto = _mapper.Map<CardDto>(cardEntity);
            return Result.Ok(cardDto);
        }
    }
}
