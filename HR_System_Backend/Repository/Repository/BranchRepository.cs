using HR_System_Backend.Model;
using HR_System_Backend.Model.Response;
using HR_System_Backend.Repository.Interface;
using Microsoft.EntityFrameworkCore;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace HR_System_Backend.Repository.Repository
{
    public class BranchRepository : IBranchRepository
    {

        private readonly HR_DBContext _context;
        public BranchRepository(HR_DBContext context)
        {
            _context = context;
        }
        public async Task<Response<string>> AddBranch(string name)
        {
            var response = new Response<string>();
            try
            {
                var branch = await _context.Branches.Where(x => x.BranchName == name).FirstOrDefaultAsync();
                if (branch != null)
                {
                    response.status = false;
                    response.message = "اسم الفرع موجود مسبقا";
                    return response;
                }
                await _context.Branches.AddAsync(new Branch { BranchName = name });
                await _context.SaveChangesAsync();

                response.status = true;
                response.message = "تم تسجيل الفرع بنجاح";
                return response;
            }
            catch (Exception ex)
            {

                response.status = false;
                response.message = ex.Message;
                return response;
            }
        }

        public async Task<Response<string>> DeleteBranch(BranchResponse input)
        {
            var response = new Response<string>();
            try
            {
                var empRepo = new EmployeeRepository(_context);
                var branch = await _context.Branches.Where(x => x.BranchId == input.branchId).FirstOrDefaultAsync();
                if (branch == null)
                {
                    response.status = false;
                    response.message = "لا يوجد هذا الفرع";
                    return response;
                }
                var devices = await _context.Devices.Where(x => x.BranchId == input.branchId).ToListAsync();
                _context.RemoveRange(devices);
                var employees = await _context.Employees.Where(x => x.BranchId == input.branchId).ToListAsync();
                foreach (var employee in employees)
                {
                    var resp = await empRepo.DeleteEmployee(employee.Id);
                    if (!resp.status)
                    {
                        response.status = false;
                        response.message = resp.message;
                        return response;
                    }
                }
                _context.Branches.Remove(branch);
                await _context.SaveChangesAsync();
                response.status = true;
                response.message = "تم حذف الفرع و اجهزه البصمه و الموظفين في القسم من البرنامج";
                return response;
            }
            catch (Exception ex)
            {
                response.status = false;
                response.message = ex.Message;
                return response;
            }
        }

        public async Task<Response<string>> EditBranch(BranchResponse input)
        {
            var response = new Response<string>();
            try
            {
                var branch = await _context.Branches.Where(x => x.BranchId == input.branchId).FirstOrDefaultAsync();

                if (branch == null)
                {
                    response.status = false;
                    response.message = "لا يوجد هذا الفرع";
                    return response;
                }
                branch.BranchName = input.branchName;
                await _context.SaveChangesAsync();
                response.status = true;
                response.message = "تم تعديل الفرع بنجاح";
                return response;

            }
            catch (Exception ex)
            {
                response.status = false;
                response.message = ex.Message;
                return response;
            }

        }

        public async Task<Response<BranchResponse>> GetAllBranches()
        {
            var response = new Response<BranchResponse>();
            try
            {
                var branches = await _context.Branches.Select(x => new BranchResponse { branchId = x.BranchId, branchName = x.BranchName }).ToListAsync();
                if (branches.Count == 0)
                {

                    response.status = false;
                    response.message = "لا يوجد فروع";
                    return response;
                }

                response.status = true;
                response.message = "تم سحب البيانات بنجاح";
                response.data = branches;
                return response;
            }
            catch (Exception ex)
            {
                response.status = false;
                response.message = ex.Message;
                return response;
            }
        }

        public async Task<Response<DeviceResponse>> GetAllDevicesInBranch(int branchId)
        {
            var response = new Response<DeviceResponse>();
            try
            {


                var devices = await _context.Devices.Select(x => new DeviceResponse
                {
                    DeviceId = x.DeviceId,
                    DeviceIp = x.DeviceIp,
                    DevicePort = x.DevicePort,
                    deviceName = x.DeviceName,
                    branchId = x.BranchId,
                    Priority = x.Priority
                }
                ).Where(x => x.branchId == branchId).ToListAsync();

                if (devices.Count == 0)
                {
                    response.status = false;
                    response.message = "لا يوجد فروع";
                    return response;
                }

                response.status = true;
                response.message = "تم سحب البيانات بنجاح";
                response.data = devices;
                return response;
            }
            catch (Exception ex)
            {
                response.status = false;
                response.message = ex.Message;
                return response;
            }
        }

        public async Task<Response<EmployeeResponse>> GetAllEmployeesInBranch(int branchId)
        {
            var response = new Response<EmployeeResponse>();
            try
            {
                var emplyees = await _context.Employees.Select(x => new EmployeeResponse
                {
                    id = x.Id,
                    name = x.Name,
                    branchId = x.BranchId
                }).ToListAsync();
                if (emplyees.Count == 0)
                {

                    response.status = false;
                    response.message = "لا يوجد فروع";
                    return response;
                }

                response.status = true;
                response.message = "تم سحب البيانات بنجاح";
                response.data = emplyees;
                return response;
            }
            catch (Exception ex)
            {
                response.status = false;
                response.message = ex.Message;
                return response;
            }
        }

        public async Task<Response<BranchResponse>> GetBranch(int id)
        {
            var response = new Response<BranchResponse>();
            try
            {
                var branche = await _context.Branches.Select(x => new BranchResponse { branchId = x.BranchId, branchName = x.BranchName }).Where(x => x.branchId == id).FirstOrDefaultAsync();
                if (branche == null)
                {

                    response.status = false;
                    response.message = "لا يوجد فروع";
                    return response;
                }

                response.status = true;
                response.message = "تم سحب البيانات بنجاح";
                response.data.Add(branche);
                return response;
            }
            catch (Exception ex)
            {
                response.status = false;
                response.message = ex.Message;
                return response;
            }
        }
    }
}
