using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using MongoDB.Bson;
using MongoDB.Driver;
using MongoDB.Driver.Linq;
using ProjecrtCars.Models;
using System.Web.Http;
using System.Net;
using System.Net.Http;

namespace ProjecrtCars.Controllers
{

    public class OwnersController : ApiController
    {
        private IMongoCollection<Car> Collection{get; set;}
        
        public OwnersController()
        {
             var client = new MongoClient();
             var database = client.GetDatabase("Cars");
             Collection = database.GetCollection<Car>("cars");
        }
       
        // POST: api/Owners/{idCar}
        public async Task Post(string id, [FromBody]Owner ownerInput)
        {
            var update = Builders<Car>.Update.Push<Owner>("Owners", ownerInput);

            await Collection.FindOneAndUpdateAsync((car) => car.Id == id, update);
        }

        // DELETE: api/Owners/{idCar}
        public async Task Delete([FromBody]DeleteOwner owner)
        {
            var findName = Builders<Owner>.Filter.Eq<string>(o => o.Name, owner.OwnerName);

            var update = Builders<Car>.Update.PullFilter<Owner>("Owners", findName);

            await Collection.FindOneAndUpdateAsync((car) => car.Id == owner.CarId, update); 
        }

        // PATCH: api/Owners/
        public async Task<IHttpActionResult> Patch([FromBody]UpdateOwner updateOwner)
        {
            //Проверка на налачие машинки с указаным владельцем
            var query = Builders<Car>.Filter.Eq<string>(c => c.Id, updateOwner.CarId);
            var queryOwner = Builders<Owner>.Filter.Eq<string>(o => o.Name, updateOwner.Name);
            var queryCarWithOwner = Builders<Car>.Filter.ElemMatch<Owner>(c => c.Owners, queryOwner);

            var fullQuery = Builders<Car>.Filter.And(query, queryCarWithOwner);

            if (await Collection.Find<Car>(fullQuery).CountAsync() == 0)
                return BadRequest();
            //////////


            var findName = Builders<Owner>.Filter.Eq<string>(o => o.Name, updateOwner.Name);
            var updateRemove = Builders<Car>.Update.PullFilter<Owner>("Owners", findName);

            var replaceOwner = new Owner { Name = updateOwner.NewName, Period = updateOwner.NewPeriod };

            var updateAdd = Builders<Car>.Update.Push<Owner>("Owners", replaceOwner);


            await Collection.FindOneAndUpdateAsync((car) => car.Id == updateOwner.CarId, updateRemove);
            await Collection.FindOneAndUpdateAsync((car) => car.Id == updateOwner.CarId, updateAdd); 

            

            return Ok();
        }
        
    }
}
