namespace AugmentationsAPI.Features.Links
{
    using Models;
    using static Infrastructure.Constants;

    public class LinkGenerationService : ILinkGenerationService
    {
        private const string LinkRelAugmentationSuffix = "_augmentation";
        
        private readonly LinkGenerator linkGenerator;
        private readonly IHttpContextAccessor httpContext;

        public LinkGenerationService(IHttpContextAccessor httpContext, LinkGenerator linkGenerator)
        {
            this.httpContext = httpContext;
            this.linkGenerator = linkGenerator;
        }

        /// <summary>
        /// Generates HATEOAS Links for a Resource.
        /// </summary>
        /// <param name="id"> The Id of the Augmentation for which the Links will be Generated. </param>
        /// <returns> Generated Links for the Resource. </returns>
        public List<Link> GenerateLinks(int id)
        {
            // Generate a List of Links
            var links = new List<Link>()
            {
                // Generate a Link for Getting this Resource
                new Link()
                {
                    Href = linkGenerator.GetUriByAction(httpContext.HttpContext!, controller: "Augmentations", action: "Get", values: new { id = id })!,
                    Rel = LinkRelSelf,
                    Method = GetUppercase
                },
                
                // Generate a Link for Posting an Resource
                new Link()
                {
                    Href = linkGenerator.GetUriByAction(httpContext.HttpContext!, controller: "Augmentations", action: "Post")!,
                    Rel = PostLowercase + LinkRelAugmentationSuffix,
                    Method = PostUppercase
                },
                
                // Generate a Link for Updating a Resource
                new Link()
                {
                    Href = linkGenerator.GetUriByAction(httpContext.HttpContext!, controller: "Augmentations", action: "Update", values: new { id = id })!,
                    Rel = UpdateLowercase + LinkRelAugmentationSuffix,
                    Method = UpdateUppercase
                },
                
                // Generate a Link for Patching a Resource
                new Link()
                {
                    Href = linkGenerator.GetUriByAction(httpContext.HttpContext!, controller: "Augmentations", action: "Patch", values: new { id = id })!,
                    Rel = PatchLowercase + LinkRelAugmentationSuffix,
                    Method = PatchUppercase
                },
                
                // Generate a Link for Deleting a Resource
                new Link()
                {
                    Href = linkGenerator.GetUriByAction(httpContext.HttpContext!, controller: "Augmentations", action: "Delete", values: new { id = id })!,
                    Rel = DeleteLowercase + LinkRelAugmentationSuffix,
                    Method = DeleteUppercase
                }
            };

            // Return the Generated Links
            return links;
        }
    }
}
