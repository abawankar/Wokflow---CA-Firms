using DAL.Master;
using Domain.Implementation.Master;
using Domain.Interface.Master;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace UserInterface.Models.Master
{
    public class TaskTypesModel:Domain.Implementation.Master.TaskTypes
    {
    }

    public class TaskTypesRepository : Repository<TaskTypesModel>
    {
        public override bool Delete(int id)
        {
            TaskTypesDAL dal = new TaskTypesDAL();
            ITaskTypes bl = dal.GetById(id);

            return dal.Delete(bl);
           
        }

        public override void Edit(TaskTypesModel obj)
        {
            TaskTypesDAL dal = new TaskTypesDAL();
            ITaskTypes bl = dal.GetById(obj.Id);
            bl.Name = obj.Name;
            bl.Particulars = obj.Particulars;
            dal.InsertOrUpdate(bl);
        }

        public override IList<TaskTypesModel> GetAll()
        {
            TaskTypesDAL dal = new TaskTypesDAL();
            AutoMapper.Mapper.CreateMap<TaskTypes, TaskTypesModel>();
            List<TaskTypesModel> model = AutoMapper.Mapper.Map<List<TaskTypesModel>>(dal.GetAll());

            return model;
        }

        public override TaskTypesModel GetById(int id)
        {
            TaskTypesDAL dal = new TaskTypesDAL();
            AutoMapper.Mapper.CreateMap<TaskTypes, TaskTypesModel>();
            TaskTypesModel model = AutoMapper.Mapper.Map<TaskTypesModel>(dal.GetById(id));

            return model;
        }

        public override void Insert(TaskTypesModel obj)
        {
            TaskTypesDAL dal = new TaskTypesDAL();
            ITaskTypes bl = new TaskTypes();
            bl.Name = obj.Name;
            bl.Particulars = obj.Particulars;
            dal.InsertOrUpdate(bl);
        }

    }
}