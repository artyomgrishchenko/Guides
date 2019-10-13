﻿using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Guides.Data.Version1;
using PipServices3.Commons.Data;
using PipServices3.Data.Persistence;

namespace Guides.Persistence
{
    public class GuidesMemoryPersistence : IdentifiableMemoryPersistence<GuideV1, string>, IGuidesPersistence
    {
        public GuidesMemoryPersistence()
        {
            _maxPageSize = 1000;
        }

        private List<Func<GuideV1, bool>> ComposeFilter(FilterParams filter)
        {
            filter = filter ?? new FilterParams();

            var id = filter.GetAsNullableString("id");
            var name = filter.GetAsNullableString("name");
            var type = filter.GetAsNullableString("type");
            var app = filter.GetAsNullableString("app");
            var tag = filter.GetAsNullableString("tag");

            return new List<Func<GuideV1, bool>>() {
                (item) =>
                {
                    if (id != null && item.Id != id)
                        return false;
                    if (name != null && item.Name != name)
                        return false;
                    if (type != null && item.Type != type)
                        return false;
                    if (app != null && item.App != app)
                        return false;
                    if (tag != null && !item.Tags.Contains(tag))
                        return false;
                    return true;
                }
            };
        }

        public Task<DataPage<GuideV1>> GetPageByFilterAsync(string correlationId, FilterParams filter, PagingParams paging)
        {
            return base.GetPageByFilterAsync(correlationId, ComposeFilter(filter), paging);
        }
    }
}
