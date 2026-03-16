using AutoMapper;
using IdentityService.Application.Abstractions.Messaging;
using IdentityService.Application.GetUser;
using IdentityService.Domain.Abstractions;
using IdentityService.Domain.Identity;
using IdentityService.Domain.Identity.Entities;
using IdentityService.Domain.Identity.ObjectValues;
using Scrypt;

namespace IdentityService.Application.CreateUser;

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

        // Hash the password using Scrypt
        var scryptEncoder = new ScryptEncoder();
        var hashedPassword = scryptEncoder.Encode(request.Password);

        var user = User.Create(
            request.Id,
            request.Name,
            new Email(request.Email),
            new Password(hashedPassword),
            request.UserType,
            request.TenentName);

        _userRepository.Add(user);

        await _unitOfWork.SaveChangesAsync(cancellationToken);

        // Map the created user to UserResponse
        var response = _mapper.Map<UserResponse>(user);
        
        return Result.Success(response);
    }
}

