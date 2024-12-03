namespace AugmentationsAPI.Data.Seeding;

using AugmentationsAPI.Data.Models;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;

/// <summary>
/// A Class for Seperating the Seeding Logic for Augmentations.
/// </summary>
public class SeederAugmentation : IEntityTypeConfiguration<Augmentation>
{
    /// <summary>
    /// Seeds multiple Augmentations into the Database
    /// </summary>
    /// <param name="builder"> An Entity Framework Model Builder. </param>
    public void Configure(EntityTypeBuilder<Augmentation> builder)
    {
        // Seed Multiple Augmentations into the Database
        builder.HasData(
            
            new Augmentation() { Id = 1,
                Name = "Microfibral Muscle",
                Description = "Muscle strength is amplified with ionic polymeric gel myofibrils that allow the agent to push and lift extraordinarily heavy objects.",
                Type = AugmentationType.NanoTechnological,
                Area = AugmentationArea.Arms,
                Activation = AugmentationActivation.Manual,
                EnergyConsumption = AugmentationEnergyConsumption.Low },

            new Augmentation() { Id = 2,
                Name = "Speed Enhancement",
                Description = "Ionic polymeric gel myofibrils are woven into the leg muscles, increasing the speed at which an agent can run and climb, the height they can jump, and reducing the damage they receive from falls.",
                Type = AugmentationType.NanoTechnological,
                Area = AugmentationArea.Legs,
                Activation = AugmentationActivation.Manual,
                EnergyConsumption = AugmentationEnergyConsumption.Low },

            new Augmentation() { Id = 3,
                Name = "Aqualung",
                Description = "Soda lime exostructures imbedded in the alveoli of the lungs convert CO2 to O2, extending the time an agent can remain underwater.",
                Type = AugmentationType.NanoTechnological,
                Area = AugmentationArea.Torso,
                Activation = AugmentationActivation.Contextual,
                EnergyConsumption = AugmentationEnergyConsumption.None },

            new Augmentation() { Id = 4,
                Name = "Glass-Shield Cloaking System",
                Description = "When activated, the Glass-Shield Cloaking System augmentation bends the light hitting the user, rendering him practically invisible. The effect will work on any wavelength that is part of the visual spectrum, including laser beams.",
                Type = AugmentationType.Mechanical,
                Area = AugmentationArea.Skin,
                Activation = AugmentationActivation.Manual,
                EnergyConsumption = AugmentationEnergyConsumption.High },

            new Augmentation() { Id = 5,
                Name = "Retinal Prosthesis",
                Description = "The Eye-Know Retinal Prosthesis is the basic 'chassis' for all optical augmentations and must be implanted in both eyes before further, more specialized devices can be purchased. The HUD projected by the prosthesis provides data on the user's medical condition, available equipment, wireless access to personal data storage, and direct audio/visual telecommunications.",
                Type = AugmentationType.Mechanical,
                Area = AugmentationArea.Eyes,
                Activation = AugmentationActivation.Passive,
                EnergyConsumption = AugmentationEnergyConsumption.None },

            new Augmentation() { Id = 6,
                Name = "Wayfinder Radar System",
                Description = "In its basic mode, the Wayfinder Radar System gives the user a limited-range 'radar' indicator, which is projected directly on to the retina optical user interface. In its advanced mode, the augmentation's detection threshold can be increased, and the movement of targets can be tracked even beyond visual range. Note that because there is no \"zoom\" control in advanced mode, the net effect is that the display is lower resolution, and enemies standing close together can blend into a single marker.",
                Type = AugmentationType.Mechanical,
                Area = AugmentationArea.Cranium,
                Activation = AugmentationActivation.Passive,
                EnergyConsumption = AugmentationEnergyConsumption.None }

            );
    }
}
