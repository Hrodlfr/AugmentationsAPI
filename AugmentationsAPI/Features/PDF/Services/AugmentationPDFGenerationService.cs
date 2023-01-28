namespace AugmentationsAPI.Features.PDF.Services
{
    using Augmentations.Models;
    using System.Collections.Generic;
    using QuestPDF.Fluent;
    using QuestPDF.Helpers;
    using QuestPDF.Infrastructure;

    public class AugmentationPDFGenerationService : IPDFGenerationService<AugResponseModel>
    {
        private const string DystopianGoldHex = "#FFD700";
        private const string HeaderText = "Augmentations";
        private const string FooterText = "Deus Ex";
        private const string Area = "Area";
        private const string Activation = "Activation";
        private const string Consumption = "Consumption";

        private const int HeaderAndFooterPadding = 40;
        private const int PagePaddingInCentimetres = 2;
        private const int RowPadding = 20;
        private const int HorizontalPadding = 50;
        private const int FontSizeDefault = 20;
        private const int FontSizeHeaderFooter = 35;
        private const int FontSizeAugmentationName = 25;
        private const int ColumnSpacing = 60;

        /// <summary>
        /// Generates a PDF File from the given Augmentations.
        /// </summary>
        /// <param name="resources"> The Augmentations which will be Used for Generating a Pdf File. </param>
        /// <returns> A Byte Array of the Generated PDF File. </returns>
        public byte[] GeneratePdf(IEnumerable<AugResponseModel> resources)
        {
            // Create the PDF Document
            var pdf = Document.Create(container =>
            {
                container.Page(page =>
                {
                    // Set the Size of a Page
                    page.Size(PageSizes.A4);
                    // Set the Background Color of a Page
                    page.PageColor(Colors.Black);
                    // Set the Default Text Style of the PDf
                    page.DefaultTextStyle(text => text.FontSize(FontSizeDefault)
                                                    .FontColor(DystopianGoldHex)
                                                    .FontFamily(Fonts.Arial));

                    // Set the Header
                    page.Header()
                        // Set Top Padding
                        .PaddingTop(HeaderAndFooterPadding)
                        // Center the Header
                        .AlignCenter()
                        .AlignMiddle()
                        // Add a Border to the Headers Bottom
                        .BorderBottom(1)
                        // Set the Borders Color
                        .BorderColor(DystopianGoldHex)
                        // Add Text to the Header
                        .Text(HeaderText)
                        // Set the Texts Style
                        .FontFamily(Fonts.Consolas)
                        .SemiBold()
                        .FontSize(FontSizeHeaderFooter)
                        .FontColor(DystopianGoldHex);

                    // Set the Footer
                    page.Footer()
                        // Set Bottom Padding
                        .PaddingBottom(HeaderAndFooterPadding)
                        // Center the Footer
                        .AlignCenter()
                        .AlignMiddle()
                        // Add a Border to the Headers Top
                        .BorderTop(1)
                        .BorderColor(DystopianGoldHex)
                        // Add Text to the Header
                        .Text(FooterText)
                        // Set the Texts Style
                        .FontFamily(Fonts.Consolas)
                        .SemiBold()
                        .FontSize(FontSizeHeaderFooter)
                        .FontColor(DystopianGoldHex);

                    // Set the Content of a Page
                    page.Content()
                        // Add Padding to a Pages Content
                        .Padding(PagePaddingInCentimetres, Unit.Centimetre)
                        // Center the Content
                        .AlignCenter()

                        // Add a Column
                        .Column(column =>
                        {
                            // Set the Spacing of the Coulmns Items
                            column.Spacing(ColumnSpacing);

                            // For Each Augmentation in the List...
                            foreach (var aug in resources)
                            {
                                // Add a Row to the Column
                                column.Item()
                                // Add a Border to the Row
                                .Border(1)
                                // Set the Borders Color
                                .BorderColor(DystopianGoldHex)
                                // Add Padding to the Row
                                .Padding(RowPadding)
                                .Row(row =>
                                {
                                    // Add a Table
                                    row.RelativeItem()
                                    .Table(table =>
                                    {
                                        // Define 3 Relative Columns for the Table
                                        table.ColumnsDefinition(columns =>
                                        {
                                            columns.RelativeColumn();
                                            columns.RelativeColumn();
                                            columns.RelativeColumn();
                                        });

                                        // Add a Table Cell for the Name of the Augmentation
                                        table.Cell()
                                        // Set the Row, Column and Span
                                        .Row(1)
                                        .Column(1)
                                        .ColumnSpan(3)
                                        // Add Text to the Cell
                                        .Text(text =>
                                        {
                                            // Align the Text to the Center
                                            text.AlignCenter();

                                            // Set the Text to the Augmentations Name
                                            text.Span(aug.Name)
                                            // Set Extra Styles for the Text
                                            .FontSize(FontSizeAugmentationName)
                                            .ExtraBold();
                                        });

                                        // Add a Table Cell for the Description of the Augmentation
                                        table.Cell()
                                        // Set the Row, Column and Span
                                        .Row(2)
                                        .Column(1)
                                        .ColumnSpan(3)
                                        // Add Text to the Cell
                                        .Text(text =>
                                        {
                                            // Align the Text to the Center
                                            text.AlignCenter();

                                            // Set the Text to the Augmentations Description
                                            text.Span(aug.Description);
                                        });

                                        // Add a Table Cell for a Horizontal Line
                                        table.Cell()
                                        // Set the Row, Column and Span
                                        .Row(3)
                                        .Column(1)
                                        .ColumnSpan(3)
                                        // Set the Horizontal Padding of the Line
                                        .PaddingHorizontal(HorizontalPadding)
                                        // Set the Size of the Line
                                        .LineHorizontal(1)
                                        // Set the Color of the Line
                                        .LineColor(DystopianGoldHex);

                                        // Add a Table Cell for the Text "Area"
                                        table.Cell()
                                        // Set the Row, Column and Span
                                        .Row(3)
                                        .Column(1)
                                        .ColumnSpan(1)
                                        // Add Text to the Cell
                                        .Text(text =>
                                        {
                                            // Align the Text to the Center
                                            text.AlignCenter();

                                            // Set the Text
                                            text.Span(Area)
                                            // Set Extra Styles for the Text
                                            .Italic();
                                        });

                                        // Add a Table Cell for a Horizontal Line which Seperates the "Area" Text and the Augmentations Area
                                        table.Cell()
                                        // Set the Row, Column and Span
                                        .Row(4)
                                        .Column(1)
                                        .ColumnSpan(1)
                                        // Set the Horizontal Padding of the Line
                                        .PaddingHorizontal(HorizontalPadding)
                                        // Set the Size of the Line
                                        .LineHorizontal(1)
                                        // Set the Color of the Line
                                        .LineColor(DystopianGoldHex);

                                        // Add a Table Cell for the Area of the Augmentation
                                        table.Cell()
                                        // Set the Row, Column and Span
                                        .Row(5)
                                        .Column(1)
                                        .ColumnSpan(1)
                                        // Add Text to the Cell
                                        .Text(text =>
                                        {
                                            // Align the Text to the Center
                                            text.AlignCenter();

                                            // Set the Text
                                            text.Span(aug.Area.ToString())
                                            // Add Extra Styles to the Text
                                            .Bold();
                                        });

                                        // Add a Table Cell for the Text "Activation"
                                        table.Cell()
                                        // Set the Row, Column and Span
                                        .Row(3)
                                        .Column(2)
                                        .ColumnSpan(1)
                                        // Add Text to the Cell
                                        .Text(text =>
                                        {
                                            // Align the Text to the Center
                                            text.AlignCenter();

                                            // Set the Text
                                            text.Span(Activation)
                                            // Add Extra Styles to the Text
                                            .Italic();
                                        });

                                        // Add a Table Cell for a Horizontal Line which Seperates the "Activation" Text and the Augmentations Activaiton
                                        table.Cell()
                                        // Set the Row, Column and Span
                                        .Row(4)
                                        .Column(2)
                                        .ColumnSpan(1)
                                        // Set the Horizontal Padding of the Line
                                        .PaddingHorizontal(HorizontalPadding)
                                        // Set the Size of the Line
                                        .LineHorizontal(1)
                                        // Set the Color of the Line
                                        .LineColor(DystopianGoldHex);

                                        // Add a Table Cell for the Activation of the Augmentation
                                        table.Cell()
                                        // Set the Row, Column and Span
                                        .Row(5)
                                        .Column(2)
                                        .ColumnSpan(1)
                                        // Add Text to the Cell
                                        .Text(text =>
                                        {
                                            // Align the Text to the Center
                                            text.AlignCenter();

                                            // Set the Text
                                            text.Span(aug.Activation.ToString())
                                            // Add Extra Styles to the Text
                                            .Bold();
                                        });

                                        // Add a Table Cell for the Text "Consumption"
                                        table.Cell()
                                        // Set the Row, Column and Span
                                        .Row(3)
                                        .Column(3)
                                        .ColumnSpan(1)
                                        // Add Text to the Cell
                                        .Text(text =>
                                        {
                                            // Align the Text to the Center
                                            text.AlignCenter();

                                            // Set the Text
                                            text.Span(Consumption)
                                            // Add Extra Styles to the Text
                                            .Italic();
                                        });

                                        // Add a Table Cell for a Horizontal Line which Seperates the "Consumption" Text and the Augmentations Consumption
                                        table.Cell()
                                        // Set the Row, Column and Span
                                        .Row(4)
                                        .Column(3)
                                        .ColumnSpan(1)
                                        // Set the Horizontal Padding of the Line
                                        .PaddingHorizontal(HorizontalPadding)
                                        // Set the Size of the Line
                                        .LineHorizontal(1)
                                        // Set the Color of the Line
                                        .LineColor(DystopianGoldHex);

                                        // Add a Table Cell for the Consumption of the Augmentation
                                        table.Cell()
                                        // Set the Row, Column and Span
                                        .Row(5)
                                        .Column(3)
                                        .ColumnSpan(1)
                                        // Add Text to the Cell
                                        .Text(text =>
                                        {
                                            // Align the Text to the Center
                                            text.AlignCenter();

                                            text
                                            // Set the Text
                                            .Span(aug.EnergyConsumption.ToString())
                                            // Add Extra Styles to the Text
                                            .Bold();
                                        });
                                    });
                                });
                            }

                        });
                });
            });

            // Generate and Return the PDF File
            return pdf.GeneratePdf();
        }
    }
}
