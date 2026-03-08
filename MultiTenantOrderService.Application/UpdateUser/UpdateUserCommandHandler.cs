using AutoMapper;
using Microsoft.EntityFrameworkCore;
using MultiTenantOrderService.Application.Abstractions.Messaging;
using MultiTenantOrderService.Application.GetUser;
using MultiTenantOrderService.Domain.Abstractions;
using MultiTenantOrderService.Domain.Identity;
using MultiTenantOrderService.Domain.Identity.ObjectValues;
using Scrypt;

namespace MultiTenantOrderService.Application.UpdateUser;

internal sealed class UpdateUserCommandHandler : ICommandHandler<UpdateUserCommand, UserResponse>
{
    private readonly IUserRepository _userRepository;
    private readonly IUnitOfWork _unitOfWork;
    private readonly IMapper _mapper;

    public UpdateUserCommandHandler(
        IUserRepository userRepository,
        IUnitOfWork unitOfWork,
        IMapper mapper)
    {
        _userRepository = userRepository;
        _unitOfWork = unitOfWork;
        _mapper = mapper;
    }

    public async Task<Result<UserResponse>> Handle(
        UpdateUserCommand request,
        CancellationToken cancellationToken)
    {
        // Get existing user
        var user = await _userRepository.GetByIdAsync(request.Id, cancellationToken);
        if (user == null)
        {
            return Result.Failure<UserResponse>(UserErrors.NotFound);
        }

        // Check if email is being changed and if new email is unique
        if (user.Email.Value != request.Email)
        {
            var newEmail = new Email(request.Email);
            var isEmailUnique = await _userRepository.IsEmailUniqueAsync(newEmail, cancellationToken);
            if (!isEmailUnique)
            {
                return Result.Failure<UserResponse>(UserErrors.EmailNotUnique);
            }
            user.Email = newEmail;
        }

        // Update user properties
        user.Name = request.Name;
        user.UserType = request.UserType;
        user.TenentName = request.TenentName;
        user.UpdateAt = DateTime.UtcNow;

        // Update phone number if provided
        if (!string.IsNullOrEmpty(request.PhoneNumber))
        {
            user.PhoneNumber = new PhoneNumber(request.PhoneNumber);
        }

        // Update password if provided
        if (!string.IsNullOrEmpty(request.Password))
        {
            var scryptEncoder = new ScryptEncoder();
            user.Password = new Password(scryptEncoder.Encode(request.Password));
        }

        await _unitOfWork.SaveChangesAsync(cancellationToken);

        // Map to response
        var response = _mapper.Map<UserResponse>(user);

        return Result.Success(response);
    }
}

