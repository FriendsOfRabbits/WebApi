using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace FriendsOfRabbits.API.Models
{
    public class Pet
    {
        public string Id {get; set;}
        public string Name { get; set;}
        public string Sex { get; set; }
        public string Description { get; set; }
        public string Breed { get; set; }
        public string Size { get; set; }
        public string Status { get; set; }
        public string Age { get; set; }
        public IEnumerable<PetPhoto> Photos{get; set;}

        public static Pet Create(Petfinder.petfinderPetRecord petFinder){
            var pet = new Pet {
                Id = petFinder.id,
                Name = petFinder.name,
                Age = petFinder.age.ToString(),
                Breed = string.Join(", ", petFinder.breeds.breed.Select( breed => breed.ToString())),
                Description = petFinder.description,
                Sex = petFinder.sex.ToString(),
                Size = petFinder.size.ToString(),
                Status = petFinder.status.ToString()
            };
            var photos = new List<PetPhoto>();

            var pfPhotos = petFinder.media.photos.GroupBy(p => p.id);
            foreach(var pfPhoto in pfPhotos)
            {
                var photo = new PetPhoto { Id = pfPhoto.Key };
                photos.Add(photo);
                foreach (var sized in pfPhoto)
                {
                    switch (sized.size)
                    {
                        case Petfinder.petPhotoTypeSize.x:
                            photo.LargeUrl = sized.Value;
                            break;
                        case Petfinder.petPhotoTypeSize.pn:
                            photo.PetnoteUrl = sized.Value;
                            break;
                        case Petfinder.petPhotoTypeSize.fpm:
                            photo.MediumUrl = sized.Value;
                            break;
                        case Petfinder.petPhotoTypeSize.t:
                            photo.ThumbnailUrl = sized.Value;
                            break;
                    }
                }
            }
            pet.Photos = photos;
            return pet;
        } 

    }
    public class PetPhoto
    {
        public string Id { get; set; }
        public string LargeUrl { get; set; }
        public string MediumUrl { get; set; }
        public string ThumbnailUrl { get; set; }
        public string PetnoteUrl {get; set;}

    }
    /*
     <!-- large (max 500x500) -->
            <xs:enumeration value="x"/>
            <!-- thumbnail (max 50 pixels high) -->
            <xs:enumeration value="t"/>
            <!-- petnote (max 300x250) -->
            <xs:enumeration value="pn"/>
            <!-- petnote thumbnail (max 60 pixels wide)-->
            <xs:enumeration value="pnt"/>
            <!-- featured pet module (max 95 pixels wide) -->
            <xs:enumeration value="fpm"/> 
   */  
}