using DAL.Master;
using Domain.Implementation.Master;
using Domain.Interface.Master;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace UserInterface.Models.Master
{
	public class CompanyModel :Domain.Implementation.Master.Company
	{

	}

    public class CompanyRepository : Repository<CompanyModel>
    {
        public override bool Delete(int id)
        {
            throw new NotImplementedException();
        }

        public override void Edit(CompanyModel obj)
        {
            CompanyDAL dal = new CompanyDAL();
            ICompany bl = dal.GetById(obj.Id);
            bl.Code = obj.Code;
            bl.Name = obj.Name;
            bl.Address = obj.Address;
            bl.PAN=obj.PAN;
            bl.ServiceTax=obj.ServiceTax;
            bl.Emailid=obj.Emailid;
            bl.PhoneNo=obj.PhoneNo;
            bl.CIN=obj.CIN;
            if(obj.RegDate != null)
            bl.RegDate= Convert.ToDateTime(obj.RegDate) + DateTime.Now.TimeOfDay;
            bl.RegNumber=obj.RegNumber;
            bl.WebSite=obj.WebSite;

            dal.InsertOrUpdate(bl);
        }

        public override IList<CompanyModel> GetAll()
        {
            CompanyDAL dal = new CompanyDAL();
            AutoMapper.Mapper.CreateMap<Company, CompanyModel>();
            List<CompanyModel> model = AutoMapper.Mapper.Map<List<CompanyModel>>(dal.GetAll());

            return model;
        }

        public override CompanyModel GetById(int id)
        {
            CompanyDAL dal = new CompanyDAL();
            AutoMapper.Mapper.CreateMap<Company, CompanyModel>();
            CompanyModel model = AutoMapper.Mapper.Map<CompanyModel>(dal.GetById(id));

            return model;
        }

        public override void Insert(CompanyModel obj)
        {
            CompanyDAL dal = new CompanyDAL();
            ICompany bl = new Company();
            bl.Code = obj.Code;
            bl.Name = obj.Name;
            bl.Address = obj.Address;
            bl.PAN = obj.PAN;
            bl.ServiceTax = obj.ServiceTax;
            bl.Emailid = obj.Emailid;
            bl.PhoneNo = obj.PhoneNo;
            bl.CIN = obj.CIN;
            if (obj.RegDate != null)
                bl.RegDate = Convert.ToDateTime(obj.RegDate) + DateTime.Now.TimeOfDay;
            bl.RegNumber = obj.RegNumber;
            bl.WebSite = obj.WebSite;
            dal.InsertOrUpdate(bl);
        }
    }
}