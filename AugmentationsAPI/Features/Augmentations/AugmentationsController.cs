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
        /// Returns a Paged List of all Augmentations from the Database.
        /// </summary>
        /// 
        /// <remarks>
        /// Sample Request:
        ///
        ///     GET Augmentations?pageSize=50&#38;pageNumber=1&#38;searchTerm=Typhoon
        ///     {
        ///     }
        /// 
        /// </remarks>
        ///
        /// <param name="pagingParameters"> The Parameters Used for Paging the List of the Augmentations. </param>
        /// <param name="searchParameters"> The Parameters Used for Searching the List of the Augmentations. </param>
        /// <param name="filteringParameters"> The Parameters Used for Filtering the List of the Augmentations. </param>
        /// 
        /// <returns> A Paged List of all Augmentations from the Database. </returns>
        ///
        /// <response code="200"> A Paged List of all Augmentations from the Database was Returned. </response>
        /// <response code="401"> The User is not Authorized to Perform this Action. </response>
        [HttpGet]
        [ProducesResponseType(StatusCodes.Status200OK)]
        [ProducesResponseType(StatusCodes.Status401Unauthorized)]
        [Produces(ContentTypeApplicationJson)]
        public async Task<ActionResult<IEnumerable<AugmentationResponseModel>>> GetAll([FromQuery]AugmentationRequestPagingParameters pagingParameters,
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
        [HttpGet(RouteIdParameter)]
        [ProducesResponseType(StatusCodes.Status200OK)]
        [ProducesResponseType(StatusCodes.Status404NotFound)]
        [ProducesResponseType(StatusCodes.Status401Unauthorized)]
        [Produces(ContentTypeApplicationJson)]
        public async Task<ActionResult<AugmentationResponseModel>> Get(int id)
        {
            // Attempt to Get the Matching Augmentation
            var matchingAug = await augRepo.Get(id, false);

            // If the Augmentation was Not Found...
            if (matchingAug == null)
            {
                // Return NotFound
                return NotFound();
            }
            
            // Generate HATEOAS Links for this Resource
            matchingAug.Links = linkGenerationService.GenerateLinks(matchingAug.Id);
            
            // Return Ok with the Augmentation
            return Ok(matchingAug);
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
        [Produces(ContentTypeApplicationJson)]
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
        /// <response code="404"> The Augmentation which was to be Updated couldn't be Found. </response>
        /// <response code="401"> The User is not Authorized to Perform this Action. </response>
        [HttpPut(RouteIdParameter)]
        [ProducesResponseType(StatusCodes.Status200OK)]
        [ProducesResponseType(StatusCodes.Status404NotFound)]
        [ProducesResponseType(StatusCodes.Status401Unauthorized)]
        public async Task<ActionResult> Update(int id, AugmentationRequestModel model)
        {
            // Attempt to Get the Augmentation which is to be Updated
            var augToUpdate = await augRepo.Get(id);

            // If The Augmentation was Not Found...
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
        /// Attempts to Patch an Existing Augmentation.
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
        /// 
        /// </remarks>
        /// 
        /// <param name="id"> The Id of an Existing Augmentation whose Data will be Patched. </param>
        /// <param name="patchDoc"> The Patch Document which includes the Patching Operations which will be Performed. </param>
        /// 
        /// <returns></returns>
        /// <response code="200"> The Augmentation was Patched. </response>
        /// <response code="404"> The Augmentation which was to be Patched couldn't be Found. </response>
        /// <response code="422"> The Patch Request Contained Validation Errors. </response>
        /// <response code="401"> The User is not Authorized to Perform this Action. </response>
        [HttpPatch(RouteIdParameter)]
        [ProducesResponseType(StatusCodes.Status200OK)]
        [ProducesResponseType(StatusCodes.Status404NotFound)]
        [ProducesResponseType(StatusCodes.Status422UnprocessableEntity)]
        [ProducesResponseType(StatusCodes.Status401Unauthorized)]
        public async Task<ActionResult> Patch(int id, JsonPatchDocument<AugmentationRequestModel> patchDoc)
        {
            // Attempt to Get the Augmentation which is to be Patched
            var augToPatch = await augRepo.Get(id);

            // If The Augmentation was Not Found...
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
        /// <response code="404"> The Augmentation which was to be Deleted couldn't be Found. </response>
        /// <response code="401"> The User is not Authorized to Perform this Action. </response>
        [HttpDelete(RouteIdParameter)]
        [ProducesResponseType(StatusCodes.Status200OK)]
        [ProducesResponseType(StatusCodes.Status404NotFound)]
        [ProducesResponseType(StatusCodes.Status401Unauthorized)]
        public async Task<ActionResult> Delete(int id)
        {
            // Attempt to Get the Augmentation which is to be Deleted
            var augToDelete = await augRepo.Get(id);
            
            // If The Augmentation was Not Found...
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
        /// Returns the Allowed Methods for Requests made to the API.
        /// </summary>
        /// <returns> The Allowed Methods for Requests made to the API. </returns>
        [HttpOptions]
        public ActionResult Options()
        {
            // Add the Allowed Methods to the Header
            Response.Headers.Add("Allow", "OPTIONS, GET, POST, PUT, PATCH, DELETE");

            // Return Ok
            return Ok();
        }

    }
}
