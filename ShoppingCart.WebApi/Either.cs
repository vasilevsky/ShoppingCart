namespace ShoppingCart.WebApi
{
    public class Either<TUnit, TFailure>
        where TFailure : Failure
    {
        public TUnit Unit { get; }

        public TFailure Failure { get; }

        public Either(TFailure failure)
        {
            Failure = failure;
        }

        public Either(TUnit result)
        {
            Unit = result;
        }

        public static implicit operator Either<TUnit, TFailure>(TUnit result)
        {
            return new Either<TUnit, TFailure>(result);
        }

        public static implicit operator Either<TUnit, TFailure>(TFailure failure)
        {
            return new Either<TUnit, TFailure>(failure);
        }
    }
}