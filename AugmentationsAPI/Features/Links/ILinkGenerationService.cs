namespace AugmentationsAPI.Features.Links
{
    using Models;

    public interface ILinkGenerationService<TResource>
    {
        /// <summary>
        /// Generates HATEOAS Links for a Resource.
        /// </summary>
        /// <param name="id"> The Id of the Resource for which the Links will be Generated. </param>
        /// <returns> Generated Links for the Resource. </returns>
        public List<Link> GenerateLinks(int id);
    }
}
