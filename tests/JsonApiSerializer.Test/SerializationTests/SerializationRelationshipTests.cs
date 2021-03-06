﻿using JsonApiSerializer.JsonApi;
using JsonApiSerializer.Test.Models.Articles;
using JsonApiSerializer.Test.TestUtils;
using Newtonsoft.Json;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Xunit;

namespace JsonApiSerializer.Test.SerializationTests
{
    public class SerializationRelationshipTests
    {
        public JsonApiSerializerSettings settings = new JsonApiSerializerSettings()
        {
            Formatting = Formatting.Indented //pretty print makes it easier to debug
        };

        [Fact]
        public void When_relationship_should_add_include()
        {
            var root = new DocumentRoot<Article>
            {
                Data = new Article
                {
                    Id = "1234",
                    Title = "My Article",
                    Author = new Person
                    {
                        Id = "333",
                        FirstName = "John",
                        LastName = "Smith",
                        Twitter = "jsmi"
                    }
                }
            };


            var json = JsonConvert.SerializeObject(root, settings);
            var expectedjson = @"{
                ""data"": {
                    ""id"": ""1234"",
                    ""type"": ""articles"",
                    ""attributes"": {
                        ""title"": ""My Article""
                    },
                    ""relationships"": {
                        ""author"": {
                            ""data"": { 
                                ""id"":""333"", 
                                ""type"":""people""
                            }
                        }
                    }
                },
                ""included"" : [
                    {
                        ""id"": ""333"",
                        ""type"": ""people"",
                        ""attributes"":{
                            ""first-name"": ""John"",
                            ""last-name"": ""Smith"",
                            ""twitter"": ""jsmi""
                        }

                    }
                ]
            }";
            Assert.Equal(json, expectedjson, JsonStringEqualityComparer.Instance);
        }

        [Fact]
        public void When_object_root_with_relationship_should_add_included()
        {
            var root = new Article
            {
                Id = "1234",
                Title = "My Article",
                Author = new Person
                {
                    Id = "333",
                    FirstName = "John",
                    LastName = "Smith",
                    Twitter = "jsmi"
                }
            };


            var json = JsonConvert.SerializeObject(root, settings);
            var expectedjson = @"{
                ""data"": {
                    ""id"": ""1234"",
                    ""type"": ""articles"",
                    ""attributes"": {
                        ""title"": ""My Article""
                    },
                    ""relationships"": {
                        ""author"": {
                            ""data"": { 
                                ""id"":""333"", 
                                ""type"":""people""
                            }
                        }
                    }
                },
                ""included"" : [
                    {
                        ""id"": ""333"",
                        ""type"": ""people"",
                        ""attributes"":{
                            ""first-name"": ""John"",
                            ""last-name"": ""Smith"",
                            ""twitter"": ""jsmi""
                        }

                    }
                ]
            }";
            Assert.Equal(expectedjson, json, JsonStringEqualityComparer.Instance);
        }

        [Fact]
        public void When_relationship_object_multiple_times_should_add_singe_included()
        {
            var person = new Person
            {
                Id = "333",
                FirstName = "John",
                LastName = "Smith",
                Twitter = "jsmi"
            };
            var root = new Article
            {
                Id = "1234",
                Title = "My Article",
                Author = person,
                Comments = new List<Comment>
                {
                    new Comment
                    {
                        Id = "c1",
                        Author = person,
                        Body = "comment 1"
                    },
                    new Comment
                    {
                        Id = "c2",
                        Author = person,
                        Body = "comment 2"
                    }
                }
            };


            var json = JsonConvert.SerializeObject(root, settings);
            var expectedjson = @"{
                ""data"": {
                    ""id"": ""1234"",
                    ""type"": ""articles"",
                    ""attributes"": {
                        ""title"": ""My Article""
                    },
                    ""relationships"": {
                        ""author"": {
                            ""data"": { 
                                ""id"":""333"", 
                                ""type"":""people""
                            }
                        },
                        ""comments"": {
                            ""data"": [
                                { 
                                    ""id"":""c1"", 
                                    ""type"":""comments""
                                },
                                { 
                                    ""id"":""c2"", 
                                    ""type"":""comments""
                                }
                            ]
                        }
                    }
                },
                ""included"" : [
                    {
                        ""id"": ""333"",
                        ""type"": ""people"",
                        ""attributes"":{
                            ""first-name"": ""John"",
                            ""last-name"": ""Smith"",
                            ""twitter"": ""jsmi""
                        }
                    },
                    {
                        ""id"": ""c1"",
                        ""type"": ""comments"",
                        ""attributes"":{
                            ""body"": ""comment 1""
                        },
                        ""relationships"": {
                            ""author"": {
                                ""data"": { 
                                    ""id"":""333"", 
                                    ""type"":""people""
                                }
                            }
                        }
                    },
                    {
                        ""id"": ""c2"",
                        ""type"": ""comments"",
                        ""attributes"":{
                            ""body"": ""comment 2""
                        },
                        ""relationships"": {
                            ""author"": {
                                ""data"": { 
                                    ""id"":""333"", 
                                    ""type"":""people""
                                }
                            }
                        }
                    }
                ]
            }";
            Assert.Equal(expectedjson, json, JsonStringEqualityComparer.Instance);
        }

        [Fact]
        public void When_explicit_relationship_should_output_metaData()
        {
            var root = new ArticleWithRelationship
            {
                Id = "1234",
                Title = "My Article",
                Author = new Relationship<Person>
                {
                    Data = new Person
                    {
                        Id = "333",
                        FirstName = "John",
                        LastName = "Smith",
                        Twitter = "jsmi"
                    },
                    Links = new Links
                    {
                        { "self" , new Link {  Href = "http://example.com/articles/1/relationships/author" } },
                        { "related", new Link {  Href = "http://example.com/articles/1/author" } }
                    },
                }
            };


            var json = JsonConvert.SerializeObject(root, settings);
            var expectedjson = @"{
                ""data"": {
                    ""id"": ""1234"",
                    ""type"": ""articles"",
                    ""attributes"": {
                        ""title"": ""My Article""
                    },
                    ""relationships"": {
                        ""author"": {
                            ""data"": { 
                                ""id"":""333"", 
                                ""type"":""people""
                            },
                            ""links"": {
                                ""self"": ""http://example.com/articles/1/relationships/author"",
                                ""related"": ""http://example.com/articles/1/author""
                            }
                        }
                    }
                },
                ""included"" : [
                    {
                        ""id"": ""333"",
                        ""type"": ""people"",
                        ""attributes"":{
                            ""first-name"": ""John"",
                            ""last-name"": ""Smith"",
                            ""twitter"": ""jsmi""
                        }

                    }
                ]
            }";
            Assert.Equal(expectedjson, json, JsonStringEqualityComparer.Instance);
        }
    }
}
