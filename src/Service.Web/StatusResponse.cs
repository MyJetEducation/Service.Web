using Microsoft.AspNetCore.Mvc;
using Service.Core.Client.Models;

namespace Service.Web
{
	public class StatusResponse
	{
		public int Status { get; set; }

		public static IActionResult Error(int code = ResponseCode.Fail) => new OkObjectResult(
			new StatusResponse
			{
				Status = code
			});

		public static IActionResult Ok() => new OkObjectResult(
			new StatusResponse
			{
				Status = ResponseCode.Ok
			});

		public static IActionResult Result(bool isSuccess) => new OkObjectResult(
			new StatusResponse
			{
				Status = isSuccess ? ResponseCode.Ok : ResponseCode.Fail
			});

		public static IActionResult Result(CommonGrpcResponse commonGrpcResponse) => Result(commonGrpcResponse?.IsSuccess == true);
	}
}