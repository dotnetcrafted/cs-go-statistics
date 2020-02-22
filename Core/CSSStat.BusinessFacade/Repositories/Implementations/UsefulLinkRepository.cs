using System;
using System.Collections.Generic;
using CsStat.Domain.Entities;
using DataService.Interfaces;

namespace BusinessFacade.Repositories.Implementations
{
    public class UsefulLinkRepository :BaseRepository, IUsefulLinkRepository
    {
        private IMongoRepositoryFactory _mongoRepository;
        public UsefulLinkRepository(IMongoRepositoryFactory mongoRepository) : base(mongoRepository)
        {
            _mongoRepository = mongoRepository;
        }

        public void Add(UsefulInfo info)
        {
            info.PublishDate = DateTime.Now;
            base.Insert(info);
        }

        public void Update(string id, UsefulInfo newInfo)
        {
            var info = GetInfo(id);
            
            if (info == null)
            {
                return;
            }

            info.Caption = newInfo.Caption;
            info.Description = newInfo.Description;
            info.Image = newInfo.Image;
            info.PublishDate = newInfo.PublishDate;
            info.Url = newInfo.Url;
            info.Tags = newInfo.Tags;
            _mongoRepository.GetRepository<UsefulInfo>().Update(info);
        }

        public void Remove(string id)
        {
            _mongoRepository.GetRepository<UsefulInfo>().Delete(x=>x.Id == id);
        }

        public IEnumerable<UsefulInfo> GetAll()
        {
           return base.GetAll<UsefulInfo>();
        }

        public UsefulInfo GetInfo(string id)
        {
            return base.GetOne<UsefulInfo>(id);
        }
    }
}
