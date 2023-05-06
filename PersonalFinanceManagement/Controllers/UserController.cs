using Microsoft.AspNetCore.Mvc;
using PersonalFinanceManagement.Repositories;

namespace PersonalFinanceManagement.Controllers;

public class UserController : Controller 
{
    private readonly IUserRepository _userRepository;

    public UserController(IUserRepository userRepository)
    {
        _userRepository = userRepository;
    }

    //...
}