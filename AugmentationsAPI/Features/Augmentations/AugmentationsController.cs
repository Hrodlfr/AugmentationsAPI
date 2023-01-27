namespace AugmentationsAPI.Features.Augmentations
{
    using Links;
    using Mapster;
    using Models;
    using Microsoft.AspNetCore.Authorization;
    using Microsoft.AspNetCore.JsonPatch;
    using Microsoft.AspNetCore.Mvc;
    using static Infrastructure.Constants;

    [Route("[controller]")]
    [ApiController]
    [Authorize]
    public class AugmentationsController : ControllerBase
    {
        private readonly IAugmentationRepository augRepo;
        private readonly ILinkGenerationService linkGenerationService;

        public AugmentationsController(IAugmentationRepository augRepo, ILinkGenerationService linkGenerationService)
        {
            this.augRepo = augRepo;
            this.linkGenerationService = linkGenerationService;
        }

        /// <summary>
        /// Returns a List of Augmentations.
        /// </summary>
        /// 
        /// <remarks>
        /// The List of Augmentations Can be Searched, Filtered and Is Paged.
        /// 
        /// Sample Request:
        ///
        ///     GET Augmentations?pageSize=50&#38;pageNumber=1&#38;searchTerm=Typhoon
        ///     {
        ///     }
        /// </remarks>
        ///
        /// <param name="pagingParameters"> The Parameters Used for Paging the List of the Augmentations. </param>
        /// <param name="searchParameters"> The Parameters Used for Searching the List of the Augmentations. </param>
        /// <param name="filteringParameters"> The Parameters Used for Filtering the List of the Augmentations. </param>
        /// 
        /// <returns> A List of Augmentations. </returns>
        ///
        /// <response code="200"> Returns the List of Augmentations. </response>
        /// <response code="400"> If the Parameters Aren't Valid. </response>
        /// <response code="401"> If the User Isn't Authorized. </response>
        [HttpGet]
        [Consumes(ContentTypeApplicationJson)]
        [Produces(ContentTypeApplicationJson)]
        [ProducesResponseType(StatusCodes.Status200OK)]
        [ProducesResponseType(StatusCodes.Status400BadRequest)]
        [ProducesResponseType(StatusCodes.Status401Unauthorized)]
        public async Task<ActionResult<IEnumerable<AugmentationResponseModel>>> GetAll(
            [FromQuery]AugmentationRequestPagingParameters pagingParameters,
            [FromQuery]AugmentationRequestSearchParameters searchParameters,
            [FromQuery]AugmentationRequestFilteringParameters filteringParameters)
        {
            // Get the List of Augmentations from the Database
            var augs = await augRepo.GetAll(filteringParameters, searchParameters, pagingParameters);

            // For Each Augmentation in the List...
            foreach (var aug in augs)
            {
                // ...Generate Links
                aug.Links = linkGenerationService.GenerateLinks(aug.Id);
            }
            
            // Return the List of Augmentations
            return Ok(augs);
        }

        /// <summary>
        /// Returns a Specific Augmentation.
        /// </summary>
        ///
        /// <remarks>
        /// Sample Request:
        ///
        ///     GET Augmentations/1
        ///     {
        ///     }
        /// </remarks>
        /// 
        /// <param name="id"> The Id which will be used to Find a Specific Augmentation. </param>
        /// 
        /// <returns> A Specific Augmentation. </returns>
        /// 
        /// <response code="200"> Returns the Specific Augmentation. </response>
        /// <response code="401"> If the User Isn't Authorized. </response>
        /// <response code="404"> If the Specific Augmentation Can't be Found. </response>
        [HttpGet(RouteIdParameter)]
        [Consumes(ContentTypeApplicationJson)]
        [Produces(ContentTypeApplicationJson)]
        [ProducesResponseType(StatusCodes.Status200OK)]
        [ProducesResponseType(StatusCodes.Status401Unauthorized)]
        [ProducesResponseType(StatusCodes.Status404NotFound)]
        public async Task<ActionResult<AugmentationResponseModel>> Get(int id)
        {
            // Get the Specific Augmentation
            var specificAug = await augRepo.Get(id, false);

            // If the Augmentation was Not Found...
            if (specificAug == null)
            {
                // Return NotFound
                return NotFound();
            }
            
            // Generate HATEOAS Links for the Specific Augmentation
            specificAug.Links = linkGenerationService.GenerateLinks(specificAug.Id);
            
            // Return Ok with the Specific Augmentation
            return Ok(specificAug);
        }

        /// <summary>
        /// Creates a New Augmentation.
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
        /// </remarks>
        /// 
        /// <param name="model"> A Data Transfer Object whose Data will be Used to Create a Augmentation. </param>
        /// 
        /// <returns> The Newly Created Augmentation. </returns>
        ///
        /// <response code="201"> Returns the Newly Created Augmentation. </response>
        /// <response code="400"> If the New Augmentation isn't Valid. </response>
        /// <response code="401"> If the User Isn't Authorized. </response>
        [HttpPost]
        [Consumes(ContentTypeApplicationJson)]
        [Produces(ContentTypeApplicationJson)]
        [ProducesResponseType(StatusCodes.Status201Created)]
        [ProducesResponseType(StatusCodes.Status400BadRequest)]
        [ProducesResponseType(StatusCodes.Status401Unauthorized)]
        public async Task<ActionResult> Post(AugmentationRequestModel model)
        {
            // Create the New Augmentation
            var idOfNewAug = await augRepo.Create(model);

            // Generate the URL of the New Augmentation
            var urlOfTheNewAug = Url.Action(nameof(Get), nameof(Augmentations), new { id = idOfNewAug })!;

            // Get the New Augmentation
            var newAug = await augRepo.Get(idOfNewAug, false);

            // Generate Links for the New Augmentation
            newAug!.Links = linkGenerationService.GenerateLinks(newAug.Id);
            
            // Return Created with the New Augmentation in the Response Body and Its URL in the Response Header
            return Created(urlOfTheNewAug, newAug);
        }

        /// <summary>
        /// Updates a Specific Augmentation.
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
        /// </remarks>
        /// 
        /// <param name="id"> The Id of a Specific Augmentation whose Data will be Updated. </param>
        /// <param name="model"> A Data Transfer Object which contains the Updated Data for an Augmentation. </param>
        /// 
        /// <returns> The Newly Updated Augmentation. </returns>
        /// 
        /// <response code="200"> Returns the Newly Updated Augmentation. </response>
        /// <response code="400"> If the Updated Data Isn't Valid. </response>
        /// <response code="401"> If the User Isn't Authorized. </response>
        /// <response code="404"> If the Specific Augmentation Can't be Found. </response>
        [HttpPut(RouteIdParameter)]
        [Consumes(ContentTypeApplicationJson)]
        [Produces(ContentTypeApplicationJson)]
        [ProducesResponseType(StatusCodes.Status200OK)]
        [ProducesResponseType(StatusCodes.Status400BadRequest)]
        [ProducesResponseType(StatusCodes.Status401Unauthorized)]
        [ProducesResponseType(StatusCodes.Status404NotFound)]
        public async Task<ActionResult> Put(int id, AugmentationRequestModel model)
        {
            // Get the Augmentation which is to be Updated
            var augToUpdate = await augRepo.Get(id);

            // If the Augmentation was Not Found...
            if (augToUpdate == null)
            {
                // ...Return NotFound
                return NotFound();
            }
            
            // Update the Augmentation
            await augRepo.Update(augToUpdate, model);
            
            // Get the Updated Augmentation
            var updatedAug = await augRepo.Get(id, false);

            // Generate Links for the Updated Augmentation
            updatedAug!.Links = linkGenerationService.GenerateLinks(updatedAug.Id);

            // Return Ok with the Updated Aug
            return Ok(updatedAug);
        }

        /// <summary>
        /// Patches a Specific Augmentation.
        /// </summary>
        ///
        /// <remarks>
        /// Sample Request:
        /// 
        ///     PATCH Augmentations/8
        ///     [
        ///         {
        ///             "op": "replace",
        ///             "path": "/name",
        ///             "value": "Patched"
        ///         }
        ///     ]
        /// </remarks>
        /// 
        /// <param name="id"> The Id of a Specific Augmentation whose Data will be Patched. </param>
        /// <param name="patchDoc"> The Patch Document which includes the Patching Operations which will be Performed. </param>
        /// 
        /// <returns> The Newly Patched Augmentation. </returns>
        /// 
        /// <response code="200"> Returns the Newly Patched Augmentation. </response>
        /// <response code="401"> If the User Isn't Authorized. </response>
        /// <response code="404"> If the Specific Augmentation Can't be Found. </response>
        /// <response code="422"> If the Patch Request Isn't Valid. </response>
        [HttpPatch(RouteIdParameter)]
        [Consumes(ContentTypeApplicationJson)]
        [Produces(ContentTypeApplicationJson)]
        [ProducesResponseType(StatusCodes.Status200OK)]
        [ProducesResponseType(StatusCodes.Status401Unauthorized)]
        [ProducesResponseType(StatusCodes.Status404NotFound)]
        [ProducesResponseType(StatusCodes.Status422UnprocessableEntity)]
        public async Task<ActionResult> Patch(int id, JsonPatchDocument<AugmentationRequestModel> patchDoc)
        {
            // Get the Augmentation which is to be Patched
            var augToPatch = await augRepo.Get(id);

            // If the Augmentation was Not Found...
            if (augToPatch == null)
            {
                // ...Return NotFound
                return NotFound();
            }

            // Map the Augmentation to a Request Model
            var model = augToPatch.Adapt<AugmentationRequestModel>();
            
            // Apply the Patching Operations to the Request Model
            patchDoc.ApplyTo(model);
            
            // If the Request Model is Not Valid...
            if (!TryValidateModel(model))
            {
                // ...Return UnprocessableEntity with the Validation Errors
                return UnprocessableEntity(ModelState);
            }
            
            // Update the Augmentation
            await augRepo.Update(augToPatch, model);
            
            // Get the Patched Augmentation
            var patchedAug = await augRepo.Get(id, false);

            // Generate Links for the Patched Augmentation
            patchedAug!.Links = linkGenerationService.GenerateLinks(patchedAug.Id);
            
            // Return Ok with the Patched Augmentation
            return Ok(patchedAug);
        }

        /// <summary>
        /// Deletes a Specific Augmentation. 
        /// </summary>
        ///
        /// <remarks>
        /// Sample Request:
        ///
        ///     DELETE Augmentations/1
        ///     {
        ///     }
        /// </remarks>
        /// 
        /// <param name="id"> The Id of a Specific Augmentation which is to be Deleted. </param>
        /// 
        /// <returns> An Action Result of Ok. </returns>
        ///
        /// <response code="200"> Returns Ok Indicating that the Specific Augmentation has been Deleted. </response>
        /// <response code="401"> If the User Isn't Authorized. </response>
        /// <response code="404"> If the Specific Augmentation Can't be Found.  </response>
        [HttpDelete(RouteIdParameter)]
        [Consumes(ContentTypeApplicationJson)]
        [Produces(ContentTypeApplicationJson)]
        [ProducesResponseType(StatusCodes.Status200OK)]
        [ProducesResponseType(StatusCodes.Status401Unauthorized)]
        [ProducesResponseType(StatusCodes.Status404NotFound)]
        public async Task<ActionResult> Delete(int id)
        {
            // Get the Augmentation which is to be Deleted
            var augToDelete = await augRepo.Get(id);
            
            // If the Augmentation was Not Found...
            if (augToDelete == null)
            {
                // ...Return NotFound
                return NotFound();
            }
        
            // Delete the Augmentation
            await augRepo.Delete(augToDelete);
            
            // Return Ok
            return Ok();
        }

        /// <summary>
        /// Returns an Action Result of Ok With the Allowed Methods for Requests in the Header.
        /// </summary>
        ///
        /// <remarks>
        /// Sample Request:
        ///
        ///     OPTIONS Augmentations
        ///     {
        ///     }
        /// </remarks>
        /// 
        /// <returns> An Action Result of Ok With the Allowed Methods for Requests in the Header. </returns>
        ///
        /// <response code="200"> Returns Ok With the Allowed Methods for Requests in the Header. </response>
        [HttpOptions]
        [Consumes(ContentTypeApplicationJson)]
        [Produces(ContentTypeApplicationJson)]
        [ProducesResponseType(StatusCodes.Status200OK)]
        public ActionResult Options()
        {
            // Add the Allowed Methods to the Header
            Response.Headers.Add("Allow", "OPTIONS, GET, POST, PUT, PATCH, DELETE");

            // Return Ok
            return Ok();
        }

    }
}
