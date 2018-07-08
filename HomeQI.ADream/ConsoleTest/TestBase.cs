//using HomeQI.ADream.EntityFrameworkCore;
//using HomeQI.ADream.Models.Entities.Identity;
//using HomeQI.ADream.Services;
//using HomeQI.ADream.Services.Identity;
//using Microsoft.Extensions.DependencyInjection;
//using System;
//using System.Collections.Generic;
//using System.Text;
//using HomeQI.ADream.Services.Identity.Extensions;
//namespace ConsoleTest
//{
//    public class TestBase
//    {
//        public IServiceCollection serviceDescriptors = new ServiceCollection();
//        public IServiceProvider serviceProvider => serviceDescriptors.BuildServiceProvider();
//        public T GetService<T>()
//        {
//            return serviceProvider.GetService<T>();
//        }
//        protected ADreamDbContext ADreamDbContext => GetService<ADreamDbContext>();
//        public TestBase()
//        {
//            serviceDescriptors.AddDbContext<ADreamDbContext>();
//            serviceDescriptors.AddEntityFrameworkInMemoryDatabase();
//            serviceDescriptors.AddScoped<UserServices>();
//            serviceDescriptors.AddIdentity(o => { }).AddEntityFrameworkStores();
//        }
//    }
//}
