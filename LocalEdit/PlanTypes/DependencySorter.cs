using Octokit;
using System.Collections.Generic;

namespace LocalEdit.PlanTypes
{
    public class DependencySorter
    {
        public static List<PlanItem> Generate(List<PlanItem> originalItemList, DateOnly projectStartDate)
        {
            DateOnly startDate = projectStartDate;
            List<PlanItem> inFiles = new List<PlanItem>(originalItemList);
            List<PlanItem> rtnVal = new List<PlanItem>(originalItemList.Count);    // setting the the correct length means that it doesn't have to waste time expanding later
            List<PlanItem> itemsToRemove = new List<PlanItem>();

            int removedItemCount = 0;
            do
            {
                itemsToRemove.Clear();

                foreach (PlanItem item in inFiles)
                {
                    // No more un-met dependencies.  Add it to the output
                    if (item.Dependencies.Count == 0)
                    {
                        // set the start date
                        item.StartDate = startDate;

                        // set the end date
                        int days = string.IsNullOrEmpty(item.Duration) ? 1 : int.Parse(item.Duration);
                        item.EndDate = item.StartDate.Value.AddDays(days);

                        rtnVal.Add(item);
                        itemsToRemove.Add(item);
                    }
                }

                removedItemCount = itemsToRemove.Count;
                foreach (PlanItem itemToRemove in itemsToRemove)
                {
                    startDate = itemToRemove.EndDate.Value.AddDays(1);

                    foreach (PlanItem item in inFiles)
                    {
                        foreach (PlanItemDependency dep in item.Dependencies)
                        {
                            if(dep.ID == itemToRemove.ID)
                            {
                                if(item.StartDate < startDate)
                                {
                                    item.StartDate = startDate;
                                }
                                item.Dependencies.Remove(dep);
                            }
                        }
                    }

                    inFiles.Remove(itemToRemove);
                }
            }
            while (removedItemCount > 0);

            return rtnVal;
        }

        //private static SimplifiedPlanItem ToSimplifiedPlanItem(PlanItem item, DateOnly startDate)
        //{
        //    SimplifiedPlanItem rtnVal = new SimplifiedPlanItem();
        //    rtnVal.ID = item.ID;
        //    rtnVal.Label = item.Label;
        //    rtnVal.StartDate = startDate;
        //    int days = string.IsNullOrEmpty(item.Duration) ? 1 : int.Parse(item.Duration);
        //    rtnVal.EndDate = startDate.AddDays(days);
        //    rtnVal.StoryId = item.StoryId;
        //    return rtnVal;
        //}

    }
}
