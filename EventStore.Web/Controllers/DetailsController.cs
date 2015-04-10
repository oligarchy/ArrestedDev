using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using System.Web;
using System.Web.Mvc;
using EventStore.Common;
using EventStore.Data;
using EventStore.Web.Models;
using MongoDB.Driver;
using MongoDB.Driver.Linq;

namespace EventStore.Web.Controllers
{
    public class DetailsController : Controller
    {
        public ActionResult ViewCollections()
        {
            ViewCollectionsModel model = new ViewCollectionsModel();

            var mongo = GetMongoDb();
            var results = mongo.ListCollectionsAsync().Result.ToListAsync().Result;

            foreach (var result in results)
            {
                string name = result["name"].ToString();

                if (name != "system.indexes")
                {
                    model.Collections.Add(name);
                }
            }

            return View(model);
        }

        public ActionResult ViewEntities(string CollectionName)
        {
            if (CollectionName == null)
            {
                CollectionName = "EventStore.Common.Hospital";
            }

            ViewEntitiesModel model = new ViewEntitiesModel();

            var mongo = GetMongoDb();
            IMongoCollection<Hospital> collection = mongo.GetCollection<Hospital>(CollectionName);
            var results = collection.FindAsync(x => x.Id != null);
            results.Wait();

            var docs = results.Result.ToListAsync();
            docs.Wait();

            model.Hospitals = docs.Result;
            model.CollectionName = CollectionName;

            return View(model);
        }

        public ActionResult ViewEntityDetails(string CollectionName, string EntityId)
        {
            ViewEntityDetailsModel model = new ViewEntityDetailsModel();

            model.HospitalDetails = getEntity(CollectionName, EntityId);
            model.CollectionName = CollectionName;

            return View(model);
        }

        private Hospital getEntity(string CollectionName, string EntityId)
        {
            if (CollectionName == null)
            {
                CollectionName = "EventStore.Common.Hospital";
            }

            if (EntityId == null)
            {
                EntityId = "5526dead3b433538081c8b25";
            }

            var mongo = GetMongoDb();
            IMongoCollection<Hospital> collection = mongo.GetCollection<Hospital>(CollectionName);
            var results = collection.FindAsync(x => x.Id == EntityId);
            results.Wait();

            var docs = results.Result.ToListAsync();
            docs.Wait();

            return docs.Result.FirstOrDefault();
        }

        public ActionResult History(string CollectionName, string EntityId)
        {
            ViewHistoryModel model = new ViewHistoryModel();

            Hospital current = getEntity(CollectionName, EntityId);
            DataManager<Hospital> dataManager = new DataManager<Hospital>();

            var historyItems = dataManager.GetHistory(current);

            model.HospitalId = EntityId;
            model.HospitalHistory = historyItems.ToList();
            model.Current = current;
            model.CollectionName = CollectionName;

            return View(model);
        }

        private IMongoDatabase GetMongoDb()
        {
            var client = new MongoClient();
            var db = client.GetDatabase("EventStore");
            return db;
        }
	}
}