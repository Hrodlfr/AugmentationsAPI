namespace AugmentationsAPI.Features.Identity.Models
{
    using System.ComponentModel.DataAnnotations;

    public class RegisterRequestModel
    {
        /// <summary>
        /// The Name of the User which Requests to be Registered.
        /// </summary>
        /// <example>JCDenton</example>
        [Required]
        public string UserName { get; set; }

        /// <summary>
        /// The Password of the User which Requests to be Registered.
        /// </summary>
        /// <example>NanoAugmented</example>
        [Required]
        public string Password { get; set; }
    }
}
