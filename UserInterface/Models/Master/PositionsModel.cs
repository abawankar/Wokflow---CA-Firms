using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using Domain.Implementation.Master;
using Domain.Interface.Master;
using DAL.Master;

namespace UserInterface.Models.Master
{
    public class PositionsModel :Domain.Implementation.Master.Positions
    {
    }

    public class PositionRepository : Repository<PositionsModel>
    {
        public override bool Delete(int id)
        {
            PositionsDAL dal = new PositionsDAL();
            IPositions bl = dal.GetById(id);

            return dal.Delete(bl);
        }

        public override void Edit(PositionsModel obj)
        {
            PositionsDAL dal = new PositionsDAL();
            IPositions bl = dal.GetById(obj.Id);
            bl.Name = obj.Name;
            dal.InsertOrUpdate(bl);
        }

        public override IList<PositionsModel> GetAll()
        {
            PositionsDAL dal = new PositionsDAL();
            AutoMapper.Mapper.CreateMap<Positions, PositionsModel>();
            List<PositionsModel> model = AutoMapper.Mapper.Map<List<PositionsModel>>(dal.GetAll());

            return model;
        }

        public override PositionsModel GetById(int id)
        {
            PositionsDAL dal = new PositionsDAL();
            AutoMapper.Mapper.CreateMap<Positions, PositionsModel>();
            PositionsModel model = AutoMapper.Mapper.Map<PositionsModel>(dal.GetById(id));

            return model;
        }

        public override void Insert(PositionsModel obj)
        {
            PositionsDAL dal = new PositionsDAL();
            IPositions bl = new Positions();
            bl.Name = obj.Name;
            dal.InsertOrUpdate(bl);
        }
    }
}