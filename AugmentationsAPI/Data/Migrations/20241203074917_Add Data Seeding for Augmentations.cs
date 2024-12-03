using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

#pragma warning disable CA1814 // Prefer jagged arrays over multidimensional

namespace AugmentationsAPI.Data.Migrations
{
    /// <inheritdoc />
    public partial class AddDataSeedingforAugmentations : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.InsertData(
                table: "Augmentations",
                columns: new[] { "Id", "Activation", "Area", "Description", "EnergyConsumption", "Name", "Type" },
                values: new object[,]
                {
                    { 1, 0, 3, "Muscle strength is amplified with ionic polymeric gel myofibrils that allow the agent to push and lift extraordinarily heavy objects.", 2, "Microfibral Muscle", 1 },
                    { 2, 0, 6, "Ionic polymeric gel myofibrils are woven into the leg muscles, increasing the speed at which an agent can run and climb, the height they can jump, and reducing the damage they receive from falls.", 2, "Speed Enhancement", 1 },
                    { 3, 2, 0, "Soda lime exostructures imbedded in the alveoli of the lungs convert CO2 to O2, extending the time an agent can remain underwater.", 1, "Aqualung", 1 },
                    { 4, 0, 5, "When activated, the Glass-Shield Cloaking System augmentation bends the light hitting the user, rendering him practically invisible. The effect will work on any wavelength that is part of the visual spectrum, including laser beams.", 4, "Glass-Shield Cloaking System", 0 },
                    { 5, 1, 2, "The Eye-Know Retinal Prosthesis is the basic 'chassis' for all optical augmentations and must be implanted in both eyes before further, more specialized devices can be purchased. The HUD projected by the prosthesis provides data on the user's medical condition, available equipment, wireless access to personal data storage, and direct audio/visual telecommunications.", 1, "Retinal Prosthesis", 0 },
                    { 6, 1, 1, "In its basic mode, the Wayfinder Radar System gives the user a limited-range 'radar' indicator, which is projected directly on to the retina optical user interface. In its advanced mode, the augmentation's detection threshold can be increased, and the movement of targets can be tracked even beyond visual range. Note that because there is no \"zoom\" control in advanced mode, the net effect is that the display is lower resolution, and enemies standing close together can blend into a single marker.", 1, "Wayfinder Radar System", 0 }
                });
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DeleteData(
                table: "Augmentations",
                keyColumn: "Id",
                keyValue: 1);

            migrationBuilder.DeleteData(
                table: "Augmentations",
                keyColumn: "Id",
                keyValue: 2);

            migrationBuilder.DeleteData(
                table: "Augmentations",
                keyColumn: "Id",
                keyValue: 3);

            migrationBuilder.DeleteData(
                table: "Augmentations",
                keyColumn: "Id",
                keyValue: 4);

            migrationBuilder.DeleteData(
                table: "Augmentations",
                keyColumn: "Id",
                keyValue: 5);

            migrationBuilder.DeleteData(
                table: "Augmentations",
                keyColumn: "Id",
                keyValue: 6);
        }
    }
}
