﻿using System.Text;

namespace LocalEdit.PlanTypes
{
    public class PlanPublisher
    {
        public static string Publish(PlanDocument plan)
        {

            string rtnVal = "";
            rtnVal = PublishMermaid(plan);

            return rtnVal;
        }

        private static string PublishMermaid(PlanDocument plan)
        {
            StringBuilder sb = new StringBuilder();

            sb.AppendLine(MermaidHeader(plan));

            foreach (PlanItem item in plan.Items)
            {
                sb.Append(MermaidItem(item));
            }

            return sb.ToString();
        }

        //private isInList(lookFor: string, lookIn: string[]): boolean
        //{
        //    var rtnVal: boolean = false;

        //    for (var lookInItem of lookIn)
        //    {
        //        if (this.ciEquals(lookFor, lookInItem))
        //        {
        //            rtnVal = true;
        //        }
        //    }

        //    return rtnVal;
        //}

        private static string MermaidHeader(PlanDocument plan)
        {
            StringBuilder sb = new StringBuilder();
            //        sb.append("flowchart TB");
            //        sb.append("\r\n");
            // classDef borderless stroke-width:0px
            // classDef darkBlue fill:#00008B, color:#fff
            // classDef brightBlue fill:#6082B6, color:#fff
            // classDef gray fill:#62524F, color:#fff
            // classDef gray2 fill:#4F625B, color:#fff

            // ");

            // start date and title MUST be filled in

            sb.AppendLine("gantt");
            // gantt
            sb.AppendLine("    dateFormat  YYYY-MM-DD");
            sb.AppendLine($"    title       {plan.Title}");
            sb.AppendLine("    excludes    weekends");
            //        sb.appendLine(`    %% (`excludes` accepts specific dates in YYYY-MM-DD format, days of the week ("sunday") or "weekends", but not the word "weekdays".)`);
            sb.AppendLine($"    Start: milestone, start, {plan.StartDate}, 0min");

            return sb.ToString();
        }

        private static string BuildIndentation(int level)
        {
            string rtnVal = "";

            for (var i = 0; i < (4 * level); i++)
            {
                rtnVal = rtnVal + " ";
            }
            return rtnVal;
        }

        private static string MermaidItem(PlanItem item, int indent = 1)
        {
            StringBuilder sb = new StringBuilder();

            string indentation = BuildIndentation(indent);
            //var displayType: string = item.itemType;
            bool goDeeper = true;

            StringBuilder deps = new StringBuilder();
            bool isAfter = true;

            //            if (item.Dependencies.Count == 0)
            {
                deps.Append("start");
            }
  //          else
            {
                foreach (PlanItemDependency dependency in item.Dependencies)
                {
                    if (dependency.DependencyType == "DATE")
                    {
                        deps.Append($" {dependency.StartDate}");
                        if (deps.ToString().StartsWith("start"))
                        {
                            deps = new StringBuilder(deps.ToString().Substring(6));
                            isAfter= false;
                        }
                    }
                    else
                    {
                        deps.Append($" {dependency.ID}");
                    }
                }
            }
            // start item was created so that this is not required
            // if(firstItem)
            // {
            //     const start: Date = new Date();
            //     var datePart = this.toIsoString(start);
            //     sb.appendLine(`${item.label} : ${datePart}, 1d`);
            // }
            // else{
            sb.Append($"{indentation}{item.Label} :{item.ID} ");

            if (deps.Length > 0)
            {
                if (isAfter)
                {
                    sb.Append($", after {deps.ToString()}");
                }
                else
                {
                    sb.Append($", {deps.ToString()}");
                }
            }

            if (!string.IsNullOrEmpty(item.Duration))
            {
                sb.Append($", {item.Duration}d");
            }

            sb.AppendLine();
            //        }

            // switch (item.itemType) {
            //     case "PERSON":
            //         if (item.external) {
            //             displayType = "External Person";
            //         }
            //         else {
            //             displayType = "Person";
            //         }
            //         break;
            //     case "SYSTEM":
            //         if (item.external) {
            //             displayType = "External System";
            //         }
            //         else {
            //             if (this.ciEquals(this.diagramType, "Context")) {
            //                 goDeeper = false;
            //                 displayType = "System";
            //             }
            //             else if (item.items.length === 0) {
            //                 displayType = "System";
            //             }
            //             else {
            //                 displayType = "System Boundary";
            //             }
            //         }
            //         break;
            //     case "CONTAINER":
            //         if (item.external) {
            //             displayType = "External Container";
            //         }
            //         else {
            //             if (this.ciEquals(this.diagramType, "Container")) {
            //                 goDeeper = false;
            //                 displayType = "Container";
            //             }
            //             else if (item.items.length === 0) {
            //                 displayType = "Container";
            //             }
            //             else {
            //                 displayType = "Container Boundary";
            //             }
            //         }
            //         break;
            //     case "DATABASE":
            //         if (item.external) {
            //             displayType = "External Database";
            //         }
            //         else {
            //             if (this.ciEquals(this.diagramType, "Container")) {
            //                 goDeeper = false;
            //                 displayType = "Database";
            //             }
            //             else if (item.items.length === 0) {
            //                 displayType = "Database";
            //             }
            //             else {
            //                 displayType = "Database Boundary";
            //             }
            //         }
            //         break;
            // }

            // var displayLabel: string = `\"<strong><u>${item.label}</u></strong>`;
            // var brokenDescription: string = item.description.replace("`", "<br/>");

            // if (item.description.length !== 0) {
            //     displayLabel = displayLabel + `<br/>${brokenDescription}`;
            // }

            // displayLabel += `<br/>&#171;${displayType}&#187;\"`;

            // if (!goDeeper || (item.items.length === 0)) {
            //     sb.append(`${indentation}${item.id}[${displayLabel}]`);
            //     sb.append("\r\n");
            // }
            // else {
            //     sb.append(`${indentation}subgraph ${item.id}[${displayLabel}]`);
            //     sb.append("\r\n");
            //     indent++;

            //     var item2: C4Item;
            //     for (var itmNum = 0; itmNum < item.items.length; itmNum++) {
            //         item2 = item.items[itmNum];
            //         sb.append(this.mermaidItem(item2, indent).trimEnd());
            //         sb.append("\r\n");
            //     }
            //     sb.append(`${indentation}end`);
            //     sb.append("\r\n");
            // }

            return sb.ToString();
        }

        //    public toIsoString(date : Date) : string
        //{
        //    var year = date.getFullYear();
        //    var month = date.getMonth() + 1;
        //    var dt = date.getDate();

        //    var dtString: string = dt.toString();
        //    var monthString: string = month.toString();

        //    if (dt < 10)
        //    {
        //        dtString = '0' + dt.toString();
        //    }
        //    if (month < 10)
        //    {
        //        monthString = '0' + month.toString();
        //    }

        //    return (year + '-' + monthString + '-' + dtString);
        //}

        //public label: string = "";
        //public description: string = "";
        //public external: boolean = false;
        //public technology: string = "";
        //public database: boolean = false;
        //private _id: string = "";

        //// https://stackoverflow.com/questions/2140627/how-to-do-case-insensitive-string-comparison
        //ciEquals(a: string, b: string) {
        //    return typeof a === 'string' && typeof b === 'string'
        //        ? a.localeCompare(b, undefined, { sensitivity: 'accent' }) === 0
        //            : a === b;
        //}
        //}
    }
}

