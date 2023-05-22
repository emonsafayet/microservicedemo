using CoreApiResponse;
using Discount.API.Models;
using Discount.API.Repository;
using Microsoft.AspNetCore.Mvc;
using System.Net;

namespace Discount.API.Controllers
{
    [Route("api/[controller]/[action]")]
    [ApiController]
    public class DiscountController : BaseController 
    {
        ICouponRepository _cuponRepository;

        public DiscountController(ICouponRepository couponRepository)
        {
            _cuponRepository = couponRepository;
        }

        [HttpGet]
        [ProducesResponseType(typeof(Coupon),(int)HttpStatusCode.OK)]
        public async Task<IActionResult> GetDiscount(string productId)
        {
            try
            {
                var cupon = await _cuponRepository.GetDiscount(productId);
                return CustomResult(cupon);
            }
            catch (Exception ex)
            {
                return CustomResult(ex.Message,HttpStatusCode.BadRequest);
             }
        }

        [HttpPost]
        [ProducesResponseType(typeof(Coupon), (int)HttpStatusCode.OK)]
        public async Task<IActionResult> CreateDiscount([FromBody] Coupon coupon)
        {
            try
            {
                var isSaved = await _cuponRepository.CreateDiscount(coupon);
                if(isSaved)
                {
                    return CustomResult("Coupon has been creared",coupon);
                }
                return CustomResult("Coupon saved failed.", coupon,HttpStatusCode.BadRequest);
             
            }
            catch (Exception ex)
            {
                return CustomResult(ex.Message,  HttpStatusCode.BadRequest);
            }
        }

        [HttpPut]
        [ProducesResponseType(typeof(Coupon), (int)HttpStatusCode.OK)]
        public async Task<IActionResult> UpdateDiscount([FromBody] Coupon coupon)
        {
            try
            {
                var isSaved = await _cuponRepository.UpdateDiscount(coupon);
                if(isSaved)
                {
                    return CustomResult("Coupon has been modified",coupon);
                }
                return CustomResult("Coupon modified failed.", coupon,HttpStatusCode.BadRequest);
             
            }
            catch (Exception ex)
            {
                return CustomResult(ex.Message,  HttpStatusCode.BadRequest);
            }
        }

        [HttpDelete]
        [ProducesResponseType(typeof(void), (int)HttpStatusCode.OK)]
        public async Task<IActionResult> DeleteDiscount(string productId)
        {
            try
            {
                var isDeleted = await _cuponRepository.DeleteDiscount(productId);
                if(isDeleted)
                {
                    return CustomResult("Coupon has been deleted");
                }
                return CustomResult("Coupon deleted failed.",HttpStatusCode.BadRequest);
             
            }
            catch (Exception ex)
            {
                return CustomResult(ex.Message,  HttpStatusCode.BadRequest);
            }
        }

    }
}
