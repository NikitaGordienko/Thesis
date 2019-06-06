using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Thesis.Models;

namespace Thesis.Models
{
    public abstract class Paging
    {
        public int CurrentPage { get; set; }
        public int PageCount { get; set; }
        public int PageSize { get; set; }
        public int RowCount { get; set; }

        public int FirstRowOnPage
        {
            get { return (CurrentPage - 1) * PageSize + 1; }
        }

        public int LastRowOnPage
        {
            get { return Math.Min(CurrentPage * PageSize, RowCount); }
        }

    }


    public class PagingResult<T> : Paging where T : class
    {
        public IList<T> Results { get; set; }

        public PagingResult()
        {
            Results = new List<T>();
        }

    }


    public static class PageResult
    {
        public static PagingResult<T> GetPaged<T>(this IQueryable<T> query,
                                     int page, int pageSize) where T : class
        {
            var result = new PagingResult<T>
            {
                CurrentPage = page,
                PageSize = pageSize,
                RowCount = query.Count()
            };

            var pageCount = (double)result.RowCount / pageSize;
            result.PageCount = (int)Math.Ceiling(pageCount);

            var skip = (page - 1) * pageSize;
            result.Results = query.Skip(skip).Take(pageSize).ToList();

            return result;
        }
    }




}
