using System.Threading.Tasks;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Configuration;
using WebApplication.Models;
using WebApplication.Models.DTOs;
using WebApplication.Repository;

namespace WebApplication.Controllers
{
    public class AnimalController
    {
        [Microsoft.AspNetCore.Components.Route("api/animals")]
    [ApiController]
    public class AnimalsController : ControllerBase
    {
        
        private readonly IConfiguration _configuration;

        
        private readonly AnimalRepository.IAnimalRepository _animalRepository;
        
        
        public AnimalsController(IConfiguration configuration, AnimalRepository.IAnimalRepository repository)
        {
            _animalRepository = repository;
            _configuration = configuration;
        }

        
        [HttpGet]
        public async Task<IActionResult> GetAnimals(string? orderBy = "name")
        {

            
            switch (orderBy)
            {
                
                case "name": break;
                case "description": break;
                case "category": break;
                case "area": break;
                default:
                    orderBy = "name";
                    break;
            }

            var animals = await _animalRepository.GetAll(orderBy);
            
            return Ok(animals);
        }


        [HttpPost]
        public async Task<IActionResult> AddAnimal(AddAnimal newAnimal)
        {
            if (await _animalRepository.Exist(newAnimal.ID))
            {
                return Conflict();
            }

            await _animalRepository.Create(new Animal
            {
                ID = newAnimal.ID,
                Area = newAnimal.Area,
                Category = newAnimal.Category,
                Description = newAnimal.Description,
                Name = newAnimal.Name
            });
            
            
            return Created($"/api/animals/{newAnimal.ID}", newAnimal);
        }

        [HttpPut("{id:int}")]
        public async Task<IActionResult> UpdateAnimal(string id, UpdateAnimal animal)
        {
            if (await _animalRepository.Update(id,animal))
            {
                
                return Ok(animal);
            }
            return NotFound();
        }

        [HttpDelete ("{id:int}")]
        public async Task<IActionResult> DeleteAnimal(string id)
        {
            if (await _animalRepository.Delete(id))
            {
                return Ok();
            }
            return NotFound();
        }
    }
    }
}