using ExpenseTracker.Application.Common.Dtos.Categories;
using ExpenseTracker.Application.Common.Dtos.UserCategories;
using ExpenseTracker.Application.Common.Errors.Controls;
using ExpenseTracker.Application.Common.Interfaces.Repositories;
using ExpenseTracker.Domain.Entities;
using FluentResults;
using MapsterMapper;
using MediatR;

namespace ExpenseTracker.Application.UserCategories.Queries.ListUserCategories
{
    public class ListUserCategoriesQueryHandler : IRequestHandler<ListUserCategoriesQuery, Result<List<CategoryDto>>>
    {
        private readonly IUnitOfWork _unitOfWork;
        private readonly IMapper _mapper;
        public ListUserCategoriesQueryHandler(
            IUnitOfWork unitOfWork,
            IMapper mapper)
        {
            _unitOfWork = unitOfWork;
            _mapper = mapper;
        }

        public async Task<Result<List<CategoryDto>>> Handle(ListUserCategoriesQuery request, CancellationToken cancellationToken)
        {
            if ((await _unitOfWork.UserRepository.GetByIdAsync(request.UserId)) is null)
            {
                return Result.Fail(new UserNotFoundError());
            }
            if ((await _unitOfWork.ActionTypeRepository.GetByIdAsync(request.ActionTypeId))
               is not ActionTypeEntity actionType)
            {
                return Result.Fail(new ActionTypeNotFoundError());
            }

            var categories = (await _unitOfWork.UserCategoryRepository
                    .FindAsync(x => x.UserId == request.UserId && x.Category.ActionTypeId == request.ActionTypeId))
                .Select(x => x.Category)
                .ToList();

            var categoriesDto = _mapper.Map<List<CategoryDto>>(categories);
            return Result.Ok(categoriesDto);
        }
    }
}
