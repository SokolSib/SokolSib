using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using Animals.DTM;
using Animals.DTM.Model;

namespace Animals.Service.Controllers
{
    [Produces("application/json")]
    [Route("api/Animals")]
    public class AnimalsController : Controller
    {
        private readonly IAnimalsRepozitory _repozitory;

        public AnimalsController(IAnimalsRepozitory repozitory)
        {
            _repozitory = repozitory;
        }

        // GET: api/Animals
        [HttpGet]
        public IEnumerable<AnimalType> GetAnimals()
        {
            return _repozitory.Get();
        }

        // GET: api/Animals
        [HttpGet("color/{color}/type/{type}/regions/{regions}")]
        public IEnumerable<AnimalType> GetAnimals(string color, string type, params string[] regions)
        {
            return _repozitory.Find(regions, color, type);
        }

        // GET: api/Animals/5
        [HttpGet("{id}")]
        public async Task<IActionResult> GetAnimals([FromRoute] int id)
        {
            if (!ModelState.IsValid)
            {
                return BadRequest(ModelState);
            }

            var animal = await _repozitory.Get(id);

            if (animal == null)
            {
                return NotFound();
            }

            return Ok(animal);
        }

        // PUT: api/Animals/5
        [HttpPut("{id}")]
        public async Task<IActionResult> PutAnimals([FromRoute] int id, [FromBody] AnimalType animal)
        {
            if (!ModelState.IsValid)
            {
                return BadRequest(ModelState);
            }

            if (id != animal.Id)
            {
                return BadRequest();
            }

            _repozitory.State = EntityState.Modified;

            try
            {
                await _repozitory.Upsert(animal, true);
            }
            catch (DbUpdateConcurrencyException)
            {
                if (!_repozitory.Exists(id))
                {
                    return NotFound();
                }
                else
                {
                    throw;
                }
            }

            return NoContent();
        }

        // POST: api/Animals
        [HttpPost]
        public async Task<IActionResult> PostAnimals([FromBody] AnimalType animal)
        {
            if (!ModelState.IsValid)
            {
                return BadRequest(ModelState);
            }

            await _repozitory.Upsert(animal);

            return CreatedAtAction("GetAnimals", new { id = animal.Id }, animal);
        }

        // DELETE: api/Animals/5
        [HttpDelete("{id}")]
        public async Task<IActionResult> DeleteAnimals([FromRoute] int id)
        {
            if (!ModelState.IsValid)
            {
                return BadRequest(ModelState);
            }

            var animals = await _repozitory.Get(id);
            if (animals == null)
            {
                return NotFound();
            }

            await _repozitory.Del(id);

            return Ok(animals);
        }
    }
}