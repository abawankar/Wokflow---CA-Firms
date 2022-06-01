using DAL.Transaction;
using Domain.Implementation.Transaction;
using Domain.Interface.Transaction;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace UserInterface.Models.Transaction
{
    public class TaskManagerModel:Domain.Implementation.Transaction.TaskManager
    {
        public int EmpId { get; set; }
        public int NboId { get; set; }
        public int ConsltId { get; set; }
        public string Type { get; set; }
        public string Assignerlist { get; set; }
    }

    public class TaskManagerRepository : Repository<TaskManagerModel>
    {
        public override TaskManagerModel GetById(int id)
        {
            TaskManagerDAL dal = new TaskManagerDAL();
            AutoMapper.Mapper.CreateMap<TaskManager, TaskManagerModel>();
            AutoMapper.Mapper.CreateMap<TaskManager, TaskManagerModel>()
                .ForMember(dest => dest.EmpId, opt => opt.MapFrom(scr => scr.Assigneer.Id))
                .ForMember(dest => dest.NboId, opt => opt.MapFrom(scr => scr.FileNumber.Id))
                .ForMember(dest => dest.ConsltId, opt => opt.MapFrom(scr => scr.Consultant.Id));
            TaskManagerModel model = AutoMapper.Mapper.Map<TaskManagerModel>(dal.GetById(id));

            return model;
        }

        public override IList<TaskManagerModel> GetAll()
        {
            TaskManagerDAL dal = new TaskManagerDAL();
            AutoMapper.Mapper.CreateMap<TaskManager, TaskManagerModel>();
            AutoMapper.Mapper.CreateMap<TaskManager, TaskManagerModel>()
                .ForMember(dest => dest.EmpId, opt => opt.MapFrom(scr => scr.Assigneer.Id))
                .ForMember(dest => dest.NboId, opt => opt.MapFrom(scr => scr.FileNumber.Id))
                .ForMember(dest => dest.ConsltId, opt => opt.MapFrom(scr => scr.Consultant.Id));

            List<TaskManagerModel> model = AutoMapper.Mapper.Map<List<TaskManagerModel>>(dal.GetAll().ToList());

            return model;
        }

        public IList<TaskManagerModel> GetTodayTask()
        {
            TaskManagerDAL dal = new TaskManagerDAL();
            AutoMapper.Mapper.CreateMap<TaskManager, TaskManagerModel>();
            AutoMapper.Mapper.CreateMap<TaskManager, TaskManagerModel>()
                .ForMember(dest => dest.EmpId, opt => opt.MapFrom(scr => scr.Assigneer.Id))
                .ForMember(dest => dest.NboId, opt => opt.MapFrom(scr => scr.FileNumber.Id))
                .ForMember(dest => dest.ConsltId, opt => opt.MapFrom(scr => scr.Consultant.Id));

            List<TaskManagerModel> model = AutoMapper.Mapper.Map<List<TaskManagerModel>>(dal.GetTodayTask());

            return model;
        }

        public IList<TaskManagerModel> GetTodayTask(int empid)
        {
            TaskManagerDAL dal = new TaskManagerDAL();
            AutoMapper.Mapper.CreateMap<TaskManager, TaskManagerModel>();
            AutoMapper.Mapper.CreateMap<TaskManager, TaskManagerModel>()
                .ForMember(dest => dest.EmpId, opt => opt.MapFrom(scr => scr.Assigneer.Id))
                .ForMember(dest => dest.NboId, opt => opt.MapFrom(scr => scr.FileNumber.Id))
                .ForMember(dest => dest.ConsltId, opt => opt.MapFrom(scr => scr.Consultant.Id));

            List<TaskManagerModel> model = AutoMapper.Mapper.Map<List<TaskManagerModel>>(dal.GetByAssigner(empid));

            return model;
        }

        public IList<TaskManagerModel> GetByAssignee(int empid)
        {
            TaskManagerDAL dal = new TaskManagerDAL();
            AutoMapper.Mapper.CreateMap<TaskManager, TaskManagerModel>();
            AutoMapper.Mapper.CreateMap<TaskManager, TaskManagerModel>()
                .ForMember(dest => dest.EmpId, opt => opt.MapFrom(scr => scr.Assigneer.Id))
                .ForMember(dest => dest.NboId, opt => opt.MapFrom(scr => scr.FileNumber.Id))
                .ForMember(dest => dest.ConsltId, opt => opt.MapFrom(scr => scr.Consultant.Id));

            List<TaskManagerModel> model = AutoMapper.Mapper.Map<List<TaskManagerModel>>(dal.GetByAssignee(empid));

            return model;
        }

        public IList<TaskManagerModel> GetPendingTask()
        {
            TaskManagerDAL dal = new TaskManagerDAL();
            AutoMapper.Mapper.CreateMap<TaskManager, TaskManagerModel>();
            AutoMapper.Mapper.CreateMap<TaskManager, TaskManagerModel>()
                .ForMember(dest => dest.EmpId, opt => opt.MapFrom(scr => scr.Assigneer.Id))
                .ForMember(dest => dest.NboId, opt => opt.MapFrom(scr => scr.FileNumber.Id))
                .ForMember(dest => dest.ConsltId, opt => opt.MapFrom(scr => scr.Consultant.Id));

            List<TaskManagerModel> model = AutoMapper.Mapper.Map<List<TaskManagerModel>>(dal.GetPendingTask());

            return model;
        }

        public IList<TaskManagerModel> GetPendingTask(int empid)
        {
            TaskManagerDAL dal = new TaskManagerDAL();
            AutoMapper.Mapper.CreateMap<TaskManager, TaskManagerModel>();
            AutoMapper.Mapper.CreateMap<TaskManager, TaskManagerModel>()
                .ForMember(dest => dest.EmpId, opt => opt.MapFrom(scr => scr.Assigneer.Id))
                .ForMember(dest => dest.NboId, opt => opt.MapFrom(scr => scr.FileNumber.Id))
                .ForMember(dest => dest.ConsltId, opt => opt.MapFrom(scr => scr.Consultant.Id));

            List<TaskManagerModel> model = AutoMapper.Mapper.Map<List<TaskManagerModel>>(dal.GetByAssignerPending(empid));

            return model;
        }

        public override void Edit(TaskManagerModel obj)
        {
            TaskManagerDAL dal = new TaskManagerDAL();
            ITaskManager bl = dal.GetById(obj.Id);
            if(obj.Due != null)
                bl.Due = obj.Due;
            bl.Notes = obj.Notes;
            bl.Compl = bl.Compl != null ? Convert.ToDateTime(obj.Compl) + DateTime.Now.TimeOfDay : obj.Compl;
            bl.StatusPercentage = obj.StatusPercentage;
            bl.Status = obj.Status;

            dal.InsertOrUpdate(bl);

        }

        public override void Insert(TaskManagerModel obj)
        {
            throw new NotImplementedException();
        }

        public override bool Delete(int id)
        {
            throw new NotImplementedException();
        }

        public IList<TaskManagerModel> GetTaskByFileId(int fileId)
        {
            TaskManagerDAL dal = new TaskManagerDAL();
            AutoMapper.Mapper.CreateMap<TaskManager, TaskManagerModel>();
            AutoMapper.Mapper.CreateMap<TaskManager, TaskManagerModel>()
                .ForMember(dest => dest.EmpId, opt => opt.MapFrom(scr => scr.Assigneer.Id))
                .ForMember(dest => dest.NboId, opt => opt.MapFrom(scr => scr.FileNumber.Id))
                .ForMember(dest => dest.ConsltId, opt => opt.MapFrom(scr => scr.Consultant.Id));

            IList<TaskManagerModel> model = AutoMapper.Mapper.Map<List<TaskManagerModel>>(dal.GetTaskByFileId(fileId));

            return model;
        }

        public IList<TaskManagerModel> GetByConsultant(int consltId)
        {
            TaskManagerDAL dal = new TaskManagerDAL();
            AutoMapper.Mapper.CreateMap<TaskManager, TaskManagerModel>();
            AutoMapper.Mapper.CreateMap<TaskManager, TaskManagerModel>()
                .ForMember(dest => dest.EmpId, opt => opt.MapFrom(scr => scr.Assigneer.Id))
                .ForMember(dest => dest.NboId, opt => opt.MapFrom(scr => scr.FileNumber.Id))
                .ForMember(dest => dest.ConsltId, opt => opt.MapFrom(scr => scr.Consultant.Id));

            List<TaskManagerModel> model = AutoMapper.Mapper.Map<List<TaskManagerModel>>(dal.GetByConsultant(consltId));

            return model;
        }

        public IList<TaskManagerModel> GetByConsultantPending(int consltId)
        {
            TaskManagerDAL dal = new TaskManagerDAL();
            AutoMapper.Mapper.CreateMap<TaskManager, TaskManagerModel>();
            AutoMapper.Mapper.CreateMap<TaskManager, TaskManagerModel>()
                .ForMember(dest => dest.EmpId, opt => opt.MapFrom(scr => scr.Assigneer.Id))
                .ForMember(dest => dest.NboId, opt => opt.MapFrom(scr => scr.FileNumber.Id))
                .ForMember(dest => dest.ConsltId, opt => opt.MapFrom(scr => scr.Consultant.Id));

            List<TaskManagerModel> model = AutoMapper.Mapper.Map<List<TaskManagerModel>>(dal.GetByConsultantPending(consltId));

            return model;
        }

    }
}