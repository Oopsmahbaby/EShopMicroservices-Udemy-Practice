using Discount.Grpc.Data;
using Discount.Grpc.Models;
using Grpc.Core;
using Mapster;
using Microsoft.EntityFrameworkCore;

namespace Discount.Grpc.Services
{
	public class DiscountService(DiscountContext dbContext, ILogger<DiscountService> logger) : DiscountProtoService.DiscountProtoServiceBase
	{
		/// <summary>
		/// Get a discount by product name
		/// </summary>
		/// <param name="request"></param>
		/// <param name="context"></param>
		/// <returns></returns>
		/// <exception cref="RpcException"></exception>
		public override async Task<CouponModel> GetDiscount(GetDiscountRequest request, ServerCallContext context)
		{
			logger.LogInformation("GetDiscount called with ProductName: {ProductName}", request.ProductName);
			if (string.IsNullOrEmpty(request.ProductName))
			{
				logger.LogWarning("GetDiscount called with empty ProductName");
				throw new RpcException(new Status(StatusCode.InvalidArgument, "ProductName cannot be empty"));
			}

			var coupon = await dbContext.Coupons
				.FirstOrDefaultAsync(c => c.ProductName == request.ProductName);

			if (coupon == null)
			{
				coupon = new Coupon
				{
					ProductName = "No Discount",
					Description = "No Discount available for this product",
					Amount = 0
				};
			}

			logger.LogInformation("GetDiscount completed for ProductName: {ProductName}, Amount: {Amount}", 
				coupon.ProductName, coupon.Amount);

			var couponModel = coupon.Adapt<CouponModel>();
			return couponModel;

		}

		/// <summary>
		/// Create a new discount
		/// </summary>
		/// <param name="request"></param>
		/// <param name="context"></param>
		/// <returns></returns>
		/// <exception cref="RpcException"></exception>
		public override async Task<CouponModel> CreateDiscount(CreateDiscountRequest request, ServerCallContext context)
		{
			logger.LogInformation("CreateDiscount called with ProductName: {ProductName}", request.Coupon.ProductName);

			// Map the request to the Coupon model
			var coupon = request.Coupon.Adapt<Coupon>();

			// Validate the coupon
			if (coupon == null)
			{
				logger.LogWarning("CreateDiscount called with null Coupon");
				throw new RpcException(new Status(StatusCode.InvalidArgument, "Coupon cannot be null"));
			}

			if (coupon.Amount <= 0)
			{
				logger.LogWarning("CreateDiscount called with invalid Amount: {Amount}", coupon.Amount);
				throw new RpcException(new Status(StatusCode.InvalidArgument, "Amount must be greater than zero"));
			}

			// Add the coupon to the database and save changes
			dbContext.Coupons.Add(coupon);
			await dbContext.SaveChangesAsync();

			logger.LogInformation("CreateDiscount completed for ProductName: {ProductName}, Amount: {Amount}", 
				coupon.ProductName, coupon.Amount);

			// Map the coupon back to the CouponModel
			var couponModel = coupon.Adapt<CouponModel>();
			return couponModel;
		}

		/// <summary>
		/// Udate an existing discount
		/// </summary>
		/// <param name="request"></param>
		/// <param name="context"></param>
		/// <returns></returns>
		/// <exception cref="RpcException"></exception>
		public override async Task<CouponModel> UpdateDiscount(UpdateDiscountRequest request, ServerCallContext context)
		{
			logger.LogInformation("UpdateDiscount called with ProductName: {ProductName}", request.Coupon.ProductName);
			
			// Map the request to the Coupon model
			var coupon = request.Coupon.Adapt<Coupon>();
			
			// Validate the coupon
			if (coupon == null)
			{
				logger.LogWarning("UpdateDiscount called with null Coupon");
				throw new RpcException(new Status(StatusCode.InvalidArgument, "Coupon cannot be null"));
			}
			if (coupon.Amount <= 0)
			{
				logger.LogWarning("UpdateDiscount called with invalid Amount: {Amount}", coupon.Amount);
				throw new RpcException(new Status(StatusCode.InvalidArgument, "Amount must be greater than zero"));
			}
			
			// Update the coupon in the database and save changes
			dbContext.Coupons.Update(coupon);
			await dbContext.SaveChangesAsync();

			logger.LogInformation("UpdateDiscount completed for ProductName: {ProductName}, Amount: {Amount}", 
				coupon.ProductName, coupon.Amount);

			// Map the coupon back to the CouponModel
			var couponModel = coupon.Adapt<CouponModel>();
			return couponModel;
		}

		/// <summary>
		/// Delete an existing discount
		/// </summary>
		/// <param name="request"></param>
		/// <param name="context"></param>
		/// <returns></returns>
		/// <exception cref="RpcException"></exception>
		public override async Task<DeleteDiscountResponse> DeleteDiscount(DeleteDiscountRequest request, ServerCallContext context)
		{
			logger.LogInformation("DeleteDiscount called with ProductName: {ProductName}", request.ProductName);

			// Validate the request
			if (string.IsNullOrEmpty(request.ProductName))
			{
				logger.LogWarning("DeleteDiscount called with empty ProductName");
				throw new RpcException(new Status(StatusCode.InvalidArgument, "ProductName cannot be empty"));
			}

			// Find the coupon by ProductName
			var coupon = await dbContext.Coupons.FirstOrDefaultAsync(c => c.ProductName == request.ProductName);

			// If the coupon does not exist, log a warning and throw an exception
			if (coupon == null)
			{
				logger.LogWarning("DeleteDiscount called for non-existing ProductName: {ProductName}", request.ProductName);
				throw new RpcException(new Status(StatusCode.NotFound, "Coupon not found"));
			}
			dbContext.Coupons.Remove(coupon);
			await dbContext.SaveChangesAsync();

			logger.LogInformation("DeleteDiscount completed for ProductName: {ProductName}", request.ProductName);
			return new DeleteDiscountResponse { Success = true };
		}
	}
}
