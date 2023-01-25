namespace AugmentationsAPI.Features.Augmentations.Models
{
    /// <summary>
    /// The Parameters Used for Searching a List of Augmentations.
    /// </summary>
    public class AugmentationRequestSearchParameters
    {
        /// <summary>
        /// A Term which will be Searched For.
        /// </summary>
        public string SearchTerm { get; set; } = String.Empty;
    }
}
