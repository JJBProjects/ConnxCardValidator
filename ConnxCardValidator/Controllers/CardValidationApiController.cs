using ConnxCardValidator.Models;
using ConnxCardValidator.Services;
using Microsoft.AspNetCore.Mvc;

namespace ConnxCardValidator.Controllers
{
    /// <summary>
    /// Controller that recieves and manages calls to the card validation API
    /// </summary>
    [ApiController]
    public class CardValidationApiController : ControllerBase
    {
        private readonly ICardNumberValidator _cardNumberValidator;
        private readonly ICardTypeDetailsRepository _cardTypeDetailsRepository;

        public CardValidationApiController(ICardNumberValidator cardNumberValidator, ICardTypeDetailsRepository cardTypeDetailsRepository)
        {
            _cardNumberValidator = cardNumberValidator;
            _cardTypeDetailsRepository = cardTypeDetailsRepository;
        }

        /// <summary>
        /// The user inputs a string representing the card type followed by a card number. The api tests the number against the validation requirements of
        /// the card type and the Luhn algorithm. 
        /// </summary>
        /// <param name="cardType">A string representing the card type</param>
        /// <param name="cardNumber">The number of the credit card to be validated</param>
        /// <returns>Json object containing information about the validation steps and overall validity of the card</returns>
        [HttpGet("api/validatecard/{cardType}/{cardNumber}")]
        [ProducesResponseType(StatusCodes.Status400BadRequest)]
        [ProducesResponseType(StatusCodes.Status200OK)]
        public ActionResult<CardValidationDto> GetCardIsValid(string cardType, string cardNumber)
        {
            if (long.TryParse(cardNumber, out long cardNumberInt))
            {
                var cardTypeDetails = _cardTypeDetailsRepository.GetCardTypeDetailsByCardType(cardType);
                if(cardTypeDetails == null)
                {
                    var validCardTypes = _cardTypeDetailsRepository.AllCardTypeDetails.Select(x => x.CardType).ToList();
                    return BadRequest($"This card type does not exist please select from the following: {string.Join(", ", validCardTypes)}");
                }

                return Ok(_cardNumberValidator.ValidateCardNumber(cardTypeDetails.CardTypeId, cardNumberInt));
            }
            else
            {
                return BadRequest("The card number must be an integer value");
            }
        }


    }
}