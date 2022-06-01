using DAL.Master;
using Domain.Implementation.Master;
using Domain.Interface.Master;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace UserInterface.Models.Master
{
    public class ConsultantModel:Domain.Implementation.Master.Consultant
    {
    }

    public class ConsultantRepository : Repository<ConsultantModel>
    {
        public override ConsultantModel GetById(int id)
        {
            ConsultantDAL dal = new ConsultantDAL();
            AutoMapper.Mapper.CreateMap<Consultant, ConsultantModel>();
            ConsultantModel model = AutoMapper.Mapper.Map<ConsultantModel>(dal.GetById(id));

            return model;
        }

        public override IList<ConsultantModel> GetAll()
        {
            ConsultantDAL dal = new ConsultantDAL();
            AutoMapper.Mapper.CreateMap<Consultant, ConsultantModel>();
            List<ConsultantModel> model = AutoMapper.Mapper.Map<List<ConsultantModel>>(dal.GetAll());

            return model;
        }

        public override void Edit(ConsultantModel obj)
        {
            ConsultantDAL dal = new ConsultantDAL();
            IConsultant bl = dal.GetById(obj.Id);
            bl.Name = obj.Name;
            bl.Address = obj.Address;
            bl.MobileNo = obj.MobileNo;
            bl.MailId = obj.MailId;
            bl.PAN = obj.PAN;
            bl.BankName = obj.BankName;
            bl.BankAccountNumber = obj.BankAccountNumber;
            bl.AccountType = obj.AccountType;
            bl.IFSCCode = obj.IFSCCode;
            dal.InsertOrUpdate(bl);
        }
        public override void Insert(ConsultantModel obj)
        {
            ConsultantDAL dal = new ConsultantDAL();
            IConsultant bl = new Consultant();
            bl.Name = obj.Name;
            bl.Address = obj.Address;
            bl.MobileNo = obj.MobileNo;
            bl.MailId = obj.MailId;
            bl.PAN = obj.PAN;
            bl.BankName = obj.BankName;
            bl.BankAccountNumber = obj.BankAccountNumber;
            bl.AccountType = obj.AccountType;
            bl.IFSCCode = obj.IFSCCode;
            dal.InsertOrUpdate(bl);
        }

        public override bool Delete(int id)
        {
            throw new NotImplementedException();
        }

        public static void AddClients(int id, string clientlist)
        {
            ConsultantDAL dal = new ConsultantDAL();
            dal.AddClients(id, clientlist);
        }

        public static void DeleteClients(int consltId, int clientid)
        {
            ConsultantDAL dal = new ConsultantDAL();
            IConsultant bl = dal.GetById(consltId);

            ClientsDAL cdal = new ClientsDAL();
            IClients client = cdal.GetById(clientid);

            int i = bl.Clients.IndexOf(client);
            bl.Clients.RemoveAt(i);
            dal.InsertOrUpdate(bl);
        }

        public static int GetByUserName(string username)
        {
            return ConsultantDAL.GetByAppLogin(username).Id;
        }

        public static string[] LoginList()
        {
            ConsultantDAL dal = new ConsultantDAL();
            return dal.GetAll().Select(x => x.MailId).ToArray();
        }

        public static IList<ConsultantModel> ConsultantOptions()
        {
            AutoMapper.Mapper.CreateMap<Consultant, ConsultantModel>();
            List<ConsultantModel> model = AutoMapper.Mapper.Map<List<ConsultantModel>>(ConsultantDAL.ConsultantOptions());
            return model;
        }
    }
}