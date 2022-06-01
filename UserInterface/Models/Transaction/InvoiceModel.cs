using DAL.Transaction;
using Domain.Implementation.Master;
using Domain.Implementation.Transaction;
using Domain.Interface.Master;
using Domain.Interface.Transaction;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace UserInterface.Models.Transaction
{
    public class InvoiceModel : Domain.Implementation.Transaction.Invoice
    {
        public int CompId { get; set; }
        public int ClientId { get; set; }
        public double? TotalAmount 
        {
            get 
            {
                if (InvTrn != null) {
                    if (InvTrn.Count != 0)
                        return InvTrn.Sum(x => x.Amount);
                    else
                        return 0;
                }                    
                else
                    return 0;
            }
        }
        public double? SerTaxAmount
        {
            get
            {
                return (TotalAmount * SerTax) / 100;
            }
        }

        public double? TDSAmount
        {
            get
            {
               return (TotalAmount * TDS) / 100;
            }
        }

        public double? NetAmount
        {
            get
            {
                return TotalAmount + SerTaxAmount - TDSAmount;
            }
        }
        
    }

    public class InvoiceRepository : Repository<InvoiceModel>
    {

        public override InvoiceModel GetById(int id)
        {
            InvoiceDAL dal = new InvoiceDAL();
            AutoMapper.Mapper.CreateMap<Invoice, InvoiceModel>();
            AutoMapper.Mapper.CreateMap<Invoice, InvoiceModel>()
                .ForMember(dest => dest.CompId, opt => opt.MapFrom(scr => scr.Company.Id))
              .ForMember(dest => dest.ClientId, opt => opt.MapFrom(scr => scr.Client.Id));
            InvoiceModel model = AutoMapper.Mapper.Map<InvoiceModel>(dal.GetById(id));
            return model;
        }

        public override IList<InvoiceModel> GetAll()
        {
            InvoiceDAL dal = new InvoiceDAL();
            AutoMapper.Mapper.CreateMap<Invoice, InvoiceModel>();
            AutoMapper.Mapper.CreateMap<Invoice, InvoiceModel>()
           .ForMember(dest => dest.CompId, opt => opt.MapFrom(scr => scr.Company.Id))
           .ForMember(dest => dest.ClientId, opt => opt.MapFrom(scr => scr.Client.Id));
            List<InvoiceModel> model = AutoMapper.Mapper.Map<List<InvoiceModel>>(dal.GetAll());
            return model;
        }

        public override void Edit(InvoiceModel obj)
        {
            InvoiceDAL dal = new InvoiceDAL();
            IInvoice bl = dal.GetById(obj.Id);
            bl.Date = obj.Date;
            bl.SerTax = obj.SerTax;
            bl.TDS = obj.TDS;
            bl.Status = obj.Status;
            bl.Received = obj.Received;
            IClients client = new Clients { Id = obj.ClientId };
            bl.Client = client;

            ICompany comp = new Company{Id=obj.CompId};
            bl.Company = comp;

            dal.InsertOrUpdate(bl);

            if (obj.Status == 2)// If payment staus is received
            {
                NBODAL taskdal = new NBODAL();
                foreach (var item in bl.InvTrn)
                {
                    if (item.Task != null)
                    {
                        INBO task = taskdal.GetById(item.Task.Id);
                        task.PaymentStatus = 2;
                        taskdal.InsertOrUpdate(task);
                    }
                }
            }
        }

        public override void Insert(InvoiceModel obj)
        {
            InvoiceDAL dal = new InvoiceDAL();
            IInvoice bl = new Invoice();

            string currentFY = "";
            
            if(obj.CompId == 2)
                currentFY = Convert.ToDateTime(obj.Date).ToInvoiceFY();

            if (obj.CompId == 1)
                currentFY = Convert.ToDateTime(obj.Date).ToInvoiceFullFY();

            string maxInvoice = InvoiceDAL.GetMaxInvoice(obj.CompId);
            if(maxInvoice != "")
            {
                if (obj.CompId == 2)
                {
                    if (currentFY == maxInvoice.Substring(0, 5))
                    {
                        bl.InvoiceNo = currentFY + "/" + (Convert.ToInt32(maxInvoice.Substring(6, 3)) + 1).ToString().PadLeft(3, '0');
                    }
                }
                if (obj.CompId == 1)
                {
                    if (currentFY == maxInvoice.Substring(4, 7))
                    {
                        bl.InvoiceNo = (Convert.ToInt32(maxInvoice.Substring(0, 3)) + 1).ToString().PadLeft(3, '0') + "/" + currentFY;
                    }
                }

            }
            else
            {
                if(obj.CompId== 2)
                    bl.InvoiceNo = currentFY + "/001";
                if (obj.CompId == 1)
                    bl.InvoiceNo = "001/" + currentFY;
            }


            bl.Date = obj.Date;

            IClients client = new Clients { Id = obj.ClientId };
            bl.Client = client;

            ICompany comp = new Company { Id = obj.CompId };
            bl.Company = comp;

            bl.Status = 1;
            bl.SerTax = Convert.ToDouble(System.Configuration.ConfigurationManager.AppSettings["SerTax"]);
            bl.TDS = Convert.ToDouble(System.Configuration.ConfigurationManager.AppSettings["TDS"]);

            dal.InsertOrUpdate(bl);
        }

        public override bool Delete(int id)
        {
            throw new NotImplementedException();
        }

        public static void AddTask(int id, string tasklist)
        {
            InvoiceDAL dal = new InvoiceDAL();
            dal.AddTask(id, tasklist);
        }
    }
}