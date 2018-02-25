﻿#region license
// Transformalize
// Configurable Extract, Transform, and Load
// Copyright 2013-2017 Dale Newman
//  
// Licensed under the Apache License, Version 2.0 (the "License");
// you may not use this file except in compliance with the License.
// You may obtain a copy of the License at
//   
//       http://www.apache.org/licenses/LICENSE-2.0
//   
// Unless required by applicable law or agreed to in writing, software
// distributed under the License is distributed on an "AS IS" BASIS,
// WITHOUT WARRANTIES OR CONDITIONS OF ANY KIND, either express or implied.
// See the License for the specific language governing permissions and
// limitations under the License.
#endregion

using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using Elasticsearch.Net;
using Newtonsoft.Json;
using Transformalize.Context;
using Transformalize.Contracts;
using Transformalize.Extensions;

namespace Transformalize.Providers.Elasticsearch {

    public class ElasticWriter : IWrite {

        readonly OutputContext _context;
        readonly IElasticLowLevelClient _client;
        readonly string _prefix;

        public ElasticWriter(OutputContext context, IElasticLowLevelClient client) {
            _context = context;
            _client = client;
            _prefix = "{\"index\": {\"_index\": \"" + context.Connection.Index + "\", \"_type\": \"" + context.Entity.Alias.ToLower() + "\", \"_id\": \"";
        }

        public void Write(IEnumerable<IRow> rows) {
            var builder = new StringBuilder();
            var fullCount = 0;
            var batchCount = (uint)0;

            foreach (var part in rows.Partition(_context.Entity.InsertSize)) {
                foreach (var row in part) {
                    batchCount++;
                    fullCount++;
                    foreach (var field in _context.OutputFields) {

                        switch (field.Type) {
                            case "guid":
                                row[field] = ((Guid)row[field]).ToString();
                                break;
                            case "datetime":
                                row[field] = ((DateTime)row[field]).ToString("o");
                                break;
                        }
                        if (field.SearchType == "geo_point") {
                            row[field] = new Dictionary<string, string> {
                                { "text", row[field].ToString() },
                                { "location", row[field].ToString() }
                            };
                        }
                    }

                    builder.Append(_prefix);
                    foreach (var key in _context.OutputFields.Where(f => f.PrimaryKey)) {
                        builder.Append(row[key]);
                    }
                    builder.AppendLine("\"}}");
                    builder.AppendLine(JsonConvert.SerializeObject(_context.OutputFields.ToDictionary(f => f.Alias.ToLower(), f => row[f])));
                }
                var response = _client.Bulk<DynamicResponse>(builder.ToString(), nv => nv
                    .AddQueryString("refresh", @"true")
                );
                if (response.Success) {

                    var count = batchCount;
                    _context.Entity.Inserts += count;
                    _context.Debug(() => $"{count} to output");
                } else {
                    _context.Error(response.OriginalException.Message);
                }
                builder.Clear();
                batchCount = 0;
            }

            _context.Info($"{fullCount} to output");
        }
    }
}
