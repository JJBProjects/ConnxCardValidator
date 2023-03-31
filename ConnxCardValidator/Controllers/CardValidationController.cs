using Microsoft.AspNetCore.Mvc;
using ConnxCardValidator.ViewModels;
using ConnxCardValidator.Models;
using Microsoft.AspNetCore.Mvc.Rendering;
using ConnxCardValidator.Services;

namespace ConnxCardValidator.Controllers
{
    /// <summary>
    /// Controller that handles credit card validation view validation information
    /// </summary>
    public class CardValidationController : Controller
    {
        private readonly ICardNumberValidator _cardNumberValidator;
        private readonly ICardTypeDetailsRepository _cardTypeDetailsRepository;

        public CardValidationController(ICardNumberValidator cardNumberValidator, ICardTypeDetailsRepository cardTypeDetailsRepository)
        {
            _cardNumberValidator = cardNumberValidator;
            _cardTypeDetailsRepository = cardTypeDetailsRepository;
        }

        /// <summary>
        /// Return the view on initial load
        /// </summary>
        public IActionResult Index()
        {
            var viewModel = new CardValidationViewModel();
            viewModel.CardTypes = new SelectList(_cardTypeDetailsRepository.AllCardTypeDetails, nameof(CardTypeDetails.CardTypeId), nameof(CardTypeDetails.CardType));
            return View(viewModel);
        }

        /// <summary>
        /// Takes information from the view and uses it to validate the card number
        /// </summary>
        [HttpPost]
        public IActionResult Index(CardValidationViewModel viewModel)
        {
            if(long.TryParse(viewModel.CardNumber, out long cardNumber))
            {
                var cardValidationDto = _cardNumberValidator.ValidateCardNumber(viewModel.CardTypeId, cardNumber);
                viewModel.IsValid = cardValidationDto.IsValid;
                viewModel.ValidationSummary = cardValidationDto.ValidationSummary;
            }

            viewModel.CardTypes = new SelectList(_cardTypeDetailsRepository.AllCardTypeDetails, nameof(CardTypeDetails.CardTypeId), nameof(CardTypeDetails.CardType));
            return View(viewModel);
        }
    }

}

