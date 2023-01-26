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

        /// <summary>
        /// Filters a List of Augmentations by the Given Parameters.
        /// </summary>
        /// <param name="augs"> A List of Augmentations which will be Filtered. </param>
        /// <param name="filteringParameters"> The Parameters Used for Filtering the List of the Augmentations. </param>
        /// <returns> A Filtered List of Augmentations. </returns>
        private List<AugmentationResponseModel> FilterAugmentationList(IEnumerable<AugmentationResponseModel> augs,
            AugmentationRequestFilteringParameters filteringParameters)
        {
            // If the Type Filter Is Set...
            if (filteringParameters.Type != null)
            {
                // ...Filter Out the Augmentations which Don't Adhere to the Filter
                augs = augs
                    .Where(aug => aug.Type == filteringParameters.Type);
            }

            // If the Area Filter Is Set...
            if (filteringParameters.Area != null)
            {
                // ...Filter Out the Augmentations which Don't Adhere to the Filter
                augs = augs
                    .Where(aug => aug.Area == filteringParameters.Area);
            }

            // If the Activation Filter Is Set...
            if (filteringParameters.Activation != null)
            {
                // ...Filter Out the Augmentations which Don't Adhere to the Filter
                augs = augs
                    .Where(aug => aug.Activation == filteringParameters.Activation);
            }

            // If the Energy Consumption Filter Is Set...
            if (filteringParameters.EnergyConsumption != null)
            {
                // ...Filter Out the Augmentations which Don't Adhere to the Filter
                augs = augs
                    .Where(aug => aug.EnergyConsumption == filteringParameters.EnergyConsumption);
            }

            // Returned the Filtered List
            return augs.ToList();
        }
        
        /// <summary>
        /// Pages a List of Augmentations by the Given Parameters.
        /// </summary>
        /// <param name="augs"> A List of Augmentations which will be Filtered. </param>
        /// <param name="pagingParameters"> The Parameters Used for Paging the List of the Augmentations. </param>
        /// <returns> A Paged List of Augmentations. </returns>
        private List<AugmentationResponseModel> PageAugmentationList(IEnumerable<AugmentationResponseModel> augs,
            AugmentationRequestPagingParameters pagingParameters)
        {
            // Return a Paged List of Augmentations
            return augs
                // Skip the Entities Contained in the Pages Before the Requested Page
                .Skip((pagingParameters.PageNumber - 1) * pagingParameters.PageSize)
                // Take the Specified Amount of Entities from the Current Page
                .Take(pagingParameters.PageSize)
                .ToList();
        }
        
        /// <summary>
        /// Searches a List of Augmentations by the Given Parameters.
        /// </summary>
        /// <param name="augs"> A List of Augmentations which will be Filtered. </param>
        /// <param name="searchParameters"> The Parameters Used for Searching the List of the Augmentations. </param>
        /// <returns> A List of Augmentations meeting the Search Criteria. </returns>
        private List<AugmentationResponseModel> SearchAugmentationList(IEnumerable<AugmentationResponseModel> augs,
            AugmentationRequestSearchParameters searchParameters)
        {
            // If there is No Search Term...
            if (string.IsNullOrWhiteSpace(searchParameters.SearchTerm))
            {
                // ...Return the Augmentations
                return augs.ToList();
            }

            // Transform the Term into Lower Case
            var lowerCaseTerm = searchParameters.SearchTerm.ToLower();

            // Return the Augmentations who meet the Search Criteria
            return augs
                .Where(aug => aug.Name.ToLower().Contains(lowerCaseTerm) 
                              || aug.Description.ToLower().Contains(lowerCaseTerm))
                .ToList();
        }
        
        public AugmentationRepository(ApplicationDbContext data)
        {
            this.data = data;
        }
        
        /// <summary>
        /// Returns a Paged List of all Augmentations from the Database.
        /// </summary>
        /// <param name="filteringParameters"> The Parameters Used for Filtering the List of the Augmentations. </param>
        /// <param name="pagingParameters"> The Parameters Used for Paging the List of the Augmentations. </param>
        /// <param name="searchParameters"> The Parameters Used for Searching the List of the Augmentations. </param>
        /// <returns> A Paged List of all Augmentations in the Database. </returns>
        public async Task<IEnumerable<AugmentationResponseModel>> GetAll(AugmentationRequestFilteringParameters filteringParameters,
            AugmentationRequestSearchParameters searchParameters,
                AugmentationRequestPagingParameters pagingParameters)
        {
            // Get the List of Augmentations as Data Transfer Objects using Projection for Better Performance
            var augs = await data.Augmentations
                .Select(aug => aug.Adapt<AugmentationResponseModel>())
                .ToListAsync();

            // Filter the List
            augs = FilterAugmentationList(augs, filteringParameters);

            // Page the List
            augs = PageAugmentationList(augs, pagingParameters);

            // Search the List
            augs = SearchAugmentationList(augs, searchParameters);

            // Return the List
            return augs;
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
