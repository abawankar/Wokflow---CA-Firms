using DAL.Master;
using Domain.Implementation.Master;
using Domain.Interface.Master;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace UserInterface.Models.Master
{
    public class ClientsModel : Domain.Implementation.Master.Clients
    {
    }

    public class ClientRepository : Repository<ClientsModel>
    {

        public override ClientsModel GetById(int id)
        {
            ClientsDAL dal = new ClientsDAL();
            AutoMapper.Mapper.CreateMap<Clients, ClientsModel>();
            ClientsModel model = AutoMapper.Mapper.Map<ClientsModel>(dal.GetById(id));

            return model;
        }

        public override IList<ClientsModel> GetAll()
        {
            ClientsDAL dal = new ClientsDAL();
            AutoMapper.Mapper.CreateMap<Clients, ClientsModel>();
            List<ClientsModel> model = AutoMapper.Mapper.Map<List<ClientsModel>>(dal.GetAll());

            return model;
        }

        public override void Edit(ClientsModel obj)
        {
            ClientsDAL dal = new ClientsDAL();
            IClients bl = dal.GetById(obj.Id);
            bl.Name = obj.Name;
            bl.MobileNo = obj.MobileNo;
            bl.EmailId = obj.EmailId;
            bl.Address = obj.Address;
            bl.PAN = obj.PAN;
            bl.CIN = obj.CIN;
            bl.DateOfIncorpration = obj.DateOfIncorpration;
            bl.TAN = obj.TAN;
            bl.ServiceTaxNumber = obj.ServiceTaxNumber;
            bl.TIN = obj.TIN;
            bl.BankName = obj.BankName;
            bl.BankAccountNumber = obj.BankAccountNumber;
            bl.AccountType = obj.AccountType;
            bl.IFSCCode = obj.IFSCCode;
            dal.InsertOrUpdate(bl);
        }

        public override void Insert(ClientsModel obj)
        {
            ClientsDAL dal = new ClientsDAL();
            IClients bl = new Clients();
            bl.Name = obj.Name;
            bl.MobileNo = obj.MobileNo;
            bl.EmailId = obj.EmailId;
            bl.Address = obj.Address;
            bl.PAN = obj.PAN;
            bl.CIN = obj.CIN;
            bl.DateOfIncorpration = obj.DateOfIncorpration;
            bl.TAN = obj.TAN;
            bl.ServiceTaxNumber = obj.ServiceTaxNumber;
            bl.TIN = obj.TIN;
            bl.BankName = obj.BankName;
            bl.BankAccountNumber = obj.BankAccountNumber;
            bl.AccountType = obj.AccountType;
            bl.IFSCCode = obj.IFSCCode;
            dal.InsertOrUpdate(bl);
        }

        public override bool Delete(int id)
        {
            ClientsDAL dal = new ClientsDAL();
            IClients bl = dal.GetById(id);
            return dal.Delete(bl);
        }

        public void AddContact(ClientContactModel obj, int id)
        {
            ClientsDAL dal = new ClientsDAL();

            dal.AddContact(obj, id);
        }

        public static string[] LoginList()
        {
            ClientsDAL dal = new ClientsDAL();
            return dal.GetAll().Select(x => x.EmailId).ToArray();
        }

        public static IList<ClientsModel>ClientOptions()
        {
            AutoMapper.Mapper.CreateMap<Clients, ClientsModel>();
            List<ClientsModel> model = AutoMapper.Mapper.Map<List<ClientsModel>>(ClientsDAL.ClientOptions());
            return model;
        }

        public static int[] ClientList(string username)
        {
            return ClientsDAL.GetByAppLogin(username).Select(x => x.Id).ToArray();
        }

    }
}