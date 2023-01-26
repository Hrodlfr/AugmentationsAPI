namespace AugmentationsAPI.Features.Augmentations.Models
{
    using Data.Models;

    /// <summary>
    /// The Parameters Used for Filtering the a List of Augmentations. 
    /// </summary>
    public class AugmentationRequestFilteringParameters
    {
        /// <summary>
        /// The Type By which the List of Augmentations List will be Filtered.
        /// </summary>
        public AugmentationType? Type { get; set; } = null;

        /// <summary>
        /// The Area By which the List of Augmentations will be Filtered.
        /// </summary>
        public AugmentationArea? Area { get; set; } = null;

        /// <summary>
        /// The Activation Type By which the List of Augmentations List will be Filtered.
        /// </summary>
        public AugmentationActivation? Activation { get; set; } = null;

        /// <summary>
        /// The Energy Consumption By which the List of Augmentations List will be Filtered.
        /// </summary>
        public AugmentationEnergyConsumption? EnergyConsumption { get; set; } = null;
    }
}
