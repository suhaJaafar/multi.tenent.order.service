using AutoMapper;
using IdentityService.Application.Abstractions.Messaging;
using Microsoft.EntityFrameworkCore;
using IdentityService.Domain.Abstractions;
using IdentityService.Domain.DBContexts;
using IdentityService.Domain.Identity.DTOs;

namespace IdentityService.Application.GetUsers;

internal sealed class GetUsersQueryHandler : IQueryHandler<GetUsersQuery, UsersListResponse>
{
    private readonly OSContext _context;
    private readonly IMapper _mapper;

    public GetUsersQueryHandler(OSContext context, IMapper mapper)
    {
        _context = context;
        _mapper = mapper;
    }

    public async Task<Result<UsersListResponse>> Handle(
        GetUsersQuery request,
        CancellationToken cancellationToken)
    {
        var specParams = request.Parameters;
        var query = _context.Users.OrderByDescending(v => v.CreateAt).AsQueryable();

        // Apply search filter
        if (!string.IsNullOrEmpty(specParams.SearchTerm))
        {
            var searchTerm = specParams.SearchTerm.ToLower().Replace(" ", "");
            query = query.Where(x =>
                x.Name.ToLower().Replace(" ", "").Contains(searchTerm) ||
                x.Email.Value.ToLower().Replace(" ", "").Contains(searchTerm) ||
                (x.PhoneNumber != null && x.PhoneNumber.Value.ToLower().Replace(" ", "").Contains(searchTerm)));
        }

        // Apply filters
        if (specParams.Id.HasValue)
            query = query.Where(x => x.Id == specParams.Id);

        if (specParams.UserType.HasValue)
            query = query.Where(x => x.UserType == specParams.UserType);

        // Get total count
        var count = await query.CountAsync(cancellationToken);

        // Apply pagination
        var users = await query
            .Skip(specParams.Skip)
            .Take(specParams.Take)
            .ToListAsync(cancellationToken);

        // Map to DTOs
        var userDtos = _mapper.Map<List<UsersToReturnDto>>(users);

        var response = new UsersListResponse
        {
            Users = userDtos,
            TotalCount = count
        };

        return Result.Success(response);
    }
}

