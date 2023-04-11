// See https://aka.ms/new-console-template for more information
using LocalEdit.PlanTypes;
using System.Text.Json;

string tempText = @"{  ""Title"": ""Hello Brad2"",  ""StartDate"": ""2022-11-06"",  ""BaseUrl"": ""https://gimmegimme"",  ""Items"": [    {      ""ID"": ""Q1"",      ""Label"": ""Question One"",      ""StoryId"": ""1"",      ""Duration"": ""1"",      ""Dependencies"": [        {          ""DependencyType"": ""OTHER"",          ""ID"": ""Q4"",          ""StartDate"": """"        }      ]    },    {      ""ID"": ""Q2"",      ""Label"": ""Question Two"",      ""StoryId"": ""2"",      ""Duration"": ""2"",      ""Dependencies"": [        {          ""DependencyType"": ""ITEM"",          ""ID"": ""Q1"",          ""StartDate"": """"        }      ]    },    {      ""ID"": ""Q3"",      ""Label"": ""Question Three"",      ""StoryId"": ""3"",      ""Duration"": ""3"",      ""Dependencies"": []    },    {      ""ID"": ""Q4"",      ""Label"": ""Question Four"",      ""StoryId"": ""4"",      ""Duration"": ""4"",      ""Dependencies"": []    }  ],  ""Sprints"": [    {      ""ID"": null,      ""Label"": ""Sprint 1"",      ""StartDate"": ""2022-11-04"",      ""EndDate"": ""2022-11-05""    },    {      ""ID"": null,      ""Label"": ""Sprint 2"",      ""StartDate"": ""2022-11-06"",      ""EndDate"": ""2022-11-07""    },    {      ""ID"": null,      ""Label"": ""Sprint 3"",      ""StartDate"": ""2022-11-08"",      ""EndDate"": ""2022-11-08""    },    {      ""ID"": null,      ""Label"": ""Sprint 4"",      ""StartDate"": ""2022-11-09"",      ""EndDate"": ""2022-11-10""    }  ]}";

PlanDocument doc = (JsonSerializer.Deserialize(tempText, typeof(PlanDocument)) as PlanDocument) ?? new PlanDocument();

List<PlanItem> items = DependencySorter.Generate(doc.Items, DateOnly.Parse(doc.StartDate));

Console.WriteLine("Hello, World!");
