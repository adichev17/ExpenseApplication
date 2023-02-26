using ExpenseTracker.Application.Common.Dtos.Categories;
using ExpenseTracker.Application.Common.Errors.Controls;
using ExpenseTracker.Application.Common.Interfaces.Repositories;
using ExpenseTracker.Application.Common.Interfaces.Services;
using ExpenseTracker.Domain.Entities;
using FluentResults;
using MapsterMapper;
using MediatR;

namespace ExpenseTracker.Application.Categories.Commands
{
    public class CreateCategoryCommandHandler : IRequestHandler<CreateCategoryCommand, Result<CategoryDto>>
    {
        private readonly IUnitOfWork _unitOfWork;
        private readonly IDateTimeProvider _dateTimeProvider;
        private readonly IMapper _mapper;
        public CreateCategoryCommandHandler(
            IUnitOfWork unitOfWork,
            IDateTimeProvider dateTimeProvider,
            IMapper mapper)
        {
            _unitOfWork = unitOfWork;
            _dateTimeProvider = dateTimeProvider;
            _mapper = mapper;
        }

        public async Task<Result<CategoryDto>> Handle(CreateCategoryCommand request, CancellationToken cancellationToken)
        {
            if ((await _unitOfWork.ActionTypeRepository.GetByIdAsync(request.ActionTypeId)) 
                is not ActionTypeEntity actionType)
            {
                return Result.Fail(new ActionTypeNotFoundError());
            }

            if ((await _unitOfWork.CategoryRepository
                .FindAsync(x => x.CategoryName == request.CategoryName && request.ActionTypeId == request.ActionTypeId))
                .Any())
            {
                return Result.Fail(new ActionTypeNotFoundError());
            }

            var categoryEntity = new CategoryEntity
            {
                CategoryName = request.CategoryName,
                ActionTypeId = request.ActionTypeId,
                ImageUri = string.Empty,
                CreatedOnUtc = _dateTimeProvider.Now
            };

            _unitOfWork.CategoryRepository.Insert(categoryEntity);
            await _unitOfWork.CommitAsync();
            
            return _mapper.Map<CategoryDto>(categoryEntity);
        }
    }
}
