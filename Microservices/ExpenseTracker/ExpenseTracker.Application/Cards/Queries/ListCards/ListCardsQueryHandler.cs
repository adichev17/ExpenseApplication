using System.ComponentModel.DataAnnotations;
using ExpenseTracker.Application.Common.Dtos.Cards;
using ExpenseTracker.Application.Common.Interfaces.Repositories;
using ExpenseTracker.Application.Common.Interfaces.Services;
using FluentResults;
using MapsterMapper;
using MediatR;

namespace ExpenseTracker.Application.Cards.Queries.ListCards
{
    public class ListCardsQueryHandler : IRequestHandler<ListCardsQuery, Result<List<CardDto>>>
    {
        private readonly IUnitOfWork _unitOfWork;
        private readonly IMapper _mapper;
        public ListCardsQueryHandler(
            IUnitOfWork unitOfWork,
            IMapper mapper)
        {
            _unitOfWork = unitOfWork;
            _mapper = mapper;
        }
        public async Task<Result<List<CardDto>>> Handle(ListCardsQuery request, CancellationToken cancellationToken)
        {
            var cards = (await _unitOfWork.CardRepository.FindAsync(x => x.UserId == request.UserId)).ToList();
            var cardDtos = new List<CardDto>();
            cardDtos.AddRange(cards.Any() ? cards.Select(_mapper.Map<CardDto>) : Enumerable.Empty<CardDto>());
            return Result.Ok(cardDtos);
        }
    }
}
