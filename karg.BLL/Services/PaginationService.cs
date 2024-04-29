﻿using karg.BLL.Interfaces;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace karg.BLL.Services
{
    public class PaginationService<T> : IPaginationService<T>
    {
        public async Task<(List<T> Items, int TotalPages)> PaginateWithTotalPages(List<T> items, int page, int pageSize)
        {
            var totalItems = items.Count;
            var totalPages = (int)Math.Ceiling((double)totalItems / pageSize);
            var paginatedItems = items
                .Skip((page - 1) * pageSize)
                .Take(pageSize)
                .ToList();

            return (paginatedItems, totalPages);
        }
    }
}
