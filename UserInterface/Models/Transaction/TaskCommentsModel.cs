using DAL.Transaction;
using Domain.Implementation.Transaction;
using Domain.Interface.Transaction;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace UserInterface.Models.Transaction
{
    public class TaskCommentsModel:Domain.Implementation.Transaction.TaskComments
    {
    }

    public class TaskCommentsRepository : Repository<TaskCommentsModel>
    {

        public override TaskCommentsModel GetById(int id)
        {
            TaskCommentsDAL dal = new TaskCommentsDAL();
            AutoMapper.Mapper.CreateMap<TaskComments, TaskCommentsModel>();
            TaskCommentsModel model = AutoMapper.Mapper.Map<TaskCommentsModel>(dal.GetById(id));

            return model;
        }

        public override IList<TaskCommentsModel> GetAll()
        {
            throw new NotImplementedException();
        }

        public IList<TaskCommentsModel> GetAll(int taskid)
        {
            TaskManagerDAL dal = new TaskManagerDAL();
            AutoMapper.Mapper.CreateMap<TaskComments, TaskCommentsModel>();
            List<TaskCommentsModel> model = AutoMapper.Mapper.Map<List<TaskCommentsModel>>(dal.GetById(taskid).Comments);
            return model;
        }

        public override void Edit(TaskCommentsModel obj)
        {
            TaskCommentsDAL dal = new TaskCommentsDAL();
            ITaskComments bl = dal.GetById(obj.Id);
            bl.Comments = obj.Comments;
            dal.InsertOrUpdate(bl);
        }

        public override void Insert(TaskCommentsModel obj)
        {
            throw new NotImplementedException();
        }

        public void Insert(TaskCommentsModel obj, int taskid)
        {
            TaskManagerDAL dal = new TaskManagerDAL();
            ITaskManager task = dal.GetById(taskid);

            ITaskComments bl = new TaskComments();
            bl.Comments = obj.Comments;
            bl.Date = obj.Date;
            bl.UserName = obj.UserName;

            task.Comments.Add(bl);

            dal.InsertOrUpdate(task);
        }
        public override bool Delete(int id)
        {
            TaskCommentsDAL dal = new TaskCommentsDAL();
            ITaskComments bl = dal.GetById(id);
            return dal.Delete(bl);
        }
    }
}