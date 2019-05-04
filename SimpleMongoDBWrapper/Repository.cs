using System;
using System.Collections.Generic;
using System.Linq.Expressions;
using System.Threading.Tasks;
using Microsoft.Extensions.Configuration;
using MongoDB.Bson;
using MongoDB.Bson.Serialization;
using MongoDB.Bson.Serialization.IdGenerators;
using MongoDB.Driver;

namespace SimpleMongoDBWrapper {
    public class Repository<TModel> where TModel : BaseCollection {
        public readonly IMongoCollection<TModel> collection;

        private int itemPerPage;

        public Repository() {
            var collectionName = typeof(TModel).Name + "Collection";
            collection = DBContext.Instance.Database.GetCollection<TModel>(collectionName);
            SetItemPerPage();
        }

        private void SetItemPerPage() {
            itemPerPage = Settings.Instance.ItemsPerPage ?? 5;
        }

        #region Create
        public async Task<TModel> InsertOne(TModel model) {
            if (model.Id == null) {
                model.Id = ObjectIdGenerator.Instance.GenerateId(this.collection, model).ToString();
            }
            await this.collection.InsertOneAsync(model);
            return model;
        }
        #endregion

        #region Read
        public async Task<IList<TModel>> GetAll() {
            return await this.collection.Find<TModel>(x => true).ToListAsync();
        }

        public async Task<IList<TModel>> Find(Expression<Func<TModel, bool>> query) {
            return await this.collection.Find<TModel>(query).ToListAsync();
        }

        public async Task<TModel> FindFirst(Expression<Func<TModel, bool>> query) {
            return await this.collection.Find<TModel>(query).FirstOrDefaultAsync();
        }

        public async Task<TModel> GetById(string id) {
            isValidObjectId(id);
            return await this.collection.Find<TModel>(x => x.Id == id).FirstOrDefaultAsync();
        }

        public async Task<IList<TModel>> GetPage(int page) {
            int itemToSkip = itemPerPage * (page - 1);
            var query = this.collection.Find(x => true);
            var itemsTask = await query
                .Skip(itemToSkip)
                .Limit(itemPerPage)
                .ToListAsync();
            return itemsTask;
        }
        #endregion

        #region Update
        public Task UpdateOne(string id, TModel modelIn) {
            isValidObjectId(id);
            if (modelIn.Id == null) {
                modelIn.Id = id;
            }
            return this.collection.ReplaceOneAsync(x => x.Id == id, modelIn);
        }
        #endregion

        #region Delete
        public Task DeleteOne(TModel model) {
            isValidObjectId(model.Id);
            return this.collection.DeleteOneAsync(x => x.Id == model.Id);
        }

        public Task DeleteOne(string id) {
            isValidObjectId(id);
            return this.collection.DeleteOneAsync(x => x.Id == id);
        }
        #endregion

        #region Helpers

        private void isValidObjectId(string id) {
            ObjectId objectId = ObjectId.Parse(id);
        }
        #endregion
    }
}