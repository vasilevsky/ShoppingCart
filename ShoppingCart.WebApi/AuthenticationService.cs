namespace ShoppingCart.WebApi
{
    public class AuthenticationService : IAuthenticationService
    {
        public bool ValidateKey(string key)
        {
            return key == "Chuck Norris";
        }
    }

    public interface IAuthenticationService
    {
        bool ValidateKey(string key);
    }
}
