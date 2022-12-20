namespace AugmentationsAPI.Features.Identity.Models
{
    using System.ComponentModel.DataAnnotations;

    public class LoginRequestModel
    {
        /// <summary>
        /// The Name of the User which Requests to be Logged In.
        /// </summary>
        /// <example>JCDenton</example>
        [Required]
        public string UserName { get; set; }

        /// <summary>
        /// The Password of the User which Requests to be Logged In.
        /// </summary>
        /// <example>NanoAugmented</example>
        [Required]
        public string Password { get; set; }
    }
}
