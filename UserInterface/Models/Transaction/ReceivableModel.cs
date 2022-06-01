using DAL.Transaction;
using Domain.Implementation.Transaction;
using Domain.Interface.Transaction;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace UserInterface.Models.Transaction
{
    public class ReceivableModel : Domain.Implementation.Transaction.Receivable
    {
        public int NboId { get; set; }
        public string ClientName { get; set; }
    }

    public class ReceivableRepository : Repository<ReceivableModel>
    {

        public override ReceivableModel GetById(int id)
        {
            ReceivableDAL dal = new ReceivableDAL();
            AutoMapper.Mapper.CreateMap<Receivable, ReceivableModel>();
            AutoMapper.Mapper.CreateMap<Receivable, ReceivableModel>()
                .ForMember(dest => dest.NboId, opt => opt.MapFrom(scr => scr.NBO.Id))
                .ForMember(dest => dest.ClientName, opt => opt.MapFrom(scr => scr.NBO.Client.Name));
            ReceivableModel model = AutoMapper.Mapper.Map<ReceivableModel>(dal.GetById(id));

            return model;
        }

        public override IList<ReceivableModel> GetAll()
        {
            ReceivableDAL dal = new ReceivableDAL();
            AutoMapper.Mapper.CreateMap<Receivable, ReceivableModel>();
            AutoMapper.Mapper.CreateMap<Receivable, ReceivableModel>()
                .ForMember(dest => dest.NboId, opt => opt.MapFrom(scr => scr.NBO.Id))
                .ForMember(dest => dest.ClientName, opt => opt.MapFrom(scr => scr.NBO.Client.Name));
            List<ReceivableModel> model = AutoMapper.Mapper.Map<List<ReceivableModel>>(dal.GetAll());

            return model;
        }

        public override void Edit(ReceivableModel obj)
        {
            ReceivableDAL dal = new ReceivableDAL();
            IReceivable bl = dal.GetById(obj.Id);
            bl.DueDate = obj.DueDate;
            bl.Amount = obj.Amount;
            bl.DepositType = obj.DepositType;
            bl.Description = obj.Description;
            bl.DateReceived = obj.DateReceived;
            bl.AmountReceived = obj.AmountReceived;
            bl.Status = obj.Status;

            dal.InsertOrUpdate(bl);
        }

        public override void Insert(ReceivableModel obj)
        {
            throw new NotImplementedException();
        }

        public override bool Delete(int id)
        {
            ReceivableDAL dal = new ReceivableDAL();
            IReceivable bl = dal.GetById(id);
            return dal.Delete(bl);
        }

        public static double Received(int nboid)
        {
            ReceivableRepository dal = new ReceivableRepository();
            var data = from m in dal.GetAll().Where(x => x.NBO.Id == nboid)
                       group m by m.NBO.Id into g
                       select new { received = g.Sum(x => x.AmountReceived) };
            return data.Select(x => x.received).SingleOrDefault();
        }

        public void InsertReceivable(ReceivableModel obj, int nboid)
        {
            ReceivableDAL dal = new ReceivableDAL();

            IReceivable bl = new Receivable();
            bl.DueDate = obj.DueDate;
            bl.Amount = obj.Amount;
            bl.DepositType = obj.DepositType;
            bl.Description = obj.Description;
            bl.DateReceived = obj.DateReceived;
            bl.AmountReceived = obj.AmountReceived;
            bl.Status = obj.Status;

            INBO nbo = new NBO { Id = nboid };
            bl.NBO = nbo;

            dal.InsertOrUpdate(bl);
        }

        public IList<ReceivableModel> GetReceivable(int nboid)
        {
            ReceivableDAL dal = new ReceivableDAL();
            AutoMapper.Mapper.CreateMap<Receivable, ReceivableModel>();
            AutoMapper.Mapper.CreateMap<Receivable, ReceivableModel>()
                .ForMember(dest => dest.NboId, opt => opt.MapFrom(scr => scr.NBO.Id))
                .ForMember(dest => dest.ClientName, opt => opt.MapFrom(scr => scr.NBO.Client.Name));
            List<ReceivableModel> model = AutoMapper.Mapper.Map<List<ReceivableModel>>(dal.GetAll().Where(x => x.NBO.Id == nboid));

            return model;
        }
    }
}