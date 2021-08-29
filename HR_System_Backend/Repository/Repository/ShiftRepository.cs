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
    public class ShiftRepository : IShiftRepository
    {
        private readonly HR_DBContext _context;
        public ShiftRepository(HR_DBContext context)
        {
            _context = context;
        }
        public async Task<Response<ShiftResponse>> AddShift(ShiftInput shift)
        {
            var response = new Response<ShiftResponse>();
           /*  if (shift.dateTo.Value.Subtract(shift.dateFrom.Value).Days <= 0)
            {
                response.status = false;
                response.message = "يجب ان يكون تاريخ الانتهاء اكبر من تاريخ البداية";
                return response;
            } */



            var shft = new Shift
            {
                ShiftName = shift.shiftName,
                DateFrom = shift.dateFrom,
                DateTo = shift.dateTo,
                TimeFrom = TimeSpan.Parse(shift.timeFrom),
                TimeTo = TimeSpan.Parse(shift.timeTo),
                AllowCome=shift.allowCome,
                AllowLeave=shift.allowLeave
            };
            try
            {
                _context.Shifts.Add(shft);
                await _context.SaveChangesAsync();


                response.status = true;
                response.message = "تم اضافة الوردية بنجاح";
                response.data.Add(new ShiftResponse
                {
                    shiftId = shft.ShiftId,
                    shiftName = shft.ShiftName,
                    dateFrom = shft.DateFrom,
                    dateTo = shft.DateTo,
                    timeFrom = shft.TimeFrom.ToString(),
                    timeTo = shft.TimeTo.ToString(),
                    allowCome=shift.allowCome ,
                    allowLeave= shift.allowLeave 
                });

                return response;
            }
            catch (Exception ex)
            {
                response.status = false;
                response.message = ex.Message;
                return response;

            }
        }
        public async Task<Response<ShiftResponse>> DeleteShift(int id)
        {
            var response = new Response<ShiftResponse>();
            try
            {
                var shift = _context.Shifts.Where(x => x.ShiftId == id).FirstOrDefault();
                if (shift == null)
                {
                    response.status = false;
                    response.message = "هذه الوردية غير موجوده";
                    return response;
                }
                if (shift.Employees.Count > 0)
                {
                    response.status = false;
                    response.message = "يوجد في الوردية موظفين ...قم بتغير وردياتهم اولا";
                    return response;
                }
                _context.Shifts.Remove(shift);
                await _context.SaveChangesAsync();
                response.status = true;
                response.message = "تم ازالة الموظف بنجاح";
                return response;
            }
            catch (Exception ex)
            {
                response.status = false;
                response.message = ex.Message;
                return response;
            }
        }
        public async Task<Response<ShiftResponse>> EditShift(ShiftResponse shift)
        {
            var response = new Response<ShiftResponse>();
            try
            {
                var shft = _context.Shifts.Where(x => x.ShiftId == shift.shiftId).FirstOrDefault();
                if (shft == null)
                {
                    response.status = false;
                    response.message = "هذه الورديه غير موجودة";
                    return response;
                }
                /* if (shift.dateTo.Value.Subtract(shift.dateFrom.Value).Days <= 0)
                {
                    response.status = false;
                    response.message = "يجب ان يكون تاريخ الانتهاء اكبر من تاريخ البداية";
                    return response;
                } */



                var shftEdited = new Shift
                {
                    ShiftName = shift.shiftName,
                    DateFrom = shift.dateFrom,
                    DateTo = shift.dateTo,
                    TimeFrom = TimeSpan.Parse(shift.timeFrom),
                    TimeTo = TimeSpan.Parse(shift.timeTo),
                    AllowCome =shift.allowCome ,
                    AllowLeave =shift.allowLeave
                };

                _context.Entry(shft).State = EntityState.Detached;
                _context.Entry(shftEdited).State = EntityState.Modified;
                await _context.SaveChangesAsync();


                response.status = true;
                response.message = "تم تعديل الوردية بنجاح";
                response.data.Add(new ShiftResponse
                {
                    shiftId = shft.ShiftId,
                    shiftName = shft.ShiftName,
                    dateFrom = shft.DateFrom,
                    dateTo = shft.DateTo,
                    timeFrom = shft.TimeFrom.ToString(),
                    timeTo = shft.TimeTo.ToString(),
                    allowCome =shift.allowCome ,
                    allowLeave =shift.allowLeave
                });

                return response;
            }
            catch (Exception ex)
            {
                response.status = false;
                response.message = ex.Message;
                return response;

            }
        }
        public async Task<Response<ShiftResponse>> GetAllShifts()
        {
            var response = new Response<ShiftResponse>();
            try
            {
                var shifts = await _context.Shifts.Select(x => new ShiftResponse
                {
                    shiftId = x.ShiftId,
                    shiftName = x.ShiftName,
                    dateFrom = x.DateFrom,
                    dateTo = x.DateTo,
                    timeFrom = x.TimeFrom.Value.ToString(@"hh\:mm"),
                    timeTo = x.TimeTo.Value.ToString(@"hh\:mm"),
                    allowCome =x.AllowCome,
                    allowLeave=x.AllowLeave 
                }).ToListAsync();
                if (shifts.Count > 0)
                {
                    response.status = true;
                    response.message = "تم سحب البيانات بنجاح ";
                    response.data = shifts;
                    return response;
                }
                else
                {
                    response.status = true;
                    response.message = "لا يوجد بيانات";
                    return response;
                }
            }
            catch (Exception ex)
            {
                response.status = false;
                response.message = ex.Message;
                return response;
            }
        }
        public async Task<Response<ShiftResponse>> GetShift(int id)
        {
            var response = new Response<ShiftResponse>();
            try
            {
                var shift = await _context.Shifts.Where(s => s.ShiftId == id).Select(x => new ShiftResponse
                {
                    shiftId = x.ShiftId,
                    shiftName = x.ShiftName,
                    dateFrom = x.DateFrom,
                    dateTo = x.DateTo,
                    timeFrom = x.TimeFrom.Value.ToString(@"hh\:mm"),
                    timeTo = x.TimeTo.Value.ToString(@"hh\:mm"),
                    allowCome =x.AllowCome ,
                    allowLeave=x.AllowLeave
                }).FirstOrDefaultAsync();
                if (shift != null)
                {
                    response.status = true;
                    response.message = "تم سحب البيانات بنجاح ";
                    response.data.Add(shift);
                    return response;
                }
                else
                {
                    response.status = true;
                    response.message = "لا يوجد بيانات";
                    return response;
                }
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
