using WorkVista.API.ModelLayer.Common;
using WorkVista.API.ModelLayer.WorkVista.MasterShift;
using WorkVista.API.RepositoryLayer.WorkVista.MasterShift;

namespace WorkVista.API.ServiceLayer.WorkVista.MasterShift
{
    public class MasterShiftService : IMasterShiftService
    {
        private readonly IMasterShiftRepository _masterShiftRepository;

        public MasterShiftService(IMasterShiftRepository masterShiftRepository)
        {
            _masterShiftRepository = masterShiftRepository;
        }

        public List<MasterShiftResponseModel> GetAll(out string errorMessage)
        {
            return _masterShiftRepository.GetAll(out errorMessage);
        }

        public MasterShiftResponseModel GetById(int id, out string errorMessage)
        {
            return _masterShiftRepository.GetById(id, out errorMessage);
        }

        public int AddOrUpdate(MasterShiftRequestModel request, out string errorMessage)
        { 
            return _masterShiftRepository.AddOrUpdate(request, out errorMessage);
        }

        public int Delete(DeleteRequest deleteRequest, out string errorMessage)
        {
            return _masterShiftRepository.Delete(deleteRequest, out errorMessage);
        }
    }
}
