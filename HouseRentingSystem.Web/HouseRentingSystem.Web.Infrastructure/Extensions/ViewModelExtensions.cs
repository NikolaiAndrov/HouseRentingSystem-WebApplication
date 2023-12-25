﻿namespace HouseRentingSystem.Web.Infrastructure.Extensions
{
	using ViewModels.Category.Interfaces;

	public static class ViewModelExtensions
	{
		public static string GetUrlInformation( this ICategoryDetailsModel model)
		{
			return model.Name.Replace(" ", "-");
		}
	}
}
