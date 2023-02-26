using ExpenseTracker.Application.Common.Dtos.UserCategories;
using ExpenseTracker.Application.Common.Errors.Controls;
using ExpenseTracker.Application.Common.Interfaces.Repositories;
using ExpenseTracker.Application.Common.Interfaces.Services;
using ExpenseTracker.Domain.Entities;
using FluentResults;
using MapsterMapper;
using MediatR;

namespace ExpenseTracker.Application.UserCategories.Commands.CreateUserCategory
{
    public class CreateUserCategoryCommandHandler : IRequestHandler<CreateUserCategoryCommand, Result<UserCategoryDto>>
    {
        private readonly IUnitOfWork _unitOfWork;
        private readonly IDateTimeProvider _dateTimeProvider;
        private readonly IMapper _mapper;
        public CreateUserCategoryCommandHandler(
            IUnitOfWork unitOfWork,
            IDateTimeProvider dateTimeProvider,
            IMapper mapper)
        {
            _unitOfWork = unitOfWork;
            _dateTimeProvider = dateTimeProvider;
            _mapper = mapper;
        }
        public async Task<Result<UserCategoryDto>> Handle(CreateUserCategoryCommand request, CancellationToken cancellationToken)
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

            var category = await _unitOfWork.CategoryRepository
                .FirstOrDefaultAsync(x => x.CategoryName == request.CategoryName && x.ActionTypeId == request.ActionTypeId);

            if (category is not null)
            {
                if ((await _unitOfWork.UserCategoryRepository.FindAsync(x => x.UserId == request.UserId && x.CategoryId == category.Id)).Any())
                {
                    return Result.Fail(new DuplicateUserCategoryError());
                }
            }
            else
            {
                category = new CategoryEntity
                {
                    CategoryName = request.CategoryName,
                    ImageUri = string.Empty,
                    ActionTypeId = request.ActionTypeId,
                    CreatedOnUtc = _dateTimeProvider.Now,
                };
                _unitOfWork.CategoryRepository.Insert(category);
            }
            AddUserCategory(request.UserId, category);
            await _unitOfWork.CommitAsync();
            return _mapper.Map<UserCategoryDto>(category);
        }

        private void AddUserCategory(int userId, CategoryEntity category)
        {
            var userCategoryEntity = new UserCategoryEntity
            {
                UserId = userId,
                CategoryId = category.Id,
                CreatedOnUtc = _dateTimeProvider.Now
            };
            _unitOfWork.UserCategoryRepository.Insert(userCategoryEntity);
        }
    }
}
