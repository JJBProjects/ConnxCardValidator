using ConnxCardValidator.Models;
using Microsoft.VisualBasic;
using System;
using static System.Runtime.InteropServices.JavaScript.JSType;

namespace ConnxCardValidator.Services
{
    public class CardNumberValidator : ICardNumberValidator
    {
        private ICardTypeDetailsRepository _cardTypeDetailsRepository;

        public CardNumberValidator(ICardTypeDetailsRepository cardTypeDetailsRepository)
        {
            _cardTypeDetailsRepository = cardTypeDetailsRepository;
        }

        /// <summary>
        /// Validate credit card number for specified card type Id
        /// </summary>
        /// <param name="cardTypeId">An integer value representing a card type</param>
        /// <param name="cardNumber">The integer value of the credit card number</param>
        /// <returns>Data transfer object with validation information</returns>
        public CardValidationDto ValidateCardNumber(int cardTypeId, long cardNumber)
        {
            var dto = new CardValidationDto { CardTypeId = cardTypeId, CardNumber = cardNumber };
            var cardTypeDetails = _cardTypeDetailsRepository.GetCardTypeDetailsByCardTypeId(cardTypeId);
            dto = ValidateAll(dto, cardTypeDetails);
            return dto;
        }

        /// <summary>
        /// Tests card details against all card type validation requirements 
        /// </summary>
        /// <param name="dto">Data transfer object with card and validation information</param>
        /// <param name="cardTypeDetails">Details about the validation requirements of the specified card type</param>
        /// <returns>Data transfer object with card and validation information</returns>
        public CardValidationDto ValidateAll(CardValidationDto dto, CardTypeDetails cardTypeDetails)
        {
            if (cardTypeDetails != null)
            {
                dto.CardTypeId = cardTypeDetails.CardTypeId;
                dto.IsValidLength = LengthValidation(dto.CardNumber, cardTypeDetails.ValidLengths);
                dto.IsValidBeginning = BeginningValidation(dto.CardNumber, cardTypeDetails.ValidBeginnings);
            }

            dto.IsValidLuhn = LuhnValidation(dto.CardNumber);

            dto.IsValid = dto.IsValidLength && dto.IsValidBeginning && dto.IsValidLuhn;

            PopulateValidationSummary(ref dto);

            return dto;
        }

        /// <summary>
        /// Checks that the card number meets the length requirement of the card type provided
        /// </summary>
        /// <param name="cardNumber">The integer value of the credit card number</param>
        /// <param name="validLengths">The valid card number lengths for the card type</param>
        /// <returns>True if the length of the card is valid</returns>
        public bool LengthValidation(long cardNumber, List<int> validLengths)
        {
            if (validLengths.Contains(cardNumber.ToString().Length))
            {
                return true;
            }
            return false;
        }

        /// <summary>
        /// Checks that the beginning of the card number meets the valide card beginning values for the card type provided
        /// </summary>
        /// <param name="cardNumber">The integer value of the credit card number</param>
        /// <param name="validBeginnings">The valid card number lengths for the card type</param>
        /// <returns>True if the beginning of the card is valid</returns>
        public bool BeginningValidation(long cardNumber, List<int> validBeginnings)
        {
            if (validBeginnings.Any(x => cardNumber.ToString().StartsWith(x.ToString())))
            {
                return true;
            }
            return false;
        }

        /// <summary>
        /// Checks that the provided card number satisfies the Luhn algorithm
        /// 1. Starting with the next to last digit and continuing with every other digit going back to the beginning of the card, double the digit
        /// 2. Sum all doubled and untouched digits in the number.For digits greater than 9 you will need to split them and sum the independently (i.e. "10", 1 + 0).
        /// 3. If that total is a multiple of 10, the number is valid.
        /// </summary>
        /// <param name="cardNumber">The integer value of the credit card number</param>
        /// <returns>True if the card number satisfies the luhn algorithm</returns>
        public bool LuhnValidation(long cardNumber)
        {
            char[] charArray = cardNumber.ToString().ToCharArray();
            Array.Reverse(charArray);

            var luhnDigitList = new List<int>();

            for (int ii = 1; ii <= charArray.Length; ii++)
            {
                int digit = int.Parse(charArray[ii - 1].ToString());
                if (ii % 2 == 0)
                {
                    int doubledDigit = digit * 2;
                    luhnDigitList.Add(doubledDigit / 10);
                    luhnDigitList.Add(doubledDigit % 10);
                }
                else
                {
                    luhnDigitList.Add(digit);
                }
            }

            return luhnDigitList.Sum() % 10 == 0;
        }

        /// <summary>
        /// Provides a natural language description of which aspects of the card have passed validation
        /// </summary>
        /// <param name="dto">Details about the validation requirements of the specified card type</param>
        public void PopulateValidationSummary(ref CardValidationDto dto)
        {
            string validationSummary = string.Empty;

            validationSummary += string.Format($"This {dto.CardType} card number is {(dto.IsValid ? "valid" : "invalid")}. \n");

            if (!dto.IsValidLength)
            {
                validationSummary += "- The length is invalid for this card type. \n";
            }

            if (!dto.IsValidBeginning)
            {
                validationSummary += "- The beginning digits are invalid for this card type. \n";
            }

            if (!dto.IsValidLuhn)
            {
                validationSummary += "- The Luhn algorithm has failed. \n";
            }

            dto.ValidationSummary = validationSummary;
        }

        /// <summary>
        /// Helper method to reverse a string
        /// </summary>
        public static string Reverse(string s)
        {
            char[] charArray = s.ToCharArray();
            Array.Reverse(charArray);
            return new string(charArray);
        }
    }
}
