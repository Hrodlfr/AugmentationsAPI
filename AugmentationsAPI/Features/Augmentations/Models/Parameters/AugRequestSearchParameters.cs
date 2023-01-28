namespace AugmentationsAPI.Features.Augmentations.Models.Parameters
{
    /// <summary>
    /// The Parameters Used for Searching a List of Augmentations.
    /// </summary>
    public class AugRequestSearchParameters
    {
        /// <summary>
        /// A Term which will be Searched For.
        /// </summary>
        public string SearchTerm { get; set; } = string.Empty;
    }
}
