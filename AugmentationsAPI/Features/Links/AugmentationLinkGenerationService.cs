namespace AugmentationsAPI.Features.Links
{
    using Models;
    using static Infrastructure.Constants;
    using AugmentationsAPI.Features.Augmentations.Models;

    public class AugmentationLinkGenerationService : ILinkGenerationService<AugmentationResponseModel>
    {
        private const string LinkRelAugmentationSuffix = "_augmentation";
        
        private readonly LinkGenerator linkGenerator;
        private readonly IHttpContextAccessor httpContext;

        public AugmentationLinkGenerationService(IHttpContextAccessor httpContext, LinkGenerator linkGenerator)
        {
            this.httpContext = httpContext;
            this.linkGenerator = linkGenerator;
        }

        /// <summary>
        /// Generates HATEOAS Links for an Augmentation.
        /// </summary>
        /// <param name="id"> The Id of the Augmentation for which the Links will be Generated. </param>
        /// <returns> Generated Links for the Augmentation. </returns>
        public List<Link> GenerateLinks(int id)
        {
            // Generate a List of Links
            var links = new List<Link>()
            {
                // Generate a Link to GET this Augmentation
                new Link()
                {
                    Href = linkGenerator
                    .GetUriByAction(httpContext.HttpContext!, controller: "Augmentations", action: "Get", values: new { id = id })!,
                    Rel = LinkRelSelf,
                    Method = GetUppercase
                },
                
                // Generate a Link to POST an Augmentation
                new Link()
                {
                    Href = linkGenerator
                    .GetUriByAction(httpContext.HttpContext!, controller: "Augmentations", action: "Post")!,
                    Rel = PostLowercase + LinkRelAugmentationSuffix,
                    Method = PostUppercase
                },
                
                // Generate a Link to UPDATE this Augmentation
                new Link()
                {
                    Href = linkGenerator
                    .GetUriByAction(httpContext.HttpContext!, controller: "Augmentations", action: "Put", values: new { id = id })!,
                    Rel = PutLowercase + LinkRelAugmentationSuffix,
                    Method = PutUppercase
                },
                
                // Generate a Link to PATCH this Augmentation
                new Link()
                {
                    Href = linkGenerator
                    .GetUriByAction(httpContext.HttpContext!, controller: "Augmentations", action: "Patch", values: new { id = id })!,
                    Rel = PatchLowercase + LinkRelAugmentationSuffix,
                    Method = PatchUppercase
                },
                
                // Generate a Link to DELETE this Augmentation
                new Link()
                {
                    Href = linkGenerator
                    .GetUriByAction(httpContext.HttpContext!, controller: "Augmentations", action: "Delete", values: new { id = id })!,
                    Rel = DeleteLowercase + LinkRelAugmentationSuffix,
                    Method = DeleteUppercase
                }
            };

            // Return the Generated Links
            return links;
        }
    }
}
