using System;
using System.Collections.Generic;
using System.Linq;
using System.Net;
using System.Net.Http;
using System.Web.Http;

using FriendsOfRabbits.API.Adapters;
using FriendsOfRabbits.API.Models;

namespace FriendsOfRabbits.API.Controllers
{
    public class PetfinderController : ApiController
    {
        [HttpGet]
        [Route("Petfinder/List")]
        public IEnumerable<Pet> List()
        {
            var adapter = new PetfinderAdapter();
            return adapter.GetPets().Select(pet => Pet.Create(pet));
        }

        [HttpGet]
        [Route("Petfinder/Pet/{id}")]
        public Pet Get(string id)
        {
            var adapter = new PetfinderAdapter();
            return Pet.Create(adapter.GetPet(id));
        }
        [HttpGet]
        [Route("Petfinder/Random")]
        public Pet Random()
        {
            var adapter = new PetfinderAdapter();
            return Pet.Create(adapter.GetRandomPet());
        }

    }
}
