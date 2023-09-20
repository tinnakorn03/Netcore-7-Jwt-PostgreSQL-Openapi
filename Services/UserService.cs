namespace NetcoreJwtJsonbOpenapi.Services;
using AutoMapper;
using BCrypt.Net;
using NetcoreJwtJsonbOpenapi.Authorization;
using NetcoreJwtJsonbOpenapi.Entities;
using NetcoreJwtJsonbOpenapi.Helpers;
using NetcoreJwtJsonbOpenapi.Repositories;
using NetcoreJwtJsonbOpenapi.Models;
using NetcoreJwtJsonbOpenapi.Models.Users;

public interface IUserService
{
    Task<AuthenticateResponse?> Authenticate(AuthenticateRequest model);
    Task<IEnumerable<User>> GetAll();
    Task<User> GetById(int id);
    Task Create(CreateRequest model);
    Task Update(int id, UpdateRequest model);
    Task Delete(int id);
}

public class UserService : IUserService
{ 
    private readonly IJwtUtils _jwtUtils;
    private IUserRepository _userRepository;
    private readonly IMapper _mapper;

    public UserService(IJwtUtils jwtUtils, IUserRepository userRepository, IMapper mapper)
    {
        _jwtUtils = jwtUtils;
        _userRepository = userRepository;
        _mapper = mapper;
    }
    
    public async Task<AuthenticateResponse?> Authenticate(AuthenticateRequest model)
    { 
        var user = await _userRepository.GetUser(model.Username);
       
        // return null if user not found
        if (user == null || !BCrypt.Verify(model.Password, user.PasswordHash))
        {
            return null;
        } 
        
        // authentication successful so generate jwt token
        var token = _jwtUtils.GenerateJwtToken(user);
        
        return new AuthenticateResponse(user, token);
    }
 
    public async Task<IEnumerable<User>> GetAll()
    {
        return await _userRepository.GetAll();
    }

    public async Task<User> GetById(int id)
    {
        var user = await _userRepository.GetById(id);

        if (user == null)
            throw new KeyNotFoundException("User not found");

        return user;
    }
    public async Task Create(CreateRequest model)
    {
        // validate
        if (await _userRepository.GetByEmail(model.Email!) != null)
            throw new AppException("User with the email '" + model.Email + "' already exists");

        // map model to new user object
        var user = _mapper.Map<User>(model);

        // hash password
        user.PasswordHash = BCrypt.HashPassword(model.Password);

        // save user
        await _userRepository.Create(user);
    }

    public async Task Update(int id, UpdateRequest model)
    {
        var user = await _userRepository.GetById(id);

        if (user == null)
            throw new KeyNotFoundException("User not found");

        // validate
        var emailChanged = !string.IsNullOrEmpty(model.Email) && user.Email != model.Email;
        if (emailChanged && await _userRepository.GetByEmail(model.Email!) != null)
            throw new AppException("User with the email '" + model.Email + "' already exists");

        // hash password if it was entered
        if (!string.IsNullOrEmpty(model.Password))
            user.PasswordHash = BCrypt.HashPassword(model.Password);

        // copy model props to user
        _mapper.Map(model, user);

        // save user
        await _userRepository.Update(user);
    }

    public async Task Delete(int id)
    {
        await _userRepository.Delete(id);
    }
}