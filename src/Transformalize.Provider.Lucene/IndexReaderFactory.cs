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
using Lucene.Net.Index;

namespace Transformalize.Providers.Lucene {
    public class IndexReaderFactory {
        private readonly DirectoryFactory _directoryFactory;
        private readonly IndexWriterFactory _writerFactory;

        public IndexReaderFactory(DirectoryFactory directoryFactory, IndexWriterFactory writerFactory) {
            _directoryFactory = directoryFactory;
            _writerFactory = writerFactory;
        }

        public IndexReader Create() {
            var directory = _directoryFactory.Create();
            if (IndexReader.IndexExists(directory))
                return IndexReader.Open(directory, true);

            //create an index and then open
            using (var writer = _writerFactory.Create()) {
                writer.Commit();
            }
            return IndexReader.Open(_directoryFactory.Create(), true);
        }
    }
}
