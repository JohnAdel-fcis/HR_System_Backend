using HR_System_Backend.Model;
using HR_System_Backend.Model.Input;
using HR_System_Backend.Model.Response;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace HR_System_Backend.Repository.Interface
{
    public interface ICategoryRepository
    {
        Task<Response<CategoryResponse>> AddCategory(CategoryInput Category);
        Task<Response<CategoryResponse>> GetAll();
        Task<Response<CategoryResponse>> Get(int id);
        Task<Response<CategoryResponse>> EditCategory(CategoryResponse category);
        Task<Response<CategoryResponse>> DeleteCategory(int id);
    }
}
