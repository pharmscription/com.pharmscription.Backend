namespace com.pharmscription.BusinessLogic.Validation
{
    public interface IValidator<in T>
    {
        void Validate(T dto);
    }
}
