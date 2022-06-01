using DAL.Master;
using Domain.Implementation.Master;
using Domain.Interface.Master;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace UserInterface.Models.Master
{
    public class EmpRightsModel:Domain.Implementation.Master.EmpRights
    {

    }

    public class EmpRightsRepository : Repository<EmpRightsModel>
    {

        public override EmpRightsModel GetById(int id)
        {
            EmpRightsDAL dal = new EmpRightsDAL();
            AutoMapper.Mapper.CreateMap<EmpRights, EmpRightsModel>();
            EmpRightsModel model = AutoMapper.Mapper.Map<EmpRightsModel>(dal.GetById(id));

            return model;
        }

        public override IList<EmpRightsModel> GetAll()
        {
            EmpRightsDAL dal = new EmpRightsDAL();
            AutoMapper.Mapper.CreateMap<EmpRights, EmpRightsModel>();
            List<EmpRightsModel> model = AutoMapper.Mapper.Map<List<EmpRightsModel>>(dal.GetAll());

            return model;
        }

        public override void Edit(EmpRightsModel obj)
        {
            EmpRightsDAL dal = new EmpRightsDAL();
            IEmpRights bl = dal.GetById(obj.Id);
            bl.Code = obj.Code;
            bl.MnuName = obj.MnuName;
            bl.TableName = obj.TableName;
            bl.Operation = obj.Operation;
            bl.Description = obj.Description;
            dal.InsertOrUpdate(bl);
        }

        public override void Insert(EmpRightsModel obj)
        {
            EmpRightsDAL dal = new EmpRightsDAL();
            IEmpRights bl = new EmpRights();
            bl.Code = obj.Code;
            bl.MnuName = obj.MnuName;
            bl.TableName = obj.TableName;
            bl.Operation = obj.Operation;
            bl.Description = obj.Description;

            dal.InsertOrUpdate(bl);
        }

        public override bool Delete(int id)
        {
            EmpRightsDAL dal = new EmpRightsDAL();
            IEmpRights bl = dal.GetById(id);
            return dal.Delete(bl);
        }

        public static string[] RightList(string username)
        {
            return EmployeeDAL.GetByAppLogin(username).EmpRight.Select(x => x.Code).ToArray();
        }
    }
}