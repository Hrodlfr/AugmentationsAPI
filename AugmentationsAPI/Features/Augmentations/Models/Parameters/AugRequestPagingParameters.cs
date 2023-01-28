namespace AugmentationsAPI.Features.Augmentations.Models.Parameters
{
    using System.ComponentModel.DataAnnotations;

    /// <summary>
    /// The Parameters Used for Paging the a List of Augmentations.
    /// </summary>
    public class AugRequestPagingParameters : IValidatableObject
    {
        /// <summary>
        /// The Minimum Page Number.
        /// </summary>
        private const int MinimumPageNumber = 1;

        /// <summary>
        /// The Error Displayed when the Page Number is Invalid.
        /// </summary>
        private const string PageNumberErrorMessage = "The Page Number Cannot be Less than 1";

        /// <summary>
        /// The Number of the Page whose Items will be Selected.
        /// </summary>
        public int PageNumber { get; set; } = MinimumPageNumber;

        /// <summary>
        /// The Minimum Size of a Page.
        /// </summary>
        private const int MinPageSize = 1;

        /// <summary>
        /// The Maximum Size of a Page.
        /// </summary>
        private const int MaxPageSize = 50;

        /// <summary>
        /// The Amount of Items which a Page can Contain.
        /// </summary>
        [Range(MinPageSize, MaxPageSize)]
        public int PageSize { get; set; } = MaxPageSize;

        /// <summary>
        /// Validates this Object.
        /// </summary>
        /// <returns> The Result of the Validation. </returns>
        public IEnumerable<ValidationResult> Validate(ValidationContext validationContext)
        {
            // If the Received Page Number is Lesser Than the Minimum Possible Page Number...
            if (PageNumber < MinimumPageNumber)
            {
                // ...Return a new Validation with the Page Number Error Message
                yield return new ValidationResult(
                    PageNumberErrorMessage,
                    new[]
                    {
                        nameof(PageNumber)
                    });
            }
        }
    }
}
