namespace AugmentationsAPI.Features.PDF
{
    public interface IPDFGenerationService<TResource>
    {
        /// <summary>
        /// Generates a PDF File from the given Resources.
        /// </summary>
        /// <param name="resources"> The Resources which will be used for Generating a PDF File. </param>
        /// <returns> A Byte Array of the Generated PDF File. </returns>
        public byte[] GeneratePdf(IEnumerable<TResource> resources);
    }
}
