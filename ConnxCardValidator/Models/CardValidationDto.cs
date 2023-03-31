namespace ConnxCardValidator.Models
{
    /// <summary>
    /// Data transfer object for passing card information and recording the outcome of validation steps
    /// </summary>
    public class CardValidationDto
    {
        public int CardTypeId { get; set; }

        public string CardType { get; set; }

        public long CardNumber { get; set; }

        public bool IsValidLength { get; set; }

        public bool IsValidBeginning { get; set; }

        public bool IsValidLuhn { get; set; }

        public bool IsValid { get; set; }

        public string ValidationSummary { get; set; }

    }
}
