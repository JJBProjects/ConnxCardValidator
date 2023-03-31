namespace ConnxCardValidator.Models
{
    /// <summary>
    /// Hard-coded class to store and return card type validation requirements
    /// </summary>
    public class MockCardTypeDetailsRepository : ICardTypeDetailsRepository
    {
        public IEnumerable<CardTypeDetails> AllCardTypeDetails => 
            new List<CardTypeDetails>
            {
                new CardTypeDetails { CardTypeId = 1, CardType = "AMEX", ValidBeginnings = new List<int> { 34,37 }, ValidLengths = new List<int> { 15 } },
                new CardTypeDetails { CardTypeId = 2, CardType = "Discover", ValidBeginnings = new List<int> { 6011 }, ValidLengths =  new List<int> { 16 } },
                new CardTypeDetails { CardTypeId = 3, CardType = "MasterCard", ValidBeginnings = new List<int> { 51,52,53,54,55 }, ValidLengths = new List<int> { 16 } },
                new CardTypeDetails { CardTypeId = 4, CardType = "Visa", ValidBeginnings = new List<int> { 4 }, ValidLengths = new List<int> { 13, 16 } }
            };

        public CardTypeDetails GetCardTypeDetailsByCardTypeId(int cardTypeId)
        {
            return AllCardTypeDetails.FirstOrDefault(x => x.CardTypeId == cardTypeId);
        }

        public CardTypeDetails GetCardTypeDetailsByCardType(string cardType)
        {
            return AllCardTypeDetails.FirstOrDefault(x => x.CardType == cardType);
        }
    }
}
