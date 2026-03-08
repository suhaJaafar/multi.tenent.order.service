using AutoMapper;
using MultiTenantOrderService.Application.Abstractions.Messaging;
using MultiTenantOrderService.Application.GetUser;
using MultiTenantOrderService.Domain.Abstractions;
using MultiTenantOrderService.Domain.Identity;
using MultiTenantOrderService.Domain.Identity.Entities;
using MultiTenantOrderService.Domain.Identity.ObjectValues;

namespace MultiTenantOrderService.Application.CreateUser;

internal sealed class CreateUserCommandHandler : ICommandHandler<CreateUserCommand, UserResponse>
{
    private readonly IUserRepository _userRepository;
    private readonly IUnitOfWork _unitOfWork;
    private readonly IMapper _mapper;

    public CreateUserCommandHandler(
        IUserRepository userRepository,
        IUnitOfWork unitOfWork,
        IMapper mapper)
    {
        _userRepository = userRepository;
        _unitOfWork = unitOfWork;
        _mapper = mapper;
    }

    public async Task<Result<UserResponse>> Handle(
        CreateUserCommand request,
        CancellationToken cancellationToken)
    {
        var email = new Email(request.Email);
    
        // Check if email is unique using efficient AnyAsync query
        var isEmailUnique = await _userRepository.IsEmailUniqueAsync(email, cancellationToken);
        if (!isEmailUnique)
        {
            return Result.Failure<UserResponse>(UserErrors.EmailNotUnique);
        }

        var user = User.Create(
            request.Id,
            request.Name,
            new Email(request.Email),
            new Password(request.Password),
            request.UserType,
            request.TenentName);

        _userRepository.Add(user);

        await _unitOfWork.SaveChangesAsync(cancellationToken);

        // Map the created user to UserResponse
        var response = _mapper.Map<UserResponse>(user);
        
        return Result.Success(response);
    }
}

