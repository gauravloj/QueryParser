# Description:

This console application converts JSON sql query input to SQL query string.

## How to run application
1. Go to project folder QueryParser\QueryParser
2. run "dotnet run -- <JSON file path>"

## How to run unit tests
1. Go to project folder QueryParser\QueryParserUnitTest
2. run "dotnet test"

## Input file format
Input file is a JSON file with below format
1. Input is an array of Query objects
2. Query object:
	- query_type : one of SELECT, CREATE, UPDATE, ALTER, DROP (Only select command is implemented for now. Other commands will be supported later)
	- query_parameters: It contains all the parameters to generate the query
3. query_parameters object:
	- display_columns : List of columns object
	- data_source: Source object for source table along with joins
	- filters: List of logical conditions object
	- aggregation: Aggregation object to generate group by clause
	- order_parameters: Sorting order parameters
4. Columns object: 
	- parent_table : Either alias or full table name where this column exists.
	- column_name: name of the column
	- alias : Column header to be displayed
	- column_type: One of String, Name, expression. "String" is a literal value of column, "Name" is real name of column, expression is any other expression. 
5. Data source object:
	- source_table: Source table object tahat contains table name and its alias
	- Joins: It is a list of joins object that contains information for other tables and how they are joined with source
6. Join object:
	- join_type: one of INNER_JOIN, LEFT_JOIN, RIGHT_JOIN, FULL_JOIN
	- join_source: table object (table name and its alias), on which join will happen
	- join_condition: Condition on which join will happen
7. Join condition object:
	- left_expression: Left side of the join condition. It is of type column object
	- right_expression: Right side of the join condition. It is of type column object
	- operator: It can only be of type "EQUALS" for now
8. Logical conditions object:
	- left_expression: Type of filter condition object that defines one condition. It should be empty if the operator is NOT
	- operator: Logical operator. One of AND, OR, NOT
	- right_expression: this is again a logical condtion object to include another list of logical expression recursively
9. Filter condition object:
	- field: Colmn type object used in the filtering criteria
	- value_type: one of literal_string, literal_number, query, depending on the type of value used. 
	- value_literal: if value_type is literal_string or literal_number then this field store the value
	- value_object: if value_type is queryliteral_number then this field store the value i.e. another select query
	- operator: operator to use between field and value. One of EQUALS, NOT_EQUALS, GREATER_THAN, LESS_THAN, LESS_THAN_EQUALS, GREATER_THAN_EQUALS, IN, NOT_IN, LIKE.
10. Aggregation object:
	- groupby: List of columns on which grouping should be done
	- group_filters: filters to use for "having" clause. It is of type Logical expression

11. order_parameters object:
	- ordering_columns : List of ordering_column objects
12. ordering_column object:
	- column_name : Column object on which sorting shold happen
	- sort_order: sort order for that column

## Pending work:
1. Implement parser for commands other than select
2. Create a separate filter condition for having clause to incorporate aggregate functions like SUM
3. Generate SQL query for different types of SQL langauges like MySql, postgre sql etc. 