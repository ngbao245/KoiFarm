using Microsoft.Extensions.Configuration;
using Repository.Data.Entity;
using Repository.Helper;
using Repository.Model.Auth;
using Repository.Model.User;
using Repository.Model;
using Repository.Repository;
using Repository.Service;

public class AuthService : IAuthService
{
    private readonly UnitOfWork _unitOfWork;
    private readonly GenerateToken _generateToken;
    private readonly IConfiguration _configuration;

    public AuthService(
        UnitOfWork unitOfWork,
        GenerateToken generateToken,
        IConfiguration configuration)
    {
        _unitOfWork = unitOfWork;
        _generateToken = generateToken;
        _configuration = configuration;
    }

    public ResponseTokenModel SignIn(SignInModel signInModel)
    {
        var user = _unitOfWork.UserRepository.GetSingle(u => u.Email == signInModel.Email);
        if (user == null || user.Password != signInModel.Password)
            throw new UnauthorizedAccessException("Invalid credentials.");

        var token = _generateToken.GenerateTokenModel(user);
        return new ResponseTokenModel
        {
            Token = token.Token,
            RefreshToken = token.RefreshToken,
            User = new ResponseUserModel
            {
                Name = user.Name,
                Email = user.Email,
                Address = user.Address,
                Phone = user.Phone,
                RoleId = user.RoleId
            }
        };
    }

    public ResponseModel SignUp(SignUpModel signUpModel)
    {
        var existingUser = _unitOfWork.UserRepository.GetSingle(u => u.Email == signUpModel.Email);
        if (existingUser != null)
        {
            return new ResponseModel
            {
                StatusCode = 400,
                MessageError = "User with this email already exists."
            };
        }

        var user = new User
        {
            Name = signUpModel.Name,
            Email = signUpModel.Email,
            Password = signUpModel.Password,  // No hashing
            Phone = signUpModel.Phone,
            Address = signUpModel.Address,
            RoleId = "0"
        };

        _unitOfWork.UserRepository.Create(user);
        _unitOfWork.SaveChange(); // Save changes using UnitOfWork

        return new ResponseModel
        {
            StatusCode = 201,
            Data = user
        };
    }

    public ResponseTokenModel RefreshToken(string refreshToken)
    {
        var storedToken = _unitOfWork.UserRefreshTokenRepository.GetSingle(t => t.RefreshToken == refreshToken && !t.isUsed);
        if (storedToken == null || storedToken.ExpireTime < DateTime.Now)
            throw new UnauthorizedAccessException("Invalid or expired refresh token.");

        storedToken.isUsed = true;
        _unitOfWork.UserRefreshTokenRepository.Update(storedToken);
        _unitOfWork.SaveChange(); // Save changes using UnitOfWork

        var user = _unitOfWork.UserRepository.GetById(storedToken.User_Id);
        return _generateToken.GenerateTokenModel(user);
    }
}
