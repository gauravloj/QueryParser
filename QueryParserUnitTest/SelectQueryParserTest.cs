using Microsoft.VisualStudio.TestTools.UnitTesting;
using Newtonsoft.Json;
using QueryParser;
using QueryParser.Helpers;
using QueryParser.Helpers.interfaces;
using QueryParser.Models;
using System.Collections.Generic;

namespace QueryParserUnitTest
{
    [TestClass]
    public class SelectQueryParserTest
    {
        /// <summary>
        /// Select parser to test
        /// </summary>
        static ICommandParser SelectParser;

        [ClassInitialize]
        public static void BeforeScenario(TestContext context)
        {
            SelectParser = new SelectCommandParser();
        }

        /// <summary>
        /// Testing JSON parser for only select query and display columns
        /// </summary>
        [TestMethod]
        public void JsonQueryParser_OnlySelectColumns()
        {
            string input = @"
    {
        ""query_type"": ""select"",
        ""query_parameters"": {
            ""display_columns"": [
                {
                    ""column_name"": ""SUM(st.population)"",
                    ""column_type"": ""name"",
                    ""alias"": ""Total Population""
                },
                {
                    ""column_name"": ""at.city"",
                    ""column_type"": ""string"",
                    ""alias"": ""City""
                },
                {
                    ""column_name"": ""at.Country"",
                    ""column_type"": ""string"",
                    ""alias"": ""Country""
                }
            ]
        }
    }
";

            var inputObj = JsonConvert.DeserializeObject<Query>(input);
            string expectedoutput = "SELECT SUM(st.population) AS \"Total Population\", \"at.city\" AS \"City\", \"at.Country\" AS \"Country\"\n";
            string parsedQuery = SelectParser.Parse(inputObj);
            Assert.AreEqual(expectedoutput, parsedQuery);
        }

        /// <summary>
        /// Testing JSON parser for only select query without display columns
        /// </summary>

        [TestMethod]
        public void JsonQueryParser_OnlySelectNoColumns()
        {
            string input = @"
    {
        ""query_type"": ""select"",
        ""query_parameters"": {
        }
    }
";

            var inputObj = JsonConvert.DeserializeObject<Query>(input);
            string expectedoutput = "SELECT * \n";
            string parsedQuery = SelectParser.Parse(inputObj);
            Assert.AreEqual(expectedoutput, parsedQuery);
        }

        /// <summary>
        /// Testing JSON parser for select query and single table
        /// </summary>
        [TestMethod]
        public void JsonQueryParser_OnlySelect_SingleTable()
        {
            string input = @"
    {
        ""query_type"": ""select"",
        ""query_parameters"": {
            ""display_columns"": [
                {
                    ""column_name"": ""SUM(st.population)"",
                    ""column_type"": ""name"",
                    ""alias"": ""Total Population""
                },
                {
                    ""column_name"": ""at.city"",
                    ""column_type"": ""string"",
                    ""alias"": ""City""
                },
                {
                    ""column_name"": ""at.Country"",
                    ""column_type"": ""string"",
                    ""alias"": ""Country""
                }
            ],
            ""data_source"": {
                ""source_table"": {
                    ""table_name"": ""sometable"",
                    ""alias"": ""st""
                }
            }
        }
    }
";

            var inputObj = JsonConvert.DeserializeObject<Query>(input);
            string expectedoutput = "SELECT SUM(st.population) AS \"Total Population\", \"at.city\" AS \"City\", \"at.Country\" AS \"Country\"\nFROM sometable AS st\n";
            string parsedQuery = SelectParser.Parse(inputObj);
            Assert.AreEqual(expectedoutput, parsedQuery);
        }

        /// <summary>
        /// Testing JSON parser for select query and multiple tables with joins
        /// </summary>
        [TestMethod]
        public void JsonQueryParser_OnlySelect_MultipleTables()
        {
            string input = @"

    {
        ""query_type"": ""select"",
        ""query_parameters"": {
            ""display_columns"": [
                {
                    ""column_name"": ""SUM(st.population)"",
                    ""column_type"": ""name"",
                    ""alias"": ""Total Population""
                },
                {
                    ""column_name"": ""at.city"",
                    ""column_type"": ""string"",
                    ""alias"": ""City""
                },
                {
                    ""column_name"": ""at.Country"",
                    ""column_type"": ""string"",
                    ""alias"": ""Country""
                }
            ],
            ""data_source"": {
                ""source_table"": {
                    ""table_name"": ""sometable"",
                    ""alias"": ""st""
                },
                ""joins"": [
                    {
                        ""join_type"": ""inner_join"",
                        ""join_source"": {
                            ""table_name"": ""anothertable"",
                            ""alias"": ""at""
                        },
                        ""join_condition"": {
                            ""left_expression"": {
                                ""parent_table"": ""st"",
                                ""column_name"": ""city_id"",
                                ""column_type"": ""name""
                            },
                            ""right_expression"": {
                                ""parent_table"": ""at"",
                                ""column_name"": ""id"",
                                ""column_type"": ""name""
                            },
                            ""operator"": ""EQUALS""
                        }
                    }
                ]
            }
        }
    }
";

            var inputObj = JsonConvert.DeserializeObject<Query>(input);
            string expectedoutput = "SELECT SUM(st.population) AS \"Total Population\", \"at.city\" AS \"City\", \"at.Country\" AS \"Country\"\nFROM sometable AS st\nJOIN anothertable AS at ON \"st\".city_id  = \"at\".id \n";
            string parsedQuery = SelectParser.Parse(inputObj);
            Assert.AreEqual(expectedoutput, parsedQuery);
        }


        /// <summary>
        /// Testing JSON parser for select query and multiple tables with filters
        /// </summary>
        [TestMethod]
        public void JsonQueryParser_OnlySelect_MultipleTablesWithFilters()
        {
            string input = @"

    {
        ""query_type"": ""select"",
        ""query_parameters"": {
            ""display_columns"": [
                {
                    ""column_name"": ""SUM(st.population)"",
                    ""column_type"": ""name"",
                    ""alias"": ""Total Population""
                },
                {
                    ""column_name"": ""at.city"",
                    ""column_type"": ""string"",
                    ""alias"": ""City""
                },
                {
                    ""column_name"": ""at.Country"",
                    ""column_type"": ""string"",
                    ""alias"": ""Country""
                }
            ],
            ""data_source"": {
                ""source_table"": {
                    ""table_name"": ""sometable"",
                    ""alias"": ""st""
                },
                ""joins"": [
                    {
                        ""join_type"": ""inner_join"",
                        ""join_source"": {
                            ""table_name"": ""anothertable"",
                            ""alias"": ""at""
                        },
                        ""join_condition"": {
                            ""left_expression"": {
                                ""parent_table"": ""st"",
                                ""column_name"": ""city_id"",
                                ""column_type"": ""name""
                            },
                            ""right_expression"": {
                                ""parent_table"": ""at"",
                                ""column_name"": ""id"",
                                ""column_type"": ""name""
                            },
                            ""operator"": ""EQUALS""
                        }
                    }
                ]
            },
            ""filter"": {
                ""left_expression"": {
                    ""field"": {
                        ""parent_table"": ""at"",
                        ""column_name"": ""city"",
                        ""column_type"": ""name""
                    },
                    ""value_literal"": ""New York"",
                    ""value_type"": ""literal_string"",
                    ""operator"": ""NOT_EQUALS""
                },
                ""operator"": ""AND"",
                ""right_expression"": {
                    ""left_expression"": {
                        ""field"": {
                            ""parent_table"": ""st"",
                            ""column_name"": ""population"",
                            ""column_type"": ""name""
                        },
                        ""value_literal"": ""10000"",
                        ""value_type"": ""literal_number"",
                        ""operator"": ""GREATER_THAN""
                    }
                }
            }
        }
    }

";

            var inputObj = JsonConvert.DeserializeObject<Query>(input);
            string expectedoutput = "SELECT SUM(st.population) AS \"Total Population\", \"at.city\" AS \"City\", \"at.Country\" AS \"Country\"\nFROM sometable AS st\nJOIN anothertable AS at ON \"st\".city_id  = \"at\".id \nWHERE ( \"at\".city  != \"New York\" ) AND  ( \"st\".population  > 10000 )  \n";
            string parsedQuery = SelectParser.Parse(inputObj);
            Assert.AreEqual(expectedoutput, parsedQuery);
        }


        /// <summary>
        /// Testing JSON parser for select query and multiple tables with Group by clause and "having" filter
        /// </summary>
        [TestMethod]
        public void JsonQueryParser_OnlySelect_MultipleTablesGroupByWithFilters()
        {
            string input = @"

    {
        ""query_type"": ""select"",
        ""query_parameters"": {
            ""display_columns"": [
                {
                    ""column_name"": ""SUM(st.population)"",
                    ""column_type"": ""name"",
                    ""alias"": ""Total Population""
                },
                {
                    ""column_name"": ""at.city"",
                    ""column_type"": ""string"",
                    ""alias"": ""City""
                },
                {
                    ""column_name"": ""at.Country"",
                    ""column_type"": ""string"",
                    ""alias"": ""Country""
                }
            ],
            ""data_source"": {
                ""source_table"": {
                    ""table_name"": ""sometable"",
                    ""alias"": ""st""
                },
                ""joins"": [
                    {
                        ""join_type"": ""inner_join"",
                        ""join_source"": {
                            ""table_name"": ""anothertable"",
                            ""alias"": ""at""
                        },
                        ""join_condition"": {
                            ""left_expression"": {
                                ""parent_table"": ""st"",
                                ""column_name"": ""city_id"",
                                ""column_type"": ""name""
                            },
                            ""right_expression"": {
                                ""parent_table"": ""at"",
                                ""column_name"": ""id"",
                                ""column_type"": ""name""
                            },
                            ""operator"": ""EQUALS""
                        }
                    }
                ]
            },
            ""filter"": {
                ""left_expression"": {
                    ""field"": {
                        ""parent_table"": ""at"",
                        ""column_name"": ""city"",
                        ""column_type"": ""name""
                    },
                    ""value_literal"": ""New York"",
                    ""value_type"": ""literal_string"",
                    ""operator"": ""NOT_EQUALS""
                },
                ""operator"": ""AND"",
                ""right_expression"": {
                    ""left_expression"": {
                        ""field"": {
                            ""parent_table"": ""st"",
                            ""column_name"": ""population"",
                            ""column_type"": ""name""
                        },
                        ""value_literal"": ""10000"",
                        ""value_type"": ""literal_number"",
                        ""operator"": ""GREATER_THAN""
                    }
                }
            },
            ""aggregation"": {
                ""groupby"": [
                    {
                        ""parent_table"": ""at"",
                        ""column_name"": ""country"",
                        ""column_type"": ""name""
                    },
                    {
                        ""parent_table"": ""at"",
                        ""column_name"": ""city"",
                        ""column_type"": ""name""
                    }
                ],
                ""group_filters"": {
                    ""left_expression"": {
                        ""field"": {
                            ""column_name"": ""Total Population"",
                            ""column_type"": ""string""
                        },
                        ""value_literal"": ""100000"",
                        ""value_type"": ""literal_number"",
                        ""operator"": ""GREATER_THAN""
                    }
                }
            }
        }
    }

";

            var inputObj = JsonConvert.DeserializeObject<Query>(input);
            string expectedoutput = "SELECT SUM(st.population) AS \"Total Population\", \"at.city\" AS \"City\", \"at.Country\" AS \"Country\"\nFROM sometable AS st\nJOIN anothertable AS at ON \"st\".city_id  = \"at\".id \nWHERE ( \"at\".city  != \"New York\" ) AND  ( \"st\".population  > 10000 )  \nGROUP BY  \"at\".country , \"at\".city  \n HAVING ( \"Total Population\"  > 100000 )  \n";
            string parsedQuery = SelectParser.Parse(inputObj);
            Assert.AreEqual(expectedoutput, parsedQuery);
        }


        /// <summary>
        /// Testing JSON parser for select query and multiple tables with Order By clause
        /// </summary>
        [TestMethod]
        public void JsonQueryParser_OnlySelect_MultipleTablesOrderBy()
        {
            string input = @"

    {
        ""query_type"": ""select"",
        ""query_parameters"": {
            ""display_columns"": [
                {
                    ""column_name"": ""SUM(st.population)"",
                    ""column_type"": ""name"",
                    ""alias"": ""Total Population""
                },
                {
                    ""column_name"": ""at.city"",
                    ""column_type"": ""string"",
                    ""alias"": ""City""
                },
                {
                    ""column_name"": ""at.Country"",
                    ""column_type"": ""string"",
                    ""alias"": ""Country""
                }
            ],
            ""data_source"": {
                ""source_table"": {
                    ""table_name"": ""sometable"",
                    ""alias"": ""st""
                },
                ""joins"": [
                    {
                        ""join_type"": ""inner_join"",
                        ""join_source"": {
                            ""table_name"": ""anothertable"",
                            ""alias"": ""at""
                        },
                        ""join_condition"": {
                            ""left_expression"": {
                                ""parent_table"": ""st"",
                                ""column_name"": ""city_id"",
                                ""column_type"": ""name""
                            },
                            ""right_expression"": {
                                ""parent_table"": ""at"",
                                ""column_name"": ""id"",
                                ""column_type"": ""name""
                            },
                            ""operator"": ""EQUALS""
                        }
                    }
                ]
            },
            ""filter"": {
                ""left_expression"": {
                    ""field"": {
                        ""parent_table"": ""at"",
                        ""column_name"": ""city"",
                        ""column_type"": ""name""
                    },
                    ""value_literal"": ""New York"",
                    ""value_type"": ""literal_string"",
                    ""operator"": ""NOT_EQUALS""
                },
                ""operator"": ""AND"",
                ""right_expression"": {
                    ""left_expression"": {
                        ""field"": {
                            ""parent_table"": ""st"",
                            ""column_name"": ""population"",
                            ""column_type"": ""name""
                        },
                        ""value_literal"": ""10000"",
                        ""value_type"": ""literal_number"",
                        ""operator"": ""GREATER_THAN""
                    }
                }
            },
            ""aggregation"": {
                ""groupby"": [
                    {
                        ""parent_table"": ""at"",
                        ""column_name"": ""country"",
                        ""column_type"": ""name""
                    },
                    {
                        ""parent_table"": ""at"",
                        ""column_name"": ""city"",
                        ""column_type"": ""name""
                    }
                ],
                ""group_filters"": {
                    ""left_expression"": {
                        ""field"": {
                            ""column_name"": ""Total Population"",
                            ""column_type"": ""string""
                        },
                        ""value_literal"": ""100000"",
                        ""value_type"": ""literal_number"",
                        ""operator"": ""GREATER_THAN""
                    }
                }
            },
            ""order_parameters"": {
                ""ordering_columns"": [
                    {
                        ""column_name"": {
                            ""parent_table"": ""at"",
                            ""column_name"": ""country"",
                            ""column_type"": ""name""
                        },
                        ""sort_order"": ""ascending""
                    },{
                        ""column_name"": {
                            ""parent_table"": ""st"",
                            ""column_name"": ""population"",
                            ""column_type"": ""name""
                        },
                        ""sort_order"": ""descending""
                    }
                ]
            }
        }
    }

";

            var inputObj = JsonConvert.DeserializeObject<Query>(input);
            string expectedoutput = "SELECT SUM(st.population) AS \"Total Population\", \"at.city\" AS \"City\", \"at.Country\" AS \"Country\"\nFROM sometable AS st\nJOIN anothertable AS at ON \"st\".city_id  = \"at\".id \nWHERE ( \"at\".city  != \"New York\" ) AND  ( \"st\".population  > 10000 )  \nGROUP BY  \"at\".country , \"at\".city  \n HAVING ( \"Total Population\"  > 100000 )  \nOrder BY \"at\".country  ASCENDING , \"st\".population  DESCENDING ";
            string parsedQuery = SelectParser.Parse(inputObj);
            Assert.AreEqual(expectedoutput, parsedQuery);
        }

    }
}
