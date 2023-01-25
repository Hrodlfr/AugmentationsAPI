namespace AugmentationsAPI.Features.Augmentations
{
    using Data;
    using Data.Models;
    using Models;
    using Mapster;
    using Microsoft.EntityFrameworkCore;

    public class AugmentationRepository : IAugmentationRepository
    {
        /// <summary>
        /// The Database Context of the Application.
        /// </summary>
        private readonly ApplicationDbContext data;

        public AugmentationRepository(ApplicationDbContext data)
        {
            this.data = data;
        }
        
        /// <summary>
        /// Returns a Paged List of all Augmentations from the Database.
        /// </summary>
        /// <param name="pagingParameters"> The Parameters Used for Paging the List of the Augmentations. </param>
        /// <param name="searchParameters"> The Parameters Used for Searching the List of the Augmentations. </param>
        /// <returns> A Paged List of all Augmentations in the Database. </returns>
        public async Task<IEnumerable<AugmentationResponseModel>> GetAll(AugmentationRequestPagingParameters pagingParameters,
            AugmentationRequestSearchParameters searchParameters)
        {
            var augs = await data.Augmentations
                // Use Projection for Better Performance
                .Select(aug => aug.Adapt<AugmentationResponseModel>())
                // Skip the Entities Contained in the Pages Before the Requested Page
                .Skip((pagingParameters.PageNumber - 1) * pagingParameters.PageSize)
                .Take(pagingParameters.PageSize)
                .ToListAsync();

            if (string.IsNullOrWhiteSpace(searchParameters.SearchTerm))
            {
                return augs;
            }

            var lowerCaseTerm = searchParameters.SearchTerm.ToLower();

            return augs.Where(aug => aug.Name.ToLower().Contains(lowerCaseTerm) || aug.Description.ToLower().Contains(lowerCaseTerm));
        }

        /// <summary>
        /// Returns an Augmentation with a Matching Id which is Tracked by Entity Framework.
        /// </summary>
        /// <param name="id"> An Id which will be used to Find a matching Augmentation. </param>
        /// <returns> An Augmentation with a Matching Id Or Null If an Augmentation with a Matching Id wasn't Found. </returns>
        public async Task<Augmentation?> Get(int id)
        {
            // Return the First Augmentation with a Matching Id
            return await data.Augmentations
                .Where(augment => augment.Id == id)
                .FirstOrDefaultAsync();
        }
        
        /// <summary>
        /// Returns an Augmentation with a Matching Id which is Not Tracked by Entity Framework.
        /// </summary>
        /// <param name="id"> An Id which will be used to Find a matching Augmentation. </param>
        /// <param name="tracking"> A Boolean Value which Indicates whether the Returned Entity should be Tracked or Not. </param>
        /// <returns> An Augmentation with a Matching Id Or Null If an Augmentation with a Matching Id wasn't Found. </returns>
        public async Task<AugmentationResponseModel?> Get(int id, bool tracking)
        {
            // Return the First Augmentation with a Matching Id
            return await data.Augmentations
                .AsNoTracking()
                .Where(augment => augment.Id == id)
                // Use Projection for Better Performance
                .Select(aug => aug.Adapt<AugmentationResponseModel>())
                .FirstOrDefaultAsync();
        }
        
        /// <summary>
        /// Adds an Augmentation to the Database.
        /// </summary>
        /// <param name="model"> A Data Transfer Object whose Data will be Used to Create a Augmentation. </param>
        /// <returns> The Id of the newly Added Augmentation. </returns>
        public async Task<int> Create(AugmentationRequestModel model)
        {
            // Initialize a new Augmentation and Map the Request Models Values to It
            var augmentation = model.Adapt<Augmentation>();
            
            // Add the Augmentation to the Database
            await data.Augmentations.AddAsync(augmentation);

            // Save the Changes to the Database
            await data.SaveChangesAsync();

            // Return the Id of the newly Added Augmentation
            return augmentation.Id;
        }

        /// <summary>
        /// Updates an Augmentation.
        /// </summary>
        /// <param name="augToUpdate"> An Augmentation whose Data will be Updated. </param>
        /// <param name="model"> A Data Transfer Object which contains the Updated Data for an Augmentation. </param>
        /// <returns> A Bool indicating whether the Update was Successful. </returns>
        public async Task Update(Augmentation augToUpdate, AugmentationRequestModel model)
        {
            // Map the Updated Values to the Existing Augmentation
            model.Adapt(augToUpdate);
            
            // Save the Changes to the Database
            await data.SaveChangesAsync();
        }

        /// <summary>
        /// Deletes an Augmentation.
        /// </summary>
        /// <param name="augToDelete"> The Augmentation to be Deleted. </param>
        /// <returns> A Bool indicating whether the Deletion was Successful. </returns>
        public async Task Delete(Augmentation augToDelete)
        {
            // Delete the Augmentation from the Database
            data.Augmentations.Remove(augToDelete);
            
            // Save the Changes to the Database
            await data.SaveChangesAsync();
        }
    }
}
