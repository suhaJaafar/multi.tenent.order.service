using AutoMapper;
using MultiTenantOrderService.Infrastructure.Interfaces;
using MultiTenantOrderService.Domain.DBContexts;
using MultiTenantOrderService.Domain.DTOs;
using MultiTenantOrderService.Domain.Entities;
using MultiTenantOrderService.Domain.FiltersParams;
using MultiTenantOrderService.Domain.Forms;
using MultiTenantOrderService.Infrastructure.Interfaces;
using MultiTenantOrderService.Infrastructure.Utilities;
using Microsoft.EntityFrameworkCore;
using Scrypt;

namespace MultiTenantOrderService.Application;

public class UserService : IUserService
{
    private readonly OSContext _context;
    private readonly IMapper _mapper;

    private bool _disposed;

    public UserService(IMapper mapper, OSContext context)
    {
        _mapper = mapper;
        _context = context;
    }

    public void Dispose()
    {
        Dispose(true);
        GC.SuppressFinalize(this);
    }

    public async Task<ServiceResponse<List<UsersToReturnDto>>> GetUsers(UsersParams specParams)
    {
        var query = _context.Users.OrderByDescending(v => v.CreateAt).AsQueryable();

        if (!string.IsNullOrEmpty(specParams.SearchTerm))
            query = query.Where(x =>
                x.Name.ToLower().Replace(" ", "").Contains(specParams.SearchTerm.Replace(" ", "")) ||
                x.Email.Value.ToLower().Replace(" ", "").Contains(specParams.SearchTerm.Replace(" ", "")) ||
                x.PhoneNumber!.Value.ToLower().Replace(" ", "").Contains(specParams.SearchTerm.Replace(" ", "")));

        if (specParams.Id.HasValue) query = query.Where(x => x.Id == specParams.Id);
        if (specParams.UserType.HasValue) query = query.Where(x => x.UserType == specParams.UserType);

        var count = await query.CountAsync();
        var result = await query.Skip(specParams.Skip).Take(specParams.Take).ToListAsync();

        var data = _mapper.Map<List<UsersToReturnDto>>(result);
        return new ServiceResponse<List<UsersToReturnDto>>(data, count);
    }


    public async Task<ServiceResponse<User>> Login(string email, string password)
    {
        var result = await _context.Users.Where(x => x.Email.Value == email).FirstOrDefaultAsync();
        if (result == null) return new ServiceResponse<User>(true, "no data");

        var scryptEncoder = new ScryptEncoder();
        if (!scryptEncoder.Compare(password, result.Password.Value)) return new ServiceResponse<User>(true, "no data");
        return new ServiceResponse<User>(result);
    }
    
    // get user by id
    public async Task<ServiceResponse<User>> GetUserById(Guid id)
    {
        var result = await _context.Users.Where(x => x.Id == id).FirstOrDefaultAsync();
        if (result == null) return new ServiceResponse<User>(true, "no data");
        return new ServiceResponse<User>(result);
    }


    public async Task<ServiceResponse<User>> AddUser(CreateUserForm form)
    {
        var result = await _context.Users.Where(x => x.Email.Value == form.Email).FirstOrDefaultAsync();
        if (result != null) return new ServiceResponse<User>(true, "Email already exist");
        
        var item = _mapper.Map<User>(form);
    
        // Set CreateAt to UTC time
        item.CreateAt = DateTime.UtcNow;
        
        // Hash the password 
        var scryptEncoder = new ScryptEncoder();
        item.Password = scryptEncoder.Encode(form.Password);
    
        // Force the user to reset password on login
        await _context.AddAsync(item);
        await _context.SaveChangesAsync();
        return new ServiceResponse<User>(item);
    }


    public async Task<ServiceResponse<User>> UpdateUser(UpdateUserForm form)
    {
        var result = await _context.Users.Where(x => x.Id == form.Id).FirstOrDefaultAsync();
        if (result == null) return new ServiceResponse<User>(true, "a user not found");
    
        // check  if email already exist
        var emailExist =
            await _context.Users.Where(x => x.Email.Value == form.Email && x.Id != form.Id).FirstOrDefaultAsync();
        if (emailExist != null) return new ServiceResponse<User>(true, "Email already exist");
    
        _mapper.Map(form, result);
    
        if (!string.IsNullOrEmpty(form.Password))
        {
            var scryptEncoder = new ScryptEncoder();
            result.Password = scryptEncoder.Encode(form.Password);
        }
    
        _context.Update(result);
        await _context.SaveChangesAsync();
        return new ServiceResponse<User>(result);
    }

    protected virtual void Dispose(bool disposing)
    {
        if (!_disposed)
            if (disposing)
                _context.Dispose();

        _disposed = true;
    }
}