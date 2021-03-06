﻿using JsonApiSerializer.JsonApi;
using JsonApiSerializer.Test.Models.Articles;
using JsonApiSerializer.Test.TestUtils;
using Newtonsoft.Json;
using Newtonsoft.Json.Linq;
using Newtonsoft.Json.Serialization;
using System;
using System.Collections;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.IO;
using System.Linq;
using System.Reflection;
using System.Text;
using System.Threading.Tasks;
using Xunit;

namespace JsonApiSerializer.Test.DeserializationTests
{
    public class DeserializationAttributeTests
    {
        [Fact]
        public void When_fields_controlled_by_jsonnet_attributes_should_respect_attributes()
        {
            var json = EmbeddedResource.Read("Data.Articles.single-item.json");

            var settings = new JsonApiSerializerSettings();
            var article = JsonConvert.DeserializeObject<ArticleWithIgnoredProperties>(
                json,
                new JsonApiSerializerSettings());

            Assert.Equal("1", article.Id);
            Assert.Equal(null, article.Title); //ignored

            var author = article.Author;
            Assert.Equal("9", author.Id);
            Assert.Equal("Dan", author.FirstName);
            Assert.Equal("Gebhardt", author.LastName);
            Assert.Equal("dgeb", author.Twitter);

            Assert.Equal(null, article.Comments); //ignored
        }

        [Fact]
        public void When_fields_are_only_getters_should_ignore()
        {
            var json = EmbeddedResource.Read("Data.Articles.single-item.json");

            var settings = new JsonApiSerializerSettings();
            var article = JsonConvert.DeserializeObject<ArticleWithGetters>(
                json,
                new JsonApiSerializerSettings());

            Assert.Equal("1", article.Id);
            Assert.Equal(null, article.Title); //ignored

            var author = article.Author;
            Assert.Equal("9", author.Id);
            Assert.Equal("Dan", author.FirstName);
            Assert.Equal("Gebhardt", author.LastName);
            Assert.Equal("dgeb", author.Twitter);

            Assert.Equal(null, article.Comments); //ignored
        }

    }
}
