namespace ConnxCardValidator.Models
{
    /// <summary>
    /// Class for storing card validation requirements for a each card type
    /// </summary>
    public class CardTypeDetails
    {
        public int CardTypeId {  get; set; }
        public string CardType {  get; set; }
        public List<int> ValidLengths { get; set; }
        public List<int> ValidBeginnings { get; set; }
    }
}
