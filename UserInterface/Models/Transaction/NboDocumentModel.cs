using DAL.Transaction;
using Domain.Implementation.Transaction;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace UserInterface.Models.Transaction
{
    public class NboDocumentModel : Domain.Implementation.Transaction.NboDocument
    {
    }

    public class NboDocumentRepository : Repository<NboDocumentModel>
    {
        public override bool Delete(int id)
        {
            throw new NotImplementedException();
        }

        public override void Edit(NboDocumentModel obj)
        {
            throw new NotImplementedException();
        }

        public override IList<NboDocumentModel> GetAll()
        {
            throw new NotImplementedException();
        }

        public IList<NboDocumentModel> GetAll(int id)
        {
            NBODAL dal = new NBODAL();
            AutoMapper.Mapper.CreateMap<NboDocument, NboDocumentModel>();
            List<NboDocumentModel> model = AutoMapper.Mapper.Map<List<NboDocumentModel>>(dal.GetById(id).DocumentList);

            return model;
        }

        public override NboDocumentModel GetById(int id)
        {
            throw new NotImplementedException();
        }

        public override void Insert(NboDocumentModel obj)
        {
            throw new NotImplementedException();
        }
    }

}