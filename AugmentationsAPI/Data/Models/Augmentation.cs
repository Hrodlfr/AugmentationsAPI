namespace AugmentationsAPI.Data.Models
{
    using System.ComponentModel.DataAnnotations;
    using System.Text.Json.Serialization;

    /// <summary>
    /// The Type of an Augmentation.
    /// There are Two Types of Augmentations.
    /// Mechanical Ones, which Replaces a Body Part Entirely,
    /// or NanoTechnological One, which use Nano Machines to Alter the Human Body without Removal of Body Parts.
    /// </summary>
    [JsonConverter(typeof(JsonStringEnumConverter))]
    public enum AugmentationType
    {
        Mechanical,
        NanoTechnological
    }

    /// <summary>
    /// The Area of the Human Body which an Augmentation Affects.
    /// </summary>
    [JsonConverter(typeof(JsonStringEnumConverter))]
    public enum AugmentationArea
    {
        Torso,
        Cranium,
        Eyes,
        Arms,
        Back,
        Skin,
        Legs
    }

    /// <summary>
    /// The Method of Activation of an Augmentation.
    /// </summary>
    [JsonConverter(typeof(JsonStringEnumConverter))]
    public enum AugmentationActivation
    {
        Manual,
        Passive,
        Contextual
    }

    /// <summary>
    /// The Amount of Energy which is Consumed by an Augmentation.
    /// </summary>
    [JsonConverter(typeof(JsonStringEnumConverter))]
    public enum AugmentationEnergyConsumption
    {
        Ammo,
        None,
        Low,
        Medium,
        High
    }

    /// <summary>
    /// An Augmentation of the Human Body. Which, either through Cybernetics or Nano Machines, 
    /// grants the User Abilities Such as Superhuman Strength, Highly Accelerated Regeneration, Invisibility and etc. 
    /// </summary>
    public class Augmentation
    {
        /// <summary>
        /// The Identification Number of the Augmentation
        /// </summary>
        /// <example>0</example>
        [Required]
        public int Id { get; set; }

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
