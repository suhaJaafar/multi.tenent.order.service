using AutoMapper;
using MultiTenantOrderService.Application.Abstractions.Messaging;
using MultiTenantOrderService.Domain.Abstractions;
using MultiTenantOrderService.Domain.Identity;

namespace MultiTenantOrderService.Application.GetUser;

internal sealed class GetUserQueryHandler : IQueryHandler<GetUserQuery, UserResponse>
{
    private readonly IUserRepository _userRepository;
    private readonly IMapper _mapper;

    public GetUserQueryHandler(IUserRepository userRepository, IMapper mapper)
    {
        _userRepository = userRepository;
        _mapper = mapper;
    }

    public async Task<Result<UserResponse>> Handle(
        GetUserQuery request,
        CancellationToken cancellationToken)
    {
        var user = await _userRepository.GetByIdAsync(request.UserId, cancellationToken);

        if (user is null)
        {
            return Result.Failure<UserResponse>(UserErrors.NotFound);
        }

        var response = _mapper.Map<UserResponse>(user);
        
        return Result.Success(response);
    }
}