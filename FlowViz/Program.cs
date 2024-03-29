﻿// See https://aka.ms/new-console-template for more information
using FlowViz;
using FlowViz.FlowTypes;
using FlowViz.LpeTypes;
using FlowViz.SequenceTypes;
using System.Reflection.Metadata;
using System.Text.Json;

SequenceDocument? document = null;
FlowDocument? flowDocument = null;

string fileText = @"{
    ""hasFooter"": true,
    ""hasHeader"": true,
    ""headerConfig"": {
        ""phoneNumber"": ""888-241-3714"",
        ""logo"": ""rocketMortgageLogo.svg""
    },
    ""itemFlow"": [
        {
            ""title"": ""This is a text input question"",
            ""itemName"": ""question1"",
            ""itemType"": ""textInput""
        },
        {
            ""title"": ""Here is the Radio button question"",
            ""itemName"": ""question2"",
            ""itemType"": ""radioInput"",
            ""entryLogic"": {
                ""valueConditions"": [
                    {
                        ""itemName"": ""question1"",
                        ""itemValue"": ""default"",
                        ""answerType"": ""exact""
                    }
                ]
            }
        },
        {
            ""title"": ""This is an external question injected into the engine"",
            ""itemName"": ""question3"",
            ""itemType"": ""external""
        }
    ]
}
";

string fileText2 = @"{ 
""hasFooter"": true, 
""hasHeader"": true, 
""headerConfig"": { 
    ""phoneNumber"": ""(888) 452-8179"", 
    ""logo"": ""https://www.rocketmortgage.com/resources-cmsassets/GlobalContent/NonStockImages/Logos/MastheadLogos_RocketMortgage.svg""
}, 
""items"": 
[ 
    { 
        ""id"": ""Logic1"", 
        ""type"": ""textInput"", 
        ""title"": ""Question #1 What state are you in?"", 
        ""queryLogic"": [], 
        ""flowEntryLogic"": [], 
        ""linkLogic"": [] 
    }, 
    { 
        ""id"": ""Logic2"", 
        ""type"": ""infoDisplay"", 
        ""title"": ""Question #2 Generic Info page"", 
        ""queryLogic"": [], 
        ""flowEntryLogic"": [], 
        ""linkLogic"": 
        [ 
            { 
                ""jumpToItemId"": ""Logic7"", 
                ""id"": ""Logic1"", 
                ""value"": ""mi"", 
                ""type"": ""exact"" 
            }, 
            { 
                ""jumpToItemId"": ""Logic5"", 
                ""id"": ""Logic1"", 
                ""value"": """", 
                ""type"": ""any"" 
            } 
        ] 
    }, 
    { 
        ""id"": ""Logic3"", 
        ""type"": ""radioInput"", 
        ""title"": ""Question #3 Do you pay rent?"", 
        ""queryLogic"": [], 
        ""flowEntryLogic"": [], 
        ""linkLogic"": 
        [ 
            { 
                ""jumpToItemId"": ""Logic4"", 
                ""id"": ""Logic3"", 
                ""value"": ""yes"", 
                ""type"": ""exact"" 
            }, 
        { 
            ""jumpToItemId"": ""Logic6"", 
            ""id"": ""Logic3"", 
            ""value"": ""no"", 
            ""type"": ""exact"" 
        } 
    ] 
}, 
{ 
    ""id"": ""Logic4"", ""type"": ""textInput"", ""title"": ""Question #4 How much do you pay in rent?"", ""queryLogic"": [], ""flowEntryLogic"": [], ""linkLogic"": [ { ""jumpToItemId"": ""Logic6"", ""id"": ""Logic4"", ""value"": """", ""type"": ""any"" } ] }, { ""id"": ""Logic5"", ""type"": ""radioInput"", ""title"": ""Question #5 Where are you at in the process?"", ""queryLogic"": [], ""flowEntryLogic"": [], ""linkLogic"": [ { ""jumpToItemId"": ""Logic3"", ""id"": ""Logic5"", ""value"": """", ""type"": ""any"" } ] }, { ""id"": ""Logic6"", ""type"": ""radioInput"", ""title"": ""Question #6 Have you considered a home buying plan?"", ""queryLogic"": [], ""flowEntryLogic"": [], ""linkLogic"": [ { ""jumpToItemId"": ""Logic10"", ""id"": ""Logic6"", ""value"": ""yes"", ""type"": ""exact"" }, { ""jumpToItemId"": ""Logic9"", ""id"": ""Logic6"", ""value"": ""no"", ""type"": ""exact"" } ] }, { ""id"": ""Logic7"", ""type"": ""textInput"", ""title"": ""Question #7 What is the Purchase Price of the home you want?"", ""queryLogic"": [], ""flowEntryLogic"": [], ""linkLogic"": [ { ""jumpToItemId"": ""Logic8"", ""id"": ""Logic7"", ""value"": """", ""type"": ""any"" } ] }, { ""id"": ""Logic8"", ""type"": ""radioInput"", ""title"": ""Question #8 Are you working with a Realtor? "", ""queryLogic"": [], ""flowEntryLogic"": [], ""linkLogic"": [ { ""jumpToItemId"": ""Logic3"", ""id"": ""Logic8"", ""value"": """", ""type"": ""any"" } ] }, { ""id"": ""Logic9"", ""type"": ""infoDisplay"", ""title"": ""Question #9 Get a Prequalification"", ""queryLogic"": [], ""flowEntryLogic"": [], ""linkLogic"": [] }, { ""id"": ""Logic10"", ""type"": ""infoDisplay"", ""title"": ""Question #10 Start a home Buying Plan"", ""queryLogic"": [], ""flowEntryLogic"": [], ""linkLogic"": [] } ] }";

string fileText3 = @"{ 
""hasFooter"": true, 
""hasHeader"": true, 
""headerConfig"": { 
    ""phoneNumber"": ""(888) 452-8179"", 
    ""logo"": ""https://www.rocketmortgage.com/resources-cmsassets/GlobalContent/NonStockImages/Logos/MastheadLogos_RocketMortgage.svg""
}, 
""items"": 
[ 
    { 
        ""id"": ""Logic1"", 
        ""type"": ""textInput"", 
        ""title"": ""Question #1 What state are you in?"", 
        ""queryLogic"": [], 
        ""flowEntryLogic"": [], 
        ""linkLogic"": [] 
    }, 
    { 
        ""id"": ""Logic2"", 
        ""type"": ""infoDisplay"", 
        ""title"": ""Question #2 Generic Info page"", 
        ""queryLogic"": [], 
        ""flowEntryLogic"": [], 
        ""linkLogic"": 
        [ 
            { 
                ""jumpToItemId"": ""Logic7"", 
                ""id"": ""Logic1"", 
                ""value"": ""mi"", 
                ""type"": ""exact"" 
            }, 
            { 
                ""jumpToItemId"": ""Logic5"", 
                ""id"": ""Logic1"", 
                ""value"": """", 
                ""type"": ""any"" 
            } 
        ] 
    }, 
    { 
        ""id"": ""Logic3"", 
        ""type"": ""radioInput"", 
        ""title"": ""Question #3 Do you pay rent?"", 
        ""queryLogic"": [], 
        ""flowEntryLogic"": [], 
        ""linkLogic"": 
        [ 
            { 
                ""jumpToItemId"": ""Logic4"", 
                ""id"": ""Logic3"", 
                ""value"": ""yes"", 
                ""type"": ""exact"" 
            }, 
        { 
            ""jumpToItemId"": ""Logic6"", 
            ""id"": ""Logic3"", 
            ""value"": ""no"", 
            ""type"": ""exact"" 
        } 
    ] 
}, 
{ 
    ""id"": ""Logic4"", ""type"": ""textInput"", ""title"": ""Question #4 How much do you pay in rent?"", ""queryLogic"": [], ""flowEntryLogic"": [], ""linkLogic"": [ { ""jumpToItemId"": ""Logic6"", ""id"": ""Logic4"", ""value"": """", ""type"": ""any"" } ] }, { ""id"": ""Logic5"", ""type"": ""radioInput"", ""title"": ""Question #5 Where are you at in the process?"", ""queryLogic"": [], ""flowEntryLogic"": [], ""linkLogic"": [ { ""jumpToItemId"": ""Logic3"", ""id"": ""Logic5"", ""value"": """", ""type"": ""any"" } ] }, { ""id"": ""Logic6"", ""type"": ""radioInput"", ""title"": ""Question #6 Have you considered a home buying plan?"", ""queryLogic"": [], ""flowEntryLogic"": [], ""linkLogic"": [ { ""jumpToItemId"": ""Logic10"", ""id"": ""Logic6"", ""value"": ""yes"", ""type"": ""exact"" }, { ""jumpToItemId"": ""Logic9"", ""id"": ""Logic6"", ""value"": ""no"", ""type"": ""exact"" } ] }, { ""id"": ""Logic7"", ""type"": ""textInput"", ""title"": ""Question #7 What is the Purchase Price of the home you want?"", ""queryLogic"": [], ""flowEntryLogic"": [], ""linkLogic"": [ { ""jumpToItemId"": ""Logic8"", ""id"": ""Logic7"", ""value"": """", ""type"": ""any"" } ] }, { ""id"": ""Logic8"", ""type"": ""radioInput"", ""title"": ""Question #8 Are you working with a Realtor? "", ""queryLogic"": [], ""flowEntryLogic"": [], ""linkLogic"": [ { ""jumpToItemId"": ""Logic3"", ""id"": ""Logic8"", ""value"": """", ""type"": ""any"" } ] }, { ""id"": ""Logic9"", ""type"": ""infoDisplay"", ""title"": ""Question #9 Get a Prequalification"", ""queryLogic"": [], ""flowEntryLogic"": [], ""linkLogic"": [] }, { ""id"": ""Logic10"", ""type"": ""infoDisplay"", ""title"": ""Question #10 Start a home Buying Plan"", ""queryLogic"": [], ""flowEntryLogic"": [], ""linkLogic"": [] } ] }";


string fileText4 = @"{ ""hasFooter"": true, ""hasHeader"": true, 
""headerConfig"": { ""phoneNumber"": ""(888) 452-8179"", ""logo"": ""https://www.rocketmortgage.com/resources-cmsassets/GlobalContent/NonStockImages/Logos/MastheadLogos_RocketMortgage.svg"" }, 
""items"": [ 
{ ""id"": ""Logic1"", ""type"": ""textInput"", ""title"": ""Question #1 What state are you in?"", ""queryLogic"": [], ""flowEntryLogic"": [] }, 
{ ""id"": ""Logic2"", ""type"": ""infoDisplay"", ""title"": ""Question #2 Generic Info page"", ""queryLogic"": [], ""flowEntryLogic"": [], 
    ""linkLogic"": [ 
        { ""jumpToItemId"": ""Logic7"", ""id"": ""Logic1"", ""value"": ""mi"", ""type"": ""exact"" }, 
        { ""jumpToItemId"": ""Logic5"", ""id"": ""Logic1"", ""value"": """", ""type"": ""any"" } 
    ] 
}, { ""id"": ""Logic3"", ""type"": ""radioInput"", ""title"": ""Question #3 Do you pay rent?"", ""queryLogic"": [], ""flowEntryLogic"": [], ""linkLogic"": [ { ""jumpToItemId"": ""Logic4"", ""id"": ""Logic3"", ""value"": ""yes"", ""type"": ""exact"" }, { ""jumpToItemId"": ""Logic6"", ""id"": ""Logic3"", ""value"": ""no"", ""type"": ""exact"" } ] }, { ""id"": ""Logic4"", ""type"": ""textInput"", ""title"": ""Question #4 How much do you pay in rent?"", ""queryLogic"": [], ""flowEntryLogic"": [], ""linkLogic"": [ { ""jumpToItemId"": ""Logic6"", ""id"": ""Logic4"", ""value"": """", ""type"": ""any"" } ] }, { ""id"": ""Logic5"", ""type"": ""radioInput"", ""title"": ""Question #5 Where are you at in the process?"", ""queryLogic"": [], ""flowEntryLogic"": [], ""linkLogic"": [ { ""jumpToItemId"": ""Logic3"", ""id"": ""Logic5"", ""value"": """", ""type"": ""any"" } ] }, { ""id"": ""Logic6"", ""type"": ""radioInput"", ""title"": ""Question #6 Have you considered a home buying plan?"", ""queryLogic"": [], ""flowEntryLogic"": [], ""linkLogic"": [ { ""jumpToItemId"": ""Logic10"", ""id"": ""Logic6"", ""value"": ""yes"", ""type"": ""exact"" }, { ""jumpToItemId"": ""Logic9"", ""id"": ""Logic6"", ""value"": ""no"", ""type"": ""exact"" } ] }, { ""id"": ""Logic7"", ""type"": ""textInput"", ""title"": ""Question #7 What is the Purchase Price of the home you want?"", ""queryLogic"": [], ""flowEntryLogic"": [], ""linkLogic"": [ { ""jumpToItemId"": ""Logic8"", ""id"": ""Logic7"", ""value"": """", ""type"": ""any"" } ] }, 
{ ""id"": ""Logic8"", ""type"": ""radioInput"", ""title"": ""Question #8 Are you working with a Realtor? "", ""queryLogic"": [], ""flowEntryLogic"": [], ""linkLogic"": [ { ""jumpToItemId"": ""Logic3"", ""id"": ""Logic8"", ""value"": """", ""type"": ""any"" } ] }, 
{ ""id"": ""Logic9"", ""type"": ""infoDisplay"", ""title"": ""Question #9 Get a Prequalification"", ""queryLogic"": [], ""flowEntryLogic"": [] }, 
{ ""id"": ""Logic10"", ""type"": ""infoDisplay"", ""title"": ""Question #10 Start a home Buying Plan"", ""queryLogic"": [], ""flowEntryLogic"": [] } ] }";

        //Root? root = JsonSerializer.Deserialize(FileManagementModalRef.FileText, typeof(Root)) as Root;
        //Root? root = JsonSerializer.Deserialize(fileText, typeof(Root)) as Root;
        Root? root = JsonSerializer.Deserialize(fileText4, typeof(Root)) as Root;
if (root != null)
{
    //document = LpeConverter.ToSequenceDocument(root);
    flowDocument = LpeConverter.ToFlowDocument(root);
}

//string pub = SequencePublisher.Publish(document);
string pub = FlowPublisher.Publish(flowDocument);

string pub2 = HtmlGenerator.WrapMermaid(pub);

Console.Write(pub2);
