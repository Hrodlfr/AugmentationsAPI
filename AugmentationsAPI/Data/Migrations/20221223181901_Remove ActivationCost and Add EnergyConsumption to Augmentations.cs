using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace AugmentationsAPI.Data.Migrations
{
    /// <inheritdoc />
    public partial class RemoveActivationCostandAddEnergyConsumptiontoAugmentations : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.RenameColumn(
                name: "ActivationCost",
                table: "Augmentations",
                newName: "EnergyConsumption");
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.RenameColumn(
                name: "EnergyConsumption",
                table: "Augmentations",
                newName: "ActivationCost");
        }
    }
}
