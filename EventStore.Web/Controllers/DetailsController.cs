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
        private string DefaultEntityId = "55281c633b43353f608aceda";
        private int PageSize = 100;

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
            return ViewEntities(CollectionName, 0);
        }

        public ActionResult ViewEntities(string CollectionName, int Page)
        {
            int skip = 0;

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

            model.Hospitals = docs.Result.Skip(skip).Take(PageSize).ToList();
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
                EntityId = this.DefaultEntityId;
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
            if (CollectionName == null)
            {
                CollectionName = "EventStore.Common.Hospital";
            }

            if (EntityId == null)
            {
                EntityId = this.DefaultEntityId;
            }

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

        public ActionResult LoginHistory()
        {
            LoginHistoryModel model = new LoginHistoryModel();

            var mongo = GetMongoDb();
            IMongoCollection<Login> collection = mongo.GetCollection<Login>("EventStore.Common.Login");
            var results = collection.FindAsync(x => x.Id != null);
            results.Wait();

            var docs = results.Result.ToListAsync();
            docs.Wait();

            model.Logins = docs.Result.ToList();

            return View(model);
        }

        public ActionResult LoginDetails(string Username)
        {
            LoginDetailsModel model = new LoginDetailsModel();

            var mongo = GetMongoDb();
            IMongoCollection<Login> collection = mongo.GetCollection<Login>("EventStore.Common.Login");
            var results = collection.FindAsync(x => x.UserName == Username);
            results.Wait();

            var docs = results.Result.ToListAsync();
            docs.Wait();

            DataManager<Login> dataManager = new DataManager<Login>();
            var historyItems = dataManager.GetHistory(docs.Result.First());

            model.Logins = historyItems.ToList();
            model.Username = Username;

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