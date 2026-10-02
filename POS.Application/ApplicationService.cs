using AutoMapper;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace POS.Application
{
    public class ApplicationService
    {
        public readonly IMapper _mapper;

        public ApplicationService(IMapper mapper)
        {
            _mapper = mapper;
        }
    }
}
