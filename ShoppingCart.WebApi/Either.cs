namespace ShoppingCart.WebApi
{
    /// <summary>
    /// Represents object holding either "good" unit or failure appered during getting the unit.
    /// </summary>
    /// <remarks>
    /// The class is used to represent result business logic execution.
    /// It allows to isolate Controllers and Services still being able to communicate results to the former from the latter
    /// in a non-exception/exception propagation way.</remarks>
    /// <typeparam name="TUnit">A "good" unit type.</typeparam>
    /// <typeparam name="TFailure">A failure type.</typeparam>
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