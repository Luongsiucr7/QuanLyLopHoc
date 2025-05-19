using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace DLL.DTO
{
    public class DoTuoiHopLeAttribute : ValidationAttribute
    {
        private readonly int _tuoiToiThieu;
        private readonly int _tuoiToiDa;

        public DoTuoiHopLeAttribute(int tuoiToiThieu, int tuoiToiDa)
        {
            _tuoiToiThieu = tuoiToiThieu;
            _tuoiToiDa = tuoiToiDa;
        }
        protected override ValidationResult IsValid(object value, ValidationContext validationContext)
        {
            if (value is DateTime ngaySinh)
            {
                var today = DateTime.Today;
                var age = today.Year - ngaySinh.Year;
                if (ngaySinh.Date > today.AddYears(-age)) age--;

                if (age < _tuoiToiThieu || age > _tuoiToiDa)
                {
                    return new ValidationResult($"Độ tuổi phải từ {_tuoiToiThieu} đến {_tuoiToiDa}.");
                }
            }

            return ValidationResult.Success;
        }
    }
}

