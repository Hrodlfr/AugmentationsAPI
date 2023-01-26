namespace AugmentationsAPI.Features.Links.Models
{
    /// <summary>
    /// A Model Used for Representing HATEOAS Links.
    /// </summary>
    public class Link
    {
        /// <summary>
        /// The Hypertext Reference of the Link.
        /// </summary>
        public string Href { get; set; } = String.Empty;

        /// <summary>
        /// The Relationship of the Linked Resource with the Current Resource.
        /// </summary>
        public string Rel { get; set; } = String.Empty;

        /// <summary>
        /// The HTTP Method of the Resource.
        /// </summary>
        public string Method { get; set; } = String.Empty;
    }
}
