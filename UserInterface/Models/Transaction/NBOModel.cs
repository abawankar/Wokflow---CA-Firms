using DAL.Transaction;
using Domain.Implementation.Master;
using Domain.Implementation.Transaction;
using Domain.Interface.Master;
using Domain.Interface.Transaction;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Net.Mail;
using System.Web;

namespace UserInterface.Models.Transaction
{
    public class NBOModel : Domain.Implementation.Transaction.NBO
    {
        public int TaskTypeId { get; set; }
        public int ClientId { get; set; }
        public string UserName { get; set; }

    }

    public class NBORepository : Repository<NBOModel>
    {
        public override bool Delete(int id)
        {
            NBODAL dal = new NBODAL();
            INBO bl = dal.GetById(id);
            return dal.Delete(bl);

        }

        public override void Edit(NBOModel obj)
        {
            NBODAL dal = new NBODAL();
            INBO bl = dal.GetById(obj.Id);
            if (obj.Received != null)
            {
                bl.Received = Convert.ToDateTime(obj.Received) + DateTime.Now.TimeOfDay;
                bl.FY = Convert.ToDateTime(obj.Received).ToFinancialYear();
            }
                
            if(obj.DueDate != null)
                bl.DueDate = Convert.ToDateTime(obj.DueDate) + DateTime.Now.TimeOfDay;

            bl.Status = obj.Status;
            bl.Cost = obj.Cost;

            if(obj.Completed != null)
            {
                bl.Completed = Convert.ToDateTime(obj.Completed) + DateTime.Now.TimeOfDay;
                bl.Status = 2;
            }

            ITaskTypes file = new TaskTypes { Id = obj.TaskTypeId };
            IClients client = new Clients { Id = obj.ClientId };

            bl.TaskTypes = file;
            bl.Client = client;

            bl.Comments = obj.Comments;

            dal.InsertOrUpdate(bl);
        }

        public void EditForAccountant(NBOModel obj)
        {
            NBODAL dal = new NBODAL();
            INBO bl = dal.GetById(obj.Id);
          
            bl.FilingStatus = obj.FilingStatus;
            bl.PaymentStatus = obj.PaymentStatus;
            bl.BillStatus = obj.BillStatus;
            bl.Comments = obj.Comments;
            bl.OfficeFileNo = obj.OfficeFileNo;

            if (obj.FilingStatus == 2 && obj.PaymentStatus == 2 && obj.BillStatus == 2)
                bl.Status = 4;

            dal.InsertOrUpdate(bl);
        }

        public override IList<NBOModel> GetAll()
        {
            NBODAL dal = new NBODAL();
            AutoMapper.Mapper.CreateMap<NBO, NBOModel>();
            AutoMapper.Mapper.CreateMap<NBO, NBOModel>()
                .ForMember(dest => dest.ClientId, opt => opt.MapFrom(scr => scr.Client.Id))
                .ForMember(dest => dest.TaskTypeId, opt => opt.MapFrom(scr => scr.TaskTypes.Id));

            List<NBOModel> model = AutoMapper.Mapper.Map<List<NBOModel>>(dal.GetAll());

            return model;
        }

        public IList<NBOModel> GetCompltedTask()
        {
            NBODAL dal = new NBODAL();
            AutoMapper.Mapper.CreateMap<NBO, NBOModel>();
            AutoMapper.Mapper.CreateMap<NBO, NBOModel>()
                .ForMember(dest => dest.ClientId, opt => opt.MapFrom(scr => scr.Client.Id))
                .ForMember(dest => dest.TaskTypeId, opt => opt.MapFrom(scr => scr.TaskTypes.Id));

            List<NBOModel> model = AutoMapper.Mapper.Map<List<NBOModel>>(dal.GetCompltedTask());

            return model;
        }

        public IList<NBOModel> GetPendingTask()
        {
            NBODAL dal = new NBODAL();
            AutoMapper.Mapper.CreateMap<NBO, NBOModel>();
            AutoMapper.Mapper.CreateMap<NBO, NBOModel>()
                .ForMember(dest => dest.ClientId, opt => opt.MapFrom(scr => scr.Client.Id))
                .ForMember(dest => dest.TaskTypeId, opt => opt.MapFrom(scr => scr.TaskTypes.Id));

            List<NBOModel> model = AutoMapper.Mapper.Map<List<NBOModel>>(dal.GetPendingTask());

            return model;
        }

        public override NBOModel GetById(int id)
        {
            NBODAL dal = new NBODAL();
            AutoMapper.Mapper.CreateMap<NBO, NBOModel>();
            AutoMapper.Mapper.CreateMap<NBO, NBOModel>()
                .ForMember(dest => dest.ClientId, opt => opt.MapFrom(scr => scr.Client.Id))
                .ForMember(dest => dest.TaskTypeId, opt => opt.MapFrom(scr => scr.TaskTypes.Id));

            NBOModel model = AutoMapper.Mapper.Map<NBOModel>(dal.GetById(id));

            return model;
        }

        public override void Insert(NBOModel obj)
        {
            NBODAL dal = new NBODAL();
            INBO bl = new NBO();

            string maxFile = NBODAL.GetMaxTaskNo();
            string currentFY = Convert.ToDateTime(obj.Received).ToFY();

            if(maxFile != "" && currentFY == maxFile.Substring(0,4))
            {
                bl.FileNumber = Convert.ToInt32(maxFile) + 1;
            }
            else
            {
                bl.FileNumber = Convert.ToInt32(currentFY + "001");
                    
            }
            
            if (obj.Received != null) {
                bl.Received = Convert.ToDateTime(obj.Received) + DateTime.Now.TimeOfDay;
                bl.FY = Convert.ToDateTime(obj.Received).ToFinancialYear();
            }
            if (obj.DueDate != null)
                bl.DueDate = Convert.ToDateTime(obj.DueDate) + DateTime.Now.TimeOfDay;
            if (obj.Completed != null)
                bl.Completed = Convert.ToDateTime(obj.Completed) + DateTime.Now.TimeOfDay;
            bl.Cost = obj.Cost;

            bl.FilingStatus = 1;
            bl.PaymentStatus = 1;
            bl.BillStatus = 1;
            bl.Status = obj.Status;

            ITaskTypes file = new TaskTypes { Id = obj.TaskTypeId };
            IClients client = new Clients { Id = obj.ClientId };

            bl.TaskTypes = file;
            bl.Client = client;

            bl.Comments = obj.Comments;

            dal.InsertOrUpdate(bl);

            


        }

        public void AddDocument(NboDocumentModel obj, int id)
        {
            NBODAL dal = new NBODAL();
            dal.AddDocument(obj, id);
        }

        public IList<NBOModel> GetClientFile(int id)
        {
            NBODAL dal = new NBODAL();
            AutoMapper.Mapper.CreateMap<NBO, NBOModel>();
            AutoMapper.Mapper.CreateMap<NBO, NBOModel>()
                .ForMember(dest => dest.ClientId, opt => opt.MapFrom(scr => scr.Client.Id))
                .ForMember(dest => dest.TaskTypeId, opt => opt.MapFrom(scr => scr.TaskTypes.Id));

            List<NBOModel> model = AutoMapper.Mapper.Map<List<NBOModel>>(dal.GetClientFile(id));

            return model;
        }

    }
}