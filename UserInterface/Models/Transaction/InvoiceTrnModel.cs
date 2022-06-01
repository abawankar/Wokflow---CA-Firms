using DAL.Transaction;
using Domain.Implementation.Transaction;
using Domain.Interface.Transaction;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace UserInterface.Models.Transaction
{
    public class InvoiceTrnModel: Domain.Implementation.Transaction.InvoiceTrn
    {
        public int TaskId { get; set; }
    }

    public class InvoiceTrnRepository : Repository<InvoiceTrnModel>
    {

        public override InvoiceTrnModel GetById(int id)
        {
            throw new NotImplementedException();
        }

        public override IList<InvoiceTrnModel> GetAll()
        {
            throw new NotImplementedException();
        }

        public IList<InvoiceTrnModel> GetAll(int invId)
        {
            InvoiceDAL dal = new InvoiceDAL();
            AutoMapper.Mapper.CreateMap<InvoiceTrn, InvoiceTrnModel>();
            AutoMapper.Mapper.CreateMap<InvoiceTrn, InvoiceTrnModel>()
                .ForMember(dest => dest.TaskId, opt => opt.MapFrom(scr => scr.Task.Id));

            List<InvoiceTrnModel> model = AutoMapper.Mapper.Map<List<InvoiceTrnModel>>(dal.GetById(invId).InvTrn);

            return model;
        }

        public override void Edit(InvoiceTrnModel obj)
        {
            InvoiceTrnDAL dal = new InvoiceTrnDAL();
            IInvoiceTrn bl = dal.GetById(obj.Id);
            bl.Amount = obj.Amount;
            bl.Particulars = obj.Particulars;
            dal.InsertOrUpdate(bl);
        }

        public override void Insert(InvoiceTrnModel obj)
        {
            throw new NotImplementedException();
        }

        public void Insert(InvoiceTrnModel obj, int invid)
        {
            InvoiceDAL dal = new InvoiceDAL();
            IInvoice bl = dal.GetById(invid);

            IInvoiceTrn trn = new InvoiceTrn();
            trn.Particulars = obj.Particulars;
            trn.Amount = obj.Amount;

            bl.InvTrn.Add(trn);

            dal.InsertOrUpdate(bl);
        }

        public override bool Delete(int id)
        {
            InvoiceTrnDAL dal = new InvoiceTrnDAL();
            IInvoiceTrn trn = dal.GetById(id);

            return dal.Delete(trn);
        }
    }
}