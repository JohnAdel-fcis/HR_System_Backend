using HR_System_Backend.Model;
using HR_System_Backend.Model.Input;
using HR_System_Backend.Model.Response;
using HR_System_Backend.Repository.Interface;
using Microsoft.EntityFrameworkCore;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace HR_System_Backend.Repository.Repository
{
    public class CategoryRepository : ICategoryRepository
    {
        private HR_DBContext _context;
        public CategoryRepository(HR_DBContext context)
        {
            _context = context;
        }


        public async Task<Response<CategoryResponse>> AddCategory(CategoryInput category)
        {
            var response = new Response<CategoryResponse>();
            try
            {

                var exist = _context.Categories.Where(x => x.CategoryName == category.categoryName).FirstOrDefault();
                if (exist != null)
                {
                    response.status = false;
                    response.message = "اسم القسم موجود مسبقا";
                    return response;
                }
                var cat = new Category { CategoryName = category.categoryName };
                await _context.Categories.AddAsync(cat);
                await _context.SaveChangesAsync();
                var categoryAfterAdd = new CategoryResponse { categoryId = cat.CategoryId, categoryName = cat.CategoryName };
                response.status = false;
                response.message = "تمت اضافة القسم بنجاح";
                response.data.Add(categoryAfterAdd);
                return response;

            }
            catch (Exception)
            {
                response.status = false;
                response.message = "حدث خطأ";
                return response;
            }
        }

        public async Task<Response<CategoryResponse>> DeleteCategory(int id)
        {
            var response = new Response<CategoryResponse>();
            try
            {
                var catg = await _context.Categories.Where(x => x.CategoryId == id).FirstOrDefaultAsync();
                if (catg == null)
                {
                    response.status = false;
                    response.message = " نوع الوظيفه غير موجود";
                    return response;
                }
                _context.Categories.Remove(catg);
                await _context.SaveChangesAsync();
                response.status = true;
                response.message = "تم المسح بنجاح";
                response.data.Add(new CategoryResponse { categoryId = catg.CategoryId, categoryName = catg.CategoryName });
                return response;
            }
            catch (Exception ex)
            {
                response.status = false;
                response.message = ex.Message;
                return response;
            }
        }

        public async Task<Response<CategoryResponse>> EditCategory(CategoryResponse emp)
        {
            var response = new Response<CategoryResponse>();
            try
            {
                var catg = _context.Categories.Where(x => x.CategoryId == emp.categoryId).FirstOrDefaultAsync();
                if (catg == null)
                {
                    response.status = false;
                    response.message = " نوع الوظيفه غير موجود";
                    return response;
                }
                _context.Entry(catg).State = EntityState.Modified;
                await _context.SaveChangesAsync();
                response.status = true;
                response.message = "تم التعديل بنجاح";
                response.data.Add(emp);
                return response;
            }
            catch (Exception ex)
            {
                response.status = false;
                response.message = ex.Message;
                return response;
            }
        }

        public async Task<Response<CategoryResponse>> Get(int id)
        {
            var response = new Response<CategoryResponse>();
            try
            {
                var category = await _context.Categories.Where(x => x.CategoryId == id).Select(x => new CategoryResponse { categoryId = x.CategoryId, categoryName = x.CategoryName }).FirstOrDefaultAsync();
                if (category ==null)
                {
                    response.status = false;
                    response.message = "غير موجود";
                    return response;
                }
                response.status = true;
                response.message = "تم السحب نجاح";
                response.data.Add(category);
                return response;
            }
            catch (Exception ex)
            {
                response.status = false;
                response.message = ex.Message;
                return response;
            }
        }

        public async Task<Response<CategoryResponse>> GetAll()
        {
            var response = new Response<CategoryResponse>();
            try
            {

                var categories = await _context.Categories.Select(x => new CategoryResponse
                {
                    categoryId = x.CategoryId,
                    categoryName = x.CategoryName
                }).ToListAsync();
                if (categories == null)
                {
                    response.status = true;
                    response.message = "لا يوجد بيانات";
                    return response;
                }
                else
                {
                    response.status = true;
                    response.message = "تم سحب البيانات بنجاح";
                    response.data = categories;
                    return response;
                }

            }
            catch (Exception)
            {
                response.status = false;
                response.message = "حدث خطأ";
                return response;
            }
        }
    }
}
