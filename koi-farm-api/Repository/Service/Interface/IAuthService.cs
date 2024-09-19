using Repository.Model.Auth;
using Repository.Model;

namespace Repository.Service
{
    public interface IAuthService
    {
        ResponseTokenModel SignIn(SignInModel signInModel);
        ResponseModel SignUp(SignUpModel signUpModel);
        ResponseTokenModel RefreshToken(string refreshToken);
    }
}
