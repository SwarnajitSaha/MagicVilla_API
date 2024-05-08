using MagicVilla_VillaAPi.Data;
using MagicVilla_VillaAPi.Models;
using MagicVilla_VillaAPi.Models.Dto;
using Microsoft.AspNetCore.Http.HttpResults;
using Microsoft.AspNetCore.JsonPatch;
using Microsoft.AspNetCore.Mvc;

namespace MagicVilla_VillaAPi.Controllers
{
    [Route("api/VillaAPI")]
    //[Route("api/controller")]
    [ApiController] //this will notify this is a api controller
    public class VillaAPIController : ControllerBase
    {
        [HttpGet]
        public ActionResult<IEnumerable<VillaDto>> GetVillas() //ActionResult is for to add the status code in the return data
        {
            return Ok(VillaStore.VillaList);
        }

        //[HttpGet("id")]
        [HttpGet("{id:int}", Name = "GetVilla")] //this is to me more explecite to get an int value as input
        [ProducesResponseType(StatusCodes.Status200OK)]
        [ProducesResponseType(StatusCodes.Status400BadRequest)]
        [ProducesResponseType(StatusCodes.Status404NotFound)]
        //[ProducesResponseType(200, type = typeof(VillaDot))] // this type parameter is required to use ActionResult instand of ActionResult<VillaDto>
        //[ProducesResponseType(400)]
        //[ProducesResponseType(404)] //to documented the response
        public ActionResult<VillaDto> GetVilla(int id)
        {
            if (id == 0)
            {
                return BadRequest(); //this comes from ControllerBase
            }
            VillaDto dto = VillaStore.VillaList.FirstOrDefault(i => i.Id==id);

            if (dto == null)
            {
                return NotFound();
            }
            else
            {
                return Ok(dto);
            }
        }

        [HttpPost]
        [ProducesResponseType(StatusCodes.Status200OK)]
        [ProducesResponseType(StatusCodes.Status404NotFound)]
        [ProducesResponseType(StatusCodes.Status400BadRequest)]
        [ProducesResponseType(StatusCodes.Status201Created)]
        public ActionResult<VillaDto> CreateVilla([FromBody] VillaDto dto)
        {
            if (dto == null)
            {
                return BadRequest(dto);
            }
            else if (VillaStore.VillaList.FirstOrDefault(i => i.Name.ToLower()==dto.Name.ToLower())!=null)
            {
                ModelState.AddModelError("UniqueKeyName", "This villa is already exist"); //this is for Custom error message
                return BadRequest(ModelState);
            }
            else if (dto.Id > 0)
            {
                return StatusCode(StatusCodes.Status500InternalServerError);
            }
            else
            {
                dto.Id = VillaStore.VillaList.OrderByDescending(i => i.Id).FirstOrDefault().Id+1;
                VillaStore.VillaList.Add(dto);

                //return Ok(dto);
                return CreatedAtRoute("GetVilla", new { Id = dto.Id }, dto); //this is for getting a new link of newly created data, here the GetVilla is the name of the action, second one is parameter, and the third one is object
            }
        }

        [HttpDelete("{id:int}")]
        [ProducesResponseType(StatusCodes.Status404NotFound)]
        [ProducesResponseType(StatusCodes.Status400BadRequest)]
        [ProducesResponseType(StatusCodes.Status204NoContent)]
        public IActionResult DeleteVilla(int id)
        {
            if(id==0)
            {
                return BadRequest();
            }
            VillaDto villa = VillaStore.VillaList.FirstOrDefault(u=>u.Id==id);
            if(villa == null)
            {
                return NotFound();
            }
            else
            {
                VillaStore.VillaList.Remove(villa);
                return NoContent();
            }

        }

        [HttpPut("{id:int}",Name = "Update")]
        public IActionResult UpdateVilla(int id,[FromBody] VillaDto villa)
        {
            if(villa == null || id!=villa.Id) 
            {
                return BadRequest();
            }
            var vill = VillaStore.VillaList.FirstOrDefault(v => v.Id==id);
            if(vill != null)
            {
               vill.Name = villa.Name;
            }
            return NoContent();
        }
        // to use patch we need to install two nuget package
        //1 = Microsoft.AspNetCore.JsonPatch
        //2 = Microsoft.AspNetCore.Mvc.Newtonsofjeson
        //need to add newtonfojeson in porgram.cs (builder.Services.AddControllers().AddNewtonsoftJson();)
        [HttpPatch("{id:int}")]
        [ProducesResponseType(StatusCodes.Status204NoContent)]
        [ProducesResponseType(StatusCodes.Status400BadRequest)]
        public IActionResult PartialUpdateVilla(int id, JsonPatchDocument<VillaDto> patchDto)
        {
            if(patchDto == null || id == 0)
            {
                return BadRequest();
            }
            var villa = VillaStore.VillaList.FirstOrDefault(u=>u.Id == id);
            if(villa == null)
            {
                return BadRequest();
            }
            patchDto.ApplyTo(villa, ModelState);
            if(!ModelState.IsValid)
            {
                return BadRequest(ModelState);
            }
            return NoContent();

            //https://jsonpatch.com/
        }
    }
}
