using DAL.Master;
using Domain.Implementation.Master;
using Domain.Interface.Master;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace UserInterface.Models.Master
{
    public class ClientContactModel:Domain.Implementation.Master.ClientContact
    {
    }

    public class ClientContactRepository : Repository<ClientContactModel>
    {

        public override ClientContactModel GetById(int id)
        {
            throw new NotImplementedException();
        }

        public override IList<ClientContactModel> GetAll()
        {
            throw new NotImplementedException();
        }

        public IList<ClientContactModel> GetAll(int id)
        {
            ClientsDAL dal = new ClientsDAL();
            AutoMapper.Mapper.CreateMap<ClientContact, ClientContactModel>();
            List<ClientContactModel> model = AutoMapper.Mapper.Map<List<ClientContactModel>>(dal.GetById(id).Contacts);

            return model;
        }

        public override void Edit(ClientContactModel obj)
        {
            ClientContactDAL dal = new ClientContactDAL();
            IClientContact bl = dal.GetById(obj.Id);
            bl.Name = obj.Name;
            bl.MobileNo = obj.MobileNo;
            bl.Mailid = obj.Mailid;
            bl.Comments = obj.Comments;
            bl.DIN=obj.DIN;
            bl.DOB = obj.DOB;
            bl.PAN = obj.PAN;
            dal.InsertOrUpdate(bl);
        }

        public override void Insert(ClientContactModel obj)
        {
            throw new NotImplementedException();
        }

        public override bool Delete(int id)
        {
            ClientContactDAL dal = new ClientContactDAL();
            IClientContact bl = dal.GetById(id);
            return dal.Delete(bl);
        }
    }
}