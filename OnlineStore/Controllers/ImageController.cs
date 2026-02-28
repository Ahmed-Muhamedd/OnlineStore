using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using OnlineStore.Core.Interfaces;
using OnlineStore.Core.Models;

namespace OnlineStore.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class ImageController : ControllerBase
    {
        private readonly IBaseRepository<ProductImage> _IBaseRepository;

        public ImageController(IBaseRepository<ProductImage> IBaseRepository)
        {
            _IBaseRepository = IBaseRepository;
        }




    }
}
