namespace ConnxCardValidator.Models
{
    public interface ICardTypeDetailsRepository
    {
        IEnumerable<CardTypeDetails> AllCardTypeDetails { get; }
        CardTypeDetails GetCardTypeDetailsByCardTypeId(int cardTypeId);
        CardTypeDetails GetCardTypeDetailsByCardType(string cardType);
    }
}
