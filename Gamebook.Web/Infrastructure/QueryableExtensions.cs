//using AutoMapper.QueryableExtensions;
//using Gamebook.Web.App_Start;
//using System;
//using System.Collections.Generic;
//using System.Linq;
//using System.Linq.Expressions;
//using System.Web;

//namespace Gamebook.Web.Infrastructure
//{
//    public static class QueryableExtensions
//    {
//        public static IQueryable<TDestination> MapTo<TDestination>(this IQueryable source, params Expression<Func<TDestination, object>>[] membersToExpand)
//        {
//            return source.ProjectTo(AutoMapperConfig.Configuration, membersToExpand);
//        }
//    }
//}