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
        public Task<IEnumerable<Augmentation>> GetAll(); 
        
        /// <summary>
        /// Returns an Augmentation with a Matching Id.
        /// </summary>
        /// <param name="id"> An Id which will be used to Find a matching Augmentation. </param>
        /// <returns> An Augmentation with a Matching Id Or Null If an Augmentation with a Matching Id wasn't Found. </returns>
        public Task<Augmentation?> Get(int id);

        /// <summary>
        /// Adds an Augmentation to the Database.
        /// </summary>
        /// <param name="model"> A Data Transfer Object whose Data will be Used to Create a Augmentation. </param>
        /// <returns> The Id of the newly Added Augmentation. </returns>
        public Task<int> Create(AugmentationRequestModel model);

        /// <summary>
        /// Attempts to Update an Augmentation.
        /// </summary>
        /// <param name="id"> The Id of an Existing Augmentation whose Data will be Updated. </param>
        /// <param name="model"> A Data Transfer Object which contains the Updated Data for an Augmentation. </param>
        /// <returns> A Bool indicating whether the Update was Successful. </returns>
        public Task<bool> Update(int id, AugmentationRequestModel model);

        /// <summary>
        /// Attempts to Delete an Augmentation.
        /// </summary>
        /// <param name="id"> The Id of the Augmentation to be Deleted. </param>
        /// <returns> A Bool indicating whether the Deletion was Successful. </returns>
        public Task<bool> Delete(int id);

    }
}
