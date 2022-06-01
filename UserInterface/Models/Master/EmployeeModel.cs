using DAL.Master;
using Domain.Implementation.Master;
using Domain.Interface.Master;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace UserInterface.Models.Master
{
    public class EmployeeModel : Domain.Implementation.Master.Employee
    {
        public int CompId { get; set; }
        public int PosiId { get; set; }
    }

    public class EmployeeRepository : Repository<EmployeeModel>
    {
        public override bool Delete(int id)
        {
            throw new NotImplementedException();
        }

        public override void Edit(EmployeeModel obj)
        {
            EmployeeDAL dal = new EmployeeDAL();
            IEmployee bl = dal.GetById(obj.Id);
            bl.Code = obj.Code;
            bl.Name = obj.Name;
            bl.Address = obj.Address;
            bl.PAN = obj.PAN;
            bl.DOB = obj.DOB;
            bl.Emailid = obj.Emailid;
            bl.MobileNo = obj.MobileNo;
            bl.Status = obj.Status;

            ICompany comp = new Company { Id = obj.CompId };
            IPositions posi = new Positions { Id = obj.PosiId };
            bl.Company = comp;
            bl.Position = posi;

            dal.InsertOrUpdate(bl);
        }

        public override IList<EmployeeModel> GetAll()
        {
            EmployeeDAL dal = new EmployeeDAL();
            AutoMapper.Mapper.CreateMap<Employee, EmployeeModel>();
            AutoMapper.Mapper.CreateMap<Employee, EmployeeModel>()
                .ForMember(dest => dest.CompId, opt => opt.MapFrom(scr => scr.Company.Id))
                .ForMember(dest => dest.PosiId, opt => opt.MapFrom(scr => scr.Position.Id));

            List<EmployeeModel> model = AutoMapper.Mapper.Map<List<EmployeeModel>>(dal.GetAll());

            return model;
        }

        public override EmployeeModel GetById(int id)
        {
            EmployeeDAL dal = new EmployeeDAL();
            AutoMapper.Mapper.CreateMap<Employee, EmployeeModel>();
            AutoMapper.Mapper.CreateMap<Employee, EmployeeModel>()
                .ForMember(dest => dest.CompId, opt => opt.MapFrom(scr => scr.Company.Id))
                .ForMember(dest => dest.PosiId, opt => opt.MapFrom(scr => scr.Position.Id));

            EmployeeModel model = AutoMapper.Mapper.Map<EmployeeModel>(dal.GetById(id));

            return model;
        }

        public override void Insert(EmployeeModel obj)
        {
            EmployeeDAL dal = new EmployeeDAL();
            IEmployee bl = new Employee();
            bl.Code = obj.Code;
            bl.Name = obj.Name;
            bl.Address = obj.Address;
            bl.PAN = obj.PAN;
            bl.DOB = obj.DOB;
            bl.Emailid = obj.Emailid;
            bl.MobileNo = obj.MobileNo;
            bl.Status = obj.Status;

            ICompany comp = new Company { Id = obj.CompId };
            IPositions posi = new Positions { Id = obj.PosiId };
            bl.Company = comp;
            bl.Position = posi;

            dal.InsertOrUpdate(bl);

        }
        
        public static void AddEmpRights(int id, string rightList)
        {
            EmployeeDAL dal = new EmployeeDAL();
            dal.AddRights(id, rightList);
        }

        public static int GetByUserName(string username)
        {
            return EmployeeDAL.GetByAppLogin(username).Id;
        }


        public static void DeleteRights(int empId, int RightId)
        {
            EmployeeDAL dal = new EmployeeDAL();
            IEmployee bl = dal.GetById(empId);

            EmpRightsDAL cdal = new EmpRightsDAL();
            IEmpRights rights = cdal.GetById(RightId);

            int i = bl.EmpRight.IndexOf(rights);
            bl.EmpRight.RemoveAt(i);
            dal.InsertOrUpdate(bl);
        }

        public static string[] LoginList()
        {
            EmployeeDAL dal = new EmployeeDAL();
            return dal.GetAll().Select(x => x.Emailid).ToArray();
        }

        public static void AddClients(int id, string clientlist)
        {
            EmployeeDAL dal = new EmployeeDAL();
            dal.AddClients(id, clientlist);
        }

        public static void DeleteClients(int empid, int clientid)
        {
            EmployeeDAL dal = new EmployeeDAL();
            IEmployee bl = dal.GetById(empid);

            ClientsDAL cdal = new ClientsDAL();
            IClients client = cdal.GetById(clientid);

            int i = bl.Client.IndexOf(client);
            bl.Client.RemoveAt(i);
            dal.InsertOrUpdate(bl);
        }

       
    }
}