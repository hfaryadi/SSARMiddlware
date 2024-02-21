namespace SSARMiddlware.Interfaces.Base
{
    internal interface IBaseService<TResponse>
    {
        void TokenValidation(string token);
    }
}
