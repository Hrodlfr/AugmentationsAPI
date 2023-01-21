namespace AugmentationsAPI.Features.Augmentations.Models
{
    using System.ComponentModel.DataAnnotations;
    using Data.Models;

    /// <summary>
    /// A Data Transfer Object Used to Create a New Augmentation or Update an Existing One.
    /// </summary>
    public class AugmentationRequestModel
    {
        /// <summary>
        /// The Type of this Augmentation.
        /// </summary>
        [Required]
        public AugmentationType Type { get; set; }

        /// <summary>
        /// The Area of the Human Body which this Augmentation Affects.
        /// </summary>
        [Required]
        public AugmentationArea Area { get; set; }

        /// <summary>
        /// The Name of this Augmentation.
        /// </summary>
        /// <example>Typhoon Explosive System</example>
        [Required]
        public string Name { get; set; } = string.Empty;

        /// <summary>
        /// The Description of this Augmentation
        /// </summary>
        /// <example>The Typhoon is a series of interlinked electromagnetic launchers installed in the user's arms and connected to a processor at the base of the user's spine. The system launches a number of spheres loaded with shaped liquid crystal elastomer filled with pentaerythritol tetranitrate microcharges which then explode, sending small steel ball bearings out as lethal shrapnel in all directions around the firer.</example>
        [Required]
        public string Description { get; set; } = string.Empty;

        /// <summary>
        /// The Method of Activation of this Augmentation.
        /// </summary>
        [Required]
        public AugmentationActivation Activation { get; set; }

        /// <summary>
        /// The Amount of Energy Consumed by this Augmentation.
        /// </summary>
        [Required]
        public AugmentationEnergyConsumption EnergyConsumption { get; set; }
    }
}
