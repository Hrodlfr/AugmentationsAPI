namespace AugmentationsAPI.Features.Augmentations
{
    using Data.Models;
    using Models;

    public interface IAugmentationService
    {
        /// <summary>
        /// Returns all Augmentations from the Database.
        /// </summary>
        /// <returns> All Augmentations in the Database. </returns>
        public Task<IEnumerable<AugmentationResponseModel>> GetAll();

        /// <summary>
        /// Returns an Augmentation with a Matching Id which is Tracked by Entity Framework..
        /// </summary>
        /// <param name="id"> An Id which will be used to Find a matching Augmentation. </param>
        /// <returns> An Augmentation with a Matching Id Or Null If an Augmentation with a Matching Id wasn't Found. </returns>
        public Task<Augmentation?> Get(int id);
        
        /// <summary>
        /// Returns an Augmentation with a Matching Id which is Not Tracked by Entity Framework..
        /// </summary>
        /// <param name="id"> An Id which will be used to Find a matching Augmentation. </param>
        /// <param name="tracking"> A Boolean Value which Indicates whether the Returned Entity should be Tracked or Not. </param>
        /// <returns> An Augmentation with a Matching Id Or Null If an Augmentation with a Matching Id wasn't Found. </returns>
        public Task<AugmentationResponseModel?> Get(int id, bool tracking);

        /// <summary>
        /// Adds an Augmentation to the Database.
        /// </summary>
        /// <param name="model"> A Data Transfer Object whose Data will be Used to Create a Augmentation. </param>
        /// <returns> The Id of the newly Added Augmentation. </returns>
        public Task<int> Create(AugmentationRequestModel model);

        /// <summary>
        /// Updates an Augmentation.
        /// </summary>
        /// <param name="augToUpdate"> An Augmentation whose Data will be Updated. </param>
        /// <param name="model"> A Data Transfer Object which contains the Updated Data for an Augmentation. </param>
        public Task Update(Augmentation augToUpdate, AugmentationRequestModel model);

        /// <summary>
        /// Deletes an Augmentation.
        /// </summary>
        /// <param name="augToDelete"> The Augmentation to be Deleted. </param>
        public Task Delete(Augmentation augToDelete);
    }
}
