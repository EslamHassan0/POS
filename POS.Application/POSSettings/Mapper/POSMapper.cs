using AutoMapper;
using POS.Application.Contract.POSSettings;
using POS.Domain.Entity.POSSettings;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace POS.Application.POSSettings.Mapper
{
    public class POSMapper : Profile
    {
        public POSMapper()
        {
            CreateMap<POSSetting, POSSettingDto>()
            .ForMember(dest => dest.LogoBase64, opt => opt.Ignore());
        }
    }
}
