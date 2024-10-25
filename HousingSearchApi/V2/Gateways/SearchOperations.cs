using Nest;
using System;
using System.Collections.Generic;
using System.Linq;


namespace HousingSearchApi.V2.Gateways;


class SearchOperations
{
    // TODO: Do we need separate wildcard functions?
    public static Func<QueryContainerDescriptor<object>, QueryContainer>
    SearchWithWildcardQuery(string searchText, List<string> fields, int boost)
    {
        List<string> ProcessWildcards(string phrase)
        {
            if (string.IsNullOrEmpty(phrase))
                return new List<string>();
            return phrase.Split(' ').Select(word => $"*{word}*").ToList();
        }

        var listOfWildcardedWords = ProcessWildcards(searchText);
        var queryString = $"({string.Join(" AND ", listOfWildcardedWords)}) " + string.Join(" ", listOfWildcardedWords);

        return q => q.QueryString(qs => qs
            .Query(queryString)
            .Fields(fields.Select(f => (Field) f).ToArray())
            .DefaultOperator(Operator.And)
            .Boost(boost)
        );
    }

    public static Func<QueryContainerDescriptor<object>, QueryContainer>
    SearchWithExactQuery(string searchText, List<string> fields, int boost)
    {
        string Process(string searchText)
        {
            searchText = searchText.Trim();
            if (searchText.Split(' ').Length == 2)
                return searchText.Replace(" ", " AND ");
            return searchText;
        }

        var processedQuery = Process(searchText);

        return q => q.QueryString(qs => qs
            .Query(processedQuery)
            .Fields(fields.Select(f => (Field) $"{f}^{boost}").ToArray())
            .DefaultOperator(Operator.And)
        );
    }

    // Score for matching a value which starts with the search text
    public static Func<QueryContainerDescriptor<object>, QueryContainer>
        MatchPhrasePrefix(string searchText, string fieldName, int boost) =>
        should => should
            .MatchPhrasePrefix(mp => mp
                .Field(fieldName)
                .Query(searchText)
                .Boost(boost)
            );

    // Score for matching a single (best) field
    public static Func<QueryContainerDescriptor<object>, QueryContainer>
        MultiMatchSingleField(string searchText, int boost) =>
        should => should
            .MultiMatch(mm => mm
                .Fields("*")
                .Query(searchText)
                .Type(TextQueryType.BestFields)
                .Operator(Operator.And)
                .Fuzziness(Fuzziness.Auto)
                .Boost(boost)
            );

    // Score for matching the combination of many fields
    public static Func<QueryContainerDescriptor<object>, QueryContainer>
        MultiMatchCrossFields(string searchText, int boost) =>
        should => should
            .MultiMatch(mm => mm
                .Fields("*")
                .Query(searchText)
                .Type(TextQueryType.CrossFields)
                .Operator(Operator.Or)
                .Boost(boost)
            );

    // Score for matching a high number (quantity) of fields
    public static Func<QueryContainerDescriptor<object>, QueryContainer>
        MultiMatchMostFields(string searchText, int boost) =>
        should => should
            .MultiMatch(mm => mm
                .Fields("*")
                .Query(searchText)
                .Type(TextQueryType.MostFields)
                .Operator(Operator.Or)
                .Fuzziness(Fuzziness.Auto)
                .Boost(boost)
            );


    // Score for matching a value which contains the search text
    public static Func<QueryContainerDescriptor<object>, QueryContainer>
        WildcardMatch(string searchText, string fieldName, int boost)
    {
        List<string> ProcessWildcards(string phrase)
        {
            if (string.IsNullOrEmpty(phrase))
                return new List<string>();
            return phrase.Split(' ').Select(word => $"*{word}*").ToList();
        }

        var listOfWildcardedWords = ProcessWildcards(searchText);
        var wildcardQueries = listOfWildcardedWords.Select(term => new WildcardQuery
        {
            Field = fieldName,
            Value = $"*{term}*",
            Boost = boost
        }).ToList();

        return q => q.Bool(b => b
            .Should(wildcardQueries.Select(wq => 
                new QueryContainerDescriptor<object>()
                    .Wildcard(w => w
                        .Field(wq.Field)
                        .Value(wq.Value)
                        .Boost(wq.Boost)
                    )
                ).ToArray()
            )
        );
    }
}
