using ConnxCardValidator.Models;

namespace ConnxCardValidator.Services
{
    public interface ICardNumberValidator
    {
        CardValidationDto ValidateCardNumber(int cardTypeId, long cardNumber);
    }
}
