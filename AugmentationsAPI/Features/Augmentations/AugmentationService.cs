namespace AugmentationsAPI.Features.Augmentations
{
    using Data;
    using Data.Models;
    using Models;
    using Microsoft.EntityFrameworkCore;

    public class AugmentationService : IAugmentationService
    {
        /// <summary>
        /// The Database Context of the Application.
        /// </summary>
        private readonly ApplicationDbContext data;

        public AugmentationService(ApplicationDbContext data)
        {
            this.data = data;
        }
        
        /// <summary>
        /// Returns all Augmentations from the Database.
        /// </summary>
        /// <returns> All Augmentations in the Database. </returns>
        public async Task<IEnumerable<Augmentation>> GetAll()
        {
            // Return All Augmentations
            return await data.Augmentations.ToListAsync();
        }

        /// <summary>
        /// Returns an Augmentation with a Matching Id.
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
        /// Adds an Augmentation to the Database.
        /// </summary>
        /// <param name="model"> A Data Transfer Object whose Data will be Used to Create a Augmentation. </param>
        /// <returns> The Id of the newly Added Augmentation. </returns>
        public async Task<int> Create(AugmentationRequestModel model)
        {
            // Initialize a new Augmentation
            var augmentation = new Augmentation();
            
            // Pass the Values of the Model into the Initialized Augmentation
            augmentation.Name = model.Name;
            augmentation.Type = model.Type;
            augmentation.Area = model.Area;
            augmentation.Description = model.Description;
            augmentation.Activation = model.Activation;
            augmentation.EnergyConsumption = model.EnergyConsumption;
            
            // Add the Augmentation to the Database
            await data.Augmentations.AddAsync(augmentation);

            // Save the Changes to the Database
            await data.SaveChangesAsync();

            // Return the Id of the newly Added Augmentation
            return augmentation.Id;
        }

        /// <summary>
        /// Attempts to Update an Augmentation.
        /// </summary>
        /// <param name="id"> The Id of an Existing Augmentation whose Data will be Updated. </param>
        /// <param name="model"> A Data Transfer Object which contains the Updated Data for an Augmentation. </param>
        /// <returns> A Bool indicating whether the Update was Successful. </returns>
        public async Task<bool> Update(int id, AugmentationRequestModel model)
        {
            // Attempt to Get the Augmentation which is to be Updated
            var augToUpdate = await Get(id);

            // If The Augmentation was Not Found...
            if (augToUpdate == null)
            {
                // ...Return False, Indicating that the Update wasn't Successful
                return false;
            }
            // Else
            
            // Pass the Updated Values to the Augmentation
            augToUpdate.Name = model.Name;
            augToUpdate.Type = model.Type;
            augToUpdate.Area = model.Area;
            augToUpdate.Description = model.Description;
            augToUpdate.Activation = model.Activation;
            augToUpdate.EnergyConsumption = model.EnergyConsumption;
            
            // Save the Changes to the Database
            await data.SaveChangesAsync();

            // Return True, Indicating that the Update was Successful
            return true;
        }

        /// <summary>
        /// Attempts to Delete an Augmentation.
        /// </summary>
        /// <param name="id"> The Id of the Augmentation to be Deleted. </param>
        /// <returns> A Bool indicating whether the Deletion was Successful. </returns>
        public async Task<bool> Delete(int id)
        {
            // Attempt to Get the Augmentation which is to be Deleted
            var augToDelete = await Get(id);
            
            // If The Augmentation was Not Found...
            if (augToDelete == null)
            {
                // ...Return False, Indicating that the Deletion wasn't Successful
                return false;
            }
            // Else

            // Delete the Augmentation from the Database
            data.Augmentations.Remove(augToDelete);
            
            // Save the Changes to the Database
            await data.SaveChangesAsync();

            // Return True, Indicating that the Deletion was Successful
            return true;
        }
    }
}
