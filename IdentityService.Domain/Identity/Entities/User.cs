using IdentityService.Domain.Abstractions;
using IdentityService.Domain.Enums;
using IdentityService.Domain.Identity.Enums;
using IdentityService.Domain.Identity.Events;
using IdentityService.Domain.Identity.ObjectValues;

namespace IdentityService.Domain.Identity.Entities;

public class User : Entity
{
    // Parameterless constructor for EF Core
    private User() : base(Guid.Empty)
    {
    }
    
    private User(Guid id, string name, Email email, Password password, UserType userType, TenentName tenentName)
        : base(id)
    {
        Email = email;
        Password = password;
        Name = name;
        UserType = userType;
        TenentName = tenentName;
    }


    
    public PhoneNumber? PhoneNumber { get; set; }
    public Email Email { get; set; }
    public string Name { get; set; }
    public Password Password { get; set; }
    public UserType UserType { get; set; }
    
    public TenentName TenentName { get; set; }
    
    public static User Create(Guid id, string name, Email email, Password password, UserType userType,
        TenentName tenentName)
    {
        var user = new User (id, name, email, password, userType, tenentName);
        user.RaiseDomainEvent(new UserCreatedDomainEvent(user.Id));
        return user;
    }
}