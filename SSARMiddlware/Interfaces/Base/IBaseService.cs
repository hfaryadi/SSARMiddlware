namespace SSARMiddlware.Interfaces.Base
{
    internal interface IBaseService
    {
        void TokenValidation(string token, string request);
        void Execute();
    }
}
