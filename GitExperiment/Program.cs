// See https://aka.ms/new-console-template for more information
using Octokit;
using System;

Console.WriteLine("Hello, World!");

// https://contentlab.io/using-c-code-to-access-the-github-api/


//Experiment (GitHub token)
//    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

// this is from https://www.daveabrock.com/2021/03/14/upload-files-to-github-repository/
var gitHubClient = new GitHubClient(new ProductHeaderValue("GitExperiment"));
gitHubClient.Credentials = new Credentials("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");

var user = await gitHubClient.User.Get("nj-brad");
Console.WriteLine($"Woah! Brad has {user.PublicRepos} public repositories.");

Repository repo = await gitHubClient.Repository.Get("nj-brad", "localedit");

var docs = await gitHubClient.Repository
                    .Content
                    .GetAllContents("nj-brad", "localedit", "LocalEdit/SampleFiles/sample.flow");


var fileContent = await gitHubClient.Repository
                    .Content
                    .GetRawContent("nj-brad", "localedit", "LocalEdit/SampleFiles/sample.flow");

var fileText = System.Text.Encoding.UTF8.GetString(fileContent, 0, fileContent.Length);

Console.WriteLine(fileText);

// https://stackoverflow.com/questions/36023902/getting-all-files-for-repository-using-octokit

//var contents = await github
//                .Repository
//                .Content
//                .GetAllContents("octokit", "octokit.net");

//...

//var docs = await github
//                .Repository
//                .Content
//                .GetAllContents("octokit", "octokit.net", "docs");

Console.ReadLine();

// this includes instructions how to get to an enterprise repo
// https://octokitnet.readthedocs.io/en/latest/getting-started/
