using DAL.Master;
using Domain.Implementation.Master;
using Domain.Interface.Master;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace UserInterface.Models.Master
{
    public class FileTypesModel:Domain.Implementation.Master.FileTypes
    {
    }

    public class FileTypesRepository : Repository<FileTypesModel>
    {
        public override bool Delete(int id)
        {
            FileTypesDAL dal = new FileTypesDAL();
            IFileTypes bl = dal.GetById(id);

            return dal.Delete(bl);
           
        }

        public override void Edit(FileTypesModel obj)
        {
            FileTypesDAL dal = new FileTypesDAL();
            IFileTypes bl = dal.GetById(obj.Id);
            bl.Name = obj.Name;
            dal.InsertOrUpdate(bl);
        }

        public override IList<FileTypesModel> GetAll()
        {
            FileTypesDAL dal = new FileTypesDAL();
            AutoMapper.Mapper.CreateMap<FileTypes, FileTypesModel>();
            List<FileTypesModel> model = AutoMapper.Mapper.Map<List<FileTypesModel>>(dal.GetAll());

            return model;
        }

        public override FileTypesModel GetById(int id)
        {
            FileTypesDAL dal = new FileTypesDAL();
            AutoMapper.Mapper.CreateMap<FileTypes, FileTypesModel>();
            FileTypesModel model = AutoMapper.Mapper.Map<FileTypesModel>(dal.GetById(id));

            return model;
        }

        public override void Insert(FileTypesModel obj)
        {
            FileTypesDAL dal = new FileTypesDAL();
            IFileTypes bl = new FileTypes();
            bl.Name = obj.Name;
            dal.InsertOrUpdate(bl);
        }
    }
}