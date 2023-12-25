﻿namespace HouseRentingSystem.Data.Configurations
{
	using HouseRentingSystem.Data.Models;
	using Microsoft.EntityFrameworkCore;
	using Microsoft.EntityFrameworkCore.Metadata.Builders;

	public class ApplicationUserEntityConfiguration : IEntityTypeConfiguration<ApplicationUser>
	{
		public void Configure(EntityTypeBuilder<ApplicationUser> builder)
		{
			builder.Property(u => u.FirstName)
				.HasDefaultValue("FirstName");

			builder.Property(u => u.LastName)
				.HasDefaultValue("LastName");
		}
	}
}
