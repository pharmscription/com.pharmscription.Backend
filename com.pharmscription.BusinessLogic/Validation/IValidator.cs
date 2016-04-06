namespace com.pharmscription.BusinessLogic.Validation
{
    public interface IValidator<T>
    {
        void Validate(T dto);
    }
}
