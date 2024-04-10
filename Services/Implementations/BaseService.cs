using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using WebDemo.Services.Interfaces;

namespace WebDemo.Services.Implementations
{
    public class BaseService
    {
        protected readonly IUnitOfWork _unitOfWork;

        public BaseService(IUnitOfWork unitOfWork)
        {
            _unitOfWork = unitOfWork;
        }
    }
}