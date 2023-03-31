namespace UnitTests
{
    using ConnxCardValidator;
    using ConnxCardValidator.Models;
    using ConnxCardValidator.Services;

    public class CardValidationUnitTests
    {
        private class CardTypes
        {
            public const int unknown = 0;
            public const int AMEX = 1;
            public const int Discover = 2;
            public const int MasterCard = 3;
            public const int Visa = 4;
        }

        private CardNumberValidator cardNumberValidator;
        private MockCardTypeDetailsRepository cardTypeDetailsRepository;

        [SetUp]
        public void SetupTests()
        {
            cardTypeDetailsRepository= new MockCardTypeDetailsRepository();
            cardNumberValidator = new CardNumberValidator(cardTypeDetailsRepository);
        }

        [Test]
        public void LengthValidationTest()
        {
            Assert.IsTrue(cardNumberValidator.ValidateCardNumber(CardTypes.AMEX, 123456789012345).IsValidLength);
            Assert.IsTrue(cardNumberValidator.ValidateCardNumber(CardTypes.Visa, 1234567890123).IsValidLength);
            Assert.IsTrue(cardNumberValidator.ValidateCardNumber(CardTypes.Visa, 1234567890123456).IsValidLength);
            Assert.IsFalse(cardNumberValidator.ValidateCardNumber(CardTypes.AMEX, 1234567890).IsValidLength);
            Assert.IsFalse(cardNumberValidator.ValidateCardNumber(CardTypes.Visa, 1234567890).IsValidLength);
        }

        [Test]
        public void BeginningValidationTest()
        {
            Assert.IsTrue(cardNumberValidator.ValidateCardNumber(CardTypes.AMEX, 343456789012345).IsValidBeginning);
            Assert.IsTrue(cardNumberValidator.ValidateCardNumber(CardTypes.AMEX, 373456789012345).IsValidBeginning);
            Assert.IsTrue(cardNumberValidator.ValidateCardNumber(CardTypes.Discover, 601167890123).IsValidBeginning);
            Assert.IsFalse(cardNumberValidator.ValidateCardNumber(CardTypes.AMEX, 123456789012345).IsValidBeginning);
            Assert.IsFalse(cardNumberValidator.ValidateCardNumber(CardTypes.Discover, 1234567890123).IsValidBeginning);
        }

        [Test]
        public void LunhValidationTest()
        {
            Assert.IsTrue(cardNumberValidator.ValidateCardNumber(CardTypes.Visa, 4111111111111111).IsValidLuhn);
            Assert.IsFalse(cardNumberValidator.ValidateCardNumber(CardTypes.Visa, 4111111111111).IsValidLuhn);
            Assert.IsTrue(cardNumberValidator.ValidateCardNumber(CardTypes.Visa, 4012888888881881).IsValidLuhn);
            Assert.IsTrue(cardNumberValidator.ValidateCardNumber(CardTypes.AMEX, 378282246310005).IsValidLuhn);
            Assert.IsTrue(cardNumberValidator.ValidateCardNumber(CardTypes.Discover, 6011111111111117).IsValidLuhn);
            Assert.IsTrue(cardNumberValidator.ValidateCardNumber(CardTypes.MasterCard, 5105105105105100).IsValidLuhn);
            Assert.IsFalse(cardNumberValidator.ValidateCardNumber(CardTypes.MasterCard, 5105105105105106).IsValidLuhn);
            Assert.IsTrue(cardNumberValidator.ValidateCardNumber(CardTypes.unknown, 4408041234567893).IsValidLuhn);
            Assert.IsFalse(cardNumberValidator.ValidateCardNumber(CardTypes.unknown, 9111111111111111).IsValidLuhn);
        }
    }
}