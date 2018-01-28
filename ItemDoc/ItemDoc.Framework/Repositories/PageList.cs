﻿using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ItemDoc.Framework.Repositories
{

    /// <summary>
    /// Paged list分页数据封装
    /// </summary>
    /// <typeparam name="T">T分页数据的实体类型</typeparam>
    [Serializable]
    public class PageList<T> : List<T>, IPageList<T>
    {

        /// <summary>
        ///  使用要分页的所有数据项、当前页索引和每页显示的记录数初始化PagedList对象
        /// </summary>
        /// <param name="allItems">要分页的所有数据项d</param>
        /// <param name="pageIndex">当前页索引</param>
        /// <param name="pageSize">每页显示的记录数</param>
        public PageList(IEnumerable<T> allItems, int pageIndex, int pageSize)
        {
            PageSize = pageSize;
            var items = allItems as IList<T> ?? allItems.ToList();
            this.TotalCount = items.Count();
            this.PageIndex = pageIndex;
            AddRange(items.Skip(StartIndex - 1).Take(pageSize));
        }


        /// <summary>
        /// 使用当前页数据项、当前页索引、每页显示的记录数和要分页的总记录数初始化PagedList对象
        /// </summary>
        /// <param name="currentPageItems">当前页数据项</param>
        /// <param name="pageIndex">当前页索引</param>
        /// <param name="pageSize">每页显示的记录数</param>
        /// <param name="totalItemCount">要分页数据的总记录数</param>
        public PageList(IEnumerable<T> currentPageItems, int pageIndex, int pageSize, int totalItemCount)
        {
            AddRange(currentPageItems);
            TotalCount = totalItemCount;
            this.PageIndex = pageIndex;
            PageSize = pageSize;
        }

        /// <summary>
        /// 使用要分页的所有数据项、当前页索引和每页显示的记录数初始化PagedList对象
        /// </summary>
        /// <param name="allItems">要分页的所有数据项</param>
        /// <param name="pageIndex">当前页索引</param>
        /// <param name="pageSize">每页显示的记录数</param>
        public PageList(IQueryable<T> allItems, int pageIndex, int pageSize)
        {
            int startIndex = (pageIndex - 1) * pageSize;
            AddRange(allItems.Skip(startIndex).Take(pageSize));
            TotalCount = allItems.Count();
            this.PageIndex = pageIndex;
            PageSize = pageSize;
        }

        /// <summary>
        /// 使用当前页数据项、当前页索引、每页显示的记录数和要分页的总记录数初始化PagedList对象
        /// </summary>
        /// <param name="currentPageItems">当前页数据项</param>
        /// <param name="pageIndex">当前页索引</param>
        /// <param name="pageSize">每页显示的记录数</param>
        /// <param name="totalItemCount">要分页数据的总记录数</param>
        public PageList(IQueryable<T> currentPageItems, int pageIndex, int pageSize, int totalItemCount)
        {
            AddRange(currentPageItems);
            TotalCount = totalItemCount;
            this.PageIndex = pageIndex;
            PageSize = pageSize;
        }

        /// <summary>
        /// 当前页索引
        /// </summary>
        public int PageIndex { get; set; }

        /// <summary>
        /// 每页显示的记录数
        /// </summary>
        public int PageSize { get; set; }

        /// <summary>
        /// 要分页的数据总数
        /// </summary>
        public int TotalCount { get; set; }

        /// <summary>
        /// 总页数
        /// </summary>
        public int TotalPages { get { return (int)Math.Ceiling(TotalCount / (double)PageSize); } }

        /// <summary>
        /// 开始记录索引
        /// </summary>
        public int StartIndex
        {
            get
            {
                return (PageIndex - 1) * PageSize + 1;
            }
        }

        /// <summary>
        /// 结束记录索引
        /// </summary>
        public int EndIndex
        {
            get { return TotalCount > PageIndex * PageSize ? PageIndex * PageSize : TotalCount; }
        }
    }
}
