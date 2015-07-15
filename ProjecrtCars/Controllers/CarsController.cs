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
     public class CarsController : ApiController
     {
         private IMongoCollection<Car> Collection{get; set;}

         public CarsController() {
             var client = new MongoClient();
             var database = client.GetDatabase("Cars");
             Collection = database.GetCollection<Car>("cars");
         }

         //GET api/Cars
         public async Task<IEnumerable<Car>> Get(){
             return await Collection.Find<Car>(new BsonDocument()).ToListAsync();
         }

         //GET api/Cars/{id}
         public async Task<Car> Get(string id){
             return await Collection.Find<Car>(c => c.Id == id).SingleAsync();
         }

         //POST: api/Cars
         public async Task Post([FromBody]Car carInput){
             carInput.Id = ObjectId.GenerateNewId().ToString();
             await Collection.InsertOneAsync(carInput);
         }

         //DELETE: api/Cars/{CarId}
         public async Task<Car> Delete(string id)
         {

             return await Collection.FindOneAndDeleteAsync(c => c.Id == id);
         }


         public async Task<Car> Patch([FromBody]CarPatch carInput)
         {

             var option = new FindOneAndUpdateOptions<Car, Car> { ReturnDocument = ReturnDocument.After };
             var update = Builders<Car>.Update.Set(c => c.Id, carInput.Id);
             if (carInput.Model != null) update = update.Set(c => c.Model, carInput.Model);
             if (carInput.Company != null) update = update.Set(c => c.Company, carInput.Company);
             if (carInput.Year != null) update = update.Set(c => c.Year, carInput.Year.Value);
             if (carInput.Owners != null) update = update.Set(c => c.Owners, carInput.Owners);
             return await Collection.FindOneAndUpdateAsync<Car>(c => c.Id == carInput.Id, update, option);

         }

    
    }
}

