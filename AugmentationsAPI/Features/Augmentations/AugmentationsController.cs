namespace AugmentationsAPI.Features.Augmentations
{
    using Data.Models;
    using Microsoft.AspNetCore.Authorization;
    using Microsoft.AspNetCore.Mvc;
    using Models;

    [Route("[controller]")]
    [ApiController]
    [Authorize]
    public class AugmentationsController : ControllerBase
    {
        private readonly IAugmentationService augmentationService;

        public AugmentationsController(IAugmentationService augmentationService)
        {
            this.augmentationService = augmentationService;
        }

        /// <summary>
        /// Returns all Augmentations from the Database.
        /// </summary>
        /// 
        /// <remarks>
        /// Sample Request:
        ///
        ///     GET Augmentations
        ///     {
        ///     }
        /// 
        /// </remarks>
        /// 
        /// <returns> All Augmentations from the Database. </returns>
        ///
        /// <response code="200"> All Augmentations from the Database were Returned. </response>
        /// <response code="401"> The User is not Authorized to Perform this Action. </response>
        [HttpGet]
        [ProducesResponseType(StatusCodes.Status200OK)]
        [ProducesResponseType(StatusCodes.Status401Unauthorized)]
        [Produces("application/json")]
        public async Task<ActionResult<IEnumerable<Augmentation>>> GetAll()
        {
            // Return all Augmentations from the Database
            return Ok(await augmentationService.GetAll());
        }

        /// <summary>
        /// Attempts to Return an Augmentation with a Matching Id from the Database.
        /// </summary>
        ///
        /// <remarks>
        /// Sample Request:
        ///
        ///     GET Augmentations/1
        ///     {
        ///     }
        ///
        /// </remarks>
        /// 
        /// <param name="id"> The Id which will be used to find a matching Augmentation. </param>
        /// 
        /// <returns> A Specific Augmentation or NotFound If an Augmentation with a Matching Id Couldn't be Found. </returns>
        /// 
        /// <response code="200"> The Matching Augmentation was Returned. </response>
        /// <response code="404"> The Matching Augmentation wasn't found. </response>
        /// <response code="401"> The User is not Authorized to Perform this Action. </response>
        [HttpGet("{id:int}")]
        [ProducesResponseType(StatusCodes.Status200OK)]
        [ProducesResponseType(StatusCodes.Status404NotFound)]
        [ProducesResponseType(StatusCodes.Status401Unauthorized)]
        [Produces("application/json")]
        public async Task<ActionResult<Augmentation>> Get(int id)
        {
            // Attempt to Get the Matching Augmentation
            var matchingAug = await augmentationService.Get(id);

            // Return Ok with the Matching Augmentation...
            // OR Not Found If the Augmentation wasn't Found
            return matchingAug == null ? NotFound() : Ok(matchingAug);
        }

        /// <summary>
        /// Attempts to Create a New Augmentation.
        /// </summary>
        ///
        /// <remarks>
        /// Sample Request:
        ///
        ///     POST Augmentations
        ///     {
        ///         "type": "Mechanical",
        ///         "area": "Torso",
        ///         "name": "Typhoon Explosive System",
        ///         "description": "The Typhoon is a series of interlinked electromagnetic launchers installed in the user's arms and connected to a processor at the base of the user's spine. The system launches a number of spheres loaded with shaped liquid crystal elastomer filled with pentaerythritol tetranitrate microcharges which then explode, sending small steel ball bearings out as lethal shrapnel in all directions around the firer.",
        ///         "activation": "Manual",
        ///         "energyConsumption": "Ammo"
        ///     }
        /// 
        /// </remarks>
        /// 
        /// <param name="model"> A Data Transfer Object whose Data will be Used to Create a Augmentation. </param>
        /// 
        /// <returns> The newly Created Augmentation or BadRequest If it wasn't valid. </returns>
        ///
        /// <response code="201"> The New Augmentation was Created and Returned. </response>
        /// <response code="400"> The New Augmentation isn't Valid. </response>
        /// <response code="401"> The User is not Authorized to Perform this Action. </response>
        [HttpPost]
        [ProducesResponseType(StatusCodes.Status201Created)]
        [ProducesResponseType(StatusCodes.Status400BadRequest)]
        [ProducesResponseType(StatusCodes.Status401Unauthorized)]
        [Produces("application/json")]
        public async Task<ActionResult> Create(AugmentationRequestModel model)
        {
            // Create the New Augmentation
            var idOfNewAug = await augmentationService.Create(model);

            // Generate the URL of the New Augmentation
            var urlOfTheNewAug = Url.Action(nameof(Get), nameof(Augmentations), new { id = idOfNewAug })!;
            
            // Return Created with the New Augmentation in the Response Body and Its URL in the Response Header
            return Created(urlOfTheNewAug, await augmentationService.Get(idOfNewAug));
        }

        /// <summary>
        /// Attempts to Update an Existing Augmentation.
        /// </summary>
        ///
        /// <remarks>
        /// Sample Request:
        ///
        ///     PUT Augmentations/1
        ///     {
        ///         "type": "NanoTechnological",
        ///         "area": "Arms",
        ///         "name": "Combat Strength",
        ///         "description": "Sorting rotors accelerate calcium ion concentration in the sarcoplasmic reticulum, increasing an agent's muscle speed several-fold and multiplying the damage they inflict in melee combat.",
        ///         "activation": "Manual",
        ///         "energyConsumption": "Medium"
        ///     }
        /// 
        /// </remarks>
        /// 
        /// <param name="id"> The Id of an Existing Augmentation whose Data will be Updated. </param>
        /// <param name="model"> A Data Transfer Object which contains the Updated Data for an Augmentation. </param>
        /// 
        /// <returns> Ok if the Update was Successful,
        ///         NotFound If the Augmentation is Not Found,
        ///         Or BadRequest If the New Values weren't Valid. </returns>
        /// 
        /// <response code="200"> The Augmentation was Updated. </response>
        /// <response code="400"> The Augmentation couldn't be Updated. </response>
        /// <response code="401"> The User is not Authorized to Perform this Action. </response>
        [HttpPut("{id:int}")]
        [ProducesResponseType(StatusCodes.Status200OK)]
        [ProducesResponseType(StatusCodes.Status400BadRequest)]
        [ProducesResponseType(StatusCodes.Status401Unauthorized)]
        public async Task<ActionResult> Update(int id, AugmentationRequestModel model)
        {
            // Attempt Update the Augmentation
            var updateSuccessful = await augmentationService.Update(id, model);

            // Return Ok If the Update was Successful...
            // OR BadRequest If the Update was Unsuccessful
            return updateSuccessful == false ? BadRequest() : Ok();
        }

        /// <summary>
        /// Attempts to Delete and Existing Augmentation. 
        /// </summary>
        ///
        /// <remarks>
        /// Sample Request:
        ///
        ///     DELETE Augmentations/1
        ///     {
        ///     }
        /// 
        /// </remarks>
        /// 
        /// <param name="id"> The Id of the Augmentation which is to be Deleted. </param>
        /// 
        /// <returns> Ok if the Deletion was Successful
        ///         or BadRequest If it wasn't. </returns>
        ///
        /// <response code="200"> The Augmentation was Deleted. </response>
        /// <response code="400"> The Augmentation couldn't be Deleted. </response>
        /// <response code="401"> The User is not Authorized to Perform this Action. </response>
        [HttpDelete("{id:int}")]
        [ProducesResponseType(StatusCodes.Status200OK)]
        [ProducesResponseType(StatusCodes.Status400BadRequest)]
        [ProducesResponseType(StatusCodes.Status401Unauthorized)]
        public async Task<ActionResult> Delete(int id)
        {
            // Attempt to Delete the Augmentation
            var result = await augmentationService.Delete(id);
            
            // Return Ok If the Deletion was Successful...
            // OR BadRequest If the Deletion was Unsuccessful
            return result == false ? BadRequest() : Ok();
        }

    }
}
