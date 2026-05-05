using WorkVista.API.ModelLayer.Common;
using WorkVista.API.ModelLayer.WorkVista.EmployeeShift;
using WorkVista.API.RepositoryLayer.WorkVista.EmployeeShift;

namespace WorkVista.API.ServiceLayer.WorkVista.EmployeeShift
{
    public class EmployeeShiftService : IEmployeeShiftService
    {
        private readonly IEmployeeShiftRepository _repository;

        public EmployeeShiftService(IEmployeeShiftRepository repository)
        {
            _repository = repository;
        }

        public List<EmployeeShiftResponseModel> GetAll(out string errorMessage)
        {
            return _repository.GetAll(out errorMessage);
        }

        public EmployeeShiftResponseModel GetById(int id, out string errorMessage)
        {
            return _repository.GetById(id, out errorMessage);
        }

        public int AddOrUpdate(EmployeeShiftRequestModel request, out string errorMessage)
        {
            return _repository.AddOrUpdate(request, out errorMessage);
        }

        public int Delete(DeleteRequest deleteRequest, out string errorMessage)
        {
            return _repository.Delete(deleteRequest, out errorMessage);
        }
    }
}
